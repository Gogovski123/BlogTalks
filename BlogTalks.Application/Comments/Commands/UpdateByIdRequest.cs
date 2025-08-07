using MediatR;
using System.Text.Json.Serialization;

namespace BlogTalks.Application.Comments.Commands
{
    public record UpdateByIdRequest([property:JsonIgnore] int Id, string Text): IRequest<UpdateByIdResponse>;
    
}
