using System;
using Digitus.Trial.Backend.Api.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Digitus.Trial.Backend.Api.Providers
{
    public class UserLogDatabaseProvider:MongoBaseDatabaseProvider<UserLog>
    {
        public UserLogDatabaseProvider(string connectionString) : base(connectionString)
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(UserLog)))
            {
                BsonClassMap.RegisterClassMap<UserLog>(cm =>
                {
                    cm.AutoMap();
                    cm.MapMember(c => c.CreateDate).SetSerializer(new DateTimeSerializer(true));
                });
            }
        }
    }
}
