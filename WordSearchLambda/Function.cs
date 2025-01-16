using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System.Net;
using System.Text.Json;
using WordSearchLambda.Contracts.IServices;
using WordSearchLambda.Repository.Models;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace WordSearchLambda;


public class Function : FunctionBase
{
    //private static readonly string? RedisConnectionString = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING");
    private static readonly int RequestThreshold = 10; // Max allowed requests
    private static readonly TimeSpan BlockDuration = TimeSpan.FromMinutes(10); // Block duration


    private static readonly Lazy<ConnectionMultiplexer> RedisConnection = new(() =>
    {
        var options = new ConfigurationOptions
        {
            EndPoints = { { "redis-14651.c16.us-east-1-2.ec2.redns.redis-cloud.com", 14651 } },
            User = Environment.GetEnvironmentVariable("REDIS_USER"),
            Password = Environment.GetEnvironmentVariable("REDIS_PASSWORD")
        };

        return ConnectionMultiplexer.Connect(options);
    });

    public async Task<Response> FunctionHandler(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        var sourceIp = request.RequestContext.Http.SourceIp;

        if (string.IsNullOrEmpty(sourceIp))
        {
            return new Response
            {
                WordSearchResponses = new List<WordSearchResponses>(),
                StatusCode = HttpStatusCode.BadRequest,
                Message = "Source IP not found in request."
            };
        }

        try
        {
            // Connect to Redis
            var redis = RedisConnection.Value.GetDatabase();

            // Debugging connection success
            if (!redis.Multiplexer.IsConnected)
            {
                throw new Exception("Failed to connect to Redis.");
            }

#if !DEBUG
            context.Logger.LogInformation($"Connection Successful: {redis.IsConnected}");
#endif

            // Increment the request count for the IP
            var requestCount = await redis.StringIncrementAsync(sourceIp);

            // Set TTL for the key if it's the first request
            if (requestCount == 1)
            {
                await redis.KeyExpireAsync(sourceIp, BlockDuration);
            }

            // Check if the IP has exceeded the threshold
            if (requestCount > RequestThreshold)
            {
                var ttl = await redis.KeyTimeToLiveAsync(sourceIp);
                var timeRemaining = ttl.HasValue ? ttl.Value.ToString(@"hh\:mm\:ss") : "unknown";

#if !DEBUG
                context.Logger.LogInformation($"Too many requests: {requestCount}. Time remaining: {timeRemaining}");
#endif
                return new Response
                {
                    WordSearchResponses = new List<WordSearchResponses>(),
                    StatusCode = HttpStatusCode.TooManyRequests,
                    Message = $"Too many requests {requestCount}/10. Please try again later. Time remaining: {timeRemaining}"
                };
            }

            // Log request for debugging
#if !DEBUG
            context.Logger.LogInformation($"Received request: {JsonSerializer.Serialize(request)}");
#endif

            // Process the request as usual
            var service = _serviceProvider.GetService<IWordSearch>();

            if (service == null)
            {
                throw new Exception("Service not found");
            }

            // Call the service
            var dictionaryEntries = await service.WordSearch(request.Body);

#if !DEBUG
            context.Logger.LogInformation($"Output response: {JsonSerializer.Serialize(dictionaryEntries)}");
#endif

            return dictionaryEntries;
        }
        catch (Exception ex)
        {
            context.Logger.LogError($"Error processing request: {ex.Message}");
            return new Response
            {
                WordSearchResponses = new List<WordSearchResponses>(),
                StatusCode = HttpStatusCode.BadGateway,
                Message = ex.Message
            };
        }
    }
}
