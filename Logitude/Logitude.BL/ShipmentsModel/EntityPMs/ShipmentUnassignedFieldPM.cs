using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    public class ShipmentUnassignedFieldPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ShipmentId { get; set; }
        public string FieldName { get; set; }
        public string ReceivedCode { get; set; }
        public string ReceivedData { get; set; }
        public string ReplacedDataId  { get; set; }
        public ChangeSetOperation ChangeSetOp { get; set; }
    }
}
