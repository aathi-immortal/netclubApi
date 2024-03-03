namespace NetClubApi.Model.ResponseModel
{
    public class MyClub:IClubResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? TotalLeagues { get; set; }
        public int? Teams { get; set; }
        public int? ActiveLeagues { get; set; }
        public string? club_label{ get; set; }

    }
}
