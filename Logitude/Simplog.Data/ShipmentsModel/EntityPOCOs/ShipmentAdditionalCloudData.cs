using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class ShipmentAdditionalCloudData
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string DeclarationXmlData { get; set; }
        public bool IsImporterApprovalRequried { get; set; }
        public string ApprovedByUserName { get; set; }
        public DateTime? ApproveDateTime { get; set; }
        public string VersionApproved { get; set; }
        public string DenyReason { get; set; }
        public string ShipmentAddtionalDataXML { get; set; }
        public Shipment Shipment { get; set; }
        public bool SendUpdatesToAgentEnabled { get; set; }
        public bool DocsSentToAgent { get; set; }

        public bool IsPaymentRequired { get; set; }
        public string PaymentRequestXML { get; set; }
        public DateTime? PaymentDateTime { get; set; }
        public string DeclarationWCOXml { get; set; }


    }
}
