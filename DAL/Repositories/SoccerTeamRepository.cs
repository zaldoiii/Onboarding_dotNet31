using DAL.Model;

namespace DAL.Repositories
{
    public class SoccerTeamRepository: BaseRepository<SoccerTeam>
    {
        public SoccerTeamRepository(SoccerDbContext dbContext) : base(dbContext)
        {
        }
    }
}
