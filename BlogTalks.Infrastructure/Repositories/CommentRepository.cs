using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Repositories;
using BlogTalks.Infrastructure.Data.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Infrastructure.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(ApplicationDbContext context) : base(context, context.Comments)
        {
        }

        public IEnumerable<Comment> GetCommentsByBlogPostId(int blogPostId)
        {
            return _dbSet.Where(c => c.BlogPostID == blogPostId);
                
        }
    }
}
