using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HouseRentingAPI.Data;
using HouseRentingAPI.Model;
using AutoMapper;
using HouseRentingAPI.Constract;
using System.Security.Cryptography;
using System.IO;
using System.Diagnostics.Metrics;
using System.Text;

namespace HouseRentingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly HouseRentingDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UsersController(HouseRentingDbContext context, IMapper mapper, IUserService userService)
        {
            this._context = context;
            this._mapper = mapper;
            this._userService = userService;
        }

        // get all user information
        // GET:api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetUserDto>>> GetUser()
        {
            var user = await _userService.GetAllAsync();
            var record = _mapper.Map<List<GetUserDto>>(user);
            return Ok(record);
        }

        // get user all information by Id
        // GET:api/Users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<GetUserByIdDto>>> GetUserById(Guid id)
        {
            var user = await _userService.GetAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            var record = _mapper.Map<GetUserByIdDto>(user);

            return Ok(record);
        }

        // Update User Data
        // PUT:api/Users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, UpdateUserDto updateUserDto)
        {
            var user = await _userService.GetAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            _mapper.Map(updateUserDto, user);

            try
            {
                await _userService.UpdateAsync(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserExists(id))
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

        // user password update
        // PUT:api/Users/{id}
        [HttpPut("updatepassword/{id}")]
        public async Task<IActionResult> UpdateUserPassword(Guid id, UpdateUserPasswordDto updateuserPasswordDto)
        {
            var errorMessage = await _userService.UpdateUserPasswordAsync(id, updateuserPasswordDto);

            if (string.IsNullOrEmpty(errorMessage))
            {
                return Ok(new { Message = "密碼已更新" });
            }
            else
            {
                return BadRequest(new { Message = errorMessage });
            }
        }


        // User Register
        // POST:api/Users/register
        [HttpPost("register")]
        public async Task<IActionResult> UserRegister([FromForm] UserRegisterDto userRegisterDto)
        {
            try
            {
                await _userService.RegisterAsync(userRegisterDto);
                return Ok(new { Message = "註冊成功" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = "註冊失敗", Details = ex.Message });
            }
        }

        // User Login
        // POST : api/Users/login
        [HttpPost("login")]
        public async Task<IActionResult> UserLogin([FromBody] UserLoginDto userLoginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(userLoginDto.Password));
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                // 根據用戶名稱查詢用戶
                var user = await _userService.loginAsync(userLoginDto);

                // 比較雜湊後的密碼是否匹配
                if (user != null && user.Password == hash)
                {
                    return Ok(new { Message = "登入成功", UserId = user.Id });
                }
                else
                {
                    return BadRequest(new { Message = "登入失敗"});
                }
            }
        }



        // Delete User
        // DELETE : api/Users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _userService.GetAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.DeleteAsync(id);

            if (!string.IsNullOrEmpty(user.StudentIdCardPath) && System.IO.File.Exists(user.StudentIdCardPath))
            {
                System.IO.File.Delete(user.StudentIdCardPath);
            }

            return Ok(new { Message = "該用戶已刪除" });
        }

        private async Task<bool> UserExists(Guid id)
        {
            return await _userService.Exists(id);
        }       
    }
}
