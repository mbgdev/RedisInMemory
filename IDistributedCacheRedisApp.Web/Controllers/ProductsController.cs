using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IDistributedCache _distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<IActionResult> IndexAsync()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new();
            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
            _distributedCache.SetString("name", "Burak", cacheEntryOptions);
           await _distributedCache.SetStringAsync("surname", "Gürener", cacheEntryOptions);
            return View();
        }

        public IActionResult Show()
        {
            string name=_distributedCache.GetString("name");
            return View();
        }


        public IActionResult Delete()
        {
            _distributedCache.Remove("name");   
            return View();  
        }
    }
}
