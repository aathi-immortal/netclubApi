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
        public Task<MatchModel> CreateMatch(MatchModel match);
        public Task<int> GetTeamPlayerId(int team_id);
        public Task<bool> isAlreadyScheduled(int leagueId);
        Task<bool> CheckSetExists(int matchId, int setNumber);

        public Task<string> SaveMatchScore(MatchScoreWrapper matchScore);
        public Task<int> getCourtId(int court_id);

        
        Task<string> SaveSetScore(MatchSetScoreWrapper setScore);

        public Task<MatchScoreSummary> GetMatchScoreSummary(int matchId);
        public Task<List<MatchScoreSummary>> GetLeagueScores(int league_id);


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
                    string sql3= @"INSERT INTO[match]
                                        (club_id, league_id, team1_id, team2_id, player1_id, player2_id, start_date, end_date, court_id, point, rating)
                                        VALUES
                                        (@ClubId, @LeagueId, @Team1Id, @Team2Id, @Player1Id, @Player2Id, @StartDate, @EndDate, @CourtId, @Point, @Rating)";
                    using (SqlCommand cmd = new SqlCommand(sql3, myCon))
                    {
                        cmd.Parameters.AddWithValue("@ClubId", 38);
                        cmd.Parameters.AddWithValue("@LeagueId",match.league_id);
                        cmd.Parameters.AddWithValue("@Team1Id", match.team1_id);
                        cmd.Parameters.AddWithValue("@Team2Id", match.team2_id);
                        cmd.Parameters.AddWithValue("@Player1Id", match.player1_id);
                        cmd.Parameters.AddWithValue("@Player2Id", match.player2_id);
                        cmd.Parameters.AddWithValue("@StartDate", match.start_date);
                        cmd.Parameters.AddWithValue("@EndDate", match.end_date);
                        cmd.Parameters.AddWithValue("@CourtId", match.court_id);
                        cmd.Parameters.AddWithValue("@Point", match.point);
                        cmd.Parameters.AddWithValue("@Rating", match.rating);

                        Console.WriteLine(match.club_id);
                    Console.WriteLine(match.league_id);
                        cmd.ExecuteNonQuery();
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
[dbo].[match].team1_point,
[dbo].[match].team1_rating,
[dbo].[match].team2_point,
[dbo].[match].team2_rating,
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
                                    venue = (string)reader["court_name"],
                                    team1_point = (int)reader["team1_point"],
                                    team1_rating = (int)reader["team1_rating"],
                                    team2_point = (int)reader["team2_point"],
                                    team2_rating = (int)reader["team2_rating"]
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
[dbo].[match].team1_point,
[dbo].[match].team1_rating,
[dbo].[match].team2_point,
[dbo].[match].team2_rating,
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
                                    venue = (string)reader["court_name"],
                                    team1_point = (int)reader["team1_point"],
                                    team1_rating = (int)reader["team1_rating"],
                                    team2_point = (int)reader["team2_point"],
                                    team2_rating = (int)reader["team2_rating"]
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


        public async Task<MatchModel> CreateMatch(MatchModel match)
        {
            try
            {
                Console.WriteLine("jiiii");
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
                        cmd.Parameters.AddWithValue("@ClubId", 38);
                        cmd.Parameters.AddWithValue("@LeagueId", 1);
                        cmd.Parameters.AddWithValue("@Team1Id", 1);
                        cmd.Parameters.AddWithValue("@Team2Id",1);
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

        public async Task<bool> isAlreadyScheduled(int leagueId)
        {

            try
            {
                using (SqlConnection myCon = sqlHelper.GetConnection())
                {
                    await myCon.OpenAsync();

                    string query = "SELECT COUNT(*) FROM [match] WHERE league_id = @LeagueId";
                    using (SqlCommand cmd = new SqlCommand(query, myCon))
                    {
                        cmd.Parameters.AddWithValue("@LeagueId", leagueId);

                        // Execute the query to check if any rows exist with the specified league ID
                        int count = (int)await cmd.ExecuteScalarAsync();

                        // If count is greater than 0, it means there are rows with the league ID
                        // Return true indicating that scheduling for this league is already done
                        return count > 0;
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

        public async Task<bool> CheckSetExists(int matchId, int setNumber)
        {
            string query = "SELECT COUNT(*) FROM match_score WHERE match_id = @MatchId AND set_number = @SetNumber";

            using (SqlConnection connection = sqlHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@MatchId", matchId);
                command.Parameters.AddWithValue("@SetNumber", setNumber);

                await connection.OpenAsync();
                int count = (int)await command.ExecuteScalarAsync();

                return count > 0;
            }
        }

        public async Task<int> getCourtId(int team_id)
        {
            int court_Id = 0;
            try
            {
                
                using (SqlConnection myCon = sqlHelper.GetConnection())
                {
                    await myCon.OpenAsync();
                    String query = "SELECT court_id from team where team_id = @Team_Id";


                    using (SqlCommand command = new SqlCommand(query, myCon))
                    {
                        command.Parameters.AddWithValue("@Team_Id", team_id);
                        object result = await command.ExecuteScalarAsync();
                        if (result != null)
                        {
                            court_Id = Convert.ToInt32(result);
                        }
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
            return court_Id;
        }

    

        

        public async Task<MatchScoreSummary> GetMatchScoreSummary(int match_id)
        {
            MatchScoreSummary matchScoreSummary = new MatchScoreSummary();
            Boolean flag = true;
            try
            {
                using (SqlConnection myCon = sqlHelper.GetConnection())
                {
                    await myCon.OpenAsync();
                    string sqlQuery = @"
                SELECT
                    [dbo].[match].match_id,
                    [dbo].[match].league_id,
                    team1.team_id AS team1_id,
                    team1.team_name AS team1_name,
                    team2.team_id AS team2_id,
                    team2.team_name AS team2_name,
                    [dbo].[match_score].set_number,
                    [dbo].[match_score].team1 AS team1_score,
                    [dbo].[match_score].team2 AS team2_score
                FROM
                    [dbo].[match]
                INNER JOIN
                    [dbo].[match_score] ON [dbo].[match].match_id = [dbo].[match_score].match_id
                INNER JOIN
                    [dbo].[team] team1 ON [dbo].[match].team1_id = team1.team_id
                INNER JOIN
                    [dbo].[team] team2 ON [dbo].[match].team2_id = team2.team_id
                WHERE
                    [dbo].[match].match_id = @MatchId";

                    using (SqlCommand myCommand = new SqlCommand(sqlQuery, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@MatchId", match_id);
                        using (SqlDataReader reader = await myCommand.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                if (flag)
                                {
                                    
                                    matchScoreSummary = new MatchScoreSummary
                                    {
                                        match_id = (int)reader["match_id"],
                                        league_id = (int)reader["league_id"],
                                        team1_id = (int)reader["team1_id"],
                                        team2_id = (int)reader["team2_id"],
                                        sets = new List<MatchSet>()
                                    };
                                    flag = false;
                                }

                                // Populate the MatchSet object
                                MatchSet matchSet = new MatchSet
                                {
                                    set_number = (int)reader["set_number"],
                                    team1score = (int)reader["team1_score"],
                                    team2score = (int)reader["team2_score"]
                                };

                                // Add the MatchSet to the sets list
                                matchScoreSummary.sets.Add(matchSet);
                            }
                        }
                    }
                }
                return matchScoreSummary;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<List<MatchScoreSummary>> GetLeagueScores(int league_id)
        {
            List<MatchScoreSummary> match_scores = new List<MatchScoreSummary>();
            try
            {
                using (SqlConnection myCon = sqlHelper.GetConnection())
                {
                    myCon.Open();
                    string sql3 = $@"
select [dbo].[match].match_id,[dbo].[match].league_id,[dbo].[match].team1_id,[dbo].[match].team2_id from [dbo].[match] where [dbo].[match].league_id={league_id}";
                    using (SqlCommand myCommand = new SqlCommand(sql3, myCon))
                    {
                        SqlDataReader reader = myCommand.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int match_id = (int)reader["match_id"];
                                MatchScoreSummary match_score = await GetMatchScoreSummary(match_id);
                                if (match_score.league_id == 0)
                                {
                                    match_score.match_id = match_id;
                                    match_score.league_id = (int)reader["league_id"];
                                    int team1IdIndex = reader.GetOrdinal("team1_id");
                                    match_score.team1_id = reader.GetInt32(team1IdIndex);
                                    int team2IdIndex = reader.GetOrdinal("team2_id");
                                    match_score.team2_id = reader.GetInt32(team2IdIndex);



                                    match_scores.Add(match_score);
                                }
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

            return match_scores;

        }

        public async Task<string> SaveMatchScore(MatchScoreWrapper matchScore)
        {
            try
            {
                using (SqlConnection myCon = sqlHelper.GetConnection())
                {
                    await myCon.OpenAsync();
                    string updateQuery = @"
            UPDATE match
            SET 
                team1_point = @Team1Point,
                team2_point = @Team2Point,
                team1_rating = @Team1Rating,
                team2_rating = @Team2Rating,
                winning_team = @WinningTeam,
                win_by_default = @WinByDefault,
                team_retired = @TeamRetired
            WHERE
                match_id = @MatchId;
        ";

                    using (SqlCommand command = new SqlCommand(updateQuery, myCon))
                    {
                        command.Parameters.AddWithValue("@Team1Point", matchScore.Team1Score);
                        command.Parameters.AddWithValue("@Team2Point", matchScore.Team2Score);
                        command.Parameters.AddWithValue("@Team1Rating", matchScore.Team1Rating);
                        command.Parameters.AddWithValue("@Team2Rating", matchScore.Team2Rating);
                        command.Parameters.AddWithValue("@WinningTeam", matchScore.WinningTeam);
                        command.Parameters.AddWithValue("@WinByDefault", matchScore.WinByDefault);
                        command.Parameters.AddWithValue("@TeamRetired", matchScore.TeamRetired);
                        command.Parameters.AddWithValue("@MatchId", matchScore.MatchId);
                        await command.ExecuteNonQueryAsync();
                    }
                }

            }
            catch (Exception ex)
            {

                return ex.ToString();
            }
            return "match table  updated";
        }
        public async Task<string> SaveSetScore(MatchSetScoreWrapper setScore)
        {
            if (setScore.TeamOneScore < 0 || setScore.TeamTwoScore < 0)
            {
                return "updated";
            }
            try
            {
                using (SqlConnection con = sqlHelper.GetConnection())
                {
                    await con.OpenAsync();
                    string query = @"insert into match_score (match_id,set_number,team1,team2) values (@MatchId,@SetNumber,@TeamOne,@TeamTwo)";
                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        command.Parameters.AddWithValue("@MatchId", setScore.MatchId);
                        command.Parameters.AddWithValue("@SetNumber", setScore.Set);
                        command.Parameters.AddWithValue("@TeamOne", setScore.TeamOneScore);
                        command.Parameters.AddWithValue("@TeamTwo", setScore.TeamTwoScore);
                        await command.ExecuteNonQueryAsync();
                    }
                }

            }
            catch (Exception ex)
            {

                return ex.ToString();
            }
            return "setScore inserted";

        }
    }
}


