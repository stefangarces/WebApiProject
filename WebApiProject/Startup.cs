using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using WebApiProject.Controllers;
using WebApiProject.Data;
using WebApiProject.Models;

namespace WebApiProject
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
            services.AddApiVersioning(o =>
            {
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ReportApiVersions = true;
            });
            services.AddVersionedApiExplorer(o =>
            {
                // Formatera som "'v'major[.minor][-state]"
                o.GroupNameFormat = "'v'VVV";

                // Url versionering
                o.SubstituteApiVersionInUrl = true;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GeoMessageV1", Version = "v1" });
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "GeoMessageV2", Version = "v2" });
                c.EnableAnnotations();
            });

            services.AddDbContext<GeoDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("GeoDbContext")));

            services.AddDefaultIdentity<MyUser>()
            .AddEntityFrameworkStores<GeoDbContext>();

            services.AddAuthentication("MegaAuth")
                .AddScheme<AuthenticationSchemeOptions, AuthController>("MegaAuth", null);

            services.AddCors(options =>
             {
                 options.AddPolicy("AnyOrigin",
                     builder =>
                     {
                         builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost").AllowCredentials();

                         builder.WithHeaders("*");
                         builder.WithMethods("POST", "GET");
                     });
             });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiProject v1");
                    c.SwaggerEndpoint("/swagger/v2/swagger.json", "WebApiProject v2");
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
