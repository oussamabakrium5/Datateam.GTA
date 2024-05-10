using Datateam.Security;
using Datateam.Utilities;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Datateam.Foundation
{
	public class TenantService : ITenantService
	{
		private readonly IGenericRepository<Tenant, EnterpriseDbContext> _tenantRepository;
        private readonly IIAMService _iIAMService;
		private readonly ILogger<TenantService> _logger;

        public TenantService(IGenericRepository<Tenant, EnterpriseDbContext> tenantRepository, 
            IIAMService iIAMService,
            ILogger<TenantService> logger)
        {
            _tenantRepository = tenantRepository;
            _iIAMService = iIAMService;
            _logger = logger;
        }
        public async Task<Tenant?> AddTenant(Tenant tenant)
		{
			try
			{
                if (tenant.ServerName is null)
                {
                    tenant.ServerName = ".\\SQLExpress";
                }
                var createdTenant = await _tenantRepository.AddAsync(tenant);
                if(createdTenant is not null)
                {
                    try
                    {
                        var user = new RegisterUser
                        {
                            Name = createdTenant.TenantName,
                            TenantId = createdTenant.TenantId
                        };
                        await _iIAMService.RegisterUser(user);
                    }
                    catch (Exception ex)
                    {
                        await _tenantRepository.DeleteAsync(createdTenant);
                    }
                    
                }
                
                return createdTenant;
            }
			catch (Exception ex)
			{

                _logger.LogError(ex.Message);
                return null;
			}
            
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
