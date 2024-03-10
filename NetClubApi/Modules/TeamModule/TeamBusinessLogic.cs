using NetClubApi.Modules.MatchModule;
using NetClubApi.Model;
using System.Collections.Generic;
using NetClubApi.Modules.ClubModule;

namespace NetClubApi.Modules.TeamModule
{
    public interface ITeamBusinessLogic
    {
        public Task<int> CreateTeam(TeamModel team, int user_id);
        public Task<string> AddTeamMember(AddMember members);
        public  Task<List<TeamModel>> GetLeagueTeams(int league_id);
        public Task<string> JoinDoubles(TeamDoubles team);
    }

    public class TeamBusinessLogic:ITeamBusinessLogic
    {
        private readonly ITeamDataAccess _teamDataAccess;
        private readonly IClubDataAccess _clubDataAccess;

        public TeamBusinessLogic(ITeamDataAccess teamDataAccess,IClubDataAccess clubDataAccess)
        {
            _teamDataAccess = teamDataAccess;
            _clubDataAccess = clubDataAccess;
        }
        public async Task<int> CreateTeam(TeamModel team,int user_id)
        {
            bool isAlreadyExist = await _teamDataAccess.checkTeamLeague(team.league_id, user_id);
            if (isAlreadyExist)
            {
                //"Already in League Team"
                return -1;
            }
            else
            {
                bool isSingles=await _teamDataAccess.checkLeagueType(team.league_id);
                if (isSingles)
                {
                return await _teamDataAccess.CreateTeam(team,user_id);
                }
                else
                {
                    //"Two player required"
                    return -2;
                }
            }
        }

        public async Task<string> AddTeamMember(AddMember members)
        {
            return await _teamDataAccess.AddMember(members);
        }
        public async Task<List<TeamModel>> GetLeagueTeams(int league_id)
        {
            return await _teamDataAccess.GetLeagueTeams(league_id);
        }

        public async Task<string> JoinDoubles(TeamDoubles team_doubles)
        {
            
            //player 1
            bool isAlreadyExist1 =await _teamDataAccess.checkTeamLeague(team_doubles.league_id,team_doubles.player1);
            if (isAlreadyExist1){return "Player-1 Already exist";}
            
            //player 2
            bool isAlreadyExist2 = await _teamDataAccess.checkTeamLeague(team_doubles.league_id, team_doubles.player2);
            if (isAlreadyExist2){return "Player- 2 Already exist";}
            if (!isAlreadyExist1 && !isAlreadyExist2  && team_doubles.player1!=0 && team_doubles.player2 != 0)
            {
                string teamName =await _teamDataAccess.GetTeamName(team_doubles.player1);
                TeamModel team = new TeamModel
                {
                    team_id = 0,
                    club_id = team_doubles.club_id,
                    league_id = team_doubles.league_id,
                    team_name =  teamName+"|"+team_doubles.team_name,
                    court_id = team_doubles.court_id,
                    points = team_doubles.points,
                    rating = team_doubles.rating
                };

                int my_team_id = await _teamDataAccess.CreateTeam(team, team_doubles.player1);

                string club_label = await _clubDataAccess.getClubLabel(team.club_id);

                Club club = new Club
                {
                    club_label = club_label
                };
                string clubreg = await _clubDataAccess.ClubRegistration(club, team_doubles.player2);

                if (my_team_id != -1 && my_team_id != -2 && clubreg != null)
                {
                    AddMember member = new AddMember
                    {
                        team_id = my_team_id,
                        team_member_user_id = [team_doubles.player2]
                    };
                    await AddTeamMember(member);
                }
            }
            else
            {
                return "Player already in Team";
            }
            return "Hello";
        }
    }
}
