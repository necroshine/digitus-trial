using System;
using System.Text.Json.Serialization;
using Digitus.Trial.Backend.Api.Security.Attributes;
using Digitus.Trial.Backend.Api.Security.Enums;
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
        
        public DateTime CreateDate { get; set; }
        public UserStatuses Status { get; set; }
        public int ActivationDate { get; set; }
        public string ActivationCode { get; set; }

    }
}
