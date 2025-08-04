using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.BlogPosts.Commands
{
    public class UpdateByIdResponse
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string Text { get; set; } = string.Empty;
        public List<string>? Tags { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<BlogTalks.Domain.Entities.Comment>? Comments { get; set; }
    }
}
