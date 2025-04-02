using Planify_BackEnd.DTOs;
using Planify_BackEnd.Models;
using Planify_BackEnd.Repositories.Medias;

namespace Planify_BackEnd.Services.Medias
{
    public class MediumService : IMediumService
    {
        private readonly IMediumRepository _mediumRepository;
        public MediumService(IMediumRepository mediumRepository)
        {
            _mediumRepository = mediumRepository;
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

        public async Task<bool> DeleteMediaEvent(List<EventMediaDto> list)
        {
            try
            {
                foreach(var item in list)
                {
                    _mediumRepository.DeleteMediaEvent(item.Id);
                }
                return true;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
