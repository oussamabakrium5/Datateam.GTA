using Datateam.Utilities;
using System.ComponentModel.DataAnnotations;

namespace Datateam.Foundation
{
    public class Organisation : BaseEntity
    {
        [Key]
        public Guid OrganisationId { get; set; }
        public string OrganisationName { get; set; }
    }
}
