using System.Collections.Generic;

namespace ArtistLookupService.Domain
{
    public class Artist
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public List<Album> Albums { get; set; }
    }
}
