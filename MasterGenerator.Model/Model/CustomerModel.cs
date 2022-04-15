using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterGenerator.Model.Model
{
    public class CustomerModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CustomerId { get; set; }
        public string? UserName { get; set; }
        public string? CustomerName { get; set; }
    }
}
