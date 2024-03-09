using Microsoft.AspNetCore.Mvc;
using NetClubApi.Modules.MatchModule;
using NetClubApi.Model;

namespace NetClubApi.Modules.TeamModule
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamBusinessLogic _teamBussinessLogics;

        public TeamController(ITeamBusinessLogic teamBussinessLogic)
        {
            _teamBussinessLogics = teamBussinessLogic;
        }
        [HttpPost]
        public async Task<int> CreateTeam(TeamModel team)
        {
            int user_id = int.Parse(User.FindFirst("id").Value);
            return await _teamBussinessLogics.CreateTeam(team, user_id);
            ; }

        [HttpPost]
        public async Task<string> AddTeamMembers(AddMember members)
        {
            return await _teamBussinessLogics.AddTeamMember(members);
        }
        [HttpGet]
        public async Task<List<TeamModel>> GetLeagueTeams(int league_id)
        {
            return await _teamBussinessLogics.GetLeagueTeams(league_id);
        }
        [HttpPost]
        public async Task<string> JoinDoubles(TeamDoubles team)
        {
            return await _teamBussinessLogics.JoinDoubles(team);
        }
    }
}
