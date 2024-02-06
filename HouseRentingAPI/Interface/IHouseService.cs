using HotelListing.API.Constract;
using HouseRentingAPI.Data;

namespace HouseRentingAPI.Interface
{
    public interface IHouseService:IGenericRepository<House>
    {
        Task<List<House>> SearchHouses(string keyword);
        Task AddCommentAsync(Guid houseId, string content, Guid userId);
        Task<List<Comment>> GetCommentsByHouseIdAsync(Guid houseId);
        Task UpdateCommentAsync(Comment comment);
        Task DeleteCommentAsync(Guid commentId);
    }
}
