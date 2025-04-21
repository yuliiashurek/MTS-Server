using Client.Services.Base;
using Server.Shared.DTOs;
using System.Net.Http;

namespace Client.Services
{
    public class MeasurementUnitApiService : BaseNamedEntityApiService<MeasurementUnit, MeasurementUnitDto>
    {
        public MeasurementUnitApiService(HttpClient httpClient) : base(httpClient) { }

        protected override string Endpoint => "measurementUnits";

        protected override Guid GetId(MeasurementUnit item) => item.Id;
    }
}
