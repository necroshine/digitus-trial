using System;
using System.Threading.Tasks;
using Digitus.Trial.Backend.Api.ApiModels;

namespace Digitus.Trial.Backend.Api.Interfaces
{
    public interface IReportManager
    {
        Task<LoginReportResultModel> PopulateLoginReport(DateTime filterValue);
        Task<VerifyUserReportResponseModel> PopulateVerifyUserReport();
        Task<UserReportResponseModel> PopulateUserReport(int timePeriodByDay);

    }
}
