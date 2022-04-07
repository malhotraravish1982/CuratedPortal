using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterGenerator.Model.Model
{
    public class DealDetailsModel
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string? CustomerName { get; set; }
        public string? ProjectName { get; set; }
        public string? ItemSKU { get; set; }
        public string? BatchNumber { get; set; }
        public string? CustomerSKU { get; set; }
        public string? ItemName { get; set; }
        public string? Qty { get; set; }
        public string? Currency { get; set; }
        public string? ApproxSellPriceExVAT { get; set; }
    }
}
