using BlogTalks.Domain.Exceptions;
using BlogTalks.Domain.Repositories;
using MediatR;

namespace BlogTalks.Application.BlogPosts.Queries
{
    public class GetByIdHandler : IRequestHandler<GetByIdRequest, GetByIdResponse>
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly ICommentRepository _commentRepository;

        public GetByIdHandler(IBlogPostRepository blogPostRepository, ICommentRepository commentRepository)
        {
            _blogPostRepository = blogPostRepository;
            _commentRepository = commentRepository;
        }



        public async Task<GetByIdResponse> Handle(GetByIdRequest request, CancellationToken cancellationToken)
        {
            var blogPost = _blogPostRepository.GetById(request.id);
            if (blogPost == null)
            {
                throw new BlogTalksException($"Blog post with ID {request.id} not found.");
            }
            //var comments = _commentRepository.GetCommentsByBlogPostId(request.id);
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
