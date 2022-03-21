using System;
using System.Collections.Generic;
using Logitude.ShipmentTests.Models;
using Logitude.ShipmentTests.Models.Builders;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;

namespace Logitude.ShipmentTests.Services.OceanInsight
{
    public class ShipmentDataPreparation
    {
        public void Prepar()
        {
            try
            {
                var Shipment = CreateShipmentWithContainer();
                MapData(Shipment);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Failed Creating Shipment Before Feature Run :" + e.InnerException);
            }
        }
       

        public ShipmentPM GetValidShipmentPM()
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
        public PackagePM GetValidPackagePM()
        {
            return new PackageBuilder().WithDefualtValues()
                .ContainerNumber("AMER1234568")
                .FlashPointTemperatureUnitCode("CEL")
                .PackageTypeByCode("PC1")
                .Quantity(1)
                .Weight(99)
                .TemperatureUnitCode("CEL")
                .Build();
        }
        public ShipmentPM CreateShipmentWithContainer()
        {
            ShipmentPM shipment = CreateShipment();
            shipment = AddContainer(shipment);
            return shipment;
        }

        public ShipmentPM AddContainer(ShipmentPM shipment)
        {
            shipment.ShipmentPackages = new List<PackagePM>() { GetValidPackagePM() };
            ApiResponse<ShipmentPM> putResponse = APICaller.CallPut<ShipmentPM>(shipment, Urls.ShipmentController, UserTenant.Token);
            return putResponse.Data;
        }

        public ShipmentPM CreateShipment()
        {
            ApiResponse<ShipmentPM> response = APICaller.CallPost<ShipmentPM>(GetValidShipmentPM(), Urls.ShipmentController, UserTenant.Token);
            string singleShipmentUrl = Urls.ShipmentGetSingle(response.Data?.Id);
            ApiResponse<ShipmentPM> getResponse = APICaller.CallGet<ShipmentPM>(singleShipmentUrl, UserTenant.Token);
            return getResponse.Data;
        }

        public void MapData(ShipmentPM shipment)
        {
            ShipmentData.ShipmentPM = shipment;
        }
    }
}
