using AutoMapper;
using EmailService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using TO_DO_List.Contracts.Repositories;
using TO_DO_List.Contracts.Services;
using TO_DO_List.Data;
using TO_DO_List.Extensions;
using TO_DO_List.Models;
using TO_DO_List.Repositories;
using TO_DO_List.Services;
using TO_DO_List.Settings;

namespace TO_DO_List
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));

            var emailConfig = Configuration
                .GetSection(Constants.EmailConfiguration)
                .Get<EmailConfiguration>();

            services.AddDbContextPool<ApplicationContext>(
                options => options.UseLazyLoadingProxies()
                .UseMySql(Configuration.GetConnectionString(Constants.DefaultConnection)));

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 12;
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<ApplicationContext>()
            .AddDefaultTokenProviders()
            .AddRoles<IdentityRole>()
            .AddRoleManager<RoleManager<IdentityRole>>();

            services.Configure<DataProtectionTokenProviderOptions>(opt =>
                opt.TokenLifespan = TimeSpan.FromHours(1));

            services.Configure<JwtSettings>(Configuration.GetSection(Constants.Jwt));
            var jwtSettings = Configuration.GetSection(Constants.Jwt).Get<JwtSettings>();
            services.Configure<SeedDataSettings>(options => Configuration.GetSection(Constants.SeedData).Bind(options));

            services.AddAuth(jwtSettings);

            services.AddSwag();

            services.AddControllers();

            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IDatabaseService, DatabaseService>();
            services.AddScoped<IToDoTaskRepository, ToDoTaskRepository>();
            services.AddScoped<IToDoTaskService, ToDoTaskService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddSingleton(emailConfig);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwag();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuth();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
