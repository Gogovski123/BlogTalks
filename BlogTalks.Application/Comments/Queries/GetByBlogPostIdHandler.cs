using BlogTalks.Domain.Repositories;
using MediatR;

namespace BlogTalks.Application.Comments.Queries
{
    public class GetByBlogPostIdHandler : IRequestHandler<GetByBlogPostIdRequest, IEnumerable<GetByBlogPostIdResponse>>
    {
        private readonly ICommentRepository _commentRepository;

        public GetByBlogPostIdHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<IEnumerable<GetByBlogPostIdResponse>> Handle(GetByBlogPostIdRequest request, CancellationToken cancellationToken)
        {
            var comments = _commentRepository.GetCommentsByBlogPostId(request.BlogPostId);
            return comments.Select(c => new GetByBlogPostIdResponse
            {
                Id = c.Id,
                Text = c.Text,
                CreatedAt = c.CreatedAt,
                CreatedBy = c.CreatedBy,
                BlogPostID = c.BlogPostID
            });
        }
    }
}
