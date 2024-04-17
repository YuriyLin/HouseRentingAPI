using HotelListing.API.Constract;
using HouseRentingAPI.Data;
using HouseRentingAPI.Model;

namespace HouseRentingAPI.Interface
{
    public interface IFavoriteService:IGenericRepository<Favorite>
    {
        Task<List<FavoriteDto>> GetUserFavoritesAsync(Guid userId);
        Task<List<FavoitebyUserIdDto>> GetUserFavoritesHouse(Guid houseId);
        Task AddFavoriteAsync(Guid userId, Guid houseId);
        Task RemoveFavoriteAsync(Guid userId, Guid houseId);
    }
}
