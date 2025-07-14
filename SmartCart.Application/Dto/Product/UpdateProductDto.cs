using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Application.Dto.Product
{
    public class UpdateProductDto
    {
        [Required(ErrorMessage = "Product ID is required")]
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int? ProductCode { get; set; }
        public decimal? ProductWeight { get; set; }
        public int? Quantity { get; set; }
        public decimal? ProductPrice { get; set; }
        public IFormFile? ProductImage { get; set; }
        public string? ProductDescription { get; set; }
        public bool? IsAvaiable { get; set; }
    }
}
