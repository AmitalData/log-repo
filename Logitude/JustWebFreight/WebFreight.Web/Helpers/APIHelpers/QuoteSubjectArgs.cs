using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.APIHelpers
{
    public class QuoteSubjectArgs
    {
        [Key]
        public string EntityId { get; set; }
        public string DirectionId { get; set; }
        public string TransportModeId { get; set; }
        public string IncotermId { get; set; }
        public string FromPartnerAddressId { get; set; }
        public string ToPartnerAddressId { get; set; }
        public bool IncludePickUp { get; set; }
        public string PickUpAddressId { get; set; }
        public string FromAddressCity { get; set; }
        public string FromAddressZipCode { get; set; }
        public string FromPortId { get; set; }
        public bool IncludeDelivery { get; set; }
        public string DeliveryAddressId { get; set; }
        public string ToAddressCity { get; set; }
        public string ToAddressZipCode { get; set; }
        public string ToPortId { get; set; }
        public string Subject { get; set; }
    }
}