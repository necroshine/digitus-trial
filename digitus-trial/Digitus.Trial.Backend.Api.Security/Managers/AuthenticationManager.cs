using System;
using System.Threading.Tasks;
using Digitus.Trial.Backend.Api.ApiModels;
using Digitus.Trial.Backend.Api.Security.Interfaces;
using Digitus.Trial.Backend.Api.Security.Models;

namespace Digitus.Trial.Backend.Api.Managers
{
    public class AuthenticationManager : IAuthenticatationManager       
    {
        IUserManager _userManager;
        public AuthenticationManager(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public Task<AuthenticationResultModel> Authenticate(AuthenticationRequestModel model)
        {
            throw new NotImplementedException();
        }

        public Task<string> ChipperPassword(string password)
        {
            throw new NotImplementedException();
        }

        public Task<bool?> ForgetPassword(string userMail)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GenerateActivationCode()
        {
            throw new NotImplementedException();
        }

        public Task ResetPassword()
        {
            throw new NotImplementedException();
        }

        public async Task<VerifyUserResultModel> VerifyUser(string verifyCode)
        {
            if (string.IsNullOrEmpty(verifyCode)) throw new ArgumentNullException("verificationCode");
            var user = await _userManager.GetUserByActivationCode(verifyCode);
            if (user == null)
            {
                return new VerifyUserResultModel() { IsVerified = false, Message = "Verification code is not matched" };
            }
            else
            {
                user.Status = Enums.UserStatuses.Active;
                await _userManager.UpdateUser(user);
                return new VerifyUserResultModel() { IsVerified = true, Message = "Verification is success" };
            }            
        }
    }
}
