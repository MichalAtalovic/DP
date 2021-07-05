using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PubCiterAPI.Model;
using PubCiterAPI.Model.SyncState;
using PubCiterAPI.Repositories;

namespace PubCiterAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
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
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            });



            services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc(@"v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = @"PubCiterAPI V1 docs" });
                    c.EnableAnnotations();
                }
            );

            services.AddSwaggerGenNewtonsoftSupport();

            // Load application settings from webconfig
            AppSettings.ConnectionString = this.Configuration.GetSection(@"AppSettings").GetChildren().FirstOrDefault(x => x.Key == @"ConnectionString")?.Value;
            AppSettings.Author = this.Configuration.GetSection(@"AppSettings").GetChildren().FirstOrDefault(x => x.Key == @"Author")?.Value;
            AppSettings.SemanticsScriptPath = this.Configuration.GetSection(@"Scraper").GetChildren().FirstOrDefault(x => x.Key == @"SemanticsScriptPath")?.Value;
            AppSettings.ScholarScriptPath = this.Configuration.GetSection(@"Scraper").GetChildren().FirstOrDefault(x => x.Key == @"ScholarScriptPath")?.Value;
            AppSettings.OutputFolder = this.Configuration.GetSection(@"Scraper").GetChildren().FirstOrDefault(x => x.Key == @"OutputFolder")?.Value;
            AppSettings.PybTexConvertScriptPath = this.Configuration.GetSection(@"Utils").GetChildren().FirstOrDefault(x => x.Key == @"PybTexConvertScriptPath")?.Value;
            AppSettings.PybTexFormatScriptPath = this.Configuration.GetSection(@"Utils").GetChildren().FirstOrDefault(x => x.Key == @"PybTexFormatScriptPath")?.Value;
            AppSettings.Ris2BibScriptPath = this.Configuration.GetSection(@"Utils").GetChildren().FirstOrDefault(x => x.Key == @"Ris2BibScriptPath")?.Value;
            
            // Reset states of synchronization
            CurrentSyncState.GoogleScholar = SyncStateEnum.Idle;
            CurrentSyncState.GoogleScholar = SyncStateEnum.Idle;
            CurrentSyncState.GoogleScholar = SyncStateEnum.Idle;

            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(AppSettings.ConnectionString));
            this.AddRepositoriesToContainer(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs\\Log.txt");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(@"/swagger/v1/swagger.json", @"PubCiterAPI V1");
                c.RoutePrefix = @"swagger";
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
            services.AddScoped<SettingsRepository>();
            services.AddScoped<QuarantineRepository>();
            services.AddScoped<UtilsRepository>();
            services.AddScoped<EnumRepository>();
        }
    }
}
