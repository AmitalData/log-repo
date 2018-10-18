using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class DueType
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }

        //public List<ChargesType> ChargesTypes { get; set; }
        //public List<ShipmentReceivable> ShipmentReceivables { get; set; }
        //public List<ShipmentPayable> ShipmentPayables { get; set; }
        //public List<ShipmentAWBPrintOnly> ShipmentAWBPrintOnlies { get; set; }
    }
}