using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PandaSearch.Client.Services;
using PandaSearch.Shared;
using PandaSearch.Shared.enums;
using Radzen;

namespace PandaSearch.Client.Pages
{
    public partial class Index
    {
        public string orderValue;
        public int selectedOrderValue = 2;
        List<Product> LsProducts = new List<Product>();
        Filter filter = new Filter();
        [Inject] DialogService DialogService { get; set; }
        [Inject] ProductService ProductService { get; set; }
        [Inject] NotificationService NotificationService { get; set; }
        public string BuscarText { get; set; }
        public int numProducts = 8;
        public int Page = 1;
        public int MaxPage;
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await GridProducts();
            Console.WriteLine(LsProducts.First().Name + " --- " + LsProducts.First().imgbyte != null ? "No es nulo" : "es nulo");
            orderValue = "NameAsc";;
        }
        public async Task ClearFilter()
        {
            BuscarText = "";
            Page = 1;
            filter.ClotheTypes = new List<ClotheType>();
            filter.LsBrandsId = new List<Guid>();
            filter.minPrice = -1;
            filter.maxPrice = -1;
            await GridProducts();

        }
        public async Task OpenFilterModal()
        {
            filter.maxPriceSlider = (await getAllProducts()).OrderByDescending(p => p.Price).FirstOrDefault().Price;
            var aux  = await DialogService.OpenAsync<FilterModal>("",new Dictionary<string, object> { { "Filter",filter} },new DialogOptions { ShowClose=true});
            if (aux != null && aux is Filter)
            {
                filter = aux;
                NotificationService.Notify(NotificationSeverity.Info, "Ok", "filter applied successfully.");
            }
                
           await GridProducts();
        }
        public async Task Paginator(bool isNext)
        {
            if (isNext)
                Page++;
            else
                Page--;
            if (Page <= 0)
                Page = 1;
           await  GridProducts();

        }
        public async Task GridProducts()
        {
            LsProducts =  await getAllProducts();
            LsProducts = LsProducts.OrderBy(x => x.Name).ToList();
            //Filtrar
            if (filter.ClotheTypes.Count > 0)
                LsProducts = LsProducts.Where(x => filter.ClotheTypes.Contains(x.ClotheType)).ToList();
            LsProducts = LsProducts.Where(x=> x.Price >= filter.minPrice).ToList();
            if (filter.LsBrandsId.Count > 0)
                LsProducts = LsProducts.Where(x => filter.LsBrandsId.Contains(x.BrandId)).ToList();
            if (filter.maxPrice != -1)
                LsProducts = LsProducts.Where(x => x.Price <= filter.maxPrice).ToList();
            //Ordenar
            if (orderValue != null)
            {
                var orderType = Enum.Parse(typeof(OrderByType), orderValue);
                switch (orderType)
                {
                    case OrderByType.NameAsc:
                        LsProducts = LsProducts.OrderBy(x => x.Name).ToList();
                        break;
                    case OrderByType.NameDesc:
                        LsProducts = LsProducts.OrderByDescending(x => x.Name).ToList();
                        break;
                    case OrderByType.PriceAsc:
                        LsProducts = LsProducts.OrderBy(x => x.Price).ToList();
                        break;
                    case OrderByType.PriceDesc:
                        LsProducts = LsProducts.OrderByDescending(x => x.Price).ToList();
                        break;
                    default: break;
                }
            }

            MaxPage = (LsProducts.Count / numProducts) + 1;
            int index = (Page - 1) * numProducts;
            if (index + numProducts > LsProducts.Count)
                LsProducts = LsProducts.GetRange(index, LsProducts.Count - index);
            else
                LsProducts = LsProducts.GetRange(index, numProducts);

        }
        public async Task<List<Product>> getAllProducts()
        {

            var pro = await ProductService.GetAll();
            if (!string.IsNullOrEmpty(BuscarText))
                pro = pro.Where(x => x.BrandAndName.ToUpper().Contains(BuscarText.ToUpper())).ToList();
            return pro;
        }
    }

}
