using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Data.Entities
{
    public class MaterialNotificationHistory
    {
        public Guid Id { get; set; }

        public Guid MaterialItemId { get; set; }
        public MaterialItem MaterialItem { get; set; } = null!;

        public string Email { get; set; } = string.Empty;

        public DateTime SentAt { get; set; }

        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }

}
