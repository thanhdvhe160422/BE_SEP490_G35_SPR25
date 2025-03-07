using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.Campus;

namespace Planify_BackEnd.Services.Campus
{
    public interface ICampusService
    {
       Task<IEnumerable<CampusDTO>> GetAllCampus();
    }
}
