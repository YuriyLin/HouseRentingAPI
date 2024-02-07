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

            if (userRegisterDto.StudentIdCard != null && userRegisterDto.StudentIdCard.Length > 0)
            {
                // 指定上傳文件的保存路徑
                var uploadDirectory = @"C:\Users\USER\Downloads\HouseRentingAPI\HouseRentingAPI\UploadStuIDCard";
                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory); // 如果目錄不存在，則創建目錄
                }

                // 生成文件名並拼接文件路徑
                var fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(userRegisterDto.StudentIdCard.FileName)}";
                var filePath = Path.Combine(uploadDirectory, fileName);

                // 將文件保存到指定路徑
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await userRegisterDto.StudentIdCard.CopyToAsync(stream);
                }

                // 將文件路徑儲存到用户註冊訊息中，或者根據需要進行其他處理
                userRegisterDto.StudentIdCardPath = filePath;
            }

            var user = _mapper.Map<User>(userRegisterDto);
            _context.User.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
