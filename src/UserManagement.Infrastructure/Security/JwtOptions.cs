using Microsoft.Extensions.Configuration;

namespace UserManagement.Infrastructure.Security;

public class JwtOptions
{
    public const string SectionName = "Jwt";
    private const int MinimumSigningKeyLength = 32;

    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required string SigningKey { get; init; }
    public required int ExpirationMinutes { get; init; }

    public static JwtOptions FromConfiguration(IConfiguration configuration)
    {
        var section = configuration.GetSection(SectionName);

        var options = new JwtOptions
        {
            Issuer = section["Issuer"] ?? throw MissingSetting("Issuer"),
            Audience = section["Audience"] ?? throw MissingSetting("Audience"),
            SigningKey = section["SigningKey"] ?? throw MissingSetting("SigningKey"),
            ExpirationMinutes = int.TryParse(section["ExpirationMinutes"], out var minutes) && minutes > 0
                ? minutes
                : throw MissingSetting("ExpirationMinutes")
        };

        if (options.SigningKey.Length < MinimumSigningKeyLength)
            throw new InvalidOperationException(
                $"La configuración '{SectionName}:SigningKey' debe tener al menos {MinimumSigningKeyLength} caracteres.");

        return options;
    }

    private static InvalidOperationException MissingSetting(string key) =>
        new($"Falta o es inválida la configuración '{SectionName}:{key}'.");
}
