namespace NetClubApi.Model.ResponseModel
{
    public class RegisterClubModel
    { 
        public int id { get; set; }
        public string club_name { get; set; }
        public string created_by { get; set; }
        public string join_date { get; set; }
        public string club_label{ get; set; }
    }
}
