﻿using System.Threading.Tasks;
using Azure;
using Azure.Core;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.Events;
using Planify_BackEnd.DTOs.JoinedProjects;
using Planify_BackEnd.DTOs.Tasks;
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
        private readonly IEventRepository _eventRepository;
        public JoinProjectService(IJoinProjectRepository joinProjectRepository, IHttpContextAccessor httpContextAccessor, IHubContext<NotificationHub> hubContext, IEventRepository eventRepository)
        {
            _joinProjectRepository = joinProjectRepository;
            _httpContextAccessor = httpContextAccessor;
            _hubContext = hubContext;
            _eventRepository = eventRepository;
        }
        public PageResultDTO<JoinedProjectDTO> JoiningEvents(int page, int pageSize, Guid userId)
        {
            try
            {
                var joinedProject = _joinProjectRepository.JoiningEvents(userId,page, pageSize);
                if (joinedProject.TotalCount == 0)

                    return new PageResultDTO<JoinedProjectDTO>(new List<JoinedProjectDTO>(), 0, page, pageSize);
                List<JoinedProjectDTO> joiningList = new List<JoinedProjectDTO>();
                foreach (var item in joinedProject.Items)
                {
                    var joinedProjectDTO = new JoinedProjectDTO
                    {
                        Id = item.Id,
                        EventId = item.EventId,
                        UserId = item.UserId,
                        TimeJoinProject = item.TimeJoinProject,
                        TimeOutProject = item.TimeOutProject,
                        EventTitle = item.Event.EventTitle,
                        EventDescription = item.Event.EventDescription,
                        StartTime = item.Event.StartTime,
                        EndTime = item.Event.EndTime,
                        AmountBudget = item.Event.AmountBudget,
                        IsPublic = item.Event.IsPublic,
                        TimePublic = item.Event.TimePublic,
                        Status = item.Event.Status,
                        CampusId = item.Event.CampusId,
                        CategoryEventId = item.Event.CategoryEventId,
                        Placed = item.Event.Placed,
                        CreatedAt = item.Event.CreatedAt,
                        CreateBy = item.Event.CreateBy,
                        UpdatedAt = item.Event.UpdatedAt,
                        UpdateBy = item.Event.UpdateBy
                    };
                    joiningList.Add(joinedProjectDTO);
                }

                return new PageResultDTO<JoinedProjectDTO>(joiningList, joinedProject.TotalCount, page, pageSize);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public PageResultDTO<JoinedProjectDTO> AttendedEvents(int page, int pageSize, Guid userId)
        {
            try
            {
                var joinedProject = _joinProjectRepository.AttendedEvents(page, pageSize, userId);
                if(joinedProject.TotalCount == 0)

                    return new PageResultDTO<JoinedProjectDTO>(new List<JoinedProjectDTO>(), 0, page, pageSize);
                List<JoinedProjectDTO> joinedList = new List<JoinedProjectDTO>();
                foreach (var item in joinedProject.Items)
                {
                    var joinedProjectDTO = new JoinedProjectDTO
                    {
                        Id = item.Id,
                        EventId = item.EventId,
                        UserId = item.UserId,
                        TimeJoinProject = item.TimeJoinProject,
                        TimeOutProject = item.TimeOutProject,
                        EventTitle = item.Event.EventTitle,
                        EventDescription = item.Event.EventDescription,
                        StartTime = item.Event.StartTime,
                        EndTime = item.Event.EndTime,
                        AmountBudget = item.Event.AmountBudget,
                        IsPublic = item.Event.IsPublic,
                        TimePublic = item.Event.TimePublic,
                        Status = item.Event.Status,
                        CampusId = item.Event.CampusId,
                        CategoryEventId = item.Event.CategoryEventId,
                        CategoryName = item.Event.CategoryEvent.CategoryEventName,
                        Placed = item.Event.Placed,
                        CreatedAt = item.Event.CreatedAt,
                        CreateBy = item.Event.CreateBy,
                        UpdatedAt = item.Event.UpdatedAt,
                        UpdateBy = item.Event.UpdateBy
                    };
                    joinedList.Add(joinedProjectDTO);
                }
              
                return new PageResultDTO<JoinedProjectDTO>(joinedList, joinedProject.TotalCount, page, pageSize);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> DeleteImplementerFromEvent(Guid userId, int eventId)
        {
            try
            {
                var response = await _joinProjectRepository.DeleteImplementerFromEvent(userId, eventId);
                //notification
                var e = await _eventRepository.GetEventByIdAsync(eventId);
                var message = "Bạn đã bị loại khỏi sự kiện " + (e.EventTitle.Length > 40 ? e.EventTitle.Substring(0, 40) + ".." : e.EventTitle) + "!";
                if (response)
                    await _hubContext.Clients.User(userId + "").SendAsync("ReceiveNotification",
                    message,
                    "/event-detail-EOG/" + eventId);
                return response;
            }
            catch (Exception ex)
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
            var e = await _eventRepository.GetEventByIdAsync(request.EventId);
            var message = "Bạn đã được thêm vào sự kiện " + (e.EventTitle.Length > 40 ? e.EventTitle.Substring(0, 40) + ".." : e.EventTitle) + "!";
            var link = "/event-detail-EOG/" + request.EventId;
            foreach (var id in newUserIds)
            {
                await _hubContext.Clients.User(id + "").SendAsync("ReceiveNotification",
                    message,
                    link);
            }
            return new ResponseDTO(200, $"Successfully added {newUserIds.Count} implementer(s) to event {request.EventId}.", result);
        }

        public async Task<PageResultDTO<JoinedProjectVM>> SearchImplementerJoinedEvent(
            int page, int pageSize, int? eventId, string? email, string? name)
        {
            try
            {
                var listImplement = await _joinProjectRepository
                    .SearchImplementerJoinedEvent(page, pageSize, eventId, email, name);
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
