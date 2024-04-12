using HotelListing.API.Constract;
using HouseRentingAPI.Data;
using HouseRentingAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingAPI.Interface
{
    public interface IHouseService:IGenericRepository<House>
    {
        Task<List<GetHouseDto>> GetAllHouses();
        Task<GetHouseByIdDto> GetHouseById(Guid id);
        Task<List<GetHouseDto>> GetHousesByIds(List<Guid> houseIds);
        //Task<List<GetHouseByIdDto>> GetHousesByLandlord(Guid landlordId);
        Task<List<GetHouseDto>> SearchHouses(string? keyword, int propertyTypeID, List<int> facilityIDs, List<int> attributeIDs, int minPrice, int maxPrice);
        Task SaveHousePhotoAsync(Guid houseId, IFormFile photoFile, bool isCoverPhoto);
        Task DeleteBlobAsync(string blobUrl);
        Task<CommentDto> AddCommentAsync(Guid houseId, string content, Guid userId, string emotionresult);
        Task<List<Comment>> GetCommentsByHouseIdAsync(Guid houseId);
        Task UpdateCommentAsync(Guid commentId, string comment);
        Task DeleteCommentAsync(Guid commentId);
    }
}
