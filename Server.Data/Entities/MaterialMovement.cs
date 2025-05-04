using Server.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Data.Entities
{
    public class MaterialMovement : IOrganizationOwnedEntity
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }

        public Guid MaterialItemId { get; set; }
        public MaterialItem MaterialItem { get; set; }

        public Guid WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }

        public int MovementType { get; set; }

        public decimal Quantity { get; set; }

        public decimal PricePerUnit { get; set; }

        public DateTime MovementDate { get; set; }

        public DateTime? ExpirationDate { get; set; }
        public string BarcodeNumber { get; set; } = string.Empty;

        public Guid? RecipientId { get; set; }
        public Recipient? Recipient { get; set; }


    }
}
