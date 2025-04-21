
using Client.Services.Base;
using Server.Shared.DTOs;
using System.Net.Http;

namespace Client.Services
{
    public class WarehouseApiService : BaseNamedEntityApiService<Warehouse, WarehouseDto>
    {
        public WarehouseApiService(HttpClient httpClient) : base(httpClient) { }

        protected override string Endpoint => "warehouses";

        protected override Guid GetId(Warehouse item) => item.Id;
    }
}
