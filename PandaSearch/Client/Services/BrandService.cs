using Microsoft.AspNetCore.Components;
using PandaSearch.Shared;
using System.Net.Http.Json;

namespace PandaSearch.Client.Services
{
    public class BrandService
    {
        private readonly HttpClient _httpClient;
        private readonly HttpClient _httpClientPublic;
        private readonly IHttpClientFactory _ClientFactory;
        private readonly NavigationManager _navigationManager;
        public BrandService(HttpClient httpClient, NavigationManager navigationManager, IHttpClientFactory clientFactory)
        {
            _ClientFactory = clientFactory;
            _httpClient = clientFactory.CreateClient("private");
            _httpClientPublic = clientFactory.CreateClient("public");
            _navigationManager = navigationManager;

        }
        public async Task<List<Brand>> GetAll()
        {

            var inf = await _httpClientPublic.GetFromJsonAsync<List<Brand>>($"/Brand");
            return inf;

        }
        public async Task CreateBrand(Brand brand)
        {

            await _httpClientPublic.PostAsJsonAsync("/Brand", brand);

        }
        public async Task UpdateBrand(Brand brand)
        {

            await _httpClientPublic.PostAsJsonAsync("/Brand/Update", brand);

        }
        public async Task Delete(Guid id)
        {
           
           await _httpClientPublic.GetAsync($"/Brand/Delete/{id}");
        }
    }
}
