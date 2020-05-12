using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using TestPlanning.Notification.Services;

namespace TestPlanning.Notification
{
    public class StartUpNotificationHandlers : IStartupFilter
    {
        private readonly IServiceProvider _serviceProvider;
        public StartUpNotificationHandlers(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            // Create a new scope to retrieve scoped services
            using (var scope = _serviceProvider.CreateScope())
            {
                // Get the DbContext instance
                var experimentHandlersService = scope.ServiceProvider.GetRequiredService<ExperimentHandlersService>();
                var methodHandlersService = scope.ServiceProvider.GetRequiredService<MethodHandlersService>();

                experimentHandlersService.ExperimentCreatedHandler();
                experimentHandlersService.ExperimentUpadatedHandler();
                experimentHandlersService.ExperimentDeletedHandler();

                methodHandlersService.MethodCreatedHandler();
                methodHandlersService.MethodUpadatedHandler();
                methodHandlersService.MethodDeletedHandler();
            }

            // don't modify the middleware pipeline
            return next;
        }
    }
}
