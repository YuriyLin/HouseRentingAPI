using HouseRentingAPI.Data;
using HouseRentingAPI.Interface;
using HouseRentingAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HouseRentingAPI.Service
{
    public class FavoriteService : GenericService<Favorite>, IFavoriteService
    {
        // 列出用戶收藏列表
        // 加入收藏
        // 移除收藏

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
                    HouseID = f.House.HouseID,
                    Housename = f.House.HouseName,
                    Address = f.House.Address,
                    PropertyTypeName = f.House.PropertyType.TypeName, 
                    SquareFeet = f.House.SquareFeet, 
                    Price = f.House.Price,
                    CoverPhotoUrl = f.House.HousePhotos.FirstOrDefault(p => p.IsCoverPhoto).Photo.PhotoURL, 
                    FacilityIDs = f.House.HouseFacilities.Select(hf => hf.FacilityID).ToList(),
                    AttributeIDs = f.House.HouseOtherAttributes.Select(hoa => hoa.OtherAttribute.AttributeID).ToList(),
                    UserId = f.UserID,
                    LandlordId = f.House.LandlordID 
                })
                .ToListAsync();

            return favorites;
        }

        public async Task<List<FavoitebyUserIdDto>> GetUserFavoritesHouse(Guid userId)
        {
            var favorites = await _context.Favorites
                .Where(f => f.UserID == userId)
                .Select(f => new FavoitebyUserIdDto
                {
                    HouseId = f.House.HouseID, 
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
