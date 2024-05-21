using AutoMapper;
using Azure.Storage.Blobs;
using HotelListing.API.Constract;
using HouseRentingAPI.Data;
using HouseRentingAPI.Interface;
using HouseRentingAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HouseRentingAPI.Service
{
    //
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
                    CoverPhotoUrl = h.HousePhotos.FirstOrDefault(p => p.IsCoverPhoto).Photo.PhotoURL,
                    FacilityIDs = h.HouseFacilities.Select(hf => hf.FacilityID).ToList(),
                    AttributeIDs = h.HouseOtherAttributes.Select(hoa => hoa.OtherAttribute.AttributeID).ToList(),
                    FavoriteCount = h.Favorites.Count,
                    CommentCount = h.Comments.Count
                })
                .ToListAsync();

            return houses;
        }

        public async Task<PagedList<GetHouseDto>> GetPagedHouses(int pageNumber, int pageSize)
        {
            // 首先計算總記錄數，以便計算總頁數
            var totalCount = await _context.Houses.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            // 使用 Skip 和 Take 方法來應用分頁，並直接投影到 DTO
            var pagedHouses = await _context.Houses
                .Include(h => h.HousePhotos) // 包含房子的照片資料
                .Select(h => new GetHouseDto
                {
                    HouseID = h.HouseID,
                    Housename = h.HouseName,
                    Address = h.Address,
                    PropertyTypeName = h.PropertyType.TypeName,
                    SquareFeet = h.SquareFeet,
                    Price = h.Price,
                    CoverPhotoUrl = h.HousePhotos.FirstOrDefault(p => p.IsCoverPhoto).Photo.PhotoURL,
                    FacilityIDs = h.HouseFacilities.Select(hf => hf.FacilityID).ToList(),
                    AttributeIDs = h.HouseOtherAttributes.Select(hoa => hoa.OtherAttribute.AttributeID).ToList(),
                    FavoriteCount = h.Favorites.Count,
                    CommentCount = h.Comments.Count
                })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // 將分頁後的結果和相關資訊打包成 PagedList<GetHouseDto> 返回
            return new PagedList<GetHouseDto>(pagedHouses, totalCount, pageNumber, pageSize, totalPages);
        }

        public async Task<GetHouseByIdDto> GetHouseById(Guid id)
        {
            var house = await _context.Houses
                .Include(h => h.Landlord)
                .Include(h => h.PropertyType)
                .Include(h => h.HouseFacilities)
                .Include(h => h.HouseOtherAttributes)
                .Include(h => h.Comments)
                .Include(h => h.HousePhotos)
                    .ThenInclude(hp => hp.Photo)
                .SingleOrDefaultAsync(h => h.HouseID == id);

            var commentCount = await _context.Comment
                .Where(c => c.HouseId == id)
                .CountAsync();

            var favoriteCount = await _context.Favorites
                .Where(f => f.HouseID == id)
                .CountAsync();

            var gethouse = new GetHouseByIdDto
            {
                Housename = house.HouseName,
                Address = house.Address,
                Description = house.Description,
                Price = house.Price,
                Squarefeet = house.SquareFeet,
                Landlordname = house.Landlord.Landlordname,
                lineID = house.Landlord.LineID,
                PropertyTypeName = house.PropertyType.TypeName,
                FacilityIDs = house.HouseFacilities.Select(f => f.FacilityID).ToList(),
                AttributeIDs = house.HouseOtherAttributes.Select(a => a.AttributeID).ToList(),
                Comments = house.Comments.Select(c => c.CommentText).ToList(),
                PhotoUrl = house.HousePhotos.Select(hp => hp.Photo.PhotoURL).ToList(),
                CommentIDs = house.Comments.Select(c => c.CommentId.ToString()).ToList(),
                CommentCount = commentCount,
                FavoriteCount = favoriteCount
            };

            return gethouse;
        }

        public async Task<List<GetHouseDto>> GetHousesByIds(List<Guid> houseIds)
        {
            var houses = await _context.Houses
                .Where(h => houseIds.Contains(h.HouseID))
                .Select(h => new GetHouseDto
                {
                    HouseID = h.HouseID,
                    Housename = h.HouseName,
                    Address = h.Address,
                    PropertyTypeName = h.PropertyType.TypeName,
                    SquareFeet = h.SquareFeet,
                    Price = h.Price,
                    CoverPhotoUrl = h.HousePhotos.FirstOrDefault(p => p.IsCoverPhoto).Photo.PhotoURL,
                    FacilityIDs = h.HouseFacilities.Select(f => f.FacilityID).ToList(),
                    AttributeIDs = h.HouseOtherAttributes.Select(a => a.AttributeID).ToList(),
                    FavoriteCount = h.Favorites.Count,
                    CommentCount = h.Comments.Count
                })
                .ToListAsync();

            return houses;
        }

        public async Task<List<GetHouseDto>> SearchHouses(string? keyword, int propertyTypeID, List<int> facilityIDs, List<int> attributeIDs, int minPrice, int maxPrice)
        {
            var query = _context.Houses.AsQueryable();

            // 根據關鍵字進行過濾
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(h => EF.Functions.Like(h.HouseName, $"%{keyword}%") ||
                                         EF.Functions.Like(h.Address, $"%{keyword}%") ||
                                         EF.Functions.Like(h.PropertyType.TypeName, $"%{keyword}%") ||
                                         EF.Functions.Like(h.Description, $"%{keyword}%"));
            }

            // 根據 PropertyTypeID 進行過濾
            if (propertyTypeID > 0)
            {
                query = query.Where(h => h.PropertyTypeID == propertyTypeID);
            }

            // 根據價格範圍進行過濾
            if (minPrice > 0)
            {
                query = query.Where(h => h.Price >= minPrice);
            }
            if (maxPrice > 0)
            {
                query = query.Where(h => h.Price <= maxPrice);
            }

            // 根據設施ID進行過濾
            if (facilityIDs != null && facilityIDs.Any())
            {
                query = query.Where(h => h.HouseFacilities.Any(hf => facilityIDs.Contains(hf.FacilityID)));
            }

            // 根據屬性ID進行過濾
            if (attributeIDs != null && attributeIDs.Any())
            {
                query = query.Where(h => h.HouseOtherAttributes.Any(hoa => attributeIDs.Contains(hoa.OtherAttribute.AttributeID)));
            }


            var result = await query
                .Select(h => new GetHouseDto
                {
                    HouseID = h.HouseID,
                    Housename = h.HouseName,
                    Address = h.Address,
                    PropertyTypeName = h.PropertyType.TypeName,
                    SquareFeet = h.SquareFeet,
                    Price = h.Price,
                    CoverPhotoUrl = h.HousePhotos.FirstOrDefault(p => p.IsCoverPhoto).Photo.PhotoURL,
                    FacilityIDs = h.HouseFacilities.Select(hf => hf.FacilityID).ToList(),
                    AttributeIDs = h.HouseOtherAttributes.Select(hoa => hoa.OtherAttribute.AttributeID).ToList(),
                    FavoriteCount = h.Favorites.Count,
                    CommentCount = h.Comments.Count
                })
                .ToListAsync();

            return result;
        }

        public async Task SaveHousePhotoAsync(Guid houseId, IFormFile photoFile, bool isCoverPhoto)
        {
            // 連接到 Azure Blob Storage 帳戶
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=houserentingdata;AccountKey=n67JkHDiB7tZS7Vrene7FZuWXX0Pz7iKLaH9EIcfc13Y9FCtQMH2P0Ul3PZt+/zThdm2DZ9l4DXv+AStDH7qnw==;EndpointSuffix=core.windows.net";
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            // 獲取或創建指定的容器
            string containerName = "housephoto";
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();

            // 生成 Blob 名稱
            string blobName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(photoFile.FileName)}";

            // 上傳照片到 Blob Storage
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            using (var stream = photoFile.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }

            // 獲取 Blob 的 URL
            string blobUrl = blobClient.Uri.ToString();

            // 將 Blob URL 保存到數據庫中的 Photo 表
             var photo = new Photo { PhotoURL = blobUrl };
            _context.Photo.Add(photo);
            await _context.SaveChangesAsync();

            // 將房屋照片訊息保存到數據庫中的 HousePhoto 表
            var housePhoto = new HousePhoto
            {
                HouseID = houseId,
                PhotoID = photo.PhotoID,
                IsCoverPhoto = isCoverPhoto
            };
            _context.HousesPhoto.Add(housePhoto);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBlobAsync(string blobUrl)
        {
            // 解析 Blob URL，獲取 Blob 名稱或 Blob Client
            BlobServiceClient blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=houserentingdata;AccountKey=n67JkHDiB7tZS7Vrene7FZuWXX0Pz7iKLaH9EIcfc13Y9FCtQMH2P0Ul3PZt+/zThdm2DZ9l4DXv+AStDH7qnw==;EndpointSuffix=core.windows.net");
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("housephoto");
            BlobClient blobClient = containerClient.GetBlobClient(blobUrl);

            // 使用 Blob Client 刪除 Blob
            await blobClient.DeleteIfExistsAsync();
        }

        public async Task<CommentDto> AddCommentAsync(Guid houseId, string content, Guid userId, string emotionresult)
        {
            // 创建评论对象
            var comment = new Comment
            {
                HouseId = houseId,
                CommentText = content,
                UserId = userId,
                CreatedAt = DateTime.UtcNow, // 在服务端自动生成 UTC 时间戳
                emotionresult= emotionresult
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
