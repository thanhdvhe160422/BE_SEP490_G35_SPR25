using Planify_BackEnd.DTOs;
using Planify_BackEnd.Models;

namespace Planify_BackEnd.Services.Medias
{
    public interface IMediumService
    {
        Task<Medium> CreateMediaItemAsync(Medium mediaItem);
        Task<bool> DeleteMediaEvent(List<EventMediaDto> list);
    }
}
