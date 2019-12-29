using System;
using System.Threading.Tasks;

namespace Digitus.Trial.Backend.Api.Interfaces
{
    public interface IMockManager
    {
        Task<string> GenerateUserDataForTest();
        Task<string> GenerateUserLogDataForTest();
    }
}
