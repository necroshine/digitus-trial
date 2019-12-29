using System;
using System.Threading.Tasks;
using Digitus.Trial.Backend.Api.Interfaces;
using Digitus.Trial.Backend.Api.Models;

namespace Digitus.Trial.Backend.Api.Managers
{
    public class MockManager :IMockManager
    {
        IDatabaseProvider<User> _userDatabaseProvider;
        IDatabaseProvider<UserLog> _userLogDatabaseProvider;
        IUserManager _userManager;
        IPasswordProvider _passwordProvider;

        public MockManager(
            IUserManager userManager,
            IDatabaseProvider<User> userDatabaseProvider,
            IDatabaseProvider<UserLog> userLogDatabaseProvider,
            IPasswordProvider passwordProvider
            )
        {
            _userManager = userManager;
            _userDatabaseProvider = userDatabaseProvider;
            _userLogDatabaseProvider = userLogDatabaseProvider;
            _passwordProvider = passwordProvider;
        }

        public async Task<string> GenerateUserDataForTest()
        {
            Random rnd = new Random();
            

            for (int i = 0; i < 100; i++)
            {
                var date = DateTime.UtcNow.AddDays(rnd.Next(3) * -1);
                var item = new User()
                {
                    Email = $"serhatyalcin{i}@gmail.com",
                    Password = await _passwordProvider.EncryptPassword("1234"),
                    ActivationCode = await _passwordProvider.GenerateActivationCode(),
                    FirstName = "serhat",
                    LastName = "yalcin",
                    UserName = $"serhatyalcin{i}",
                    CreateDate = date,
                    ActivationCodeSentDate = date,
                    Status = i % 2 == 0 ? Enums.Statuses.PendingAcitivation : Enums.Statuses.Active,
                };
                await _userDatabaseProvider.Add(item);
            }
            return "ok";
        }

        public async Task<string> GenerateUserLogDataForTest()
        {
            Random rnd = new Random();
            for (int i = 0; i < 1000; i++)
            {
                var item = new UserLog()
                {
                    CreateDate = DateTime.UtcNow.AddDays(rnd.Next(3)),
                    Duration = rnd.Next(1000),
                    Operation = Enums.UserOperations.Login,
                    UserId = Guid.NewGuid()

                };
                await _userLogDatabaseProvider.Add(item);
            }
            return "ok";
        }
    }
}
