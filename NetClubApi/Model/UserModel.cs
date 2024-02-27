using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetClubApi.Model
{
    public class UserModel
    {

        [Key]
        public int Id { get; set; }

        public string User_name { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }

        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone_number { get; set; }

        public DateTime date_of_birth { get; set; }
        public string gender { get; set; }

        [NotMapped]
        public bool IsSuccess { get; set; }
        [NotMapped]
        public List<string> Message { get; set; }
        [NotMapped]
        public string Token { get; set; }

        
    }
}
