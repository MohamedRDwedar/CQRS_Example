using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestPlanning.Common;
using TestPlanning.Common.Helpers;
using TestPlanning.Common.Interfaces;
using TestPlanning.Common.Models;
using TestPlanning.Common.Wrappers;
using TestPlanning.Method.CommandHandlers;
using TestPlanning.Method.CommandSubscribers;
using TestPlanning.Method.Context;
//using TestPlanning.Method.Events;
using TestPlanning.Method.Queries;
using Newtonsoft.Json;

namespace TestPlanning.Method
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                // Use the default property (Pascal) casing
                options.SerializerSettings.TypeNameHandling = TypeNameHandling.All;
               // options.SerializerSettings.
                options.UseCamelCasing(true);
                // Configure a custom converter
                options.SerializerSettings.MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead;
            });


            services.AddTransient<MethodContext>();
            services.AddScoped<Messages>();
            services.AddTransient<ConumerWrapper>();
            services.AddTransient<ProducerWrapper>();
            services.AddHostedService<StartUpMethodCommands>(); 
            //services.AddHandlers();
            services.AddTransient<ICommandHandler<AddMethodCommand>, AddMethodCommandHandler>();
            //services.AddTransient<ICommandHandler<EditMethodCommand>, EditMethodCommandHandler>();
            //services.AddTransient<ICommandHandler<DeleteMethodCommand>, DeleteMethodCommandHandler>();

            //services.AddTransient<IEventHandler<MethodAddedEvent>, MethodAddedEventHandler>();
            //services.AddTransient<IEventHandler<MethodEditedEvent>, MethodEditedEventHandler>();
            //services.AddTransient<IEventHandler<MethodDeletedEvent>, MethodDeletedEventHandler>();

            services.AddTransient<ICommandSubscriber<AddMethodCommand>, AddMethodCommandSubscriber>();
            //services.AddTransient<ICommandSubscriber<EditMethodCommand>, EditMethodCommandSubscriber>();
            //services.AddTransient<ICommandSubscriber<DeleteMethodCommand>, DeleteMethodCommandSubscriber>();

            services.AddTransient<IQueryHandler<GetMethodQuery, MethodModel>, GetMethodQueryHandler>();

            //services.AddScoped<MethodCommandsService>();
            //services.AddScoped<MethodEventsService>();
            //services.AddScoped<MethodHandlersService>();

            //services.AddTransient<IStartupFilter, StartUpMethodCommands>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
