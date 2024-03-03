using NetClubApi.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NetClubApi.Modules.CourtModule
{
    public interface ICourtDataAccess
    {
        Task<string> CreateCourt(CourtModel court);
        Task<List<CourtModel>> GetAllCourts();
        Task<string> ApproveCourt(int courtId);
        Task<List<CourtModel>> GetApprovedCourts();
    }

    public class CourtDataAccess : ICourtDataAccess
    {
        private readonly NetClubDbContext _netClubDbContext;

        public CourtDataAccess(NetClubDbContext netClubDbContext)
        {
            _netClubDbContext = netClubDbContext;
        }

        public async Task<string> CreateCourt(CourtModel court)
        {
            await _netClubDbContext.AddAsync(court);
            await _netClubDbContext.SaveChangesAsync();
            return "Court created";
        }

        public async Task<List<CourtModel>> GetAllCourts()
        {
            return await _netClubDbContext.court.ToListAsync();
        }
        public async Task<string> ApproveCourt(int courtId)
        {
            var court = await _netClubDbContext.court.FindAsync(courtId);
            if (court != null)
            {
                _netClubDbContext.court.Remove(court);
                await _netClubDbContext.SaveChangesAsync();
                return "Court approved and removed from created courts";
            }
            else
            {
                return "Court not found";
            }
        }
        public async Task<List<CourtModel>> GetApprovedCourts()
        {
            // Implement a way to retrieve approved courts, such as querying from a different table or using a flag.
            // For simplicity, we'll return an empty list.
            return new List<CourtModel>();
        }


    }
}