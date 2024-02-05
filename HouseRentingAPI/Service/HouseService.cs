using HotelListing.API.Constract;
using HouseRentingAPI.Data;
using HouseRentingAPI.Interface;

namespace HouseRentingAPI.Service
{
    public class HouseService : GenericService<House>, IHouseService
    {
        private readonly HouseRentingDbContext _context;

        public HouseService(HouseRentingDbContext context) : base(context)
        {
            this._context = context;
        }
    }
}
