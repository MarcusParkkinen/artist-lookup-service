using System;
using ArtistLookupService.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ArtistLookupService.Converters
{
    public class ArtistConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            /*
             * Not currently used, and not implemented due to time restrictions! Would be implemented
             * in a "real world" scenario.
             */
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof (Artist);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var artist = jObject.ToObject<Artist>();

            var wikipediaUrlToken = jObject.SelectToken(JPathToWikipediaUrl);

            if (wikipediaUrlToken != null)
            {
                artist.WikipediaUrl = wikipediaUrlToken.ToString();
            }

            return artist;
        }

        public string JPathToWikipediaUrl => "$..relations[?(@.type=='wikipedia')].url.resource";
    }
}
