using StackExchange.Redis;

namespace RedisExampleApp.API.Services
{
    public class RedisService
    {
        private ConnectionMultiplexer _connectionMultiplexer;


        public RedisService(string url)
        {
            _connectionMultiplexer = ConnectionMultiplexer.Connect(url);
        }


        public IDatabase GetDb(int dbIndex)
        {
            return _connectionMultiplexer.GetDatabase(dbIndex);
        }

    }
}
