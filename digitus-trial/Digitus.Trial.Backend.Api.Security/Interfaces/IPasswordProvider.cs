using System;
using System.Threading.Tasks;

namespace Digitus.Trial.Backend.Api.Interfaces
{
    public interface IPasswordProvider
    {
        Task<string> GenerateActivationCode();

        Task<string> EncryptPassword(string password);
        Task<string> DecryptPassword(string password);
        Task<string> GeneratePassword();
    }
}
