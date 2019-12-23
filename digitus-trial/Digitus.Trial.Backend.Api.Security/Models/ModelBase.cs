using System;
using Digitus.Trial.Backend.Api.Security.Attributes;
using Digitus.Trial.Backend.Api.Security.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Digitus.Trial.Backend.Api.Models
{
    public class ModelBase : IModelBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string _id { get; set; }

        [IdentityField]
        public int Id { get; set; }
    }
}
