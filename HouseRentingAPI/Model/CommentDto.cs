using HouseRentingAPI.Data;

namespace HouseRentingAPI.Model
{
    public class CommentDto
    {
        public Guid UserId { get; set; }
        public Guid HouseId { get; set; }
        public string CommentText { get; set; }
        public string? CreatedAt { get; set; }
    }
    public class CommentUpdateDto
    {
        public Guid CommentId { get; set;}
        public string CommentText { get; set; }
    }

}
