using BlogTalks.Domain.Repositories;
using BlogTalks.Domain.Shared;
using MediatR;

namespace BlogTalks.Application.BlogPosts.Queries
{
    public class GetAllHandler : IRequestHandler<GetAllRequest, GetAllResponse>
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IUserRepository _userRepository;

        public GetAllHandler(IBlogPostRepository blogPostRepository, IUserRepository userRepository)
        {
            _blogPostRepository = blogPostRepository;
            _userRepository = userRepository;
        }

        public async Task<GetAllResponse> Handle(GetAllRequest request, CancellationToken cancellationToken)
        {
            
            var (initialResult, totalCount) = _blogPostRepository.GetFiltered(
                request.SearchWord,
                request.Tag,
                request.PageNumber,
                request.PageSize
            );


            int totalPages = (request.PageSize.HasValue && request.PageSize > 0)
                ? (int)Math.Ceiling((double)totalCount / request.PageSize.Value)
                : 1;


            var blogPostModels = initialResult.Select(bp => new BlogPostModel
            {
                Id = bp.Id,
                Title = bp.Title,
                Text = bp.Text,
                Tags = bp.Tags,
                CreatedBy = bp.CreatedBy
            }).ToList();

            var userIds = initialResult.Select(bp => bp.CreatedBy).Distinct().ToList();
            var userList = _userRepository.GetUsersByIds(userIds);
            foreach (var blog in blogPostModels)
            {
                blog.CreatorName = userList.FirstOrDefault(u => u.Id == blog.CreatedBy)?.Name ?? "Unknown";
            }

            return new GetAllResponse
            {
                BlogPosts = blogPostModels,
                Metadata = new Metadata
                {
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber.Value,
                    PageSize = request.PageSize ?? totalCount,
                    TotalPages = totalPages
                }
            };
        }
    }
}
