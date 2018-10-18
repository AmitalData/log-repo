using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.LT2UT
{
    public partial class ItemsTableService
    {
        public class UpdateItemsTable
        {
            public List<ItemData> ItemsDataList { get; set; }
        }

        public class ItemData
        {
            public string customerId { get; set; }
            public string supplierId { get; set; }
            public string itemNo { get; set; }
            public string itemName { get; set; }
            public string pratCode { get; set; }
            public string originCountryCode { get; set; }
            public string unitId { get; set; }
        }
    }
}

