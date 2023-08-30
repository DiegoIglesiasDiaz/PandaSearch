using PandaSearch.Shared.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PandaSearch.Shared
{
    public class Filter
    {
        public Filter()
        {
            ClotheTypes = new List<ClotheType>();
            LsBrandsId = new List<Guid>();
            minPrice = -1;
            maxPrice = -1;
        }
        public List<ClotheType> ClotheTypes { get; set; }
        public List<Guid> LsBrandsId { get; set; }
        public double minPrice { get;set; }
        public double maxPrice { get;set; }
        public double maxPriceSlider { get; set; }

    }
}
