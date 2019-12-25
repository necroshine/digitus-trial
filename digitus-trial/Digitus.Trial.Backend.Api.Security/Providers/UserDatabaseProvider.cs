using System;
using Digitus.Trial.Backend.Api.Models;

namespace Digitus.Trial.Backend.Api.Providers
{
    public class UserDatabaseProvider : MongoBaseDatabaseProvider<User>
    {
        public UserDatabaseProvider(string connectionString) : base(connectionString)
        {
        }
    }
}
