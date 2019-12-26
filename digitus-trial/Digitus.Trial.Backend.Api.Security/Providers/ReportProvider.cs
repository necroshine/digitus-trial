using System;
using System.Threading.Tasks;
using Digitus.Trial.Backend.Api.ApiModels;
using Digitus.Trial.Backend.Api.Interfaces;
using Digitus.Trial.Backend.Api.Models;

namespace Digitus.Trial.Backend.Api.Providers
{
    public class ReportProvider:IReportProvider
    {
        IDatabaseProvider<User> _userDatabaseProvider;
        IDatabaseProvider<UserLog> _userLogDatabaseProvider;
        public ReportProvider(IDatabaseProvider<User> userDatabaseProvider,
            IDatabaseProvider<UserLog> userLogDatabaseProvider)
        {
            _userDatabaseProvider = userDatabaseProvider;
            _userLogDatabaseProvider = userLogDatabaseProvider;
        }

        public Task<LoginReportResultModel> PopulateLoginReport(LoginReportRequestModel request)
        {
            throw new NotImplementedException();
        }

        public Task<UserReportResponseModel> PopulateUserReport(UserReportRequestModel request)
        {
            throw new NotImplementedException();
        }

        public Task<VerifyUserReportResponseModel> PopulateVerifyUserReport()
        {
            throw new NotImplementedException();
        }
    }
}
