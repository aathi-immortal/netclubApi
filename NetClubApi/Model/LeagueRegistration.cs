using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace NetClubApi.Model
{
    public class LeagueRegistration
    {
        [Key]
        public int Id { get; set; }
        public int user_id { get; set; }
        public int club_id { get; set; }
        public int league_id { get; set; }
    }
}
