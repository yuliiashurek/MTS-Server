using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Data.Entities
{
    public class MeasurementUnit : IOrganizationOwnedEntity
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
    }

}
