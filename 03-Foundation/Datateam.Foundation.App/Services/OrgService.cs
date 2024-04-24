using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Datateam.Foundation
{
	public class OrgService : IOrgService
    {

        private readonly ITenantService _iTenantService;

        public OrgService(ITenantService iTenantService)
        {
            _iTenantService = iTenantService;
        }
        public async Task<Tenant> CreatOrgDataBase(Guid tenantId)
        {
            Expression<Func<Tenant, bool>> predicate = tenant => tenant.TenantId == tenantId;
            var Tenants =await _iTenantService.FindTenant(predicate);
            var tenant =await Tenants.FirstOrDefaultAsync();

            string connectionString = $"Server=" + tenant.ServerName +";Integrated Security=True;TrustServerCertificate=True;";

            // Connect to the server
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                // Execute SQL command to create the database
                string createDbQuery = $"CREATE DATABASE " + tenant.TenantName;
                using (SqlCommand command = new SqlCommand(createDbQuery, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
            }

            return tenant;
        }
    }
}
