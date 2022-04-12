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
    public class DealDetailsRepository: IDealDetailsRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public DealDetailsRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public IEnumerable<string> GetAllCustomerMap()
        {
            return _context.DealDetails.Select(x=>x.CustomerName).Distinct().AsQueryable();
        }       
        public async Task<bool> AddDealDetailsRange(List<DealDetails> dealDetails)
        {
            try
            {
                await _context.DealDetails.AddRangeAsync(dealDetails);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
