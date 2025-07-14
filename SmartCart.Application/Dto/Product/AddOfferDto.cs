using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Application.Dto.Product
{
    public class AddOfferDto
    {
        [Required]
        public int ProductId { get; set; }

        [Range(0.01, 100, ErrorMessage = "Offer percentage must be between 0.01 and 100")]
        public decimal OfferPercentage { get; set; }

    }
}
