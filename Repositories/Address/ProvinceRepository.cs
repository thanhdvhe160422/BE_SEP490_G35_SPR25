using Microsoft.EntityFrameworkCore;
using Planify_BackEnd.Models;

namespace Planify_BackEnd.Repositories.Address
{
    public class ProvinceRepository : IProvinceRepository
    {
        private readonly PlanifyContext _context;
        public ProvinceRepository(PlanifyContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<District>> getAllDistrictByProvinceID(int id)
        {
            var district = await _context.Districts.Where(d => d.ProvinceId == id).ToListAsync();
            return district;
        }

        public async Task<IEnumerable<Province>> getAllProvince()
        {
            var p = await _context.Provinces.ToListAsync();
            return p;
        }

        public async Task<IEnumerable<Ward>> getAllWardByDistrictID(int id)
        {
            var p = await _context.Wards.Where(w=>w.DistrictId==id).ToListAsync();
            return p;
        }
    }
}
