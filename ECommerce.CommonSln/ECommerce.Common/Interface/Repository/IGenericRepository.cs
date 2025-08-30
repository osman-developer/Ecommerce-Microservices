using System.Linq.Expressions;
using ECommerce.Common.Response;

namespace ECommerce.Common.Interface.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<Response<T>> CreateAsync(T entity);
        Task<Response<T>> UpdateAsync(T entity);
        Task<Response<Unit>> DeleteAsync(T entity);
        Task<Response<IEnumerable<T>>> GetAllAsync();
        Task<Response<T>> GetByIdAsync(int id);
        Task<Response<T>> GetByAsync(Expression<Func<T, bool>> predicate);
    }
}
