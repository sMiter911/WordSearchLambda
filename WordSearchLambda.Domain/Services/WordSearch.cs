using Amazon.Lambda.APIGatewayEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WordSearchLambda.Contracts.IServices;
using WordSearchLambda.Repository.Models;

namespace WordSearchLambda.Domain.Services
{
    public class WordSearch : IWordSearch
    {
        public WordSearch()
        {
            
        }

        async Task<Response> IWordSearch.WordSearch(string request)
        {
            Response wordSearchResponse = new Response();
            var uri = $"https://api.dictionaryapi.dev/api/v2/entries/en/{request}";
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var dictionaryEntries = JsonSerializer.Deserialize<List<WordSearchResponses>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Handle case-insensitive JSON properties
                });

               wordSearchResponse = new Response
                {
                    WordSearchResponses = dictionaryEntries,
                    StatusCode = HttpStatusCode.OK
                };

                return wordSearchResponse;
            }
            else
            {
                return new Response
                {
                    WordSearchResponses = null,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message= $"Error: {response.StatusCode} - {response.ReasonPhrase}"
                };
            }
        }
    }
}
