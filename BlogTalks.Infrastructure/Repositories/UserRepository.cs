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
    }
}
