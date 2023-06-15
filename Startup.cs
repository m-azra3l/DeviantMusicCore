using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using DeviantMusicCore.Data;
using DeviantMusicCore.Models;
using DeviantMusicCore.Logic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;

namespace DeviantMusicCore
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
            services.AddTransient<IPasswordValidator<ApplicationUser>, CustomPasswordPolicy>();
            services.AddTransient<IUserValidator<ApplicationUser>, CustomUsernamePolicy>();
            services.AddDbContext<DeviantContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("Default"), p => p.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddIdentity<ApplicationUser,IdentityRole>()
                .AddEntityFrameworkStores<DeviantContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();
                //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                //.AddEntityFrameworkStores<DeviantContext>()
                //.AddDefaultUI()
                //.AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options =>{
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "0123456789abcdefghijklmnopqrstuvwxyz";
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = true;
            });    
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));                
            services.AddScoped<IDBInitializer, DBInitializer>();
            services.AddControllersWithViews();
             /*services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });*/
            services.AddRazorPages();
            
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.KnownProxies.Add(IPAddress.Parse("10.0.0.100"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,  IDBInitializer dBInitializer)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseCookiePolicy();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            dBInitializer.Initialize();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
