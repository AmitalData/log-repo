using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class BlockListInWarehouseRequestParams : RequestParamsBase
    {
        public enum ShowResetBlocksTypesEnum
        {
            No = 0,
            Yes = 1,
            All = 2,
        }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string StorageSiteNumber { get; set; }
        public ShowResetBlocksTypesEnum ShowResetBlocks { get; set; }
    }
}
