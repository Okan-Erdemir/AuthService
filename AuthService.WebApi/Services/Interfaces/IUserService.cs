using AuthService.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.WebApi.Services.Interfaces
{
    public interface IUserService
    {
        AuthResponseModel Authenticate(AuthRequestModel model);

        UserModel GetById(int id);

        IEnumerable<UserModel> GetAll();
    }
}
