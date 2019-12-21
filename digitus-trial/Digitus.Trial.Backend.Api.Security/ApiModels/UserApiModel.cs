﻿using System;
namespace Digitus.Trial.Backend.Api.Security.ApiModels
{
    public class UserApiModel
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
