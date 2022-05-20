﻿



namespace SkillTracker.Services.Admin.API.Services;
public class RedisCacheService : IRedisCacheService
{
    private readonly IDistributedCache _cache;

    public RedisCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }
    public T Get<T>(string key)
    {
        var value = _cache.GetString(key);

        if (value != null)
        {
            return JsonSerializer.Deserialize<T>(value);
        }

        return default;
    }



    public T Set<T>(string key, T value)
    {
        var timeOut = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
            SlidingExpiration = TimeSpan.FromMinutes(2)
        };

        _cache.SetString(key, JsonSerializer.Serialize(value), timeOut);

        return value;
    }


}

