using AutoMapper;
using MasterGenerator.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterGenerator.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private DataContext _context;
        private IMapper _mapper;
        public UnitOfWork(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public IProjectRepository IProjectRepository => new ProjectRepository(_context, _mapper);
        public IDealDetailsRepository IDealDetailsRepository => new DealDetailsRepository(_context, _mapper);
        public IUserRepository Userrepository => new UserRepository(_context, _mapper);
        public ICustomerMapRepository ICustomerMapRepository => new CustomerMapRepository(_context, _mapper);
        public ICustomerRepository CustomerRepository => new CustomerRepository(_context, _mapper);
        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        
        public bool HasChanges()
        {
            _context.ChangeTracker.DetectChanges();
            var changes = _context.ChangeTracker.HasChanges();

            return changes;
        }

    }
}
