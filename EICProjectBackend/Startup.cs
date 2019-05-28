using System; 
using EICProjectBackend.Setup;
using EICProjectBackend.Setup.Setups; 
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http; 
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EICProjectBackend
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
             services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            
            //Should fix Coors problem.
            services.Configure<IISOptions>(options =>
            {
                options.ForwardClientCertificate = false;
            }); 
            //Adds services for the application.
            SetupRunner.Run(new IdentitySetup(services, Configuration));
            SetupRunner.Run(new ApplicationSetup(services, Configuration));
            SetupRunner.Run(new AuthenticationSetup(services, Configuration));
            SetupRunner.Run(new MVCSetup(services, Configuration));
            SetupRunner.Run(new MediatRSetup(services, Configuration));
            SetupRunner.Run(new SwaggerSetup(services, Configuration));
            SetupRunner.Run(new ServiceWorkersSetup(services, Configuration));        
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        { 
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();

                // Tell the Web Api to use swagger.
                app.UseSwagger();

                // Proivde user interface for Web Api.
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "EICProject Backend-API");
                });              
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            //Prevents http prefligths
            app.UseCors(options => options.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
            );

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });           
        }

        


    }
}
