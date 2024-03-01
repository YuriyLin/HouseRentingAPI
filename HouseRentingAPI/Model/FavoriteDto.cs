using HouseRentingAPI.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseRentingAPI.Model
{
    public class FavoriteDto:GetHouseDto
    {
        public Guid UserId { get; set; }
        public Guid LandlordId { get; set; }
    }
}
