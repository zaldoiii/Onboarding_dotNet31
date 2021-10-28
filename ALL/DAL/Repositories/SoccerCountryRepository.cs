using DAL.Model;

namespace DAL.Repositories
{
    public class SoccerCountryRepository : BaseRepository<SoccerTeam>
    {
        public SoccerCountryRepository(SoccerDbContext dbContext) : base(dbContext)
        {
        }
    }
}