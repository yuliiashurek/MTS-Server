namespace Server.Shared.DTOs
{
    public class MaterialCurrentStockDto
    {
        public Guid MaterialItemId { get; set; }
        public string MaterialName { get; set; } = string.Empty;
        public string UnitName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string WarehouseName { get; set; } = string.Empty;
        public decimal CurrentStock { get; set; }
        public decimal MinimumStock { get; set; }

        public bool IsCritical => CurrentStock <= MinimumStock;

        public bool IsWarning =>
            !IsCritical && CurrentStock <= MinimumStock * (decimal)1.1;

        public bool IsNormal =>
            !IsCritical && !IsWarning;
    }
}
