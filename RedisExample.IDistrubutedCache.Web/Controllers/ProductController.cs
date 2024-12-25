using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace RedisExample.IDistrubutedCache.Web.Controllers
{
    public class ProductController(IDistributedCache distrubutedCache) : Controller
    {
        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions distributedCacheEntryOptions = new();
            distributedCacheEntryOptions.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(2);

            #region string
            //distrubutedCache.SetString("name", "emir", distributedCacheEntryOptions);
            //await distrubutedCache.SetStringAsync("surname", "kuzucu");
            #endregion

            #region class
            //Product product = new() { Id = 1, Name = "Kalem 1", Price = 100 };
            //string jsonProduct = JsonConvert.SerializeObject(product);
            //await distrubutedCache.SetStringAsync("product:1", jsonProduct, distributedCacheEntryOptions);
            #endregion


            return View();
        }

        public async Task<IActionResult> Show()
        {
            #region string
            //string name = distrubutedCache.GetString("name");
            //string surname = await distrubutedCache.GetStringAsync("name");
            //ViewBag.name = name;
            //ViewBag.surname = surname;
            #endregion

            #region product
            //string jsonProduct = distrubutedCache.GetString("product:1");
            //Product product = JsonConvert.DeserializeObject<Product>(jsonProduct);
            //ViewBag.product = product;
            #endregion


            return View();
        }

        public IActionResult Remove()
        {
            #region string
            //distrubutedCache.Remove("name");
            #endregion

            return View();
        }

        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/sf.jpg");
            byte[] imageByte = System.IO.File.ReadAllBytes(path);
            distrubutedCache.Set("image",imageByte);
            return View();
        }

        public IActionResult ImageUrl()
        {
            byte[] resimByte = distrubutedCache.Get("image");
            return File(resimByte, "image/jpg");
        }
    }
}
