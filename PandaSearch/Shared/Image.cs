using PandaSearch.Shared.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PandaSearch.Shared
{
    [Table("Images")]
    public class Image : BaseModel
    {
      
        public string imageUrl { get; set; }
        public bool isDefaultPhoto { get; set; }


        [NotMapped]
        public Product Product { get; set; }
        [ForeignKey("Product")]
        public Guid ProductId { get; set; }

    }
}
