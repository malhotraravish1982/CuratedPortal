using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterGenerator.Model.Model
{
    public class MasterSCSModel
    {      
        public Guid MasterScsId { get; set; }
      
        public string? Ref { get; set; }
       
        public string? ImageRef { get; set; }
       
        public string? Category { get; set; }
    
        public string? SubCategory { get; set; }
       
        public string? ProductName { get; set; }
        public double? Quantity { get; set; }
       
        public string? DisplayUnit { get; set; }
       
        public string? FactoryName { get; set; }
        public double? CartonL { get; set; }
        public double? CartonW { get; set; }
        public double? CartonH { get; set; }
        public double? UnitsPerCarton { get; set; }
        public double? CartonGW { get; set; }
        
        public string? Currency { get; set; }
        public decimal? UnitCostPriceInCurrency { get; set; }
        public decimal? UnitPackagingCasePriceInCurrency { get; set; }
        public decimal? TotalCostPriceInCurrency { get; set; }
       
        public string? ProductDesc { get; set; }
        
        public string? Certification { get; set; }
       
        public string? ProductionLeadTime { get; set; }
        public double? ProductL { get; set; }
        public double? ProductW { get; set; }
        public double? ProductH { get; set; }
        public double? PackagingL { get; set; }
        public double? PackagingW { get; set; }
        public double? PackagingH { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }

    }
}
