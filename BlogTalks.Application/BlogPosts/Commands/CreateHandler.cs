using BlogTalks.Domain.Repositories;
using MediatR;

namespace BlogTalks.Application.BlogPosts.Commands
{
    public class CreateHandler : IRequestHandler<CreateRequest, CreateResponse>
    {
        private readonly IBlogPostRepository _blogPostRepository;

        public CreateHandler(IBlogPostRepository blogPostRepository)
        {
            _blogPostRepository = blogPostRepository;
        }

        public async Task<CreateResponse> Handle(CreateRequest request, CancellationToken cancellationToken)
        {
            var blogPost = new BlogTalks.Domain.Entities.BlogPost
            {
                Title = request.Title,
                Text = request.Text,
                Tags = request.Tags,
                CreatedBy = 55,
                CreatedAt = DateTime.UtcNow
            };

            _blogPostRepository.Add(blogPost);

            return new CreateResponse { Id = blogPost.Id };
        }
    }
}
