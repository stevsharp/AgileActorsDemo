using AgileActorsDemo.Services;

using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;

using System.Text;

namespace AgileActorTestProject
{
    public class UserServiceTests
    {
        [Fact]
        public async Task ValidateUser_ValidCredentials_ReturnsTrue()
        {
            // Arrange
            var userService = new UserService(CreateJwtSettingsOptions());
            var username = "admin";
            var password = "password";
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await userService.ValidateUser(username, password, cancellationToken);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ValidateUser_InvalidCredentials_ReturnsFalse()
        {
            // Arrange
            var userService = new UserService(CreateJwtSettingsOptions());
            var username = "invalidUser";
            var password = "invalidPassword";
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await userService.ValidateUser(username, password, cancellationToken);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GenerateToken_ValidUserName_ReturnsToken()
        {
            // Arrange
            var userService = new UserService(CreateJwtSettingsOptions());
            var userName = "admin";
            var cancellationToken = CancellationToken.None;

            // Act
            var token = userService.GenerateToken(userName, cancellationToken);

            // Assert
            Assert.NotNull(token);
            // You may add more specific assertions based on your requirements
        }

        // Helper method to create IOptions<JwtSettings> for testing
        private IOptions<JwtSettings> CreateJwtSettingsOptions()
        {

            var key = Encoding.ASCII.GetBytes("iNivDmHLpUA223sqsfhqGbMRdRj1PVkHsfsdafsaww3544465465");

            var jwtSettings = new JwtSettings
            {
                // Set your JwtSettings properties here for testing
                Issuer = "testIssuer",
                Audience = "testAudience",
                ValidTime = 30, // Set an appropriate value
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };


            var options = new Mock<IOptions<JwtSettings>>();
            options.Setup(x => x.Value).Returns(jwtSettings);

            return options.Object;
        }
    }
}
