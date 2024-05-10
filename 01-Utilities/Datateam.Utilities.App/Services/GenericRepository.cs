using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Datateam.Utilities
{
	public class GenericRepository<TEntity, TDbContext> : IGenericRepository<TEntity, TDbContext>
		where TEntity : class
		where TDbContext : DbContext
	{

		private readonly DbSet<TEntity> _dbSet;
		private readonly TDbContext _appDbContext;
		private readonly ILogger<GenericRepository<TEntity, TDbContext>> _logger;
		public GenericRepository(TDbContext appDbContext, ILogger<GenericRepository<TEntity, TDbContext>> logger)
		{
			_dbSet = appDbContext.Set<TEntity>();
			_appDbContext = appDbContext;
			_logger = logger;
		}

		public async Task<TEntity?> AddAsync(TEntity entity)
		{
			try
			{

				await _dbSet.AddAsync(entity);
				await _appDbContext.SaveChangesAsync();
				return entity;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return null;
			}


			//throw new NotImplementedException();
		}

		public async Task<TEntity?> DeleteAsync(TEntity entity)
		{
			try
			{
                _dbSet.Remove(entity);
                await _appDbContext.SaveChangesAsync();
                return entity;
            }
			catch (Exception ex)
			{
                _logger.LogError(ex.Message);
                return null;
            }
			
		}

		public async Task<IQueryable<TEntity>?> FindAsync(Expression<Func<TEntity, bool>> predicate)
		{
			var entities = _dbSet.Where(predicate);
			var totalEntitiesFound = await entities.CountAsync();
			_logger.LogInformation($"Total {typeof(TEntity)} found : {totalEntitiesFound}");
			return entities;

			//throw new NotImplementedException();
		}

		public async Task<IQueryable<TEntity>?> GetAllAsync()
		{

			var entities = _dbSet;
			var totalEntitiesFound = await entities.CountAsync();
			_logger.LogInformation($"Total {typeof(TEntity)} found : {totalEntitiesFound}");
			return entities;

			//throw new NotImplementedException();
		}

		/*public async Task<TEntity> UpdateAsync(TEntity entity)
		{

			_dbSet.Attach(entity);
			_appDbContext.Entry(entity).State = EntityState.Modified;
			await _appDbContext.SaveChangesAsync();
			return entity;
		}*/
	}
}
