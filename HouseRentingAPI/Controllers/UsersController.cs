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
using System.Diagnostics.Metrics;

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

            return Ok(user);
        }

        // Update User Data
        // PUT:api/Users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, UpdateUserDto updateUserDto)
        {
            if (id != updateUserDto.Id)
            {
                return BadRequest("Invalid Record Id");
            }

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
            var user = await _userService.GetAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            // 檢查舊密碼是否正確
            if (user.Password != updateuserPasswordDto.OldPassword)
            {
                return BadRequest(new { Message = "舊密碼不正確" });
            }

            // 檢查新密碼和確認新密碼是否一致
            if (updateuserPasswordDto.NewPassword != updateuserPasswordDto.ConfirmNewPassword)
            {
                return BadRequest(new { Message = "密碼不一致" });
            }

            // 更新密碼
            user.Password = updateuserPasswordDto.NewPassword;

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

            return Ok(new { Message = "密碼已更新" });
        }

        // User Register
        // POST:api/Users/register
        [HttpPost("register")]
        public IActionResult UserRegister([FromBody] UserRegisterDto userRegisterDto)
        {
            if (ModelState.IsValid)
            {
                // 檢查是否已經存在相同的學號（StuId）
                if (_context.User.Any(u => u.StuId == userRegisterDto.StuId))
                {
                    return BadRequest(new { Message = "學號已被使用" });
                }

                var user = _mapper.Map<User>(userRegisterDto);
                _context.User.Add(user);
                _context.SaveChanges();

                return Ok(new { Message = "註冊成功" });
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // User Login
        // POST : api/Users/login
        [HttpPost("login")]
        public IActionResult UserLogin([FromBody] UserLoginDto userLoginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _context.User.FirstOrDefault(u => u.StuId == userLoginDto.StuId && u.Password == userLoginDto.Password);

            if (user != null)
            {
                return Ok(new { Message = "登入成功" });
            }

            else
            {
                return BadRequest(new { Message = "登入失敗" });
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

            return Ok(new { Message = "該用戶已刪除" });
        }

        private async Task<bool> UserExists(Guid id)
        {
            return await _userService.Exists(id);
        }       
    }
}
