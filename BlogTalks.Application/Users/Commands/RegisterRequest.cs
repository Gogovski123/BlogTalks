using MediatR;

namespace BlogTalks.Application.Users.Commands
{
    public record RegisterRequest(string Name, string Email, string Password) : IRequest<RegisterResponse>;
}
