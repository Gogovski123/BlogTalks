using MediatR;

namespace BlogTalks.Application.Comments.Queries
{
    public record GetByIdRequest(int id) : IRequest<GetByIdResponse>;

}
