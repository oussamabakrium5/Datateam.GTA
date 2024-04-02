namespace Datateam.Utilities
{
	public abstract class BaseEntity
	{
        public Guid Id { get; set; }
		public Guid TenantId { get; set; }
		public Guid OrgId { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public DateTime LastUpdatedAt { get; set;}	= DateTime.Now;
		public DateTime CreatedAtUTC { get; set; } = DateTime.UtcNow;
		public DateTime LastUpdatedAtUTC { get; set; } = DateTime.UtcNow;
		public Guid CreatedBy { get; set; } = Guid.Empty;
		public Guid LastUpdatedBy { get; set; } = Guid.Empty;
	}
}
