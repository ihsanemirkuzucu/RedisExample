using Microsoft.AspNetCore.Mvc;
using RedisExample.RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExample.RedisExchangeAPI.Web.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _database;
        private string _listKey = "names";

        public ListTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _database = redisService.GetDb(1);
        }

        public IActionResult Index()
        {
            List<string> namesList = new();
            if (_database.KeyExists(_listKey))
            {
                _database.ListRange(_listKey).ToList().ForEach(x =>
                {
                    namesList.Add(x.ToString());
                });
            }
            return View(namesList);
        }

        [HttpPost]
        public IActionResult Add(string name)
        {
            _database.ListRightPush(_listKey, name);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(string name)
        {
            _database.ListRemoveAsync(_listKey, name);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult LeftPop()
        {
            _database.ListLeftPop(_listKey);
            return RedirectToAction(nameof(Index));
        }
    }
}
