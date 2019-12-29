using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digitus.Trial.Backend.Api.ApiModels;
using Digitus.Trial.Backend.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Digitus.Trial.Backend.Api.Controllers
{
    [Route("api/[controller]")]
    public class SecurityController : Controller
    {
        IAuthenticatationManager _authenticationManager;
        public SecurityController(IAuthenticatationManager authenticatationManager) {
            _authenticationManager = authenticatationManager;
        }

        [HttpPost("Authenticate")]
        [AllowAnonymous]
        public async Task<AuthenticationResultModel> Authenticate([FromBody] AuthenticationRequestModel request) {            
            AuthenticationResultModel result = await _authenticationManager.Authenticate(request); ;
            return await Task.FromResult(result);
        }

        [HttpGet("VerifyUser/{activationCode}")]
        [AllowAnonymous]
        public async Task<VerifyUserResultModel> VerifyUser(string activationCode) {
            VerifyUserResultModel model = await _authenticationManager.VerifyUser(activationCode);
            return await Task.FromResult(model);
        }
    }
}
