using Microsoft.AspNetCore.Mvc;

namespace RateLimitingDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RateLimiterController : ControllerBase
    {
        /// <summary>
        /// Fixed Window Rate Limiting.
        /// </summary>
        /// <remarks>
        /// This endpoint uses the Fixed Window algorithm for rate limiting.
        /// The limit is set to a maximum of 10 requests per minute.
        /// </remarks>
        /// <response code="200">Request was successful.</response>
        /// <response code="429">Too many requests. Try again later.</response>
        [HttpGet("fixed-window")]
        public IActionResult FixedWindowEndpoint()
        {
            return Ok("Fixed Window Rate Limiting endpoint.");
        }

        /// <summary>
        /// Token Bucket Rate Limiting.
        /// </summary>
        /// <remarks>
        /// This endpoint uses the Token Bucket algorithm for rate limiting.
        /// The bucket has a maximum of 20 tokens and refills at a rate of 1 token per second.
        /// </remarks>
        /// <response code="200">Request was successful.</response>
        /// <response code="429">Too many requests. Try again later.</response>
        [HttpGet("token-bucket")]
        public IActionResult TokenBucketEndpoint()
        {
            return Ok("Token Bucket Rate Limiting endpoint.");
        }

        /// <summary>
        /// Leaky Bucket Rate Limiting.
        /// </summary>
        /// <remarks>
        /// This endpoint uses the Leaky Bucket algorithm for rate limiting.
        /// The bucket processes 1 request per second, and any overflow is throttled.
        /// </remarks>
        /// <response code="200">Request was successful.</response>
        /// <response code="429">Too many requests. Try again later.</response>
        [HttpGet("leaky-bucket")]
        public IActionResult LeakyBucketEndpoint()
        {
            return Ok("Leaky Bucket Rate Limiting endpoint.");
        }

        /// <summary>
        /// Sliding Log Rate Limiting.
        /// </summary>
        /// <remarks>
        /// This endpoint uses the Sliding Log algorithm for rate limiting.
        /// A user is allowed 15 requests per minute.
        /// </remarks>
        /// <response code="200">Request was successful.</response>
        /// <response code="429">Too many requests. Try again later.</response>
        [HttpGet("sliding-log")]
        public IActionResult SlidingLogEndpoint()
        {
            return Ok("Sliding Log Rate Limiting endpoint.");
        }

        /// <summary>
        /// Sliding Window Rate Limiting.
        /// </summary>
        /// <remarks>
        /// This endpoint uses the Sliding Window algorithm for rate limiting.
        /// The rate limit is set to 50 requests per 5 minutes.
        /// </remarks>
        /// <response code="200">Request was successful.</response>
        /// <response code="429">Too many requests. Try again later.</response>
        [HttpGet("sliding-window")]
        public IActionResult SlidingWindowEndpoint()
        {
            return Ok("Sliding Window Rate Limiting endpoint.");
        }
    }
}
