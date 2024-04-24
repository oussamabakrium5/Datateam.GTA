using Microsoft.AspNetCore.Identity;

namespace Datateam.Security
{
    public class ApplicationUser : IdentityUser
    {
        public Guid TenantId { get; set; }
        public Guid OrganisationId { get; set; }
    }
}
