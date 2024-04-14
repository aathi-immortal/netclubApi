using NetClubApi.Modules.ClubModule;
using NetClubApi.Model;
using NetClubApi.Model.ResponseModel;
using Org.BouncyCastle.Utilities;
using NetClubApi.Modules.LeagueModule;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection;

namespace NetClubApi.Modules.MatchModule
{
    public interface IMatchBusinessLogic

    {
        public Task<string> CreateSchedule(MatchModel match);
        public Task<List<Schedule>> GetSchedule(int league_id);
        public Task<List<Schedule>> getMyMatches(int user_id);
        public Task<string> ScheduleMatch(int clubId, int leagueId);
        public Task<List<MatchModel>> SchedulingLogic(List<TeamModel> listOfTeams, int clubId, int leagueId);
        public Task<int> getTeamPlayerId(TeamModel playerOne);
        public  Task<bool> isAlreadyScheduled(int leagueId);
        Task<string> SaveScore(MatchDetails matchDetails);
        Task<MatchScoreSummary> GetMatchScoreSummary(int match_id);
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
            await _matchDataAccess.createMatch(match);
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

        public async Task<string> ScheduleMatch(int clubId, int leagueId)
        {

            //retrive all the teams in the league
            try
            {
                if(! await isAlreadyScheduled(leagueId))
                {
                    
                    List<TeamModel> listOfTeams = await _leagueBussinessLayer.GetLeagueTeams(leagueId);
                    //mathc scheduling
                    List<MatchModel> matchmodels = await SchedulingLogic(listOfTeams, clubId, leagueId);
                    
                    //court scheduling
                    List<MatchModel> matches = await CourtScheduling(matchmodels, listOfTeams);
                    string msg=await createMatches(matches);
                    Console.WriteLine(msg);
                    return msg;
                }
                return "Match is Already Scheduled";
                
            }
            catch(Exception )
            {
                throw ;
            }
        }

        private async Task<string> createMatches(List<MatchModel> matches)
        {
            foreach(MatchModel match in matches)
            {
                await _matchDataAccess.createMatch(match);
            }
            return "Match Scheduled Successfully";

        }

        public  async Task<bool> isAlreadyScheduled(int leagueId)
        {
            return await _matchDataAccess.isAlreadyScheduled(leagueId);
        }

        public async Task<List<MatchModel>> SchedulingLogic(List<TeamModel> listOfTeams, int clubId, int leagueId)
        {
            List<MatchModel> listOfMatch = new();
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

                


            }

            if (notFair(courts, homeCourtLimit))
            {


                // list of minimum home court team matches 
                List<MatchModel> listofminteammatches = getMinTeamMatches(courts, matches, homeCourtLimit);

                //list of maximum home court team mathces
                List<MatchModel> listofmaxteammatches = getMaxTeamMatches(courts, matches, homeCourtLimit);

                int maxteamid = listofmaxteammatches[0].team1_id == listofmaxteammatches[0].court_id ? listofmaxteammatches[0].team1_id : listofmaxteammatches[0].team2_id;


                int minteamid = listofminteammatches[0].team1_id == listofminteammatches[0].court_id ? listofminteammatches[0].team2_id : listofminteammatches[0].team1_id;
                foreach (MatchModel match in listofminteammatches)
                {

                    if (match.team1_id == maxteamid || match.team2_id == maxteamid)
                    {
                        match.court_id = minteamid;
                        break;
                    }
                    else if (match.team1_id == minteamid)
                    {
                        if (search(match.team2_id, listofmaxteammatches,minteamid))
                        {
                            
                            break;
                        }
                    }
                    else
                    {
                        if (search(match.team1_id, listofmaxteammatches,minteamid))
                        {
                            
                            break;
                        }
                    }
                }
            }

            matches=await PlayerIdToCourtId(matches);
            return matches;
                




        }

        private async Task<List<MatchModel>> PlayerIdToCourtId(List<MatchModel> matches)
        {
            
            foreach(MatchModel match in matches)
            {
                match.court_id = await _matchDataAccess.getCourtId(match.court_id);

            }
            return matches;
        }

        private bool search(int team2_id,List<MatchModel> matches,int minTeamId)
        {
            foreach(MatchModel match in matches)
            {
                if(match.team1_id == team2_id || match.team2_id == team2_id)
                {
                    match.court_id = minTeamId ;
                    return true;
                }
            }
            return false;
        }

        private List<MatchModel> getMaxTeamMatches(Dictionary<int, int> courts, List<MatchModel> matches, int limit)
        {
            int maxCourtsTeamId = 0;
            foreach (int court in courts.Keys)
            {
                if (courts[court] > limit)
                {
                    maxCourtsTeamId = court;
                    break;
                }

            }

            List<MatchModel> listOfMatches = new();
            foreach (MatchModel match in matches)
            {


                if (match.team1_id == maxCourtsTeamId || match.team2_id == maxCourtsTeamId)
                {
                    if (match.court_id == maxCourtsTeamId)
                    {
                        listOfMatches.Add(match);
                    }
                }

            }
            return listOfMatches;
        }

