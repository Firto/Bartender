using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DataBase;
using API.Helpers;
using API.Hostings.ServiceInitialization;
using API.Managers.CClientErrorManager;
using API.Managers.CClientErrorManager.Middleware;
using API.Services.Abstraction;
using API.Services.Implementation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace API
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
            services.AddHostedService<ServiceInitializationHosting>();

            // Db Context
            services.AddDbContext<DbContext, ApplicationContext>(opt =>
                opt.UseSqlite("Filename=base.db"), optionsLifetime: ServiceLifetime.Singleton);
            services.AddSingleton<ClientErrorManager>();
            services.AddSingleton<IValidatingService, ValidatingService>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IStatsService, StatsService>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("SW", new OpenApiInfo { Title = "My API", Version = "SW" });
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseSwagger();
            app.UseAuthorization();
            app.UseMiddleware<ClientErrorsMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/SW/swagger.json", "SW");
            });
        }
    }
}
