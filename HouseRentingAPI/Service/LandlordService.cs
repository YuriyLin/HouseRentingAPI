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

    }
}
