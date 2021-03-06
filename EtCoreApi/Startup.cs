using EtCore.Api.Repositories;
using EtCoreApi.Repositories;
using EtCoreApi.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Net.Mime;
using System.Text.Json;

namespace EtCoreApi
{
    public class Startup
    {
        private readonly string _dataBaseSystemUsed;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _dataBaseSystemUsed = Configuration.GetSection("CustomSettings").GetValue<string>("DatabaseSystem");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EtCoreApi v1"));
            }

            if (env.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            app.UseRouting();

            app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});

            app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller}/{action}/{id?}");
                }
            );

            if (string.IsNullOrEmpty(_dataBaseSystemUsed) == false)
            {
                if (_dataBaseSystemUsed.Equals("sql"))
                {
                    PrepSqlDb.PrepPopulation(app);
                }
                else if (_dataBaseSystemUsed.Equals("mongo"))
                {
                    app.UseEndpoints(endpoints =>
                    {
                        //endpoints.MapControllers();

                        endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions
                        {
                            Predicate = (check) => check.Tags.Contains("ready"),
                            ResponseWriter = async (context, report) =>
                            {
                                var result = JsonSerializer.Serialize(new
                                {
                                    status = report.Status.ToString(),
                                    checks = report.Entries.Select(entry => new
                                    {
                                        name = entry.Key,
                                        status = entry.Value.Status.ToString(),
                                        exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "none",
                                        duration = entry.Value.Duration.ToString()
                                    })
                                });

                                context.Response.ContentType = MediaTypeNames.Application.Json;
                                await context.Response.WriteAsync(result);
                            }
                        });

                        endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
                        {
                            Predicate = (_) => false
                        });
                    });
                }
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

            if (string.IsNullOrEmpty(_dataBaseSystemUsed) == false)
            {
                if (_dataBaseSystemUsed.Equals("sql"))
                {
                    #region EntityFramework SQL Server Settings

                    services.AddScoped<IExpensesRepository, SqlDbExpensesRepository>();
                    services.AddDbContext<AppSqlDbContext>(options => options.UseInMemoryDatabase("InMem"));

                    #endregion EntityFramework SQL Server Settings
                }
                else if (_dataBaseSystemUsed.Equals("mongo"))
                {
                    #region MongoDB Settings

                    var mongoDbSettings = Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();

                    services.AddSingleton<IMongoClient>(serviceProvider => new MongoClient(mongoDbSettings.ConnectionString));

                    services.AddSingleton<IExpensesRepository, MongoDbExpensesRepository>();

                    services.AddHealthChecks().AddMongoDb(
                        mongoDbSettings.ConnectionString,
                        name: "mongodb",
                        timeout: TimeSpan.FromSeconds(3),
                        tags: new[] { "ready" }
                    );

                    #endregion MongoDB Settings
                }
            }

            services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EtCoreApi", Version = "v1" });
            });
        }
    }
}