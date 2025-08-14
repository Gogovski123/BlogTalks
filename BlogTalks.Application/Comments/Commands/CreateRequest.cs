using MediatR;

namespace BlogTalks.Application.Comments.Commands
{
    public record CreateRequest(string Text, int BlogPostId) : IRequest<CreateResponse>;
}
