using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;

        private readonly IDatabase db;

        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(0);
        }

        public async Task<IActionResult> Index()
        {
          

           await db.StringSetAsync("name", "Burak");
           await db.StringSetAsync("ziyaretçi", 100);

            return View();
        }


        public async Task<IActionResult> Show()
        { 

            var value=await db.StringGetAsync("name");
            var value1=await db.StringIncrementAsync("ziyaretçi",1);
            
            if(value.HasValue)
            {
                ViewBag.v = value;
                ViewBag.v1 = value1;
            }

            return View();
        }




        }
}
