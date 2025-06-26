using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Application.Dto.Product
{
    public class ProductRequest
    {
        public int ProductCode { get; set; }
        public string CartId { get; set; }
    }
}
