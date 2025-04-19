using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Shared.DTOs
{
    public class RegisterOrganizationDto
    {
        public string OrganizationName { get; set; }
        public string AdminEmail { get; set; }
        public string AdminPassword { get; set; }
    }

}
