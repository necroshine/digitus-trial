using System;
using System.Threading.Tasks;

namespace Digitus.Trial.Backend.Api.Interfaces
{
    public interface INotificationManager
    {
        Task SendMail(string sender, string receiver, string subject, string body);

        string GenerateActivationMailBody(string activationCode);
    }
}
