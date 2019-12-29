using System;
using System.Threading.Tasks;
using Digitus.Trial.Backend.Api.ApiModels;
using Digitus.Trial.Backend.Api.Controllers;
using Digitus.Trial.Backend.Api.Interfaces;
using Digitus.Trial.Backend.Api.Managers;
using Digitus.Trial.Backend.Api.Models;
using Digitus.Trial.Backend.Api.Test;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace Digitus.Trial.Backend.Api.Security.Test
{
    public class SecurityControllerTests : IDisposable
    {
        FakeObjectFactory fakeFactory;
        public SecurityControllerTests()
        {
            fakeFactory = new FakeObjectFactory();
        }

        [Fact]
        public void AuthenticationSuccess()
        {
            using (var mock = Autofac.Extras.Moq.AutoMock.GetLoose())
            {
                var userLogDatabaseProvider = mock.Mock<IDatabaseProvider<UserLog>>();
                userLogDatabaseProvider.Setup(s => s.Add(It.IsAny<UserLog>())).ReturnsAsync(fakeFactory.GetUserLog());
                var userManager = mock.Mock<IUserManager>();
                userManager.Setup(m => m.GetUserByUserName(It.IsAny<string>())).ReturnsAsync(fakeFactory.GetUserByUserName("serhatyalcin", Enums.Statuses.Active));
                var passwordProvider = mock.Mock<IPasswordProvider>();
                passwordProvider.Setup(m => m.DecryptPassword(It.IsAny<string>())).ReturnsAsync(() => { return "1234"; });

                var configSection = mock.Mock<IConfigurationSection>();
                configSection.Setup(x => x.Value).Returns("2562d232dcg213fWEW!@");
                var configuration = mock.Mock<IConfiguration>();
                configuration.Setup(m => m.GetSection(It.IsAny<string>())).Returns(() => { return configSection.Object; });

                var notificationManager = mock.Mock<INotificationManager>();


                var authenticationManager =
                    new AuthenticationManager(userManager.Object,
                    passwordProvider.Object,
                    configuration.Object,
                    notificationManager.Object,
                    userLogDatabaseProvider.Object);

                var model = new AuthenticationRequestModel()
                {
                    UserName = "serhatyalcin",
                    Password = "1234"
                };

                var task = authenticationManager.Authenticate(model);
                var result = task.GetAwaiter().GetResult();

                Assert.True((result.isAuthenticated && result.CurrentUser != null
                    && !string.IsNullOrEmpty(result.CurrentUser.Token)));
            }
        }

        [Fact]
        public void AuthenticationFailedDueUserNotFound()
        {
            using (var mock = Autofac.Extras.Moq.AutoMock.GetLoose())
            {
                var userLogDatabaseProvider = mock.Mock<IDatabaseProvider<UserLog>>();
                userLogDatabaseProvider.Setup(s => s.Add(It.IsAny<UserLog>())).ReturnsAsync(fakeFactory.GetUserLog());
                var userManager = mock.Mock<IUserManager>();
                userManager.Setup(m => m.GetUserByUserName(It.IsAny<string>())).ReturnsAsync(() => { return default; });
                var passwordProvider = mock.Mock<IPasswordProvider>();
                passwordProvider.Setup(m => m.DecryptPassword(It.IsAny<string>())).ReturnsAsync(() => { return "1234"; });
                
                var notificationManager = mock.Mock<INotificationManager>();

                var configSection = mock.Mock<IConfigurationSection>();
                configSection.Setup(x => x.Value).Returns("2562d232dcg213fWEW!@");
                var configuration = mock.Mock<IConfiguration>();
                configuration.Setup(m => m.GetSection(It.IsAny<string>())).Returns(() => { return configSection.Object; });

                var authenticationManager =
                    new AuthenticationManager(userManager.Object,
                    passwordProvider.Object,
                    configuration.Object,
                    notificationManager.Object,
                    userLogDatabaseProvider.Object);

                var model = new AuthenticationRequestModel()
                {
                    UserName = "serhatyalcin",
                    Password = "1234"
                };

                var task = authenticationManager.Authenticate(model);
                var result = task.GetAwaiter().GetResult();

                Assert.True((!result.isAuthenticated && result.CurrentUser == null && result.Message == "User not found"));
            }
        }

        [Fact]
        public void AuthenticationFailedDueAccountNotActivated()
        {
            using (var mock = Autofac.Extras.Moq.AutoMock.GetLoose())
            {
                var userLogDatabaseProvider = mock.Mock<IDatabaseProvider<UserLog>>();
                userLogDatabaseProvider.Setup(s => s.Add(It.IsAny<UserLog>())).ReturnsAsync(fakeFactory.GetUserLog());
                var userManager = mock.Mock<IUserManager>();
                userManager.Setup(m => m.GetUserByUserName(It.IsAny<string>())).ReturnsAsync(fakeFactory.GetUserByUserName("serhatyalcin", Enums.Statuses.PendingAcitivation));

                var passwordProvider = mock.Mock<IPasswordProvider>();
                passwordProvider.Setup(m => m.DecryptPassword(It.IsAny<string>())).ReturnsAsync(() => { return "1234"; });
                
                var notificationManager = mock.Mock<INotificationManager>();

                var configSection = mock.Mock<IConfigurationSection>();
                configSection.Setup(x => x.Value).Returns("2562d232dcg213fWEW!@");
                var configuration = mock.Mock<IConfiguration>();
                configuration.Setup(m => m.GetSection(It.IsAny<string>())).Returns(() => { return configSection.Object; });

                var authenticationManager =
                    new AuthenticationManager(userManager.Object,
                    passwordProvider.Object,
                    configuration.Object,
                    notificationManager.Object,
                    userLogDatabaseProvider.Object);

                var model = new AuthenticationRequestModel()
                {
                    UserName = "serhatyalcin",
                    Password = "1234"
                };

                var task = authenticationManager.Authenticate(model);
                var result = task.GetAwaiter().GetResult();

                Assert.True((!result.isAuthenticated && result.CurrentUser == null && result.Message == "Account pending activation."));
            }
        }

        [Fact]
        public void AuthenticationFailedDueInvalidCredential()
        {
            using (var mock = Autofac.Extras.Moq.AutoMock.GetLoose())
            {
                var userLogDatabaseProvider = mock.Mock<IDatabaseProvider<UserLog>>();
                var userManager = mock.Mock<IUserManager>();
                userManager.Setup(m => m.GetUserByUserName(It.IsAny<string>())).ReturnsAsync(fakeFactory.GetUserByUserName("serhatyalcin", Enums.Statuses.Active));
                var passwordProvider = mock.Mock<IPasswordProvider>();
                passwordProvider.Setup(m => m.DecryptPassword(It.IsAny<string>())).ReturnsAsync(() => { return "1234"; });
                var configuration = mock.Mock<IConfiguration>();
                var notificationManager = mock.Mock<INotificationManager>();


                var authenticationManager =
                    new AuthenticationManager(userManager.Object,
                    passwordProvider.Object,
                    configuration.Object,
                    notificationManager.Object,
                    userLogDatabaseProvider.Object);

                var model = new AuthenticationRequestModel()
                {
                    UserName = "serhatyalcin",
                    Password = "12345"
                };

                var task = authenticationManager.Authenticate(model);
                var result = task.GetAwaiter().GetResult();

                Assert.True((!result.isAuthenticated && result.CurrentUser == null && result.Message == "Invalid credentials"));
            }
        }

        [Fact]
        public async Task AuthenticatioFailedDueUsernameNullValidation()
        {
            using (var mock = Autofac.Extras.Moq.AutoMock.GetLoose())
            {
                var userLogDatabaseProvider = mock.Mock<IDatabaseProvider<UserLog>>();
                var userManager = mock.Mock<IUserManager>();
                var passwordProvider = mock.Mock<IPasswordProvider>();
                var configuration = mock.Mock<IConfiguration>();
                var notificationManager = mock.Mock<INotificationManager>();


                var authenticationManager =
                    new AuthenticationManager(userManager.Object,
                    passwordProvider.Object,
                    configuration.Object,
                    notificationManager.Object,
                    userLogDatabaseProvider.Object);

                var model = new AuthenticationRequestModel()
                {
                    UserName = default,
                    Password = "1234"
                };

                var controller = new SecurityController(authenticationManager);

                await Assert.ThrowsAsync<ArgumentNullException>(() => controller.Authenticate(model)).ConfigureAwait(false);
            }
        }

        [Fact]
        public async Task AuthenticatioFailedDuePasswordNullValidation()
        {
            using (var mock = Autofac.Extras.Moq.AutoMock.GetLoose())
            {
                var userLogDatabaseProvider = mock.Mock<IDatabaseProvider<UserLog>>();
                var userManager = mock.Mock<IUserManager>();
                var passwordProvider = mock.Mock<IPasswordProvider>();
                var configuration = mock.Mock<IConfiguration>();
                var notificationManager = mock.Mock<INotificationManager>();


                var authenticationManager =
                    new AuthenticationManager(userManager.Object,
                    passwordProvider.Object,
                    configuration.Object,
                    notificationManager.Object,
                    userLogDatabaseProvider.Object);

                var model = new AuthenticationRequestModel()
                {
                    UserName = "serhat",
                    Password = default
                };

                var controller = new SecurityController(authenticationManager);

                await Assert.ThrowsAsync<ArgumentNullException>(() => controller.Authenticate(model)).ConfigureAwait(false);
            }
        }


        [Fact]
        public void VerifyUserSuccess()
        {
            using (var mock = Autofac.Extras.Moq.AutoMock.GetLoose())
            {
                Guid activationCode = Guid.NewGuid();
                var userLogDatabaseProvider = mock.Mock<IDatabaseProvider<UserLog>>();                
                var userManager = mock.Mock<IUserManager>();
                userManager.Setup(m => m.GetUserByActivationCode(It.IsAny<string>()))
                    .ReturnsAsync(fakeFactory.GetUserByActivationCode(activationCode.ToString()));
                var passwordProvider = mock.Mock<IPasswordProvider>();
                passwordProvider.Setup(m => m.DecryptPassword(It.IsAny<string>()))
                    .ReturnsAsync(() => { return "1234"; });

                var configSection = mock.Mock<IConfigurationSection>();
                configSection.Setup(x => x.Value).Returns("2562d232dcg213fWEW!@");
                var configuration = mock.Mock<IConfiguration>();
                configuration.Setup(m => m.GetSection(It.IsAny<string>())).Returns(() => { return configSection.Object; });

                var notificationManager = mock.Mock<INotificationManager>();


                var authenticationManager =
                    new AuthenticationManager(userManager.Object,
                    passwordProvider.Object,
                    configuration.Object,
                    notificationManager.Object,
                    userLogDatabaseProvider.Object);

                

                var task = authenticationManager.VerifyUser(activationCode.ToString());
                var result = task.GetAwaiter().GetResult();

                Assert.True((result.IsVerified && result.Message == "Verification is success"));
            }
        }
        [Fact]
        public void VerifyUserFailedDueAlreadyVerifiedUser()
        {
            using (var mock = Autofac.Extras.Moq.AutoMock.GetLoose())
            {
                Guid activationCode = Guid.NewGuid();
                var userLogDatabaseProvider = mock.Mock<IDatabaseProvider<UserLog>>();
                var userManager = mock.Mock<IUserManager>();
                userManager.Setup(m => m.GetUserByActivationCode(It.IsAny<string>()))
                    .ReturnsAsync(fakeFactory.GetUserByActivationCode(activationCode.ToString(),Enums.Statuses.Active));
                var passwordProvider = mock.Mock<IPasswordProvider>();
                passwordProvider.Setup(m => m.DecryptPassword(It.IsAny<string>()))
                    .ReturnsAsync(() => { return "1234"; });

                var configSection = mock.Mock<IConfigurationSection>();
                configSection.Setup(x => x.Value).Returns("2562d232dcg213fWEW!@");
                var configuration = mock.Mock<IConfiguration>();
                configuration.Setup(m => m.GetSection(It.IsAny<string>())).Returns(() => { return configSection.Object; });

                var notificationManager = mock.Mock<INotificationManager>();


                var authenticationManager =
                    new AuthenticationManager(userManager.Object,
                    passwordProvider.Object,
                    configuration.Object,
                    notificationManager.Object,
                    userLogDatabaseProvider.Object);



                var task = authenticationManager.VerifyUser(activationCode.ToString());
                var result = task.GetAwaiter().GetResult();

                Assert.True((!result.IsVerified && result.Message == "User Account already verified."));
            }
        }
        [Fact]
        public void VerifyUserFailedDueInvalidVerificationCode()
        {
            using (var mock = Autofac.Extras.Moq.AutoMock.GetLoose())
            {
                Guid activationCode = Guid.NewGuid();
                var userLogDatabaseProvider = mock.Mock<IDatabaseProvider<UserLog>>();
                var userManager = mock.Mock<IUserManager>();
                userManager.Setup(m => m.GetUserByActivationCode(It.IsAny<string>()))
                    .ReturnsAsync(() => { return default; });
                var passwordProvider = mock.Mock<IPasswordProvider>();
                passwordProvider.Setup(m => m.DecryptPassword(It.IsAny<string>()))
                    .ReturnsAsync(() => { return "1234"; });

                var configSection = mock.Mock<IConfigurationSection>();
                configSection.Setup(x => x.Value).Returns("2562d232dcg213fWEW!@");
                var configuration = mock.Mock<IConfiguration>();
                configuration.Setup(m => m.GetSection(It.IsAny<string>())).Returns(() => { return configSection.Object; });

                var notificationManager = mock.Mock<INotificationManager>();


                var authenticationManager =
                    new AuthenticationManager(userManager.Object,
                    passwordProvider.Object,
                    configuration.Object,
                    notificationManager.Object,
                    userLogDatabaseProvider.Object);



                var task = authenticationManager.VerifyUser(Guid.NewGuid().ToString());
                var result = task.GetAwaiter().GetResult();

                Assert.True((!result.IsVerified && result.Message == "Verification code is not matched"));
            }
        }
        public void Dispose()
        {
            fakeFactory = null;
        }
    }
}
