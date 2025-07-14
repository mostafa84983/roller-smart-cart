using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Application.Dto.Product
{
    public class ProductAddOrRemoveDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductCode { get; set; }
        public decimal ProductWeight { get; set; }
        public int Quantity { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductImage { get; set; }
        public string ProductDescription { get; set; }
        public bool IsAvaiable { get; set; }
        public bool IsOffer { get; set; }
        public decimal OfferPercentage { get; set; }
        public int CategoryId { get; set; }
        public int OrderId { get; set; }
    }
}
