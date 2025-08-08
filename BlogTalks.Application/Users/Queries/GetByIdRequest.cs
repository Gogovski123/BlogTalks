using MediatR;

namespace BlogTalks.Application.Users.Queries
{
    public record GetByIdRequest(int Id) : IRequest<GetByIdResponse>;
}
