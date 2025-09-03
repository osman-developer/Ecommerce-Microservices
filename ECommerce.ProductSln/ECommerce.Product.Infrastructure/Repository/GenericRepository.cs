using ECommerce.Common.Entities;
using ECommerce.Common.Interface.Repository;
using ECommerce.Common.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;


namespace ECommerce.Product.Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;
        private readonly ILogger<GenericRepository<T>> _logger;
        public GenericRepository(DbContext context, ILogger<GenericRepository<T>> logger)
        {
            _context = context;
            _dbSet = _context.Set<T>();
            _logger = logger;
        }

        public async Task<Response<T>> CreateAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return Response<T>.Ok(entity, "Created Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating entity of type {EntityType}", typeof(T).Name);
                return Response<T>.Fail(ex.Message);
            }
        }

        public async Task<Response<T>> UpdateAsync(T entity)
        {
            try
            {
                var existingEntity = await _dbSet.FindAsync(entity.Id);
                if (existingEntity == null)
                    return Response<T>.Fail("Entity not found.");

                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();

                return Response<T>.Ok(existingEntity, "Updated Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating entity of type {EntityType} with Id {EntityId}", typeof(T).Name, entity.Id);
                return Response<T>.Fail(ex.Message);
            }
        }


        public async Task<Response<Unit>> DeleteAsync(int id)
        {
            try
            {
                var existingEntity = await _dbSet.FindAsync(id);
                if (existingEntity == null)
                    return Response<Unit>.Fail("Entity not found.");

                _dbSet.Remove(existingEntity);
                await _context.SaveChangesAsync();
                return Response<Unit>.Ok(Unit.Value, "Deleted Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting entity of type {EntityType} with Id {EntityId}", typeof(T).Name, id);
                return Response<Unit>.Fail(ex.Message);
            }
        }

        public async Task<Response<IEnumerable<T>>> GetAllAsync()
        {
            try
            {
                var entities = await _dbSet.AsNoTracking().ToListAsync();
                if (entities == null || !entities.Any())
                    return Response<IEnumerable<T>>.Ok(new List<T>(), "No records found.");

                return Response<IEnumerable<T>>.Ok(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing GetAllAsync for entity of type {EntityType}", typeof(T).Name);
                return Response<IEnumerable<T>>.Fail(ex.Message);
            }
        }

        public async Task<Response<T>> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                return entity != null
                    ? Response<T>.Ok(entity)
                    : Response<T>.Fail("Entity not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing GetByIdAsync for entity of type {EntityType}", typeof(T).Name);
                return Response<T>.Fail(ex.Message);
            }
        }

        public async Task<Response<IEnumerable<T>>> FindByAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var entities = await _dbSet.Where(predicate).ToListAsync();

                return entities.Any()
                    ? Response<IEnumerable<T>>.Ok(entities)
                    : Response<IEnumerable<T>>.Fail("No matching entities found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing FindByAsync for entity of type {EntityType}", typeof(T).Name);
                return Response<IEnumerable<T>>.Fail(ex.Message);
            }
        }

    }
}