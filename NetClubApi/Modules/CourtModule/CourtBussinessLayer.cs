using NetClubApi.Model;

namespace NetClubApi.Modules.CourtModule
{
    public interface ICourtBussinessLayer
    {
        public Task<string> CreateCourt(CourtModel court, int court_id);
        Task<string> CreateCourt(CourtModel court);
    }

    public class CourtBusinessLayer : ICourtBussinessLayer
    {
        private readonly ICourtDataAccess _courtDataAccess;
        public async Task<string> CreateCourt(CourtModel court, int court_id)
        {
            return await _courtDataAccess.CreateCourt(court,court_id);
        }

        Task<string> ICourtBussinessLayer.CreateCourt(CourtModel court, int court_id)
        {
            throw new NotImplementedException();
        }

        Task<string> ICourtBussinessLayer.CreateCourt(CourtModel court)
        {
            throw new NotImplementedException();
        }
    }
}
