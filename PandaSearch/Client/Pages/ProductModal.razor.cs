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
    public partial class ProductModal
    {
        [Inject] NotificationService NotificationService { get; set; }
        [Inject] DialogService DialogService { get; set; }
        [Inject] BrandService BrandService { get; set; }
        [Inject] ProductService ProductService { get; set; }
        [Parameter] public Product Product { get; set; }
        public Product ProductFields = new Product();
        private List<Brand> LsBrands;
        private string SelectedClotheType;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            LsBrands = await BrandService.GetAll();
            if (Product.Id != Guid.Empty)
            {
                SelectedClotheType = EnumHelper.GetDescription(Product.ClotheType);
                fillFields();
            }


        }
        public void Apply()
        {

            if (!validation()) return;

            Product.ClotheType = (ClotheType)Enum.Parse(typeof(ClotheType), SelectedClotheType);
            Product.Brand = null;
            if (Product.Id != Guid.Empty)
            {
                ProductService.UpdateProduct(Product);
            }
            else
            {
                Product.Id = Guid.NewGuid();
                ProductService.CreateProduct(Product);

            }
            Product.Brand = LsBrands.FirstOrDefault(x=>x.Id == Product.BrandId);
            DialogService.Close(Product);
        }
        public void fillFields()
        {
            ProductFields.Name = Product.Name;
            ProductFields.Price = Product.Price;
            ProductFields.Link = Product.Link;
            ProductFields.BrandId = Product.BrandId;
        }
        public bool validation()
        {
            if (string.IsNullOrWhiteSpace(ProductFields.Name) || string.IsNullOrWhiteSpace(ProductFields.Link) || Double.IsNaN(ProductFields.Price) ||
                 string.IsNullOrWhiteSpace(ProductFields.Name) || ProductFields.BrandId == Guid.Empty)
            {
                NotificationService.Notify(NotificationSeverity.Error,"You must fill the fields.");
                return false;
            }
            else
            {
                Product.Name = ProductFields.Name;
                Product.Price = ProductFields.Price;
                Product.Link = ProductFields.Link;
                Product.BrandId = ProductFields.BrandId;
                return true;
            }
          
           
        }

    }
}
