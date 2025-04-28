using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public class MaterialMovement
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid MaterialItemId { get; set; }
        public Guid WarehouseId { get; set; }
        public int MovementType { get; set; } // IN = 0, OUT = 1
        public decimal Quantity { get; set; }
        public decimal PricePerUnit { get; set; }
        public DateTime MovementDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string BarcodeNumber { get; set; }

        [NotMapped]
        public string MaterialItemName { get; set; } = string.Empty;
        [NotMapped]
        public string WarehouseName { get; set; } = string.Empty;
        [NotMapped]
        public string CategoryName { get; set; } = string.Empty;
        [NotMapped]
        public string SupplierName { get; set; } = string.Empty;
    }
}
