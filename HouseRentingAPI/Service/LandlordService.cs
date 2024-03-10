using AutoMapper;
using HouseRentingAPI.Data;
using HouseRentingAPI.Interface;
using HouseRentingAPI.Model;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace HouseRentingAPI.Service
{
    public class LandLordService:GenericService<Landlord>, ILandlordService
    {
        // 獲取所有用戶
        // 以ID獲取用戶訊息
        // 更新用戶資訊
        // 刪除用戶
        // 用戶註冊
        // 用戶登入

        private readonly HouseRentingDbContext _context;
        private readonly IMapper _mapper;

        public LandLordService(HouseRentingDbContext context,IMapper mapper) : base(context)
        {
            this._context = context;
            this._mapper = mapper;
        }
        public async Task<Landlord> loginAsync(LandlordLoginDto landlordLoginDto)
        {
            return await _context.Landlords.FirstOrDefaultAsync(u => u.Phone == landlordLoginDto.Phone);
        }

        public async Task RegisterAsync(LandlordRegisterDto landlordRegisterDto)
        {
            // 檢查是否已經存在相同的手機號碼
            if (_context.Landlords.Any(u => u.Phone == landlordRegisterDto.Phone))
            {
                throw new Exception("該號碼已被使用");
            }

            //將密碼進行雜湊加密
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(landlordRegisterDto.Password));
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                // 保存加密後的密碼到用戶對象中
                landlordRegisterDto.Password = hash;
            }

            var landlord = _mapper.Map<Landlord>(landlordRegisterDto);
            _context.Landlords.Add(landlord);
            await _context.SaveChangesAsync();
        }
        public async Task<String> UpdateLandlordPasswordAsync(Guid userId, UpdateLandlordPasswordDto updateLandlordPasswordDto)
        {
            // 檢查舊密碼是否正確
            // 如果舊密碼不正確，返回 false
            // 否則，使用相同的加密方法對新密碼進行處理，並更新密碼
            // 如果更新成功，返回 true，否則返回 false

            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(updateLandlordPasswordDto.OldPassword));
                var oldPasswordHash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                var landlord = await _context.Landlords.FindAsync(userId);

                if (landlord == null || landlord.Password != oldPasswordHash)
                {
                    return "找不到房東或舊密碼不正確";
                }

                if (updateLandlordPasswordDto.NewPassword != updateLandlordPasswordDto.ConfirmNewPassword)
                {
                    return "新密碼和確認新密碼不一致";
                }

                var newHashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(updateLandlordPasswordDto.NewPassword));
                var newPasswordHash = BitConverter.ToString(newHashedBytes).Replace("-", "").ToLower();

                landlord.Password = newPasswordHash;

                await _context.SaveChangesAsync();

                return "";
            }
        }
    }
}
