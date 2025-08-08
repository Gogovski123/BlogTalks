using BlogTalks.Domain.Repositories;
using BlogTalks.Domain.Shared;
using MediatR;

namespace BlogTalks.Application.Users.Commands
{
    public class LoginHandler : IRequestHandler<LoginRequest, LoginResponse>
    {
        private readonly IUserRepository _userRepository;

        public LoginHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            var user = _userRepository.GetByEmail(request.Email);
            
            if (user == null)
            {
                return Task.FromResult(new LoginResponse { Success = false, Message = "User not found." });
            }

            var passwordValid = PasswordHasher.VerifyPassword(request.Password, user.Password);

            if(!passwordValid)
            {
                return Task.FromResult(new LoginResponse { Success = false, Message = "Invalid password." });
            }

            var fakeToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            return Task.FromResult(new LoginResponse
            {
                Success = true,
                Message = $"Welcome user { _userRepository.GetByEmail(request.Email).Name }!",
                Token = fakeToken
            });
        }
    }
}
