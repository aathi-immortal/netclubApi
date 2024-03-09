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
                // Copy the data of the court
                var approvedCourt = new CourtModel
                {
                    court_name = court.court_name,
                    address1 = court.address1,
                    address2 = court.address2,
                    city = court.city,
                    state = court.state,
                    zip = court.zip
                };

                // Add the copied data to ApprovedCourts
                await _netClubDbContext.court.AddAsync(approvedCourt);

                // Remove the court from the original table
                _netClubDbContext.court.Remove(court);

                await _netClubDbContext.SaveChangesAsync();
                return "Court approved and moved to ApprovedCourts.";
            }
            else
            {
                return "Court not found.";
            }
        }
        public async Task<List<CourtModel>> GetApprovedCourts()
        {
            return await _netClubDbContext.court.ToListAsync(); // Return all courts from the table where approved courts are stored
        }


    }
}