using BlogTalks.Domain.Shared;

namespace BlogTalks.Application.BlogPost.Queries
{
    public class GetAllResponse
    {
        public List<BlogPostModel> BlogPosts { get; set; } = new List<BlogPostModel>();
        public Metadata Metadata { get; set; }
    }
}