            private List<MatchModel> getMinTeamMatches(Dictionary<int, int> courts, List<MatchModel> matches,int limit)
        {
            int minCourtsTeamId = 0;
            foreach (int court in courts.Keys)
            {
                if (courts[court] < limit)
                {
                    minCourtsTeamId = court;
                    break;
                }

            }
            
            List<MatchModel> listOfMatches = new();
            foreach (MatchModel match in matches)
            {


                if (match.team1_id == minCourtsTeamId || match.team2_id == minCourtsTeamId)
                {
                    if (match.court_id != minCourtsTeamId)
                    {
                        listOfMatches.Add(match);
                    }
                }

            }
            return listOfMatches;
        }

        private bool notFair(Dictionary<int, int> courts,int limit)
        {
            foreach(int court in courts.Keys  )
            {
                if(courts[court] < limit)
                {
                    return true;
                }
            }
            return false;
        }


        public async Task<string> SaveScore(MatchDetails matchDetailes)
        {
            
            MatchScoreInputModel inputModel = await calculateScore(matchDetailes);
            MatchScoreWrapper matchScore = new MatchScoreWrapper
            {
                MatchId = inputModel.MatchId,
                Team1Score = inputModel.Team1Score,
                Team2Score = inputModel.Team2Score,
                Team1Rating = inputModel.Team1Rating,
                Team2Rating = inputModel.Team2Rating,
                WinningTeam = inputModel.WinningTeam,
                WinByDefault = inputModel.WinByDefault,
                TeamRetired = inputModel.TeamRetired
               
                
            };
            //MatchScoreInputModel setScore = new MatchScoreInputModel
            //{
            //    MatchId = inputModel.MatchId,
            //    TeamOneSetScore = inputModel.TeamOneSetScore,
            //    TeamTwoSetScore = inputModel.TeamTwoSetScore


            //};

            string result = await _matchDataAccess.SaveMatchScore(matchScore);
            MatchSetScoreWrapper setScore = new MatchSetScoreWrapper
            {
                MatchId = inputModel.MatchId,
                Set = 1,
                TeamOneScore = matchDetailes.TeamOneSetScore.SetScores[0],
                TeamTwoScore = matchDetailes.TeamTwoSetScore.SetScores[0],
                
                
            };

            result = await _matchDataAccess.SaveSetScore(setScore);
            setScore = new MatchSetScoreWrapper
            {
                MatchId = inputModel.MatchId,
                Set = 2,
                TeamOneScore = matchDetailes.TeamOneSetScore.SetScores[1],
                TeamTwoScore = matchDetailes.TeamTwoSetScore.SetScores[1],

            };
            result = await _matchDataAccess.SaveSetScore(setScore);
            int team1score3 = matchDetailes.TeamOneSetScore.SetScores[2];
            int team2score3 = matchDetailes.TeamTwoSetScore.SetScores[2];
            Console.WriteLine(team1score3);
            Console.WriteLine(team2score3);
            if (team1score3!=0 || team2score3 != 0)
            {
                setScore = new MatchSetScoreWrapper
                {
                    MatchId = inputModel.MatchId,
                    Set = 3,
                    TeamOneScore = team1score3,
                    TeamTwoScore = team2score3,
                };

                Console.WriteLine("Hello");
                result = await _matchDataAccess.SaveSetScore(setScore);
            }
            return result;
        }

        public async  Task<MatchScoreInputModel> calculateScore(MatchDetails matchDetailes)
        {
            MatchScoreInputModel result = new();
            
            //case 1 (somebody give default)
            if(matchDetailes.defaultBy != 0)
            {
                result = SetScoreAndRatingForDefault(matchDetailes);
            }
            //case 2 (somebody give retired)
            else if(matchDetailes.retiredBy != 0)
            {
                result = SetScoreAndratingForRetired(matchDetailes);
            }
            //cae3 normal case
            else
            {

                result = SetPointAndRating(matchDetailes);
            }
            result.MatchId = matchDetailes.match_id;
            return result;
        }

        private MatchScoreInputModel SetPointAndRating(MatchDetails matchDetailes)
        {
            Winner winnerDetails = WinnerCalculation(matchDetailes);
            MatchScoreInputModel result = PointAndScoreCalculation(winnerDetails);
            return result;


        }

        private MatchScoreInputModel PointAndScoreCalculation(Winner winnerDetails)
        {
            int team1Point = 0;
            int team2Point = 0;
            int team1Rating = 0;
            int team2Rating = 0;
            if (winnerDetails.winner == 1)
            {
                team1Point = getWinnerPoint(winnerDetails);
                team2Point = getLosserPoint(winnerDetails);
            }
            else
            {
                team1Point = getLosserPoint(winnerDetails);
                team2Point = getWinnerPoint(winnerDetails);

            }
            team1Rating = getRating(team1Point, team2Point);
            team2Rating = getRating(team2Point, team1Point);
            MatchScoreInputModel result = new()
            {
                Team1Score = team1Point,
                Team1Rating = team1Rating,
                Team2Score = team2Point,
                Team2Rating = team2Rating,
                WinningTeam = winnerDetails.winner,
                WinByDefault = 0,
                TeamRetired = 0,
                
                
            };
            return result;
        }

