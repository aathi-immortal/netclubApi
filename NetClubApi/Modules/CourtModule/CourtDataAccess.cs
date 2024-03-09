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
        Task<List<CourtModel>> GetApprovedCourts();
        Task ApproveCourt(int courtId);
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
            court.approved = false;
            await _netClubDbContext.AddAsync(court);
            await _netClubDbContext.SaveChangesAsync();
            return "Court created";
        }

        public async Task<List<CourtModel>> GetAllCourts()
        {
            return await _netClubDbContext.court.ToListAsync();
        }
        public async Task ApproveCourt(int courtId)
        {
            var court = await _netClubDbContext.court.FindAsync(courtId);
            if (court != null)
            {
                court.approved = true;
                await _netClubDbContext.SaveChangesAsync();
            }
        }
        public async Task<List<CourtModel>> GetApprovedCourts()
        {
            return await _netClubDbContext.court.Where(c => c.approved).ToListAsync();
        }


    }
}