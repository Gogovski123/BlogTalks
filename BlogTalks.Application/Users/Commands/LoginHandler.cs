using BlogTalks.Application.Abstractions;
using BlogTalks.Domain.Repositories;
using BlogTalks.Domain.Shared;
using MediatR;

namespace BlogTalks.Application.Users.Commands
{
    public class LoginHandler : IRequestHandler<LoginRequest, LoginResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public LoginHandler(IUserRepository userRepository, IAuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }

        public Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            var user = _userRepository.GetByEmail(request.Email);
            
            if (user == null)
            {
                return Task.FromResult(new LoginResponse { Message = "User not found." });
            }

            var passwordValid = PasswordHasher.VerifyPassword(request.Password, user.Password);

            if(!passwordValid)
            {
                return Task.FromResult(new LoginResponse { Message = "Invalid password." });
            }

            var token = _authService.CreateToken(user);


            return Task.FromResult(new LoginResponse
            {
                Token = token
            });
        }
    }
}
