using RedisExample.APP.API.Model;
using RedisExample.APP.Cache;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisExample.APP.API.Repository
{
    public class ProductRepositoryWithCache : IProductRepository
    {
        private const string _productKey = "productCaches";
        private readonly IProductRepository _productRepository;
        private readonly RedisService _redisService;
        private readonly IDatabase _database;

        public ProductRepositoryWithCache(IProductRepository productRepository, RedisService redisService)
        {
            _productRepository = productRepository;
            _redisService = redisService;
            _database = _redisService.GetDb(2);
        }


        public async Task<List<Product>> GetAllAsync()
        {
            if (!await _database.KeyExistsAsync(_productKey))
                return await LoadToCacheFromDbAsync();

            var products = new List<Product>();
            var cacheProducts = await _database.HashGetAllAsync(_productKey);
            foreach (var item in cacheProducts.ToList())
            {
                var product = JsonSerializer.Deserialize<Product>(item.Value);
                products.Add(product);
            }

            return products;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            if (!await _database.KeyExistsAsync(_productKey))
            {
                var product = await _database.HashGetAsync(_productKey, id);
                return product.HasValue ? JsonSerializer.Deserialize<Product>(product) : null;
            }

            var products = await LoadToCacheFromDbAsync();
            return products.FirstOrDefault(x => x.Id == id);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            var newProduct = await _productRepository.CreateAsync(product);

            if (await _database.KeyExistsAsync(_productKey))
            {
                await _database.HashSetAsync(_productKey, product.Id, JsonSerializer.Serialize(newProduct));
            }

            return newProduct;
        }

        private async Task<List<Product>> LoadToCacheFromDbAsync()
        {
            var products = await _productRepository.GetAllAsync();
            products.ForEach(p =>
            {
                _database.HashSetAsync(_productKey, p.Id, JsonSerializer.Serialize(p));
            });
            return products;
        }
    }
}
