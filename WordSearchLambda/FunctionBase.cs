using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordSearchLambda.Modules;

namespace WordSearchLambda
{
    public class FunctionBase
    {
        internal readonly ServiceProvider _serviceProvider;
        public FunctionBase()
        {
            var services = ConfigurationServices();
            _serviceProvider = services.BuildServiceProvider();
        }

        public FunctionBase(IServiceCollection serviceCollection)
        {
            var services = serviceCollection;
            _serviceProvider = services.BuildServiceProvider();
        }

        private IServiceCollection ConfigurationServices()
        {
            IServiceCollection services = new ServiceCollection();

            CommonModule.Load(services);

            return services;
        }
    }
}
