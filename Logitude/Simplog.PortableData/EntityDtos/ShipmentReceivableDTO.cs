using System;
using System.Collections.Generic;
using System.Text;

namespace Simplog.PortableData.EntityDTOs
{
    public class ShipmentReceivableDTO : EntityDTO<ShipmentReceivableDTO>
    {
        public string Id { get; set; }
        public int Tenant { get; set; }

      
        public string ShipmentId { get; set; }

        
        public string ChargesTypeId { get; set; }
        public string ChargesTypeCode { get; set; }
        public string ChargesTypeName { get; set; }
        public string ChargesGroupCode { get; set; }      
    }
}
