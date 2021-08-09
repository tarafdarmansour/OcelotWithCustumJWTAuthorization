using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace OcelotAPIGateWay
{
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Http;
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

            services.AddHttpClient<IAuthorizationService, AuthorizationService>();

            services.AddScoped<IJWTHelpers, JWTHelpers>();
            

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
                AuthorizationMiddleware = async (ctx, next) =>
                {

                    if (! await authorizationService.IsValid(ctx))
                    {
                        ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        await ctx.Response.WriteAsync("Some Error !");
                    }
                    else
                    {
                        await next.Invoke();

                    }
                },
            };

            app.UseOcelot(configuration).Wait();
        }
    }
}
