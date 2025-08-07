namespace BlogTalks.Application.Comment.Queries
{
    public class GetAllResponse
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int BlogPostID { get; set; }
    }
}
