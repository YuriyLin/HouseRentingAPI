using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HouseRentingAPI.Data;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using HouseRentingAPI.Interface;
using HouseRentingAPI.Model;
using System.ComponentModel.DataAnnotations;
using HouseRentingAPI.Service;
using Azure.Storage.Blobs;

namespace HouseRentingAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Landlord")]
    [ApiController]
    public partial class HousesController : ControllerBase
    {
        private readonly HouseRentingDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHouseService _houseService;
        private readonly ICommentService _commentService;
        private readonly IHouseAttributeService _houseAttributeService;
        private readonly IHouseFacilityService _houseFacilityService;
        private readonly ILandlordService _landlordService;

        public HousesController(HouseRentingDbContext context, IMapper mapper, IHouseService houseService, ICommentService commentService,IHouseAttributeService houseAttributeService,IHouseFacilityService houseFacilityService,ILandlordService landlordService)
        {
            this._context = context;
            this._mapper = mapper;
            this._houseService = houseService;
            this._commentService = commentService;
            this._houseAttributeService = houseAttributeService;
            this._houseFacilityService = houseFacilityService;
            this._landlordService = landlordService;
        }

        // GET: api/Houses
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetHouseDto>>> GetHouses()
        {
            var houses = await _houseService.GetAllHouses();
            var record = _mapper.Map<List<GetHouseDto>>(houses);
            return Ok(record);
        }

        // GET: api/Houses/{id}
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<GetHouseByIdDto>> GetHouseById(Guid id)
        {
            var house = await _houseService.GetHouseById(id);

            if (house == null)
            {
                return NotFound();
            }

            return Ok(house);
        }

        // GET: api/Houses/compare
        [AllowAnonymous]
        [HttpGet("compare")]
        public async Task<IActionResult> CompareHouses([FromQuery] List<Guid> houseIds)
        {
            if (houseIds == null || houseIds.Count != 2)
            {
                return BadRequest("Please provide exactly two house IDs.");
            }

            try
            {
                var houses = await _houseService.GetHousesByIds(houseIds);
                if (houses == null || houses.Count == 0)
                {
                    return NotFound("One or both of the specified houses could not be found.");
                }

                return Ok(houses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [AllowAnonymous]
        [HttpGet("Paged")]
        public async Task<IActionResult> GetPagedHouses(int pageNumber = 1, int pageSize = 12)
        {
            try
            {
                var pagedHouses = await _houseService.GetPagedHouses(pageNumber, pageSize);
                return Ok(pagedHouses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // House Searching
        // Get:api/Houses/Search/{keyword}  
        [AllowAnonymous]
        [HttpGet("SearchAndSort")]
        public async Task<ActionResult<IEnumerable<GetHouseDto>>> SearchHousesAndSort(
        [FromQuery] string? keyword,
        [FromQuery] int propertyTypeID = 0,
        [FromQuery] string? facilityIDs = null,
        [FromQuery] string? attributeIDs = null,
        [FromQuery] int minPrice = 0,
        [FromQuery] int maxPrice = 0,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool isDescending = false,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 8)
        {
            try
            {
                // 将逗号分隔的字符串拆分为列表
                List<int>? facilityIDsList = facilityIDs?.Split(',').Select(int.Parse).ToList();
                List<int>? attributeIDsList = attributeIDs?.Split(',').Select(int.Parse).ToList();

                // 进行房屋搜索
                var result = await _houseService.SearchHouses(keyword, propertyTypeID, facilityIDsList, attributeIDsList, minPrice, maxPrice);

                // 如果指定了排序参数，则进行排序
                if (!string.IsNullOrWhiteSpace(sortBy))
                {
                    switch (sortBy.ToLower())
                    {
                        case "price":
                            result = isDescending ? result.OrderByDescending(h => h.Price).ToList() : result.OrderBy(h => h.Price).ToList();
                            break;
                        case "favoritecount":
                            result = isDescending ? result.OrderByDescending(h => h.FavoriteCount).ToList() : result.OrderBy(h => h.FavoriteCount).ToList();
                            break;
                        case "commentcount":
                            result = isDescending ? result.OrderByDescending(h => h.CommentCount).ToList() : result.OrderBy(h => h.CommentCount).ToList();
                            break;
                        default:
                            break;
                    }
                }
                return Ok(result);

                //var pagedResult = result.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                //return Ok(pagedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        // POST: api/Houses
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<HouseAddDto>> CreateHouse(HouseAddDto houseAddDto)
        {
            var landlord = await _landlordService.GetAsync(houseAddDto.LandlordID);

            if (landlord == null)
            {
                return BadRequest("Landlord not found.");
            }

            var house = _mapper.Map<House>(houseAddDto);

            house.Landlord = landlord;
            await _houseService.AddAsync(house);
            var houseID = house.HouseID;

            // 將 houseID 設置到 houseFacility 和 houseAttribute 的 HouseID 屬性
            foreach (var facilityId in houseAddDto.FacilityIDs)
            {
                var facility = await _houseFacilityService.GetByIdAsync(houseID, facilityId);
                if (facility == null)
                {
                    house.HouseFacilities.Add(new HouseFacility { HouseID = houseID, FacilityID = facilityId });
                }
            }

            foreach (var attributeId in houseAddDto.AttributeIDs)
            {
                var attribute = await _houseAttributeService.GetByIdAsync(houseID, attributeId);
                if (attribute == null)
                {
                    house.HouseOtherAttributes.Add(new HouseOtherAttribute { HouseID = houseID, AttributeID = attributeId });
                }
            }

            bool isFirstPhoto = true;
            foreach (var photoFile in houseAddDto.HousePhotos)
            {
                await _houseService.SaveHousePhotoAsync(houseID, photoFile, isFirstPhoto);
                //將第一張照片設置為封面照片，其餘照片為非封面照片
                isFirstPhoto = false;
            } 

            // 更新 house
            await _houseService.UpdateAsync(house);
            return CreatedAtAction("GetHouseById", new { id = house.HouseID }, _mapper.Map<GetHouseByIdDto>(house));
        }




        // PUT: api/Houses/{id}
        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHouse(Guid id, UpdateHouseDto updateHouseDto)
        {
            if (id != updateHouseDto.HouseID)
            {
                return BadRequest("Invalid Record Id");
            }

            var house = await _houseService.GetAsync(id);

            if (house == null)
            {
                return NotFound();
            }

            _mapper.Map(updateHouseDto, house);

            // 先刪除該房屋原有的所有 HouseFacility 和 HouseOtherAttribute 記錄
            var existingHouseFacilities = _context.HouseFacilities.Where(hf => hf.HouseID == id);
            _context.HouseFacilities.RemoveRange(existingHouseFacilities);

            var existingHouseOtherAttributes = _context.HouseOtherAttributes.Where(hoa => hoa.HouseID == id);
            _context.HouseOtherAttributes.RemoveRange(existingHouseOtherAttributes);

            // 重新添加 HouseFacility 和 HouseOtherAttribute 記錄
            foreach (var facilityId in updateHouseDto.FacilityIDs)
            {
                var facility = await _houseFacilityService.GetByIdAsync(id, facilityId);
                if (facility == null)
                {
                    house.HouseFacilities.Add(new HouseFacility { HouseID = id, FacilityID = facilityId });
                }
            }

            foreach (var attributeId in updateHouseDto.AttributeIDs)
            {
                var attribute = await _houseAttributeService.GetByIdAsync(id, attributeId);
                if (attribute == null)
                {
                    house.HouseOtherAttributes.Add(new HouseOtherAttribute { HouseID = id, AttributeID = attributeId });
                }
            }

            // 刪除該房屋原有的所有照片記錄
            var existingHousePhotos = _context.HousesPhoto.Where(hp => hp.HouseID == id);
            _context.HousesPhoto.RemoveRange(existingHousePhotos);

            // 上傳新的照片
            bool isFirstPhoto = true;
            foreach (var photoFile in updateHouseDto.HousePhotos)
            {
                await _houseService.SaveHousePhotoAsync(id, photoFile, isFirstPhoto);
                isFirstPhoto = false;
            }

            try
            {
                await _houseService.UpdateAsync(house);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await HouseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { Message = "資料已更新" });
        }

        // DELETE: api/Houses/{id}
        [AllowAnonymous]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHouse(Guid id)
        {
            var house = await _houseService.GetAsync(id);
            if (house == null)
            {
                return NotFound();
            }

            // 獲取房屋的所有相關照片
            var housePhotos = await _context.HousesPhoto
                .Include(hp => hp.Photo)
                .Where(hp => hp.HouseID == id)
                .ToListAsync();

            // 逐一刪除房屋的相關照片
            foreach (var photo in housePhotos)
            {
                // 刪除 Azure Blob Storage 中的照片
                await _houseService.DeleteBlobAsync(photo.Photo.PhotoURL);

                // 從數據庫中刪除 HousePhoto 記錄
                _context.HousesPhoto.Remove(photo);
                _context.Photo.Remove(photo.Photo);
            }

            await _context.SaveChangesAsync();

            await _houseService.DeleteAsync(id);

            return Ok(new { Message = "房屋資料已刪除" });
        }


        private async Task<bool> HouseExists(Guid id)
        {
            return await _houseService.Exists(id);
        }

        //-----------Comment-------------------(Add/Update/Delete)
        // POST: api/Houses/comments
        [AllowAnonymous]
        [HttpPost("comments")]
        public async Task<IActionResult> AddComment([FromBody] CommentDto commentDto)
        {
            try
            {
                Guid houseId = commentDto.HouseId;
                string content = commentDto.CommentText;
                Guid userId = commentDto.UserId;
                string emotionresult = commentDto.emotionresult;
                await _houseService.AddCommentAsync(houseId, content, userId, emotionresult);
                return Ok(new { Message = "Comment added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Failed to add comment.", Details = ex.Message });
            }
        }

        // GET: api/Houses/{houseId}/comments
        [AllowAnonymous]
        [HttpGet("{houseId}/comments")]
        public async Task<ActionResult<List<AllCommentDto>>> GetCommentsByHouseId(Guid houseId)
        {
            var house = await _context.Houses
                .Include(h => h.Comments)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(h => h.HouseID == houseId);

            if (house == null)
            {
                return NotFound();
            }
            var comments = house.Comments.Select(c => new AllCommentDto
            {
                CommentId=c.CommentId,
                Name = c.User.Name, 
                CommentText = c.CommentText,
                emotionresult=c.emotionresult
            }).ToList();

            return Ok(comments);
        }

        // PUT: api/Houses/comments/{commentId}
        [AllowAnonymous]
        [HttpPut("comments/{commentId}")]
        public async Task<IActionResult> UpdateComment(Guid commentId, [FromBody] CommentUpdateDto commentUpdateDto)
        {
            try
            {
                if (commentId != commentUpdateDto.CommentId)
                    return BadRequest("Invalid comment ID.");

                // 提取要更新的評論文本
                string commentText = commentUpdateDto.CommentText;

                // 呼叫服務方法來更新評論
                await _houseService.UpdateCommentAsync(commentId, commentText);

                return Ok(new { Message = "Comment updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Failed to update comment.", Details = ex.Message });
            }
        }

        // DELETE: api/Houses/comments/{commentId}
        [AllowAnonymous]
        [HttpDelete("comments/{commentId}")]
        public async Task<IActionResult> DeleteComment(Guid commentId)
        {
            try
            {
                await _houseService.DeleteCommentAsync(commentId);
                return Ok(new { Message = "Comment deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Failed to delete comment.", Details = ex.Message });
            }
        }

        //---------------------------------------------------------
    }
}
