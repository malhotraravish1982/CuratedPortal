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
    public class CustomerMapRepository : ICustomerMapRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CustomerMapRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public IEnumerable<CustomerModel> CustomerMap()
        {
            return _context.CustomerMap
            .ProjectTo<CustomerModel>(_mapper.ConfigurationProvider).AsQueryable();

        }
        public IEnumerable<CustomerModel> GetCutomerMaped()
        {
            try
            {
                var model = (from usr in _context.Users
                             join map in _context.CustomerMap on usr.Id equals map.UserId
                             join cus in _context.Customers on map.CustomerId equals cus.CustomerId
                             select new CustomerModel
                             {
                                 Id = map.Id,
                                 UserId = map.Id,
                                 UserName = usr.FirstName,
                                 CustomerName = cus.CustomerName,
                                 CustomerId = map.CustomerId,
                             }).AsQueryable();
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task AddCustomerMap(CustomerMap customerMap)
        {
                await _context.CustomerMap.AddRangeAsync(customerMap);
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
