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
        public Task<bool> AlreadyRegisterd(LeagueRegistration league);
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
            //using (SqlConnection mycon = sqlHelper.GetConnection())
            //{
            //    mycon.Open();
            //    string query = "SELECT TOP 1 1 FROM THE CLUB_REGISTRATION WHERE CLUB_ID = @CLUB_ID AND USER_ID = @USER_ID AND ISADMIN = 1";
            //    using(SqlCommand cmd = new SqlCommand(query,mycon))
            //    {
            //        cmd.Parameters.AddWithValue("@CLUB_ID", club_id);
            //        cmd.Parameters.AddWithValue("@USER_ID", user_id);
            //        Object result = cmd.ExecuteScalar();
            //        if (result != null )
            //        {
            //            return true;
            //        }
            //        return false;
            //    }
            //}

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
                                TeamModel team = new TeamModel
                                {
                                    team_id = (int)reader["team_id"],
                                    club_id = (int)reader["club_id"],
                                    league_id = (int)reader["league_id"],
                                    team_name = (string)reader["team_name"],
                                    court_id = (int)reader["court_id"],
                                    points = (int)reader["points"],
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

        public async Task<bool> AlreadyRegisterd(LeagueRegistration league)
        {

            LeagueRegistration registeredLeague =  await netClubDbContext.league_registration.FirstOrDefaultAsync(leagues => leagues.user_id == league.user_id);
            if (registeredLeague == default)
                return false;
            return true;

        }
    }
}

