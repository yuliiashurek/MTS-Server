using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Data.Entities
{
    public class Recipient
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }

        public string Name { get; set; } = null!;
        public string Edrpou { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string ContactPerson { get; set; } = null!;
    }

}
