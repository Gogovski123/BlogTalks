using BlogTalks.Domain.Entities;

namespace BlogTalks.Application.Abstractions
{
    public interface IAuthService
    {
        string CreateToken(User user);
    }
}
