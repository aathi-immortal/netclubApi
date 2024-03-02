using Microsoft.Data.SqlClient;
using NetClubApi.Helper;
using NetClubApi.Model;

namespace NetClubApi.Modules.MatchModule
{
    public interface IMatchDataAccess
    {
        public Task<string> createMatch(MatchModel match);
        public Task<List<Schedule>> getSchedule(int league_id);
        public Task<List<Schedule>> getMyMatches(int user_id);

    }
    public class MatchDataAccess : IMatchDataAccess
    {
        public Task<string> createMatch(MatchModel match)
        {
            try
            {
                using (SqlConnection myCon = sqlHelper.GetConnection())
                {
                    myCon.Open();
                    string sql1 = $@"INSERT INTO [dbo].[match] (club_id, league_id, team1_id, team2_id,player1_id,player2_id,start_date,end_date,court_id,point)
                                   VALUES ('{match.club_id}','{match.league_id}','{match.team1_id}',    '{match.team2_id}','{match.player1_id}','{match.player2_id}','{match.start_date}','{match.end_date}','{match.court_id}',{0})";
                    using (SqlCommand myCommand1 = new SqlCommand(sql1, myCon))
                    {
                        myCommand1.ExecuteNonQuery();
                    }
                    myCon.Close();
                }
                return Task.FromResult("Success");
            }
            catch (Exception ex)
            {
                return Task.FromResult("Failed" + ex.Message);
            }
        }

        public Task<List<Schedule>> getSchedule(int league_id)
        {
            List<Schedule> schedules = new List<Schedule>();
            try
            {
                using (SqlConnection myCon = sqlHelper.GetConnection())
                {
                    myCon.Open();
                    string sql2 = $@"SELECT 
    [dbo].[match].match_id,
    team1_id,
    Team1.team_name Team1name,
    team2_id,
    Team2.team_name Team2name,
    [dbo].[match].start_date start_date,
    [dbo].[match].end_date end_date,
    [dbo].[match].point,
    [dbo].[court].court_name court_name 
FROM 
     [dbo].[match]
INNER JOIN 
     [dbo].[team] Team1 ON  [dbo].[match].team1_id = Team1.team_id
INNER JOIN
 [dbo].[team] Team2 ON  [dbo].[match].team2_id = Team2.team_id
INNER JOIN 
[dbo].[court] ON [dbo].[match].court_id=[dbo].[court].court_id
where [dbo].[match].league_id={league_id}";
                    using (SqlCommand myCommand = new SqlCommand(sql2, myCon))
                    {
                        SqlDataReader reader = myCommand.ExecuteReader();
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                                Schedule schedule = new Schedule
                                {
                                    match_id = (int)reader["match_id"],
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
            }
            return Task.FromResult(schedules);
        }
        public Task<List<Schedule>> getMyMatches(int user_id)
        {
            List<Schedule> matches = new List<Schedule>();
            try
            {
                using (SqlConnection myCon = sqlHelper.GetConnection())
                {
                    myCon.Open();
                    string sql3 = $@"
select [dbo].[match].match_id,
team1.team_id team1_id,team1.team_name team1name,
team2.team_id team2_id,team2.team_name team2name,
[dbo].[match].start_date start_date,
[dbo].[match].end_date end_date,
[dbo].[match].point,
[dbo].[court].court_name court_name 
from [dbo].[match] 
JOIN [dbo].[team] team1 ON team1.team_id=[dbo].[match].team1_id
JOIN [dbo].[team] team2 ON team2.team_id= [dbo].[match].team2_id
JOIN [dbo].[court] ON [dbo].[match].court_id=[dbo].[court].court_id
JOIN[dbo].[team_member] ON team1.team_id= [dbo].[team_member].team_id or team2.team_id= [dbo].[team_member].team_id
where[dbo].[team_member].team_member_user_id={user_id}";
                    using (SqlCommand myCommand = new SqlCommand(sql3, myCon))
                    {
                        SqlDataReader reader = myCommand.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Schedule match = new Schedule
                                {
                                    match_id = (int)reader["match_id"],
                                    team1 = new Team { team_id = (int)reader["team1_id"], team_name = (string)reader["team1name"] },
                                    team2 = new Team { team_id = (int)reader["team2_id"], team_name = (string)reader["team2name"] },
                                    start_date = $"{(DateTime)reader["start_date"]}",
                                    end_date = $"{(DateTime)reader["end_date"]}",
                                    score = (int)reader["point"],
                                    venue = (string)reader["court_name"]
                                };
                                matches.Add(match);
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
            return Task.FromResult(matches);
        }
    }
}
