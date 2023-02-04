using FreelanceStormer.Models;

namespace FreelanceStormer.Services.Interfaces
{
    public interface IOrganizationsService
    {
        /// <summary>
        /// Get a list of organizations
        /// </summary>
        /// <returns>IReadOnlyList is used because it better represents the nature/function of the collection being returned, avoiding leaky abstractions like IQueryable (which requires
        /// knowledge of the implementation - lazy or not lazy method calls) and is more honest than using IList (which would imply that modifying the list would reflect the changes
        /// in the db, which is not true)</returns>
        Task<IReadOnlyList<Organization>> GetAll(BasicQuery query);
        Task<Organization> Get(int id);
        Task<IReadOnlyList<Organization>> GetAllWithRawSql();
        Task<Organization> GetWithRawSql(int id);
    }
}