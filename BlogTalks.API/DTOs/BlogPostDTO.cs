namespace BlogTalks.API.DTOs
{
    public class BlogPostDTO
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public List<CommentDTO> Comments { get; set; } = new List<CommentDTO>();
        public List<string> Tags { get; set; } = new List<string>();
    }
}
