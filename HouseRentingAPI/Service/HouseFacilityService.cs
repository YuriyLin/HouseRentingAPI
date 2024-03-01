using HouseRentingAPI.Data;
using HouseRentingAPI.Interface;

namespace HouseRentingAPI.Service
{
    public class HouseFacilityService:IHouseFacilityService
    {
        private readonly HouseRentingDbContext _context;

        public HouseFacilityService(HouseRentingDbContext context)
        {
            _context = context;
        }
        public async Task<HouseFacility> GetByIdAsync(Guid houseId, int facilityId)
        {
            return await _context.HouseFacilities.FindAsync(houseId, facilityId);
        }
    }

}
