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

        [Fact]
        public async Task Handle_SpotifyDataSource_ReturnsAggregatorDtoWithSpotifyData()
        {
            // Arrange
            var httpRepositoryMock = new Mock<IHttpRepository>();
            var cacheMock = new Mock<IAppCache>();
            cacheMock.Setup(x => x.GetOrAddAsync<SpotifyDto>(It.IsAny<string>(), It.IsAny<Func<Task<SpotifyDto>>>(), It.IsAny<DateTimeOffset>()))
                .ReturnsAsync(new SpotifyDto());

            var spotifyApiHttpRepositoryMock = new Mock<ISpotifyApiHttpRepository>();

            var handler = new AggregatorHandler(httpRepositoryMock.Object, cacheMock.Object, spotifyApiHttpRepositoryMock.Object);
            var request = new AggregatorQuery { DataSource = ApplicationConstants.DataSource.Spotify };
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await handler.Handle(request, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.weatherDto);
            Assert.NotNull(result.spotifyDto);
            Assert.Null(result.newsApiDto);

        }

        [Fact]
        public async Task Handle_NewsDataSource_ReturnsAggregatorDtoWithNewsData()
        {
            // Arrange
            var httpRepositoryMock = new Mock<IHttpRepository>();
            var cacheMock = new Mock<IAppCache>();
            cacheMock.Setup(x => x.GetOrAddAsync<NewsApiDto>(It.IsAny<string>(), It.IsAny<Func<Task<NewsApiDto>>>(), It.IsAny<DateTimeOffset>()))
                .ReturnsAsync(new NewsApiDto());

            var spotifyApiHttpRepositoryMock = new Mock<ISpotifyApiHttpRepository>();

            var handler = new AggregatorHandler(httpRepositoryMock.Object, cacheMock.Object, spotifyApiHttpRepositoryMock.Object);
            var request = new AggregatorQuery { DataSource = ApplicationConstants.DataSource.News };
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await handler.Handle(request, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.weatherDto);
            Assert.Null(result.spotifyDto);
            Assert.NotNull(result.newsApiDto);

        }

        [Fact]
        public async Task Handle_DefaultDataSource_ReturnsAggregatorDtoWithAllData()
        {
            // Arrange
            var httpRepositoryMock = new Mock<IHttpRepository>();
            var cacheMock = new Mock<IAppCache>();
            cacheMock.Setup(x => x.GetOrAddAsync<WeatherApiDto>(It.IsAny<string>(), It.IsAny<Func<Task<WeatherApiDto>>>(), It.IsAny<DateTimeOffset>()))
                .ReturnsAsync(new WeatherApiDto());
            cacheMock.Setup(x => x.GetOrAddAsync<SpotifyDto>(It.IsAny<string>(), It.IsAny<Func<Task<SpotifyDto>>>(), It.IsAny<DateTimeOffset>()))
                .ReturnsAsync(new SpotifyDto());
            cacheMock.Setup(x => x.GetOrAddAsync<NewsApiDto>(It.IsAny<string>(), It.IsAny<Func<Task<NewsApiDto>>>(), It.IsAny<DateTimeOffset>()))
                .ReturnsAsync(new NewsApiDto ());

            var spotifyApiHttpRepositoryMock = new Mock<ISpotifyApiHttpRepository>();

            var handler = new AggregatorHandler(httpRepositoryMock.Object, cacheMock.Object, spotifyApiHttpRepositoryMock.Object);
            var request = new AggregatorQuery { DataSource = "UnknownDataSource" };
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await handler.Handle(request, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.weatherDto);
            Assert.NotNull(result.spotifyDto);
            Assert.NotNull(result.newsApiDto);
        }
    }
}
