using Client.Models;
using Client.Services.Base;
using Server.Shared.DTOs;
using System.Net.Http;

namespace Client.Services
{
    public class MaterialMovementApiService : BaseNamedEntityApiService<MaterialMovement, MaterialMovementDto>
    {
        public MaterialMovementApiService(HttpClient httpClient) : base(httpClient) { }

        protected override string Endpoint => "materialMovements";

        protected override Guid GetId(MaterialMovement item) => item.Id;
    }
}
