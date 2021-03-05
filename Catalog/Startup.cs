using Catalog.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System.Reflection;
using System.Diagnostics;
using Microsoft.Extensions.Options;
using Catalog.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog
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
            services.Configure<SecretSettings>(Configuration.GetSection(nameof(SecretSettings))); //dotnet user-secrets set "SecretSettings:ServiceApiKey" "12345"
            services.Configure<MongoSettings>(Configuration.GetSection(nameof(MongoSettings)));

            BsonSerializer.RegisterSerializer(new GuidSerializer(MongoDB.Bson.BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(MongoDB.Bson.BsonType.String));

            services.AddSingleton(CreateMongoClient);

            services.AddSingleton<IRepository, MongoRepository>();
            //services.AddSingleton<IRepository, InMemRepository>();

            services.AddControllers();
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog", Version = "v1" }));

            services.AddDbContext<GameDbContext>(options => options.UseSqlServer("name=SqlServerSettings:ConnectionString"));
        }

        private IMongoClient CreateMongoClient(IServiceProvider serviceProvider)
        {
            var settings = serviceProvider.GetService<IOptions<MongoSettings>>().Value;
            serviceProvider.GetService<ILogger<Startup>>().LogInformation($"Creating  MongoClient: {settings.ConnectionString}");
            return new MongoClient(settings.ConnectionString);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env, 
            ILoggerFactory loggerFactory, 
            ILogger<Startup> logger, 
            IOptions<SecretSettings> secretSettings,
            IOptions<MongoSettings> mongoSettings)
        {
            loggerFactory.AddFile("Logs/Log.txt", outputTemplate: "{Timestamp:o} {RequestId,13} [{Level:u3}] {SourceContext} {Message} ({EventId:x8}){NewLine}{Exception}");

            logger.LogInformation($"Starting ... {FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion}....");
            logger.LogInformation($" {nameof(MongoSettings)}: {JsonSerializer.Serialize(mongoSettings)}");
            logger.LogInformation($" {nameof(SecretSettings)}: {JsonSerializer.Serialize(secretSettings)}");

            if (true /*env.IsDevelopment()*/)
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
