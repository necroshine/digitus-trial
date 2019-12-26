using System;
namespace Digitus.Trial.Backend.Api.Enums
{
    public enum Statuses
    {
        PendingAcitivation = 1,
        Active = 2,
        Disabled = 3,
        Deleted = 4
    }

    public enum UserStatuses
    {
        Online = 1,
        Offline = 2
    }

    public enum UserOperations
    {
        Login = 1
     
    }
}
