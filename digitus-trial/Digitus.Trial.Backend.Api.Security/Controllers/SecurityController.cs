using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Digitus.Trial.Backend.Api.Security.Controllers
{
    [Route("api/[controller]")]
    public class SecurityController : Controller
    {
        public SecurityController() { }

        [HttpPost("Authenticate")]
        public void Authenticate() { }
    }
}
