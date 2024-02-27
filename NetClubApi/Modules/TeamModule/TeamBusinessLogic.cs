using NetClubApi.Modules.MatchModule;
using NetClubApi.Model;
using System.Collections.Generic;

namespace NetClubApi.Modules.TeamModule
{
    public interface ITeamBusinessLogic
    {
        public Task<string> CreateTeam(TeamModel team, int user_id);
        public Task<string> AddTeamMember(AddMember members);
        public  Task<List<TeamModel>> GetLeagueTeams(int league_id);
    }

    public class TeamBusinessLogic:ITeamBusinessLogic
    {
        private readonly ITeamDataAccess _teamDataAccess;

        public TeamBusinessLogic(ITeamDataAccess teamDataAccess)
        {
            _teamDataAccess = teamDataAccess;
        }
        public async Task<string> CreateTeam(TeamModel team,int user_id)
        {
            return await _teamDataAccess.CreateTeam(team,user_id);
        }

        public async Task<string> AddTeamMember(AddMember members)
        {
            return await _teamDataAccess.AddMember(members);
        }
        public async Task<List<TeamModel>> GetLeagueTeams(int league_id)
        {
            return await _teamDataAccess.GetLeagueTeams(league_id);
        }
    }
}
