using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Digitus.Trial.Backend.Api.Interfaces;
using Digitus.Trial.Backend.Api.Managers;
using Digitus.Trial.Backend.Api.Models;
using Digitus.Trial.Backend.Api.Providers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace Digitus.Trial.Backend.Api
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
            services.AddDataProtection();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("CoreSwagger", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Swagger on ASP.NET Core",
                    Version = "1.0.0",
                    
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Name = "Digitus Trial",
                        Url = new Uri("http://digitustrial.com")

                    },
                    TermsOfService = new Uri("http://swagger.io/terms/")
                });
            });

            services.AddTransient<IDatabaseProvider<User>>(p => new UserDatabaseProvider(Configuration["MongoConnectionString"]));
            services.AddTransient<IDatabaseProvider<UserLog>>(p => new UserLogDatabaseProvider(Configuration["MongoConnectionString"]));
            services.AddTransient<INotificationManager, NotificationManager>();
            services.AddTransient<IPasswordProvider, PasswordProvider>();
            services.AddTransient<IAuthenticatationManager, AuthenticationManager>();
            services.AddTransient<IUserManager, UserManager>();
            services.AddTransient<IReportManager, ReportManager>();
            services.AddTransient<IMockManager, MockManager>();


            var tokenSecret = Configuration.GetSection("TokenGeneratorSecretKey").Value;
            
            var key = Encoding.ASCII.GetBytes(tokenSecret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger()
           .UseSwaggerUI(c =>
           {
               //TODO: Either use the SwaggerGen generated Swagger contract (generated from C# classes)
               c.SwaggerEndpoint("/swagger/CoreSwagger/swagger.json", "Swagger Test .Net Core");

               //TODO: Or alternatively use the original Swagger contract that's included in the static files
               // c.SwaggerEndpoint("/swagger-original.json", "Swagger Petstore Original");
           });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
           
        }
    }
}
