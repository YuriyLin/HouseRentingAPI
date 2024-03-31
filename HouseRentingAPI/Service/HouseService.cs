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
            var houses = await _context.Houses
                .Include(h => h.HousePhotos)
                .Select(h => new GetHouseDto
                {
                    HouseID = h.HouseID,
                    Housename = h.HouseName,
                    Address = h.Address,
                    PropertyTypeName = h.PropertyType.TypeName,
                    SquareFeet = h.SquareFeet,
                    Price = h.Price,
                    CoverPhotoUrl = h.HousePhotos.FirstOrDefault(p => p.IsCoverPhoto).Photo.PhotoURL
                })
                .ToListAsync();

            return houses;
        }
        /*public async Task<List<GetHouseByIdDto>> GetHousesByLandlord(Guid landlordId)
        {
            var houses = await _context.Houses
                .Include(h => h.PropertyType)
                .Include(h => h.HousePhotos)
                    .ThenInclude(hp => hp.Photo)
                .Where(h => h.LandlordID == landlordId)
                .Select(h => new GetHouseByIdDto
                {
                    Housename = h.HouseName,
                    Address = h.Address,
                    PropertyTypeName = h.PropertyType.TypeName,
                    Squarefeet = h.SquareFeet,
                    Price = h.Price,
                    PhotoUrl = h.HousePhotos.Select(hp => hp.Photo.PhotoURL).ToList()
                })
                .ToListAsync();

            return houses;
        }*/

        public async Task<List<GetHouseDto>> SearchHouses(string? keyword, int propertyTypeID, List<int> facilityIDs, List<int> attributeIDs, int minPrice, int maxPrice)
        {
            var query = _context.Houses.AsQueryable();

            // 根据关键字进行过滤
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(h => EF.Functions.Like(h.HouseName, $"%{keyword}%") ||
                                         EF.Functions.Like(h.Address, $"%{keyword}%") ||
                                         EF.Functions.Like(h.PropertyType.TypeName, $"%{keyword}%") ||
                                         EF.Functions.Like(h.Description, $"%{keyword}%"));
            }

            // 根据设施ID进行过滤
            if (facilityIDs != null && facilityIDs.Any())
            {
                query = query.Where(h => h.HouseFacilities.Any(hf => facilityIDs.Contains(hf.FacilityID)));
            }

            // 根据属性ID进行过滤
            if (attributeIDs != null && attributeIDs.Any())
            {
                query = query.Where(h => h.HouseOtherAttributes.Any(hoa => attributeIDs.Contains(hoa.OtherAttribute.AttributeID)));
            }

            // 根据 PropertyTypeID 进行过滤
            if (propertyTypeID > 0)
            {
                query = query.Where(h => h.PropertyTypeID == propertyTypeID);
            }

            // 根据价格范围进行过滤
            if (minPrice > 0)
            {
                query = query.Where(h => h.Price >= minPrice);
            }
            if (maxPrice > 0)
            {
                query = query.Where(h => h.Price <= maxPrice);
            }

            var result = await query
                .Select(h => new GetHouseDto
                {
                    HouseID = h.HouseID,
                    Housename = h.HouseName,
                    Address = h.Address,
                    PropertyTypeName = h.PropertyType.TypeName,
                    SquareFeet = h.SquareFeet,
                    Price = h.Price
                })
                .ToListAsync();

            return result;
        }
        public async Task SaveHousePhotoAsync(Guid houseId, IFormFile photoFile, bool isCoverPhoto)
        {
            // 指定上传文件的保存路径
            var uploadDirectory = @"C:\Users\USER\Downloads\HouseRentingAPI\HouseRentingAPI\Housephoto";

            // 创建上传目录
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            // 生成文件名并拼接文件路径
            var fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(photoFile.FileName)}";
            var filePath = Path.Combine(uploadDirectory, fileName);

            // 将文件保存到指定路径
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await photoFile.CopyToAsync(stream);
            }

            // 將照片路徑保存到數據庫中的Photo表
            var photo = new Photo { PhotoURL = filePath };
            _context.Photo.Add(photo);
            await _context.SaveChangesAsync();

            // 將房屋照片訊息保存到數據庫中的HousePhoto表
            var housePhoto = new HousePhoto
            {
                HouseID = houseId,
                PhotoID = photo.PhotoID,
                IsCoverPhoto = isCoverPhoto
            };
            _context.HousesPhoto.Add(housePhoto);
            await _context.SaveChangesAsync();
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
