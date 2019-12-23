using System;
using System.Threading.Tasks;
using Digitus.Trial.Backend.Api.ApiModels;
using Digitus.Trial.Backend.Api.Security.Models;

namespace Digitus.Trial.Backend.Api.Interfaces
{
    public interface IUserManager
    {
        Task<UserRegisterResultModel> Register(UserApiModel model);
        Task<User> GetUserByUserName(string userName);
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserByActivationCode(string activationCode);
        Task<User> GetUserById(Guid id);
        Task<User> UpdateUser(User model);

    }
}
