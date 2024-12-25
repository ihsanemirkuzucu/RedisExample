using Microsoft.AspNetCore.Mvc;
using RedisExample.RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExample.RedisExchangeAPI.Web.Controllers
{
    public class SetTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _database;
        private string _listKey = "setnames";

        public SetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _database = redisService.GetDb(2);
        }

        public IActionResult Index()
        {
            HashSet<string> namesList = new();
            if (_database.KeyExists(_listKey))
            {
                _database.SetMembers(_listKey).ToList().ForEach(x =>
                {
                    namesList.Add(x.ToString());
                });
            }
            return View(namesList);
        }

        [HttpPost]
        public IActionResult Add(string name)
        {
            _database.KeyExpire(_listKey, DateTime.Now.AddMinutes(5));
            _database.SetAdd(_listKey, name);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(string name)
        {
            _database.SetRemove(_listKey, name);
            return RedirectToAction(nameof(Index));
        }
    }
}
