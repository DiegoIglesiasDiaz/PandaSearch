using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PandaSearch.Shared
{
    public class BaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? ObsoleteBy { get; set; }
        public DateTime? ObsoleteDate { get; set; }


    }
}
