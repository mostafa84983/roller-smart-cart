using SmartCart.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Application.Dto
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public decimal OrderPrice { get; set; }
        public string OrderNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public decimal OrderDiscount { get; set; } = 0m;
        public int UserId { get; set; }
        //public OrderStatus Status { get; set; }
    }
}
