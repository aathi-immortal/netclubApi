﻿using Microsoft.Data.SqlClient;
using NetClubApi.Helper;
using NetClubApi.Model;
using System.Text.RegularExpressions;

namespace NetClubApi.Modules.TeamModule
{
    public interface ITeamDataAccess
    {
        public Task<int> CreateTeam(TeamModel team,int user_id);

        public Task<string> AddMember(AddMember members);
        public Task<List<TeamModel>> GetLeagueTeams(int league_id);
        public Task<bool> checkTeamLeague(int league_id, int user_id);
        public Task<bool>  checkLeagueType(int league_id);
        public  Task<string> GetTeamName(int user_id);
    }
    public class TeamDataAccess:ITeamDataAccess
    {
        public Task<int> CreateTeam(TeamModel team,int user_id)
        {
            int insertedId = 0;
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
                            insertedId = Convert.ToInt32(result);
                            string sql2 = $@"INSERT INTO [dbo].[team_member] (team_member_user_id,team_id)
                                   VALUES ('{user_id}','{insertedId}')";
                            using (SqlCommand myCommand2 = new SqlCommand(sql2, myCon))
                            {
                                myCommand2.ExecuteNonQuery();
                            }
                            Console.WriteLine("Inserted row ID: " + insertedId);
                        }
                    }
                    string leagueUpdate = $@"update [dbo].[league] set number_of_teams = number_of_teams + 1 where id = @League_id";
                    using (SqlCommand cmd1 = new SqlCommand(leagueUpdate, myCon))
                    {
                        cmd1.Parameters.AddWithValue("@League_id", team.league_id);
                        cmd1.ExecuteNonQuery();
                        Console.WriteLine("Inserted row ID:1");
                    }

                    string leagueRegistration = $@"INSERT INTO [dbo].[league_registration] (club_id,league_id,user_id)
                                   VALUES ('{team.club_id}','{team.league_id}','{user_id}')";
                    using (SqlCommand cmd3 = new SqlCommand(leagueRegistration, myCon))
                    {
                        cmd3.ExecuteNonQuery();
                        Console.WriteLine("Inserted row ID:2");
                    }


                    myCon.Close();
                }
                return Task.FromResult(insertedId);
            }catch(Exception ex)
            {
                return Task.FromResult(insertedId);
            }
        }

        public Task<string> AddMember(AddMember team)
        {
            try
            {
                int member= team.team_member_user_id;
                    using (SqlConnection myCon = sqlHelper.GetConnection())
                    {
                     myCon.Open();
                     
                    string sql1 = $@"INSERT INTO [dbo].[team_member] (team_member_user_id, team_id)
                                   VALUES ('{member}','{team.team_id}')";
                     using (SqlCommand myCommand1 = new SqlCommand(sql1, myCon))
                     {
                         myCommand1.ExecuteNonQuery();
                     }

                     string leagueUpdate = $@"update [dbo].[league] set number_of_teams = number_of_teams + 1 where id = @League_id";
                    using (SqlCommand cmd1 = new SqlCommand(leagueUpdate, myCon))
                     {
                        cmd1.Parameters.AddWithValue("@League_id", team.league_id);
                        cmd1.ExecuteNonQuery();
                        Console.WriteLine("Inserted row ID:1");
                     }

                    string leagueRegistration = $@"INSERT INTO [dbo].[league_registration] (club_id,league_id,user_id)
                                   VALUES ('{team.club_id}','{team.league_id}','{member}')";
                    using (SqlCommand cmd3 = new SqlCommand(leagueRegistration, myCon))
                    {
                        cmd3.ExecuteNonQuery();
                        Console.WriteLine("Inserted row ID:2");
                    }


                    myCon.Close();

                    }
                
                return Task.FromResult("Team member added");
            }
            catch (Exception ex)
            {
                return Task.FromResult($"Team creation not added{ex.Message}");
            }
        }
        public async Task<bool> checkTeamLeague(int league_id, int user_id)
        {
            try
            {
                using (SqlConnection myCon = sqlHelper.GetConnection())
                {
                    myCon.Open();
                    string sql2 = $@"select [dbo].[team].league_id,[dbo].[team_member].team_member_user_id from [dbo].[team] INNER JOIN [dbo].[team_member] on [dbo].[team].team_id=[dbo].[team_member].team_id where [dbo].[team_member].team_member_user_id={user_id} and [dbo].[team].league_id={league_id}";
                    using (SqlCommand myCommand = new SqlCommand(sql2, myCon))
                    {
                        SqlDataReader reader = myCommand.ExecuteReader();
                        if (reader.HasRows)
                        {
                            Console.WriteLine("Already in League team");
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("new League team");
                            return false;
                            reader.Close();
                        }
                        myCon.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return true;
                throw;
            }
        }
        public async Task<bool> checkLeagueType(int league_id)
        {
            bool isSingle = false;
            try
            {
                using (SqlConnection myCon = sqlHelper.GetConnection())
                {
                    myCon.Open();
                    string sql2 = $@"select [dbo].[league].league_type_id from  [dbo].[league] where [dbo].[league].id={league_id}";
                    using (SqlCommand myCommand = new SqlCommand(sql2, myCon))
                    {
                        SqlDataReader reader = myCommand.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int league_type = (int)reader["league_type_id"];
                                if (league_type == 1 || league_type == 2)
                                {
                                    isSingle=true;
                                }
                                else
                                {
                                    isSingle = false;
                                }
                            }
                        }
                        else
                        {
                            isSingle=false;
                            reader.Close();
                        }
                        myCon.Close();
                    }
                }
                return isSingle;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                isSingle=false;
                return isSingle;
            }
        }
        public async Task<string> GetTeamName(int user_id)
        {
            string user_name = "";
            try
            {
                using (SqlConnection myCon = sqlHelper.GetConnection())
                {
                    myCon.Open();
                    string sql2 = $@"select [dbo].[user_detail].user_name from  [dbo].[user_detail] where [dbo].[user_detail].Id={user_id}";
                    using (SqlCommand myCommand = new SqlCommand(sql2, myCon))
                    {
                        SqlDataReader reader = myCommand.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                user_name= (string)reader["user_name"];
                            }
                        }
                        reader.Close();
                        myCon.Close();
                    }
                }
                return user_name;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return user_name;
            }
        }
        public async Task<List<TeamModel>> GetLeagueTeams(int league_id)
        {
            List<TeamModel> teams = new List<TeamModel>();
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
                                TeamModel team = new TeamModel
                                {
                                     club_id= (int)reader["club_id"],
                                    team_id = (int)reader["team_id"],
                                     team_name = (string)reader[ "team_name"],
                                     court_id = (int)reader["court_id"],
                                     points = (int)reader["points"],
                                     rating = (int)reader["rating"],
                                     league_id = (int)reader["league_id"]
                                     

                    
                                    
                                  
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
                return teams;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }
    }
}