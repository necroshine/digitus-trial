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
        IDatabaseProvider<UserLog> _userLogDatabaseProvider;
        public AuthenticationManager(
            IUserManager userManager,
            IPasswordProvider passwordProvider,
            IConfiguration configuration,
            INotificationManager notificationManager,
            IDatabaseProvider<UserLog> userLogDatabaseProvider)
        {
            _userManager = userManager;
            _passwordProvider = passwordProvider;
            _configuration = configuration;
            _notificationManager = notificationManager;
            _userLogDatabaseProvider = userLogDatabaseProvider;
        }



        private string GenerateToken(User user)
        {
            if (user == null) throw new ArgumentNullException("GenerateToken.User");

            var tokenSecret = _configuration.GetSection("TokenGeneratorSecretKey").Value;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(tokenSecret);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.RoleName.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public async Task<AuthenticationResultModel> Authenticate(AuthenticationRequestModel model)
        {
            if (string.IsNullOrEmpty(model.UserName)) throw new ArgumentNullException("Authentication.Username");
            if (string.IsNullOrEmpty(model.Password)) throw new ArgumentNullException("Authentication.Password");
            var watch = System.Diagnostics.Stopwatch.StartNew();
            // the code that you want to measure comes here
            
            var user = await _userManager.GetUserByUserName(model.UserName);
            if (user == null)
            {
                return new AuthenticationResultModel() { isAuthenticated = false, CurrentUser = default, Message = "User not found" };
            }
            if(user.Status == Enums.Statuses.PendingAcitivation)
            {
                return new AuthenticationResultModel() { isAuthenticated = false, CurrentUser = default, Message = "Account pending activation." };
            }
            if(model.Password !=  await _passwordProvider.DecryptPassword(user.Password))
            {
                return new AuthenticationResultModel() { isAuthenticated = false, CurrentUser = default, Message = "Invalid credentials" };
            }
            var currentUser = ModelMapper.ToApiModel(user);
            currentUser.Token = GenerateToken(user);
            user.UserStatus = Enums.UserStatuses.Online;
            await _userManager.UpdateUser(user);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            await _userLogDatabaseProvider.Add(new UserLog()
            {
                CreateDate = DateTime.UtcNow,
                Duration = elapsedMs,
                UserId = user.Id,
                Operation = Enums.UserOperations.Login,
               
            });
            return new AuthenticationResultModel() { isAuthenticated = true, CurrentUser = currentUser };
        }

        public async Task<string> GenerateActivationCode()
        {
            return await Task.FromResult(Guid.NewGuid().ToString()).ConfigureAwait(false);   
        }


        public async Task<VerifyUserResultModel> VerifyUser(string verifyCode)
        {
            if (string.IsNullOrEmpty(verifyCode)) throw new ArgumentNullException("verificationCode");
            var user = await _userManager.GetUserByActivationCode(verifyCode);
            if (user == null)
            {
                return new VerifyUserResultModel() { IsVerified = false, Message = "Verification code is not matched" };
            }
            else if (user.Status != Enums.Statuses.PendingAcitivation)
            {
                return new VerifyUserResultModel() { IsVerified = false, Message = "User Account already verified." };
            }
            else
            {
                user.Status = Enums.Statuses.Active;
                user.ActivationDate = DateTime.Now;
                await _userManager.UpdateUser(user);
                return new VerifyUserResultModel() { IsVerified = true, Message = "Verification is success" };
            }            
        }

     
    }
}
