using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Domain.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductCode { get; set; }
        public decimal ProductWeight { get; set; }
        public int Quantity { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductImage { get; set; }
        public string ProductDescription { get; set; }
        public bool IsAvaiable { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public bool IsOffer { get; set; } = false;
        public decimal OfferPercentage { get; set; } = 0m;
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
