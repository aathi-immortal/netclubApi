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

    public class ClubMember
    {
        public int user_id { get; set;}
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email{ get; set; }
        public string phone_number { get; set; }
        public string gender { get; set; }
        public DateTime join_date{ get; set; }
    }
}
