using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebFreight.Web.DataProviders
{
    public class PickUpDeliveryLine
    {
        public PickUpDeliveryLine()
        {
            this.PickUpDeliveryPackages = new List<PackageLine>();
        }

        [Key]
        public string Id { get; set; }
        public string Address { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public DateTime? ETD { get; set; }
        public DateTime? ETA { get; set; }
        public DateTime? ATD { get; set; }
        public DateTime? ATA { get; set; }
        public double? Weight { get; set; }
        public string Notes { get; set; }
        public string FullAddress { get; set; }
        public string TransportMode { get; set; }
        public string EmptyContainerReturn { get; set; }
        public string EmptyContainerReturnRef { get; set; }
        public string EmptyContainerReturnName { get; set; }
        public string EmptyContainerReturnAddress { get; set; }
        public List<PackageLine> PickUpDeliveryPackages { get; set; }
        public string TruckerContactName { get; set; }
        public string CarrierName { get; set; }
    }
}