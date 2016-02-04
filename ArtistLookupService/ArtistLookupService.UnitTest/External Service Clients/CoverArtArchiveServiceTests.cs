using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ArtistLookupService.External_Service_Clients;
using ArtistLookupService.Logging;
using ArtistLookupService.UnitTest.Extensions;
using ArtistLookupService.Wrappers;
using FluentAssertions;
using Moq;
using Ploeh.AutoFixture;
using Xunit;

namespace ArtistLookupService.UnitTest.External_Service_Clients
{
    public class CoverArtArchiveServiceTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IHttpClientWrapper> _mockedHttpClient;
        private readonly Mock<IErrorLogger> _mockedLogger;

        private readonly CoverArtArchiveService _sut;
        private string _albumId;

        public CoverArtArchiveServiceTests()
        {
            _fixture = new Fixture();

            _albumId = _fixture.Create<string>();
            _mockedHttpClient = new Mock<IHttpClientWrapper>();
            _mockedLogger = new Mock<IErrorLogger>();

            SetupHttpClientMockResponse(HttpStatusCode.OK, ExampleJsonResponse);

            _sut = new CoverArtArchiveService(_mockedHttpClient.Object, _mockedLogger.Object);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GetAsync_Returns_Null_When_Provided_With_A_Non_Existing_Album_Id(string invalidAlbumId)
        {
            var coverArtUri = _sut.GetAsync(invalidAlbumId).Result;

            coverArtUri.Should().BeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GetAsync_Logs_An_Error_If_Provided_Album_Id_Is_Invalid(string invalidAlbumId)
        {
            _sut.GetAsync(invalidAlbumId).Wait();

            _mockedLogger.Verify(m => m.Log(It.IsAny<Exception>()), Times.Once());
        }

        [Fact]
        public void GetAsync_Deserializes_Json_Response_From_Cover_Art_Archive_Into_Album_Cover_Art_Uri()
        {
            var coverArtUri = _sut.GetAsync(_albumId).Result;

            coverArtUri.Should().NotBeNullOrEmpty();
        }

        [Theory]
        [InlineData(HttpStatusCode.Forbidden)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.ServiceUnavailable)]
        public void GetAsync_Returns_Null_On_Non_200_Response_From_Cover_Art_Archive(HttpStatusCode httpStatusCode)
        {
            var albumId = _fixture.Create<string>();
            SetupHttpClientMockResponse(httpStatusCode, ExampleJsonResponse);

            var coverArtUrl = _sut.GetAsync(albumId).Result;

            coverArtUrl.Should().BeNull();
        }

        [Theory]
        [InlineData(HttpStatusCode.Forbidden)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.ServiceUnavailable)]
        public void GetAsync_Logs_Error_On_Non_200_Response_From_Cover_Art_Archive(HttpStatusCode httpStatusCode)
        {
            var albumId = _fixture.Create<string>();
            SetupHttpClientMockResponse(httpStatusCode, ExampleJsonResponse);

            _sut.GetAsync(albumId).Wait();

            _mockedLogger.Verify(m => m.Log(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void GetAsync_Returns_Null_If_Deserialization_Of_Response_Json_Fails()
        {
            var albumId = _fixture.Create<string>();
            var invalidJsonResponse = _fixture.Create<string>();
            SetupHttpClientMockResponse(HttpStatusCode.OK, invalidJsonResponse);

            var description = _sut.GetAsync(albumId).Result;

            description.Should().BeNull();
        }

        private void SetupHttpClientMockResponse(HttpStatusCode httpStatusCode, string jsonResponse)
        {
            var httpResponseMessage = new HttpResponseMessage().PopulatedWith(httpStatusCode, jsonResponse);
            _mockedHttpClient.Setup(m => m.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(httpResponseMessage));
        }

        private static string ExampleJsonResponse =>  "{\"images\":[{\"types\":[\"Front\"],\"front\":true,\"back\":false,\"edit\":31151246,\"image\":\"http://coverartarchive.org/release/93666902-58d7-4792-9748-9a66fdab0ea6/9422498754.jpg\",\"comment\":\"\",\"approved\":true,\"id\":\"9422498754\",\"thumbnails\":{\"large\":\"http://coverartarchive.org/release/93666902-58d7-4792-9748-9a66fdab0ea6/9422498754-500.jpg\",\"small\":\"http://coverartarchive.org/release/93666902-58d7-4792-9748-9a66fdab0ea6/9422498754-250.jpg\"}}],\"release\":\"http://musicbrainz.org/release/93666902-58d7-4792-9748-9a66fdab0ea6\"}";
    }
}
