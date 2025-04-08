using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.Events;
using Planify_BackEnd.DTOs.Medias;
using Planify_BackEnd.Models;
using Planify_BackEnd.Repositories.Medias;
using Planify_BackEnd.Services.Events;

namespace Planify_BackEnd.Services.Medias
{
    public class MediumService : IMediumService
    {
        private readonly IMediumRepository _mediumRepository;
        private readonly IEventService _eventService;
        public MediumService(IMediumRepository mediumRepository, IEventService eventService)
        {
            _mediumRepository = mediumRepository;
            _eventService = eventService;
        }

        public Task<Medium> CreateMediaItemAsync(Medium mediaItem)
        {
            try
            {
                return _mediumRepository.CreateMediaItemAsync(mediaItem);
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> UpdateMediaEvent(UpdateMediaEvent updateDTO)
        {
            try
            {
                var listEventMediaExist = updateDTO.ListMedia.Select(m => new EventMedium
                {
                    Id = m
                }).ToList();
                var listMediaDeleted = await _mediumRepository.GetDeleteMediaEvent(updateDTO.EventId,listEventMediaExist);
                foreach(var item in listMediaDeleted)
                {
                    await _mediumRepository.DeleteMediaEvent(item.Id);
                }
                UploadImageRequestDTO listUpload = new UploadImageRequestDTO
                {
                    EventId = updateDTO.EventId,
                    EventMediaFiles = updateDTO.EventMediaFiles
                };
                await _eventService.UploadImageAsync(listUpload);
                return true;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
