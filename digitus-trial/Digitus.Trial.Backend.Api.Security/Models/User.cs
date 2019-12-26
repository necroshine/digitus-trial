using System;
using System.Text.Json.Serialization;
using Digitus.Trial.Backend.Api.Attributes;
using Digitus.Trial.Backend.Api.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Digitus.Trial.Backend.Api.Models
{
    [Model(CollectionName = "Users")]
    public class User
    {
        [JsonIgnore]
        [BsonId]
        public ObjectId _Id { get; set; }

        [IdentityField(IdentityFieldType = IdentityFieldTypes.UID)]
        public Guid Id { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }       
        public UserStatuses UserStatus { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateDate { get; set; }
        public Statuses Status { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ActivationDate { get; set; }
        public string ActivationCode { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ActivationCodeSentDate { get; set; }

    }
}
