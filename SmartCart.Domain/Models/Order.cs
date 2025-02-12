using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Domain.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public decimal OrderPrice { get; set; }
        public string OrderNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public decimal OrderDiscount { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
