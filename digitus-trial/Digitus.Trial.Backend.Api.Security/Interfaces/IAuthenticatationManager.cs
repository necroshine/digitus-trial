using System;
using System.Threading.Tasks;
using Digitus.Trial.Backend.Api.ApiModels;
using Digitus.Trial.Backend.Api.Models;

namespace Digitus.Trial.Backend.Api.Interfaces
{
    public interface IAuthenticatationManager
    {
        Task<AuthenticationResultModel> Authenticate(AuthenticationRequestModel model);
        Task<VerifyUserResultModel> VerifyUser(string verifyToken);
        Task<CommonResultModel> ForgetPassword(string userMail);

        

    }
}
