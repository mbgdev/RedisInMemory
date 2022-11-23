using RedisExampleApp.API.Models;
using RedisExampleApp.API.Services;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisExampleApp.API.Repositories
{
    public class ProductRepositoryWithCache : IProductRepository
    {
        private const string productKey = "productCaches";

        private readonly IProductRepository _productRepository;

        private readonly RedisService _redisService;

        private readonly IDatabase _cacheRepository;

        public ProductRepositoryWithCache(IProductRepository productRepository, RedisService redisService)
        {
            _productRepository = productRepository;
            _redisService = redisService;
            _cacheRepository = redisService.GetDb(0);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            var newProduct = await _productRepository.CreateAsync(product);

            if (await _cacheRepository.KeyExistsAsync(productKey))
            {
                await _cacheRepository.HashSetAsync(productKey, product.Id, JsonSerializer.Serialize(newProduct));
            }

            return newProduct;
        }

        public async Task<List<Product>> GetAsync()
        {
            if (!await _cacheRepository.KeyExistsAsync(productKey))
            {
                return await LoadToCacheFromDbAsync();
            }
            var productList = new List<Product>();

            var cacheProduct = (await _cacheRepository.HashGetAllAsync(productKey)).ToList();
            foreach (var item in cacheProduct)
            {
                var product = JsonSerializer.Deserialize<Product>(item.Value);
                productList.Add(product);
            }

            return productList;

        }

        public async Task<Product> GetByIdAsync(int id)
        {
            if (await _cacheRepository.KeyExistsAsync(productKey))
            {
                var product = await _cacheRepository.HashGetAsync(productKey, id);
                return product.HasValue ? JsonSerializer.Deserialize<Product>(product) : null;
            }


            var products = await LoadToCacheFromDbAsync();

            return products.FirstOrDefault(x => x.Id == id);

        }

        private async Task<List<Product>> LoadToCacheFromDbAsync()
        {
            var product = await _productRepository.GetAsync();

            product.ForEach(p =>
            {
                _cacheRepository.HashSetAsync(productKey, p.Id, JsonSerializer.Serialize(p));
            });



            return product;
        }





    }
}
