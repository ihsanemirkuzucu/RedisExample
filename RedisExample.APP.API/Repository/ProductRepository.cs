using Microsoft.EntityFrameworkCore;
using RedisExample.APP.API.Model;

namespace RedisExample.APP.API.Repository
{
    public class ProductRepository(AppDbContext context) : IProductRepository
    {
        public async Task<List<Product>> GetAllAsync()
        {
            return await context.Products.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await context.Products.FindAsync(id);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
            return product;
        }
    }
}
