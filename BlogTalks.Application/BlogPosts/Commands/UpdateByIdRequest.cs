using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlogTalks.Application.BlogPosts.Commands
{
    public record UpdateByIdRequest([property:JsonIgnore] int Id, string Title, 
        string Text, List<string> Tags) : IRequest<UpdateByIdResponse>;


}
