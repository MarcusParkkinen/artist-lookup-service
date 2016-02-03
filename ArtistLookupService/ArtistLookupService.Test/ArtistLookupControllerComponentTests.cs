using System;
using System.Net;
using System.Net.Http;
using ArtistLookupService.Extensions;
using ArtistLookupService.External_Service_Interfaces;
using ArtistLookupService.Logging;
using ArtistLookupService.Model;
using ArtistLookupService.Test.Configuration;
using FluentAssertions;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.TestHost;
using Moq;
using Ploeh.AutoFixture;
using Xunit;

namespace ArtistLookupService.Test
{
    public class ArtistLookupControllerComponentTests
    {
        private const string Uri = "api/artistlookup";
        private readonly Fixture _fixture;

        private readonly Mock<IArtistDetailsService> _mockedArtistService;
        private readonly Mock<IExceptionLogger> _mockedExceptionLogger;

        public ArtistLookupControllerComponentTests()
        {
            _fixture = new Fixture();

            _mockedArtistService = new Mock<IArtistDetailsService>();
            _mockedExceptionLogger = new Mock<IExceptionLogger>();

            _mockedArtistService.Setup(m => m.Get(It.IsAny<string>())).Returns(_fixture.Create<Artist>());
        }

        [Fact]
        public async void Get_Without_Specifying_Mbid_Returns_404()
        {
            var client = CreateClient();

            var response = await client.GetAsync(Uri);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async void Get_Returns_Error_500_On_Exception()
        {
            _mockedArtistService.Setup(m => m.Get(It.IsAny<string>())).Throws(new Exception());
            var client = CreateClient();
            var mbid = _fixture.Create<string>();

            var response = await client.GetAsync($"{Uri}/{mbid}");

            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async void Get_Calls_Logger_On_Exception()
        {
            _mockedArtistService.Setup(m => m.Get(It.IsAny<string>())).Throws(new Exception());
            var client = CreateClient();
            var mbid = _fixture.Create<string>();

            await client.GetAsync($"{Uri}/{mbid}");

            _mockedExceptionLogger.Verify(m => m.Log(It.IsAny<HttpRequest>(), It.IsAny<Exception>()), Times.Once());
        }

        [Fact]
        public async void Get_With_Mbid_For_Existing_Artist_Returns_Artist()
        {
            var mbid = _fixture.Create<string>();
            var client = CreateClient();

            var response = await client.GetAsync($"{Uri}/{mbid}");
            var artist = await response.Content.ReadAsJsonAsync<Artist>();

            artist.Should().NotBeNull();
        }

        [Fact]
        public async void Get_With_Mbid_Without_Artist_Returns_404()
        {
            _mockedArtistService.Setup(m => m.Get(It.IsAny<string>())).Returns(() => null);

            var mbid = _fixture.Create<string>();
            var client = CreateClient();

            var response = await client.GetAsync($"{Uri}/{mbid}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        private HttpClient CreateClient()
        {
            return CreateTestServer().CreateClient();
        }

        private TestServer CreateTestServer()
        {
            return TestServerFactory.CreateTestServerWith(_mockedArtistService.Object,
                _mockedExceptionLogger.Object);
        }
    }
}

