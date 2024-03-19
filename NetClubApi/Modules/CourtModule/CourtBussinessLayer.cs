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
        Task<List<CourtModel>> SearchCourtsByName(string searchQuery);

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
            var existingCourtWithSameZip = await _courtDataAccess.GetCourtByZip(court.zip);
            if (existingCourtWithSameZip != null)
            {
                return "Court with the same zip code already exists.";
            }
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

        public async Task<List<CourtModel>> SearchCourtsByName(string searchQuery)
        {
            return await _courtDataAccess.SearchCourtsByName(searchQuery);
        }
    }
}