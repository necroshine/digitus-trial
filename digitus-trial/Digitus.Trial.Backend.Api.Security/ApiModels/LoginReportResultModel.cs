using System;
namespace Digitus.Trial.Backend.Api.ApiModels
{
    public class LoginReportResultModel
    {
        public DateTime ReportDate { get; set; }
        public long AverageLoginDuration { get; set; }
    }
}
