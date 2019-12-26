using System;
using Digitus.Trial.Backend.Api.ApiModels;
using Digitus.Trial.Backend.Api.Models;

namespace Digitus.Trial.Backend.Api.Mappers
{
    public class ModelMapper
    {
        public ModelMapper()
        {
        }

        public static User ToModel(UserApiModel apiModel)
        {
            if (apiModel == null) throw new ArgumentNullException("apiModel");

            return new User()
            {
                Email = apiModel.Email,
                FirstName = apiModel.FirstName,
                LastName = apiModel.LastName,
                UserName = apiModel.UserName,
                
            };
        }
        public static UserApiModel ToApiModel(User model)
        {
            if (model == null) throw new ArgumentNullException("model");

            return new UserApiModel()
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Id = model.Id

            };
        }

        public static User ToModel(UserRegisterRequestModel model)
        {
            if (model == null) throw new ArgumentNullException("model");

            return new User()
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Password = model.Password                
            };
        }
    }
}
