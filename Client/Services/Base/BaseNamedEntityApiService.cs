using AutoMapper;
using System.Net.Http;
using System.Net.Http.Json;

namespace Client.Services.Base
{
    public abstract class BaseNamedEntityApiService<TModel, TDto>
    {
        protected readonly HttpClient _httpClient;
        protected readonly IMapper _mapper;

        protected abstract string Endpoint { get; }

        protected BaseNamedEntityApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _mapper = App.Mapper;
        }

        public async Task<List<TModel>> GetAllAsync()
        {
            var dtos = await _httpClient.GetFromJsonAsync<List<TDto>>(Endpoint);
            return _mapper.Map<List<TModel>>(dtos);
        }

        public async Task<TModel?> GetByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"{Endpoint}/{id}");
            response.EnsureSuccessStatusCode();
            var model = await response.Content.ReadFromJsonAsync<TModel>();
            return model;
        }


        public async Task<TModel> AddAsyncAndReturn(TModel item)
        {
            var dto = _mapper.Map<TDto>(item);

            var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/create-and-return", dto);
            response.EnsureSuccessStatusCode();
            var returnedDto = await response.Content.ReadFromJsonAsync<TDto>();
            return _mapper.Map<TModel>(returnedDto);
        }

        public async Task AddAsync(TModel item)
        {
            var dto = _mapper.Map<TDto>(item);
            var response = await _httpClient.PostAsJsonAsync(Endpoint, dto);

            response.EnsureSuccessStatusCode();
        }



        public async Task UpdateAsync(TModel item)
        {
            var dto = _mapper.Map<TDto>(item);
            await _httpClient.PutAsJsonAsync($"{Endpoint}/{GetId(item)}", dto);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _httpClient.DeleteAsync($"{Endpoint}/{id}");
        }

        protected abstract Guid GetId(TModel item);
    }
}
