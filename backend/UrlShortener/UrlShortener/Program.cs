using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UrlShortener.BLL.Services.JwtService;
using UrlShortener.BLL.Services.JwtService.Interfaces;
using UrlShortener.BLL.Services.UrlService;
using UrlShortener.BLL.Services.UrlService.Interfaces;
using UrlShortener.BLL.Services.UserContext;
using UrlShortener.BLL.Services.UserContext.Interfaces;
using UrlShortener.DAL.DbContext;
using UrlShortener.DAL.Entities;
using UrlShortener.DAL.UnitOfWork;
using UrlShortener.DAL.UnitOfWork.Interfaces;

namespace UrlShortener
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<UrlShorterContext>
                (o => o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<User, IdentityRole<int>>()
                .AddEntityFrameworkStores<UrlShorterContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddScoped<RoleManager<IdentityRole<int>>>();

            builder.Services.AddAuthorization();

            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer"),
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration.GetValue<string>("Jwt:Audience"),
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Jwt:Key")))
                    };
                });

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddTransient<ClaimsPrincipal>(s =>
                s.GetService<IHttpContextAccessor>().HttpContext.User);


            builder.Services.AddScoped<IUrlService, UrlService>();
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IUserContext, UserContext>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors("AllowAnyOrigin");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=About}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
