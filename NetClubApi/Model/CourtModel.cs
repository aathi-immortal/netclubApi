using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetClubApi.Model
{
    public class CourtModel
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int court_id { get; set; }
        public string court_name { get; set; }
        public string address1 { get; set; }

        public string address2 { get; set; }

        public string city { get; set; }

        public string state { get; set; }

        public string zip { get; set; }

        public bool approved { get; set; }

    }
}
