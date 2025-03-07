﻿using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.Campus;

namespace Planify_BackEnd.Services.Campus
{
    public class CampusService : ICampusService
    {
        private ICampusRepository _campusRepository;
        public CampusService(ICampusRepository campusRepository)
        {
            _campusRepository = campusRepository;
        }
        public async Task<IEnumerable<CampusDTO>> GetAllCampus()
        {
            var campuses = await _campusRepository.getAllCampus();
            var campusDTOs = campuses.Select(c => new CampusDTO
            {
                Id = c.Id,
                CampusName = c.CampusName,
                Status = c.Status
            }).ToList();
            return campusDTOs;
        }
    }
}
