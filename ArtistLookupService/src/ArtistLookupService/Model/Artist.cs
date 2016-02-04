using System.Collections.Generic;
using Newtonsoft.Json;

namespace ArtistLookupService.Model
{
    public class Artist
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "release-groups")]
        public List<Album> Albums { get; set; }

        [JsonIgnore]
        public string WikipediaUri { get; set; }
    }
}
