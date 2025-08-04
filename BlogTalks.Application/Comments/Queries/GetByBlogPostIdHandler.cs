using BlogTalks.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.Comments.Queries
{
    public class GetByBlogPostIdHandler : IRequestHandler<GetByBlogPostIdRequest, IEnumerable<GetByBlogPostIdResponse>>
    {
        private readonly FakeDataStore _dataStore;

        public GetByBlogPostIdHandler(FakeDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public async Task<IEnumerable<GetByBlogPostIdResponse>> Handle(GetByBlogPostIdRequest request, CancellationToken cancellationToken)
        {
            var comments = await _dataStore.GetByBlogPostId(request.BlogPostId);
            return comments.Select(c => new GetByBlogPostIdResponse
            {
                Id = c.Id,
                Text = c.Text,
                CreatedAt = c.CreatedAt,
                CreatedBy = c.CreatedBy,
                BlogPostID = c.BlogPostID
            });
        }
    }
}
