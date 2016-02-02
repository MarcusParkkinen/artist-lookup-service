using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ArtistLookupService.External_Services;
using Moq;
using Moq.Protected;
using Ploeh.AutoFixture;
using Xunit;

namespace ArtistLookupService.UnitTest
{
    public class MusicBrainzArtistServiceTests
    {
        private readonly MusicBrainzArtistService _sut;
        private readonly Fixture _fixture;

        public MusicBrainzArtistServiceTests()
        {
            _fixture = new Fixture();

            var mockedHttpClient = new Mock<HttpClient>();
            var mockedDescriptionService = new Mock<IDescriptionService>();
            var mockedCoverArtUrlService = new Mock<ICoverArtUrlService>();

            _sut = new MusicBrainzArtistService(mockedHttpClient.Object, mockedDescriptionService.Object,
                mockedCoverArtUrlService.Object);
        }
    }
}
