using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Repositories;
using BlogTalks.Domain.Shared;
using MediatR;

namespace BlogTalks.Application.Users.Commands
{
    public class RegisterHandler : IRequestHandler<RegisterRequest, RegisterResponse>
    {
        private readonly IUserRepository _userRepository;

        public RegisterHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<RegisterResponse> Handle(RegisterRequest request, CancellationToken cancellationToken)
        {
            var existingUser = _userRepository.GetByEmail(request.Email);

            if (existingUser != null)
            {
                return Task.FromResult(new RegisterResponse { Message = "User already exists with this email." });
            }

            var hashedPassword = PasswordHasher.HashPassword(request.Password);

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = hashedPassword
            };

            _userRepository.Add(user);

            return Task.FromResult(new RegisterResponse
            {
                Message = "User registered successfully.",
            });
        }
    }
}
