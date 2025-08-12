using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BlogTalks.Application.BlogPosts.Commands
{
    public class UpdateByIdHandler : IRequestHandler<UpdateByIdRequest, UpdateByIdResponse>
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateByIdHandler(IBlogPostRepository blogPostRepository, IHttpContextAccessor httpContextAccessor)
        {
            _blogPostRepository = blogPostRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UpdateByIdResponse> Handle(UpdateByIdRequest request, CancellationToken cancellationToken)
        {
            var blogPost = _blogPostRepository.GetById(request.Id);
            if (blogPost == null)
            {
                return null;
            }
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;
            if (!int.TryParse(userIdClaim, out int currentUserId))
            {
                return null;
            }
            if (blogPost.CreatedBy != currentUserId)
            {
                return null;
            }
            blogPost.Title = request.Title;
            blogPost.Text = request.Text;
            blogPost.CreatedAt = DateTime.UtcNow;


            _blogPostRepository.Update(blogPost);

            return new UpdateByIdResponse
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                Text = blogPost.Text,
                Tags = blogPost.Tags,
                CreatedBy = blogPost.CreatedBy,
                CreatedAt = DateTime.UtcNow,

            };

        }
    }
}
