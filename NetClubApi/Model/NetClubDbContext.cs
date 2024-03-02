using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Mozilla;

namespace NetClubApi.Model
{
    public class NetClubDbContext : DbContext
    {

        public NetClubDbContext(DbContextOptions<NetClubDbContext> options):base(options)
        {

        }


        public DbSet<UserModel> User_detail { get; set; } 
        public DbSet<Club> club { get; set; }
        public DbSet<ClubRegistration> club_registration { get; set; }
        public DbSet<League> league { get; set; }
        public DbSet<LeagueRegistration> league_registration { get; set; }

        public DbSet<CourtModel> court { get; set; }

    }
}
