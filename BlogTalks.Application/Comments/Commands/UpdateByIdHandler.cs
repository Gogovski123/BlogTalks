using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BlogTalks.Application.Comments.Commands
{
    public class UpdateByIdHandler : IRequestHandler<UpdateByIdRequest, UpdateByIdResponse>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateByIdHandler(ICommentRepository commentRepository, IHttpContextAccessor httpContextAccessor)
        {
            _commentRepository = commentRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UpdateByIdResponse> Handle(UpdateByIdRequest request, CancellationToken cancellationToken)
        {
            var comment = _commentRepository.GetById(request.Id);
            if (comment == null)
            {
                return null;
            }
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;
            if (!int.TryParse(userIdClaim, out int currentUserId))
            {
                return null;
            }
            if (comment.CreatedBy != currentUserId)
            {
                return null;
            }
            comment.Text = request.Text;
            

            _commentRepository.Update(comment);

            return new UpdateByIdResponse
            {
                //Id = comment.Id,
                Text = comment.Text,
                //CreatedAt = comment.CreatedAt,
                //CreatedBy = comment.CreatedBy,
                //BlogPostId = comment.BlogPostID
            };
        }
    }
}
