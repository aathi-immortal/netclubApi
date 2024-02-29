using NetClubApi.Model;

namespace NetClubApi.Modules.CourtModule
{
    public interface ICourtDataAccess
    {
        public Task<string> CreateCourt(CourtModel court);
    }
    public class CourtDataAccess : ICourtDataAccess
    {
        private readonly NetClubDbContext netClubDbContext;
        public CourtDataAccess(NetClubDbContext netClubDbContext)
        {
            this.netClubDbContext = netClubDbContext;
        }
        public async Task<string> CreateCourt(CourtModel court)
        {
            await netClubDbContext.AddAsync(court);
            netClubDbContext.SaveChanges();
            return "Court created";
        }


    }
}
