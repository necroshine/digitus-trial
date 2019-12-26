using System;
using System.Threading.Tasks;
using Digitus.Trial.Backend.Api.ApiModels;

namespace Digitus.Trial.Backend.Api.Interfaces
{
    public interface IReportProvider
    {
        Task<LoginReportResultModel> PopulateLoginReport(LoginReportRequestModel request);
        Task<VerifyUserReportResponseModel> PopulateVerifyUserReport();
        Task<UserReportResponseModel> PopulateUserReport(UserReportRequestModel request);

    }
}
