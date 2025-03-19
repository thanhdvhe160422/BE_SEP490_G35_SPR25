using Planify_BackEnd.Models;

namespace Planify_BackEnd.Services.Medias
{
    public interface IMediumService
    {
        Task<Medium> CreateMediaItemAsync(Medium mediaItem);
    }
}
