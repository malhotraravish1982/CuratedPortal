using AutoMapper;
using MasterGenerator.Data.Context;
using MasterGenerator.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterGenerator.Data.Repository
{
    public class UserPermissionRepository: IUserPermissionRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserPermissionRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task AddUserPermission(FieldPermission fieldPermission)
        {
            try
            {
                await _context.FieldPermissions.AddRangeAsync(fieldPermission);
                await _context.SaveChangesAsync();
                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }
}
