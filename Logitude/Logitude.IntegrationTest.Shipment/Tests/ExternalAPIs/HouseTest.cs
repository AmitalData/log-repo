    
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
        public async Task TestHouseShipment()
        {
            service = new ExternalHouseAPITestService();

            await CreateShipment();
            await GetShipment();

        }
        private async Task CreateShipment()
        {
            var entityPM = new House()
            {
                Direction = new BL.InfrastructureModel.APIDataContract.ApiV1.Direction() //
                {
                    Code = "E",
                },

                TransportMode = new BL.InfrastructureModel.APIDataContract.ApiV1.TransportMode() // 
                {
                    Code = "O",
                },

                ShipmentType = new ShipmentType() //
                {
                    Code = "LCL",
                },

                Shipper = new BL.CommonDataModel.APIDataContract.ApiV1.Card() //
                {
                    Code = "10009",
                },

                Customer = new BL.CommonDataModel.APIDataContract.ApiV1.Card() //
                {
                    Code = "10009",
                },

                GrossWeightUnit = new BL.CommonDataModel.APIDataContract.ApiV1.WeightUnit()//
                {
                    Code = "KG",
                },

                DimensionsUnit = new BL.CommonDataModel.APIDataContract.ApiV1.DimensionsUnit() //
                {
                    Code = "inc",
                },

                VolumeUnit = new BL.InfrastructureModel.APIDataContract.ApiV1.VolumeUnit()//
                {
                    Code = "CBI",
                },

                HouseNo = "CTIBGUM203248LAX", //

                HouseDate = new DateTime(2020, 11, 11), //

                FromPort = new BL.CommonDataModel.APIDataContract.ApiV1.Port()
                {
                    Code = "DE222",
                },

                ToPort = new BL.CommonDataModel.APIDataContract.ApiV1.Port()
                {
                    Code = "DE223",
                },

                Incoterm = new BL.CommonDataModel.APIDataContract.ApiV1.Incoterm()//
                {
                    Code = "CIF",
                },

                OceanOrInlandPackages = new List<OceanOrInlandPackage>(),
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

            shipmentId = await service.CreateHouse(entityPM);
        }

        private async Task GetShipment()
        {
            shipmentPM = await service.GetHouse(shipmentId);
        }

    }
}
