using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetClubApi.Model;
using NetClubApi.Model.ResponseModel;
using System.Security.Claims;

namespace NetClubApi.Modules.ClubModule
{


    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClubController : ControllerBase
    {



        private readonly IClubBussinessLogics _clubBussinessLogics;
        private readonly IClubDataAccess _clubDataAccess;

        public ClubController(IClubBussinessLogics clubBussinessLogic, IClubDataAccess clubDataAccess)
        {
            _clubBussinessLogics = clubBussinessLogic;
            _clubDataAccess = clubDataAccess;

        }
        // my club action
        [HttpGet]
        [Authorize]
        //get the list of club created by user
        public async Task<IActionResult> MyClubs()
        {
            try
            {

                var userClaims = User.FindFirst("id");
                var id = userClaims.Value;

                List<IClubResponse> clubs = await _clubBussinessLogics.getMyClubs(int.Parse(id));
                if (clubs.Any() && clubs[0] is MyClub)
                {
                    // If the first object is of type MyClub, return a list of MyClub objects
                    var myClub = clubs.Cast<MyClub>().ToList();
                    return Ok(myClub);
                }

                else
                {
                    // If the list is empty or doesn't contain objects of expected types, return an empty response or appropriate status code
                    return Ok("you have no clubs! create it ");
                }



            }
            catch (Exception)
            {
                throw;
            }


        }

        [HttpPost]
        [Authorize]
        //create club
        public async Task<IActionResult> CreateClub(Club club)
        {
            try
            {
                return Ok(await _clubDataAccess.CreateClub(club, int.Parse(User.FindFirst("id").Value)));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Authorize]
        //get the list of club rigistered 
        public async Task<List<RegisterClubModel>> RegisteredClubs()
        {
            var userClaims = User.FindFirst("id");
            int user_id = int.Parse(userClaims.Value);
            return await _clubDataAccess.getRegisteredClubModel(user_id);
              /*  List<RegisterClub> listOfRegisterClubs = new();
            try
            {
                var userClaims = User.FindFirst("id");
                var registerClub = await _clubBussinessLogics.RegisteredClubs(int.Parse(userClaims.Value));
                if (registerClub.Any() && registerClub[0] is RegisterClub)
                {
                    return listOfRegisterClubs = registerClub.Cast<RegisterClub>().ToList();
                }
                else
                {
                    return listOfRegisterClubs;

                }
            }
            catch (Exception)
            {
                return listOfRegisterClubs;

            }*/
        }

        [HttpPost]
        [Authorize]
        //add to club action
        public async Task<IActionResult> JoinClub(Club club)
        {
            try
            {
                var claim = User.FindFirst("id");
                //Console.WriteLine(claim.Value);
                return Ok(await _clubDataAccess.ClubRegistration(club, int.Parse(claim.Value)));
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


    }
}
