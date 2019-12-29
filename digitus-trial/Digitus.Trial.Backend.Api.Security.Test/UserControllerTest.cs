using System;
using Xunit;
using Moq;
using Digitus.Trial.Backend.Api.Interfaces;
using Digitus.Trial.Backend.Api.Models;
using Digitus.Trial.Backend.Api.Managers;
using Digitus.Trial.Backend.Api.Controllers;
using System.Threading.Tasks;

namespace Digitus.Trial.Backend.Api.Test
{
    public class UserControllerTest: IDisposable
    {

        FakeObjectFactory fakeObjectFactory;
        public UserControllerTest()
        {
            fakeObjectFactory = new FakeObjectFactory();
        }

        public void Dispose()
        {
            fakeObjectFactory = null;
        }

        [Fact]
        public void UserRegistrationExistingEmailValidationTest()
        {
            using (var mock = Autofac.Extras.Moq.AutoMock.GetLoose())
            {
                var userDatabaseProvider = mock.Mock<IDatabaseProvider<User>>();
                userDatabaseProvider.Setup(x => x.GetAllAsync()).ReturnsAsync(fakeObjectFactory.GetUserAsEnumarable());

                var passwordProvider = mock.Mock<IPasswordProvider>();
                var notificationManager = mock.Mock<INotificationManager>();

                var userManager = new UserManager(userDatabaseProvider.Object,
                    notificationManager.Object,
                    passwordProvider.Object);

                var userController = new UserController(userManager);

                var task = userController.Register(new ApiModels.UserRegisterRequestModel()
                {
                    Email="serhatyalcin@gmail.com",
                    FirstName = "Serhat",
                    LastName = "Yalcin",
                    Password = "1234",
                    UserName = "serhatyalcin"
                }).ConfigureAwait(false);
                var result = task.GetAwaiter().GetResult();
                Assert.Equal("An User already registered with same email address.", result.Message);

            }
        }

        [Fact]
        public void UserRegistrationExistingUsernameValidationTest()
        {
            using (var mock = Autofac.Extras.Moq.AutoMock.GetLoose())
            {
                var userDatabaseProvider = mock.Mock<IDatabaseProvider<User>>();
                userDatabaseProvider.Setup(x => x.GetAllAsync()).ReturnsAsync(fakeObjectFactory.GetUserAsEnumarable());
                var passwordProvider = mock.Mock<IPasswordProvider>();
                var notificationManager = mock.Mock<INotificationManager>();

                var userManager = new UserManager(userDatabaseProvider.Object, notificationManager.Object, passwordProvider.Object);

                var userController = new UserController(userManager);

                var task = userController.Register(new ApiModels.UserRegisterRequestModel()
                {
                    Email = "serhatyalcin2@gmail.com",
                    FirstName = "Serhat",
                    LastName = "Yalcin",
                    Password = "1234",
                    UserName = "serhatyalcin"
                }).ConfigureAwait(false);
                var result = task.GetAwaiter().GetResult();
                Assert.Equal("An User already registered with same username.", result.Message);

            }
        }

        [Fact]
        public async Task UserRegistrationEmailNullArgumentExceptionTest()
        {
            using (var mock = Autofac.Extras.Moq.AutoMock.GetLoose())
            {
                var userDatabaseProvider = mock.Mock<IDatabaseProvider<User>>();
                userDatabaseProvider.Setup(x => x.GetAllAsync()).ReturnsAsync(fakeObjectFactory.GetUserAsEnumarable());
                var passwordProvider = mock.Mock<IPasswordProvider>();
                var notificationManager = mock.Mock<INotificationManager>();

                var userManager = new UserManager(userDatabaseProvider.Object, notificationManager.Object, passwordProvider.Object);

                var userController = new UserController(userManager);
                var request = new ApiModels.UserRegisterRequestModel()
                {
                    Email = default,
                    FirstName = "Serhat",
                    LastName = "Yalcin",
                    Password = "1234",
                    UserName = "serhatyalcin"
                };
                
                await Assert.ThrowsAsync<ArgumentNullException>(() => userController.Register(request));

            }
        }

