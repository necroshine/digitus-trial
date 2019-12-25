using System;
using System.Threading.Tasks;
using Digitus.Trial.Backend.Api.Interfaces;
using Microsoft.AspNetCore.DataProtection;

namespace Digitus.Trial.Backend.Api.Providers
{
    public class PasswordProvider : IPasswordProvider
    {
        IDataProtectionProvider _dataProtection;
        public PasswordProvider(
            //IDataProtectionProvider dataProtection
            )
        {
            //_dataProtection = dataProtection;
        }

        public async Task<string> DecryptPassword(string password)
        {
            var protector = _dataProtection.CreateProtector("Digitus.Trial.User.Password");
            return await Task.FromResult(protector.Unprotect(password));
        }

        public async Task<string> EncryptPassword(string password)
        {
            var protector = _dataProtection.CreateProtector("Digitus.Trial.User.Password");

            return await Task.FromResult(protector.Protect(password)).ConfigureAwait(false);

        }

        public async Task<string> GenerateActivationCode()
        {
            return await Task.FromResult(Guid.NewGuid().ToString()).ConfigureAwait(false);
        }
    }
}
