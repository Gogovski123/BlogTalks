using BlogTalks.Domain.Entities;

namespace BlogTalks.Domain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        User? GetByEmail(string email);
        User? GetByName(string name);
        string GetUserNameById(int id);
        public IEnumerable<User> GetUsersByIds(IEnumerable<int> ids);
    }
}
