using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using svc_backscratcher.DataAccessLayers;
using svc_backscratcher.DatabaseAccess;
using svc_backscratcher.Models;
using svc_backscratcher.TypeConverters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace svc_backscratcher
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
            var autoMapperSingleton = new MapperConfiguration((IMapperConfigurationExpression conf) =>
            {
                conf.CreateMap<BackScratcherDal, BackScratcherRest>().ConvertUsing<DalToRestModelTypeConverter>();
                conf.CreateMap<BackScratcherRest, BackScratcherDal>().ConvertUsing<RestToDalModelTypeConverter>();
            }).CreateMapper();

            services.AddControllers();
            services.Configure<PostgreSqlDataAccessOptions>(Configuration.GetSection(nameof(PostgreSqlDataAccessOptions)));

            services.AddScoped<IBackScratcherRepository, BackScratcherRepository>();
            services.AddScoped<IDatabaseAccess, PostgreSqlDataAccess>();
            
            services.AddSingleton(autoMapperSingleton);
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
