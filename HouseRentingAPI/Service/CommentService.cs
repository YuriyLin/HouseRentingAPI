using HouseRentingAPI.Data;
using HouseRentingAPI.Interface;
using Microsoft.EntityFrameworkCore;

namespace HouseRentingAPI.Service
{
    public class CommentService : GenericService<Comment>, ICommentService
    {
        //以ID列出所有評論

        private readonly HouseRentingDbContext _context;

        public CommentService(HouseRentingDbContext context) : base(context)
        {
            this._context = context;
        }
        public async Task<List<Comment>> GetCommentsByHouseIdAsync(Guid houseId)
        {
            return await _context.Comment
                .Where(c => c.HouseId == houseId)
                .ToListAsync();
        }
    }
}
