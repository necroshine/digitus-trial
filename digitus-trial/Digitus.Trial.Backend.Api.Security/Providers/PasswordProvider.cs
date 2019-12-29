using System;
using System.Text;
using System.Threading.Tasks;
using Digitus.Trial.Backend.Api.Interfaces;
using Microsoft.AspNetCore.DataProtection;

namespace Digitus.Trial.Backend.Api.Providers
{
    public class PasswordProvider : IPasswordProvider
    {
        IDataProtectionProvider _dataProtection;
        public PasswordProvider(
            IDataProtectionProvider dataProtection
            )
        {
            _dataProtection = dataProtection;
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

        public async Task<string> GeneratePassword()
        {
            StringBuilder password = new StringBuilder();
            Random random = new Random();
            var digit = true;
            var lowercase = true;
            var uppercase = true;
            var nonAlphanumeric = true;
            while (password.Length < 8)
            {
                char c = (char)random.Next(32, 126);

                password.Append(c);

                if (char.IsDigit(c))
                    digit = false;
                else if (char.IsLower(c))
                    lowercase = false;
                else if (char.IsUpper(c))
                    uppercase = false;
                else if (!char.IsLetterOrDigit(c))
                    nonAlphanumeric = false;
            }

            if (nonAlphanumeric)
                password.Append((char)random.Next(33, 48));
            if (digit)
                password.Append((char)random.Next(48, 58));
            if (lowercase)
                password.Append((char)random.Next(97, 123));
            if (uppercase)
                password.Append((char)random.Next(65, 91));

            return await Task.FromResult(password.ToString());
        }
    }
}
