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
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public IEnumerable<UserModel> GetUsers()
        {
            return _context.Users
            .ProjectTo<UserModel>(_mapper.ConfigurationProvider).AsQueryable();
        }
        public IEnumerable<UserModel> GetUsersByRole(string roleName)
        {
            try
            {
                var users = (from usr in _context.Users
                             join userRole in _context.UserRoles on usr.Id equals userRole.UserId
                             join role in _context.Roles on userRole.RoleId equals role.Id //into roles
                             where role.Name == roleName
                             select usr).ProjectTo<UserModel>(_mapper.ConfigurationProvider).AsQueryable();
                return users;
            }
            catch (Exception)
            {

                throw ;
            }
        }
        public async Task<AppUser> GetUserById( int csId)
        {
            
          return await _context.Users.Where(x => x.Id == csId).FirstOrDefaultAsync();
          
        }
        public async Task<AppUserRole> FindUserRoleById(int Id)
        {

            AppUserRole result = await _context.UserRoles.Where(x => x.UserId == Id).FirstOrDefaultAsync();
            return result;
           

        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
        public async Task<int> AddUser(AppUser user)
        {
            _context.Users.Add(user);
            return await _context.SaveChangesAsync();
        }
        public void Delete(AppUser user)
        {
            _context.Users.Remove(user);
        }
    }
}
