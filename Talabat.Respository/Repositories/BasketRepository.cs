using StackExchange.Redis;
using System.Text.Json;
using Talabat.Core.Entities;
using Talabat.Core.IRepositories;

namespace Talabat.Respository.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _redisDb;

        public BasketRepository(IConnectionMultiplexer redis)
        {
            _redisDb = redis.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsync(string id)
        {
            return await _redisDb.KeyDeleteAsync(id);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string id)
        {
            var basket = await _redisDb.StringGetAsync(id);
            return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(basket);
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
            var isUpdatedOrCreated = await _redisDb.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));
            return isUpdatedOrCreated ? await GetBasketAsync(basket.Id) : null;
        }
    }
}
