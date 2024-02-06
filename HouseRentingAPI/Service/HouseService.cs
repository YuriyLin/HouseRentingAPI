using HotelListing.API.Constract;
using HouseRentingAPI.Data;
using HouseRentingAPI.Interface;
using Microsoft.EntityFrameworkCore;

namespace HouseRentingAPI.Service
{
    public class HouseService : GenericService<House>, IHouseService
    {
        private readonly HouseRentingDbContext _context;

        public HouseService(HouseRentingDbContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<List<House>> SearchHouses(string keyword)
        {
            return await _context.Houses
                .Where(h => EF.Functions.Like(h.HouseName, $"%{keyword}%") ||
                            EF.Functions.Like(h.Address, $"%{keyword}%") ||
                            EF.Functions.Like(h.PropertyType.TypeName, $"%{keyword}%") ||
                            EF.Functions.Like(h.Description, $"%{keyword}%") ||
                            h.HouseFacilities.Any(hf => EF.Functions.Like(hf.Facility.FacilityName, $"%{keyword}%")) ||
                            h.HouseOtherAttributes.Any(hoa => EF.Functions.Like(hoa.OtherAttribute.AttributeName, $"%{keyword}%"))
                )
                .ToListAsync();
        }
    }
}
