using Logitude.BL.ShipmentsModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class RoutingDataProvider
    {
        public string PreForwardingFrom { get; set; }
        public string PreForwardingTo { get; set; }
        public DateTime? PreForwardingETD { get; set; }
        public DateTime? PreForwardingETA { get; set; }
        public DateTime? PreForwardingATD { get; set; }
        public DateTime? PreForwardingATA { get; set; }
        public string PreForwardingCarrierCode { get; set; }
        public string PreForwardingCarrierNumber { get; set; }
        public string OnForwardingFrom { get; set; }
        public string OnForwardingTo { get; set; }
        public DateTime? OnForwardingETD { get; set; }
        public DateTime? OnForwardingETA { get; set; }
        public DateTime? OnForwardingATD { get; set; }
        public DateTime? OnForwardingATA { get; set; }
        public string OnForwardingCarrierCode { get; set; }
        public string OnForwardingCarrierNumber { get; set; }
        public string PreCarriageFrom { get; set; }
        public string PreCarriageTo { get; set; }
        public DateTime? PreCarriageETD { get; set; }
        public DateTime? PreCarriageETA { get; set; }
        public DateTime? PreCarriageATD { get; set; }
        public DateTime? PreCarriageATA { get; set; }
        public string PreCarriageCarrierCode { get; set; }
        public string PreCarriageCarrierNumber { get; set; }
        public string OnCarriageFrom { get; set; }
        public string OnCarriageTo { get; set; }
        public DateTime? OnCarriageETD { get; set; }
        public DateTime? OnCarriageETA { get; set; }
        public DateTime? OnCarriageATD { get; set; }
        public DateTime? OnCarriageATA { get; set; }
        public string OnCarriageCarrierCode { get; set; }
        public string OnCarriageCarrierNumber { get; set; }
        public string OnForwardingFromName { get; set; }
        public string OnForwardingToName { get; set; }
        public string PreForwardingFromName { get; set; }
        public string PreForwardingToName { get; set; }
        public string OnCarriageFromName { get; set; }
        public string OnCarriageToName { get; set; }
        public string PreCarriageFromName { get; set; }
        public string PreCarriageToName { get; set; }

        private ShipmentPM shipmentPM;
        public RoutingDataProvider(ShipmentPM shipment)
        {
            shipmentPM = shipment;
            this.GetPreForwardingData();
            this.GetOnForwardingData();
            this.GetPreCarriageData();
            this.GetOnCarriageData();
        }
        private void GetPreForwardingData()
        {
            this.PreForwardingFrom = shipmentPM.PreForwardingFromPortCode;
            this.PreForwardingFromName = shipmentPM.PreForwardingFromPortName;
            this.PreForwardingTo = shipmentPM.PreForwardingToPortCode;
            this.PreForwardingToName = shipmentPM.PreForwardingToPortName;
            this.PreForwardingETD = shipmentPM.PreForwardingETD;
            this.PreForwardingETA = shipmentPM.PreForwardingETA;
            this.PreForwardingATD = shipmentPM.PreForwardingATD;
            this.PreForwardingATA = shipmentPM.PreForwardingATA;
            this.PreForwardingCarrierCode = shipmentPM.PreForwardingCarrierCode;
            this.PreForwardingCarrierNumber = shipmentPM.PreForwardingCarrierNumber;
        }
        private void GetOnForwardingData()
        {
            this.OnForwardingFrom = shipmentPM.OnForwardingFromPortCode;
            this.OnForwardingFromName = shipmentPM.OnForwardingFromPortName;
            this.OnForwardingTo = shipmentPM.OnForwardingToPortCode;
            this.OnForwardingToName = shipmentPM.OnForwardingToPortName;
            this.OnForwardingETD = shipmentPM.OnForwardingETD;
            this.OnForwardingETA = shipmentPM.OnForwardingETA;
            this.OnForwardingATD = shipmentPM.OnForwardingATD;
            this.OnForwardingATA = shipmentPM.OnForwardingATA;
            this.OnForwardingCarrierCode = shipmentPM.OnForwardingCarrierCode;
            this.OnForwardingCarrierNumber = shipmentPM.OnForwardingCarrierNumber;
        }
        private void GetPreCarriageData()
        {
            this.PreCarriageFrom = shipmentPM.PreCarriageFromPortCode;
            this.PreCarriageFromName = shipmentPM.PreCarriageFromPortName;
            this.PreCarriageTo = shipmentPM.PreCarriageToPortCode;
            this.PreCarriageToName = shipmentPM.PreCarriageToPortName;
            this.PreCarriageETD = shipmentPM.PreCarriageETD;
            this.PreCarriageETA = shipmentPM.PreCarriageETA;
            this.PreCarriageATD = shipmentPM.PreCarriageATD;
            this.PreCarriageATA = shipmentPM.PreCarriageATA;
            this.PreCarriageCarrierCode = shipmentPM.PreCarriageCarrierCode;
            this.PreCarriageCarrierNumber = shipmentPM.PreCarriageCarrierNumber;
        }
        private void GetOnCarriageData()
        {
            this.OnCarriageFrom = shipmentPM.OnCarriageFromPortCode;
            this.OnCarriageFromName = shipmentPM.OnCarriageFromPortName;
            this.OnCarriageTo = shipmentPM.OnCarriageToPortCode;
            this.OnCarriageToName = shipmentPM.OnCarriageToPortName;
            this.OnCarriageETD = shipmentPM.OnCarriageETD;
            this.OnCarriageETA = shipmentPM.OnCarriageETA;
            this.OnCarriageATD = shipmentPM.OnCarriageATD;
            this.OnCarriageATA = shipmentPM.OnCarriageATA;
            this.OnCarriageCarrierCode = shipmentPM.OnCarriageCarrierCode;
            this.OnCarriageCarrierNumber = shipmentPM.OnCarriageCarrierNumber;
        }
    }
}