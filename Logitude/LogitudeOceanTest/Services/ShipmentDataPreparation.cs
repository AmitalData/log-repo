using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.OceanTest.Models;
using Logitude.OceanTest.Models.Builders;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;

namespace Logitude.OceanTest.Services
{
    public class ShipmentDataPreparation
    {
        public void Prepar()
        {
            try
            {
                var Shipment = CreateShipmentWithContainer();
                DataMap(Shipment);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Failed Creating Shipment Before Feature Run :" + e.InnerException);
            }
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
                //.MainCarriageCarrierCode("MAEU")
                .OtherPrepaidCollectId("P")
                .FreightPrepaidCollectId("C")
                .GrossWeight(100)
                .Build();
        }
        private PackagePM GetValidPackagePM()
        {
            return new PackageBuilder().WithDefualtValues()
                .ChangeSetOp("Insert")
                .ContainerNumber("AMER1234568")
                .FlashPointTemperatureUnitCode("CEL")
                .PackageTypeByCode("PC1")
                .Quantity(1)
                .Weight(99)
                .TemperatureUnitCode("CEL")
                .Build();
        }
        private ShipmentPM CreateShipmentWithContainer()
        {
            ShipmentPM shipment = CreateShipment();
            shipment = AddContainer(shipment);
            return shipment;
        }

        private ShipmentPM AddContainer(ShipmentPM shipment)
        {
            shipment.ShipmentPackages = new List<PackagePM>() { GetValidPackagePM() };
            ApiResponse<ShipmentPM> putResponse = APICaller.CallPut<ShipmentPM>(shipment, Urls.ShipmentController, UserTenant.Token);
            return putResponse.Data;
        }

        private ShipmentPM CreateShipment()
        {
            ApiResponse<ShipmentPM> response = APICaller.CallPost<ShipmentPM>(GetValidShipmentPM(), Urls.ShipmentController, UserTenant.Token);
            string singleShipmentUrl = Urls.ShipmentGetSingle(response.Data?.Id);
            ApiResponse<ShipmentPM> getResponse = APICaller.CallGet<ShipmentPM>(singleShipmentUrl, UserTenant.Token);
            return getResponse.Data;
        }

        private void DataMap(ShipmentPM shipment)
        {
            OceanData.ShipmentPM = shipment;
        }
    }
}
