using NetClubApi.Model;
using NetClubApi.Model.ResponseModel;
using System.Text.RegularExpressions;

namespace NetClubApi.Modules.LeagueModule
{
    public interface ILeagueBussinessLayer
    {
        public Task<string> CreateLeague(League league,int user_id);
        public Task<List<League>> GetClubLeagues(int club_id);
        public Task<List<TeamModel>> GetLeagueTeams(int league_id);
        public Task<List<LeagueResponse>> ConvertToLeagueResponse(List<League> leagues);
        public Task<string> RegisterLeague(LeagueRegistration league);
        public Task<List<MyLeagues>> GetMyLeagues(int user_id);
        public Task<string> InvitePlayer(string email,String url);
        public Task<List<MatchModel>> ScheduleMatch(int clubId, int leagueId);
        public Task<List<MatchModel>> SchedulingLogic(List<TeamModel> listOfTeams,int clubId,int leagueId);
        public Task<int> getTeamPlayerId(TeamModel playerOne);

    }


    public class LeagueBussinessLayer : ILeagueBussinessLayer
    {
        private readonly ILeagueDataAccess _dataAccess;
        private readonly IEmailSender emailSender;

        public LeagueBussinessLayer(ILeagueDataAccess dataAccess,IEmailSender emailSender)
        {
            this._dataAccess = dataAccess;
            this.emailSender = emailSender;
        }
        public async Task<string> CreateLeague(League league, int user_id)
        {
             return await _dataAccess.CreateLeague(league,user_id);
        }
        public async Task<List<League>> GetClubLeagues(int club_id)
        {
            List<League> leagues = await _dataAccess.GetLeagues(club_id);
           // List<LeagueResponse> leagueResponses = await ConvertToLeagueResponse(leagues);
            return leagues;
        }

        public async Task<List<TeamModel>> GetLeagueTeams(int league_id)
        {
            return await _dataAccess.getLeagueTeams(league_id);
        }

        public async Task<List<LeagueResponse>> ConvertToLeagueResponse(List<League> leagues)
        {
            List<LeagueResponse> responses = new();
            foreach (League league in leagues)
            {
                LeagueResponse leagueResponse = new();
                leagueResponse.LeagueName = league.name;
                leagueResponse.StartDate = league.start_date;
                leagueResponse.EndDate = league.end_date;
                leagueResponse.Teams = league.number_of_teams;
                leagueResponse.Matches = 0;
                responses.Add(leagueResponse);
            }
            return responses;

        }

        public async Task<string> RegisterLeague(LeagueRegistration league)
        {
            try
            {

                return await _dataAccess.RegisterLeague(league);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<MyLeagues>> GetMyLeagues(int user_id)
        {
            List<LeagueRegistration> registeredLeagues = await _dataAccess.GetMyLeagues(user_id);
            List<MyLeagues> myLeagues = new();
            foreach (LeagueRegistration league in registeredLeagues)
            {
                MyLeagues myLeague = new();
                myLeague.LeagueName = GetLeagueName(await _dataAccess.GetLeague(league.league_id));
                myLeague.ClubName = GetClubName(await _dataAccess.GetClub(league.club_id));
                myLeague.Win = 0;
                myLeague.Points = 0;
                myLeague.Rank = 12;
                myLeague.Loss = 6;
                myLeague.Rating = 0;
                myLeague.WinPercentage = 20;
                myLeagues.Add(myLeague);

            }
            return myLeagues;
        }

        private string GetClubName(Club club)
        {
            return club.club_name;
        }

        private string GetLeagueName(League league)
        {
            return league.name;
        }

        public async Task<string> InvitePlayer(string email, String url)
        {
            // check is there any user with  the given email  
            //UserModel user = await _dataAccess.GetUserByEmail(email);
            //user is not there
            
            

                
            
            
            

                return await emailSender.SendEmailAsync(email,url);
           
        }

        public async Task<List<MatchModel>> ScheduleMatch(int clubId, int leagueId)
        {
            //retrive all the teams in the league
            List<TeamModel> listOfTeams = await  GetLeagueTeams(leagueId);
            return await SchedulingLogic(listOfTeams,clubId,leagueId);
            

        }

        public async  Task<List<MatchModel>> SchedulingLogic(List<TeamModel> listOfTeams,int clubId,int leagueId)
        {
            List<MatchModel> listOfMatch = new();
            for (int i = 0; i < listOfTeams.Count - 1; i++)
            {
                for (int j = i + 1; j < listOfTeams.Count; j++)
                {
                    TeamModel playerOne = listOfTeams[i];
                    TeamModel playerTwo = listOfTeams[j];
                    MatchModel match = new MatchModel();
                    match.club_id = clubId;
                    match.league_id = leagueId;
                    match.team1_id = playerOne.team_id;
                    match.team2_id = playerTwo.team_id;
                    match.player1_id = await getTeamPlayerId(playerOne);
                    match.player2_id = await getTeamPlayerId(playerTwo);
                    match.start_date = "yet not decided";
                    match.end_date = "yet not decided";
                    match.court_id = 0;
                    match.point = 0;
                    match.rating = 0;
                    match = await _dataAccess.CreateMatch(match);
                    listOfMatch.Add(match);
                        

                }
            }
            return listOfMatch;

        }

        public async Task<int> getTeamPlayerId(TeamModel player)
        {
            return await  _dataAccess.GetTeamPlayerId(player.team_id);
        }

        
    }
}
