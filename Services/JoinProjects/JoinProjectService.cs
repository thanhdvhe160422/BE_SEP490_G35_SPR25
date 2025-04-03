using Azure;
using Microsoft.AspNetCore.SignalR;
using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.Events;
using Planify_BackEnd.DTOs.JoinedProjects;
using Planify_BackEnd.DTOs.Users;
using Planify_BackEnd.Hub;
using Planify_BackEnd.Models;
using Planify_BackEnd.Repositories;
using Planify_BackEnd.Repositories.JoinGroups;

namespace Planify_BackEnd.Services.JoinProjects
{
    public class JoinProjectService : IJoinProjectService
    {
        private readonly IJoinProjectRepository _joinProjectRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHubContext<NotificationHub> _hubContext;
        public JoinProjectService(IJoinProjectRepository joinProjectRepository, IHttpContextAccessor httpContextAccessor, IHubContext<NotificationHub> hubContext)
        {
            _joinProjectRepository = joinProjectRepository;
            _httpContextAccessor = httpContextAccessor;
            _hubContext = hubContext;
        }
        public async Task<IEnumerable<JoinedProjectDTO>> GetAllJoinedProjects(Guid userId, int page, int pageSize)
        {
            var joinedProject = await _joinProjectRepository.GetAllJoinedProjects(userId, page, pageSize);
            var joinedProjectDTOs = joinedProject.Select(j => new JoinedProjectDTO
            {
                Id = j.Id,
                EventId = j.EventId,
                UserId = j.UserId,
                TimeJoinProject = j.TimeJoinProject,
                TimeOutProject = j.TimeOutProject,
                EventTitle = j.Event.EventTitle,
                EventDescription = j.Event.EventDescription,
                StartTime = j.Event.StartTime,
                EndTime = j.Event.EndTime,
                AmountBudget = j.Event.AmountBudget,
                IsPublic = j.Event.IsPublic,
                TimePublic = j.Event.TimePublic,
                Status = j.Event.Status,
                CampusId = j.Event.CampusId,
                CategoryEventId = j.Event.CategoryEventId,
                Placed = j.Event.Placed,
                CreatedAt = j.Event.CreatedAt,
                CreateBy = j.Event.CreateBy,
                UpdatedAt = j.Event.UpdatedAt,
                UpdateBy = j.Event.UpdateBy
            }).ToList();
            return joinedProjectDTOs;
            
        }
        public async Task<bool> DeleteImplementerFromEvent(Guid userId, int eventId)
        {
            try
            {
                return await _joinProjectRepository.DeleteImplementerFromEvent(userId, eventId);
            }catch (Exception ex)
            {
                Console.WriteLine("join project service - delete implementer from event: " + ex.Message);
                return false;
            }
        }

        public async Task<ResponseDTO> AddImplementersToEventAsync(AddImplementersToEventDTO request)
        {
            if (request == null || request.UserIds == null || !request.UserIds.Any())
            {
                return new ResponseDTO(400, "Invalid request data. UserIds list cannot be empty.", null);
            }

            if (!await _joinProjectRepository.EventExistsAsync(request.EventId))
            {
                return new ResponseDTO(404, $"Event with ID {request.EventId} not found.", null);
            }

            var invalidUserIds = await _joinProjectRepository.GetInvalidUserIdsAsync(request.UserIds);
            if (invalidUserIds.Any())
            {
                return new ResponseDTO(400, $"The following UserIds do not exist: {string.Join(", ", invalidUserIds)}", null);
            }

            var existingUserIds = await _joinProjectRepository.GetExistingImplementerIdsAsync(request.EventId);
            var newUserIds = request.UserIds.Except(existingUserIds).ToList();

            if (!newUserIds.Any())
            {
                return new ResponseDTO(200, "All provided users are already implementers for this event.", null);
            }

            var addImplementersSuccess = await _joinProjectRepository.AddImplementersToProject(newUserIds, request.EventId);
            if (!addImplementersSuccess)
            {
                return new ResponseDTO(500, "Failed to add implementers to the event.", null);
            }

            var addRolesSuccess = await _joinProjectRepository.AddRoleImplementers(newUserIds);
            if (!addRolesSuccess)
            {
                return new ResponseDTO(500, "Failed to assign Implementer role to users.", null);
            }

            var result = new { AddedCount = newUserIds.Count, EventId = request.EventId };
            //notification
            foreach (var id in newUserIds)
            {
                await _hubContext.Clients.User(id + "").SendAsync("ReceiveNotification",
                    "You have been added to a event plan!",
                    "/event-detail-EOG/" + request.EventId);
            }
            return new ResponseDTO(200, $"Successfully added {newUserIds.Count} implementer(s) to event {request.EventId}.", result);
        }

        public async Task<PageResultDTO<JoinedProjectVM>> GetImplementerJoinedEvent(int page,int pageSize, int eventId)
        {
            try
            {
                var listImplement = await _joinProjectRepository.GetImplementerJoinedEvent(page, pageSize, eventId);
                List<JoinedProjectVM> list = listImplement.Items.Select(jp => new JoinedProjectVM
                {
                    Id = jp.Id,
                    EventId = jp.EventId,
                    UserId = jp.UserId,
                    TimeJoinProject = jp.TimeJoinProject,
                    TimeOutProject = jp.TimeOutProject,
                    User = new UserNameVM
                    {
                        Id = jp.User.Id,
                        Email = jp.User.Email,
                        FirstName = jp.User.FirstName,
                        LastName = jp.User.LastName,
                    },
                }).ToList();
                return new PageResultDTO<JoinedProjectVM>(list, listImplement.TotalCount, page, pageSize);
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

   
}
