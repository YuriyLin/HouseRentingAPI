using System.ComponentModel.DataAnnotations;

namespace HouseRentingAPI.Data
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string StuId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string PhoneNum { get; set; }
        public string? Email { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<ComparisonList> ComparisonLists { get; set; }
    }
}
