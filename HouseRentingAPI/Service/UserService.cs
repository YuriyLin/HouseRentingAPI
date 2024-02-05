using HotelListing.API.Constract;
using HouseRentingAPI.Constract;
using HouseRentingAPI.Data;

namespace HouseRentingAPI.Service
{
    public class UserService : GenericService<User>, IUserService
    {
        // 獲取所有用戶
        // 通過ID獲取用戶訊息
        // 增加新用戶
        // 更新用戶資訊
        // 刪除用戶
        private readonly HouseRentingDbContext _context;

        public UserService(HouseRentingDbContext context) : base(context)
        {
            this._context = context;
        }
    }
}
