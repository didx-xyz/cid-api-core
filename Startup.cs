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
using Newtonsoft.Json.Serialization;
using System.Reflection;
using CoviIDApiCore.Models.AppSettings;
using CoviIDApiCore.V1.Brokers;
using System.Net.Http.Headers;
using CoviIDApiCore.Data;
using CoviIDApiCore.V1.Interfaces.Brokers;
using CoviIDApiCore.V1.Interfaces.Repositories;
using CoviIDApiCore.V1.Interfaces.Services;
using CoviIDApiCore.V1.Repositories;
using CoviIDApiCore.V1.Services;

namespace CoviIDApiCore
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly string _environment, _connectionString, _applicationName;
        private const string _applicationJson = "application/json";

        public Startup(IConfiguration configuration)
        {
            _applicationName = Assembly.GetExecutingAssembly().GetName().Name;
            _environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Local";
            Console.WriteLine($"Environment: {_environment}");
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
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

            ConfigureDatabaseContext(services);
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

        private void ConfigureDatabaseContext(IServiceCollection services)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            var connection = new SqlConnectionStringBuilder(connectionString)
            {
                ConnectRetryInterval = 3,
                MinPoolSize = 3
            };
            services.AddDbContext<ApplicationDbContext>(
                b => b.UseSqlServer(
                    connection.ToString(),
                    options => options.EnableRetryOnFailure())
            );
        }

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

        private void ConfigureDependecyInjection(IServiceCollection services)
        {
            #region Service layer
            services.AddTransient<IWalletService, WalletService>();
            services.AddTransient<IConnectionService, ConnectionService>();
            services.AddTransient<IVerifyService, VerifyService>();

            services.AddScoped<IOrganisationService, OrganisationService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddSingleton<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IQRCodeService, QRCodeService>();
            services.AddScoped<IOtpService, OtpService>();
            #endregion

            #region Repository Layer
            services.AddScoped<IOrganisationRepository, OrganisationRepository>();
            services.AddScoped<IOrganisationCounterRepository, OrganisationCounterRepository>();
            services.AddScoped<IOtpTokenRepository, OtpTokenRepository>();
            services.AddScoped<IWalletRepository, WalletRepository>();
            #endregion

            #region Broker Layer
            services.AddTransient<IAgencyBroker, AgencyBroker>();
            services.AddTransient<ICustodianBroker, CustodianBroker>();
            services.AddTransient<ISendGridBroker, SendGridBroker>();
            services.AddTransient<IClickatellBroker, ClickatellBroker>();
            #endregion
        }

        private void ConfigureHttpClients(IServiceCollection services)
        {
            var agencyApiBaseUrl = _configuration.GetConnectionString("AgencyApiBaseUrl");
            var custodianApiBaseUrl = _configuration.GetConnectionString("CustodianApiBaseUrl");
            var tenantId = _configuration.GetValue<string>("TenantId");
            var sendGridCredentials = new SendGridCredentials();
            _configuration.Bind(nameof(SendGridCredentials), sendGridCredentials);

            var streetCredCredentials = new StreetCredCredentials();
            _configuration.Bind(nameof(StreetCredCredentials), streetCredCredentials);

            var clickatellCredentials = new ClickatellCredentials();
            _configuration.Bind(nameof(ClickatellCredentials), clickatellCredentials);

            services.AddHttpClient<IAgencyBroker, AgencyBroker>(client =>
            {
                client.BaseAddress = new Uri(agencyApiBaseUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", streetCredCredentials.AuthorizationToken);
                client.DefaultRequestHeaders.Add("X-Streetcred-Subscription-Key", streetCredCredentials.SubscriptionKey);
                client.DefaultRequestHeaders.Add("X-Streetcred-Tenant-Id", tenantId);
            });

            services.AddHttpClient<ICustodianBroker, CustodianBroker>(client =>
            {
                client.BaseAddress = new Uri(custodianApiBaseUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", streetCredCredentials.AuthorizationToken);
                client.DefaultRequestHeaders.Add("X-Streetcred-Subscription-Key", streetCredCredentials.SubscriptionKey);
                client.DefaultRequestHeaders.Add("X-Streetcred-Tenant-Id", tenantId);
            });

            services.AddHttpClient<ISendGridBroker, SendGridBroker>(client =>
                {
                    client.BaseAddress = new Uri(sendGridCredentials.BaseUrl);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sendGridCredentials.Key);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_applicationJson));
                }
            );

            services.AddHttpClient<IClickatellBroker, ClickatellBroker>(client =>
                {
                    client.BaseAddress = new Uri(clickatellCredentials.BaseUrl);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",clickatellCredentials.Key);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_applicationJson));
                }
            );
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = $"The Delta Studio .NET Core 2.2 Web API {_applicationName}", Version = "v1" });
                options.AddSecurityDefinition("AccessToken", new ApiKeyScheme
                {
                    Description = "Your authorization token allows this organization to authenticate to the API. Example: \"Bearer {token}\"",
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
