using System;
using System.Collections.Concurrent;

namespace RateLimitingDemo.RateLimiters
{
    public interface ISlidingWindowRateLimiter
    {
        bool IsRequestAllowed(string clientId);
    }

    public class SlidingWindowRateLimiter : ISlidingWindowRateLimiter
    {
        private readonly int _limit;
        private readonly TimeSpan _window;
        private readonly ConcurrentDictionary<string, (DateTime[] Timestamps, int CurrentCount)> _clients;

        public SlidingWindowRateLimiter(int limit = 5, int windowSeconds = 60)
        {
            _limit = limit;
            _window = TimeSpan.FromSeconds(windowSeconds);
            _clients = new ConcurrentDictionary<string, (DateTime[], int)>();
        }

        public bool IsRequestAllowed(string clientId)
        {
            var now = DateTime.UtcNow;
            var clientData = _clients.GetOrAdd(clientId, (new DateTime[_limit], 0));

            // Remove old timestamps
            for (int i = 0; i < clientData.CurrentCount; i++)
            {
                if (now - clientData.Timestamps[i] > _window)
                {
                    clientData.Timestamps[i] = DateTime.MinValue;
                }
            }

            // Count valid timestamps
            int count = 0;
            foreach (var timestamp in clientData.Timestamps)
            {
                if (timestamp != DateTime.MinValue) count++;
            }

            if (count < _limit)
            {
                // Add new timestamp
                clientData.Timestamps[count] = now;
                clientData.CurrentCount++;
                _clients[clientId] = clientData; // Update client data
                return true;
            }

            return false; // Rate limit exceeded
        }
    }
}
