using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.BlogPosts.Commands
{
    public record UpdateByIdRequest(int Id, string Title, 
        string Text, List<string>? Tags, int CreatedBy, DateTime CreatedAt, 
        List<BlogTalks.Domain.Entities.Comment>? Comments) : IRequest<UpdateByIdResponse>;


}
