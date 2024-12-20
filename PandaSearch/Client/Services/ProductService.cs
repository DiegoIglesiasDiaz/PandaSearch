﻿using Microsoft.AspNetCore.Components;
using PandaSearch.Shared;
using System.Net.Http.Json;

namespace PandaSearch.Client.Services
{
    public class ProductService
    {
        private readonly HttpClient _httpClient;
        private readonly HttpClient _httpClientPublic;
        private readonly IHttpClientFactory _ClientFactory;
        private readonly NavigationManager _navigationManager;
        public ProductService(HttpClient httpClient, NavigationManager navigationManager, IHttpClientFactory clientFactory)
        {
            _ClientFactory = clientFactory;
            _httpClient = clientFactory.CreateClient("private");
            _httpClientPublic = clientFactory.CreateClient("public");
            _navigationManager = navigationManager;

        }
        public async Task<List<Product>> GetAll()
        {

            var inf = await _httpClientPublic.GetFromJsonAsync<List<Product>>($"/Product");
            return inf;

        }
        public async Task CreateProduct(Product product)
        {

            await _httpClientPublic.PostAsJsonAsync("/Product", product);

        }
        public async Task UpdateProduct(Product product)
        {
            await _httpClientPublic.PostAsJsonAsync("/Product/Update", product);
        }
        public async Task DeleteProduct(Guid id)
        {

            await _httpClientPublic.GetAsync($"/Product/Delete/{id}");
        }
        public async Task DeleteImage(string status)
        {

            await _httpClientPublic.GetAsync($"/Product/DeleteImage/{status}");
        }
        public async Task<byte[]> GetImgById(Guid id)
        {

           return await _httpClientPublic.GetFromJsonAsync<byte[]>($"/Product/Img/{id}");
        }
    }
}

