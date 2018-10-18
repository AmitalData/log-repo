using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public  class ST_8328_Web01_WarehouseBlockBalanceRequestParams : RequestParamsBase
    {
        public string CustomFileNo { get; set; }
        public string DeclarationNumber { get; set; }
        public string StorageSiteNumber { get; set; }
        public string WarehouseBlockNumber { get; set; }
        public string DisplayGoodsItemByInvoice { get; set; }

        public bool DeclarationRadio { get; set; }
        public bool StorageSiteRadio { get; set; }
    }
}
