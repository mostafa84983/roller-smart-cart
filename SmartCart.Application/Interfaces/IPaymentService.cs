using SmartCart.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<GenericResult<string>> CreateStripeSession(int orderId, int userId);
    }
}
