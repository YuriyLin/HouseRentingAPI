using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HouseRentingAPI.Data;
using HouseRentingAPI.Model;
using AutoMapper;
using HouseRentingAPI.Constract;
using HouseRentingAPI.Interface;

namespace HouseRentingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly HouseRentingDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFavoriteService _favoriteService;

        public FavoritesController(HouseRentingDbContext context, IMapper mapper,  IFavoriteService favoriteService)
        {
            this._context = context;
            this._mapper = mapper;
            this._favoriteService = favoriteService;
        }

        // GET: api/Favorites/{userId}
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<FavoriteDto>>> GetUserFavorites(Guid userId)
        {
            var favorites = await _favoriteService.GetUserFavoritesAsync(userId);
            if (favorites == null)
            {
                return NotFound();
            }
            return Ok(favorites);
        }

        // POST: api/Favorites/Add/{userid/{houseId}}
        [HttpPost("Add/{userid}/{houseId}")]
        public async Task<IActionResult> AddToFavorite([FromRoute]Guid userId, [FromRoute] Guid houseId)
        {
            try
            {
                if (_context.Favorites.Any(f => f.UserID == userId && f.HouseID == houseId))
                {
                    return BadRequest(new { Message = "房屋已在收藏列表中" });
                }

                // 使用 Repository 將收藏記錄新增到資料庫
                await _favoriteService.AddFavoriteAsync(userId, houseId);

                return Ok(new { Message = "房屋已成功添加到收藏列表" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE : api/Favorites/Remove/{userid}
        [HttpDelete("Remove/{userId}/{houseId}")]
        public async Task<IActionResult> RemoveFromFavorites([FromRoute]Guid userId, [FromRoute] Guid houseId)
        {
            try
            {
                // 使用 Repository 將房屋從收藏列表中刪除
                await _favoriteService.RemoveFavoriteAsync(userId, houseId);

                return Ok(new { Message = "房屋已成功從收藏列表中移除" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
