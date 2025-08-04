using BlogTalks.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.Comments.Commands
{
    public class CreateHandler : IRequestHandler<AddCommentCommand, CreateResponse>
    {
        private readonly FakeDataStore _fakeDataStore;

        public CreateHandler(FakeDataStore fakeDataStore)
        {
            _fakeDataStore = fakeDataStore;
        }


        public async Task<CreateResponse> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            
            var comment = new BlogTalks.Domain.Entities.Comment
            {
                Id = request.CommentResponse.Id,
                Text = request.CommentResponse.Text,
                CreatedBy = request.CommentResponse.CreatedBy,
                CreatedAt = request.CommentResponse.CreatedAt,
                BlogPostID = request.CommentResponse.BlogPostId
            };

            await _fakeDataStore.AddComment(comment);

            return request.CommentResponse;
        }
    }
}
