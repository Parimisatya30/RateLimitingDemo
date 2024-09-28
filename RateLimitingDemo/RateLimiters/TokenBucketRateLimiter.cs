using System;
using System.Collections.Concurrent;

namespace RateLimitingDemo.RateLimiters
{
    public interface ITokenBucketRateLimiter
    {
        bool IsRequestAllowed(string clientId);
    }

    public class TokenBucketRateLimiter : ITokenBucketRateLimiter
    {
        private readonly int _capacity;
        private readonly int _tokensPerInterval;
        private readonly TimeSpan _interval;
        private readonly ConcurrentDictionary<string, (int Tokens, DateTime LastRefillTime)> _clients;

        public TokenBucketRateLimiter(int capacity = 10, int tokensPerInterval = 5, int intervalSeconds = 1)
        {
            _capacity = capacity;
            _tokensPerInterval = tokensPerInterval;
            _interval = TimeSpan.FromSeconds(intervalSeconds);
            _clients = new ConcurrentDictionary<string, (int, DateTime)>();
        }

        public bool IsRequestAllowed(string clientId)
        {
            var now = DateTime.UtcNow;
            var clientData = _clients.GetOrAdd(clientId, (_capacity, now));

            if (now - clientData.LastRefillTime > _interval)
            {
                var newTokens = Math.Min(clientData.Tokens + _tokensPerInterval, _capacity);
                clientData = (newTokens, now);
            }

            if (clientData.Tokens > 0)
            {
                clientData.Tokens--;
                _clients[clientId] = clientData; // Update client data
                return true;
            }

            return false; // Rate limit exceeded
        }
    }
}
