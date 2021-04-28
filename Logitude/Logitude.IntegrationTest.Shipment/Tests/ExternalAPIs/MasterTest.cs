using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.IntegrationTest.Shipment.EntitiesInitializer;
using Logitude.IntegrationTest.Shipment.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.Shipment.Tests.ExternalAPIs
{
    [TestClass]
    public class MasterTest
    {
        string shipmentId;
        Master shipmentPM;
        ExternalMasterAPITestService service;

        [TestMethod]
        public async Task TestMasterShipment()
        {
            service = new ExternalMasterAPITestService();
            await CreateShipment();
            await GetShipment();
            await UpdateShipmentWithInvalidFutureATD();
            await UpdateShipmentWithInvalidFutureATA();
            await UpdateShipmentWithValidDates();
        }

        private async Task CreateShipment()
        {
            var entityPM = new Master()
            {
                Agent = new BL.CommonDataModel.APIDataContract.ApiV1.Card()//
                {
                    Code = "79694"
                },

                Branch = new BL.CommonDataModel.APIDataContract.ApiV1.Branch()//
                {
                    Code = "HybridB1"
                },
                Department = new BL.CommonDataModel.APIDataContract.ApiV1.Department()//
                {
                    Code = "HybridD1"
                },
                Direction = new BL.InfrastructureModel.APIDataContract.ApiV1.Direction()//
                {
                    Code = "I",
                },

                TransportMode = new BL.InfrastructureModel.APIDataContract.ApiV1.TransportMode()//
                {
                    Code = "O",
                },
                ShipmentType = new ShipmentType() //
                {
                    Code = "LCL",
                },

                Shipper = new BL.CommonDataModel.APIDataContract.ApiV1.Card()//
                {
                    Code = "79695",
                },
                Consignee = new BL.CommonDataModel.APIDataContract.ApiV1.Card()//
                {
                    Code = "79696",
                },

                MainCarriageCarrier = new BL.CommonDataModel.APIDataContract.ApiV1.Card()//
                {
                    Code = "79697",
                },

                SpecialServicesType = new BL.ShipmentsModel.APIDataContract.ApiV1.SpecialServicesType()//
                {
                    Code = "HSST"
                },

                ShipperNotExporter = new BL.CommonDataModel.APIDataContract.ApiV1.Card()
                {
                    Code = "79698"
                },
                MasterDate = new DateTime(2020, 11, 16),
                Ratio =8.1,
                MainCarriageCarrierNumber = "155",//
                OrderNumberOfPackages=10,
                OrderGrossWeight =10,
                OrderVolume =10,
                OrderChargeableWeight=10,
                OrderIsDangerouseGoods=true,
                MasterNumber = "66998875", //
                MainCarriageATD = new DateTime(2020, 11, 16),//
                MainCarriageATA = new DateTime(2020, 11, 16),//
                Deliveries = new List<Delivery>(),
                PickUps = new List<PickUp>(),
                Houses = new List<House>(),
                MainCarriageLegs=new List<MainCarriageLeg>(),    
            };

       
            entityPM.Houses.Add(new House()
            {
                ShipmentNumber = "179ae92f-033e-4"
            });

            entityPM.MainCarriageLegs.Add(new MainCarriageLeg()
            {
                LegIndex = 1,

                Carrier = new BL.CommonDataModel.APIDataContract.ApiV1.Card()
                {
                    Code = "79701",
                },

                FromPort = new BL.CommonDataModel.APIDataContract.ApiV1.Port()
                {
                    Code = "BBQ",
                },

                ToPort = new BL.CommonDataModel.APIDataContract.ApiV1.Port()
                {
                    Code = "EZE",
                },
                ETD = new DateTime(2020, 11, 16),
                ETA = new DateTime(2020, 11, 16),
                ATD = new DateTime(2020, 11, 16),
                ATA = new DateTime(2020, 11, 16),
                MasterNumber = "66998875",
                CarrierNumber = "155",
            });

            shipmentId = await service.CreateMaster(entityPM);
        }

        private async Task GetShipment()
        {
            shipmentPM = await service.GetMaster(shipmentId);
        }

        private async Task UpdateShipmentWithInvalidFutureATD()
        {
            shipmentPM.MainCarriageLegs.First().ATD = new DateTime(2021, 10, 2);
            shipmentPM = await service.UpdateMaster(shipmentPM, false);

            Assert.IsTrue(service.HasException == true);
            Assert.IsTrue(service.ExceptionMessage == "Can't set MainCarriageATD to future date");
            shipmentPM.MainCarriageLegs.First().ATD = null;
        }

        private async Task UpdateShipmentWithInvalidFutureATA()
        {
            shipmentPM.MainCarriageLegs.First().ATA = new DateTime(2021, 10, 4);
            shipmentPM = await service.UpdateMaster(shipmentPM, false);

            Assert.IsTrue(service.HasException == true);
            Assert.IsTrue(service.ExceptionMessage == "Can't set MainCarriageATA to future date");
            shipmentPM.MainCarriageLegs.First().ATA = null;
        }

        private async Task UpdateShipmentWithValidDates()
        {
            shipmentPM.MainCarriageLegs.First().ETD = new DateTime(2020, 10, 5);
            shipmentPM.MainCarriageLegs.First().ATD = new DateTime(2020, 10, 6);
            shipmentPM.MainCarriageLegs.First().ETA = new DateTime(2020, 10, 7);
            shipmentPM.MainCarriageLegs.First().ATA = new DateTime(2020, 10, 8);

            shipmentPM = await service.UpdateMaster(shipmentPM);
            shipmentPM = await service.GetMaster(shipmentId);

            Assert.IsTrue(shipmentPM.MainCarriageLegs.First().ETD == new DateTime(2020, 10, 5));
            Assert.IsTrue(shipmentPM.MainCarriageLegs.First().ATD == new DateTime(2020, 10, 6));
            Assert.IsTrue(shipmentPM.MainCarriageLegs.First().ETA == new DateTime(2020, 10, 7));
            Assert.IsTrue(shipmentPM.MainCarriageLegs.First().ATA == new DateTime(2020, 10, 8));
        }
    }
}
