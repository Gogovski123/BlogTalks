using MediatR;

namespace BlogTalks.Application.BlogPosts.Commands
{
    public record DeleteByIdRequest(int Id) : IRequest<DeleteByIdResponse>;
    

}
