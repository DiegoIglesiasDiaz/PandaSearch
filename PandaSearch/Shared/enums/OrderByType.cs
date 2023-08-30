using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PandaSearch.Shared.enums
{
    public enum OrderByType
    {
        [Description("Name A-Z")]
        NameAsc,
        [Description("Name Z-A")]
        NameDesc,
        [Description("Price ⬆️")]
        PriceAsc,
        [Description("Price ⬇️")]
        PriceDesc,
    }

}
