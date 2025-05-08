using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Data.Entities
{
    public class MaterialItem : IOrganizationOwnedEntity
    {
        public Guid Id { get; set; }

        public Guid OrganizationId { get; set; }
        public Organization Organization { get; set; } = null!;

        public string Name { get; set; } = string.Empty;

        public Guid MeasurementUnitId { get; set; }
        public MeasurementUnit MeasurementUnit { get; set; } = null!;

        public decimal MinimumStock { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public Guid SupplierId { get; set; }
        public Supplier Supplier { get; set; } = null!;

        public string NotificationEmails { get; set; } = string.Empty;

        public List<string> GetNotificationEmails()
        {
            return NotificationEmails
                .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToList();
        }
    }
}

