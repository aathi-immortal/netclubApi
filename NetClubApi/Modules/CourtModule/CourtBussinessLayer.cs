using NetClubApi.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetClubApi.Modules.CourtModule
{
    public interface ICourtBussinessLayer
    {
        Task<string> CreateCourt(CourtModel court);
        Task<List<CourtModel>> GetAllCourts(); 
        Task<List<CourtModel>> GetApprovedCourts();
        Task ApproveCourt(int courtId);
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
        public async Task ApproveCourt(int courtId)
        {
            await _courtDataAccess.ApproveCourt(courtId);
        }

        public async Task<List<CourtModel>> GetApprovedCourts()
        {
            return await _courtDataAccess.GetApprovedCourts();
        }
    }
}