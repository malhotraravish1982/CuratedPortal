using MasterGenerator.Data.Entity;
using MasterGenerator.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterGenerator.Data.Repository
{
    public interface ICustomerRepository
    {
        public IEnumerable<Customer> GetAllCustomers();
        Task<bool> AddCustomerRange(List<string> customers);
        IEnumerable<CustomerModel> GetCustomer();
        Task<List<string>> GetCustomerNamesByUserId(int id);
        Task<List<Customer>> FindCustomerName(int Id);
    }
}
