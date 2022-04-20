using MasterGenerator.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterGenerator.Data.Repository
{
    public interface IUserPermissionRepository
    {
        Task AddUserPermission(FieldPermission fieldPermission);
    }
}
