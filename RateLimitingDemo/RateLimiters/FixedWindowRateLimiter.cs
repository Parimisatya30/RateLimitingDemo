using System;
using System.Collections.Concurrent;

namespace RateLimitingDemo.RateLimiters
{
    public interface IFixedWindowRateLimiter
    {
        bool IsRequestAllowed(string clientId);
    }

    public class FixedWindowRateLimiter : IFixedWindowRateLimiter
    {
        private readonly int _limit;
        private readonly TimeSpan _window;
        private readonly ConcurrentDictionary<string, (DateTime LastRequestTime, int RequestCount)> _clients;

        public FixedWindowRateLimiter(int limit = 5, int windowSeconds = 60)
        {
            _limit = limit;
            _window = TimeSpan.FromSeconds(windowSeconds);
            _clients = new ConcurrentDictionary<string, (DateTime, int)>();
        }

        public bool IsRequestAllowed(string clientId)
        {
            var now = DateTime.UtcNow;
            var clientData = _clients.GetOrAdd(clientId, (now, 0));

            if (now - clientData.LastRequestTime > _window)
            {
                clientData = (now, 0); // Reset count after the window
            }

            if (clientData.RequestCount < _limit)
            {
                clientData.RequestCount++;
                _clients[clientId] = clientData; // Update client data
                return true;
            }

            return false; // Rate limit exceeded
        }
    }
}
