using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digitus.Trial.Backend.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Digitus.Trial.Backend.Api.Controllers
{
    [Route("api/[controller]")]
    public class DataGeneratorController : Controller
    {
        IMockManager _mockManager;
        
        public DataGeneratorController(IMockManager mockManager)
        {
            _mockManager = mockManager;
        }
        
        [HttpGet("GenerateUserData")]
        [AllowAnonymous]
        public async Task<string> GenerateUserData()
        {
            return await _mockManager.GenerateUserDataForTest();
        }

        [HttpGet("GenerateUserLogData")]
        [AllowAnonymous]
        public async Task<string> GenerateUserLogData()
        {
            return await _mockManager.GenerateUserLogDataForTest();
        }
    }
}
