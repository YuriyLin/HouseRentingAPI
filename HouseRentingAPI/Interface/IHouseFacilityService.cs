using HotelListing.API.Constract;
using HouseRentingAPI.Data;
using HouseRentingAPI.Model;

namespace HouseRentingAPI.Interface {
    public interface IHouseFacilityService
    {
        Task<HouseFacility> GetByIdAsync(Guid houseId, int facilityId);
    }
}
