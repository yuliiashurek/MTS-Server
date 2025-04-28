using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Shared.DTOs
{
    public class MaterialItemDto : IBaseDto
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid MeasurementUnitId { get; set; }
        public decimal MinimumStock { get; set; }
        public Guid CategoryId { get; set; }
        public Guid SupplierId { get; set; }

    }
}
