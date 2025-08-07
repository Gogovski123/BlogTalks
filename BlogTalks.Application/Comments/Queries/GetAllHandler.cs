using BlogTalks.Application.Comments.Queries;
using BlogTalks.Domain.Repositories;
using MediatR;

namespace BlogTalks.Application.Comment.Queries
{
    public class GetAllHandler : IRequestHandler<GetAllRequest, IEnumerable<GetAllResponse>>
    {
        private readonly IRepository<BlogTalks.Domain.Entities.Comment> _commentRepository;

        public GetAllHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<IEnumerable<GetAllResponse>> Handle(GetAllRequest request, CancellationToken cancellationToken)
        {

            var comments = _commentRepository.GetAll();

            var response = comments.Select(c => new GetAllResponse
            {
                Id = c.Id,
                Text = c.Text,
                CreatedAt = DateTime.Now, // Assuming CreatedAt is set to current time for the response
                CreatedBy = c.CreatedBy,
                BlogPostID = c.BlogPostID
            });


            return response;
        }
    }
}
