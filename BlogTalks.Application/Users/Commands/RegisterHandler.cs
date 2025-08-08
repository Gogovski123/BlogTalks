using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Repositories;
using MediatR;
using System.Net.Mail;
using BlogTalks.Domain.Shared;

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
            
            var existingUser = _userRepository.GetByName(request.Name);

            if (existingUser != null)
            {
                return Task.FromResult(new RegisterResponse { Message = "User already exists with this name." });
            }

            existingUser = _userRepository.GetByEmail(request.Email);

            try
            {
                var addr = new MailAddress(request.Email);
                if (addr.Address != request.Email)
                {
                    return Task.FromResult(new RegisterResponse { Message = "Invalid email format." });
                }
            }
            catch
            {
                return Task.FromResult(new RegisterResponse { Message = "Invalid email format." });
            }

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
