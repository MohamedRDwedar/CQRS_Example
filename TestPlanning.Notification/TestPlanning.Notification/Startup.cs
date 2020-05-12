using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestPlanning.Notification.Hubs;
using TestPlanning.Notification.Services;

namespace TestPlanning.Notification
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
            services.AddControllers();
            services.AddSignalR();
            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                options.AddPolicy("CORSpolicy", builder => builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin()
                    .WithOrigins("*")
                    .SetIsOriginAllowed((host) => true)
                    .AllowCredentials()
                );
            });

            services.AddScoped<ExperimentHandlersService>();
            services.AddScoped<MethodHandlersService>();
 
            services.AddTransient<IStartupFilter, StartUpNotificationHandlers>();
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
            app.UseCors("CORSpolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ExperimentHub>("/ExperimentHub");
                endpoints.MapHub<MethodHub>("/MethodHub");
            });
        }
    }
}
