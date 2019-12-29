using System;
using System.Collections.Generic;
using Digitus.Trial.Backend.Api.Enums;
using Digitus.Trial.Backend.Api.Models;

namespace Digitus.Trial.Backend.Api.Test
{
    public class FakeObjectFactory
    {
        public FakeObjectFactory()
        {
        }


        public UserLog GetUserLog()
        {
            return new UserLog()
            {
                CreateDate = DateTime.UtcNow,
                Duration = 100,
                Operation = Enums.UserOperations.Login,
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };
        }

        public User GetUserByActivationCode(string activationCode,Statuses status = Statuses.PendingAcitivation)
        {
            return new User()
            {
                Id = Guid.Empty,
                Email = "serhatyalcin@gmail.com",
                FirstName = "serhat",
                LastName = "yalcin",
                CreateDate = DateTime.UtcNow,
                ActivationCode = activationCode,
                ActivationCodeSentDate = DateTime.UtcNow,
                Role = Enums.UserRoles.StandartUser,
                Status = status,
                UserStatus = Enums.UserStatuses.Offline,
                Password = "12345",
                UserName = "serhatyalcin"

            };
        }

        public  User GetUserByUserName(string username,Enums.Statuses status = Enums.Statuses.Active)
        {
            return new User()
            {
                Id = Guid.Empty,
                Email = "serhatyalcin@gmail.com",
                FirstName = "serhat",
                LastName = "yalcin",
                CreateDate = DateTime.UtcNow,
                ActivationCode = Guid.NewGuid().ToString(),
                ActivationCodeSentDate = DateTime.UtcNow,
                Role = Enums.UserRoles.StandartUser,
                Status = status,
                UserStatus = Enums.UserStatuses.Offline,
                Password = "12345",
                UserName = username

            };
        }


        public IList<User> GetUserAsEnumarable()
        {
            IList<User> list = new List<User>();
            list.Add(
                new User()
                {
                    Id = Guid.Empty,
                    Email = "serhatyalcin@gmail.com",
                    FirstName = "serhat",
                    LastName = "yalcin",
                    CreateDate = DateTime.UtcNow,
                    ActivationCode = Guid.NewGuid().ToString(),
                    ActivationCodeSentDate = DateTime.UtcNow,
                    Role = Enums.UserRoles.StandartUser,
                    Status = Enums.Statuses.PendingAcitivation,
                    UserStatus = Enums.UserStatuses.Offline,
                    UserName = "serhatyalcin"

                });
            list.Add(
                new User()
                {
                    Id = Guid.Empty,
                    Email = "arminyalcin@gmail.com",
                    FirstName = "armin",
                    LastName = "yalcin",
                    CreateDate = DateTime.UtcNow,
                    ActivationCode = Guid.NewGuid().ToString(),
                    ActivationCodeSentDate = DateTime.UtcNow,
                    Role = Enums.UserRoles.StandartUser,
                    Status = Enums.Statuses.PendingAcitivation,
                    UserStatus = Enums.UserStatuses.Offline,
                    UserName = "arminyalcin"

                });
            list.Add(
                new User()
                {
                    Id = Guid.Empty,
                    Email = "caganyalcin@gmail.com",
                    FirstName = "cagan",
                    LastName = "yalcin",
                    CreateDate = DateTime.UtcNow,
                    ActivationCode = Guid.NewGuid().ToString(),
                    ActivationCodeSentDate = DateTime.UtcNow,
                    Role = Enums.UserRoles.StandartUser,
                    Status = Enums.Statuses.PendingAcitivation,
                    UserStatus = Enums.UserStatuses.Offline,
                    UserName = "caganyalcin"

                }
               );
            return list;
        }
        public  IList<User> GetUserAsEnumarable(string username,string email)
        {
            IList<User> list = new List<User>();
            list.Add(new User()
            {
                Id = Guid.Empty,
                Email = email,
                FirstName = "serhat",
                LastName = "yalcin",
                CreateDate = DateTime.UtcNow,
                ActivationCode = Guid.NewGuid().ToString(),
                ActivationCodeSentDate = DateTime.UtcNow,
                Role = Enums.UserRoles.StandartUser,
                Status = Enums.Statuses.PendingAcitivation,
                UserStatus = Enums.UserStatuses.Offline,
                UserName = username

            });
            return list;
        }

        public  User GetUser()
        {
            return new User()
            {
                Id = Guid.Empty,
                Email = "serhatyalcin@gmail.com",
                FirstName = "serhat",
                LastName = "yalcin",
                CreateDate = DateTime.UtcNow,
                ActivationCode = Guid.NewGuid().ToString(),
                ActivationCodeSentDate = DateTime.UtcNow,
                Role = Enums.UserRoles.StandartUser,
                Status = Enums.Statuses.PendingAcitivation,
                UserStatus = Enums.UserStatuses.Offline,
                UserName = "serhatyalcin"

            };
        }

        public  User GetUserByUserNameWithNullResult(string username)
        {
            return default;
        }

        public  User GetUserByEmail(string email)
        {
            return new User()
            {
                Id = Guid.Empty,
                Email = "serhatyalcin@gmail.com",
                FirstName = "serhat",
                LastName = "yalcin",
                CreateDate = DateTime.UtcNow,
                ActivationCode = Guid.NewGuid().ToString(),
                ActivationCodeSentDate = DateTime.UtcNow,
                Role = Enums.UserRoles.StandartUser,
                Status = Enums.Statuses.PendingAcitivation,
                UserStatus = Enums.UserStatuses.Offline,
                UserName = "serhatyalcin"

            };
        }

        public  User GetUserByEmailWithNullResult(string email)
        {
            return default;
        }
    }
}
