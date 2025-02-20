﻿using Amazon.Lambda.APIGatewayEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordSearchLambda.Repository.Models;

namespace WordSearchLambda.Contracts.IServices
{
    public interface IWordSearch
    {
        Task<Response> WordSearch(string request);
    }
}
