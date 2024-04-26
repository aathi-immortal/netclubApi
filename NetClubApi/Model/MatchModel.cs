using System.ComponentModel.DataAnnotations;

namespace NetClubApi.Model
{
    public class MatchModel
    {

        [Key]
        public int match_id { get; set; }
        public int club_id { get; set; }
        public int league_id { get; set; }

        public int team1_id{ get; set; }
        public int  team2_id{ get; set; }
        public int player1_id { get; set; }
        public int player2_id { get; set; }
        public DateTime start_date { get; set; }
        public DateTime  end_date { get; set; }
        public int court_id { get; set; }

        public int point { get; set; }
        public int rating { get; set; }
        public int team1_point { get; set; }
        public int team1_rating{ get; set; }
        public int team2_point { get; set; }
        public int team2_rating{ get; set; }
    }
    public class Schedule
    {
        public int match_id { get; set; }
        public int league_id { get; set; }
        public int winning_team { get; set; }
        public Team? team1 { get; set; }
        public Team? team2 { get; set; }
        public string? start_date { get; set; }
        public string? end_date { get; set; }
        public int? score{ get; set; }
        public string? venue { get; set; }
        public int team1_point { get; set; }
        public int team1_rating { get; set; }
        public int team2_point { get; set; }
        public int team2_rating { get; set; }
        public int player1_id { get; set; }
        public int player2_id { get; set; }
    }
    public class Team
    {
        public int team_id { get; set; }
        public string team_name { get; set; }
    }
    public class MyMatch : MatchModel
    {
        public int winning_team { get; set; }
        public string? venue { get; set; }
        public Team? team1 { get; set; }
        public Team? team2 { get; set; }
    } 
}
