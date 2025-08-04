using BlogTalks.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.Comments.Commands
{
    public class UpdateByIdHandler : IRequestHandler<UpdateCommentByIdCommand, UpdateByIdResponse>
    {
        private readonly FakeDataStore _dataStore;

        public UpdateByIdHandler(FakeDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public async Task<UpdateByIdResponse> Handle(UpdateCommentByIdCommand request, CancellationToken cancellationToken)
        {
            var comment = await _dataStore.GetCommentById(request.Id);
            if(comment == null)
            {
                return null;
            }
            
            comment.Text = request.Text;
            comment.CreatedAt = request.CreatedAt;
            comment.CreatedBy = request.CreatedBy;
            comment.BlogPostID = request.BlogPostId;

            await _dataStore.UpdateCommentById(comment);

            return new UpdateByIdResponse
            {
                Id = comment.Id,
                Text = comment.Text,
                CreatedAt = comment.CreatedAt,
                CreatedBy = comment.CreatedBy,
                BlogPostId = comment.BlogPostID
            };
        }
    }
}
