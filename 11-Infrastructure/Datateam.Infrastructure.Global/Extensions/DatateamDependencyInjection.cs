using Microsoft.Extensions.DependencyInjection;
using Datateam.Utilities;
using Datateam.Foundation;
using Datateam.Security;
using Microsoft.Extensions.Configuration;
namespace Datateam.Infrastructure
{
	public static class DatateamDependencyInjection
	{
		public static IServiceCollection AddDatateamServices(this IServiceCollection services, IConfiguration config) 
		{
			
			services.AddDatateamUtilities();
			services.AddDatateamSecurity(config);
			services.AddDatateamFoundation(config);
			//services.AddDatateamGTA();

			return services;
		}
	}
}
