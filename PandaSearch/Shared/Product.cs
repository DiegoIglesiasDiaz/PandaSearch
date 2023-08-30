using PandaSearch.Shared.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PandaSearch.Shared
{
    [Table("Products")]
    public class Product : BaseModel
    {

        public Product()
        {
            Brand = new Brand();
        }
        public string Name { get; set; }

        public double Price { get; set; }
        public ClotheType ClotheType { get; set; }
        public string Link { get; set; }
        public string? Description { get; set; }

        [NotMapped]
        public Brand? Brand { get; set; }
        [ForeignKey("Brand")]
        public Guid BrandId { get; set; }

        [NotMapped]

        public List<Image>? Images { get; set; }


        [NotMapped]
        public string PriceToString { get { return Price.ToString("0.00") + "€"; } }
        [NotMapped]
        public string BrandAndName { get { return Brand != null ? Brand.Name + " " + Name : ""; } }


    }
}
