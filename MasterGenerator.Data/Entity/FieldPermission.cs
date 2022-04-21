using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterGenerator.Data.Entity
{
    [Table("FieldPermissions")]
    public class FieldPermission
    {
        [Key]
        public int Id { get; set; } 
        public int UserId { get; set; }
        public string? ProjectId { get; set; }
        public string CustomerName { get; set; }
        public string ProjectName { get; set; }
        public string PONumber { get; set; }
        public string PODate { get; set; }
        public string TotalQuantity { get; set; }
        public string Currency { get; set; }
        public string POAmount { get; set; }
        public string CSStatus { get; set; }
        public string DisplayStatus { get; set; }
        public string ShipmentMethod { get; set; }
        public string ProductionCompletion { get; set; }
        public string VesselETA { get; set; }
        public string VesselETD { get; set; }
        public string EstimatedDeliveryDate { get; set; }
        public string ActualDeliveryDate { get; set; }
        public string PreProductionManager { get; set; }
        public string OnTimeStatus { get; set; }

    }
}
