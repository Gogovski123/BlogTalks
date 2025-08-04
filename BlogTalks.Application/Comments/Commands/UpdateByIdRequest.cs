using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.Comments.Commands
{
    public record UpdateCommentByIdCommand(int Id, string Text, 
        DateTime CreatedAt, int CreatedBy, int BlogPostId): IRequest<UpdateByIdResponse>;
    
}
