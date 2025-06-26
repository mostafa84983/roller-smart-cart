using SmartCart.Domain.Interfaces;
using SmartCart.Domain.Models;
using SmartCart.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Infrastructure.Repositories
{
    public class OrderProductRepository : GenericRepository<OrderProduct> , IOrderProductRepository
    {
        public OrderProductRepository(DataContext context): base(context)
        {

        }

    }
}
