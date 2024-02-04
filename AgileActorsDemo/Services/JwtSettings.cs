using Microsoft.IdentityModel.Tokens;

namespace AgileActorsDemo.Services
{
    public class JwtSettings
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Subject { get; set; }
        public string Audience { get; set; }
        public int ValidTime { get; set; } = 120;
        public SigningCredentials SigningCredentials { get; set; }
    }
}


