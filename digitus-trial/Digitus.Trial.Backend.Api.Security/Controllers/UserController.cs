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
    public class UserController : Controller
    {
        private  IUserManager _userManager;
        public UserController(IUserManager userManager) {
            _userManager = userManager;
        }
        // GET: api/values
        [HttpGet("HealthCheck")]
        [AllowAnonymous]
        public IActionResult HelathCheck()
        {
            return Ok("Healthy");
        }
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<UserRegisterResultModel> Register([FromBody]UserRegisterRequestModel request)
        {
            var result = await _userManager.Register(request);
            return result;
        }

        [HttpPost("ForgetPassword")]
        [Authorize]
        public async Task<CommonResultModel> ForgetPassword([FromBody] ForgetPasswordRequestModel request) {
            return await _userManager.ForgetPassword(request.Email);
        }
    }
}
