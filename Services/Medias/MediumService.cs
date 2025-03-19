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
    }
}
