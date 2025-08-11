using MediatR;

namespace BlogTalks.Application.Users.Commands
{
    public record LoginRequest(string Email, string Password) : IRequest<LoginResponse>;
}
