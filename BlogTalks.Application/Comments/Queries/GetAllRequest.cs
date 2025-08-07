using BlogTalks.Application.Comment.Queries;
using MediatR;

namespace BlogTalks.Application.Comments.Queries
{
    public record GetAllRequest:IRequest<IEnumerable<GetAllResponse>>;
}
