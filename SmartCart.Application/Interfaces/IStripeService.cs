using SmartCart.Application.Dto.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Application.Interfaces
{
    public interface IStripeService
    {
        Task<string> CreateCheckoutSessionAsync(CreateStripeSessionDto dto);
    }
}
