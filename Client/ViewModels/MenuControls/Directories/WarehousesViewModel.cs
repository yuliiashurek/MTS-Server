
using Client.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client
{
    public partial class WarehousesViewModel : BaseNamedEntityViewModel<Warehouse>
    {
        private readonly WarehouseApiService _apiService = new(App.SharedHttpClient);

        protected override Func<Task<List<Warehouse>>> LoadFromApi => () => _apiService.GetAllAsync();
        protected override Func<Warehouse, Task> AddToApi => c => _apiService.AddAsync(c);
        protected override Func<Warehouse, Task> UpdateToApi => c => _apiService.UpdateAsync(c);
        protected override Func<Warehouse, Task> DeleteFromApi => c => _apiService.DeleteAsync(c.Id);
    }
}
