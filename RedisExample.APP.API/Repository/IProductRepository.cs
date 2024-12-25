using RedisExample.APP.API.Model;

namespace RedisExample.APP.API.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task<Product> CreateAsync(Product product);

    }
}
