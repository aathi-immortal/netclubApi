using System.ComponentModel.DataAnnotations;

namespace NetClubApi.Model
{
    public class MatchScore
    {
        [Key]
        public int match_score_id { get; set; }
        public int match_id { get; set; }
        public int set_number { get; set; }

        public int team1 { get; set; }

        public int team2 { get; set; }

    }

    public class MatchScoreInputModel
    {
        public int MatchId { get; set; }
        public int SetNumber { get; set; }
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }
    }

    public class MatchScoreSummary
    {
        public int match_id { get; set; }
        public int league_id { get; set; }
        public int team1_id { get; set; }
        public int team2_id { get; set; }
        public List<MatchSet> sets { get; set; }
       // public List<(int set_number, int team1_score, int team2_score)> sets { get; set; }
    }

    public class MatchSet
    {
        public int set_number { get; set; }
        public int team1score { get; set; }
        public int team2score { get; set; }
    }
}
