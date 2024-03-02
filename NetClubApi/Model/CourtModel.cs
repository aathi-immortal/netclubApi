using System.ComponentModel.DataAnnotations;

namespace NetClubApi.Model
{
    public class CourtModel
    {
        [Key]
        public int CourtId { get; set; }
        
        [Required]
        public string? CourtName { get; set; }
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

      
    }
}
