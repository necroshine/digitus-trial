using System;
namespace Digitus.Trial.Backend.Api.Security.ApiModels
{
    public class AuthenticationResultModel
    {
        public bool isAuthenticated { get; set; }
        public UserApiModel CurrentUser { get; set; }
    }
}
