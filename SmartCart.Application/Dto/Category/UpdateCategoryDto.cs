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
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Category image is required")]
        public string CategoryImage { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        public string CategoryName { get; set; }
    }
}
