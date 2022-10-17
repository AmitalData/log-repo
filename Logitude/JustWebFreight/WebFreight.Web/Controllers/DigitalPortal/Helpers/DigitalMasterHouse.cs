using Logitude.BL.CommonDataModel.EntityPMs;
using System;
using System.Collections.Generic;

namespace WebFreight.Web.Controllers.DigitalPortal.Helpers
{
    public class DigitalMasterHouse
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ShipperName { get; set; }
        public string ConsigneeName { get; set; }
        public string ShipmentLevelCode { get; set; }
        public double? GrossWeight { get; set; }
        public int? Quantity { get; set; }
        public DateTime? MainCarriageATD { get; set; }
        public string MainCarriageCarrierName { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public List<SharedLogisticDocumentPM> Documents { get; set; }
    }
}