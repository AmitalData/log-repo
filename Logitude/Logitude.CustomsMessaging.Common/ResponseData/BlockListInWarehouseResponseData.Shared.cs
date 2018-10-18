using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class BlockListInWarehouseResponseData : ResponseDataBase
    {
        public List<BlockListInWarehouseResult> BlockListInWarehouseResultList { get; set; }
        public string NumberOfBlocksInList { get; set; }
    }

    public class BlockListInWarehouseResult
    {
        public string DeclarationNumber { get; set; }
        public string WarehouseBlockNumber { get; set; }
        public string ImporterNumber { get; set; }
        public string ImporterTitle { get; set; }
        public string OriginalOpeningDate { get; set; }
        public string OpeningDate { get; set; }
        public string LogicalPackagesQuantityBalance { get; set; }
        public string PhysicalPackagesQuantityBalance { get; set; }
        public string Value { get; set; }
        public string SpecialActivityTypeName { get; set; }
    }
}
