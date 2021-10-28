using DAL.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IUserRepository: IBaseRepository<User>
    {
        Task<User> GetByUserNameAsync(string userName);
        Task<User> GetByEmailAsync(string email);

        Task<Guid> IsAuthenticatedAsync(string username, string password);
    }
}
