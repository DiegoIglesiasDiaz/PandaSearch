using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using PandaSearch.Client.Services;
using PandaSearch.Shared;
using PandaSearch.Shared.enums;
using Radzen;
using Radzen.Blazor;
using System.ComponentModel;
using System.Diagnostics.Tracing;
using System.IO;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;
using FileInfo = System.IO.FileInfo;

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
        private static Guid imageId;
        private RadzenUpload radzenUpload;
        private bool ImageMssg = false;
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            LsBrands = await BrandService.GetAll();
            if (Product.Id != Guid.Empty)
            {
                SelectedClotheType = EnumHelper.GetDescription(Product.ClotheType);
                imageId = Product.Id;
                fillFields();
            }
        }
        public async Task Apply()
        {
            try
            {
                if (!validation()) return;

                Product.ClotheType = (ClotheType)Enum.Parse(typeof(ClotheType), SelectedClotheType);
                Product.Brand = null;
                if (Product.Id != Guid.Empty)
                {
                    await ProductService.UpdateProduct(Product);
                }
                else
                {
                    Product.Id = Guid.NewGuid();
                    await ProductService.CreateProduct(Product);

                }
            }
            catch (Exception ex)
            {

            }
            Product.Brand = LsBrands.FirstOrDefault(x => x.Id == Product.BrandId);
            if (ImageMssg)
                Product.imgbyte = await ProductService.GetImgById(Product.Id);
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
                NotificationService.Notify(NotificationSeverity.Error, "You must fill the fields.");
                return false;
            }
            else
            {
                //if ((!ImageMssg && Product.Id == Guid.Empty))
                //{
                //    NotificationService.Notify(NotificationSeverity.Error, "You must select an Image.");
                //    return false;
                //}
                Product.Name = ProductFields.Name;
                Product.Price = ProductFields.Price;
                Product.Link = ProductFields.Link;
                Product.BrandId = ProductFields.BrandId;
                return true;
            }


        }
        public void UploadMessage(UploadProgressArgs e)
        {
            if (e.Progress == 100)
            {
                NotificationService.Notify(NotificationSeverity.Info, "Image Uploaded Succesfully");
                ImageMssg = true;
            }
            else
            {
                ImageMssg = false;
            }
        }
    }
}
