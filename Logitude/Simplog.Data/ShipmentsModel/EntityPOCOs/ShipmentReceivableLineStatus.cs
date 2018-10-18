using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class ShipmentReceivableLineStatus
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }

        ////[Include]
        ////[Association("ShipmentReceivableStatusType", "Code", "StatusTypeCode")]
        //public  List<ShipmentReceivable> ShipmentReceivables { get; set; }

    }
}