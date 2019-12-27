using System;
using System.Threading.Tasks;
using System.Linq;
using Digitus.Trial.Backend.Api.ApiModels;
using Digitus.Trial.Backend.Api.Interfaces;
using Digitus.Trial.Backend.Api.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using Digitus.Trial.Backend.Api.Providers;
using Digitus.Trial.Backend.Api.Enums;

namespace Digitus.Trial.Backend.Api.Managers
{
    public class ReportManager : IReportManager
    {
        IDatabaseProvider<User> _userDatabaseProvider;
        IDatabaseProvider<UserLog> _userLogDatabaseProvider;
        public ReportManager(IDatabaseProvider<User> userDatabaseProvider,
                             IDatabaseProvider<UserLog> userLogDatabaseProvider)
        {
            _userDatabaseProvider = userDatabaseProvider;
            _userLogDatabaseProvider = userLogDatabaseProvider;
        }

        public async Task<LoginReportResultModel> PopulateLoginReport(DateTime filterValue)
        {
            var filterEnd = filterValue.AddDays(1);
            var data = (await _userLogDatabaseProvider.Collection
                .FindAsync(x => x.CreateDate >= filterValue && x.CreateDate < filterEnd)).ToList();
            var duration = data.Average(x => x.Duration);
            return new LoginReportResultModel() { ReportDate = filterValue.Date, AverageLoginDuration = duration };
        }


        public async Task<UserReportResponseModel> PopulateUserReport(int timePeriodByDay)
        {
            var onlineUsers = (await _userDatabaseProvider.GetByFilter($"{{'UserStatus': {(int)UserStatuses.Online}}}")).Count();

            var filterStart = DateTime.UtcNow.AddDays(timePeriodByDay * -1).Date;
            var filterEnd = DateTime.UtcNow.AddDays(1).Date;
            var registeredUsers = (await _userDatabaseProvider.Collection
                        .FindAsync(x => x.CreateDate >= filterStart && x.CreateDate < filterEnd)).ToList().Count();

            return new UserReportResponseModel()
            {
                OnlineUserCount = onlineUsers,
                RegisteredUserCountInPeriod = timePeriodByDay,
                RegisteredUserTimePeriod = registeredUsers
            };

        }

        public async Task<VerifyUserReportResponseModel> PopulateVerifyUserReport()
        {
            var data = await _userDatabaseProvider.GetByFilter("{'Status': 1}");

            var result = (from row in data where (DateTime.UtcNow.Date - row.ActivationCodeSentDate.Date).TotalDays >= 1 select row).Count();

            return new VerifyUserReportResponseModel() { UserCount = result };
        }
    }
}
