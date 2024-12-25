using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RedisExample.InMemory.Web.Models;

namespace RedisExample.InMemory.Web.Controllers
{
    public class ProductController(IMemoryCache memoryCache) : Controller
    {
        public IActionResult Index()
        {
            Product p = new() { Id = 1, Name = "Kalem", Price = 200 };
            memoryCache.Set<Product>("Product1", p);

            if (!memoryCache.TryGetValue("zaman", out string zamanCache))
            {
                MemoryCacheEntryOptions options = new();
                options.Priority = CacheItemPriority.High;
                options.RegisterPostEvictionCallback((key, value, reason, state) =>
                {
                    memoryCache.Set("callback", $"{key}->{value} => sebep:{reason}");
                });
                //options.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(30);
                options.SlidingExpiration= TimeSpan.FromSeconds(10);
                memoryCache.Set<string>("zaman", DateTime.Now.ToString(),options);
            }
            return View();
        }

        public IActionResult Show()
        {
            memoryCache.TryGetValue("zaman", out string zamanCache);
            memoryCache.TryGetValue("callback", out string callback);
            memoryCache.TryGetValue("Product1", out Product product);


            ViewBag.zaman = zamanCache;
            ViewBag.callback = callback;
            ViewBag.product = product;
            //memoryCache.GetOrCreate<string>("zaman", entry =>
            //{
            //    return DateTime.Now.ToString();
            //});
            // memoryCache.Remove("zaman");
            return View();
        }
    }
}
