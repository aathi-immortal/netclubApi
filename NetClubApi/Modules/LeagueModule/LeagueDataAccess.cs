using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NetClubApi.Helper;
using NetClubApi.Model;
using NetClubApi.Model.ResponseModel;
using Org.BouncyCastle.Utilities.IO;
using System.Text.RegularExpressions;

namespace NetClubApi.Modules.LeagueModule
{


    public interface ILeagueDataAccess
    {
        public Task<string> CreateLeague(League league, int user_id);
        public Task<League> GetLeague(int league_id);
        public Task<List<userLeague>> GetLeagues(int club_id,int user_id);
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

        public async Task<List<userLeague>> GetLeagues(int club_Id,int user_id)
        {
            List<userLeague> leagues = new List<userLeague>();
            
            using (SqlConnection myCon = sqlHelper.GetConnection())
            {
             
        myCon.Open();
    string sqlquery = $@"
 select 
[dbo].[league].id ,
[dbo].[league].name ,
[dbo].[league].club_id ,
[dbo].[league].start_date ,
[dbo].[league].end_date ,
[dbo].[league].league_type_id ,
[dbo].[league].schedule_type_id ,
[dbo].[league].number_of_teams ,
[dbo].[league].number_of_teams_playoffs ,
[dbo].[league].playoff_start_date ,
[dbo].[league].playoff_end_date ,
[dbo].[league].playoff_type_id ,
[dbo].[league].registration_start_date ,
[dbo].[league].registration_end_date,
[dbo].[league_registration].user_id 
from [dbo].[league] 
left join [dbo].[league_registration] on 
[dbo].[league_registration].league_id= [dbo].[league].id 
and [dbo].[league_registration].user_id ={user_id}
where [dbo].[league].club_id = '{club_Id}'
    ";
                    using (SqlCommand myCommand1 = new SqlCommand(sqlquery, myCon))
                    {
                       SqlDataReader reader= myCommand1.ExecuteReader();
                        if (reader.HasRows)
                        {
                        while(reader.Read())
                        {
                            userLeague league = new userLeague {
                            Id = (int)reader["id"],
                            name = (string)reader["name"],
                            club_id = (int)reader["club_id"],
                            start_date = (DateTime)reader["start_date"],
                            end_date = (DateTime)reader["end_date"],
                            league_type_id = (int)reader["league_type_id"],
                            schedule_type_id = (int)reader["schedule_type_id"],
                            number_of_teams = (int)reader["number_of_teams"],
                            number_of_teams_playoffs = (int)reader["number_of_teams_playoffs"],
                            playoff_start_date = (DateTime)reader["playoff_start_date"],
                            playoff_end_date = (DateTime)reader["playoff_end_date"],
                            playoff_type_id = (int)reader["playoff_type_id"],
                            registration_start_date = (DateTime)reader["registration_start_date"],
                            registration_end_date = (DateTime)reader["registration_end_date"],
                            is_join= !reader.IsDBNull(14)
                            };
                        leagues.Add(league);
                        }
                    }
                    }
                myCon.Close();
            }
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
        private async Task<int> getLeagueOwner(int league_id)
        {
            int leagueOwner = 0;
            try
            {
                using (SqlConnection myCon = sqlHelper.GetConnection())
                {
                    myCon.Open();
                    string sql2 = $@"
select  [dbo].[club_registration].user_id,[dbo].[club_registration].club_id,[dbo].[club_registration].isadmin,[dbo].[league].id from [dbo].[league]
inner join [dbo].[club_registration] on [dbo].[club_registration].club_id=[dbo].[league].club_id and [dbo].[club_registration].isadmin=1
where league.id={league_id}";
                    using (SqlCommand myCommand = new SqlCommand(sql2, myCon))
                    {
                        SqlDataReader reader = myCommand.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                leagueOwner=(int)reader["user_id"];
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
            return leagueOwner;
        }
    }
}

