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
}
