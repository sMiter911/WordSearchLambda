using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text.Json;
using WordSearchLambda.Contracts.IServices;
using WordSearchLambda.Repository.Models;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace WordSearchLambda;

public class Function : FunctionBase
{
    public async Task<Response> FunctionHandler(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
#if !DEBUG
        context.Logger.LogInformation($"Received request: {JsonSerializer.Serialize(request)}");
#endif

        var service = _serviceProvider.GetService<IWordSearch>();

        if (service == null)
        {
            throw new Exception("Service not found");
        }

        try
        {
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
                WordSearchResponses = null,
                StatusCode = HttpStatusCode.BadGateway,
                Message = ex.Message
            };
        }
    }
}
