using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using RateLimitingDemo.RateLimiters;
using System.Threading.RateLimiting;

namespace RateLimitingDemo.Middleware
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IFixedWindowRateLimiter _fixedWindowLimiter;
        private readonly ITokenBucketRateLimiter _tokenBucketLimiter;
        private readonly ILeakyBucketRateLimiter _leakyBucketLimiter;
        private readonly ISlidingLogRateLimiter _slidingLogLimiter;
        private readonly ISlidingWindowRateLimiter _slidingWindowLimiter;

        public RateLimitingMiddleware(
        RequestDelegate next,
            IFixedWindowRateLimiter fixedWindowLimiter,
            ITokenBucketRateLimiter tokenBucketLimiter,
            ILeakyBucketRateLimiter leakyBucketLimiter,
            ISlidingLogRateLimiter slidingLogLimiter,
            ISlidingWindowRateLimiter slidingWindowLimiter)
        {
            _next = next;
            _fixedWindowLimiter = fixedWindowLimiter;
            _tokenBucketLimiter = tokenBucketLimiter;
            _leakyBucketLimiter = leakyBucketLimiter;
            _slidingLogLimiter = slidingLogLimiter;
            _slidingWindowLimiter = slidingWindowLimiter;
        }

        public async Task Invoke(HttpContext context)
        {
            var clientId = context.Connection.RemoteIpAddress.ToString(); // Simple client identification

            var path = context.Request.Path.Value?.ToLowerInvariant();

            switch (path)
            {
                case "/api/ratelimit/fixedwindow":
                    if (!_fixedWindowLimiter.IsRequestAllowed(clientId))
                    {
                        context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                        await context.Response.WriteAsync("Too many requests for Fixed Window. Try again later.");
                        return;
                    }
                    break;

                case "/api/ratelimit/tokenbucket":
                    if (!_tokenBucketLimiter.IsRequestAllowed(clientId))
                    {
                        context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                        await context.Response.WriteAsync("Too many requests for Token Bucket. Try again later.");
                        return;
                    }
                    break;

                case "/api/ratelimit/leakybucket":
                    if (!_leakyBucketLimiter.IsRequestAllowed(clientId))
                    {
                        context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                        await context.Response.WriteAsync("Too many requests for Leaky Bucket. Try again later.");
                        return;
                    }
                    break;

                case "/api/ratelimit/slidinglog":
                    if (!_slidingLogLimiter.IsRequestAllowed(clientId))
                    {
                        context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                        await context.Response.WriteAsync("Too many requests for Sliding Log. Try again later.");
                        return;
                    }
                    break;

                case "/api/ratelimit/slidingwindow":
                    if (!_slidingWindowLimiter.IsRequestAllowed(clientId))
                    {
                        context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                        await context.Response.WriteAsync("Too many requests for Sliding Window. Try again later.");
                        return;
                    }
                    break;

                default:
                    break;
            }

            await _next(context);
        }
    }
}
