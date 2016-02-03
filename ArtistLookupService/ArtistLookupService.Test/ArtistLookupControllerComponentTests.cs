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

            var response = await client.GetAsync("api/artistlookup");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async void Get_Returns_Error_500_On_Exception()
        {
            _mockedArtistService.Setup(m => m.Get(It.IsAny<string>())).Throws(new Exception());
            var client = CreateClient();
            var mbid = _fixture.Create<string>();

            var response = await client.GetAsync($"api/artistlookup/{mbid}");

            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async void Get_Calls_Logger_On_Exception()
        {
            _mockedArtistService.Setup(m => m.Get(It.IsAny<string>())).Throws(new Exception());
            var client = CreateClient();
            var mbid = _fixture.Create<string>();

            await client.GetAsync($"api/artistlookup/{mbid}");

            _mockedExceptionLogger.Verify(m => m.Log(It.IsAny<HttpRequest>(), It.IsAny<Exception>()), Times.Once());
        }

        [Fact]
        public async void Get_With_Non_Empty_Mbid_Returns_Artist()
        {
            var mbid = _fixture.Create<string>();
            var client = CreateClient();

            var response = await client.GetAsync($"api/artistlookup/{mbid}");
            var artist = await response.Content.ReadAsJsonAsync<Artist>();

            artist.Should().NotBeNull();
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

