using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Nornickel.Contracts.Configurations;
using Nornickel.Contracts.Interfaces;
using Nornickel.Database.Contexts;
using Nornickel.Database.DataModels;
using Nornickel.Database.Enums;
using Nornickel.Infrastructure.Interfaces;
using Nornickel.Infrastructure.Services;
using Npgsql;
using System.Text;
using System.Text.Json.Serialization;

namespace Nornickel.Infrastructure;

public static class DependencyInjection
{
    public static void ConfigureServices(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddOptions<JwtTokenConfiguration>()
            .Bind(configuration.GetSection(JwtTokenConfiguration.ConfigurationSectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        serviceCollection.AddScoped<IJwtTokenService, JwtTokenService>();
        serviceCollection.AddScoped<IPasswordHasher<UserDataModel>, PasswordHasher<UserDataModel>>();

        serviceCollection.AddScoped<IAuthenticationService, AuthenticationService>();
        serviceCollection.AddScoped<ICandidateService, CandidateService>();
        serviceCollection.AddScoped<IAnalyticsService, AnalyticsService>();

        serviceCollection.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        serviceCollection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                };
            });

        serviceCollection.AddAuthorizationBuilder();
    }

    public static void ConfigureAppDbContext(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddDbContextFactory<HrSystemDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("NornickelHrSystemDB");

            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
            dataSourceBuilder.MapEnum<UserRole>("UserRole");
            dataSourceBuilder.MapEnum<CandidateStatus>("CandidateStatus");

            var dataSource = dataSourceBuilder.Build();
            options.UseNpgsql(dataSource);
        });
    }

    public static void ConfigureDbMigrate(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<HrSystemDbContext>>();
            var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<UserDataModel>>();

            using (var db = factory.CreateDbContext())
            {
                db.Database.Migrate();

                if (!db.Set<UserDataModel>().Any())
                {
                    var adminUser = new UserDataModel
                    {
                        Username = "admin",
                        Role = UserRole.Admin
                    };

                    adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "admin123");

                    db.Set<UserDataModel>().Add(adminUser);
                    db.SaveChanges();
                }
            }
        }
    }
}
