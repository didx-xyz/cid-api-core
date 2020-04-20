using System;
using System.IO;
using System.Net;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using Newtonsoft.Json;
using CoviIDApiCore.Middleware;
using CoviIDApiCore.V1.DTOs.System;
using CoviIDApiCore.Data;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using CoviIDApiCore.Models.AppSettings;
using CoviIDApiCore.V1.Brokers;
using System.Net.Http.Headers;
using CoviIDApiCore.V1.Interfaces.Brokers;
using CoviIDApiCore.V1.Interfaces.Services;
using CoviIDApiCore.V1.Services;

namespace CoviIDApiCore
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly string _envrionment;
        private readonly string _applicationName;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            _envrionment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Local";
            _applicationName = Assembly.GetExecutingAssembly().GetName().Name;

        }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureCORS(services);
            ConfigureSwagger(services);

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(o => o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver())
                .AddJsonOptions(o => o.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None)
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var problems = new CustomBadRequestMiddleware(context);
                        return new BadRequestObjectResult(problems);
                    };
                });

            ConfigureDbConnections(services);
            ConfigureDependecyInjection(services);
            ConfigureHttpClients(services);            
        }        

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment() || env.IsEnvironment("Local"))
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();              
                app.UseSwaggerUI(swagger =>
                {
                    swagger.SwaggerEndpoint("/swagger/v1/swagger.json", $"Cov-ID Core {_applicationName}");
                    swagger.RoutePrefix = string.Empty;
                });
            }
            else
                app.UseHsts();
            
            app.UseCors("AllowSpecificOrigin");
            app.UseAuthentication();
            ConfigureDefaultResponses(app);
            app.UseMiddleware<ExceptionHandler>();
            app.UseHttpsRedirection();
            app.UseMvc();
        }       

        #region Private Configuration Methods

        private void ConfigureCORS(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });
        }

        private void ConfigureDbConnections(IServiceCollection services)
        {
            var coreAppDbStr = _configuration.GetConnectionString("CoreApplicationContext");
            var coreAppConnection = new SqlConnectionStringBuilder(coreAppDbStr)
            {
                ConnectRetryInterval = 3,
                MinPoolSize = 3
            };
            services.AddDbContext<ApplicationCoreDbContext>(
                b => b.UseSqlServer(
                    coreAppConnection.ToString(),
                    options => options.EnableRetryOnFailure()));
        }

        private void ConfigureDependecyInjection(IServiceCollection services)
        {
            #region Repository Layer

            #endregion Repository Layer

            #region Service Layer
            services.AddTransient<IAgencyBroker, AgencyBroker>();
            services.AddTransient<ICustodianBroker, CustodianBroker>();

            services.AddTransient<IWalletService, WalletService>();
            services.AddTransient<IConnectionService, ConnectionService>();
            services.AddTransient<IVerifyService, VerifyService>();




            #endregion Service Layer

            #region Gateway Layer

            #endregion Gateway Layer
        }

        private void ConfigureHttpClients(IServiceCollection services)
        {
            // Configure and add any httpclients that are used for gateways here.
            string agencyApiBaseUrl = _configuration.GetConnectionString("AgencyApiBaseUrl");
            string custodianApiBaseUrl = _configuration.GetConnectionString("CustodianApiBaseUrl");
            string tenantId = _configuration.GetValue<string>("TenantId");

            // This retrieves the config from the AppSettings, based on the structure of the model
            var streetCredCredentials = new StreetCredCredentials();
            _configuration.Bind(nameof(StreetCredCredentials), streetCredCredentials);

            // Agency API
            services.AddHttpClient<IAgencyBroker, AgencyBroker>(client =>
            {
                
                client.BaseAddress = new Uri(agencyApiBaseUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", streetCredCredentials.AuthorizationToken);
                client.DefaultRequestHeaders.Add("X-Streetcred-Subscription-Key", streetCredCredentials.SubscriptionKey);
                client.DefaultRequestHeaders.Add("X-Streetcred-Tenant-Id", tenantId);
            });
            // Custodian API
            services.AddHttpClient<ICustodianBroker, CustodianBroker>(client =>
            {
                client.BaseAddress = new Uri(custodianApiBaseUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", streetCredCredentials.AuthorizationToken);
                client.DefaultRequestHeaders.Add("X-Streetcred-Subscription-Key", streetCredCredentials.SubscriptionKey);
                client.DefaultRequestHeaders.Add("X-Streetcred-Tenant-Id", tenantId);
            });
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = $"The Delta Studio .NET Core 2.2 Web API {_applicationName}", Version = "v1" });
                options.AddSecurityDefinition("AccessToken", new ApiKeyScheme
                {
                    Description = "Your authorization token allows this organization to autenticate to the API. Example: \"Bearer {token}\"",
                    In = "header",
                    Name = "Authorization",
                    Type = "apiKey"
                });

                options.AddSecurityDefinition("Subscription-Key", new ApiKeyScheme
                {
                    In = "header",
                    Description = "Your subscription key allows the API to understand the permissions associated with your account.",
                    Name = "X-Subscription-Key",
                    Type = "apiKey",
                });
            });
        }

        private void ConfigureDefaultResponses(IApplicationBuilder app)
        {
            app.UseStatusCodePages(async context =>
            {
                if (context.HttpContext.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    await context.HttpContext.Response.WriteAsync(
                        JsonConvert.SerializeObject(new V1.DTOs.System.Response(false, HttpStatusCode.Unauthorized, 
                        "Unauthorised")));
                }
            });
        }

        #endregion Private Configuration Methods
    }
}
