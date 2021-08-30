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
                var createdShipment = GetValidShipmentPM();
                var addShipment = CreateAndGetShipment(createdShipment);
                DataMap(addShipment);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Failed Creating Tariff Before Feature Run :" + e.InnerException);
            }
        }
       

        private ShipmentPM GetValidShipmentPM()
        {
            return new ShipmentBuilder().WithDefualtValues()
                .DirectionId("E")
                .TransportModeId("A")
                .ShipmentLevelCode("D")
                .MainCarriageFromPortIdByCode("LHR")
                .MainCarriageToPortIdByCode("MIA")
                .OtherPrepaidCollectId("P")
                .FreightPrepaidCollectId("C")
                .GrossWeight(100)
                .ShipmentPackages(new List<PackagePM>() { GetValidPackagePM()})
                .Build();
        }
        private PackagePM GetValidPackagePM()
        {
            return new PackageBuilder().WithDefualtValues()
                .ChangeSetOp("Insert")
                .ContainerNumber("AMER1234567")
                .FlashPointTemperatureUnitCode("CEL")
                .PackageTypeByCode("PC1")
                .Quantity(1)
                .TemperatureUnitCode("CEL")
                .Build();
        }
        private ShipmentPM CreateAndGetShipment(ShipmentPM shipmentPM)
        {
            ApiResponse<ShipmentPM> response = APICaller.CallPost<ShipmentPM>(GetValidShipmentPM(), Urls.ShipmentController, UserTenant.Token);
            string singleShipmentUrl = Urls.ShipmentGetSingle(response.Data?.Id);
            ApiResponse<ShipmentPM> GetResponse = APICaller.CallGet<ShipmentPM>(singleShipmentUrl, UserTenant.Token);
            return GetResponse.Data;
        }
        private void DataMap(ShipmentPM shipment)
        {
            OceanData.ShipmentPM = shipment;
        }
    }
}