        [Fact]
        public async Task UserRegistrationUsernameNullArgumentExceptionTest()
        {
            using (var mock = Autofac.Extras.Moq.AutoMock.GetLoose())
            {
                var userDatabaseProvider = mock.Mock<IDatabaseProvider<User>>();
                userDatabaseProvider.Setup(x => x.GetAllAsync()).ReturnsAsync(fakeObjectFactory.GetUserAsEnumarable());
                var passwordProvider = mock.Mock<IPasswordProvider>();
                var notificationManager = mock.Mock<INotificationManager>();

                var userManager = new UserManager(userDatabaseProvider.Object, notificationManager.Object, passwordProvider.Object);

                var userController = new UserController(userManager);
                var request = new ApiModels.UserRegisterRequestModel()
                {
                    Email = "serhatyalcin@gmail.com",
                    FirstName = "Serhat",
                    LastName = "Yalcin",
                    Password = "1234",
                    UserName = null
                };

                await Assert.ThrowsAsync<ArgumentNullException>(() => userController.Register(request));

            }
        }

        [Fact]
        public async Task UserRegistrationPasswordNullArgumentExceptionTest()
        {
            using (var mock = Autofac.Extras.Moq.AutoMock.GetLoose())
            {
                var userDatabaseProvider = mock.Mock<IDatabaseProvider<User>>();
                userDatabaseProvider.Setup(x => x.GetAllAsync()).ReturnsAsync(fakeObjectFactory.GetUserAsEnumarable());
                var passwordProvider = mock.Mock<IPasswordProvider>();
                var notificationManager = mock.Mock<INotificationManager>();

                var userManager = new UserManager(userDatabaseProvider.Object, notificationManager.Object, passwordProvider.Object);

                var userController = new UserController(userManager);
                var request = new ApiModels.UserRegisterRequestModel()
                {
                    Email = "serhatyalcin@gmail.com",
                    FirstName = "Serhat",
                    LastName = "Yalcin",
                    Password = null,
                    UserName = "serhatyalcin"
                };

                await Assert.ThrowsAsync<ArgumentNullException>(() => userController.Register(request));

            }
        }

        [Fact]
        public void UserRegistrationWithSuccess()
        {
            using (var mock = Autofac.Extras.Moq.AutoMock.GetLoose())
            {
                var userDatabaseProvider = mock.Mock<IDatabaseProvider<User>>();
                userDatabaseProvider.Setup(x => x.Add(It.IsAny<User>())).ReturnsAsync(fakeObjectFactory.GetUser());
                userDatabaseProvider.Setup(x => x.GetAllAsync()).ReturnsAsync(fakeObjectFactory.GetUserAsEnumarable());
                var passwordProvider = mock.Mock<IPasswordProvider>();
                var notificationManager = mock.Mock<INotificationManager>();

                var userManager = new UserManager(userDatabaseProvider.Object, notificationManager.Object, passwordProvider.Object);

                var userController = new UserController(userManager);
                var task = userController.Register(new ApiModels.UserRegisterRequestModel()
                {
                    Email = "serhatyalcin22@gmail.com",
                    FirstName = "Serhat",
                    LastName = "Yalcin",
                    Password = "1234",
                    UserName = "serhatyalcin22"
                }).ConfigureAwait(false);
                var result = task.GetAwaiter().GetResult();
                Assert.Equal(true, result.IsSuccess);



            }
        }

