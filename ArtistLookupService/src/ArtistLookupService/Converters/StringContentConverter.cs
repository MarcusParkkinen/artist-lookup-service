﻿using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ArtistLookupService.Converters
{
    public abstract class StringContentConverter : JsonConverter
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
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var content = jObject.SelectToken(JPathToResource);

            return content?.ToString();
        }

        public abstract string JPathToResource { get; }
    }
}
