using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ArtistLookupService.External_Service_Clients;
using ArtistLookupService.External_Service_Interfaces;
using Moq;
using Moq.Protected;
using Ploeh.AutoFixture;
using Xunit;

namespace ArtistLookupService.UnitTest
{
    public class MusicBrainzArtistServiceTests
    {
        private readonly MusicBrainzService _sut;
        private readonly Fixture _fixture;

        public MusicBrainzArtistServiceTests()
        {
            _fixture = new Fixture();

            var mockedDescriptionService = new Mock<IDescriptionService>();
            var mockedCoverArtUrlService = new Mock<ICoverArtUrlService>();

            _sut = new MusicBrainzService(mockedDescriptionService.Object, mockedCoverArtUrlService.Object);
        }
    }
}
