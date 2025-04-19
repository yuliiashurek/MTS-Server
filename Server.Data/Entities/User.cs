using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Data.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string? PasswordHash { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiryTime { get; set; }

        public Guid OrganizationId { get; set; }
        public Organization Organization { get; set; }

        public string Role { get; set; } // "Admin", "Employee"
    }


}
