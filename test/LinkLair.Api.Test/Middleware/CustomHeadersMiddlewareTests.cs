using System.Threading.Tasks;
using LinkLair.Api.Middleware;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace LinkLair.Api.Test.Middleware
{
    public class CustomHeadersMiddlewareTests
    {
        [Fact]
        public async Task CustomHeadersMiddleware_Invoke_Success()
        {
            RequestDelegate next = context => Task.CompletedTask;

            var middleware = new CustomHeadersMiddleware(next);

            var headers = new HeaderDictionary();
            var responseMock = new Mock<HttpResponse>();
            responseMock.SetupGet(response => response.Headers).Returns(headers);

            var contextMock = new Mock<HttpContext>();
            contextMock.SetupGet(response => response.Response).Returns(responseMock.Object);
            contextMock.SetupProperty(response => response.TraceIdentifier, "SYSTEM");

            await middleware.Invoke(contextMock.Object);

            Assert.NotNull(headers);
            Assert.Equal("SYSTEM", headers["x-linklair-correlationid"]);
        }
    }
}
