using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Datateam.Utilities
{
	public interface IGenericRepository<TEntity, TDbContext>
		where TEntity : class
		where TDbContext : DbContext
	{
		Task<IQueryable<TEntity>?> GetAllAsync();
		//Task<TEntity> DeleteAsync(TEntity entity);
		Task<IQueryable<TEntity>?> FindAsync(Expression<Func<TEntity, bool>> predicate);
		Task<TEntity?> AddAsync(TEntity entity);
		//Task<TEntity> UpdateAsync(TEntity entity);
	}
}
