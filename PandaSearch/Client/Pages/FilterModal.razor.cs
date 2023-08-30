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
    public partial class FilterModal
    {
        [Inject] DialogService DialogService { get; set; }
        [Inject] BrandService BrandService { get; set; }
        [Parameter] public Filter Filter { get; set; }
        public string clothes;
        public RadzenDropDown<string> dropDown;
        private List<string> selectedClotheValues = new List<string>();
        private IEnumerable<int> selectedPricesValues;
        private List<Guid> selectedBrandId = new List<Guid>();
        private List<Brand> LsBrands;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (Filter.maxPrice == -1)
                selectedPricesValues = new int[] { 0, (int)Filter.maxPriceSlider };
            else
                selectedPricesValues = new int[] { (int)Filter.minPrice, (int)Filter.maxPrice };
            LsBrands = await BrandService.GetAll();
            foreach (var item in Filter.ClotheTypes)
            {
                selectedClotheValues.Add(item.ToString());
            }


            selectedBrandId = Filter.LsBrandsId;


        }
        public void Apply()
        {
            Filter.minPrice = selectedPricesValues.First();
            Filter.maxPrice = selectedPricesValues.Last();
            Filter.ClotheTypes = selectedClothes();
            Filter.LsBrandsId = selectedBrandId.ToList();
            DialogService.Close(Filter);

        }
        public List<ClotheType> selectedClothes()
        {
            var clotheType = new List<ClotheType>();
            if (selectedClotheValues != null)
            {
                foreach (var item in selectedClotheValues)
                {
                    clotheType.Add((ClotheType)Enum.Parse(typeof(ClotheType), item));
                }
            }

            return clotheType;
        }
    }
}
