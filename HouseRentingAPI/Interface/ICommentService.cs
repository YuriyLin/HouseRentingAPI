using HotelListing.API.Constract;
using HouseRentingAPI.Data;

namespace HouseRentingAPI.Interface
{
    public interface ICommentService:IGenericRepository<Comment>
    {
        Task<List<Comment>> GetCommentsByHouseIdAsync(Guid houseId);
    }
}
