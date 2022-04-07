using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterGenerator.Data.Repository
{
    public interface IUnitOfWork
    {
        IProjectRepository IProjectRepository { get; }
        IUserRepository Userrepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
    
}
