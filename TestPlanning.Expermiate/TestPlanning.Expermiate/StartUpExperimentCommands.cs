using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using TestPlanning.Experiment.Services;

namespace TestPlanning.Experiment
{
    public class StartUpExperimentCommands : IStartupFilter
    {
        private readonly IServiceProvider _serviceProvider;
        public StartUpExperimentCommands(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            // Create a new scope to retrieve scoped services
            using (var scope = _serviceProvider.CreateScope())
            {
                // Get the DbContext instance
                var experimentService = scope.ServiceProvider.GetRequiredService<ExperimentHandlersService>();

                experimentService.CreateExperimentHandler();
                experimentService.UpadateExperimentHandler();
                experimentService.DeleteExperimentHandler();
            }

            // don't modify the middleware pipeline
            return next;
        }
    }
}
