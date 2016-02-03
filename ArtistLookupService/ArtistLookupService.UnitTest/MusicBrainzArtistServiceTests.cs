using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ArtistLookupService.External_Service_Clients;
using ArtistLookupService.External_Service_Interfaces;
using ArtistLookupService.Wrappers;
using Moq;
using Ploeh.AutoFixture;
using Xunit;

namespace ArtistLookupService.UnitTest
{
    public class MusicBrainzArtistServiceTests
    {
        private readonly MusicBrainzService _sut;
        private readonly Fixture _fixture;
        private readonly Mock<IHttpClientWrapper> _mockedHttpClient;

        public MusicBrainzArtistServiceTests()
        {
            _fixture = new Fixture();

            _mockedHttpClient = new Mock<IHttpClientWrapper>();

            var mockedDescriptionService = new Mock<IDescriptionService>();
            var mockedCoverArtUrlService = new Mock<ICoverArtUrlService>();

            _sut = new MusicBrainzService(_mockedHttpClient.Object,
                mockedDescriptionService.Object,
                mockedCoverArtUrlService.Object);
        }

        [Theory]
        [InlineData(HttpStatusCode.Forbidden)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.NotFound)]
        public void GetAsync_Returns_Null_On_Non_200_Response_From_MusicBrainz(HttpStatusCode responseFromMusicBrainz)
        {
            var mbid = _fixture.Create<string>();
            var httpResponseMessage = new HttpResponseMessage { StatusCode = responseFromMusicBrainz };
            _mockedHttpClient.Setup(m => m.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(httpResponseMessage));

            var result = _sut.GetAsync(mbid).Result;

            Assert.Null(result);
        }
    }
}
