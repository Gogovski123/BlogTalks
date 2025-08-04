using BlogTalks.Application.BlogPost.Queries;
using BlogTalks.Application.Comment.Queries;
using BlogTalks.Application.Comments.Queries;
using BlogTalks.Domain.Entities;
using MediatR;

namespace BlogTalks.Application.BlogPosts.Queries
{
    public class GetAllHandler : IRequestHandler<BlogPost.Queries.GetAllRequest, IEnumerable<BlogPost.Queries.GetAllResponse>>
    {
        private readonly FakeDataStore _fakeDataStore;

        public GetAllHandler(FakeDataStore fakeDataStore)
        {
            _fakeDataStore = fakeDataStore;
        }

        public async Task<IEnumerable<BlogPost.Queries.GetAllResponse>> Handle(BlogPost.Queries.GetAllRequest request, CancellationToken cancellationToken)
        {
            var blogPosts = await _fakeDataStore.GetAllBlogPosts();
            var response = blogPosts.Select(bp => new BlogPost.Queries.GetAllResponse
            {
                Id = bp.Id,
                Title = bp.Title,
                Text = bp.Text,
                Tags = bp.Tags,
                CreatedBy = bp.CreatedBy,
                CreatedAt = bp.CreatedAt,
                Comments = bp.Comments
            });

            return response;
        }
    }
}
