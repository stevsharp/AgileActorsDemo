
using Microsoft.Extensions.Options;

using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AgileActorsDemo.Services
{
    public class UserService : IUserService
    {
        private readonly JwtSettings jwtSettings;
        public UserService(IOptions<JwtSettings> jwtOptions)
        {
            jwtSettings = jwtOptions.Value;
        }

        public Task<bool> ValidateUser(string username, string password, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException($"'{nameof(username)}' cannot be null or whitespace.", nameof(username));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException($"'{nameof(password)}' cannot be null or whitespace.", nameof(password));
            }

            // Simulate the database call

            if (!username.Equals("admin") || !password.Equals("password"))
                return Task.FromResult(false);

            return Task.FromResult(true);

        }

        public string GenerateToken(string userName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException($"'{nameof(userName)}' cannot be null or whitespace.", nameof(userName));
            }

            var notBefore = DateTime.UtcNow;
            var issuedAt = DateTime.UtcNow;
            var validFor = TimeSpan.FromMinutes(jwtSettings.ValidTime);
            var expiration = issuedAt.Add(validFor);

            var claims = new List<Claim>()
            {
                new(JwtRegisteredClaimNames.Sub, userName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.UniqueName, userName),
                new(ClaimTypes.Expiration, expiration.ToString(CultureInfo.InvariantCulture)),
            };


            var token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                notBefore: notBefore,
                expires: expiration,
                signingCredentials: jwtSettings.SigningCredentials);


            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodedToken;
        }
    }
}


