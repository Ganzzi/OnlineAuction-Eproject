using DomainLayer.Entities.Models;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Interface
{
    public class RedisService
    {
        private readonly IDistributedCache? _cache;

        public RedisService(IDistributedCache cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task SetCachedData<T>(string key, T data, TimeSpan cacheDuration)
        {
            try
            {
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = cacheDuration
                };

                var jsonOptions = GetDefaultJsonSerializerOptions();
                var jsonData = JsonSerializer.Serialize(data, jsonOptions);

                await _cache.SetStringAsync(key, jsonData, options);
            }
            catch (Exception ex)
            {
                LogError($"Error caching data for key '{key}': {ex.Message}");
            }
        }

        public async Task<T> GetCachedData<T>(string key)
        {
            try
            {
                var jsonData = await _cache.GetStringAsync(key);

                if (string.IsNullOrEmpty(jsonData))
                {
                    return default(T);
                }

                var jsonOptions = GetDefaultJsonSerializerOptions();
                return JsonSerializer.Deserialize<T>(jsonData, jsonOptions);
            }
            catch (Exception ex)
            {
                LogError($"Error retrieving cached data for key '{key}': {ex.Message}");
                return default;
            }
        }

        private JsonSerializerOptions GetDefaultJsonSerializerOptions()
        {
            return new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                MaxDepth = 64 // Set the maximum depth as needed
            };
        }

        private void LogError(string message)
        {
            // Log the error using your preferred logging mechanism
            Console.WriteLine($"Error: {message}");
            // You might want to log to a dedicated logging framework like Serilog or NLog in a real-world application.
        }
    }

}