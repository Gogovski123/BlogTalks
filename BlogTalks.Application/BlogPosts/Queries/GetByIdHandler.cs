using BlogTalks.Domain.Exceptions;
using BlogTalks.Domain.Repositories;
using MediatR;

namespace BlogTalks.Application.BlogPosts.Queries
{
    public class GetByIdHandler : IRequestHandler<GetByIdRequest, GetByIdResponse>
    {
        private readonly IBlogPostRepository _blogPostRepository;

        public GetByIdHandler(IBlogPostRepository blogPostRepository)
        {
            _blogPostRepository = blogPostRepository;
        }

        public async Task<GetByIdResponse> Handle(GetByIdRequest request, CancellationToken cancellationToken)
        {
            var blogPost = _blogPostRepository.GetById(request.id);
            if (blogPost == null)
            {
                throw new BlogTalksException($"Blog post with ID {request.id} not found.");
            }
            return new GetByIdResponse
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                Text = blogPost.Text,
                CreatedBy = blogPost.CreatedBy,
                CreatedAt = blogPost.CreatedAt,
                Tags = blogPost.Tags,
                Comments = blogPost.Comments
            };
        }
    }
}
