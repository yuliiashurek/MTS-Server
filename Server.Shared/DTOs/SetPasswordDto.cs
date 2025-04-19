using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Shared.DTOs
{
    public class SetPasswordDto
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }

}
