using System;
using System.Collections.Generic;
using Logitude.DocumentTests.Models;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;

namespace Logitude.DocumentTests.Services.Preparation
{
    public class ShipmentDataPreparation
    {

        public void Prepare()
        {
            DocumentData.ShipmentId = CreateShipment().Id;
        }
        public ShipmentPM CreateShipment()
        {
            var shipmentInstace = GetValidShipmentPM();
            ApiResponse<ShipmentPM> response = APICaller.CallPost<ShipmentPM>(shipmentInstace, Urls.ShipmentController, UserTenant.Token);
            string singleShipmentUrl = Urls.ShipmentGetSingle(response.Data?.Id);
            ApiResponse<ShipmentPM> getResponse = APICaller.CallGet<ShipmentPM>(singleShipmentUrl, UserTenant.Token);
            return getResponse.Data;
        }
        private ShipmentPM GetValidShipmentPM()
        {
            return new ShipmentBuilder().WithDefualtValues()
                .DirectionId("E")
                .TransportModeId("O")
                .ShipmentLevelCode("D")
                .MainCarriageFromPortIdByCode("LHR")
                .MainCarriageToPortIdByCode("MIA")
                .MainCarriageCarrierIdByCode("MSCU")
                .ShipmentTypeId("FCLD")
                .Master("MasterTest")
                .LongMaster("MasterTest")
                .OtherPrepaidCollectId("P")
                .FreightPrepaidCollectId("P")
                .GrossWeight(100)
                .Build();
        }
       


    }
}
