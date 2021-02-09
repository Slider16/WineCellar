using System;
using System.IO;
using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using WineCellar.API.Repositories;
using WineCellar.API.Models;
using WineCellar.API.Interfaces;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace WineCellar.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Trying to do loggging to the Console with my MongoEventsLogger in Helpers folder.
            services.AddLogging(builder =>
            {
                builder.AddConfiguration(Configuration.GetSection("Logging"))
                       .AddConsole()
                       .AddDebug();
            });

            services.AddControllers(setupAction =>
            {

                setupAction.ReturnHttpNotAcceptable = true;

            }).AddNewtonsoftJson(options => options.UseMemberCasing())
            .AddXmlDataContractSerializerFormatters();

            // Map Entities to our DTO Models.  Uses "profiles" in the MappingProfiles folder.
            // Use mapping in conjunction with DTO objects (Data Transfer Objects) in order 
            // to "re-shape" data passed to the client
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.Configure<WineCellarDatabaseSettings>(Configuration.GetSection(nameof(WineCellarDatabaseSettings)));

            services.AddSingleton<IWineCellarDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<WineCellarDatabaseSettings>>().Value);

            /* The singleton service lifetime is most appropriate because WineService 
             * takes a direct dependency on MongoClient. Per the official Mongo Client 
             * reuse guidelines, MongoClient should be registered in DI with a 
             * singleton service lifetime.*/
            services.AddSingleton<IWineRepository, WineRepositoryMongoDB>();
            services.AddSingleton<IVendorRepository, VendorRepositoryMongoDB>();
            services.AddSingleton<IWinePurchaseRepository, WinePurchaseRepositoryMongoDB>();
            services.AddSingleton<IVineyardRepository, VineyardRepositoryMongoDB>();

            //services.AddTransient<IWineService, MockWineService>();

            services.AddCors(options =>
            {
                options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("WineCellarOpenAPISpecification", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "WineCellar.Net.API",
                    Version = "1.0",
                    Description = "A simple wine tracking API based app with some external data sources",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Name = "Elgin Rogers",
                        Email = "elginr16@gmail.com",
                        Url = new Uri("https://www.twitter.com/Slider16")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    },
                    TermsOfService = new Uri("https://www.slidersAPIs/termsofservice"),
                });

                //setupAction.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFileFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

                setupAction.IncludeXmlComments(xmlCommentsFileFullPath);
                
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors("Open");

            app.UseSwagger();

            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint("/swagger/WineCellarOpenAPISpecification/swagger.json",
                    "Wine Cellar API v1");

                setupAction.RoutePrefix = "";
                
            });

            app.UseEndpoints(endpoints =>
            {               
                endpoints.MapControllers();
            });
        }
    }
}
