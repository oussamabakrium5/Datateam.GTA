using System.Linq.Expressions;

namespace Datateam.Foundation
{
	public interface ITenantService
	{
		Task<Tenant> AddTenant(Tenant tenant);

		Task<IQueryable<Tenant>?> FindTenant(Expression<Func<Tenant, bool>> predicate);

		Task<IQueryable<Tenant>?> GetAllTenant();
	}
}
