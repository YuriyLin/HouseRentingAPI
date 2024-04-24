using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HouseRentingAPI.Data;
using AutoMapper;
using HouseRentingAPI.Interface;
using HouseRentingAPI.Model;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;

namespace HouseRentingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LandlordsController : ControllerBase
    {
        private readonly HouseRentingDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILandlordService _landlordService;

        public LandlordsController(HouseRentingDbContext context,IMapper mapper,ILandlordService landlordService)
        {
            this._context = context;
            this._mapper = mapper;
            this._landlordService = landlordService;
        }

        // GET: api/Landlords
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetLandlordDto>>> GetLandlord()
        {
            var landlord = await _landlordService.GetAllAsync();
            var record = _mapper.Map<List<GetLandlordDto>>(landlord);
            return Ok(record);
        }

        // get landlord all information by Id
        // GET: api/Landlords/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<GetLandlordByIdDto>>> GetLandlordById(Guid id)
        {
            var landlord = await _landlordService.GetAsync(id);

            if (landlord == null)
            {
                return NotFound();
            }

            return Ok(landlord);
        }

        // GET: api/Landlords/GetHouse/{LandlordId}
        [AllowAnonymous]
        [HttpGet("GetHouse/{landlordId}")]
        public async Task<ActionResult<GetHouseByIdDto>> GetHouseByLandlordId(Guid landlordId)
        {
            var Landlord = await _context.Landlords
             .Include(l => l.Houses)
                 .ThenInclude(h => h.PropertyType)
             .Include(l => l.Houses)
                 .ThenInclude(h => h.HouseFacilities)
             .Include(l => l.Houses)
                 .ThenInclude(h => h.HouseOtherAttributes)
             .Include(l => l.Houses)
                 .ThenInclude(h => h.Comments)
             .Include(l => l.Houses)
                 .ThenInclude(h => h.HousePhotos)
                     .ThenInclude(hp => hp.Photo)
             .FirstOrDefaultAsync(l => l.LandlordID == landlordId);

            if (Landlord == null)
            {
                return NotFound();
            }

            var houses = Landlord.Houses.Select(h => new GetHouseByIdDto
            {
                Housename = h.HouseName,
                Address = h.Address,
                Description = h.Description,
                Price = h.Price,
                Squarefeet = h.SquareFeet,
                Landlordname = Landlord.Landlordname,
                lineID = Landlord.LineID,
                PropertyTypeName = h.PropertyType.TypeName,
                FacilityIDs = h.HouseFacilities.Select(f => f.FacilityID).ToList(),
                AttributeIDs = h.HouseOtherAttributes.Select(a => a.AttributeID).ToList(),
                Comments = h.Comments.Select(c => c.CommentText).ToList(),
                PhotoUrl = h.HousePhotos.Select(hp => hp.Photo.PhotoURL).ToList(),
                CommentCount = h.Comments?.Count ?? 0 ,
                FavoriteCount = _context.Favorites.Count(f => f.HouseID == h.HouseID)
            }).ToList();

            return Ok(houses);
        }

        // Update Landlord Data
        // PUT: api/Landlords/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLandlord(Guid id,  UpdateLandlordDto updateLandlordDto)
        {
            var landlord = await _landlordService.GetAsync(id);

            if (landlord == null)
            {
                return NotFound();
            }

            _mapper.Map(updateLandlordDto, landlord);

            try
            {
                await _landlordService.UpdateAsync(landlord);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await LandlordExists(id))
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

        // landlord password update
        // PUT: api/Landlords/updatepassword/{id}
        [HttpPut("updatepassword/{id}")]
        public async Task<IActionResult> UpdateUserPassword(Guid id, UpdateLandlordPasswordDto updateLandlordPasswordDto)
        {
            var errorMessage = await _landlordService.UpdateLandlordPasswordAsync(id, updateLandlordPasswordDto);

            if (string.IsNullOrEmpty(errorMessage))
            {
                return Ok(new { Message = "密碼已更新" });
            }
            else
            {
                return BadRequest(new { Message = errorMessage });
            }
        }

        // POST: api/Landlords/register
        [HttpPost("register")]
        public async Task<IActionResult> LandlordRegister([FromForm] LandlordRegisterDto landlordRegisterDto)
        {
            try
            {
                await _landlordService.RegisterAsync(landlordRegisterDto);
                return Ok(new { Message = "註冊成功" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = "註冊失敗", Details = ex.Message });
            }
        }

        // POST: api/Landlords/login
        [HttpPost("login")]
        public async Task<IActionResult> LandlordLogin([FromBody]  LandlordLoginDto landlordLoginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(landlordLoginDto.Password));
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                // 根據用戶名稱查詢用戶
                var landlord = await _landlordService.loginAsync(landlordLoginDto);

                // 比較雜湊後的密碼是否匹配
                if (landlord != null && landlord.Password == hash)
                {
                    return Ok(new { Message = "登入成功", LandlordId = landlord.LandlordID });
                }
                else
                {
                    return BadRequest(new { Message = "登入失敗" });
                }
            }
        }

        // DELETE: api/Landlords/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLandlord(Guid id)
        {
            var landlord = await _landlordService.GetAsync(id);
            if (landlord == null)
            {
                return NotFound();
            }

            await _landlordService.DeleteAsync(id);

            return Ok(new { Message = "房東資料已刪除" });
        }

        private async Task<bool> LandlordExists(Guid id)
        {
            return await _landlordService.Exists(id);
        }

        //fsfsd
        //agewsg
        //wfgheg
    }
}
