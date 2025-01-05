using System;
using System.Collections.Generic;
using System.Linq;
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

        async Task<List<Response>> IWordSearch.WordSearch(Request request)
        {
            var uri = $"https://api.dictionaryapi.dev/api/v2/entries/en/{request.DictionaryWord}";
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var dictionaryEntries = JsonSerializer.Deserialize<List<Response>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Handle case-insensitive JSON properties
                });

                return dictionaryEntries;
            }
            else
            {
                throw new Exception($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }
    }
}
