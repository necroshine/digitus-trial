using System;
using Digitus.Trial.Backend.Api.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Digitus.Trial.Backend.Api.Providers
{
    public class UserDatabaseProvider : MongoBaseDatabaseProvider<User>
    {
        public UserDatabaseProvider(string connectionString) : base(connectionString)
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(User)))
            {
                BsonClassMap.RegisterClassMap<User>(cm =>
                {
                    cm.AutoMap();
                    cm.MapMember(c => c.ActivationDate).SetSerializer(new DateTimeSerializer(true));
                    cm.MapMember(c => c.CreateDate).SetSerializer(new DateTimeSerializer(true));
                    cm.MapMember(c => c.ActivationCodeSentDate).SetSerializer(new DateTimeSerializer(true));
                });
            }
        }
    }
}
