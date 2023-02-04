using System.Data;
using Dapper;
using FreelanceStormer.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FreelanceStormer.Data
{
    public class DirectDbConnection : IDirectDbConnection, IDisposable
    {
        private readonly IFreelanceStormerDbContext _context;

        public DirectDbConnection(
            IFreelanceStormerDbContext context)
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

        public async Task<T> QuerySingleOrDefaultAsync<T>(
            string sql, 
            object? param = null,
            IDbTransaction? transaction = null, 
            CancellationToken cancellationToken = default)
        {
            return await _context.Connection.QuerySingleOrDefaultAsync<T>(
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
