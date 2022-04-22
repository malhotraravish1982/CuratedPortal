using MasterGenerator.Data.Entity;
using MasterGenerator.Model.Model;
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
        IEnumerable<PermissionModel> GetUserPermissionRecord();
        Task<FieldPermission> GetVisibleFieldByUserId(int csId);
        void Update(FieldPermission user);
        PermissionModel GetPermisedRecordById(PermissionModel permissionModel);
    }
}
