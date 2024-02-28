using NetClubApi.Model;

namespace NetClubApi.Modules.CourtModule
{
    public interface ICourtDataAccess
    {
        public Task<string> CreateCourt(CourtModel court, int court_id);
        Task<string> CreateCourt(CourtModel court, object court_id);
    }
    public class CourtDataAccess : ICourtDataAccess
    {
        private readonly NetClubDbContext netClubDbContext;
        public CourtDataAccess(NetClubDbContext netClubDbContext)
        {
            this.netClubDbContext = netClubDbContext;
        }
        public async Task<string> CreateCourt(CourtModel court, int court_id) 
        {
            await netClubDbContext.AddAsync(court);
            netClubDbContext.SaveChanges();
            return "Court created";
        }

        Task<string> ICourtDataAccess.CreateCourt(CourtModel court, int court_id)
        {
            throw new NotImplementedException();
        }

        Task<string> ICourtDataAccess.CreateCourt(CourtModel court, object court_id)
        {
            throw new NotImplementedException();
        }
    }
}
