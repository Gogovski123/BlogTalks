using BlogTalks.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BlogTalks.Application.BlogPosts.Commands
{
    public class CreateHandler : IRequestHandler<CreateRequest, CreateResponse>
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateHandler(IBlogPostRepository blogPostRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _blogPostRepository = blogPostRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CreateResponse> Handle(CreateRequest request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;

            if(string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var blogPost = new Domain.Entities.BlogPost
            {
                Title = request.Title,
                Text = request.Text,
                Tags = request.Tags,
                CreatedBy = userId,
                CreatedAt = DateTime.UtcNow
            };

            _blogPostRepository.Add(blogPost);

            return new CreateResponse { Id = blogPost.Id };
        }
    }
}
