using MasterGenerator.Data.Entity;
using MasterGenerator.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterGenerator.Data.Repository
{
    public interface IDealDetailsRepository
    {
        IEnumerable<string> GetAllCustomers();
        Task<bool> AddDealDetailsRange(List<DealDetails> dealDetails);
        IEnumerable<DealDetailsModel> GetDealDetails();
    }
}
