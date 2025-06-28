using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Application.Dto.Payment
{
    public class CreateStripeSessionDto
    {
        public int OrderId { get; set; }
        public string Email { get; set; }
        public List<StripeProductDto> Products { get; set; }
    }
}
