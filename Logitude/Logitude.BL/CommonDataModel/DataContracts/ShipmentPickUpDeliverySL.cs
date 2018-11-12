using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.DataContracts
{
    public class ShipmentPickUpDeliverySL
    {
        public PortSL FromPort { get; set; }
        public PortSL ToPort { get; set; }
        public string PickUpDeliveryTypeCode { get; set; }
        public string PickUpDeliveryFromTypeCode { get; set; }
        public string PickUpDeliveryToTypeCode { get; set; }

        public string FromPortId { get; set; }
        public string ToPortId { get; set; }
        public PartnerSL FromPartner { get; set; }
        public PartnerSL ToPartner { get; set; }

        public string FromAddressCity_Dummy { get; set; }
        public string FromAddressCity { get; set; }
        public string FromAddressZipCode { get; set; }
        public string FromAddressCountryId { get; set; }
        public string FromAddressCountryCode { get; set; }
        public string FromAddressCountryName { get; set; }

        public string ToAddressCity_Dummy { get; set; }
        public string ToAddressCity { get; set; }
        public string ToAddressZipCode { get; set; }
        public string ToAddressCountryId { get; set; }
        public string ToAddressCountryCode { get; set; }
        public string ToAddressCountryName { get; set; }

        public DateTime? MainCarriageETD { get; set; }
        public DateTime? MainCarriageATD { get; set; }
        public DateTime? MainCarriageETA { get; set; }
        public DateTime? MainCarriageATA { get; set; }

        public string TransportModeCode { get; set; }
        public string TransportModeName { get; set; }
    }
}
