namespace HouseRentingAPI.Data
{
    public class Comment
    {
        public Guid CommentId { get; set; }
        public Guid UserId { get; set; }
        public Guid HouseId { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedAt { get; set; }
        public string emotionresult {  get; set; }
        public User User { get; set; }
        public House House { get; set; }
    }
}
