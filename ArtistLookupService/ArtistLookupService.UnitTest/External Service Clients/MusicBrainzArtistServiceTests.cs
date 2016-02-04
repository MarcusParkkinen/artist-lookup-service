using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ArtistLookupService.External_Service_Clients;
using ArtistLookupService.External_Service_Interfaces;
using ArtistLookupService.Logging;
using ArtistLookupService.UnitTest.Extensions;
using ArtistLookupService.Wrappers;
using FluentAssertions;
using Moq;
using Ploeh.AutoFixture;
using Xunit;

namespace ArtistLookupService.UnitTest.External_Service_Clients
{
    public class MusicBrainzArtistServiceTests
    {
        private readonly MusicBrainzService _sut;
        private readonly Fixture _fixture;
        private readonly Mock<IHttpClientWrapper> _mockedHttpClient;
        private readonly Mock<IErrorLogger> _mockedLogger;
        private readonly Mock<IDescriptionService> _mockedDescriptionService;
        private readonly Mock<ICoverArtUrlService> _mockedCoverArtUrlService;
        private readonly string _mbid;

        public MusicBrainzArtistServiceTests()
        {
            _fixture = new Fixture();

            _mockedHttpClient = new Mock<IHttpClientWrapper>();
            _mockedCoverArtUrlService = new Mock<ICoverArtUrlService>();
            _mockedDescriptionService = new Mock<IDescriptionService>();
            _mockedLogger = new Mock<IErrorLogger>();
            _mbid = _fixture.Create<string>();

            SetupDescriptionServiceMockResponse();
            SetupCoverArtUrlServiceMockResponse();
            SetupHttpClientMockResponse(HttpStatusCode.OK, _mbid);

            _sut = new MusicBrainzService(_mockedHttpClient.Object,
                _mockedDescriptionService.Object,
                _mockedCoverArtUrlService.Object,
                _mockedLogger.Object);
        }

        [Theory]
        [InlineData(HttpStatusCode.Forbidden)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.ServiceUnavailable)]
        public void GetAsync_Returns_Null_On_Non_200_Response_From_MusicBrainz(HttpStatusCode httpStatusCode)
        {
            SetupHttpClientMockResponse(httpStatusCode, _mbid);

            var artist = _sut.GetAsync(_mbid).Result;

            artist.Should().BeNull();
        }

        [Theory]
        [InlineData(HttpStatusCode.Forbidden)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.ServiceUnavailable)]
        public void GetAsync_Logs_Error_On_Non_200_Response_From_MusicBrainz(HttpStatusCode httpStatusCode)
        {
            SetupHttpClientMockResponse(httpStatusCode, _mbid);

            _sut.GetAsync(_mbid).Wait();

            _mockedLogger.Verify(m => m.Log(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void GetAsync_Deserializes_Json_Response_From_MusicBrainz_Into_Artist()
        {
            var artist = _sut.GetAsync(_mbid).Result;

            artist.Should().NotBeNull();
        }

        [Fact]
        public void GetAsync_Populates_The_Artist_Description_With_Result_From_DescriptionService()
        {
            var artist = _sut.GetAsync(_mbid).Result;

            artist.Description.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GetAsync_Uses_Null_As_Description_On_Exception_From_DescriptionService()
        {
            _mockedDescriptionService.Setup(m => m.GetAsync(It.IsAny<string>())).Throws<Exception>();

            var artist = _sut.GetAsync(_mbid).Result;

            artist.Description.Should().BeNull();
        }

        [Fact]
        public void GetAsync_Populates_Cover_Art_Uris()
        {
            var artist = _sut.GetAsync(_mbid).Result;

            artist.Albums.ForEach(album => album.CoverArtUrl.Should().NotBeNullOrEmpty());
        }

        [Fact]
        public void GetAsync_Uses_Null_As_Cover_Art_Uri_On_Exception_From_CoverArtUriService()
        {
            _mockedCoverArtUrlService.Setup(m => m.GetAsync(It.IsAny<string>())).Throws<Exception>();

            var artist = _sut.GetAsync(_mbid).Result;

            artist.Albums.ForEach(album => album.CoverArtUrl.Should().BeNull());
        }

        [Fact]
        public void GetAsync_Logs_Errors_Occurring_When_Attempting_To_Populate_Album_Cover_Art_Uris()
        {
            _mockedCoverArtUrlService.Setup(m => m.GetAsync(It.IsAny<string>())).Throws<Exception>();

            var artist = _sut.GetAsync(_mbid).Result;

            _mockedLogger.Verify(m => m.Log(It.IsAny<string>()), Times.Exactly(artist.Albums.Count));
        }

        private void SetupDescriptionServiceMockResponse()
        {
            var description = _fixture.Create<string>();
            _mockedDescriptionService.Setup(m => m.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(description));
        }

        private void SetupHttpClientMockResponse(HttpStatusCode httpStatusCode, string mbid)
        {
            var httpResponseMessage = new HttpResponseMessage().PopulatedWith(httpStatusCode, CreateExampleJsonResponse(mbid));
            _mockedHttpClient.Setup(m => m.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(httpResponseMessage));
        }

        private void SetupCoverArtUrlServiceMockResponse()
        {
            var coverArtUrl = _fixture.Create<string>();
            _mockedCoverArtUrlService.Setup(m => m.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(coverArtUrl));
        }

        private static string CreateExampleJsonResponse(string mbid)
        {
            return "{\"id\":\"" + mbid + "\",\"gender\":null,\"name\":\"Nirvana\",\"life-span\":null,\"type\":\"Group\",\"release-groups\":[{\"first-release-date\":\"1995\",\"id\":\"ff9dec8b-3674-35a3-aa39-9f9ba3d30b71\",\"title\":\"Twilight of the Gods\",\"primary-type\":\"Album\"}],\"sort-name\":\"Nirvana\"}";
        }
    }
}
