using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ArtistLookupService.External_Service_Clients;
using ArtistLookupService.External_Service_Interfaces;
using ArtistLookupService.Logging;
using ArtistLookupService.UnitTest.Extensions;
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
        private readonly Mock<IExceptionLogger> _mockedLogger;

        public MusicBrainzArtistServiceTests()
        {
            _fixture = new Fixture();

            _mockedHttpClient = new Mock<IHttpClientWrapper>();

            var mockedDescriptionService = new Mock<IDescriptionService>();
            var mockedCoverArtUrlService = new Mock<ICoverArtUrlService>();
            _mockedLogger = new Mock<IExceptionLogger>();

            _sut = new MusicBrainzService(_mockedHttpClient.Object,
                mockedDescriptionService.Object,
                mockedCoverArtUrlService.Object,
                _mockedLogger.Object);
        }

        [Theory]
        [InlineData(HttpStatusCode.Forbidden)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.ServiceUnavailable)]
        public void GetAsync_Returns_Null_On_Non_200_Response_From_MusicBrainz(HttpStatusCode httpStatusCode)
        {
            var mbid = _fixture.Create<string>();
            var httpResponseMessage = new HttpResponseMessage().PopulatedWith(httpStatusCode, CreateExampleJsonResponse(mbid));
            _mockedHttpClient.Setup(m => m.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(httpResponseMessage));

            var artist = _sut.GetAsync(mbid).Result;

            Assert.Null(artist);
        }

        [Theory]
        [InlineData(HttpStatusCode.Forbidden)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.ServiceUnavailable)]
        public void GetAsync_Logs_Error_On_Non_200_Response_From_MusicBrainz(HttpStatusCode httpStatusCode)
        {
            var mbid = _fixture.Create<string>();
            var httpResponseMessage = new HttpResponseMessage().PopulatedWith(httpStatusCode, CreateExampleJsonResponse(mbid));
            _mockedHttpClient.Setup(m => m.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(httpResponseMessage));

            _sut.GetAsync(mbid).Wait();

            _mockedLogger.Verify(m => m.Log(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void GetAsync_Deserializes_Json_Response_From_MusicBrainz_Into_Artist()
        {
            var mbid = _fixture.Create<string>();
            var httpResponseMessage = new HttpResponseMessage().PopulatedWith(HttpStatusCode.OK, CreateExampleJsonResponse(mbid));
            _mockedHttpClient.Setup(m => m.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(httpResponseMessage));

            var artist = _sut.GetAsync(mbid).Result;

            Assert.NotNull(artist);
        }

        private static string CreateExampleJsonResponse(string mbid)
        {
            return "{\"id\":\"" + mbid + "\",\"gender\":null,\"name\":\"Nirvana\",\"life-span\":null,\"type\":\"Group\",\"release-groups\":[{\"first-release-date\":\"1995\",\"id\":\"ff9dec8b-3674-35a3-aa39-9f9ba3d30b71\",\"title\":\"Twilight of the Gods\",\"primary-type\":\"Album\"}],\"sort-name\":\"Nirvana\"}";
        }
    }
}
