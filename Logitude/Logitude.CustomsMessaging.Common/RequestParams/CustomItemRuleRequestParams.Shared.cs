using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class CustomItemRuleRequestParams : RequestParamsBase
    {
        public int customsBookType { get; set; }
        public int customsItemId { get; set; }
        public DateTime validToDate { get; set; }
    }
}
