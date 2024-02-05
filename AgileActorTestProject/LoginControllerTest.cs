using AgileActorsDemo.Controllers;
using AgileActorsDemo.Models;
using AgileActorsDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgileActorTestProject
{
    public class LoginControllerTests
    {
        [Fact]
        public async Task Login_ValidCredentials_ReturnsOkResultWithToken()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.ValidateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            userServiceMock.Setup(x => x.GenerateToken(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns("fakeAccessToken");

            var controller = new LoginController(userServiceMock.Object);
            var credentials = new UserInput { Username = "admin", Password = "password" };
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await controller.Login(credentials, cancellationToken) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("fakeAccessToken", result.Value);
            Assert.Equal((int)System.Net.HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorizedResult()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.ValidateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var controller = new LoginController(userServiceMock.Object);
            var credentials = new UserInput { Username = "invalidUser", Password = "invalidPassword" };
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await controller.Login(credentials, cancellationToken) as StatusCodeResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)System.Net.HttpStatusCode.Unauthorized, result.StatusCode);
        }
    }
}
