using MediatR;

namespace BlogTalks.Application.BlogPost.Queries
{
    public class GetAllRequest : IRequest<GetAllResponse>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string? SearchWord { get; set; }
        public string? Tag { get; set; } 
    }
}
    
