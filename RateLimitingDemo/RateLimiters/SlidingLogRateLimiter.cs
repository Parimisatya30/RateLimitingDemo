using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace RateLimitingDemo.RateLimiters
{
    public interface ISlidingLogRateLimiter
    {
        bool IsRequestAllowed(string clientId);
    }

    public class SlidingLogRateLimiter : ISlidingLogRateLimiter
    {
        private readonly int _limit;
        private readonly TimeSpan _window;
        private readonly ConcurrentDictionary<string, LinkedList<DateTime>> _clients;

        public SlidingLogRateLimiter(int limit = 5, int windowSeconds = 60)
        {
            _limit = limit;
            _window = TimeSpan.FromSeconds(windowSeconds);
            _clients = new ConcurrentDictionary<string, LinkedList<DateTime>>();
        }

        public bool IsRequestAllowed(string clientId)
        {
            var now = DateTime.UtcNow;
            var clientRequests = _clients.GetOrAdd(clientId, new LinkedList<DateTime>());

            // Remove old requests
            while (clientRequests.Count > 0 && now - clientRequests.First.Value > _window)
            {
                clientRequests.RemoveFirst();
            }

            if (clientRequests.Count < _limit)
            {
                clientRequests.AddLast(now);
                return true;
            }

            return false; // Rate limit exceeded
        }
    }
}
