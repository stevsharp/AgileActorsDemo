using AgileActorsDemo.Constant;
using AgileActorsDemo.Models;
using AgileActorsDemo.Services;

using Microsoft.Extensions.Logging;

using Moq;
using Moq.Protected;

using System.Net;
using Xunit.Abstractions;

namespace AgileActorTestProject
{
    public class HttpRepositoryTests
    {
        private readonly ITestOutputHelper _output;

        public HttpRepositoryTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task GetAsync_ShouldReturnData()
        {
          
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var loggerMock = new Mock<ILogger<HttpRepository>>();

            var handlerMock = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(handlerMock.Object);

            var httpRepository = new HttpRepository(httpClientFactoryMock.Object, loggerMock.Object);

            var expectedData = new NewsApiDto();
            var expectedJson = System.Text.Json.JsonSerializer.Serialize(expectedData);

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedJson),
            };

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            httpClientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await httpRepository.GetAsync<NewsApiDto>(ApplicationConstants.Cache.GetNewsCacheKey);

            // Assert
            Assert.NotNull(result);

        }

    }
}
