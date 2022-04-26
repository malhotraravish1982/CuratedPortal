using AutoMapper;
using AutoMapper.QueryableExtensions;
using MasterGenerator.Data.Context;
using MasterGenerator.Data.Entity;
using MasterGenerator.Model.Model;
using Microsoft.EntityFrameworkCore;
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
        public async Task<FieldPermission> GetVisibleFieldByUserId(int csId)
        {

            return await _context.FieldPermissions.Where(x => x.UserId == csId).FirstOrDefaultAsync();

        }
        public void Update(FieldPermission user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
        public IEnumerable<PermissionModel> GetUserPermissionRecord()
        {
            return _context.FieldPermissions
            .ProjectTo<PermissionModel>(_mapper.ConfigurationProvider).OrderByDescending(d => d.Id).AsQueryable();
        }
        public PermissionModel GetUserPermissionByUserId(int id)
        {
            var query = _context.FieldPermissions.Where(x => x.UserId == id).ProjectTo<PermissionModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            return query.Result;
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
        public PermissionModel GetPermisedRecordById(PermissionModel permissionModel)
        {
            var query = _context.FieldPermissions.Where(x =>x.UserId ==permissionModel.UserId).ProjectTo<PermissionModel>(_mapper.ConfigurationProvider).FirstOrDefault();
            return query;
        }
    }
}
