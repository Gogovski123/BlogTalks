using MediatR;
using System.Text.Json.Serialization;

namespace BlogTalks.Application.BlogPosts.Commands
{
    public record UpdateByIdRequest([property:JsonIgnore] int Id, string Title, 
        string Text, List<string> Tags) : IRequest<UpdateByIdResponse>;


}
