namespace NetClubApi.Model.ResponseModel
{
    public class RegisterClub :IClubResponse
    {
        public string? Name { get; set; }
        public DateTime JoinDate { get; set; }

        public int LeaguesPlayed { get; set; }

        public string? CreatedBy { get; set; }
    }
}
