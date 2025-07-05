/*using SmartCart.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Application.Interfaces
{
    public interface IPaymentVerificationService
    {
        Task<GenericResult<(string Status, int OrderId)>> GetPaymentStatus(string sessionId);
        Task<Result> HandleCheckoutSessionCompleted(string sessionId);
    }
}
*/