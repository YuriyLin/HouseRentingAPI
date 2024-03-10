using AutoMapper;
using HotelListing.API.Constract;
using HouseRentingAPI.Constract;
using HouseRentingAPI.Data;
using HouseRentingAPI.Model;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;

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
            return await _context.User.FirstOrDefaultAsync(u => u.StuId == userLoginDto.StuId);
        }

        public async Task RegisterAsync(UserRegisterDto userRegisterDto)
        {
            // 檢查是否已經存在相同的學號
            if (_context.User.Any(u => u.StuId == userRegisterDto.StuId))
            {
                throw new Exception("此學號已被使用");
            }

            //將密碼進行雜湊加密
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(userRegisterDto.Password));
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                // 保存加密後的密碼到用戶對象中
                userRegisterDto.Password = hash;
            }



            if (userRegisterDto.StudentIdCard != null && userRegisterDto.StudentIdCard.Length > 0)
            {
                // 指定上传文件的保存路径
                var uploadDirectory = @"C:\Users\USER\Downloads\HouseRentingAPI\HouseRentingAPI\UploadStuIDCard";

                // 创建上传目录
                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory);
                }

                // 生成文件名并拼接文件路径
                var fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(userRegisterDto.StudentIdCard.FileName)}";
                var filePath = Path.Combine(uploadDirectory, fileName);


                // 将文件保存到指定路径
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await userRegisterDto.StudentIdCard.CopyToAsync(stream);
                }

                // 将文件路径保存到用户注册信息中
                userRegisterDto.StudentIdCardPath = filePath;

                // 将用户信息保存到数据库
                var user = _mapper.Map<User>(userRegisterDto);
                _context.User.Add(user);
                await _context.SaveChangesAsync();  
            }
        }
        public async Task<String> UpdateUserPasswordAsync(Guid userId, UpdateUserPasswordDto updateuserPasswordDto)
        {
            // 檢查舊密碼是否正確
            // 如果舊密碼不正確，返回 false
            // 否則，使用相同的加密方法對新密碼進行處理，並調用 UserRepository 更新密碼
            // 如果更新成功，返回 true，否則返回 false

            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(updateuserPasswordDto.OldPassword));
                var oldPasswordHash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                var user = await _context.User.FindAsync(userId);

                if (user == null || user.Password != oldPasswordHash)
                {
                    return "找不到用戶或舊密碼不正確";
                }

                if (updateuserPasswordDto.NewPassword != updateuserPasswordDto.ConfirmNewPassword)
                {
                    return "新密碼和確認新密碼不一致";
                }

                var newHashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(updateuserPasswordDto.NewPassword));
                var newPasswordHash = BitConverter.ToString(newHashedBytes).Replace("-", "").ToLower();

                user.Password = newPasswordHash;

                await _context.SaveChangesAsync();

                return "";
            }
        }

    }
}
