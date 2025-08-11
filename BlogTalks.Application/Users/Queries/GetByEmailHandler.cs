using BlogTalks.Domain.Repositories;
using MediatR;

namespace BlogTalks.Application.Users.Queries
{
    public class GetByEmailHandler : IRequestHandler<GetByEmailRequest, GetByEmailResponse>
    {
        private readonly IUserRepository _userRepository;

        public GetByEmailHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetByEmailResponse> Handle(GetByEmailRequest request, CancellationToken cancellationToken)
        {
            var user = _userRepository.GetByEmail(request.Email);
            if (user == null)
            {
                return null;
            }

            return new GetByEmailResponse
            {
                Name = user.Name
            };
        }
    }
}
