using Microsoft.EntityFrameworkCore;
using SmartCart.Domain.Interfaces;
using SmartCart.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;

        public ICategoryRepository Category { get; }
        public IProductRepository Product { get; }

        public UnitOfWork(DataContext context, ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            _context = context;
            Category = categoryRepository;
            Product = productRepository;
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }
}
