using NetClubApi.Model;
using System.ComponentModel.DataAnnotations;

namespace NetClubApi.Model
{
    public class Club
    {
        [Key]
        public int Id { get; set; }
        public string club_name { get; set; }
        public string address1 { get; set; }

        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string created_by { get; set; }
        public string club_label {get;set; }

        //public int total_league { get; set; }
        //public int active_league { get; set; }

        //public int teams { get; set; }
        public DateTime created_date { get; set; }


    }
}
