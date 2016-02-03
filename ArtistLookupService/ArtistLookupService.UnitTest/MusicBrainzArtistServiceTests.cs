using ArtistLookupService.External_Service_Clients;
using ArtistLookupService.External_Service_Interfaces;
using ArtistLookupService.Wrappers;
using Moq;
using Ploeh.AutoFixture;

namespace ArtistLookupService.UnitTest
{
    public class MusicBrainzArtistServiceTests
    {
        private readonly MusicBrainzService _sut;
        private readonly Fixture _fixture;

        public MusicBrainzArtistServiceTests()
        {
            _fixture = new Fixture();

            var mockedHttpClient = new Mock<IHttpClientWrapper>();
            var mockedDescriptionService = new Mock<IDescriptionService>();
            var mockedCoverArtUrlService = new Mock<ICoverArtUrlService>();

            _sut = new MusicBrainzService(mockedHttpClient.Object, mockedDescriptionService.Object, mockedCoverArtUrlService.Object);
        }
    }
}