        private int getRating(int team1Point, int team2Point)
        {
            return team1Point - team2Point;
        }

        private int getLosserPoint(Winner winnerDetails)
        {
            if (winnerDetails.winner == 1)
            {
                return winnerDetails.team2SumOfSetScore <= 8 ? winnerDetails.team2SumOfSetScore : 8;
            }
            return winnerDetails.team1SumOfSetScore <= 8 ? winnerDetails.team1SumOfSetScore : 8;
        }

        private int getWinnerPoint(Winner winnerDetails)
        {
            if ((winnerDetails.set) == 2)
            {
                return 14;
            }
            else
            {
                return 12;
            }
        }

        private Winner WinnerCalculation(MatchDetails matchDetailes)
        {
            int teamOneWinningCount = 0;
            int teamTwoWinngCount = 0;
            int winnerTeam = 0;
            // int setArray
            int set =0;
            int team1SumOfSetScore = 0;
             int team2SumOfSetScore = 0;
            while (true)
            {

                int setWinner = getSetWinner(matchDetailes.TeamOneSetScore.SetScores[set], matchDetailes.TeamTwoSetScore.SetScores[set]);
                if (setWinner == 1)
                {
                    teamOneWinningCount += 1;
                }
                else if (setWinner == 2)
                {
                    teamTwoWinngCount += 1;
                }
                else
                {
                    winnerTeam = 0;
                    break;
                }
                winnerTeam = getWinner(teamOneWinningCount, teamTwoWinngCount);

                team1SumOfSetScore += matchDetailes.TeamOneSetScore.SetScores[set];
                team2SumOfSetScore += matchDetailes.TeamTwoSetScore.SetScores[set];

                if (winnerTeam > 0)
                {
                    break;
                }
                set++;


            }
            return new Winner()
            {
                winner = winnerTeam,
                set = set + 1,
                team1SumOfSetScore = team1SumOfSetScore,
                team2SumOfSetScore = team2SumOfSetScore
            };

        }

        private int getWinner(int teamOneWinningCount, int teamTwoWinngCount)
        {
            if(teamOneWinningCount >= 2)
            {
                return 1;
            }
            else if(teamTwoWinngCount >= 2)
            {
                return 2;
            }
            return 0;
        }

        private int getSetWinner(int v1, int v2)
        {
            if (v1 < 0)
            {
                return 0;
            }
            else if (v1 > v2)
            {
                return 1;
            }
            else
                return 2;
        }

        private MatchScoreInputModel SetScoreAndratingForRetired(MatchDetails matchDetailes)
        {
            int team1Point = 0;
            int team2Point = 0;
            int team1Rating = 0;
            int team2Rating = 0;
            Winner winner = WinnerCalculation(matchDetailes);
            team1Point=  winner.team1SumOfSetScore <= 14 ? winner.team1SumOfSetScore : 14;
            team2Point = winner.team2SumOfSetScore <= 14 ? winner.team2SumOfSetScore : 14;
            int winnerTeam= winner.team1SumOfSetScore<winner.team2SumOfSetScore?2:1;
            team1Rating = getRating(team1Point, team2Point);
            team2Rating = getRating(team2Point, team1Point);
            return new MatchScoreInputModel()
            {
                Team1Score = team1Point,
                Team1Rating = team1Rating,
                Team2Score = team2Point,
                Team2Rating = team2Rating,
                WinningTeam = winnerTeam,
                WinByDefault = 0,
                TeamRetired = matchDetailes.retiredBy
            };
        }

        private MatchScoreInputModel SetScoreAndRatingForDefault(MatchDetails matchDetails)
        {
            int team1Point = 0;
            int team2Point = 0;
            int team1Rating = 0;
            int team2Rating = 0;
            int winner = 0;
            if(matchDetails.defaultBy == 1)
            {
                team1Point = 0;
                team2Point = 12;
                winner = 2;

            }
            else
            {
                team2Point = 0;
                team1Point = 12;
                winner = 1;
            }
            team1Rating = 0;
            team2Rating = 0;
            return new MatchScoreInputModel()
            {
                Team1Score = team1Point,
                Team1Rating = team1Rating,
                Team2Score = team2Point,
                Team2Rating = team2Rating,
                WinningTeam = winner,
                WinByDefault = matchDetails.defaultBy,
                TeamRetired = 0
            };
        }

        public async Task<MatchScoreSummary> GetMatchScoreSummary(int match_id)

        {
            return await _matchDataAccess.GetMatchScoreSummary(match_id);
        }
        private int getTeamId(int winner,int match_id)
        {
            //return await _matchDataAccess.getTeamId(winner,match_id);
            return 0;
        }
    }
}
