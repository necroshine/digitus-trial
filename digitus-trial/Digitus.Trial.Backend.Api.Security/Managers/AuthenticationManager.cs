using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Digitus.Trial.Backend.Api.ApiModels;
using Digitus.Trial.Backend.Api.Interfaces;
using Digitus.Trial.Backend.Api.Mappers;
using Digitus.Trial.Backend.Api.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Digitus.Trial.Backend.Api.Managers
{
    public class AuthenticationManager : IAuthenticatationManager       
    {
        IUserManager _userManager;
        IPasswordProvider _passwordProvider;
        IConfiguration _configuration;
        INotificationManager _notificationManager;
        public AuthenticationManager(
            IUserManager userManager,
            IPasswordProvider passwordProvider,
            IConfiguration configuration,
            INotificationManager notificationManager)
        {
            _userManager = userManager;
            _passwordProvider = passwordProvider;
            _configuration = configuration;
            _notificationManager = notificationManager;
        }

        private string GenerateToken(User user)
        {
            var tokenSecret = _configuration.GetSection("TokenGeneratorSecretKey").Value;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(tokenSecret);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public async Task<AuthenticationResultModel> Authenticate(AuthenticationRequestModel model)
        {
           
            var user = await _userManager.GetUserByUserName(model.UserName);
            if (user == null)
            {
                return new AuthenticationResultModel() { isAuthenticated = false, CurrentUser = default };
            }
            if(model.Password !=  await _passwordProvider.DecryptPassword(user.Password))
            {
                return new AuthenticationResultModel() { isAuthenticated = false, CurrentUser = default };
            }
            var currentUser = ModelMapper.ToApiModel(user);
            currentUser.Token = GenerateToken(user);
            return new AuthenticationResultModel() { isAuthenticated = true, CurrentUser = currentUser };
        }

       

        public async Task<CommonResultModel> ForgetPassword(string userMail)
        {
            var user = await _userManager.GetUserByEmail(userMail);
            if(user == null) return new CommonResultModel() { IsSuccess = false, Message = "Account not founded with releated email" };
            var generatedPassword = GeneratePassword();
            var chipperPassword = await _passwordProvider.EncryptPassword(generatedPassword).ConfigureAwait(false);
            user.Password = chipperPassword;
            var mailBody = "";
            await _notificationManager.SendMail("info@digitus.com", user.Email, "Password Reset", mailBody);

            return new CommonResultModel() { IsSuccess = true, Message = "New password sent to your email address. Please check your emailbox." };

        }

        private string GeneratePassword()
        {
            StringBuilder password = new StringBuilder();
            Random random = new Random();
            var digit = true;
            var lowercase = true;
            var uppercase = true;
            var nonAlphanumeric = true;
            while (password.Length < 8)
            {
                char c = (char)random.Next(32, 126);

                password.Append(c);

                if (char.IsDigit(c))
                    digit = false;
                else if (char.IsLower(c))
                    lowercase = false;
                else if (char.IsUpper(c))
                    uppercase = false;
                else if (!char.IsLetterOrDigit(c))
                    nonAlphanumeric = false;
            }

            if (nonAlphanumeric)
                password.Append((char)random.Next(33, 48));
            if (digit)
                password.Append((char)random.Next(48, 58));
            if (lowercase)
                password.Append((char)random.Next(97, 123));
            if (uppercase)
                password.Append((char)random.Next(65, 91));

            return password.ToString();
        }

        public async Task<string> GenerateActivationCode()
        {
            return await Task.FromResult(Guid.NewGuid().ToString()).ConfigureAwait(false);   
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
