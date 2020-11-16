
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
    public class HouseTest
    {
        string shipmentId;
        House shipmentPM;
        ExternalHouseAPITestService service;

        [TestMethod]
        public async Task TestDirectShipment()
        {
            service = new ExternalHouseAPITestService();
            //factory = new EntityInitializerFactory();

            await CreateShipment();
            await GetShipment();
            await UpdateShipmentWithInvalidFutureATD();
            await UpdateShipmentWithInvalidFutureATA();
            await UpdateShipmentWithValidDates();
        }
        private async Task CreateShipment()
        {
            var entityPM = new House()
            {
                Agent = new BL.CommonDataModel.APIDataContract.ApiV1.Card()
                {
                    Code = "10026"
                },

                Direction = new BL.InfrastructureModel.APIDataContract.ApiV1.Direction()
                {
                    Code = "E",
                },

                TransportMode = new BL.InfrastructureModel.APIDataContract.ApiV1.TransportMode()
                {
                    Code = "O",
                },

                ShipmentType = new ShipmentType()
                {
                    Code = "FCLD",
                },

                Shipper = new BL.CommonDataModel.APIDataContract.ApiV1.Card()
                {
                    Code = "10009",
                },

                ShipperReference1 = "SR1",
                ShipperReference2 = "SR2",

                GrossWeightUnit = new BL.CommonDataModel.APIDataContract.ApiV1.WeightUnit()
                {
                    Code = "KG",
                },

                ChargeableWeightUnit = new BL.CommonDataModel.APIDataContract.ApiV1.WeightUnit()
                {
                    Code = "KG",
                },

                VolumeUnit = new BL.InfrastructureModel.APIDataContract.ApiV1.VolumeUnit()
                {
                    Code = "CBM",
                },

                Commodity = "653",

                Incoterm = new BL.CommonDataModel.APIDataContract.ApiV1.Incoterm()
                {
                    Code = "CIF",
                },

                MainCarriageCarrier = new BL.CommonDataModel.APIDataContract.ApiV1.Card()
                {
                    Code = "DD",
                },

                MainCarriageCarrierNumber = "CC",

                MainCarriageATD = new DateTime(2018, 12, 31),

                OceanOrInlandPackages = new List<OceanOrInlandPackage>(),
                MainCarriageLegs = new List<MainCarriageLeg>(),
            };

            entityPM.OceanOrInlandPackages.Add(new OceanOrInlandPackage()
            {
                PackageType = new BL.CommonDataModel.APIDataContract.ApiV1.PackageType()
                {
                    Code = "20BU",
                },

                Pieces = 1,

                GrossWeight = 250,
            });

            entityPM.MainCarriageLegs.Add(new MainCarriageLeg()
            {
                LegIndex = 1,

                Carrier = new BL.CommonDataModel.APIDataContract.ApiV1.Card()
                {
                    Code = "DD",
                },

                FromPort = new BL.CommonDataModel.APIDataContract.ApiV1.Port()
                {
                    Code = "DE222",
                },

                ToPort = new BL.CommonDataModel.APIDataContract.ApiV1.Port()
                {
                    Code = "DE223",
                },

                MasterNumber = "456",
                CarrierNumber = "123",
            });

            shipmentId = await service.CreateHouse(entityPM);
        }

        private async Task GetShipment()
        {
            shipmentPM = await service.GetHouse(shipmentId);
        }

    }
}
