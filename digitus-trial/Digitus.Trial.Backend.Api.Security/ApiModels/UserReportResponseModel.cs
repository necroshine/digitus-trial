using System;
namespace Digitus.Trial.Backend.Api.ApiModels
{
    public class UserReportResponseModel
    {
        public int OnlineUserCount { get; set; }
        public string RegisteredUserTimePeriod { get; set; }
        public int RegisteredUserCountInPeriod { get; set; }
    }
}
