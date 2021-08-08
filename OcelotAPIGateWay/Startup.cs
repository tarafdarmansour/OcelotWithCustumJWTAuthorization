using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace OcelotAPIGateWay
{
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.Hosting;
    using Microsoft.IdentityModel.Tokens;
    using OcelotAPIGateWay.Dependency;
    using OcelotAPIGateWay.Services;
    using System.Net;
    using System.Text;

    public class Startup
    {
        private readonly ILogger<Startup> _logger;


        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string AuthenticationKey = "AuthenticationKey";
            services.AddLogging();
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(AuthenticationKey, option =>
             {
                 option.RequireHttpsMetadata = true;
                 option.SaveToken = true;
                 option.TokenValidationParameters = new TokenValidationParameters
                 {
                     IssuerSigningKey =
                         new SymmetricSecurityKey(
                             Encoding.ASCII.GetBytes(Configuration.GetSection("SecretKey").Value)),
                     ValidateIssuerSigningKey = true,
                     ValidateIssuer = false,
                     ValidateAudience = false
                 };

                 //option.ForwardSignIn

             });


            services.AddScoped<IJWTHelpers, JWTHelpers>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();

            services.AddOcelot();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IAuthorizationService authorizationService)
        {
            if (env.EnvironmentName == Environments.Development)
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            var configuration = new OcelotPipelineConfiguration
            {
                //PreErrorResponderMiddleware = async (ctx, next) =>
                //{
                //    _logger.LogInformation("PreErrorResponderMiddleware Before");
                //    await next.Invoke();
                //    _logger.LogInformation("PreErrorResponderMiddleware After");
                //},
                //AuthenticationMiddleware = async (ctx, next) =>
                //{
                //    _logger.LogInformation("AuthenticationMiddleware Before");
                //    await next.Invoke();
                //    _logger.LogInformation("AuthenticationMiddleware After");
                //},

                //AuthorizationMiddleware = async (ctx, next) =>
                //{

                //    if (!authorizationService.Validate(ctx))
                //    {
                //        ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                //        byte[] data = System.Text.Encoding.UTF8.GetBytes("error");
                //        await ctx.Response.Body.WriteAsync(data, 0, data.Length);
                //    }
                //    else
                //    {
                //        await next.Invoke();

                //    }
                //},

                //PreAuthenticationMiddleware = async (ctx, next) =>
                //{
                //    _logger.LogInformation("PreAuthenticationMiddleware Before");
                //    await next.Invoke();
                //    _logger.LogInformation("PreAuthenticationMiddleware After");
                //},
                //PreAuthorizationMiddleware = async (ctx, next) =>
                //{
                //    _logger.LogInformation("PreAuthorizationMiddleware Before");
                //    await next.Invoke();
                //    _logger.LogInformation("PreAuthorizationMiddleware After");
                //},
                // PreQueryStringBuilderMiddleware = async (ctx, next) =>
                //{
                //    _logger.LogInformation("PreQueryStringBuilderMiddleware Before");
                //    await next.Invoke();
                //    _logger.LogInformation("PreQueryStringBuilderMiddleware After");
                //},

            };


            app.UseOcelot(configuration).Wait();
        }
    }
}
