
using Client.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client
{
    public partial class CategoriesViewModel : BaseNamedEntityViewModel<Category>
    {
        private readonly CategoryApiService _apiService = new(App.SharedHttpClient);

        protected override Func<Task<List<Category>>> LoadFromApi => () => _apiService.GetAllAsync();
        protected override Func<Category, Task> AddToApi => c => _apiService.AddAsync(c);
        protected override Func<Category, Task> UpdateToApi => c => _apiService.UpdateAsync(c);
        protected override Func<Category, Task> DeleteFromApi => c => _apiService.DeleteAsync(c.Id);
    }
}
