using Microsoft.Extensions.DependencyInjection;

namespace Datateam.Security
{
    public static class SecurityDependencyInjection
    {
        public static IServiceCollection AddDatateamSecurity(this IServiceCollection services)
        {
            return services;
        }
    }
}
