using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Application.Dto.Category
{
    public class UpdateCategoryDto
    {
        [Required(ErrorMessage = "Category ID is required")]
        public int CategoryId { get; set; }
        public IFormFile? CategoryImage { get; set; }
        public string? CategoryName { get; set; }
    }
}
