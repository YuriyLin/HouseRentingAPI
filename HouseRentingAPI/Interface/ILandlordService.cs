using HotelListing.API.Constract;
using HouseRentingAPI.Data;
using HouseRentingAPI.Model;

namespace HouseRentingAPI.Interface
{
    public interface ILandlordService:IGenericRepository<Landlord>
    {
        Task<Landlord> loginAsync(LandlordLoginDto landlordLoginDto);
        Task RegisterAsync(LandlordRegisterDto landlordRegisterDto);
        Task<String> UpdateLandlordPasswordAsync(Guid userId, UpdateLandlordPasswordDto updateLandlordPasswordDto);
    }
}
