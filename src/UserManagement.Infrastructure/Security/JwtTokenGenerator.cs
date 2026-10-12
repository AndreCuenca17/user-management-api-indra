using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UserManagement.Application.DTOs.Auth;
using UserManagement.Application.Ports.Output;
using UserManagement.Domain.Entities;

namespace UserManagement.Infrastructure.Security;

public class JwtTokenGenerator : ITokenGenerator
{
    public const string PermissionClaimType = "permission";

    private readonly JwtOptions _options;

    public JwtTokenGenerator(JwtOptions options)
    {
        _options = options;
    }

    public GeneratedToken Generate(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        claims.AddRange(user.GetPermissions().Select(permission => new Claim(PermissionClaimType, permission)));

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SigningKey));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_options.ExpirationMinutes),
            signingCredentials: credentials);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        return new GeneratedToken(accessToken, _options.ExpirationMinutes * 60);
    }
}
