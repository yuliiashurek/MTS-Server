using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Shared.DTOs
{
    public class MaterialMovementDto : IBaseDto
    {
        public Guid Id { get; set; }

        public Guid OrganizationId { get; set; }

        public Guid MaterialItemId { get; set; }

        public Guid WarehouseId { get; set; }

        public int MovementType { get; set; }

        public decimal Quantity { get; set; }

        public decimal PricePerUnit { get; set; }

        public DateTime MovementDate { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public string BarcodeNumber { get; set; }
    }
}
