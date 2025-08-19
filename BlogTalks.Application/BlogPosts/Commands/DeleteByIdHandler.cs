using BlogTalks.Domain.Exceptions;
using BlogTalks.Domain.Repositories;
using MediatR;

namespace BlogTalks.Application.BlogPosts.Commands
{
    public class DeleteByIdHandler : IRequestHandler<DeleteByIdRequest, DeleteByIdResponse>
    {
        private readonly IBlogPostRepository _blogPostRepository;

        public DeleteByIdHandler(IBlogPostRepository blogPostRepository)
        {
            _blogPostRepository = blogPostRepository;
        }

        public async Task<DeleteByIdResponse> Handle(DeleteByIdRequest request, CancellationToken cancellationToken)
        {
            var blogPost = _blogPostRepository.GetById(request.Id);
            if (blogPost == null)
            {
                throw new BlogTalksException($"Blog post with ID {request.Id} not found.");
            }
            _blogPostRepository.Delete(blogPost);



            return new DeleteByIdResponse();
        }
    }
}
