using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class ShipmentUnassignedField
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ShipmentId { get; set; }
        public string FieldName { get; set; }
        public string ReceivedCode { get; set; }
        public string ReceivedData { get; set; }
        public string ReplacedDataId { get; set; }
        public string ObjectTableId { get; set; }
        public string ComputingPartnrCode { get; set; }
    }
}
