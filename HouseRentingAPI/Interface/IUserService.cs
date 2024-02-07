using HotelListing.API.Constract;
using HouseRentingAPI.Data;
using HouseRentingAPI.Model;

namespace HouseRentingAPI.Constract
{
    public interface IUserService : IGenericRepository<User>
    {
        Task<User> loginAsync(UserLoginDto userLoginDto);
        Task RegisterAsync(UserRegisterDto userRegisterDto);
    }
}
