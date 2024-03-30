using NetClubApi.Modules.ClubModule;
using NetClubApi.Model;
using NetClubApi.Model.ResponseModel;
using Org.BouncyCastle.Utilities;
using NetClubApi.Modules.LeagueModule;

namespace NetClubApi.Modules.MatchModule
{
    public interface IMatchBusinessLogic
    {
        public Task<string> CreateSchedule(MatchModel match);
        public Task<List<Schedule>> GetSchedule(int league_id);
        public Task<List<Schedule>> getMyMatches(int user_id);
        public Task<String> ScheduleMatch(int clubId, int leagueId);
        public Task<List<MatchModel>> SchedulingLogic(List<TeamModel> listOfTeams, int clubId, int leagueId);
        public Task<int> getTeamPlayerId(TeamModel playerOne);
        public  Task<bool> isAlreadyScheduled(int leagueId);
        Task<string> SaveScore(MatchScoreInputModel inputModel);
        public Task<List<MatchModel>> CourtScheduling(List<MatchModel> matches, List<TeamModel> teams);
    }
    public class MatchBusinessLogic : IMatchBusinessLogic
    {
        private readonly IMatchDataAccess _matchDataAccess;
        private readonly ILeagueBussinessLayer _leagueBussinessLayer;
        public MatchBusinessLogic(IMatchDataAccess matchDataAccess,ILeagueBussinessLayer leagueBussinessLayer)
        {
            _matchDataAccess = matchDataAccess;
            _leagueBussinessLayer = leagueBussinessLayer;
        }
        public async Task<string> CreateSchedule(MatchModel match)
        {
            await _matchDataAccess.CreateMatch(match);
            return "success";
        }

        public async Task<List<Schedule>> GetSchedule(int league_id)
        {
            return await _matchDataAccess.getSchedule(league_id);
        }

        public async Task<List<Schedule>> getMyMatches(int user_id)
        {
            return await _matchDataAccess.getMyMatches(user_id);
        }

        public async Task<String> ScheduleMatch(int clubId, int leagueId)
        {

            //retrive all the teams in the league
            try
            {
                if(! await isAlreadyScheduled(leagueId))
                {
                    Console.WriteLine(clubId + " " + leagueId);
                    List<TeamModel> listOfTeams = await _leagueBussinessLayer.GetLeagueTeams(leagueId);

                    //mathc scheduling
                    List<MatchModel> matches = await SchedulingLogic(listOfTeams, clubId, leagueId);
                    Console.WriteLine("size fo matches   " + matches.Count);
                    //court scheduling
                    await CourtScheduling(matches,listOfTeams);

                    return "Match Scheduled Successfully";
                }
                return "Match is Already Scheduled";
                
            }
            catch(Exception )
            {
                throw ;
            }
            


        }

        public  async Task<bool> isAlreadyScheduled(int leagueId)
        {
            return await _matchDataAccess.isAlreadyScheduled(leagueId);
        }

        public async Task<List<MatchModel>> SchedulingLogic(List<TeamModel> listOfTeams, int clubId, int leagueId)
        {
            List<MatchModel> listOfMatch = new();
            Console.WriteLine(clubId + " " + leagueId);

            List<TeamModel> teamsList = new List<TeamModel>();
            for (int i = 0; i < listOfTeams.Count; i++)
            {
                teamsList.Add(listOfTeams[i]);
            }

            if (teamsList.Count % 2 != 0)
            {
                TeamModel team = new TeamModel();
                team.team_id = -1;
                teamsList.Add(team);
            }

            List<TeamModel> x = teamsList.GetRange(0, teamsList.Count / 2);
            List<TeamModel> y = teamsList.GetRange(teamsList.Count / 2, teamsList.Count / 2);
            List<Dictionary<TeamModel, TeamModel>> matches = new List<Dictionary<TeamModel, TeamModel>>();

            for (int i = 0; i < teamsList.Count - 1; i++)
            {
                Dictionary<TeamModel, TeamModel> round = new Dictionary<TeamModel, TeamModel>();

                if (i != 0)
                {
                    x.Insert(1, y[0]);
                    y.Add(x[x.Count - 1]);
                    x.RemoveAt(x.Count - 1);
                    y.RemoveAt(0);
                }

                matches.Add(round);
                for (int j = 0; j < x.Count; j++)
                {
                    round.Add(x[j], y[j]);
                }
            }


            for (int i = 0; i < matches.Count; i++)
            {
             
                foreach (var pair in matches[i])
                {
                    if(pair.Key.team_id != -1 && pair.Value.team_id != -1 )
                    {
                       MatchModel match = await  MatchModelWrapper(
pair.Key,pair.Value);
                        

                        


                        
                        match.club_id = clubId;
                        match.league_id = leagueId;
                        
                        listOfMatch.Add(match);
                       // Console.WriteLine(match.league_id);
                    }

                }
            }

            



            return listOfMatch;

        }
        private async Task<MatchModel> MatchModelWrapper(TeamModel playerOne,TeamModel playerTwo)
        {

            MatchModel match = new MatchModel();
            
            match.team1_id = playerOne.team_id;
            match.team2_id = playerTwo.team_id;
            
            match.player1_id = await getTeamPlayerId(playerOne);
            match.player2_id = await getTeamPlayerId(playerTwo);
            match.start_date = DateTime.Now;
            match.end_date = DateTime.Now;
            match.court_id = 0;
            match.point = 0;
            match.rating = 0;
            return match;
        }
        public async Task<int> getTeamPlayerId(TeamModel player)
        {
            return await _matchDataAccess.GetTeamPlayerId(player.team_id);
        }

        public async Task<List<MatchModel>> CourtScheduling(List<MatchModel> matches,List<TeamModel> teams)
        {
            //            requirement
            //       court id of the each team in the list of matches

            //hashmap to store the court details

            
            int homeCourtLimit = matches.Count/teams.Count;
            Console.WriteLine("court " + homeCourtLimit);
            Dictionary<int, int> courts = new();
            
            foreach(TeamModel team in teams)
            {
                courts.Add(team.team_id, 0);
            }
            // traverse the MatchModel
            foreach(MatchModel match in matches)
            {
                //if team 1 is not more than limit
                int teamACourt = courts[match.team1_id];
                int teamBCourt = courts[match.team2_id];

                int selectedCourt = match.team1_id;
                if(teamACourt < homeCourtLimit)
                {
                    selectedCourt = match.team1_id;
                    courts[match.team1_id] = teamACourt + 1;
                }
                else if(teamBCourt < homeCourtLimit)
                {
                    selectedCourt = match.team2_id;
                    courts[match.team2_id] = teamBCourt + 1;
                }
                else
                    courts[match.team1_id] = teamACourt + 1;

                
                match.court_id = selectedCourt;
                
                await _matchDataAccess.createMatch(match);


            }
            return matches;
                




        }


        public async Task<string> SaveScore(MatchScoreInputModel inputModel)
        {
            bool setExists = await _matchDataAccess.CheckSetExists(inputModel.MatchId, inputModel.SetNumber);
            if (setExists)
            {
                return "Set number already exists for this match.";
            }

            MatchScore matchScore = new MatchScore
            {
                match_id = inputModel.MatchId,
                set_number = inputModel.SetNumber,
                team1 = inputModel.Team1Score,
                team2 = inputModel.Team2Score
            };

            string result = await _matchDataAccess.SaveScore(matchScore);
            return result;
        }


    }
}
