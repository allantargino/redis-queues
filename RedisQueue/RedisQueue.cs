using Newtonsoft.Json;
using StackExchange.Redis;
using System.Linq;
using System.Threading.Tasks;

namespace RedisQueue
{
    public class RedisQueue<T> : IQueue<T>
    {
        private readonly ConnectionMultiplexer redis;
        private readonly IDatabase database;
        private readonly string endpoint;
        private readonly string queueName;

        public RedisQueue(string queueName = "defaultQueue", string endpoint = "localhost")
        {
            this.queueName = queueName;
            this.endpoint = endpoint;
            this.redis = ConnectionMultiplexer.Connect(endpoint);
            this.database = redis.GetDatabase();
        }

        public long Count => database.ListLength(queueName);

        public async Task ClearAsync()
        {
            await redis.GetServer(endpoint).FlushDatabaseAsync(database.Database);
        }

        public async Task<T> DequeueAsync()
        {
            var value = await database.ListLeftPopAsync(queueName);
            return JsonConvert.DeserializeObject<T>(value);
        }

        public async Task EnqueueAsync(T obj)
        {
            var value = JsonConvert.SerializeObject(obj);
            await database.ListLeftPushAsync(queueName, value);
        }

        public async Task<T> PeekAsync()
        {
            var range = await database.ListRangeAsync(queueName, 0, 0);
            var value = range.FirstOrDefault().ToString();
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}
