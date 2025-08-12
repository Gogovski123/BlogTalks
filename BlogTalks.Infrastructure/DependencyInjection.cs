using BlogTalks.Application.Abstractions;
using BlogTalks.Domain.Repositories;
using BlogTalks.Infrastructure.Authentication;
using BlogTalks.Infrastructure.Data.DataContext;
using BlogTalks.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BlogTalks.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Database");

            services.AddDbContext<ApplicationDbContext>(
                options => options
                .UseNpgsql(connectionString, npgsqlOptions =>
                npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Default )));

            services.AddScoped<ApplicationDbContext>();
            services.AddTransient<IBlogPostRepository, BlogPostRepository>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddTransient<IUserRepository, UserRepository>();

            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            //services.Configure<JwtSettings>(options =>
            //{
            //    options.Issuer = configuration["JwtSettings:Issuer"] ?? string.Empty;
            //    options.Audience = configuration["JwtSettings:Audience"] ?? string.Empty;
            //    options.SecretKey = configuration["JwtSettings:SecretKey"] ?? string.Empty;
            //    options.ExpiresInMinutes = int.TryParse(configuration["JwtSettings:ExpiresInMinutes"], out var expiresIn) ? expiresIn : 60;
            //});
            

            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            var key = Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"] ?? string.Empty);

            services.AddAuthentication(options =>
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
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            

            return services;
        }
    }
}
