using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class CustomItemMekachRequestParams : RequestParamsBase
    {
        public int customsItemId { get; set; }
        public DateTime validToDate { get; set; }
        public int languageType { get; set; }
    }
}
