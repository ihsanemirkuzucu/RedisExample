using Microsoft.AspNetCore.Mvc;
using RedisExample.RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExample.RedisExchangeAPI.Web.Controllers
{
    public class HashTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _database;
        private string _listKey = "sözlük";

        public HashTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _database = redisService.GetDb(4);
        }
        public IActionResult Index()
        {
            Dictionary<string,string> list = new();
            if (_database.KeyExists(_listKey))
            {
                _database.HashGetAll(_listKey).ToList().ForEach(x =>
                {
                    list.Add(x.Name,x.Value);
                });
            }
            return View(list);
        }

        [HttpPost]
        public IActionResult Add(string name, string value)
        {
            _database.HashSet(_listKey, name, value);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(string name)
        {
            _database.HashDelete(_listKey, name);
            return RedirectToAction(nameof(Index));
        }
    }
}
