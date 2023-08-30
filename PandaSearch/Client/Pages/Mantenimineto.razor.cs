using Microsoft.AspNetCore.Components;
using PandaSearch.Client.Services;
using PandaSearch.Shared;
using PandaSearch.Shared.enums;
using Radzen;
using Radzen.Blazor;
using System.Diagnostics.Tracing;
using System.Text.RegularExpressions;

namespace PandaSearch.Client.Pages
{
    public partial class Mantenimineto
    {
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] DialogService DialogService { get; set; }
        [Inject] BrandService BrandService { get; set; }
        [Inject] ProductService ProductService { get; set; }

        public bool isLogIn = true;
        public string password;
        public List<Brand> LsBrands;
        public List<Product> LsProducts;
        public RadzenDataGrid<Brand> BrandGrid;
        public RadzenDataGrid<Product> ProductGrid;
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            LsProducts = await ProductService.GetAll();
            LsBrands = await BrandService.GetAll();

        }
        public async Task CheckPassword()
        {
            if (password.Equals("Abrete01!"))
            {
                isLogIn = true;
                LsProducts = await ProductService.GetAll();
                LsBrands = await BrandService.GetAll();
            }
            else
            {
                NavigationManager.NavigateTo("/", true);
            }
        }
        public async Task BrandModal(Brand brand)
        {
            bool isNew = true;
            if (brand != null) isNew = false;
            var aux = await DialogService.OpenAsync<BrandModal>(isNew ? "Add Brand" : "Edit Brand", new Dictionary<string, object> { { "Brand", brand } }, new DialogOptions { ShowClose = true });
            if (isNew && aux!= null)
            {
                BrandGrid.InsertRow(aux);
                LsBrands.Add(aux);
            }
        }
        public async Task ProductModal(Product product)
        {
            bool isNew = true;
            if (product != null) isNew = false;
            var aux = await DialogService.OpenAsync<ProductModal>(isNew ? "Add Product" : "Edit Product", new Dictionary<string, object> { { "Product", isNew ? new Product() :product } }, new DialogOptions { ShowClose = true });
            if (isNew && aux != null)
            {
                ProductGrid.InsertRow(aux);
                LsProducts.Add(aux);
            }
               
        }
        public async Task DeleteBrand(Brand brand)
        {
            var result = await DialogService.OpenAsync<ConfirmModal>($"Do you want to delete {brand.Name}?",null);
            if ( result != null && result == true)
            {
                LsBrands.Remove(brand);
                await BrandService.Delete(brand.Id);
                await BrandGrid.Reload();
            }
           
        }
        public async Task DeleteProduct(Product pro)
        {
            var result = await DialogService.OpenAsync<ConfirmModal>($"Do you want to delete {pro.Name}?", null);
            if (result != null && result == true)
            {
                LsProducts.Remove(pro);
                await ProductService.DeleteProduct(pro.Id);
                await ProductGrid.Reload();
            }

        }
    }
}
