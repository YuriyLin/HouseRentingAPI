using HotelListing.API.Constract;
using HouseRentingAPI.Data;
using HouseRentingAPI.Model;

namespace HouseRentingAPI.Interface
{
    public interface IHouseService:IGenericRepository<House>
    {
        Task<List<GetHouseDto>> GetAllHouses();
        Task<List<GetHouseDto>> SearchHouses(string? keyword, int propertyTypeID, List<int> facilityIDs, List<int> attributeIDs, int minPrice, int maxPrice);
        Task SaveHousePhotoAsync(Guid houseId, IFormFile photoFile, bool isCoverPhoto);
        Task<CommentDto> AddCommentAsync(Guid houseId, string content, Guid userId);
        Task<List<Comment>> GetCommentsByHouseIdAsync(Guid houseId);
        Task UpdateCommentAsync(Guid commentId, string comment);
        Task DeleteCommentAsync(Guid commentId);
    }
}
