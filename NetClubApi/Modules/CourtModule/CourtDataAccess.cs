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
    }
}