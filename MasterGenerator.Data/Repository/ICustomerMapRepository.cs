using MasterGenerator.Data.Entity;
using MasterGenerator.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterGenerator.Data.Repository
{
    public interface ICustomerMapRepository
    {
        Task AddCustomerMap(CustomerMap customerMap);
        IEnumerable<CustomerModel> GetCutomerMaped();
        Task<CustomerMap> GetCustomerMappingById(int Id);
        //List<string> GetMappingRecordById(CustomerModel customerModel);
        CustomerModel GetMappingRecordById(CustomerModel customerModel);
        void DeleteCustomerMapping(CustomerMap customerMap);
    }
}
 