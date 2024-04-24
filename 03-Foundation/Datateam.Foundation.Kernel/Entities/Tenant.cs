using Datateam.Utilities;
using System.ComponentModel.DataAnnotations;

namespace Datateam.Foundation
{
	public class Tenant : BaseEntity
	{
        [Key]
        public Guid TenantId { get; set; }
        public string TenantName { get; set; }
        public string? ServerName { get; set; }
    }
}
