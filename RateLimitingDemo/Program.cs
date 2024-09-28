using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RateLimitingDemo.Middleware;
using RateLimitingDemo.RateLimiters;
using System;
using System.IO;
using System.Reflection;

namespace RateLimitingDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices(services =>
                    {
                        // Register rate limiters as singletons
                        services.AddSingleton<IFixedWindowRateLimiter, FixedWindowRateLimiter>();
                        services.AddSingleton<ITokenBucketRateLimiter, TokenBucketRateLimiter>();
                        services.AddSingleton<ILeakyBucketRateLimiter, LeakyBucketRateLimiter>();
                        services.AddSingleton<ISlidingLogRateLimiter, SlidingLogRateLimiter>();
                        services.AddSingleton<ISlidingWindowRateLimiter, SlidingWindowRateLimiter>();

                        services.AddControllers();

                        // Add Swagger services
                        services.AddSwaggerGen(c =>
                        {
                            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                            {
                                Title = "Rate Limiting API",
                                Version = "v1",
                                Description = "This API demonstrates various rate-limiting algorithms such as Fixed Window, Token Bucket, Leaky Bucket, Sliding Log, and Sliding Window."
                            });

                            // Configure Swagger to use XML comments
                            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                            c.IncludeXmlComments(xmlPath);
                        });
                    });

                    webBuilder.Configure(app =>
                    {
                        if (app.ApplicationServices.GetService<IHostEnvironment>().IsDevelopment())
                        {
                            app.UseDeveloperExceptionPage();
                        }

                        app.UseRouting();

                        // Enable Swagger and Swagger UI
                        app.UseSwagger();
                        app.UseSwaggerUI(c =>
                        {
                            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rate Limiting API v1");
                            // You can remove this comment to serve Swagger UI at the app's root
                            // c.RoutePrefix = string.Empty;
                        });

                        // Add rate limiting middleware
                        app.UseMiddleware<RateLimitingMiddleware>();

                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();
                        });
                    });
                });
    }
}
