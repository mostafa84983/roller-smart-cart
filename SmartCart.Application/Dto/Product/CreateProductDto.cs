using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Application.Dto.Product
{
    public class CreateProductDto
    {
        [Required(ErrorMessage = "Product name is required")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Product code is required")]
        public int ProductCode { get; set; }

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "Weight must be greater than 0")]
        public decimal ProductWeight { get; set; }

        [Range(0.01, 999999, ErrorMessage = "Price must be greater than 0")]
        public decimal ProductPrice { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Product image is required")]
        public IFormFile ProductImage { get; set; }
        public string ProductDescription { get; set; }

        [Required(ErrorMessage = "Category ID is required")]
        public int CategoryId { get; set; }
    }
}
