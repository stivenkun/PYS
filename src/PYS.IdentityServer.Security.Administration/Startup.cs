using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IdentityServerWithAspIdAndEF.Data;
using IdentityServerWithAspIdAndEF.Models;
using System.Reflection;
using IdentityServer4.Services;
using IdentityServerWithAspIdAndEF.Profiles;
using IdentityServerWithAspIdAndEF.Services;
using PYS.IdentityServer.Security.Administration.ConfigurationStore;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;
using AccessData;
using Repository.Interfaces;
using Repository.Repositories;
//using Repository.Interfaces;
//using Repository.Repository;

namespace IdentityServerWithAspIdAndEF
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Environment = environment;
            //appsettings.json configuration
            var builder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            #region basic configuration
            string connectionString = Configuration.GetConnectionString("IdentityServerConnection");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<ConfigurationStoreContext>(options =>
                options.UseSqlServer(connectionString));
            services.AddDbContext<AccessData.Datas.ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));


            services.AddScoped<IUserRepository, UserRepository>();
            //services.AddScoped<UserManager>();
            services.AddMvc();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            #endregion

            #region identity server configuration
            // configure identity server with in-memory stores, keys, clients and scopes
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients());
            #endregion

            #region Stores configuration
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IResourceStoreExtended, ResourceStore>();
            services.AddTransient<IProfileService, ProfileService>();
            services.AddTransient<IClientStoreExtended, ClientStore>();
            #endregion

            #region  Authentication

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
            {
                options.SignInScheme = "Cookies";

                options.Authority = "http://localhost:5000/";
                options.RequireHttpsMetadata = false;
                options.ClientId = "mvc";
                options.SaveTokens = true;
            });

            #endregion

            #region authorization
            services.AddAuthorization(options => {
                options.AddPolicy("AdministratorIS", policy => policy
                                                                .RequireClaim("Application", "IdentityServer")
                                                                .RequireClaim("IdentityServer","Admin"));
            }
            );

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Home/AccessDenied";
            });

            #endregion

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseMvcWithDefaultRoute();
        }
    }
}
