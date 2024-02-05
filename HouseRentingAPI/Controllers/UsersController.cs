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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            var user = await _userService.GetAllAsync();
            var record = _mapper.Map<List<GetUserDto>>(user);
            return Ok(record);
        }

        // get user all information by Id
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<GetUserDto>>> GetUserById(Guid id)
        {
            var user = await _userService.GetAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        //Update User Data
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

            return NoContent();
        }

        //User Register
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

        //User Login
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
                // 登入成功，可以返回一個Token或其他身份驗證信息
                return Ok(new { Message = "登入成功" });
            }

            else
            {
                return BadRequest(new { Message = "登入失敗" });
            }
        }

        //Delete User
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
