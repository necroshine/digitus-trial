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
    public class ReportsController : Controller
    {
        IReportManager _reportManager;
        public ReportsController(IReportManager reportManager) {
            _reportManager = reportManager;
        }

        [HttpGet("PopulateUserReport/{duration}")]
        [AllowAnonymous]
        public async Task<UserReportResponseModel> PopulateUserReport(int duration)
        {
            return await _reportManager.PopulateUserReport(duration);
        }
        [HttpGet("PopulateVerifyUserReports")]
        [AllowAnonymous]
        public async Task<VerifyUserReportResponseModel> PopulateVerifyUserReport()
        {
            return await _reportManager.PopulateVerifyUserReport();
        }

        [HttpGet("PopulateLoginReport/{date}")]
        [AllowAnonymous]
        public async Task<LoginReportResultModel> PopulateLoginReport(DateTime date)
        {
            return await _reportManager.PopulateLoginReport(date);
        }
     }
}
