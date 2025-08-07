using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogTalks.Domain.Entities;

namespace BlogTalks.Application.Abstractions
{
    public interface IApplicationDbContext
    {
        public DbSet<Domain.Entities.BlogPost> BlogPosts { get; set; }
        public DbSet<BlogTalks.Domain.Entities.Comment> Comments { get; set; }
    }
}
