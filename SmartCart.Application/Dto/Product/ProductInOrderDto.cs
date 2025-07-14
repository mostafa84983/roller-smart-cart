using SmartCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Application.Dto.Product
{
    public class ProductInOrderDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductCode { get; set; }
        public decimal ProductWeight { get; set; }
        public int QuantityInOrder { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductImage { get; set; }
        public string ProductDescription { get; set; }
        public int CategoryId { get; set; }
    }
}
