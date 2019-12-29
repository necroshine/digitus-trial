using System;
using System.Text.Json.Serialization;
using Digitus.Trial.Backend.Api.Attributes;
using Digitus.Trial.Backend.Api.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Digitus.Trial.Backend.Api.Models
{
    [Model(CollectionName ="UserLogs")]
    public class UserLog
    {
        [JsonIgnore]
        [BsonId]
        public ObjectId _Id { get; set; }

        [IdentityField(IdentityFieldType = IdentityFieldTypes.UID)]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public DateTime CreateDate { get; set; }
        public UserOperations Operation { get; set; }
        public long Duration { get; set; }
    }
}
