using MasterGenerator.Data.Entity;
using MasterGenerator.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterGenerator.Data.Repository
{
    public interface IUserRepository
    {
      IEnumerable<UserModel> GetUsers();
        Task<AppUser> GetUserById(int csId);
        void Update(AppUser user);
        Task<int> AddUser(AppUser user);
        void Delete(AppUser user);
        Task<AppUserRole> FindUserRoleById(int Id);
    }
}
