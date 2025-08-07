using BlogTalks.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Domain.Entities
{
    public class BlogPost : IEntity
    {

        public int Id { get; set; }
        public  string Title { get; set; } = string.Empty;
        public  string Text { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new List<string>();
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        
        //Navigation
        public List<Comment> Comments { get; set; } = new List<Comment>();
        
    }
}
