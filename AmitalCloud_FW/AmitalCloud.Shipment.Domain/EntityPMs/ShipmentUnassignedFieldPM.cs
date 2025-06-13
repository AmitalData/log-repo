using Logitude.Server.Tools;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace  AmitalCloud.Shipment.Def.EntityPMs
{
    public partial class ShipmentUnassignedFieldPM : EntityPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ShipmentId { get; set; }
        public string FieldName { get; set; }
        public string ReceivedCode { get; set; }
        public string ReceivedData { get; set; }
        public string ReplacedDataId  { get; set; }
        public string ObjectTableId { get; set; }
        public string ComputingPartnrCode { get; set; }
        //public ChangeSetOperation ChangeSetOp { get; set; }
    }
}
