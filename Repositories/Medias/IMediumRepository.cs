using Planify_BackEnd.Models;

namespace Planify_BackEnd.Repositories.Medias
{
    public interface IMediumRepository
    {
        Task<Medium> CreateMediaItemAsync(Medium mediaItem);
    }
}
