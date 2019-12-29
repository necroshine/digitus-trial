using System;
namespace Digitus.Trial.Backend.Api.ApiModels
{
    public class AuthenticationResultModel
    {
        public bool isAuthenticated { get; set; }
        public UserApiModel CurrentUser { get; set; }
        public string Message { get; set; }
    }
}
