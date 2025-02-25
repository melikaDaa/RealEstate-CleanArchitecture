using Microsoft.AspNetCore.Identity;
using RealEstateApp.Infrastructure.Context;
using RealEstateApp.Infrastructure.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Infrastructure.Identity.Seeds;
using RealEstateApp.Application.Repositories;
using RealEstateApp.Infrastructure.Repositories;
using RealEstateApp.Application.Interfaces;
using RealEstateApp.Infrastructure.Mapping;
using RealEstateApp.Infrastructure.Services;
using RealEstateApp.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace RealEstateApp.Presentation.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // پیکربندی Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // ثبت AutoMapper برای تمامی پروفایل‌ها
            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile(new CategoryMappingProfile());
                cfg.AddProfile(new UserMappingProfile());
                cfg.AddProfile(new EstateMappingProfile());
            });

            // ثبت UnitOfWork و سرویس‌ها
            builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>)); // واحد کار عمومی
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IEstateService, EstateService>();
            builder.Services.AddScoped<IUserService, UserService>();

            // ثبت سایر سرویس‌ها
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // پیکربندی JWT
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                ValidAudience = builder.Configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
            };
        });
            builder.Services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true; // نمایش نسخه‌های پشتیبانی‌شده در هدر پاسخ
                options.AssumeDefaultVersionWhenUnspecified = true; // مقدار پیش‌فرض را تنظیم می‌کند
                options.DefaultApiVersion = new ApiVersion(1, 0); // نسخه پیش‌فرض اگر نسخه مشخص نشده باشد
                options.ApiVersionReader = new UrlSegmentApiVersionReader(); // تعیین روش دریافت نسخه
            });

            // برای نمایش نسخه‌های API در Swagger
            builder.Services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV"; // تعیین فرمت نسخه در Swagger
                options.SubstituteApiVersionInUrl = true; // جایگذاری نسخه در مسیر
            });


            var app = builder.Build();

            // Seed کردن کاربر پیش‌فرض
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                await DefaultAdminUser.SeedAsync(userManager, roleManager);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication(); // نیاز به فراخوانی UseAuthentication برای احراز هویت
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
