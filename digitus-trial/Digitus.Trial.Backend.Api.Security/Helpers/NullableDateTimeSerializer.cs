using System;
using System.Resources;
using MongoDB.Bson.Serialization;

namespace Digitus.Trial.Backend.Api.Helpers
{
    public class NullableDateTimeSerializer<TDateTime>: IBsonSerializer
    {
        public NullableDateTimeSerializer()
        {
            if (typeof(TDateTime) != typeof(DateTime) && typeof(TDateTime) != typeof(DateTime?))
            {
                throw new InvalidOperationException($"MyCustomDateTimeSerializer could be used only with {nameof(DateTime)} or {nameof(Nullable<DateTime>)}");
            }
        }

        public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            // Deserialization logic
            return null;
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            // Serialization logic
        }

        public Type ValueType => typeof(TDateTime);
    }

    
}
