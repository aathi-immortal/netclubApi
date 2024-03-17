using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NetClubApi.Helper;
using NetClubApi.Model;
using NetClubApi.Model.ResponseModel;
using System.Text.RegularExpressions;

namespace NetClubApi.Modules.LeagueModule
{


    public interface ILeagueDataAccess
    {
        public Task<string> CreateLeague(League league, int user_id);
        public Task<League> GetLeague(int league_id);
        public Task<List<League>> GetLeagues(int club_id);
        Task<List<TeamModel>> getLeagueTeams(int league_id);
        public Task<List<LeagueRegistration>> GetMyLeagues(int user_id);
        Task<int?> getNumberOfMatches(int league_id);
        public Task<bool> IsAdmin(int? club_id, int user_id);
        public Task<string> RegisterLeague(LeagueRegistration league);
        public Task<Club> GetClub(int club_id);
        public Task<UserModel> GetUserByEmail(String email);
        public Task<MatchModel> CreateMatch(MatchModel match);
        public Task<int> GetTeamPlayerId(int team_id);
    }
    public class LeagueDataAccess : ILeagueDataAccess
    {
        private readonly NetClubDbContext netClubDbContext;
        public LeagueDataAccess(NetClubDbContext netClubDbContext)
        {
            this.netClubDbContext = netClubDbContext;
        }

        public async Task<string> CreateLeague(League league, int user_id)
        {
            try
            {
                if (await IsAdmin(league.club_id, user_id))
                {
                    await netClubDbContext.AddAsync(league);
                    netClubDbContext.SaveChanges();
                    return "League created";
                }
                return "League not created";
            }
            catch (Exception)
            {
                throw;
            }



        }

        public async Task<bool> IsAdmin(int? club_id, int user_id)
        {
            var club = await netClubDbContext.club_registration.FirstOrDefaultAsync(club => club.club_id == club_id && club.user_id == user_id && club.isadmin);
            if (club == default)
                return false;
            return true;
        }

        public async Task<List<League>> GetLeagues(int club_Id)
        {
            List<League> leagues = await netClubDbContext.league.Where(league => league.club_id == club_Id).ToListAsync();
            return leagues;

        }

        public async Task<List<TeamModel>> getLeagueTeams(int league_id)
        {
            //League league = await netClubDbContext.league.FirstOrDefaultAsync(league => league.Id == league_id);
            List<TeamModel> teams = new List<TeamModel>();
            try
            {
                using (SqlConnection myCon = sqlHelper.GetConnection())
                {
                    myCon.Open();
                    string sql2 = $@"select * from [dbo].[team] where league_id={league_id}";
                    using (SqlCommand myCommand = new SqlCommand(sql2, myCon))
                    {
                        SqlDataReader reader = myCommand.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                TeamModel team= new TeamModel
                                {
                                    team_id = (int)reader["team_id"],
                                    club_id = (int)reader["club_id"],
                                    league_id = (int)reader["league_id"],
                                    team_name = (string)reader["team_name"],
                                    court_id = (int)reader["court_id"],
                                    points= (int)reader["points"],
                                    rating = (int)reader["rating"],
                                };
                                teams.Add(team);
                            }
                        }
                        else
                        {
                            reader.Close();
                        }
                        myCon.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return teams;
        }

        public async Task<int?> getNumberOfMatches(int league_id)
        {
            return await netClubDbContext.league.CountAsync(league => league.Id == league_id);
        }

        public async Task<string> RegisterLeague(LeagueRegistration league)
        {
            try
            {
                await netClubDbContext.league_registration.AddAsync(league);
                await netClubDbContext.SaveChangesAsync();
                return "register";

            }
            catch (Exception)
            {
                return "NotRegister";
            }
        }

        public async Task<List<LeagueRegistration>> GetMyLeagues(int user_id)
        {
            return await netClubDbContext.league_registration.Where(league => league.user_id == user_id).ToListAsync();
        }

        public async Task<League> GetLeague(int league_id)
        {
            return await netClubDbContext.league.FirstOrDefaultAsync(league => league.Id == league_id);
        }

        public async Task<Club> GetClub(int club_id)
        {
            return await netClubDbContext.club.FirstAsync(club => club.Id == club_id);
        }

        public async Task<UserModel> GetUserByEmail(string email)
        {
            return await netClubDbContext.User_detail.FirstOrDefaultAsync(user => user.Email == email);            

        }

        public async Task<MatchModel> CreateMatch(MatchModel match)
        {
            try
            {
                using (SqlConnection myCon = sqlHelper.GetConnection())
                {
                    await myCon.OpenAsync();

                    string insertQuery = @"INSERT INTO [match] 
                                        (club_id, league_id, team1_id, team2_id, player1_id, player2_id, start_date, end_date, court_id, point, rating) 
                                        VALUES 
                                        (@ClubId, @LeagueId, @Team1Id, @Team2Id, @Player1Id, @Player2Id, @StartDate, @EndDate, @CourtId, @Point, @Rating); 
                                        SELECT SCOPE_IDENTITY();";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, myCon))
                    {
                        cmd.Parameters.AddWithValue("@ClubId", match.club_id);
                        cmd.Parameters.AddWithValue("@LeagueId", match.league_id);
                        cmd.Parameters.AddWithValue("@Team1Id", match.team1_id);
                        cmd.Parameters.AddWithValue("@Team2Id", match.team2_id);
                        cmd.Parameters.AddWithValue("@Player1Id", match.player1_id);
                        cmd.Parameters.AddWithValue("@Player2Id", match.player2_id);
                        cmd.Parameters.AddWithValue("@StartDate", match.start_date);
                        cmd.Parameters.AddWithValue("@EndDate", match.end_date);
                        cmd.Parameters.AddWithValue("@CourtId", match.court_id);
                        cmd.Parameters.AddWithValue("@Point", match.point);
                        cmd.Parameters.AddWithValue("@Rating", match.rating);

                        // Execute the command and retrieve the newly inserted match ID
                        int matchId = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                        // Set the match_id property of the match object
                        match.match_id = matchId;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Optionally, you might want to throw the exception to propagate it further
                throw;
            }

            return match;

        }

        public async Task<int> GetTeamPlayerId(int team_id)
        {
            try
            {
                using (SqlConnection myCon = sqlHelper.GetConnection())
                {
                    await myCon.OpenAsync();

                    string selectQuery = "SELECT team_member_user_id FROM team_member WHERE team_id = @TeamId";

                    using (SqlCommand cmd = new SqlCommand(selectQuery, myCon))
                    {
                        cmd.Parameters.AddWithValue("@TeamId", team_id);


                        object result = await cmd.ExecuteScalarAsync();

                        if (result != null && result != DBNull.Value)
                        {

                            return Convert.ToInt32(result);
                        }
                        else
                        {
                            return -1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Optionally, you might want to throw the exception to propagate it further
                throw;
            }
        }


        }
}
