using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {

            MemoryCacheEntryOptions options = new();

            options.AbsoluteExpiration = DateTime.Now.AddSeconds(5);
            //options.SlidingExpiration = TimeSpan.FromSeconds(10);
            options.Priority = CacheItemPriority.Normal;
            options.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                _memoryCache.Set("callback",$"{key}:{value} => sebeb {reason}");
            });
            _memoryCache.Set<string>("zaman", DateTime.Now.ToString(), options);

            return View();
        }

        public IActionResult Show()
        {

            _memoryCache.TryGetValue("zaman", out string zamancache);

            _memoryCache.TryGetValue("callback", out string callback);

            ViewBag.Zaman = zamancache;
            ViewBag.Callback = callback;
            return View();
        }
    }
}
