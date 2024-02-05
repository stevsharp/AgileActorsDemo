using AgileActorsDemo.Constant;
using AgileActorsDemo.Models;
using AgileActorsDemo.Queries;
using AgileActorsDemo.Services;
using LazyCache;
using Moq;


namespace AgileActorTestProject
{
    public class AggregatorHandlerTests
    {
        [Fact]
        public async Task Handle_WeatherDataSource_ReturnsAggregatorDtoWithWeatherData()
        {
            // Arrange
            var httpRepositoryMock = new Mock<IHttpRepository>();
            httpRepositoryMock.Setup(x => x.GetAsync<WeatherApiDto>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new WeatherApiDto ());

            var cacheMock = new Mock<IAppCache>();

            var spotifyApiHttpRepositoryMock = new Mock<ISpotifyApiHttpRepository>();

            var handler = new AggregatorHandler(httpRepositoryMock.Object, cacheMock.Object, spotifyApiHttpRepositoryMock.Object);
            var request = new AggregatorQuery { DataSource = ApplicationConstants.DataSource.Weather };
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await handler.Handle(request, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.weatherDto);
            Assert.Null(result.spotifyDto);
            Assert.Null(result.newsApiDto);
        }

    }
}
