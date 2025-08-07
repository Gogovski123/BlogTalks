using MediatR;

namespace BlogTalks.Application.BlogPosts.Commands
{
    public record CreateRequest(string Title, string Text, List<string> Tags) : IRequest<CreateResponse>;
}