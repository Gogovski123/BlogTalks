using BlogTalks.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Domain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        User? GetByEmail(string email);
        User? GetByName(string name);
    }
}
