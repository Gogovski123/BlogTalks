using BlogTalks.Domain.Repositories;
using BlogTalks.EmailSenderAPI.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;

namespace BlogTalks.Application.Comments.Commands
{
    public class CreateHandler : IRequestHandler<CreateRequest, CreateResponse>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateHandler(ICommentRepository commentRepository, IBlogPostRepository blogPostRepository,
            IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory)
        {
            _commentRepository = commentRepository;
            _blogPostRepository = blogPostRepository;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
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

            var comment = new Domain.Entities.Comment
            {
                Text = request.Text,
                CreatedBy = userId,
                CreatedAt = DateTime.UtcNow,
                BlogPostID = request.BlogPostId,
                BlogPost = blogPost
            };

            _commentRepository.Add(comment);

            var httpClient = _httpClientFactory.CreateClient("EmailSenderApi");

            var dto = new EmailDto
            {
                From = "vg123@gmail.com",
                To = "test@gmail.com",
                Subject = "New Comment Added",
                Body = $"A new comment has been added to the blog post '{blogPost.Title}' by user with id {userId}"
            };

            await httpClient.PostAsJsonAsync("/email", dto, cancellationToken);  

            return new CreateResponse { Id = comment.Id };

        }
    }
}
