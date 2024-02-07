using AutoMapper;
using HotelListing.API.Constract;
using HouseRentingAPI.Constract;
using HouseRentingAPI.Data;
using HouseRentingAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace HouseRentingAPI.Service
{
    public class UserService : GenericService<User>, IUserService
    {
        // 獲取所有用戶
        // 通過ID獲取用戶訊息
        // 更新用戶資訊
        // 刪除用戶
        // 用戶註冊
        // 用戶登入

        private readonly HouseRentingDbContext _context;
        private readonly IMapper _mapper;

        public UserService(HouseRentingDbContext context,IMapper mapper) : base(context)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<User> loginAsync(UserLoginDto userLoginDto)
        {
            return await _context.User.FirstOrDefaultAsync(u => u.StuId == userLoginDto.StuId && u.Password == userLoginDto.Password);
        }

        public async Task RegisterAsync(UserRegisterDto userRegisterDto)
        {
            // 檢查是否已經存在相同的學號
            if (_context.User.Any(u => u.StuId == userRegisterDto.StuId))
            {
                throw new Exception("此學號已被使用");
            }

            var user = _mapper.Map<User>(userRegisterDto);
            _context.User.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
