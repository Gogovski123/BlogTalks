using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Repositories;
using BlogTalks.Infrastructure.Data.DataContext;

namespace BlogTalks.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        
        public UserRepository(ApplicationDbContext context) : base(context, context.Users) { }

        public User? GetByEmail(string email)
        {
            return _dbSet.Where(u => u.Email == email).FirstOrDefault();
        }

        public User? GetByName(string name)
        {
            return _dbSet.Where(u => u.Name == name).FirstOrDefault();
        }

        public string GetUserNameById(int id)
        {
            var user = _dbSet.FirstOrDefault(u => u.Id == id);
            return user?.Name ?? "Unknown";
        }

        public IEnumerable<User> GetUsersByIds(IEnumerable<int> ids)
        {
            return _context.Users
                .Where(u => ids.Contains(u.Id));
        }
    }
}
