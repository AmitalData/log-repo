    
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
            //await UpdateShipmentWithInvalidFutureATD();
            //await UpdateShipmentWithInvalidFutureATA();
            //await UpdateShipmentWithValidDates();
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
                    Code = "",
                    PartnerCode = "MSALOG",
                    IsDisconnectedFromGLAccount = false
                },
                Consignee = new BL.CommonDataModel.APIDataContract.ApiV1.Card() //
                {
                    Code = "",
                    PartnerCode = "MSALOG",
                    IsDisconnectedFromGLAccount = false
                },

                ConsigneeReference1 = "SR1",
                ConsigneeReference2 = "SR2",
                Customer = new BL.CommonDataModel.APIDataContract.ApiV1.Card() //
                {
                    Code = "",
                    PartnerCode = "MSALOG",
                    IsDisconnectedFromGLAccount = false
                },
                GrossWeightUnit = new BL.CommonDataModel.APIDataContract.ApiV1.WeightUnit()//
                {
                    Code = "KG",
                },

                ChargeableWeightUnit = new BL.CommonDataModel.APIDataContract.ApiV1.WeightUnit()
                {
                    Code = "KG",
                },

                VolumeUnit = new BL.InfrastructureModel.APIDataContract.ApiV1.VolumeUnit()//
                {
                    Code = "CBI",
                },

                HouseNo = "CTIBGUM203248LAX", //

                HouseDate = DateTime.Parse("2020-11-11T10:10:48"), //

                Commodity = "653",

                Incoterm = new BL.CommonDataModel.APIDataContract.ApiV1.Incoterm()//
                {
                    Code = "CFR",
                },

                OceanOrInlandPackages = new List<OceanOrInlandPackage>(),
            };

            entityPM.OceanOrInlandPackages.Add(new OceanOrInlandPackage()
            {
                PackageType = new BL.CommonDataModel.APIDataContract.ApiV1.PackageType()
                {
                    Code = "IP",
                    PartnerCode=""
                },
                Length =22,
                Width = 25,
                Height=5,
                Pieces = 1,
                Volume = 1.59,
                GrossWeight = 36,
                Reference1 = "AUTO PARTS",
                Reference2 = "15061200",
                Reference3 = "10/28/2020",
                Reference4 = "4236564",
                IsDangerous =false,
                Notes = "ROUGH COUNTRY",
              

            });

       
            shipmentId = await service.CreateHouse(entityPM);
        }

        private async Task GetShipment()
        {
            shipmentPM = await service.GetHouse(shipmentId);
        }

    }
}
