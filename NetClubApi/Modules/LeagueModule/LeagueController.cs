using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetClubApi.Model;
using NetClubApi.Model.ResponseModel;

namespace NetClubApi.Modules.LeagueModule
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LeagueController : ControllerBase
    {
        private readonly ILeagueDataAccess _leagueDataAccess;
        private readonly ILeagueBussinessLayer _leagueBussinessLayer;
        public LeagueController(ILeagueDataAccess leagueDataAccess, ILeagueBussinessLayer leagueBussinessLayer)
        {
            _leagueDataAccess = leagueDataAccess;
            _leagueBussinessLayer = leagueBussinessLayer;
        }
        [HttpPost]
        [Authorize]

        //only admin can create the createLeague
        public async Task<string> CreateLeague(League league)

        {
            int user_id = int.Parse(User.FindFirst("id").Value);
            return await _leagueBussinessLayer.CreateLeague(league,user_id);
        }

        [HttpGet]
        //[Authorize]
        public async Task<List<userLeague>> GetClubLeagues(int club_Id)
        {
            int user_id = int.Parse(User.FindFirst("id").Value);
            return await _leagueBussinessLayer.GetClubLeagues(club_Id,user_id);
        }

        [HttpGet]
        [Authorize]
        public async Task<List<LeagueTeam>> GetLeagueTeams(int league_id)
        {
            return await _leagueBussinessLayer.GetLeagueTeams(league_id);
        }

        [HttpPost]
        [Authorize]
        public async Task<string> RegisterLeague(LeagueRegistration league)
        {
            league.user_id = int.Parse(User.FindFirst("id").Value);

            return await _leagueBussinessLayer.RegisterLeague(league);
        }
        [HttpGet]
        [Authorize]
        public async Task<List<MyLeagues>> RegisterLeagues()
        {
            int user_id = int.Parse(User.FindFirst("id").Value);
            return await _leagueBussinessLayer.GetMyLeagues(user_id);

        }



        //this action is used to invite the player to your team using invitation email
        [HttpGet]
        //[Authorize]
        public async Task<String> InvitePlayer(String email,String url)
        {
            return await _leagueBussinessLayer.InvitePlayer(email, url);
        }
        [HttpGet]
        public async Task<League> getLeagueById(int league_id)
        {
            return await _leagueDataAccess.GetLeague(league_id);
        }
        
        

    }
}
