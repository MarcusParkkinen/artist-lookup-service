﻿using System;
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
    public class WikipediaDescriptionServiceTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IHttpClientWrapper> _mockedHttpClient;
        private readonly Mock<IErrorLogger> _mockedLogger;

        private readonly WikipediaDescriptionService _sut;

        public WikipediaDescriptionServiceTests()
        {
            _fixture = new Fixture();

            _mockedHttpClient = new Mock<IHttpClientWrapper>();
            _mockedLogger = new Mock<IErrorLogger>();

            _sut = new WikipediaDescriptionService(_mockedHttpClient.Object, _mockedLogger.Object);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GetAsync_Returns_Null_When_Provided_With_Invalid_Uri(string invalidWikipediaDescriptionUri)
        {
            var description = _sut.GetAsync(invalidWikipediaDescriptionUri).Result;

            description.Should().BeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GetAsync_Logs_An_Error_If_Provided_Uri_Is_Invalid(string invalidWikipediaDescriptionUri)
        {
            _sut.GetAsync(invalidWikipediaDescriptionUri).Wait();

            _mockedLogger.Verify(m => m.Log(It.IsAny<Exception>()), Times.Once());
        }

        [Theory]
        [InlineData(HttpStatusCode.Forbidden)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.ServiceUnavailable)]
        public void GetAsync_Returns_Null_On_Non_200_Response_From_Wikipedia(HttpStatusCode httpStatusCode)
        {
            var uri = _fixture.Create<string>();
            SetupHttpClientMockResponse(httpStatusCode, ExampleJsonResponse);

            var description = _sut.GetAsync(uri).Result;

            description.Should().BeNull();
        }

        [Theory]
        [InlineData(HttpStatusCode.Forbidden)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.ServiceUnavailable)]
        public void GetAsync_Logs_Error_On_Non_200_Response_From_Wikipedia(HttpStatusCode httpStatusCode)
        {
            var uri = _fixture.Create<string>();
            SetupHttpClientMockResponse(httpStatusCode, ExampleJsonResponse);

            _sut.GetAsync(uri).Wait();

            _mockedLogger.Verify(m => m.Log(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void GetAsync_Deserializes_Json_Response_From_Wikipedia_Into_Description()
        {
            var uri = _fixture.Create<string>();
            SetupHttpClientMockResponse(HttpStatusCode.OK, ExampleJsonResponse);

            var description = _sut.GetAsync(uri).Result;

            description.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GetAsync_Returns_Null_If_Deserialization_Of_Response_Json_Fails()
        {
            var uri = _fixture.Create<string>();
            var invalidJsonResponse = _fixture.Create<string>();
            SetupHttpClientMockResponse(HttpStatusCode.OK, invalidJsonResponse);

            var description = _sut.GetAsync(uri).Result;

            description.Should().BeNull();
        }

        private void SetupHttpClientMockResponse(HttpStatusCode httpStatusCode, string jsonResponse)
        {
            var httpResponseMessage = new HttpResponseMessage().PopulatedWith(httpStatusCode, jsonResponse);
            _mockedHttpClient.Setup(m => m.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(httpResponseMessage));
        }

        private static string ExampleJsonResponse => "{\"batchcomplete\":\"\",\"query\":{\"normalized\":[{\"from\":\"Nirvana_(band)\",\"to\":\"Nirvana (band)\"}],\"pages\":{\"21231\":{\"pageid\":21231,\"ns\":0,\"title\":\"Nirvana (band)\",\"extract\":\"<p><b>Nirvana</b></p>\"}}}}";
    }
}
