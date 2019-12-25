using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digitus.Trial.Backend.Api.Interfaces;
using Digitus.Trial.Backend.Api.Managers;
using Digitus.Trial.Backend.Api.Models;
using Digitus.Trial.Backend.Api.Providers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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

            services.AddTransient<IDatabaseProvider<User>>(p => new UserDatabaseProvider(Configuration["MongoConnectionString"]));                        
            
            services.AddTransient<IPasswordProvider, PasswordProvider>();
            services.AddTransient<IAuthenticatationManager, AuthenticationManager>();
            services.AddTransient<IUserManager, UserManager>();

            

          
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
           
        }
    }
}
