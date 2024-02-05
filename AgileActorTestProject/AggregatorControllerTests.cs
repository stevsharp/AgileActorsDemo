using AgileActorsDemo.Controllers;
using AgileActorsDemo.Models;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Moq;

namespace AgileActorTestProject
{
    public class AggregatorControllerTests
    {
        [Fact]
        public async Task Get_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<AggregatorQuery>(), default))
                .ReturnsAsync(new AggregatorDto(new WeatherApiDto(), new SpotifyDto(), new NewsApiDto()));

            var controller = new AggregatorController(mediatorMock.Object);
            var request = new AggregatorQuery(/* Set your query parameters here */);

            // Act
            var result = await controller.Get(request) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)System.Net.HttpStatusCode.OK, result.StatusCode);

        }

        [Fact]
        public async Task Get_NullRequest_ThrowsArgumentNullException()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var controller = new AggregatorController(mediatorMock.Object);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => controller.Get(null));
        }
    }
}
