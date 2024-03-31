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

        [HttpPut("SaveScore")]
        public async Task<IActionResult> SaveScore(MatchScoreInputModel inputModel)
        {
            var result = await _matchBussinessLogics.SaveScore(inputModel);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetMatchScoreSummary(int match_id)
        {
            var result = await _matchBussinessLogics.GetMatchScoreSummary(match_id);

            if (result is string errorMessage)
            {
                return BadRequest(errorMessage); // Return a 400 Bad Request status with the error message
            }

            return Ok(result); 
        }

        [HttpGet]
        // [Authorize]
        public async Task<List<Schedule>> GetMyMatches(int user_id)
        {
            // int user_id = 25;
            return await _matchBussinessLogics.getMyMatches(user_id);
        }

        [HttpGet]
        // schedule action
        public async Task<String> ScheduleMatch(int clubId, int leagueId)
        {
            return await _matchBussinessLogics.ScheduleMatch(clubId,leagueId);
        }
    }
}
