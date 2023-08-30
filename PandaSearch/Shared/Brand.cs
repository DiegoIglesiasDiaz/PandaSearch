using System.ComponentModel.DataAnnotations.Schema;

namespace PandaSearch.Shared
{
    [Table("Brands")]
    public class Brand : BaseModel
    {
        public string Name { get; set; }
        public List<Product>? Product { get; set; }
    }
}