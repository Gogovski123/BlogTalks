using BlogTalks.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Domain.Repositories
{
    public interface IBlogPostRepository : IRepository<BlogTalks.Domain.Entities.BlogPost>
    {
        BlogPost? GetBlogByName(string name);
    }
}
