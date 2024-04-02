using Microsoft.EntityFrameworkCore;

namespace Datateam.Foundation
{
	public class EnterpriseDbContext : DbContext
	{
		public EnterpriseDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
		{
			
		}
		public DbSet<Tenant> Tenants { get; set; }
	}

}
