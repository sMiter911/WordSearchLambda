using Amazon.Lambda.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using WordSearchLambda.Contracts.IServices;
using WordSearchLambda.Repository.Models;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace WordSearchLambda;

public class Function : FunctionBase
{

    public async Task<List<Response>> FunctionHandler(Request request, ILambdaContext context)
    {

        var service = _serviceProvider.GetService<IWordSearch>();

        if (service == null)
        {
            throw new Exception("Service not found");
        }

        try
        {
            var dictionaryEntries = await service.WordSearch(request);

            return dictionaryEntries;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
}
