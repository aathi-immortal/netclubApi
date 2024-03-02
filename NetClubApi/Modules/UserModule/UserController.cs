using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetClubApi.Comman;
using NetClubApi.Model;

namespace NetClubApi.Modules.UserModule
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserDataAccess _dataAccess;
        private readonly IHelper _helper;


        public UserController(IUserDataAccess dataAccess, IHelper helper)
        {
            _dataAccess = dataAccess;
            _helper = helper;
        }

        [HttpGet]
        [Authorize]
        public async Task<string> testing()
        {
            return "hlooo";
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<UserModel> Authentication(UserModel user)
        {
            user = await _dataAccess.AuthenticateUser(user);

            if (user.IsSuccess)
                user.Token = _helper.generateToken(user);
            return user;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<UserModel> Registration(UserModel user)
        {
            user = await _dataAccess.RegisterUser(user);
            if (user.IsSuccess)
                user.Token = _helper.generateToken(user);
            return user;
        }

    }
}
/*
{
    "id": 0,
  "userName": "Abishek",
  "firstName": "Abishek",
  "lastName": "A",
  "password": "Abishek",
  "email": "abishek@gmail.com",
  "phoneNumber": "1234567890",
  "isSuccess": true,
  "message": [
    "string"
  ],
  "token": "string"
}*/
