using System;
namespace Digitus.Trial.Backend.Api.Security.ApiModels
{
    public class AuthenticationRequestModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
