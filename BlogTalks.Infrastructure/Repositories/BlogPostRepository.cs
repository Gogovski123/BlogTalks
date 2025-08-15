using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Repositories;
using BlogTalks.Domain.Shared;
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

        public (List<BlogPost> Results, int TotalCount) GetFiltered(string? searchWord, string? tag, int? pageNumber, int? pageSize)
        {
            var query = _dbSet.AsQueryable();

            if (!string.IsNullOrEmpty(searchWord))
            {
                
                query = query.Where(bp => bp.Title.ToLower().Contains(searchWord.ToLower()) || bp.Text.ToLower().Contains(searchWord.ToLower()));
            }
            
            if (!string.IsNullOrEmpty(tag))
            {
                query = query.Where(bp => bp.Tags.Contains(tag));
            }

            var totalCount = query.Count();

            if (pageNumber.HasValue && pageSize.HasValue && pageSize > 0)
            {
                query = query
                    .Skip((pageNumber.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);
            }

            

            return (query.ToList(), totalCount);
        }
    }
}
