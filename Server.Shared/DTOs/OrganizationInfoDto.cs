using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Shared.DTOs
{
    public class OrganizationInfoDto
    {
        //public Guid Id { get; set; }
        public string Name { get; set; }
        public string EdrpouCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string CityForDocs { get; set; } = string.Empty;
        public string FioForDocs { get; set; } = string.Empty;
    }

}
