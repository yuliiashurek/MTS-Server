using Client.Models;
using Client.Services.Base;
using Server.Shared.DTOs;
using System.Net.Http;

namespace Client.Services
{
    public class MaterialItemApiService : BaseNamedEntityApiService<MaterialItem, MaterialItemDto>
    {
        public MaterialItemApiService(HttpClient httpClient) : base(httpClient) { }

        protected override string Endpoint => "materialItems";

        protected override Guid GetId(MaterialItem item) => item.Id;
    }
}
