using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.Comments.Commands
{
    public class CreateHandler : IRequestHandler<CreateRequest, CreateResponse>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IBlogPostRepository _blogPostRepository;

        public CreateHandler(ICommentRepository commentRepository, IBlogPostRepository blogPostRepository)
        {
            _commentRepository = commentRepository;
            _blogPostRepository = blogPostRepository;
        }

        public async Task<CreateResponse> Handle(CreateRequest request, CancellationToken cancellationToken)
        {
            var blogPost = _blogPostRepository.GetById(request.BlogPostId);
            if (blogPost == null)
            {
                throw new ArgumentException($"Blog post with ID {request.BlogPostId} does not exist.");
            }

            var comment = new BlogTalks.Domain.Entities.Comment
            {
                Text = request.Text,
                CreatedBy = 55,
                CreatedAt = DateTime.UtcNow,
                BlogPostID = request.BlogPostId,
                BlogPost = blogPost
            };

            _commentRepository.Add(comment);

            return new CreateResponse { Id = comment.Id };

        }
    }
}
