using AutoMapper;
using MasterGenerator.Data.Context;
using MasterGenerator.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterGenerator.Data.Repository
{
    public class CustomerRepository:ICustomerRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CustomerRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<bool> AddCustomerRange(List<string> customers)
        {
            try
            {
                foreach (var customerName in customers) 
                {
                    var existingCustomer = await _context.Customers.Where(x => x.CustomerName.ToLower() == customerName.ToLower()).FirstOrDefaultAsync();
                    if (existingCustomer == null)
                    {
                        var customer = new Customer { CustomerName = customerName };
                        await _context.Customers.AddAsync(customer);
                        await _context.SaveChangesAsync();
                    }                    
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
