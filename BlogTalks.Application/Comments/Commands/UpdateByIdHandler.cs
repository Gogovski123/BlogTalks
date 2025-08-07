using BlogTalks.Domain.Repositories;
using MediatR;

namespace BlogTalks.Application.Comments.Commands
{
    public class UpdateByIdHandler : IRequestHandler<UpdateByIdRequest, UpdateByIdResponse>
    {
        private readonly ICommentRepository _commentRepository;

        public UpdateByIdHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<UpdateByIdResponse> Handle(UpdateByIdRequest request, CancellationToken cancellationToken)
        {
            var comment = _commentRepository.GetById(request.Id);
            if (comment == null)
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
