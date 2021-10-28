using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using DAL.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly SoccerDbContext soccerDbContext;

        public IBaseRepository<SoccerTeam> SoccerTeamRepository { get; }
        public IBaseRepository<SoccerCountry> SoccerCountryRepository { get; }

        public UnitOfWork(SoccerDbContext _soccerContext)
        {
            soccerDbContext = _soccerContext;

            SoccerTeamRepository = new BaseRepository<SoccerTeam>(_soccerContext);
            SoccerCountryRepository = new BaseRepository<SoccerCountry>(_soccerContext);
        }

        public void Save()
        {
            soccerDbContext.SaveChanges();
        }

        public Task SaveAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return soccerDbContext.SaveChangesAsync(cancellationToken);
        }

        public IDbContextTransaction StartNewTransaction()
        {
            return soccerDbContext.Database.BeginTransaction();
        }

        public Task<IDbContextTransaction> StartNewTransactionAsync()
        {
            return soccerDbContext.Database.BeginTransactionAsync();
        }

        public Task<int> ExecuteSqlCommandAsync(string sql, object[] parameters, CancellationToken cancellationToken = default(CancellationToken))
        {
            return soccerDbContext.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    soccerDbContext?.Dispose();
                }
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
