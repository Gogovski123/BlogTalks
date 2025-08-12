using BlogTalks.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BlogTalks.Application.Comments.Commands
{
    public class CreateHandler : IRequestHandler<CreateRequest, CreateResponse>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateHandler(ICommentRepository commentRepository, IBlogPostRepository blogPostRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _commentRepository = commentRepository;
            _blogPostRepository = blogPostRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CreateResponse> Handle(CreateRequest request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var blogPost = _blogPostRepository.GetById(request.BlogPostId);
            if (blogPost == null)
            {
                throw new ArgumentException($"Blog post with ID {request.BlogPostId} does not exist.");
            }

            var comment = new BlogTalks.Domain.Entities.Comment
            {
                Text = request.Text,
                CreatedBy = userId,
                CreatedAt = DateTime.UtcNow,
                BlogPostID = request.BlogPostId,
                BlogPost = blogPost
            };

            _commentRepository.Add(comment);

            return new CreateResponse { Id = comment.Id };

        }
    }
}
