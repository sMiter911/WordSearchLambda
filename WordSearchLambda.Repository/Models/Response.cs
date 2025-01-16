using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WordSearchLambda.Repository.Models
{
    public class Response
    {
        public List<WordSearchResponses> WordSearchResponses { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
    }
}
