using AutoMapper;
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
    public class CustomerRepository : ICustomerRepository
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
        public IEnumerable<CustomerModel> GetCustomer()
        {
            List<CustomerModel> result = new List<CustomerModel>();
            var query = _context.Customers.ToList();

            var q = (from i in query
                     select
             new CustomerModel
             {
                 CustomerId = i.CustomerId,
                 CustomerName = i.CustomerName,
             }).ToList();
            return q.ToList();
        }
        public async Task<List<string>> GetCustomerNamesByUserId(int id)
        {
            var query = await (from cmap in _context.CustomerMap
                               join c in _context.Customers on cmap.CustomerId equals c.CustomerId
                               where cmap.UserId == id
                               select c.CustomerName).ToListAsync();
            return query;
        }
        public async Task<List<Customer>> FindCustomerName(int Id)
        {
            List<Customer> customerName = _context.Customers.Where(x => x.CustomerId == Id).ToList();
            return customerName;

        }

    }
}
