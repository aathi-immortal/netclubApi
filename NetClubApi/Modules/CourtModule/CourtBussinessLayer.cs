using NetClubApi.Model;

namespace NetClubApi.Modules.CourtModule
{
    public interface ICourtBussinessLayer
    {
        public Task<string> CreateCourt(CourtModel court);
    }

    public class CourtBusinessLayer : ICourtBussinessLayer
    {
        private readonly ICourtDataAccess _courtDataAccess;

        public CourtBusinessLayer(ICourtDataAccess courtDataAccess)
        {
            _courtDataAccess = courtDataAccess;
        }
        public async Task<string> CreateCourt(CourtModel court)
        {
            return await _courtDataAccess.CreateCourt(court);
        }
    }
}

