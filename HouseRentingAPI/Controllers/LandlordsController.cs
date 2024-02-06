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
            _context = context;
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

        // Update Landlord Data
        // PUT: api/Landlords/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLandlord(Guid id,  UpdateLandlordDto updateLandlordDto)
        {
            if (id != updateLandlordDto.LandlordID)
            {
                return BadRequest("Invalid Record Id");
            }

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
        public async Task<IActionResult> UpdateLandlordPassword(Guid id,  UpdateLandlordPasswordDto updateLandlordPasswordDto)
        {
            var landlord = await _landlordService.GetAsync(id);

            if (landlord == null)
            {
                return NotFound();
            }

            // 檢查舊密碼是否正確
            if (landlord.Password != updateLandlordPasswordDto.OldPassword)
            {
                return BadRequest(new { Message = "舊密碼不正確" });
            }

            // 檢查新密碼和確認新密碼是否一致
            if (updateLandlordPasswordDto.NewPassword != updateLandlordPasswordDto.ConfirmNewPassword)
            {
                return BadRequest(new { Message = "密碼不一致" });
            }

            // 更新密碼
            landlord.Password = updateLandlordPasswordDto.NewPassword;

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

            return Ok(new { Message = "密碼已更新" });
        }

        // POST: api/Landlords/register
        [HttpPost("register")]
        public IActionResult UserRegister([FromBody]  LandlordRegisterDto landlordRegisterDto)
        {
            if (ModelState.IsValid)
            {
                // 檢查是否已經存在相同的手機號碼
                if (_context.Landlords.Any(u => u.Phone == landlordRegisterDto.Phone))
                {
                    return BadRequest(new { Message = "該號碼已被使用" });
                }

                var landlord = _mapper.Map<Landlord>(landlordRegisterDto);
                _context.Landlords.Add(landlord);
                _context.SaveChanges();

                return Ok(new { Message = "註冊成功" });
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        // POST: api/Landlords/login
        [HttpPost("login")]
        public IActionResult UserLogin([FromBody]  LandlordLoginDto landlordLoginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var landlord = _context.Landlords.FirstOrDefault(u => u.Phone == landlordLoginDto.Phone && u.Password == landlordLoginDto.Password);

            if (landlord != null)
            {
                return Ok(new { Message = "登入成功" });
            }

            else
            {
                return BadRequest(new { Message = "登入失敗" });
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
    }
}
