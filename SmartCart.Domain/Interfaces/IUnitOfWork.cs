using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Category { get; }
        IOrderRepository Order { get; }
        IProductRepository Product { get; }
        IUserRepository User { get; }
        IOrderProductRepository OrderProduct { get; }
        ICartSessionService CartService { get; }

        int Save();
    }
}