        [Fact]
        public void UserRegistrationWithFail()
        {
            using (var mock = Autofac.Extras.Moq.AutoMock.GetLoose())
            {
                var userDatabaseProvider = mock.Mock<IDatabaseProvider<User>>();
                
                userDatabaseProvider.Setup(x => x.GetAllAsync()).ReturnsAsync(fakeObjectFactory.GetUserAsEnumarable());
                var passwordProvider = mock.Mock<IPasswordProvider>();
                var notificationManager = mock.Mock<INotificationManager>();

                var userManager = new UserManager(userDatabaseProvider.Object, notificationManager.Object, passwordProvider.Object);

                var userController = new UserController(userManager);
                var task = userController.Register(new ApiModels.UserRegisterRequestModel()
                {
                    Email = "serhatyalcin22@gmail.com",
                    FirstName = "Serhat",
                    LastName = "Yalcin",
                    Password = "1234",
                    UserName = "serhatyalcin22"
                }).ConfigureAwait(false);
                var result = task.GetAwaiter().GetResult();
                Assert.Equal(false, result.IsSuccess);



            }
        }

        [Fact]
        public void ForgetPasswordSuccess()
        {
            using (var mock = Autofac.Extras.Moq.AutoMock.GetLoose())
            {
                var userDatabaseProvider = mock.Mock<IDatabaseProvider<User>>();
                userDatabaseProvider.Setup(x => x.Add(It.IsAny<User>())).ReturnsAsync(fakeObjectFactory.GetUser());
                userDatabaseProvider.Setup(x => x.GetAllAsync()).ReturnsAsync(fakeObjectFactory.GetUserAsEnumarable());
                var passwordProvider = mock.Mock<IPasswordProvider>();
                var notificationManager = mock.Mock<INotificationManager>();

                var userManager = new UserManager(userDatabaseProvider.Object, notificationManager.Object, passwordProvider.Object);

                var userController = new UserController(userManager);
                var task = userController.ForgetPassword(new ApiModels.ForgetPasswordRequestModel()
                {
                    Email = "serhatyalcin@gmail.com"
                }).ConfigureAwait(false);
                var result = task.GetAwaiter().GetResult();
                Assert.Equal(true, result.IsSuccess);

            }
        }
        [Fact]
        public async Task ForgetPasswordFailedDueInvalidArgument()
        {
            using (var mock = Autofac.Extras.Moq.AutoMock.GetLoose())
            {
                var userDatabaseProvider = mock.Mock<IDatabaseProvider<User>>();
                userDatabaseProvider.Setup(x => x.Add(It.IsAny<User>())).ReturnsAsync(fakeObjectFactory.GetUser());
                userDatabaseProvider.Setup(x => x.GetAllAsync()).ReturnsAsync(fakeObjectFactory.GetUserAsEnumarable());
                var passwordProvider = mock.Mock<IPasswordProvider>();
                var notificationManager = mock.Mock<INotificationManager>();

                var userManager = new UserManager(userDatabaseProvider.Object, notificationManager.Object, passwordProvider.Object);

                var userController = new UserController(userManager);
                var model = new ApiModels.ForgetPasswordRequestModel()
                {
                    Email = default
                };
                                
                await Assert.ThrowsAsync<ArgumentNullException>(() => userController.ForgetPassword(model));

            }
        }
        [Fact]
        public void ForgetPasswordFailedDueAccountNotFound()
        {
            using (var mock = Autofac.Extras.Moq.AutoMock.GetLoose())
            {
                var userDatabaseProvider = mock.Mock<IDatabaseProvider<User>>();
                userDatabaseProvider.Setup(x => x.Add(It.IsAny<User>())).ReturnsAsync(fakeObjectFactory.GetUser());
                userDatabaseProvider.Setup(x => x.GetAllAsync()).ReturnsAsync(fakeObjectFactory.GetUserAsEnumarable());
                var passwordProvider = mock.Mock<IPasswordProvider>();
                var notificationManager = mock.Mock<INotificationManager>();

                var userManager = new UserManager(userDatabaseProvider.Object, notificationManager.Object, passwordProvider.Object);

                var userController = new UserController(userManager);
                var model = new ApiModels.ForgetPasswordRequestModel()
                {
                    Email = "tahresniclay@gmail.com"
                };
                var task = userController.ForgetPassword(model).ConfigureAwait(false);
                var result = task.GetAwaiter().GetResult();
                Assert.True(!result.IsSuccess);

            }
        }
    }
}
