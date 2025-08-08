using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Repositories;
using BlogTalks.Infrastructure.Data.DataContext;

namespace BlogTalks.Infrastructure.Repositories
{
    public class BlogPostRepository : GenericRepository<BlogPost>, IBlogPostRepository
    {
        public BlogPostRepository(ApplicationDbContext context) : base(context, context.BlogPosts)
        {
        }
        public BlogPost? GetBlogByName(string name)
        {
            return _dbSet.FirstOrDefault(b => b.Title.Equals(name));
        }
    }
}
