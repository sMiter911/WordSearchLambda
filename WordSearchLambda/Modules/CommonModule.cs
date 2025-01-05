using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordSearchLambda.Contracts.IServices;
using WordSearchLambda.Domain.Services;

namespace WordSearchLambda.Modules
{
    public class CommonModule
    {
        public static void Load(IServiceCollection services)
        {
            services.AddSingleton<IWordSearch, WordSearch>();
        }
    }
}
