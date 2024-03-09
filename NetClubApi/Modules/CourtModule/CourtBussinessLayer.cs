using NetClubApi.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetClubApi.Modules.CourtModule
{
    public interface ICourtBussinessLayer
    {
        Task<string> CreateCourt(CourtModel court);
        Task<List<CourtModel>> GetAllCourts();
        Task<string> ApproveCourt(int courtId);
        Task<List<CourtModel>> GetApprovedCourts();
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

        public async Task<List<CourtModel>> GetAllCourts()
        {
            return await _courtDataAccess.GetAllCourts();
        }
        public async Task<string> ApproveCourt(int courtId)
        {
            return await _courtDataAccess.ApproveCourt(courtId);
        }

        public async Task<List<CourtModel>> GetApprovedCourts()
        {
            return await _courtDataAccess.GetApprovedCourts();
        }
    }
}