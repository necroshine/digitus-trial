using System;
using System.Linq;
using System.Threading.Tasks;
using Digitus.Trial.Backend.Api.ApiModels;
using Digitus.Trial.Backend.Api.Interfaces;
using Digitus.Trial.Backend.Api.Mappers;

using Digitus.Trial.Backend.Api.Security.Models;

namespace Digitus.Trial.Backend.Api.Managers
{
    public class UserManager: IUserManager
    {
        IDatabaseProvider<User> _userDatabaseProvider;
        IAuthenticatationManager _authenticationManager;
        INotificationManager _notificationManager;
        public UserManager(
            IDatabaseProvider<User> userDatabaseProvider,
            INotificationManager notificationManager,
            IAuthenticatationManager authenticatationManager)
        {
            _userDatabaseProvider = userDatabaseProvider;
            _authenticationManager = authenticatationManager;
            _notificationManager = notificationManager;
        }

        public async Task<User> GetUserByActivationCode(string activationCode)
        {
            if (string.IsNullOrEmpty(activationCode)) throw new ArgumentNullException("activationCode");

            var result = await _userDatabaseProvider.GetByFilter($"{{'ActivationCode': '{activationCode}'}}").ConfigureAwait(false);
            return result.FirstOrDefault();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) throw new ArgumentNullException("email");

            var result = await _userDatabaseProvider.GetByFilter($"{{'Email': '{email}'}}").ConfigureAwait(false);
            return result.FirstOrDefault();
           
        }

        public async Task<User> GetUserById(Guid id)
        {
            if (id == null) throw new ArgumentNullException("id");

            return await _userDatabaseProvider.GetById(id).ConfigureAwait(false);
        }

        public async Task<User> GetUserByUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException("userName");

            var result = await _userDatabaseProvider.GetByFilter($"{{'UserName': '{userName}'}}").ConfigureAwait(false);
            return result.FirstOrDefault();
        }

        public async Task<UserRegisterResultModel> Register(UserApiModel model)
        {
            var userModel = ModelMapper.ToModel(model);
            userModel.ActivationCode = await _authenticationManager.GenerateActivationCode();
            var result = await _userDatabaseProvider.Add(userModel).ConfigureAwait(false);
            if(result != null)
            {
                string activationCode = await _authenticationManager.GenerateActivationCode();
                result.ActivationCode = activationCode;
                await _userDatabaseProvider.Update(result);

                string bodyHtml = _notificationManager.GenerateActivationMailBody(activationCode);
                
                await _notificationManager.SendMail("info@digitus.com",
                    result.Email,
                    "Account Activation",
                    bodyHtml)
                    .ConfigureAwait(false);
                return new UserRegisterResultModel() { IsSuccess = true, Message = "User Registered successfully." };
            }
            else
            {
                return new UserRegisterResultModel() { IsSuccess = false, Message = "User not Registered due an error." };
            }
            
        }

        public async Task<User> UpdateUser(User model)
        {
            if (model == null) throw new ArgumentNullException("model");
            return await _userDatabaseProvider.Update(model);
        }
    }
}
