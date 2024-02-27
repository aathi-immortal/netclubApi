using Microsoft.Data.SqlClient;
using NetClubApi.Helper;
using NetClubApi.Model;
using System.Text.RegularExpressions;

namespace NetClubApi.Modules.TeamModule
{
    public interface ITeamDataAccess
    {
        public Task<string> CreateTeam(TeamModel team,int user_id);

        public Task<string> AddMember(AddMember members);
        public Task<List<TeamModel>> GetLeagueTeams(int league_id);
    }
    public class TeamDataAccess:ITeamDataAccess
    {
        public Task<string> CreateTeam(TeamModel team,int user_id)
        {
            try
            {
                using (SqlConnection myCon = sqlHelper.GetConnection())
                {
                    myCon.Open();
                    string sql1 = $@"INSERT INTO [dbo].[team] (club_id, league_id, team_name, court_id,points,rating)
                                   VALUES ('{team.club_id}','{team.league_id}','{team.team_name}',    '{team.court_id}','{team.points}','{team.rating}');
                        SELECT SCOPE_IDENTITY();";
                    using (SqlCommand myCommand1 = new SqlCommand(sql1, myCon))
                    {
                        //myCommand1.ExecuteNonQuery();
                        object result = myCommand1.ExecuteScalar();
                        if (result != null)
                        {
                            int insertedId = Convert.ToInt32(result);
                            string sql2 = $@"INSERT INTO [dbo].[team_member] (team_member_user_id,team_id)
                                   VALUES ('{user_id}','{insertedId}')";
                            using (SqlCommand myCommand2 = new SqlCommand(sql2, myCon))
                            {
                                myCommand2.ExecuteNonQuery();
                            }
                            Console.WriteLine("Inserted row ID: " + insertedId);
                        }
                    }

                    
                    myCon.Close();
                }
                return Task.FromResult("Team created");
            }catch(Exception ex)
            {
                return Task.FromResult("Team creation failed");
            }
        }

        public Task<string> AddMember(AddMember team)
        {

            try
            {
                List<int>members= team.team_member_user_id.ToList();
                int member_count = members.Count;
                if (member_count > 0)
                {
                    using (SqlConnection myCon = sqlHelper.GetConnection())
                    {
                        myCon.Open();
                        for (int i = 0; i < member_count; i++)
                        {
                            string sql1 = $@"INSERT INTO [dbo].[team_member] (team_member_user_id, team_id)
                                   VALUES ('{members[i]}','{team.team_id}')";
                            using (SqlCommand myCommand1 = new SqlCommand(sql1, myCon))
                            {
                                myCommand1.ExecuteNonQuery();
                            }
                        }
                        myCon.Close();
                    }
                }
                return Task.FromResult("Team member added");
            }
            catch (Exception ex)
            {
                return Task.FromResult($"Team creation not added{ex.Message}");
            }
        }
        public async Task<List<TeamModel>> GetLeagueTeams(int league_id)
        {/*
            List<TeamModel> teams= new List<TeamModel>();
            try
            {
                using (SqlConnection myCon = sqlHelper.GetConnection())
                {
                    myCon.Open();
                    string sql2 = $@"SELECT * FROM [dbo].[team] where league_id={league_id}";
                    using (SqlCommand myCommand = new SqlCommand(sql2, myCon))
                    {
                        SqlDataReader reader = myCommand.ExecuteReader();
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                                TeamModel team= new TeamModel
                                {
                                     = (int)reader["match_id"],
                                    team1 = new Team { team_id = (int)reader["team1_id"], team_name = (string)reader["team1name"] },
                                    team2 = new Team { team_id = (int)reader["team2_id"], team_name = (string)reader["team2name"] },
                                    start_date = $"{(DateTime)reader["start_date"]}",
                                    end_date = $"{(DateTime)reader["end_date"]}",
                                    score = (int)reader["point"],
                                    venue = (string)reader["court_name"]
                                };
                                schedules.Add(schedule);
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
            }*/
            return [];
        }
    }
}


/* using (SqlConnection myCon = sqlHelper.GetConnection())
 {
     myCon.Open();
     string sql1 = $@"INSERT INTO [dbo].[team_member] (team_member_user_id, team_id)
                    VALUES ('{team.team_member_user_id[0]}','{team.team_id}')";
     using (SqlCommand myCommand1 = new SqlCommand(sql1, myCon))
     {
         myCommand1.ExecuteNonQuery();
     }
     myCon.Close();
 }*/
