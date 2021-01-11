using System;
using System.IO;
using System.Linq;

using FluentScheduler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PubCiterAPI.Data;
using PubCiterAPI.Repositories;

namespace PubCiterAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            // TODO: jobs
            //JobManager.AddJob(new MyJob(new MyJobRepository()), s => s.NonReentrant().ToRunNow().AndEvery(1).Seconds());

            var builder = new ConfigurationBuilder()
                .AddJsonFile("webconfig.json", optional: true, reloadOnChange: true);

            this.Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            services.AddControllers().AddNewtonsoftJson(options =>
                     options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


            services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc(@"v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = @"PubCiter API documentation" });
                }
            );

            AppSettings.ConnectionString = this.Configuration.GetSection(@"AppSettings").GetChildren().FirstOrDefault(x => x.Key == @"ConnectionString")?.Value;

            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(AppSettings.ConnectionString));
            this.AddRepositoriesToContainer(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint(@"/swagger/v1/swagger.json", @"API v1");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void AddRepositoriesToContainer(IServiceCollection services)
        {
            services.AddScoped<AuthorRepository>();
            services.AddScoped<PublicationRepository>();
            services.AddScoped<CitationRepository>();
        }
    }
}
