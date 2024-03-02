using NetClubApi.Modules.ClubModule;
using NetClubApi.Model;
using NetClubApi.Model.ResponseModel;
using Org.BouncyCastle.Utilities;

namespace NetClubApi.Modules.MatchModule
{
    public interface IMatchBusinessLogic
    {
        public Task<string> CreateSchedule(MatchModel match);
        public Task<List<Schedule>> GetSchedule(int league_id);
        public Task<List<Schedule>> getMyMatches(int user_id);
    }
    public class MatchBusinessLogic : IMatchBusinessLogic
    {
        private readonly IMatchDataAccess _matchDataAccess;

        public MatchBusinessLogic(IMatchDataAccess matchDataAccess)
        {
            _matchDataAccess = matchDataAccess;
        }
        public async Task<string> CreateSchedule(MatchModel match)
        {
            return await _matchDataAccess.createMatch(match);
        }

        public async Task<List<Schedule>> GetSchedule(int league_id)
        {
            return await _matchDataAccess.getSchedule(league_id);
        }

        public async Task<List<Schedule>> getMyMatches(int user_id)
        {
            return await _matchDataAccess.getMyMatches(user_id);
        }
    }
}
