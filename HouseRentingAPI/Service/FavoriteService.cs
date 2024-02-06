using HouseRentingAPI.Data;
using HouseRentingAPI.Interface;
using HouseRentingAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace HouseRentingAPI.Service
{
    public class FavoriteService : GenericService<Favorite>, IFavoriteService
    {
        private readonly HouseRentingDbContext _context;

        public FavoriteService(HouseRentingDbContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<List<FavoriteDto>> GetUserFavoritesAsync(Guid userId)
        {
            var favorites = await _context.Favorites
                .Where(f => f.UserID == userId)
                .Select(f => new FavoriteDto
                {
                    Housename = f.House.HouseName,
                    Address = f.House.Address,
                    Price = f.House.Price,
                })
                .ToListAsync();

            return favorites;
        }

        public async Task AddFavoriteAsync(Guid userId, Guid houseId)
        {
            var favorite = new Favorite
            {
                UserID = userId,
                HouseID = houseId
            };

            await _context.Favorites.AddAsync(favorite);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFavoriteAsync(Guid userId, Guid houseId)
        {
            var favorite = await _context.Favorites.FirstOrDefaultAsync(f => f.UserID == userId && f.HouseID == houseId);

            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }
        }
    }
}
