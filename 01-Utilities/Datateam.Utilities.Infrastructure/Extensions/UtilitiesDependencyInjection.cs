using Microsoft.Extensions.DependencyInjection;
namespace Datateam.Utilities
{
	public static class UtilitiesDependencyInjection
	{
		public static IServiceCollection AddDatateamUtilities(this IServiceCollection services) 
		{
			services.AddScoped<INotificationService, NotificationService>();
			return services;
		}
	}
}
