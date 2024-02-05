using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HouseRentingAPI.Data
{
    public class ComparisonList
    {
        [Key]
        public Guid ComparisonID { get; set; }
        public Guid UserID { get; set; }
        public Guid HouseID { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; }

        [ForeignKey("HouseID")]
        public House House { get; set; }
    }
}
