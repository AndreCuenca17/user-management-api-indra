using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.Application.Ports.Output;
using UserManagement.Infrastructure.Persistence;
using UserManagement.Infrastructure.Persistence.Repositories;
using UserManagement.Infrastructure.Security;

namespace UserManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Falta la cadena de conexión 'ConnectionStrings:Default'.");
        var serverVersion = configuration["Database:ServerVersion"]
            ?? throw new InvalidOperationException("Falta la configuración 'Database:ServerVersion'.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(connectionString, new MySqlServerVersion(Version.Parse(serverVersion))));

        services.AddScoped<IUserRepository, EfUserRepository>();

        services.AddSingleton(JwtOptions.FromConfiguration(configuration));
        services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
        services.AddSingleton<ITokenGenerator, JwtTokenGenerator>();

        return services;
    }
}
