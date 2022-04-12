using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterGenerator.Data.Entity
{
    [Table("CustomerMap")]
    public class CustomerMap
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CustomerId { get; set; }
    }
}
