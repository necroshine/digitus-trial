using System;
using Digitus.Trial.Backend.Api.Security.Models;

namespace Digitus.Trial.Backend.Api.Security.Providers
{
    public class UserDatabaseProvider : MongoBaseDatabaseProvider<User>
    {
        public UserDatabaseProvider(string connectionString) : base(connectionString)
        {
        }
    }
}
