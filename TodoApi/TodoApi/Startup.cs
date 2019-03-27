using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TodoApi.Models;

namespace TodoApi
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddDbContext<TodoContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,//проверить сервер, который создал этот токен
                    ValidateAudience = true,//убедиться, что получатель токена авторизован для его получения
                    ValidateLifetime = true,//проверить, что токен не истек и что ключ подписи элитента действителен
                    ValidateIssuerSigningKey = true,//убедиться, что ключ, используемый для подписи входящего токена, является частью списка доверенных ключей
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder appBuilder, IHostingEnvironment hostEnvironment)
        {
            if (hostEnvironment.IsDevelopment())//приложение находится в состоянии разработки
            {
                appBuilder.UseDeveloperExceptionPage();//детальное описание ошибки
            }
            else
            {
                appBuilder.UseHsts();
            }

            appBuilder.UseHttpsRedirection();
            appBuilder.UseAuthentication();
            appBuilder.UseMvc();
        }
    }
}