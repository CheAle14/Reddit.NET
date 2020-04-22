using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reddit
{
    /// <summary>
    /// For some reason the active_user_count and accounts_active on SubredditData returned as an object '{}' from reddit, rather than null.
    /// </summary>
    public class IntWorkaround : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(int?);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if(reader.TokenType == JsonToken.StartObject)
            {
                do
                {
                    reader.Read();
                } while (reader.TokenType != JsonToken.EndObject);
                return null;
            }
            if (reader.TokenType == JsonToken.Null)
                return null;
            return Convert.ToInt32((long)reader.Value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var jval = new JValue(value);
            jval.WriteTo(writer);
        }
    }
}
