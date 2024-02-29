using System.ComponentModel.DataAnnotations;

namespace NetClubApi.Model
{
    public class CourtModel
    {
        [Key]
        public int court_id { get; set; }
        public string court_name { get; set; }
        public string address1 { get; set; }

        public string address2 { get; set; }

        public string city { get; set; }

        public string state { get; set; }

        public string zip { get; set; }

      
    }
}
