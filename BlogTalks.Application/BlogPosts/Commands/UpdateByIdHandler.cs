using BlogTalks.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.BlogPosts.Commands
{
    public class UpdateByIdHandler : IRequestHandler<UpdateByIdRequest, UpdateByIdResponse>
    {
        private readonly FakeDataStore _fakeDataStore;

        public UpdateByIdHandler(FakeDataStore fakeDataStore)
        {
            _fakeDataStore = fakeDataStore;
        }


        public async Task<UpdateByIdResponse> Handle(UpdateByIdRequest request, CancellationToken cancellationToken)
        {
            var blogPost = await _fakeDataStore.GetBlogPostById(request.Id);
            if (blogPost == null)
            {
                return null;
            }
            blogPost.Title = request.Title;
            blogPost.Text = request.Text;
            blogPost.Tags = request.Tags ?? new List<string>();
            blogPost.CreatedBy = request.CreatedBy;
            blogPost.CreatedAt = request.CreatedAt;
            blogPost.Comments = request.Comments ?? new List<BlogTalks.Domain.Entities.Comment>();

            return new UpdateByIdResponse
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                Text = blogPost.Text,
                Tags = blogPost.Tags,
                CreatedBy = blogPost.CreatedBy,
                CreatedAt = blogPost.CreatedAt,
                Comments = blogPost.Comments
            };

        }
    }
}
