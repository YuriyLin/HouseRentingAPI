using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HouseRentingAPI.Data
{
    public class Landlord
    {
        [Key]
        public Guid LandlordID { get; set; }
        [Required]
        public string Landlordname { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Phone { get; set; }
        public string? LineID { get; set; }
        public ICollection<House> Houses { get; set; }
    }
}
