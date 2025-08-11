using MediatR;

namespace BlogTalks.Application.Users.Commands
{
    public record DeleteByIdRequest(int Id) : IRequest<DeleteByIdResponse>;
}
