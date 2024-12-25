using StackExchange.Redis;

namespace RedisExample.RedisExchangeAPI.Web.Services
{
    public class RedisService
    {
        private readonly string _redisHost;
        private readonly string _redisPort;
        private ConnectionMultiplexer _connectionMultiplexer;
        public IDatabase Database { get; set; }

        public RedisService(IConfiguration configuration)
        {
            _redisHost = configuration["Redis:Host"]!;
            _redisPort = configuration["Redis:Port"]!;
        }

        public void Connect()
        {
            var configString = $"{_redisHost}:{_redisPort}";
            _connectionMultiplexer=ConnectionMultiplexer.Connect(configString);
        }

        public IDatabase GetDb(int db)
        {
            return _connectionMultiplexer.GetDatabase(db);
        }
    }
}
