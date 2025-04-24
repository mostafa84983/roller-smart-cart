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
        int Save();
    }
}
