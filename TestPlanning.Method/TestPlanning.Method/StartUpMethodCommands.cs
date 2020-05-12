using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using TestPlanning.Common.Helpers;
using TestPlanning.Method.CommandHandlers;

namespace TestPlanning.Method
{
    public class StartUpMethodCommands : BackgroundService
    {
        // https://andrewlock.net/running-async-tasks-on-app-startup-in-asp-net-core-part-1/

        private readonly IServiceProvider _serviceProvider;
        public StartUpMethodCommands(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            // Create a new scope to retrieve scoped services
            using (var scope = _serviceProvider.CreateScope())
            {
                // Get the DbContext instance
                var _messages = scope.ServiceProvider.GetRequiredService<Messages>();
                _messages.Subscribe(new AddMethodCommand());
                //_messages.Subscribe(new EditMethodCommand());
                //_messages.Subscribe(new DeleteMethodCommand());
            }

            // don't modify the middleware pipeline
            return next;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Factory.StartNew(() =>
             {
                // Create a new scope to retrieve scoped services
                using (var scope = _serviceProvider.CreateScope())
                 {
                    // Get the DbContext instance
                    var _messages = scope.ServiceProvider.GetRequiredService<Messages>();
                     _messages.Subscribe(new AddMethodCommand());
                    //_messages.Subscribe(new EditMethodCommand());
                    //_messages.Subscribe(new DeleteMethodCommand());
                }
             });
        }
    }
}
