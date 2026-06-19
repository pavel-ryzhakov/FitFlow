using System.Text;
using FitFlow.Infrastructure.Auth;
using FitFlow.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace FitFlow.IntegrationTests.Common;

public class FitFlowWebApplicationFactory : WebApplicationFactory<Program>
{
    private const string TestIssuer = "FitFlow";
    private const string TestAudience = "FitFlow";
    private const string TestSecretKey = "fitflow-integration-tests-secret-key-123456789";

    private readonly string _databaseName = $"FitFlowTests_{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, configurationBuilder) =>
        {
            var settings = new Dictionary<string, string?>
            {
                ["Jwt:Issuer"] = TestIssuer,
                ["Jwt:Audience"] = TestAudience,
                ["Jwt:SecretKey"] = TestSecretKey,
                ["Jwt:ExpirationMinutes"] = "60"
            };

            configurationBuilder.AddInMemoryCollection(settings);
        });

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<FitFlowDbContext>>();
            services.RemoveAll<IDbContextOptionsConfiguration<FitFlowDbContext>>();
            services.RemoveAll<FitFlowDbContext>();

            services.AddDbContext<FitFlowDbContext>(options =>
            {
                options.UseInMemoryDatabase(_databaseName);
            });

            services.PostConfigure<JwtOptions>(options =>
            {
                options.Issuer = TestIssuer;
                options.Audience = TestAudience;
                options.SecretKey = TestSecretKey;
                options.ExpirationMinutes = 60;
            });

            services.PostConfigure<JwtBearerOptions>(
                JwtBearerDefaults.AuthenticationScheme,
                options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = TestIssuer,

                        ValidateAudience = true,
                        ValidAudience = TestAudience,

                        ValidateLifetime = true,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(TestSecretKey))
                    };
                });

            using var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<FitFlowDbContext>();

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        });
    }
}