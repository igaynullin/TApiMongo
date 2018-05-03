using System;
using Newtonsoft.Json;

namespace TApiMongo.Data.JsonConverters
{
    public class LongJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var longValue = (long)value;
            writer.WriteRawValue($"NumberLong(\"{longValue}\")");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(long);
        }
    }
}