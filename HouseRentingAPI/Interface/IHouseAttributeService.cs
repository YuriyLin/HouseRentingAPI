using HotelListing.API.Constract;
using HouseRentingAPI.Data;
using HouseRentingAPI.Model;
namespace HouseRentingAPI.Interface
{
    public interface IHouseAttributeService
    {
        Task<HouseOtherAttribute> GetByIdAsync(Guid houseId, int OtherAttributeId);
    }

}
