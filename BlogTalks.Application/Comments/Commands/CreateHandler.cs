using BlogTalks.Application.Abstractions;
using BlogTalks.Domain.Repositories;
using BlogTalks.Application.Contracts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;

namespace BlogTalks.Application.Comments.Commands
{
    public class CreateHandler : IRequestHandler<CreateRequest, CreateResponse>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IServiceProvider _serviceProvider;
        private readonly IFeatureManager _featureManager;
        private readonly IUserRepository _userRepository;

        public CreateHandler(ICommentRepository commentRepository, IBlogPostRepository blogPostRepository,
            IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory, IFeatureManager featureManager, IServiceProvider serviceProvider, IUserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _blogPostRepository = blogPostRepository;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _featureManager = featureManager;
            _serviceProvider = serviceProvider;
            _userRepository = userRepository;
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

            var blogPostCreator = _userRepository.GetById(blogPost.CreatedBy);
            var commentCreator = _userRepository.GetById(userId);

            var dto = new EmailDto
            {
                From = commentCreator.Email,
                To = blogPostCreator.Email,
                Subject = "New Comment Added",
                Body = $"A new comment has been added to the blog post '{blogPost.Title}' by user with id {userId}"
            };

            if(await _featureManager.IsEnabledAsync("EmailHttpSender"))
            {
                var service = _serviceProvider.GetRequiredKeyedService<IMessagingService>("MessagingHttpService");
                await service.Send(dto);
            }
            else if(await _featureManager.IsEnabledAsync("EmailRabbitMQSender"))
            {
                var service = _serviceProvider.GetRequiredKeyedService<IMessagingService>("MessagingRabbitMQService");
                await service.Send(dto);
            }
            else
            {

            }

         return new CreateResponse { Id = comment.Id };

        }
    }
}
