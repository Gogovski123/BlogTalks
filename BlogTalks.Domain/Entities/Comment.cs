using BlogTalks.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Domain.Entities
{
    public class Comment : IEntity
    {

        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }


        //Navigation
        public int BlogPostID { get; set; }
        public BlogPost BlogPost { get; set; } = new BlogPost();

    }
}
