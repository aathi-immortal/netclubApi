using System.ComponentModel.DataAnnotations;

namespace NetClubApi.Model
{
    public class League
    {
        [Key]
        public int Id { get; set; }
        public string name { get; set; }
        public int? club_id { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public int? league_type_id { get; set; }
        public int? schedule_type_id { get; set; }
        public int? number_of_teams { get; set; }
        public int? number_of_teams_playoffs { get; set; }
        public DateTime? playoff_start_date { get; set; }
        public DateTime? playoff_end_date { get; set; }
        public int? playoff_type_id { get; set; }
        public DateTime? registration_start_date { get; set; }
        public DateTime? registration_end_date { get; set; }
    }
}
