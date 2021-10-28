using DAL.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UserRepository: BaseRepository<User>, IUserRepository
    {
        public UserRepository(AuthDbContext dbContext): base(dbContext)
        {

        }

        public async Task<User> GetByUserNameAsync(string userName)
        {
            User result = await GetSingleAsync(U => U.UserName.Equals(userName));
            return result;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            User result = await GetSingleAsync(U => U.Email.Equals(email));
            return result;
        }

        public async Task<Guid> IsAuthenticatedAsync(string userName, string password)
        {
            var user = await GetAll()
                .Where(U => U.UserName.Equals(userName))
                .Select(X => new { X.UserId, X.Password })
                .SingleOrDefaultAsync();
            bool valid = BCrypt.Net.BCrypt.Verify(password, user.Password);
            if (valid)
            {
                return user.UserId;
            }
            else
            {
                return new Guid();
            }
        }
    }
}
