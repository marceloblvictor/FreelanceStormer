using System.Data;
using Dapper;
using RestaurantScheduler.Data.Interfaces;

namespace RestaurantScheduler.Data
{
    public class DirectDbConnection : IDirectDbConnection, IDisposable
    {
        private readonly IRestaurantSchedulerDbContext _context;

        public DirectDbConnection(
            IRestaurantSchedulerDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<T>> QueryAsync<T>(
            string sql, 
            object? param = null, 
            IDbTransaction? transaction = null, 
            CancellationToken cancellationToken = default)
        {
            return (await _context.Connection
                .QueryAsync<T>(sql, param, transaction))
                .AsList();
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(
            string sql, 
            object? param = null, 
            IDbTransaction? transaction = null, 
            CancellationToken cancellationToken = default)
        {
            return await _context.Connection.QueryFirstOrDefaultAsync<T>(
                sql, param, transaction);
        }

        public async Task<T> QuerySingleAsync<T>(
            string sql, 
            object? param = null,
            IDbTransaction? transaction = null, 
            CancellationToken cancellationToken = default)
        {
            return await _context.Connection.QuerySingleAsync<T>(
                sql, param, transaction);
        }

        public async Task<int> ExecuteAsync(
            string sql, 
            object? param = null, 
            IDbTransaction? transaction = null,
            CancellationToken cancellationToken = default)
        {
            return await _context.Connection.ExecuteAsync(
                sql, param, transaction);
        }

        public void Dispose()
        {
            _context.Connection.Dispose();
        }
    }
}
