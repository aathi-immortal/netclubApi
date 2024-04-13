using Org.BouncyCastle.Asn1.Mozilla;
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

    public class MatchSetScore
    {
        public int[] SetScores { get; set; }
    }
    public class MatchSetScoreWrapper
    {
        public int MatchId;
        public int Set;
        public int TeamOneScore;
        public int TeamTwoScore;
    }
    public class MatchScoreInputModel : MatchScoreWrapper
    {


        public MatchSetScore TeamOneSetScore { get; set; }

        public MatchSetScore TeamTwoSetScore { get; set; }

    }
    public class MatchScoreWrapper
    {
        public int MatchId { get; set; }
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }

        public int Team1Rating { get; set; }
        public int Team2Rating { get; set; }
        public int WinningTeam { get; set; }
        public int WinByDefault { get; set; }
        public int TeamRetired { get; set; }
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

    public class MatchDetails
    {
        public int match_id { get; set; }
        public int defaultBy { get; set; }
        public int retiredBy { get; set; }
        public MatchSetScore TeamOneSetScore { get; set; }

        public MatchSetScore TeamTwoSetScore { get; set; }


    }
    public class Winner
        {
            public int winner { get; set; }
    public int set { get; set; }
    public int team1SumOfSetScore { get; set; }
    public int team2SumOfSetScore { get; set; }
    }
}
