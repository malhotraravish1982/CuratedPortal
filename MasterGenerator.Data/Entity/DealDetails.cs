using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterGenerator.Data.Entity
{
    public class DealDetails
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Project")]
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
        public virtual Project Project { get; set; }
    }
}
