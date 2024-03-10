using NetClubApi.Model;
using NetClubApi.Model.ResponseModel;

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
    }
}
