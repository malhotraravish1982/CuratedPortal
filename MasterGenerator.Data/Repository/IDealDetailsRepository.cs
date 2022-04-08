using MasterGenerator.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterGenerator.Data.Repository
{
    public interface IDealDetailsRepository
    {
        Task<bool> AddDealDetailsRange(List<DealDetails> dealDetails);
    }
}
