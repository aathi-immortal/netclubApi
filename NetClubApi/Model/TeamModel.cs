using System.ComponentModel.DataAnnotations;

namespace NetClubApi.Model
{
    public class TeamModel
    {
        [Key]
        public int team_id { get; set; }
        public int club_id { get; set; }
        public int league_id { get; set; }
        public string team_name { get; set; }
        public int court_id { get; set; }
        public int points { get; set; }
        public int rating { get; set; }
    }
    public class AddMember:TeamModel
    {
        public List<int> team_member_user_id { get; set; }

    }
    public class TeamDoubles: TeamModel
    {
        public int player1 { get; set; }
        public int player2 { get; set; }

    }
}
