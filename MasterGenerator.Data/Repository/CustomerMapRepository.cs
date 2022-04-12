using AutoMapper;
using AutoMapper.QueryableExtensions;
using MasterGenerator.Data.Context;
using MasterGenerator.Data.Entity;
using MasterGenerator.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterGenerator.Data.Repository
{
    public class CustomerMapRepository: ICustomerMapRepository
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
        public async Task AddCustomerMap(CustomerMap customerMap)
        {
            try
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
