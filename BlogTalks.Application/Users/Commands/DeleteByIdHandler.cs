using BlogTalks.Domain.Repositories;
using MediatR;

namespace BlogTalks.Application.Users.Commands
{
    public class DeleteByIdHandler : IRequestHandler<DeleteByIdRequest, DeleteByIdResponse>
    {
        private readonly IUserRepository _userRepository;

        public DeleteByIdHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<DeleteByIdResponse> Handle(DeleteByIdRequest request, CancellationToken cancellationToken)
        {
            var user = _userRepository.GetById(request.Id);

            if (user == null)
            {
                return Task.FromResult(new DeleteByIdResponse
                {
                    Message = $"User with ID {request.Id} not found."
                });
            }

            _userRepository.Delete(user);
            
            return Task.FromResult(new DeleteByIdResponse
            {
                Message = $"User with ID {request.Id} has been successfully deleted."
            });
        }
    }
}
