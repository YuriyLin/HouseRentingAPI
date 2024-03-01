using HouseRentingAPI.Data;

namespace HouseRentingAPI.Model
{
    public class CommentAddDto
    {
        public Guid UserId { get; set; }
        public Guid HouseId { get; set; }
        public string CommentText { get; set; }
        public string? CreatedAt { get; set; }
    }
}
