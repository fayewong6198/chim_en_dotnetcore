using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Chim_En_DOTNET.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Chim_En_DOTNET.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Chim_En_DOTNET
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
      services.AddDbContext<ApplicationDbContext>(options =>
          options.UseNpgsql(
                    Configuration.GetConnectionString("DefaultConnection")));
      services.AddDefaultIdentity<ApplicationUser>(options =>
      {
        options.SignIn.RequireConfirmedAccount = true;
        options.Password.RequiredUniqueChars = 0;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.User.RequireUniqueEmail = true;
      })
          .AddEntityFrameworkStores<ApplicationDbContext>();
      services.AddControllersWithViews();
      services.AddRazorPages();
      services.AddControllers().AddNewtonsoftJson(x =>
          {
            x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
          });


      services.AddAuthentication(config =>
{
  config.DefaultScheme = "smart";
})
.AddPolicyScheme("smart", "Bearer or Jwt", options =>
{
  options.ForwardDefaultSelector = context =>
   {
     var bearerAuth = context.Request.Headers["Authorization"].FirstOrDefault()?.StartsWith("Bearer ") ?? false;
     // You could also check for the actual path here if that's your requirement:
     // eg: if (context.HttpContext.Request.Path.StartsWithSegments("/api", StringComparison.InvariantCulture))
     if (bearerAuth)
       return JwtBearerDefaults.AuthenticationScheme;
     else
       return IdentityConstants.ApplicationScheme;
   };
})
.AddJwtBearer(options =>
       {
         options.TokenValidationParameters = new TokenValidationParameters
         {
           ValidateIssuerSigningKey = true,
           IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
           .GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
           ValidateIssuer = false,
           ValidateAudience = false
         };
       });
      // services.AddMvc(config =>
      // {
      //   var defaultPolicy = new AuthorizationPolicyBuilder(new[] { JwtBearerDefaults.AuthenticationScheme, IdentityConstants.ApplicationScheme })
      //                  .RequireAuthenticatedUser()
      //                  .Build();
      //   config.Filters.Add(new AuthorizeFilter(defaultPolicy));
      //   config.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
      //   // config.Filters.Add(new ValidateModelAttribute());
      // });


    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseDatabaseErrorPage();
      }
      else
      {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
        endpoints.MapRazorPages();
        endpoints.MapControllers();
      });
    }
  }
}
