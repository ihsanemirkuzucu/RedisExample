using Microsoft.AspNetCore.Mvc;
using RedisExample.RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExample.RedisExchangeAPI.Web.Controllers
{
    public class SortedSetTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _database;
        private string _listKey = "sortedsetnames";

        public SortedSetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _database = redisService.GetDb(3);
        }

        public IActionResult Index()
        {
            HashSet<string> list = new();
            if (_database.KeyExists(_listKey))
            {
                //_database.SortedSetScan(_listKey).ToList().ForEach(x =>
                //{
                //    list.Add(x.ToString());
                //});
                _database.SortedSetRangeByRank(_listKey,order:Order.Descending).ToList().ForEach(x =>
                {
                    list.Add(x.ToString());
                });
            }
            return View(list);
        }

        [HttpPost]
        public IActionResult Add(string name,int score)
        {
            _database.SortedSetAdd(_listKey, name, score);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(string name)
        {
            _database.SortedSetRemove(_listKey, name);
            return RedirectToAction(nameof(Index));
        }
    }
}
