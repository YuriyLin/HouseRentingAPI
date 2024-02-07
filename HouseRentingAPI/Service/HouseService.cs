using HotelListing.API.Constract;
using HouseRentingAPI.Data;
using HouseRentingAPI.Interface;
using Microsoft.EntityFrameworkCore;

namespace HouseRentingAPI.Service
{
    // 獲取所有房屋資訊
    // 以ID獲取房屋資訊
    // 刊登房屋
    // 更新房屋
    // 刪除房屋
    // 房屋查詢
    // 評論新增
    // 修改評論
    // 刪除評論

    public class HouseService : GenericService<House>, IHouseService
    {
        private readonly HouseRentingDbContext _context;
        private readonly ICommentService _commentService;

        public HouseService(HouseRentingDbContext context, ICommentService commentService) : base(context)
        {
            this._context = context;
            this._commentService = commentService;
        }

        public async Task<List<House>> SearchHouses(string keyword)
        {
            return await _context.Houses
                .Where(h => EF.Functions.Like(h.HouseName, $"%{keyword}%") ||
                            EF.Functions.Like(h.Address, $"%{keyword}%") ||
                            EF.Functions.Like(h.PropertyType.TypeName, $"%{keyword}%") ||
                            EF.Functions.Like(h.Description, $"%{keyword}%") ||
                            h.HouseFacilities.Any(hf => EF.Functions.Like(hf.Facility.FacilityName, $"%{keyword}%")) ||
                            h.HouseOtherAttributes.Any(hoa => EF.Functions.Like(hoa.OtherAttribute.AttributeName, $"%{keyword}%"))
                )
                .ToListAsync();
        }

        public async Task AddCommentAsync(Guid houseId, string content, Guid userId)
        {
            var comment = new Comment
            {
                HouseId = houseId,
                CommentText = content,
                UserId = userId,
                CreatedAt = DateTime.Now
            };

            await _commentService.AddAsync(comment);
        }
        public async Task<List<Comment>> GetCommentsByHouseIdAsync(Guid houseId)
        {
            return await _commentService.GetCommentsByHouseIdAsync(houseId);
        }

        public async Task UpdateCommentAsync(Comment comment)
        {
            await _commentService.UpdateAsync(comment);
        }

        public async Task DeleteCommentAsync(Guid commentId)
        {
            await _commentService.DeleteAsync(commentId);
        }
    }
}
