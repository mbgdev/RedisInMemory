using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IDistributedCache _distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new();
            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(30);

            Product product = new Product { Id = 1, Name = "Kalem1", Price = 1000 };

            string jsonProduct = JsonConvert.SerializeObject(product);

            await _distributedCache.SetStringAsync("product:1", jsonProduct, cacheEntryOptions);

            return View();
        }

        public async Task<IActionResult> Show()
        {
            string JsonProduct = await _distributedCache.GetStringAsync("product:1");
            Product p = JsonConvert.DeserializeObject<Product>(JsonProduct);

            ViewBag.name = p.Name;

            return View();
        }


        public IActionResult Delete()
        {
            _distributedCache.Remove("name");
            return View();
        }

        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/1.png");
            byte[] imageByte = System.IO.File.ReadAllBytes(path);
            _distributedCache.Set("Resim", imageByte);
            return View();
        }



        public async Task<IActionResult> ImageUrl()
        {
            byte[] resimByte = _distributedCache.Get("Resim");
            return File(resimByte, "image/png");
        }
    }
}
