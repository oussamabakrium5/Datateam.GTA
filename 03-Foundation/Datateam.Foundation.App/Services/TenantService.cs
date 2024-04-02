using Datateam.Utilities;
using System.Linq.Expressions;

namespace Datateam.Foundation
{
	public class TenantService : ITenantService
	{
		private readonly IGenericRepository<Tenant, EnterpriseDbContext> _tenantRepository;

		public TenantService(IGenericRepository<Tenant, EnterpriseDbContext> tenantRepository)
		{
			_tenantRepository = tenantRepository;
		}
		public async Task<Tenant> AddTenant(Tenant tenant)
		{
			return await _tenantRepository.AddAsync(tenant);
		}

		public async Task<IQueryable<Tenant>?> FindTenant(Expression<Func<Tenant, bool>> predicate)
		{
			return await _tenantRepository.FindAsync(predicate);
		}

		public async Task<IQueryable<Tenant>?> GetAllTenant()
		{
			return await _tenantRepository.GetAllAsync();
		}
	}
}
