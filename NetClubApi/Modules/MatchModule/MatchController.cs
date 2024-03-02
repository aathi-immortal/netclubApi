using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NetClubApi.Modules.ClubModule;
using NetClubApi.Comman;
using NetClubApi.Helper;
using NetClubApi.Model;

namespace NetClubApi.Modules.MatchModule
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly IMatchBusinessLogic _matchBussinessLogics;
        //private readonly IMatchDataAccess _matchDataAccess;

        public MatchController(IMatchBusinessLogic matchBussinessLogic)
        {
            _matchBussinessLogics = matchBussinessLogic;
        }
        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> CreateSchedule(MatchModel match)
        {
            string msg = await _matchBussinessLogics.CreateSchedule(match);
            return Ok(msg);
        }

        [HttpGet]
        //[Authorize]
        public async Task<List<Schedule>> GetSchedule(int league_id)
        {
            return await _matchBussinessLogics.GetSchedule(league_id);
        }

        [HttpGet]
        [Authorize]
        public async Task<string> GetStandings(int league_id)
        {
            return "";
        }

        [HttpPut]
        [Authorize]
        public async Task<string> SaveScore(int score)
        {
            return "";
        }

        [HttpGet]
        [Authorize]
        public async Task<string> GetScore(int match_id)
        {
            return "";
        }

        [HttpGet]
        // [Authorize]
        public async Task<List<Schedule>> GetMyMatches(int user_id)
        {
            // int user_id = 25;
            return await _matchBussinessLogics.getMyMatches(user_id);
        }
    }
}
