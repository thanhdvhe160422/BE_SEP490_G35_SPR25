using Planify_BackEnd.DTOs.Events;
using Planify_BackEnd.DTOs;
using Planify_BackEnd.Models;
using Planify_BackEnd.Repositories.Participants;

namespace Planify_BackEnd.Services.Participants
{
    public class ParticipantService : IParticipantService
    {
        private readonly IParticipantRepository _repository;

        public ParticipantService(IParticipantRepository repository)
        {
            _repository = repository;
        }

        public ResponseDTO GetParticipantCount(int eventId, int pageNumber, int pageSize)
        {
            if (eventId <= 0)
                return new ResponseDTO(400, "Invalid EventId", null);
            if (pageNumber < 1 || pageSize < 1)
                return new ResponseDTO(400, "Invalid page number or page size", null);

            if (!_repository.EventExists(eventId))
                return new ResponseDTO(404, "Event not found", null);

            var (participants, totalCount) = _repository.GetParticipantsWithDetails(eventId, pageNumber, pageSize);

            var result = new ParticipantCountDTO
            {
                EventId = eventId,
                TotalParticipants = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Participants = participants.Select(p => new ParticipantDetailDTO
                {
                    FullName = $"{p.User?.FirstName} {p.User?.LastName}".Trim(),
                    Email = p.User?.Email ?? "Unknown",
                    RegistrationTime = p.RegistrationTime
                }).ToList()
            };

            return new ResponseDTO(200, "Success", result);
        }

        public ResponseDTO RegisterParticipant(RegisterEventDTO registerDto)
        {
            if (registerDto.EventId <= 0)
                return new ResponseDTO(400, "Invalid EventId", null);
            if (registerDto.UserId == Guid.Empty)
                return new ResponseDTO(400, "Invalid UserId", null);

            if (!_repository.EventExists(registerDto.EventId))
                return new ResponseDTO(404, "Event not found", null);
            if (!_repository.UserExists(registerDto.UserId))
                return new ResponseDTO(404, "User not found", null);
            if (_repository.IsAlreadyRegistered(registerDto.EventId, registerDto.UserId))
                return new ResponseDTO(400, "User already registered for this event", null);
            if (_repository.IsOrganizer(registerDto.UserId, registerDto.EventId))
                return new ResponseDTO(403, "Organizers cannot register as participants", null);

            var participant = new Participant
            {
                EventId = registerDto.EventId,
                UserId = registerDto.UserId,
                RegistrationTime = DateTime.UtcNow
            };

            _repository.RegisterParticipant(participant);
            return new ResponseDTO(201, "Registered successfully", null);
        }

        public ResponseDTO GetRegisteredEvents(Guid userId)
        {
            if (userId == Guid.Empty)
                return new ResponseDTO(400, "Invalid UserId", null);

            if (!_repository.UserExists(userId))
                return new ResponseDTO(404, "User not found", null);

            var events = _repository.GetRegisteredEvents(userId);
            var result = events.Select(p => new RegisteredEventDTO
            {
                EventId = p.EventId,
                EventTitle = p.Event.EventTitle,
                RegistrationTime = p.RegistrationTime
            }).ToList();

            return new ResponseDTO(200, "Success", result);
        }
        public ResponseDTO UnregisterParticipant(RegisterEventDTO unregisterDto)
        {
            if (unregisterDto.EventId <= 0)
                return new ResponseDTO(400, "Invalid EventId", null);
            if (unregisterDto.UserId == Guid.Empty)
                return new ResponseDTO(400, "Invalid UserId", null);

            if (!_repository.EventExists(unregisterDto.EventId))
                return new ResponseDTO(404, "Event not found", null);
            if (!_repository.UserExists(unregisterDto.UserId))
                return new ResponseDTO(404, "User not found", null);
            if (!_repository.IsAlreadyRegistered(unregisterDto.EventId, unregisterDto.UserId))
                return new ResponseDTO(400, "User is not registered for this event", null);

            var success = _repository.UnregisterParticipant(unregisterDto.EventId, unregisterDto.UserId);
            if (!success)
                return new ResponseDTO(500, "Failed to unregister", null);

            return new ResponseDTO(200, "Unregistered successfully", null);
        }
    }
}
