using Microsoft.AspNetCore.Mvc;
using RedisExample.RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExample.RedisExchangeAPI.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _database;
        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _database = redisService.GetDb(0);
        }
        public IActionResult Index()
        {

            _database.StringSet("name", "İhsan Emir Kuzucu");
            _database.StringSet("guest", "100");

            _database.StringIncrement("guest", 1);
            return View();
        }

        public IActionResult Show()
        {
            var value = _database.StringGet("name");
            if (value.HasValue)
            {
                ViewBag.value = value.ToString();
            }
            _database.StringIncrement("guest", 1);

            return View();
        }
    }
}
