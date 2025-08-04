using BlogTalks.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.BlogPosts.Commands
{
    public class CreateHandler : IRequestHandler<CreateRequest, CreateResponse>
    {
        private readonly FakeDataStore _fakeDataStore;

        public CreateHandler(FakeDataStore fakeDataStore)
        {
            _fakeDataStore = fakeDataStore;
        }

        public async Task<CreateResponse> Handle(CreateRequest request, CancellationToken cancellationToken)
        {
            var BlogPost = new BlogTalks.Domain.Entities.BlogPost
            {
                Id =  request.Response.Id,
                Title = request.Response.Title,
                Text = request.Response.Text,
                Tags = request.Response.Tags ?? new List<string>(),
                CreatedBy = request.Response.CreatedBy,
                CreatedAt = request.Response.CreatedAt,
                Comments = request.Response.Comments ?? new List<BlogTalks.Domain.Entities.Comment>()
            };

            await _fakeDataStore.AddBlogPost(BlogPost);

            return request.Response;
        }
    }
}
