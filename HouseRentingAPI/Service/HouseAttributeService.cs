using HouseRentingAPI.Data;
using HouseRentingAPI.Interface;

namespace HouseRentingAPI.Service
{
    public class HouseAttributeService:IHouseAttributeService
    {
        private readonly HouseRentingDbContext _context;

        public HouseAttributeService(HouseRentingDbContext context)
        {
            _context = context;
        }
        public async Task<HouseOtherAttribute> GetByIdAsync(Guid houseId, int OtherAttributeId)
        {
            return await _context.HouseOtherAttributes.FindAsync(houseId, OtherAttributeId);
        }
    }

}
