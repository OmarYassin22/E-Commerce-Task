using Microsoft.Owin;
using Microsoft.Owin.Security.Jwt;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using E_Commerce.Contract.AuthContract;
using E_Commerce.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Owin;
using System.Linq;
using Unity;
using Serilog;

[assembly: OwinStartup(typeof(E_Commerce.WebApiApplication))]

namespace E_Commerce
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private IJwtProvider _jwtProvider;

        protected void Application_Start()
        {
            // Register all the usual configurations
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            UnityConfig.RegisterComponents();

            // Resolve JwtProvider from Unity container
            var container = UnityConfig.GetConfiguredContainer();
            if (container != null)
            {
                _jwtProvider = UnityConfig.GetConfiguredContainer().Resolve<IJwtProvider>();
            }
            // Seed initial admin user

            SeedAdminUser();
        }

        private void SeedAdminUser()
        {
            using (var context = new AppDbContext())
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                if (!roleManager.RoleExists("Admin"))
                    roleManager.Create(new IdentityRole("Admin"));

                if (!roleManager.RoleExists("Customer"))
                    roleManager.Create(new IdentityRole("Customer"));

                if (!context.Users.Any(u => u.Email == "admin@admin.com"))
                {
                    var admin = new ApplicationUser
                    {
                        UserName = "admin@admin.com",
                        Email = "admin@admin.com",
                        FirstName = "Admin",
                        LastName = "Admin",
                        Address = "Benha"
                    };

                    var result = userManager.Create(admin, "P@ssword123");
                    if (result.Succeeded)
                        userManager.AddToRole(admin.Id, "Admin");
                }
            }
        }

        public void Configuration(IAppBuilder app)
        {
            ConfigureJwtAuth(app);
        }

        private void ConfigureJwtAuth(IAppBuilder app)
        {
            var secretKey = "R&hO6]0=::JC*Yeiww>eYB2[ql>]lFK@4FT>oFNWMy^|d`+Ds%2aZ^~vyF(1G(;";  // Same as in JwtProvider
            var issuer = "E-Commerice";
            var audience = "E-Commerice Users";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Passive,
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }
            });
        }



    }
}
