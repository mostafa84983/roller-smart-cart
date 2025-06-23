using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Application.Dto.Product
{
    public class CreateProductDto
    {
        public string ProductName { get; set; }
        public int ProductCode { get; set; }
        public decimal ProductWeight { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductImage { get; set; }
        public string ProductDescription { get; set; }
        /*public bool IsAvaiable { get; set; } = true;*/
        public int CategoryId { get; set; }
    }
}
