using AutoMapper;
using HotelListing.API.Constract;
using HouseRentingAPI.Data;
using HouseRentingAPI.Interface;
using HouseRentingAPI.Model;
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
        private readonly IMapper _mapper;

        public HouseService(HouseRentingDbContext context, ICommentService commentService,IMapper mapper) : base(context)
        {
            this._context = context;
            this._commentService = commentService;
            this._mapper = mapper;
        }

        public async Task<List<GetHouseDto>> GetAllHouses()
        {
            return await _context.Houses
                .Select(h => new GetHouseDto
                {
                    Housename = h.HouseName,
                    Address = h.Address,
                    PropertyTypeName = h.PropertyType.TypeName,
                    SquareFeet = h.SquareFeet,
                    Price = h.Price
                })
                .ToListAsync();
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

        public async Task<CommentDto> AddCommentAsync(Guid houseId, string content, Guid userId)
        {
            // 创建评论对象
            var comment = new Comment
            {
                HouseId = houseId,
                CommentText = content,
                UserId = userId,
                CreatedAt = DateTime.UtcNow // 在服务端自动生成 UTC 时间戳
            };

            // 将评论对象添加到评论服务中
            await _commentService.AddAsync(comment);

            // 使用 AutoMapper 将评论对象映射为 CommentAddDto 对象
            var commentDto = _mapper.Map<CommentDto>(comment);

            // 将 UTC 时间转换为台湾标准时间并进行格式化
            var taiwanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
            var taiwanTime = TimeZoneInfo.ConvertTimeFromUtc(comment.CreatedAt, taiwanTimeZone);
            commentDto.CreatedAt = taiwanTime.ToString("yyyy-MM-dd HH:mm");

            return commentDto;
        }

        public async Task<List<Comment>> GetCommentsByHouseIdAsync(Guid houseId)
        {
            return await _commentService.GetCommentsByHouseIdAsync(houseId);
        }

        public async Task UpdateCommentAsync(Guid commentId, string commentText)
        {
            var comment = await _commentService.GetAsync(commentId);
            if (comment == null)
            {
                throw new Exception("Comment not found.");
            }
            comment.CommentText = commentText;
            await _commentService.UpdateAsync(comment);
        }

        public async Task DeleteCommentAsync(Guid commentId)
        {
            await _commentService.DeleteAsync(commentId);
        }
    }

}
