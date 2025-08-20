using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Domain.Repositories
{
    public interface ICommentRepository : IRepository<BlogTalks.Domain.Entities.Comment>
    {
        IEnumerable<Entities.Comment> GetCommentsByBlogPostId(int blogPostId);
    }
}
