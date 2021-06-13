using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApp.Strategy.Models;

namespace WebApp.Strategy.Repositories
{
    public class ProductRepositorySqlServer : IProductRepository
    {
        private readonly AppIdentityDbContext _context;

        public ProductRepositorySqlServer(AppIdentityDbContext context)
        {
            _context = context;
        }

        public async Task<Product> GetById(string id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> Save(Product product)
        {
            product.Id = Guid.NewGuid().ToString();
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task Update(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAllByUserId(string userId)
        {
            return await _context.Products.Where(s => s.UserId == userId).ToListAsync();
        }
    }
}
