using System.Net;
using ArtistLookupService.Domain;
using ArtistLookupService.External_Services;
using ArtistLookupService.Test.Configuration;
using ArtistLookupService.Test.Extensions;
using FluentAssertions;
using Microsoft.AspNet.TestHost;
using Moq;
using Ploeh.AutoFixture;
using Xunit;

namespace ArtistLookupService.Test
{
    public class ArtistLookupControllerComponentTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IArtistService> _mockedArtistService;
        private readonly Mock<ICoverArtUrlService> _mockedCoverArtUrlService;
        private readonly Mock<IDescriptionService> _mockedDescriptionService;

        public ArtistLookupControllerComponentTests()
        {
            _fixture = new Fixture();

            _mockedArtistService = new Mock<IArtistService>();
            _mockedCoverArtUrlService = new Mock<ICoverArtUrlService>();
            _mockedDescriptionService = new Mock<IDescriptionService>();

            _mockedArtistService.Setup(m => m.Get(It.IsAny<string>())).Returns((string mbid) => new Artist {Id = mbid});
        }

        private TestServer CreateTestServer()
        {
            return TestServerFactory.CreateTestServerWith(_mockedArtistService.Object,
                _mockedCoverArtUrlService.Object,
                _mockedDescriptionService.Object);
        }

        [Fact]
        public async void Get_With_Empty_Mbid_Returns_404()
        {
            using (var client = CreateTestServer().CreateClient())
            {
                var response = await client.GetAsync("api/artistlookup");

                response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            }
        }

        [Fact]
        public async void Get_With_Non_Empty_Mbid_Returns_Artist()
        {
            var mbid = _fixture.Create<string>();

            using (var client = CreateTestServer().CreateClient())
            {
                var response = await client.GetAsync($"api/artistlookup/{mbid}");
                var artist = await response.Content.ReadAsJsonAsync<Artist>();

                artist.Should().NotBeNull();
                artist.Id.Should().Be(mbid);
            }
        }
    }
}

