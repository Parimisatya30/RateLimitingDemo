Here’s a detailed explanation of each rate-limiting algorithm, along with a clear configuration for minimum and maximum limits, that can be used to generate a `README.md` file for your API project.

---

# Rate Limiting Algorithms in .NET Core API

This project demonstrates how to implement various rate-limiting algorithms in a .NET Core API, specifically the following algorithms:

- Fixed Window Rate Limiting
- Token Bucket Rate Limiting
- Leaky Bucket Rate Limiting
- Sliding Log Rate Limiting
- Sliding Window Rate Limiting

Each of these algorithms is implemented using middleware in .NET Core, and each API endpoint demonstrates the different rate-limiting strategies. Rate limits are applied to simulate real-world rate-limiting scenarios, allowing for requests from clients to be restricted based on predefined rules.

## Algorithms Overview

### 1. **Fixed Window Rate Limiting**
This algorithm divides time into fixed intervals (windows) and limits the number of requests that can be made within each window.

- **How It Works**: All requests in the current time window are allowed until the request limit is reached. Once the limit is exceeded, all subsequent requests are denied until the next time window.
- **Use Case**: Useful when you need to limit access over a predictable period (e.g., X requests per minute or hour).
- **Configuration**:
  - **Min**: 5 requests per minute
  - **Max**: 100 requests per minute

### 2. **Token Bucket Rate Limiting**
The Token Bucket algorithm is a more flexible version of rate limiting. Requests are "tokens" that are generated at a constant rate, and clients "consume" tokens with each request.

- **How It Works**: A bucket is filled with tokens at a fixed rate. Each request consumes a token from the bucket. If the bucket is empty, the request is denied. Tokens accumulate over time, allowing burst traffic up to a certain limit.
- **Use Case**: Ideal for situations where occasional bursts of traffic are expected but must be smoothed out over time.
- **Configuration**:
  - **Min**: 10 tokens initially, replenished at 1 token per second
  - **Max**: 200 tokens initially, replenished at 5 tokens per second

### 3. **Leaky Bucket Rate Limiting**
This algorithm controls the rate of requests by allowing them to "leak" out of a bucket at a fixed rate.

- **How It Works**: Requests are added to a queue (bucket) and processed at a fixed rate. If the bucket becomes full, subsequent requests are denied until space is available as requests leak out of the bucket.
- **Use Case**: Useful for controlling the flow of requests evenly over time to prevent spikes.
- **Configuration**:
  - **Min**: Process 2 requests per second with a bucket size of 10
  - **Max**: Process 10 requests per second with a bucket size of 50

### 4. **Sliding Log Rate Limiting**
This algorithm tracks the timestamp of each request in a log and limits requests based on the time intervals between requests.

- **How It Works**: The timestamp of each request is recorded in a log. When a new request is made, the log is checked, and if too many requests have been made in the recent past, the new request is denied.
- **Use Case**: Effective for granular rate limiting, as it can handle sporadic traffic more accurately than fixed windows.
- **Configuration**:
  - **Min**: 5 requests in a 30-second window
  - **Max**: 50 requests in a 30-second window

### 5. **Sliding Window Rate Limiting**
Sliding Window Rate Limiting combines the fixed window and sliding log algorithms to provide a more accurate distribution of rate-limited requests over time.

- **How It Works**: This algorithm splits the time into small windows and ensures that the rate limit applies to the current time window and a portion of the previous window, providing smoother rate limiting.
- **Use Case**: More accurate than Fixed Window Rate Limiting when you need smooth transitions between time windows.
- **Configuration**:
  - **Min**: 10 requests in a 60-second sliding window
  - **Max**: 100 requests in a 60-second sliding window

## Configuration Parameters

Each rate-limiting algorithm is configured with specific parameters, including the minimum and maximum number of requests allowed, and the time window or rate at which requests are processed.

### Example Configuration for Each Algorithm:

| Algorithm            | Minimum Limit                       | Maximum Limit                        |
|----------------------|-------------------------------------|--------------------------------------|
| Fixed Window         | 5 requests per minute               | 100 requests per minute              |
| Token Bucket         | 10 tokens, 1 token/second replenish | 200 tokens, 5 tokens/second replenish|
| Leaky Bucket         | Process 2 requests/second           | Process 10 requests/second           |
| Sliding Log          | 5 requests in 30 seconds            | 50 requests in 30 seconds            |
| Sliding Window       | 10 requests in 60 seconds           | 100 requests in 60 seconds           |

## How to Run the Application

1. **Clone the Repository**: 
   ```bash
   git clone <your-repo-url>
   cd RateLimitingDemo
   ```

2. **Build and Run the Application**:
   Make sure you have the .NET SDK installed. Then, run the following commands:
   ```bash
   dotnet build
   dotnet run
   ```

3. **Access Swagger UI**:
   After running the application, Swagger will be available at:
   ```
   http://localhost:<port>/swagger/index.html
   ```
   You can test the rate limits for each API endpoint directly through the Swagger UI.

## API Endpoints

Below are the API endpoints along with their respective rate-limiting algorithm:

| Endpoint                          | Algorithm            | Description |
|-----------------------------------|----------------------|-------------|
| `/api/ratelimit/fixedwindow`       | Fixed Window         | Demonstrates Fixed Window Rate Limiting |
| `/api/ratelimit/tokenbucket`       | Token Bucket         | Demonstrates Token Bucket Rate Limiting |
| `/api/ratelimit/leakybucket`       | Leaky Bucket         | Demonstrates Leaky Bucket Rate Limiting |
| `/api/ratelimit/slidinglog`        | Sliding Log          | Demonstrates Sliding Log Rate Limiting |
| `/api/ratelimit/slidingwindow`     | Sliding Window       | Demonstrates Sliding Window Rate Limiting |

## Testing the Rate Limiting

You can use Swagger UI to test each endpoint by making requests. Once the rate limit is exceeded, the API will return a `429 Too Many Requests` status code, along with a message indicating the rate limit has been exceeded.

For example:

- If you make too many requests to `/api/ratelimit/fixedwindow`, the response will be:
  ```
  429 Too Many Requests
  Too many requests for Fixed Window. Try again later.
  ```

Satya Parimi
---

This README file provides a detailed overview of each algorithm, configurations, and how to use and test the rate-limiting in your API project. You can modify or extend it based on your specific needs.