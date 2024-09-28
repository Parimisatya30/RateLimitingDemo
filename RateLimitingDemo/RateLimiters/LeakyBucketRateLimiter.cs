using System;
using System.Collections.Concurrent;

namespace RateLimitingDemo.RateLimiters
{
    public interface ILeakyBucketRateLimiter
    {
        bool IsRequestAllowed(string clientId);
    }

    public class LeakyBucketRateLimiter : ILeakyBucketRateLimiter
    {
        private readonly int _capacity;
        private readonly TimeSpan _leakRate;
        private readonly ConcurrentDictionary<string, (DateTime LastRequestTime, int WaterLevel)> _clients;

        public LeakyBucketRateLimiter(int capacity = 10, int leakRateSeconds = 1)
        {
            _capacity = capacity;
            _leakRate = TimeSpan.FromSeconds(leakRateSeconds);
            _clients = new ConcurrentDictionary<string, (DateTime, int)>();
        }

        public bool IsRequestAllowed(string clientId)
        {
            var now = DateTime.UtcNow;
            var clientData = _clients.GetOrAdd(clientId, (now, _capacity));

            // Leak the bucket
            var elapsed = now - clientData.LastRequestTime;
            var leakedWater = (int)(elapsed.TotalSeconds / _leakRate.TotalSeconds);
            clientData.WaterLevel = Math.Min(clientData.WaterLevel + leakedWater, _capacity);

            if (clientData.WaterLevel > 0)
            {
                clientData.WaterLevel--;
                clientData.LastRequestTime = now;
                _clients[clientId] = clientData; // Update client data
                return true;
            }

            return false; // Rate limit exceeded
        }
    }
}
