namespace NetClubApi.Model.ResponseModel
{
    public class LeagueResponse
    {

        public string LeagueName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Teams { get; set; }
        public int? Matches { get; set; }
    }
}
