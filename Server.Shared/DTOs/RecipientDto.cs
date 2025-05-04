using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Shared.DTOs
{
    public class RecipientDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string Edrpou { get; set; } = "";
        public string Address { get; set; } = "";
        public string ContactPerson { get; set; } = "";
    }
}
