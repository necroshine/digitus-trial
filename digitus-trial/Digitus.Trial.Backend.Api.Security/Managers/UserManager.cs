using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Digitus.Trial.Backend.Api.ApiModels;
using Digitus.Trial.Backend.Api.Interfaces;
using Digitus.Trial.Backend.Api.Mappers;

using Digitus.Trial.Backend.Api.Models;

namespace Digitus.Trial.Backend.Api.Managers
{
    public class UserManager: IUserManager
    {
        IDatabaseProvider<User> _userDatabaseProvider;
        IPasswordProvider _passwordProvider;        
        INotificationManager _notificationManager;
        public UserManager(
            IDatabaseProvider<User> userDatabaseProvider,
            INotificationManager notificationManager,
            IPasswordProvider passwordProvider
            )
        {
            _userDatabaseProvider = userDatabaseProvider;
            _passwordProvider = passwordProvider;
            _notificationManager = notificationManager;
        }

        public async Task<CommonResultModel> ForgetPassword(string email)
        {
            if (string.IsNullOrEmpty(email)) throw new ArgumentNullException("ForgetPassword.email");

            var user = await GetUserByEmail(email);
            if (user == null) return new CommonResultModel() { IsSuccess = false, Message = "Account not founded with releated email" };
            var generatedPassword = await _passwordProvider.GeneratePassword().ConfigureAwait(false);
            var chipperPassword = await _passwordProvider.EncryptPassword(generatedPassword).ConfigureAwait(false);
            user.Password = chipperPassword;
            await UpdateUser(user);
            var mailBody = "<html><head></head><body>" +
                "<p>Your password has been changed.</p> " +
                $"<p>Your new password is:<strong> {generatedPassword} </strong></p> " +                
                "</body></html>";
            await _notificationManager.SendMail("info@digitus.com", user.Email, "Password Reset", mailBody);

            return new CommonResultModel() { IsSuccess = true, Message = "New password sent to your email address. Please check your emailbox." };
        }

        public async Task<User> GetUserByActivationCode(string activationCode)
        {
            if (string.IsNullOrEmpty(activationCode)) throw new ArgumentNullException("activationCode");
            var result = await _userDatabaseProvider.GetAllAsync().ConfigureAwait(false);
            var usr = result.Where(u => u.ActivationCode == activationCode).FirstOrDefault();
            return usr;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) throw new ArgumentNullException("email");

            var result = await _userDatabaseProvider.GetAllAsync().ConfigureAwait(false);
            var usr = result.Where(u => u.Email == email).FirstOrDefault();
            return usr;
           
        }

        public async Task<User> GetUserById(Guid id)
        {
            if (id == null) throw new ArgumentNullException("id");

            return await _userDatabaseProvider.GetById(id).ConfigureAwait(false);
        }

        public async Task<User> GetUserByUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException("userName");

            var result = await _userDatabaseProvider.GetAllAsync().ConfigureAwait(false);
            var usr = result.Where(u => u.UserName == userName).FirstOrDefault();
            return usr;
        }

        public async Task<UserRegisterResultModel> Register(UserRegisterRequestModel model)
        {
            if (string.IsNullOrEmpty(model.UserName)) throw new ArgumentNullException("UserReqisterRequestModel.UserName");
            if (string.IsNullOrEmpty(model.Email)) throw new ArgumentNullException("UserReqisterRequestModel.Email");
            if (string.IsNullOrEmpty(model.Password)) throw new ArgumentNullException("UserReqisterRequestModel.Password");
            if (string.IsNullOrEmpty(model.FirstName)) throw new ArgumentNullException("UserReqisterRequestModel.FirstName");
            if (string.IsNullOrEmpty(model.LastName)) throw new ArgumentNullException("UserReqisterRequestModel.LastName");

            if (await GetUserByEmail(model.Email) != null)
            {
                return new UserRegisterResultModel() { IsSuccess = false, Message = "An User already registered with same email address." };
            }
            if (await GetUserByUserName(model.UserName) != null)
            {
                return new UserRegisterResultModel() { IsSuccess = false, Message = "An User already registered with same username." };
            }
            var userModel = ModelMapper.ToModel(model);

            userModel.ActivationCode = await _passwordProvider.GenerateActivationCode();
            userModel.CreateDate = DateTime.UtcNow;
            userModel.ActivationDate = DateTime.MinValue;
            userModel.ActivationCodeSentDate = DateTime.UtcNow;
            userModel.Password = await _passwordProvider.EncryptPassword(userModel.Password);
            userModel.Status = Enums.Statuses.PendingAcitivation;
            var result = await _userDatabaseProvider.Add(userModel).ConfigureAwait(false);
            if(result != null)
            {
                string bodyHtml = _notificationManager.GenerateActivationMailBody(result.ActivationCode);
                
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
