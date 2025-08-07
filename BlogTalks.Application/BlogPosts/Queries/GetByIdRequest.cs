using MediatR;

namespace BlogTalks.Application.BlogPosts.Queries
{
    public record GetByIdRequest(int id) : IRequest<GetByIdResponse>;



}
