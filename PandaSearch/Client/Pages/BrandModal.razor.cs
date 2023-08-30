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
    public partial class BrandModal
    {
        [Inject] DialogService DialogService { get; set; }
        [Inject] NotificationService NotificationService { get; set; }
        [Inject] BrandService BrandService { get; set; }
        [Parameter] public Brand brand { get; set; }
        public string name;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (brand != null)
            {
                name = brand.Name;
            }
            else
            {
                brand = new Brand();
            }
        }
        public async Task Apply()
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                NotificationService.Notify(NotificationSeverity.Error, "Error", "Name is a required field");
                return;
            }
            if (brand.Id != Guid.Empty)
            {
                brand.Name = name;
                await BrandService.UpdateBrand(brand);
            }
            else
            {
                brand.Name = name;
                brand.Id = Guid.NewGuid();
                await BrandService.CreateBrand(brand);
            }
            DialogService.Close(brand);

        }
    }
}
