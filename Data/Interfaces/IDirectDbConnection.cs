using System.Data;

namespace RestaurantScheduler.Data.Interfaces
{
    public interface IDirectDbConnection 
    {
        Task<IReadOnlyList<T>> QueryAsync<T>(
            string sql, 
            object? param = null, 
            IDbTransaction? transaction = null, 
            CancellationToken cancellationToken = default);
        Task<T> QueryFirstOrDefaultAsync<T>(
            string sql, 
            object? param = null, 
            IDbTransaction? transaction = null, 
            CancellationToken cancellationToken = default);
        Task<T> QuerySingleAsync<T>(
            string sql, 
            object? param = null, 
            IDbTransaction? transaction = null, 
            CancellationToken cancellationToken = default);
        Task<int> ExecuteAsync(
            string sql, 
            object? param = null, 
            IDbTransaction? transaction = null, 
            CancellationToken cancellationToken = default);
    }
}