using BlogTalks.Domain.Repositories;
using MediatR;

namespace BlogTalks.Application.Comments.Commands
{
    public class DeleteByIdHandler : IRequestHandler<DeleteByIdRequest, DeleteByIdResponse>
    {
        private readonly ICommentRepository _commentRepository;

        public DeleteByIdHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<DeleteByIdResponse> Handle(DeleteByIdRequest request, CancellationToken cancellationToken)
        {
            var comment = _commentRepository.GetById(request.id);

            if (comment == null)
            {
                return null;
            }

            _commentRepository.Delete(comment);

            return new DeleteByIdResponse();
        }
    }
}
