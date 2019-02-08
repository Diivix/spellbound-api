using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using spellbound_api.Models;
using System.Collections.Generic;

namespace spellbound_api
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
      // Add CORS
      services.AddCors(options => options
        .AddPolicy("CorsPolicy", builder => builder
          .WithOrigins(Configuration.GetSection("Cors:WithOrigins").Get<String[]>())
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials()
          )
        );

      // Add Swagger
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new Info { Title = "Spellbound API", Version = "v1" });
        c.AddSecurityDefinition("Bearer", new ApiKeyScheme
        {
          Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\". Dont forget to add \"Bearer\" at the begining.",
          Name = "Authorization",
          In = "header",
          Type = "apiKey"
        });

        // Swagger 2+ support for supplying token back to the server on subsequent api calls.
        var security = new Dictionary<string, IEnumerable<string>>
        {
            {"Bearer", new string[] { }},
        };
        c.AddSecurityRequirement(security);
      });

      // Database context
      var connection = "Data Source=" + Configuration["Database:Connection"];
      services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connection));

      // Identity management
      services.AddIdentity<User, IdentityRole>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

      // Add Jwt Authentication
      JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
      services.AddAuthentication(options =>
          {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
          }).AddJwtBearer(cfg =>
          {
            cfg.RequireHttpsMetadata = false;
            cfg.SaveToken = true;
            cfg.TokenValidationParameters = new TokenValidationParameters
            {
              ValidateIssuerSigningKey = true,
              ValidIssuer = Configuration["Jwt:Issuer"],
              ValidateIssuer = true,
              ValidAudience = Configuration["Jwt:Audience"],
              ValidateAudience = false,
              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
              ClockSkew = TimeSpan.Zero // remove delay of token when expire
            };
          });

      services.AddAuthorization(options =>
      {
        options.AddPolicy("UserAuth", policy => policy.RequireClaim("roles", "User"));
        options.AddPolicy("AdminAuth", policy => policy.RequireClaim("roles", "Admin"));
      });

      // Add MVC
      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public async void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbContext dbContext, UserManager<User> userManager)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        // Enable middleware to serve generated Swagger as a JSON endpoint.
        app.UseSwagger();
        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
        // specifying the Swagger JSON endpoint.
        app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Spellbound V1"); c.DocExpansion(DocExpansion.None); });
      }
      else // Production
      {
        app.UseHsts();
        app.UseHttpsRedirection();
      }

      // Seed database with admin user. A non-terminating exception will be thrown if the account exists.
      var existingAdmin = await userManager.FindByEmailAsync(Configuration["AdminUser:Email"]);
      if(existingAdmin == null) {
        User user = new User { UserName = Configuration["AdminUser:Name"], Email = Configuration["AdminUser:Email"] };
        await userManager.CreateAsync(user, Configuration["AdminUser:Password"]);
        await userManager.AddToRoleAsync(user, "User");
        await userManager.AddToRoleAsync(user, "Admin");
      }

      app.UseCors("CorsPolicy");
      app.UseAuthentication();
      app.UseMvc();
    }
  }
}
