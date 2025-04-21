
using Client.Services.Base;
using Server.Shared.DTOs;
using System.Net.Http;

namespace Client.Services
{
    public class CategoryApiService : BaseNamedEntityApiService<Category, CategoryDto>
    {
        public CategoryApiService(HttpClient httpClient) : base(httpClient) { }

        protected override string Endpoint => "categories";

        protected override Guid GetId(Category item) => item.Id;
    }
}
