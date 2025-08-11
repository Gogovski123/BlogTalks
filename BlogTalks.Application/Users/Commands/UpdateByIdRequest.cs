using MediatR;
using System.Text.Json.Serialization;

namespace BlogTalks.Application.Users.Commands
{
    public record UpdateByIdRequest([property:JsonIgnore] int Id, 
        string Name, 
        string Email, 
        string Password) : IRequest<UpdateByIdResponse>;
    

}
