using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Shared;

namespace BlogTalks.Domain.Repositories
{
    public interface IBlogPostRepository : IRepository<BlogPost>
    {
        BlogPost? GetBlogByName(string name);
        (List<BlogPost> Results, int TotalCount) GetFiltered(
            string? searchWord,
            string? tag,
            int? pageNumber,
            int? pageSize
        );

    }
}
