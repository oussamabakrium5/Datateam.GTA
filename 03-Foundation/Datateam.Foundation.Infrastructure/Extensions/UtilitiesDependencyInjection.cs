using Datateam.Foundation;
using Datateam.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Datateam.Foundation
{
	public static class UtilitiesDependencyInjection
	{
		public static IServiceCollection AddDatateamFoundation(this IServiceCollection services, IConfiguration config) 
		{
			services.AddDbContext<EnterpriseDbContext>(options => 
			{
				options.UseSqlServer(config.GetConnectionString("EnterpriseConnection"));
			}) ;
			services.AddScoped<IGenericRepository<Tenant, EnterpriseDbContext>, GenericRepository<Tenant, EnterpriseDbContext>>();
			services.AddScoped<ITenantService, TenantService>();
			return services;
		}
	}
}
