using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.BL.CommonDataModel.APIDataContract;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.APIDataContract;
namespace Logitude.CargoTracking.BL.APIDataContract
{
    public class CargoTrackingShipmentDetails
    {
        public string CourierBLNumber { get; set; }
        public string ShipmentNumber { get; set; }
        public string CarrierPrefixAWB { get; set; }
        public DateTime? EstimatedArrivalDateTime { get; set; }
        public DateTime? ActualArrivalDateTime { get; set; }
        public DateTime? PaymentDateTime { get; set; }
        public DateTime? CustomsClearanceDateTime { get; set; }
        public bool IsPaymentRequired { get; set; }
        public string ChargesAmountInNIS { get; set; }
        public bool IsCustomerIDNumberRequired { get; set; }
        public LastMileDetails LastMileDetails { get; set; }
        public List<MilestoneData> ShipmentMilestones { get; set;}
        public List<StatusDetails> StatusDetails { get; set; }

        public ShipmentExceptions ShipmentExceptions { get; set; }

    }
}
