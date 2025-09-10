using System.Linq.Expressions;
using ECommerce.Common.Entities;
using ECommerce.Common.Response;

namespace ECommerce.Common.Interface.Repository
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<Response<T>> GetByIdAsync(int id);
        Task<Response<T>> CreateAsync(T entity);
        Task<Response<T>> UpdateAsync(T entity);
        Task<Response<Unit>> DeleteAsync(int id);
        Task<Response<IEnumerable<T>>> GetAllAsync();
        Task<Response<IEnumerable<T>>> FindByAsync(Expression<Func<T, bool>> predicate);
        Task<Response<IEnumerable<T>>> GetIncludingAsync(Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[] includes);

    }
}
