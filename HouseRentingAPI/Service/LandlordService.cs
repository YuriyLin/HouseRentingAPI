using HouseRentingAPI.Data;
using HouseRentingAPI.Interface;

namespace HouseRentingAPI.Service
{
    public class LandLordService:GenericService<Landlord>, ILandlordService
    {
        // 獲取所有用戶
        // 通過ID獲取用戶訊息
        // 增加新用戶
        // 更新用戶資訊
        // 刪除用戶
        private readonly HouseRentingDbContext _context;

        public LandLordService(HouseRentingDbContext context) : base(context)
        {
            this._context = context;
        }
    }
}
