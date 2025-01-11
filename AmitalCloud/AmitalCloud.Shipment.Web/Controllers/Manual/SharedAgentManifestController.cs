using Logitude.BL.CommonDataModel.DataContracts;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.ShipmentsModel.Extended
{
    public class SharedAgentManifestController : ApiController
    {

        public HttpResponseMessage GetSharedAgentManifest(string shipmentId, bool isUpdateAgent ,int tenant)
        {
            try
            {

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
                //SecurityUtility.CheckContactFeature("Shipment", "AgentSharedManifest", tenant);



                ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                ShipmentPM pm = shipmentQuery.GetSingleShipmentPM(shipmentId, tenant);

                ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
                CountryRepository countryRepository = new CountryRepository(commonContext);
                CardRepository cardRepository = new CardRepository(commonContext);
                AgentRepository agentRepository = new AgentRepository(commonContext);
                ContactRepository contactRepository = new ContactRepository(commonContext);
                TenantRepository tenantRepository = new TenantRepository(commonContext);

                string email = HttpContext.Current.User.Identity.Name;
                Contact loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);

                if (!string.IsNullOrEmpty(pm.AgentId))
                {
                    AgentSharedLogisticsKeyRepository agentSharedLogisticsKeyRepository = new AgentSharedLogisticsKeyRepository();
                    Agent currentAgent = agentRepository.GetSingleAgent(tenant, pm.AgentId);
                    AgentSharedLogisticsKey agentSharedLogisticsKey = agentSharedLogisticsKeyRepository.GetSingleAgentSharedLogisticsKey(currentAgent.AgentSharedLogisticsKey);
                    int sourceAgentTenant = tenant;
                    int destinationAgentTenant = agentSharedLogisticsKey.Agent1Tenant == tenant ? agentSharedLogisticsKey.Agent2Tenant : agentSharedLogisticsKey.Agent1Tenant;

                    Tenant sourceAgentTenantPOCO = tenantRepository.GetSingleTenant(sourceAgentTenant);
                    Tenant destinationAgentTenantPOCO = tenantRepository.GetSingleTenant(destinationAgentTenant);

                    Country countryNotify1 = null;
                    Card notify1Card = null;

                    PortSL transshipment1ToPort = null;
                    PortSL transshipment1FromPort = null;
                    PortSL transshipment2ToPort = null;
                    PortSL transshipment2FromPort = null;
                    PortSL transshipment3ToPort = null;
                    PortSL transshipment3FromPort = null;

                    //Card
                    if (!string.IsNullOrEmpty(pm.Notify1Id)) notify1Card = cardRepository.GetSingleCard(pm.Notify1Id, tenant);

                    //Country
                    if (!string.IsNullOrEmpty(pm.Notify1CountryId)) countryNotify1 = countryRepository.GetSingleCountry(pm.Notify1CountryId, tenant);
                    PartnerSL shipperSL = null;
                    PartnerSL consigneeSL = null;
                    if (pm.ShipmentLevelCode != "C")
                    { 
                        Card shipperCard = null;
                        Card consigneeCard = null;
                        Country countryShipper = null;
                        Country countryConsignee = null;
                        if (!string.IsNullOrEmpty(pm.ShipperId)) shipperCard = cardRepository.GetSingleCard(pm.ShipperId, tenant);
                        if (!string.IsNullOrEmpty(pm.ConsigneeId)) consigneeCard = cardRepository.GetSingleCard(pm.ConsigneeId, tenant);

                        if (!string.IsNullOrEmpty(pm.ShipperCountryId)) countryShipper = countryRepository.GetSingleCountry(pm.ShipperCountryId, tenant);
                        if (!string.IsNullOrEmpty(pm.ConsigneeCountryId)) countryConsignee = countryRepository.GetSingleCountry(pm.ConsigneeCountryId, tenant);


                        #region shipperSL
                        shipperSL = new PartnerSL()
                        {
                            Id = pm.ShipperId,
                            Code = shipperCard != null ? shipperCard.Code : "",
                            EnglishName = pm.ShipperName,
                            Address1 = pm.ShipperAddress1,
                            Address2 = pm.ShipperAddress2,
                            City = pm.ShipperCity,
                            CountryCode = countryShipper != null ? countryShipper.Code : "",
                            CountryName = countryShipper != null ? countryShipper.EnglishName : "",

                        };

                        #endregion

                        #region consigneeSL
                        consigneeSL = new PartnerSL()
                        {
                            Id = pm.ConsigneeId,
                            Code = consigneeCard != null ? consigneeCard.Code : "",
                            EnglishName = pm.ConsigneeName,
                            Address1 = pm.ConsigneeAddress1,
                            Address2 = pm.ConsigneeAddress2,
                            City = pm.ConsigneeCity,
                            CountryCode = countryConsignee != null ? countryConsignee.Code : "",
                            CountryName = countryConsignee != null ? countryConsignee.EnglishName : "",

                        };

                    }

                    #endregion

                    #region Notify1SL
                    PartnerSL notify1SL = new PartnerSL()
                    {
                        Id = pm.Notify1Id,
                        Code = notify1Card != null ? notify1Card.Code : "",
                        EnglishName = pm.Notify1Name,
                        Address1 = pm.Notify1Address1,
                        Address2 = pm.Notify1Address2,
                        City = pm.Notify1City,
                        CountryCode = countryNotify1 != null ? countryNotify1.Code : "",
                        CountryName = countryNotify1 != null ? countryNotify1.EnglishName : "",

                    };
                    #endregion

                    #region fromPortSL

                    PortSL mainCarriageFromPortSL = new PortSL()
                    {
                        Code = pm.FromPort,
                        EnglishName = pm.FromPortName,
                        CountryCode = pm.MainCarriageFromPortCountryCode,
                    };


                     transshipment1FromPort = new PortSL()
                    {
                        Code = pm.Transshipment1FromPortCode,
                        EnglishName = pm.Transshipment1FromPortName,
                        CountryCode = pm.Transshipment1FromPortCountryCode,
                    };
                    

                     transshipment2FromPort = new PortSL()
                    {
                        Code = pm.Transshipment2FromPortCode,
                        EnglishName = pm.Transshipment2FromPortName,
                        CountryCode = pm.Transshipment2FromPortCountryCode,
                    };


                     transshipment3FromPort = new PortSL()
                    {
                        Code = pm.Transshipment3FromPortCode,
                        EnglishName = pm.Transshipment3FromPortName,
                        CountryCode = pm.Transshipment3FromPortCountryCode,
                    };


                    #endregion

                    #region ToPortSL

                    PortSL finalDistenationPort = null;
                    if (!string.IsNullOrEmpty(pm.FinalDistenationPortId))
                    {
                        PortQuery portQuery = new PortQuery(tenant);
                        PortPM portPM = portQuery.GetSinglePM(pm.FinalDistenationPortId, pm.Tenant);
                         finalDistenationPort = new PortSL()
                        {
                            Code = portPM.Code,
                            EnglishName = portPM.EnglishName,
                            CountryCode = portPM.CountryCode,
                        };
                    }
               


                    PortSL mainCarriageToPortSL = new PortSL()
                    {
                        Code = pm.ToPort,
                        EnglishName = pm.ToPortName,
                        CountryCode = pm.MainCarriageToPortCountryCode,
                    };


                     transshipment1ToPort = new PortSL()
                    {
                        Code = pm.Transshipment1ToPortCode,
                        EnglishName = pm.Transshipment1ToPortName,
                        CountryCode = pm.Transshipment1ToPortCountryCode,
                    };

                     transshipment2ToPort = new PortSL()
                    {
                        Code = pm.Transshipment2ToPortCode,
                        EnglishName = pm.Transshipment2ToPortName,
                        CountryCode = pm.Transshipment2ToPortCountryCode,
                    };


                     transshipment3ToPort = new PortSL()
                    {
                        Code = pm.Transshipment3ToPortCode,
                        EnglishName = pm.Transshipment3ToPortName,
                        CountryCode = pm.Transshipment3ToPortCountryCode,
                    };


                    #endregion


                    #region ManifestSL

                    PortRepository portRepository = new PortRepository(tenant);

                    ManifestSL manifestSL = new ManifestSL()
                    {
                        AgentSharedManifestId  = IdCounter.GetNumber("AgentSharedManifest", destinationAgentTenant),
                        FinalDistenationPort = finalDistenationPort,
                        MAWBOBLDate = pm.MAWBOBLDate,
                        AgentSharedKey = agentSharedLogisticsKey.SharedKey,
                        SourceAgentTenant = sourceAgentTenant,
                        DestinationAgentTenant = destinationAgentTenant,
                        AgentName = currentAgent != null ? currentAgent.Card != null ? currentAgent.Card.EnglishName : "" : "",
                        DirectionId = pm.DirectionId,
                        ShipmentTypeId = pm.ShipmentTypeId,
                        TransportModeId = pm.TransportModeId,
                        FreightPrepaidCollectId = pm.FreightPrepaidCollectId,
                        OtherPrepaidCollectId = pm.OtherPrepaidCollectId,
                        ShipmentNumber = pm.ShipmentNumber,
                        IncotermCode = pm.IncotermCode,
                        IncotermName = !string.IsNullOrEmpty(pm.IncotermName) ? pm.IncotermName : pm.IncotermCode,
                        IncotermAddedManually = true,
                        CarrierCode = pm.MainCarriageCarrierCode,
                        CarrierName = !string.IsNullOrEmpty(pm.MainCarriageCarrierName) ? pm.MainCarriageCarrierName : pm.MainCarriageCarrierCode,
                        CarrierAddedManually = true,
                        MainCarriageFromPort = mainCarriageFromPortSL,
                        MainCarriageToPort = mainCarriageToPortSL,
                        Transshipment1FromPort = transshipment1FromPort,
                        Transshipment1ToPort = transshipment1ToPort,
                        Transshipment2FromPort = transshipment2FromPort,
                        Transshipment2ToPort = transshipment2ToPort,
                        Transshipment3FromPort = transshipment3FromPort,
                        Transshipment3ToPort = transshipment3ToPort,
                        
                        AgentReference1 = pm.AgentReference1,
                        AgentReference2 = pm.AgentReference2,
                        ShipmentLevelCode = pm.ShipmentLevelCode,
                        HouseNumber = pm.House,
                        MasterNumber = pm.Master,
                        LongMaster = pm.TransportModeId == "A" ? (!string.IsNullOrEmpty(pm.AirlinePrefix) && !string.IsNullOrEmpty(pm.Master) ? pm.AirlinePrefix + "-" + pm.Master : null) : pm.Master,
                        Consignee = !string.IsNullOrEmpty(pm.ConsigneeId) ? consigneeSL : null,
                        Shipper = !string.IsNullOrEmpty(pm.ShipperId) ? shipperSL : null,
                        Notify1 = !string.IsNullOrEmpty(pm.Notify1Id) ? notify1SL : null,
                        GeneralDescriptionOfGoods = pm.DescriptionOfGoods,
                        ShipmentPackages = ReBulidShipmentPackages(destinationAgentTenant, pm.ShipmentPackages),
                        GrossWeight = pm.GrossWeight,
                        ChargeableWeight = pm.ChargeableWeight,
                        TEU = pm.TEU,
                        PackagesQuantity = pm.PackagesQuantity,
                        Volume = pm.Volume,
                        VolumetricWeight = pm.VolumetricWeight,
                        NumberOfPackages = pm.NumberOfPackages,
                        NumberOfContainers = pm.NumberOfContainers,
                        GrossWeightInKG = pm.GrossWeightInKG,
                        ChargeableWeightInKG = pm.ChargeableWeightInKG,
                        GrossWeightEdited = pm.GrossWeightEdited,
                        GrossWeightUnitCode = pm.GrossWeightUnitCode,
                        ChargeableWeightUnitCode = pm.ChargeableWeightUnitCode,
                        VolumeUnitCode = pm.VolumeUnitCode,
                        DimensionsUnitCode = pm.DimensionsUnitCode,
                        ShipmentTypeName = pm.ShipmentTypeName,
                        OrderGrossWeight = pm.OrderGrossWeight,
                        ShipperName = pm.ShipperName,

                        ShipperReference1 =pm.ShipperReference1,
                        ShipperReference2 = pm.ShipperReference2,
                        ConsigneeReference1 = pm.ConsigneeReference1,
                        ConsigneeReference2 = pm.ConsigneeReference2,

                        MainHarmonize = pm.MainHarmonize,
                        MainCarriageAirlinePrefix = pm.AirlinePrefix,
                        MainCarriageVesselName = pm.MainCarriageVesselName,//translate
                        MainCarriageVesselAddedManually = true,
                        InterlineAddedManually = true, //translate
                        MainCarriageETD = pm.MainCarriageETD,
                        MainCarriageATD = pm.MainCarriageATD,
                        MainCarriageETA = pm.MainCarriageETA,
                        MainCarriageATA = pm.MainCarriageATA,

                        Transshipment1ETD = pm.Transshipment1ETD,
                        Transshipment1ATD = pm.Transshipment1ATD,
                        Transshipment1ETA = pm.Transshipment1ETA,
                        Transshipment1ATA = pm.Transshipment1ATA,

                        Transshipment1MAWBOBL = pm.Transshipment1AdditionalMAWBOBLBL,
                        Transshipment1VesselName = pm.Transshipment1VesselName,//translate
                        Transshipment1VesselAddedManually = true,

                        Transshipment1CarrierCode = pm.Transshipment1CarrierCode,
                        Transshipment1CarrierName = pm.Transshipment1CarrierName,
                        Transshipment1CarrierAddedManually = true,

                        ValueOfGoods = pm.ValueOfGoods,
                        Transshipment2ETD = pm.Transshipment2ETD,
                        Transshipment2ATD = pm.Transshipment2ATD,
                        Transshipment2ETA = pm.Transshipment2ETA,
                        Transshipment2ATA = pm.Transshipment2ATA,
                        Transshipment2MAWBOBL = pm.Transshipment2AdditionalMAWBOBLBL,
                        Transshipment2VesselName = pm.Transshipment2VesselName,//translate
                        Transshipment2VesselAddedManually = true,

                        Transshipment2CarrierCode = pm.Transshipment2CarrierCode,
                        Transshipment2CarrierName = pm.Transshipment2CarrierName,
                        Transshipment2CarrierAddedManually = true,


                        Transshipment3ETD = pm.Transshipment3ETD,
                        Transshipment3ATD = pm.Transshipment3ATD,
                        Transshipment3ETA = pm.Transshipment3ETA,
                        Transshipment3ATA = pm.Transshipment3ATA,
                        Transshipment3MAWBOBL = pm.Transshipment3AdditionalMAWBOBLBL,
                        Transshipment3VesselName = pm.Transshipment3VesselName,//translate
                        Transshipment3VesselAddedManually = true,

                        Transshipment3CarrierCode = pm.Transshipment3CarrierCode,
                        Transshipment3CarrierName = pm.Transshipment3CarrierName,
                        Transshipment3CarrierAddedManually = true,

                        MainCarriageCarrierNumber = pm.MainCarriageCarrierNumber,
                        Transshipment1CarrierNumber = pm.Transshipment1CarrierNumber ,
                        Transshipment2CarrierNumber = pm.Transshipment2CarrierNumber,
                        Transshipment3CarrierNumber = pm.Transshipment3CarrierNumber,
                        IsDangerous =  pm.IsDangerous ,
                        TruckNumber= pm.TruckNumber,
                        TrailerNumber = pm.TrailerNumber,
                        HAWBDate = pm.HAWBDate,
                        MasterDate = pm.CreateDateTime,
                        MoveTypeCode = pm.MoveTypeCode,
                        MoveTypeName = !string.IsNullOrEmpty(pm.MoveTypeName) ? pm.MoveTypeName : pm.MoveTypeCode,
                        OrginalAgentId = pm.AgentId,

                    };



                    #region Pickup  Delivery Leg
                    ShipmentPickUpQuery shipmentPickUpQuery = new ShipmentPickUpQuery(tenant);
              
                    ShipmentPickUpPM shipmentPickUpPM = shipmentPickUpQuery.GetFistShipmentPickUpPMByTenantAndShipmentId(pm.Id, pm.ShipmentNumber, pm.Tenant);
                    if (shipmentPickUpPM != null) manifestSL.ShipmentPickUp = BuildShipmentPickUpDeliverySL(shipmentPickUpPM, null);


                    ShipmentDeliveryQuery shipmentDeliveryQuery = new ShipmentDeliveryQuery(tenant);
                    ShipmentDeliveryPM shipmentDeliveryPM = shipmentDeliveryQuery.GetFirstShipmentDeliveryPMsByTenantAndShipment(pm.Id, pm.ShipmentNumber , pm.Tenant);
                    if (shipmentDeliveryPM != null) manifestSL.ShipmentDelivery = BuildShipmentPickUpDeliverySL(null, shipmentDeliveryPM);
                    #endregion



                    #endregion


                    TransLateOtherShipmentDetails(pm , manifestSL , tenant, destinationAgentTenant);

                    if (pm.ShipmentLevelCode == "C")
                    {
                        #region HouseList
                        List<ShipmentPM> housesList = shipmentQuery.GetShipmentPMsByMasterIdAndTenantForSharedManifest(pm.Id, tenant);

                        foreach (ShipmentPM housePM in housesList)
                        {
                            Country countryShipperHouse = null;
                            Country countryConsigneeHouse = null;
                            Country countryNotify1House = null;
                            Country countryNotify2House = null;

                            Card shipperCardHouse = null;
                            Card consigneeCardHouse = null;

                            Card notify1CardHouse = null;
                            Card notify2CardHouse = null;


                            ShipmentPackageQuery shipmentPackageQuery = new ShipmentPackageQuery(tenant);
                            housePM.ShipmentPackages = shipmentPackageQuery.GetShipmentPackages(housePM.Id, housePM.ShipmentNumber, housePM.Tenant);

                            if (!string.IsNullOrEmpty(housePM.ShipperAddressId))
                            {
                                AddressRepository addressRepository = new AddressRepository(tenant);
                                Address shipperAddress = addressRepository.GetSingleAddress(housePM.ShipperAddressId, tenant);
                                if (shipperAddress != null)
                                {
                                    housePM.ShipperAddress1 = shipperAddress.Address1;
                                    housePM.ShipperAddress2 = shipperAddress.Address2;
                                    housePM.ShipperCity = shipperAddress.City;
                                    housePM.ShipperCountryId = shipperAddress.CountryId;

                                }
                            }

                            if (!string.IsNullOrEmpty(housePM.ConsigneeAddressId))
                            {
                                AddressRepository addressRepository = new AddressRepository(tenant);
                                Address consigneeAddress = addressRepository.GetSingleAddress(housePM.ConsigneeAddressId, tenant);
                                if (consigneeAddress != null)
                                {
                                    housePM.ConsigneeAddress1 = consigneeAddress.Address1;
                                    housePM.ConsigneeAddress2 = consigneeAddress.Address2;
                                    housePM.ConsigneeCity = consigneeAddress.City;
                                    housePM.ConsigneeCountryId = consigneeAddress.CountryId;

                                }
                            }

                            //Card
                            if (!string.IsNullOrEmpty(housePM.ShipperId)) shipperCardHouse = cardRepository.GetSingleCard(housePM.ShipperId, tenant);
                            if (!string.IsNullOrEmpty(housePM.ConsigneeId)) consigneeCardHouse = cardRepository.GetSingleCard(housePM.ConsigneeId, tenant);
                            if (!string.IsNullOrEmpty(housePM.Notify1Id)) notify1CardHouse = cardRepository.GetSingleCard(housePM.Notify1Id, tenant);
                            if (!string.IsNullOrEmpty(housePM.Notify2Id)) notify2CardHouse = cardRepository.GetSingleCard(housePM.Notify2Id, tenant);


                            //Country
                            if (!string.IsNullOrEmpty(housePM.ShipperCountryId)) countryShipperHouse = countryRepository.GetSingleCountry(housePM.ShipperCountryId, tenant);
                            if (!string.IsNullOrEmpty(housePM.ConsigneeCountryId)) countryConsigneeHouse = countryRepository.GetSingleCountry(housePM.ConsigneeCountryId, tenant);
                            if (!string.IsNullOrEmpty(housePM.Notify1CountryId)) countryNotify1House = countryRepository.GetSingleCountry(housePM.Notify1CountryId, tenant);


                            #region houseShipperSL
                            PartnerSL houseShipperSL = new PartnerSL()
                            {
                                Id = housePM.ShipperId,
                                Code = shipperCardHouse != null ? shipperCardHouse.Code : "",
                                EnglishName = shipperCardHouse != null ? shipperCardHouse.EnglishName : housePM.ShipperName,
                                Address1 = housePM.ShipperAddress1,
                                Address2 = housePM.ShipperAddress2,
                                City = housePM.ShipperCity,
                                CountryCode = countryShipperHouse != null ? countryShipperHouse.Code : "",
                                CountryName = countryShipperHouse != null ? countryShipperHouse.EnglishName : "",

                            };

                            #endregion

                            #region houseConsigneeSL
                            PartnerSL houseConsigneeSL = new PartnerSL()
                            {
                                Id = housePM.ConsigneeId,
                                Code = consigneeCardHouse != null ? consigneeCardHouse.Code : "",
                                EnglishName = consigneeCardHouse != null ? consigneeCardHouse.EnglishName : housePM.ConsigneeName,
                                Address1 = housePM.ConsigneeAddress1,
                                Address2 = housePM.ConsigneeAddress2,
                                City = housePM.ConsigneeCity,
                                CountryCode = countryConsigneeHouse != null ? countryConsigneeHouse.Code : "",
                                CountryName = countryConsigneeHouse != null ? countryConsigneeHouse.EnglishName : "",

                            };

                            #endregion

                            #region houseNotify1SL
                            PartnerSL houseNotify1SL = new PartnerSL()
                            {
                                Id = housePM.Notify1Id,
                                Code = notify1CardHouse != null ? notify1CardHouse.Code : "",
                                EnglishName = notify1CardHouse != null ? notify1CardHouse.EnglishName : housePM.Notify1Name,
                                Address1 = housePM.Notify1Address1,
                                Address2 = housePM.Notify1Address2,
                                City = housePM.Notify1City,
                                CountryCode = countryNotify1House != null ? countryNotify1House.Code : "",
                                CountryName = countryNotify1House != null ? countryNotify1House.EnglishName : "",

                            };
                            #endregion

                            #region houseNotify2SL
                            //PartnerSL houseNotify2SL = new PartnerSL()
                            //{
                            //    Id = housePM.Notify2Id,
                            //    Code = notify2CardHouse != null ? notify2CardHouse.Code : "",
                            //    EnglishName = notify2CardHouse != null ? notify2CardHouse.EnglishName : housePM.Notify2Name,
                            //    Address1 = housePM.Notify2AddressId,
                            //    Address2 = housePM.Notify2AddressId,

                            //};

                            #endregion

                            #region HouseSL
                            HouseSL houseSL = new HouseSL()
                            {
                           
                                AgentName = currentAgent != null ? currentAgent.Card != null ? currentAgent.Card.EnglishName : "" : "",
                                ShipmentNumber = housePM.ShipmentNumber,
                                HouseNumber = housePM.House,
                                FreightPrepaidCollectId = housePM.FreightPrepaidCollectId,
                                OtherPrepaidCollectId = housePM.OtherPrepaidCollectId,
                                IncotermCode = housePM.IncotermCode,
                                IncotermName = !string.IsNullOrEmpty(housePM.IncotermName) ? housePM.IncotermName : housePM.IncotermCode,
                                IsDangerous = housePM.IsDangerous,
                                Consignee = !string.IsNullOrEmpty(housePM.ConsigneeId) ? houseConsigneeSL : null,
                                Shipper = !string.IsNullOrEmpty(housePM.ShipperId) ? houseShipperSL : null,
                                Notify1 = !string.IsNullOrEmpty(housePM.Notify1Id) ? houseNotify1SL : null,
                                MainCarriageCarrierNumber = housePM.MainCarriageCarrierNumber,
                                CarrierCode = housePM.MainCarriageCarrierCode,
                                CarrierName = !string.IsNullOrEmpty(housePM.MainCarriageCarrierName) ? housePM.MainCarriageCarrierName : housePM.MainCarriageCarrierCode,
                                ValueOfGoods = housePM.ValueOfGoods,
                                GeneralDescriptionOfGoods = housePM.DescriptionOfGoods,
                                ShipmentPackages = ReBulidShipmentPackages(destinationAgentTenant, housePM.ShipmentPackages),
                                GrossWeight = housePM.GrossWeight,
                                ChargeableWeight = housePM.ChargeableWeight,
                                TEU = housePM.TEU,
                                PackagesQuantity = housePM.PackagesQuantity,
                                Volume = housePM.Volume,
                                VolumetricWeight = housePM.VolumetricWeight,
                                NumberOfPackages = housePM.NumberOfPackages,
                                NumberOfContainers = housePM.NumberOfContainers,
                                GrossWeightInKG = housePM.GrossWeightInKG,
                                ChargeableWeightInKG = housePM.ChargeableWeightInKG,
                                GrossWeightEdited = housePM.GrossWeightEdited,
                                GrossWeightUnitCode = housePM.GrossWeightUnitCode,
                                ChargeableWeightUnitCode = housePM.ChargeableWeightUnitCode,
                                VolumeUnitCode = housePM.VolumeUnitCode,
                                DimensionsUnitCode = housePM.DimensionsUnitCode,
                                ShipmentTypeName = housePM.ShipmentTypeName,
                                OrderGrossWeight = housePM.OrderGrossWeight,
                                ShipmentTypeId = housePM.ShipmentTypeId,
                                HAWBDate = housePM.HAWBDate,
               
                                ShipperReference1 = housePM.ShipperReference1,
                                ShipperReference2 = housePM.ShipperReference2,
                                ConsigneeReference1 = housePM.ConsigneeReference1,
                                ConsigneeReference2 = housePM.ConsigneeReference2,

                                MainHarmonize = housePM.MainHarmonize,
                                MoveTypeCode = housePM.MoveTypeCode,
                                MoveTypeName = !string.IsNullOrEmpty(housePM.MoveTypeName) ? housePM.MoveTypeName : housePM.MoveTypeCode,
                            };


                            if (!string.IsNullOrEmpty(housePM.IncotermId))
                            {
                                IncotermQuery incotermQuery = new IncotermQuery(tenant);
                                IncotermPM incotermPM = incotermQuery.GetIncotermPMById(housePM.IncotermId, tenant);
                                if (incotermPM != null)
                                {
                                    if (!incotermPM.AddedManually)
                                    {
                                        string desIncotermId = incotermQuery.GetIncotermIdByCode(incotermPM.Code, destinationAgentTenant);
                                        if (!string.IsNullOrEmpty(desIncotermId))
                                        {
                                            houseSL.IncotermId = desIncotermId;
                                            houseSL.IncotermAddedManually = false;
                                        }

                                    }
                                }

                            }

                            if (!string.IsNullOrEmpty(housePM.MoveTypeId))
                            {
                                MoveTypeQuery moveTypeQuery = new MoveTypeQuery(tenant);
                                MoveTypePM moveTypePM = moveTypeQuery.GetSingleMoveTypePM(housePM.MoveTypeId, tenant);
                                if (moveTypePM != null)
                                {
                                    if (!moveTypePM.AddedManually)
                                    {
                                        string desMoveTypeId = moveTypeQuery.GetMoveTypeIdByCode(moveTypePM.Code, destinationAgentTenant);
                                        if (!string.IsNullOrEmpty(desMoveTypeId))
                                        {
                                            houseSL.MoveTypeId = desMoveTypeId;
                                            houseSL.MoveTypeAddedManually = false;
                                        }

                                    }
                                }

                            }

                       
                            #region ValueOfGoodsCurrency
                            if (!string.IsNullOrEmpty(housePM.ValueOfGoodsCurrencyId))
                            {
                                CurrencyQuery currencyQuery = new CurrencyQuery(tenant);
                                CurrencyPM currencyPM = currencyQuery.GetSinglePM(housePM.ValueOfGoodsCurrencyId, tenant);
                                if (currencyPM != null)
                                {
                                    houseSL.ValueOfGoodsCurrencyCode = currencyPM.Code;
                                    houseSL.ValueOfGoodsCurrencyName = currencyPM.EnglishName;

                                    if (!currencyPM.AddedManually)
                                    {
                                        string desIncotermId = currencyQuery.GetCurrencyIdByCode(currencyPM.Code, destinationAgentTenant);
                                        if (!string.IsNullOrEmpty(desIncotermId))
                                        {
                                            if (houseSL != null)
                                            {
                                                houseSL.ValueOfGoodsCurrencyId = desIncotermId;
                                                houseSL.ValueOfGoodsCurrencyAddedManually = false;
                                            }

                                        }

                                    }
                                }

                            }

                            #endregion


                            houseSL.CarrierId = manifestSL.CarrierId;
                            houseSL.CarrierAddedManually = manifestSL.CarrierAddedManually = false;

                            #endregion

                            manifestSL.Houses.Add(houseSL);
                        }
                        #endregion
                    }

  
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        //string manifestXML = LogitudeXmlSerializer.SerializeObjectToXmlString(manifestSL);
                        byte[] manifestXML = LogitudeXmlSerializer.SerializeObject(manifestSL);
                        ObjectTableQuery tablesQuery = new ObjectTableQuery(tenant);
                        ObjectTablePM table = tablesQuery.GetObjectTableByName("Shipment", 0);
                        CommunicationsParams logParams = new CommunicationsParams()
                        {
                            Tenant = tenant,
                            From = sourceAgentTenantPOCO.Company + " " + sourceAgentTenant,
                            To = destinationAgentTenantPOCO.Company + " " + destinationAgentTenant,
                            CommunicationLogTypeCode = "Q",
                            QueueName = "AgentsSharedLogisticsQueue",
                            Priority = 1,
                            InOut = "O",
                            Status = "W",
                            LoggingUserId = loggedContact.Id,
                            LoggingObjectTableId = table.Id,
                            LoggingEntityId = pm.Id,
                            Subject = !isUpdateAgent ? "Shared Manifest" : "Update Shared Agent",
                            FolderName = "AgentsSharedLogisticsQueue",
                            ByteData = manifestXML,
                        };



                        Communications.AddCommunicationLog(logParams);

                        /*

                         * Shipment XML will be sent via the communication log to the Agent Tenant
    Subject: Agents Shared Logistics
    Log type: Transmission
    Object table: Shipment
    From (tenant name)
    To (recipient tenant name + number)
    Mapping shipments data


                         * */



                        //ICommonDataContext agentContext = CommonDataContext.GetContext(agentTenant);
                        //ContactRepository contactRepository = new ContactRepository(agentContext);
                        //Contact systemContact = contactRepository.GetSingleContactByEmail("system@tenant" + agentTenant + ".com", agentTenant, false);

                        //AgentSharedManifestService service = new AgentSharedManifestService(agentContext, agentTenant);
                        //AgentSharedManifestPM agentSharedPM = new AgentSharedManifestPM()
                        //{
                        //    Id = IdCounter.GetNumber("AgentSharedManifest", agentTenant),
                        //    CreateDate = DateTime.Now,
                        //    Tenant = agentTenant,
                        //    UpdateDate = DateTime.Now,
                        //    UpdatedByUserId = systemContact.Id,
                        //    AgentReference = pm.ShipmentNumber,
                        //    Master = pm.Master,
                        //    ManifestXML = manifestXML,
                        //    StatusCode = "WAIT",
                        //    TransportModeId = pm.TransportModeId,
                        //    DirectionId = pm.DirectionId,
                        //    FromPortId = pm.MainCarriageFromPortId,
                        //    ToPortId = pm.MainCarriageToPortId,
                        //    GrossWeight = pm.GrossWeight,
                        //    ChargeableWeight = pm.ChargeableWeight,
                        //    TEU = pm.TEU,
                        //    PackagesQuantity = pm.PackagesQuantity,
                        //    AgentId = pm.AgentId,
                        //};

                        //service.Create(agentSharedPM);

                        #region UpDate Shipment IsManifestSentToAgent
                        ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                        Shipment shipment = shipmentRepository.GetSingleShipmentwithOutIncludes(shipmentId, tenant);
                        shipment.IsManifestSentToAgent = true;
                        shipment.ManifestLastSharingDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                        shipmentRepository.Update(shipment);
                        shipmentRepository.SubmitChanges();
                        #endregion

                        scope.Complete();
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, pm);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Shipment should have an agent!");


            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetSingle(string id)
        {
            try
            {

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                AgentSharedManifestQuery agentSharedManifestQuery = new AgentSharedManifestQuery(authToken.Tenant);
                PortRepository portRepository = new PortRepository(authToken.Tenant);

                AgentSharedManifestPM agentSharedManifestPM = agentSharedManifestQuery.GetSinglePM(id, authToken.Tenant);

                ManifestSL manifestSL = LogitudeXmlSerializer.DeserializeObject<ManifestSL>(agentSharedManifestPM.ManifestXML);
                /*
                 * WI 27979
                 * use the first agent (from agents table) on the receiver tenant as the AgentId for the translations
                 * */
                   // AgentRepository agentRep = new AgentRepository(authToken.Tenant);
               // Agent agentCard = agentRep.GetAgents(authToken.Tenant).OrderBy(a => a.Id).FirstOrDefault();
                /* 
                 * 
                */
                SharedManifestTranslationRepository sharedManifestTransRepository = new SharedManifestTranslationRepository(authToken.Tenant);
                SharedManifestTranslationQuery sharedTranslationsQuery = new SharedManifestTranslationQuery(authToken.Tenant);

                List<SharedManifestTranslationPM> translations = sharedTranslationsQuery.GetSharedManifestTranslationPMsByAgentId(authToken.Tenant, agentSharedManifestPM.AgentId).ToList();

                agentSharedManifestPM.SharedManifestTranslations = translations;
                agentSharedManifestPM.ManifestSL = manifestSL;

                if (manifestSL.MainCarriageFromPort != null)
                {
                    WcfServicesHelper.GetPortOrCopyToTenant(manifestSL.MainCarriageFromPort.CountryCode + manifestSL.MainCarriageFromPort.Code, authToken.Tenant, portRepository);
                }
                if (manifestSL.MainCarriageToPort != null)
                {
                    WcfServicesHelper.GetPortOrCopyToTenant(manifestSL.MainCarriageToPort.CountryCode + manifestSL.MainCarriageToPort.Code, authToken.Tenant, portRepository);
                }

                if (manifestSL.Transshipment1FromPort != null)
                {
                    WcfServicesHelper.GetPortOrCopyToTenant(manifestSL.Transshipment1FromPort.CountryCode + manifestSL.Transshipment1FromPort.Code, authToken.Tenant, portRepository);
                }
                if (manifestSL.Transshipment1ToPort != null)
                {
                    WcfServicesHelper.GetPortOrCopyToTenant(manifestSL.Transshipment1ToPort.CountryCode + manifestSL.Transshipment1ToPort.Code, authToken.Tenant, portRepository);
                }

                if (manifestSL.Transshipment2FromPort != null)
                {
                    WcfServicesHelper.GetPortOrCopyToTenant(manifestSL.Transshipment2FromPort.CountryCode + manifestSL.Transshipment2FromPort.Code, authToken.Tenant, portRepository);
                }
                if (manifestSL.Transshipment2ToPort != null)
                {
                    WcfServicesHelper.GetPortOrCopyToTenant(manifestSL.Transshipment2ToPort.CountryCode + manifestSL.Transshipment2ToPort.Code, authToken.Tenant, portRepository);
                }

                if (manifestSL.Transshipment3FromPort != null)
                {
                    WcfServicesHelper.GetPortOrCopyToTenant(manifestSL.Transshipment3FromPort.CountryCode + manifestSL.Transshipment3FromPort.Code, authToken.Tenant, portRepository);
                }
                if (manifestSL.Transshipment3ToPort != null )
                {
                    WcfServicesHelper.GetPortOrCopyToTenant(manifestSL.Transshipment3ToPort.CountryCode + manifestSL.Transshipment3ToPort.Code, authToken.Tenant, portRepository);
                }


                return Request.CreateResponse(HttpStatusCode.OK, agentSharedManifestPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetSharedAgentManifestTransLateIdByCode(string code ,int tenant )
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);

                CardRepository cardRepository = new CardRepository(tenant);
                string result = cardRepository.GetActiveCardIdByCode(code, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, result);

            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage Put(AgentSharedManifestPM entityPM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string logKey = PerformanceLogger.LogCurrentTime();
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
                        SecurityUtility.AuthenticationOnEntityTenant("AgentSharedManifest" , entityPM.Tenant, authToken.Tenant);

                        string entityName = "AgentSharedManifest" + entityPM.Id + entityPM.Tenant;
                        string entityPmName = "AgentSharedManifestPM" + entityPM.Id + entityPM.Tenant;
                        if (CacheManager.CacheWrapper.Get(entityName) != null)
                        {
                            CacheManager.CacheWrapper.Invalidate(entityName);
                        }
                        if (CacheManager.CacheWrapper.Get(entityPmName) != null)
                        {
                            CacheManager.CacheWrapper.Invalidate(entityPmName);
                        }

                        ICommonDataContext MyContext = CommonDataContext.GetContext(entityPM.Tenant);
                        AgentSharedManifestService service = new AgentSharedManifestService(MyContext, entityPM.Tenant);
                        service.Update(entityPM);

                      


                        scope.Complete();
                        PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                        return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                    }
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }

        public HttpResponseMessage PostAgentSharedManifesRefShipmentListsByIds(List<string> agentManifestSharedRefListIds)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                ShipmentQuery shipmentQuery = new ShipmentQuery(authToken.Tenant);

                List<AgentSharedManifesRefShipment> result = shipmentQuery.GetAgentSharedManifesRefShipmentLists(agentManifestSharedRefListIds, authToken.Tenant);
                return Request.CreateResponse(HttpStatusCode.OK, result);

            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        
        public HttpResponseMessage GetAgentSharedManifestsWorkspaceSummary()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                AgentSharedManifestsSummaryClass agentSharedManifestsSummaryClass = new AgentSharedManifestsSummaryClass();

                AgentSharedManifestQuery agentSharedManifestQuery = new AgentSharedManifestQuery(authToken.Tenant);
                IQueryable<AgentSharedManifestList> AgentSharedManifestLists = agentSharedManifestQuery.GetAgentSharedManifestListsByTenant(authToken.Tenant);

                if (AgentSharedManifestLists != null)
                {

                    agentSharedManifestsSummaryClass.AgentSharedManifestsAirCount = AgentSharedManifestLists.Where(d => d.TransportModeId == "A" && d.StatusCode =="WAIT").Count();
                    agentSharedManifestsSummaryClass.AgentSharedManifestsOceanCount = AgentSharedManifestLists.Where(d => d.TransportModeId == "O" && d.StatusCode == "WAIT").Count();
                    agentSharedManifestsSummaryClass.AgentSharedManifestsInlandCount = AgentSharedManifestLists.Where(d => d.TransportModeId == "I" && d.StatusCode == "WAIT").Count();
                    agentSharedManifestsSummaryClass.AgentSharedManifestsCancelledCount = AgentSharedManifestLists.Where(d => d.StatusCode == "CANC").Count();
                    agentSharedManifestsSummaryClass.AgentSharedManifestsAllCount = AgentSharedManifestLists.Count();

                }
                return Request.CreateResponse(HttpStatusCode.OK, agentSharedManifestsSummaryClass);

            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetCheckIfAnyShipmentHaveMasterNumber(string master, string longMaster , int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                SecurityUtility.AuthenticationOnTenant(tenant);

                ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);

                if (longMaster == "null" || longMaster == "undefined") longMaster = null;
                if (master == "null" || master == "undefined") master = null;
                string reulst = shipmentQuery.GetShipmentNumberByMaster(master, longMaster, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, reulst);

            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        private void TransLateOtherShipmentDetails(ShipmentPM pm , ManifestSL manifestSL,  int tenant , int desTenant)
        {
            #region Incoterm

   
            if (!string.IsNullOrEmpty(pm.IncotermId))
            {
                IncotermQuery incotermQuery = new IncotermQuery(tenant);
                IncotermPM incotermPM = incotermQuery.GetIncotermPMById(pm.IncotermId, tenant);
                if (incotermPM != null)
                {
                    if (!incotermPM.AddedManually)
                    {
                        string desIncotermId= incotermQuery.GetIncotermIdByCode(incotermPM.Code, desTenant);
                        if (!string.IsNullOrEmpty(desIncotermId))
                        {
                            if (manifestSL != null)
                            {
                                manifestSL.IncotermId = desIncotermId;
                                manifestSL.IncotermAddedManually = false;
                            }
                           
                        }
                 
                    }
                }

            }
            #endregion

                #region MainCarriageCarrierId
            //Carrier
            TransLateCarriage(pm.MainCarriageCarrierId, "MainCarriageCarrierId", manifestSL, tenant, desTenant);
            TransLateCarriage(pm.Transshipment1CarrierId, "Transshipment1CarrierId", manifestSL, tenant, desTenant);
            TransLateCarriage(pm.Transshipment2CarrierId, "Transshipment2CarrierId", manifestSL, tenant, desTenant);
            TransLateCarriage(pm.Transshipment3CarrierId, "Transshipment3CarrierId", manifestSL, tenant, desTenant);

            #endregion
            
                #region Vessel

                if (pm.TransportModeId == "O")
                {
                    TransLateVessel(pm.MainCarriageVesselId, "MainCarriageVesselId", manifestSL, tenant, desTenant);
                    TransLateVessel(pm.Transshipment1VesselId, "Transshipment1VesselId", manifestSL, tenant, desTenant);
                    TransLateVessel(pm.Transshipment2VesselId, "Transshipment2VesselId", manifestSL, tenant, desTenant);
                    TransLateVessel(pm.Transshipment3VesselId, "Transshipment3VesselId", manifestSL, tenant, desTenant);

                }


                #endregion

                #region Interline

                if (pm.TransportModeId == "A")
                {
                    TransLateInterline(pm.InterlineId, "InterlineId", manifestSL, tenant, desTenant);
                }

            #endregion

               #region MoveType


            if (!string.IsNullOrEmpty(pm.MoveTypeId))
            {
                MoveTypeQuery moveTypeQuery = new MoveTypeQuery(tenant);
                MoveTypePM moveTypePM = moveTypeQuery.GetSingleMoveTypePM(pm.MoveTypeId, tenant);
                if (moveTypePM != null)
                {
                    manifestSL.MoveTypeTransportModeId = moveTypePM.TransportModeId;
                    if (!moveTypePM.AddedManually)
                    {
                        string desMoveTypeId = moveTypeQuery.GetMoveTypeIdByCode(moveTypePM.Code, desTenant);
                        if (!string.IsNullOrEmpty(desMoveTypeId))
                        {
                            if (manifestSL != null)
                            {
                                manifestSL.MoveTypeId = desMoveTypeId;
                                manifestSL.MoveTypeAddedManually = false;
                            }

                        }

                    }
                }

            }
            #endregion

               #region ValueOfGoodsCurrency
            if (!string.IsNullOrEmpty(pm.ValueOfGoodsCurrencyId))
            {
                CurrencyQuery currencyQuery = new CurrencyQuery(tenant);
                CurrencyPM currencyPM = currencyQuery.GetSinglePM(pm.ValueOfGoodsCurrencyId, tenant);
                if (currencyPM != null)
                {
                    manifestSL.ValueOfGoodsCurrencyCode = currencyPM.Code;
                    manifestSL.ValueOfGoodsCurrencyName = currencyPM.EnglishName;

                    if (!currencyPM.AddedManually)
                    {
                        string desIncotermId = currencyQuery.GetCurrencyIdByCode(currencyPM.Code, desTenant);
                        if (!string.IsNullOrEmpty(desIncotermId))
                        {
                            if (manifestSL != null)
                            {
                                manifestSL.ValueOfGoodsCurrencyId = desIncotermId;
                                manifestSL.ValueOfGoodsCurrencyAddedManually = false;
                            }

                        }

                    }
                }

            }

            #endregion

        }

        private void TransLateCarriage(string carrierId,string fieldName, ManifestSL manifestSL, int tenant, int desTenant) 
        {
            CardQuery cardQuery = new CardQuery(tenant);
            if (!string.IsNullOrEmpty(carrierId))
            {

                CardPM cardPM = cardQuery.GetSinglePM(carrierId, tenant);
                string cardCode = "";
                if (cardPM != null)
                {
                    bool addedManually = false;
                    if (cardPM.PartnerTypeId == "TR")
                    {
                        TruckerQuery truckerQuery = new TruckerQuery(0);
                        addedManually = truckerQuery.CheckTruckerAddedManually(cardPM.Id, cardPM.Tenant);
                        if (!addedManually) cardCode = cardPM.Code;

                    }

                    if (cardPM.PartnerTypeId == "AL")
                    {
                        AirlineQuery airlineQuery = new AirlineQuery(0);
                        addedManually = airlineQuery.CheckAirlinesAddedManually(cardPM.Id, cardPM.Tenant);
                        if (!addedManually) cardCode = cardPM.Code;
                    }

                    if (cardPM.PartnerTypeId == "SL")
                    {
                        ShippingLineQuery shippingLineQuery = new ShippingLineQuery(0);
                        addedManually = shippingLineQuery.CheckShippingLinesAddedManually(cardPM.Id, cardPM.Tenant);
                        if (!addedManually) cardCode = cardPM.Code;
                    }

                }

                if (!string.IsNullOrEmpty(cardCode))
                {
                    CardRepository cardRepository = new CardRepository(desTenant);
                    Card desCard = cardRepository.GetSingleCardByCode(cardPM.Code, desTenant, true);

                    if (desCard != null)
                    {
                        if (manifestSL != null)
                        {
                            if(fieldName == "MainCarriageCarrierId")
                            {
                                manifestSL.CarrierId = desCard.Id;
                                manifestSL.CarrierAddedManually = false;
                            }
                            else if (fieldName == "Transshipment1CarrierId")
                            {
                                manifestSL.Transshipment1CarrierId = desCard.Id;
                                manifestSL.Transshipment1CarrierAddedManually = false;
                            }
                            else if (fieldName == "Transshipment2CarrierId")
                            {
                                manifestSL.Transshipment2CarrierId = desCard.Id;
                                manifestSL.Transshipment2CarrierAddedManually = false;
                            }
                            else if (fieldName == "Transshipment3CarrierId")
                            {
                                manifestSL.Transshipment3CarrierId = desCard.Id;
                        
                                manifestSL.Transshipment3CarrierAddedManually = false;
                            }

                        }
                      

                    }


                }
            }
        }

        private void TransLateVessel(string vesselId, string fieldName, ManifestSL manifestSL,  int tenant, int desTenant)
        {
            if (manifestSL != null && !string.IsNullOrEmpty(vesselId))
            {
                VesselQuery vesselQuery = new VesselQuery(tenant);
                VesselPM vesselPM = vesselQuery.GetSinglePM(vesselId, tenant);
                if (vesselPM != null)
                {
                    if (!vesselPM.AddedManually)
                    {
                        VesselPM desvesselPM = vesselQuery.GetVesselPMByCode(vesselPM.Code, desTenant);
                        if (desvesselPM != null)
                        {
                            if(fieldName == "MainCarriageVesselId")
                            {
                                manifestSL.MainCarriageVesselId = desvesselPM.Id;
                                manifestSL.MainCarriageVesselName = desvesselPM.EnglishName;
                                manifestSL.MainCarriageVesselCode = desvesselPM.Code;
                                manifestSL.MainCarriageVesselAddedManually = false;
                            }

                            else if (fieldName == "Transshipment1VesselId")
                            {
                                manifestSL.Transshipment1VesselId = desvesselPM.Id;
                                manifestSL.Transshipment1VesselName = desvesselPM.EnglishName;
                                manifestSL.Transshipment1VesselCode = desvesselPM.Code;
                                manifestSL.Transshipment1VesselAddedManually = false;

                            }
                            else if (fieldName == "Transshipment2VesselId")
                            {
                                manifestSL.Transshipment2VesselId = desvesselPM.Id;
                                manifestSL.Transshipment2VesselName = desvesselPM.EnglishName;
                                manifestSL.Transshipment2VesselCode = desvesselPM.Code;
                                manifestSL.Transshipment2VesselAddedManually = false;

                            }
                            else if (fieldName == "Transshipment3VesselId")
                            {
                                manifestSL.Transshipment3VesselId = desvesselPM.Id;
                                manifestSL.Transshipment3VesselName = desvesselPM.EnglishName;
                                manifestSL.Transshipment3VesselCode = desvesselPM.Code;
                                manifestSL.Transshipment3VesselAddedManually = false;

                            }
                        }

                        else
                        {
                            SetVesselWithOrginValue(fieldName, manifestSL, vesselPM);

                        }
                    }
                    else
                    {
                        SetVesselWithOrginValue(fieldName, manifestSL, vesselPM);
                    }
                }

            }
        }

        private static void SetVesselWithOrginValue(string fieldName, ManifestSL manifestSL, VesselPM vesselPM)
        {
            if(vesselPM!=null && manifestSL!=null)

            if (fieldName == "MainCarriageVesselId")
            {
                manifestSL.MainCarriageVesselName = vesselPM.EnglishName;
                manifestSL.MainCarriageVesselCode = vesselPM.Code;
            }
            else if (fieldName == "Transshipment1VesselId")
            {
                manifestSL.Transshipment1VesselName = vesselPM.EnglishName;
                manifestSL.Transshipment1VesselCode = vesselPM.Code;

            }
            else if (fieldName == "Transshipment2VesselId")
            {
                manifestSL.Transshipment2VesselName = vesselPM.EnglishName;
                manifestSL.Transshipment2VesselCode = vesselPM.Code;

            }

            else if (fieldName == "Transshipment3VesselId")
            {
                manifestSL.Transshipment3VesselName = vesselPM.EnglishName;
                manifestSL.Transshipment3VesselCode = vesselPM.Code;

            }
        }

        private void TransLateInterline(string interlineId, string fieldName, ManifestSL manifestSL, int tenant, int desTenant)
        {
            if (manifestSL != null && !string.IsNullOrEmpty(interlineId))
            {
                CardQuery cardQuery = new CardQuery(tenant);
                AirlineQuery airlineQuery = new AirlineQuery(0);
                bool addedManually = airlineQuery.CheckAirlinesAddedManually(interlineId, tenant);
                CardPM cardPM = null;
                if (!addedManually)
                {
                    cardPM = cardQuery.GetSinglePM(interlineId, tenant);
                    if (cardPM != null)
                    {
                        CardRepository cardRepository = new CardRepository(desTenant);
                        Card desCard = cardRepository.GetSingleCardByCode(cardPM.Code, desTenant, true);
                        if (desCard != null)
                        {
                            if (fieldName == "InterlineId")
                            {

                                manifestSL.InterlineId = desCard.Id;
                                manifestSL.InterlineName = desCard.EnglishName;
                                manifestSL.InterlineCode = desCard.Code;
                                manifestSL.InterlineAddedManually = false;
                            }
                        }
                        else
                        {
                            manifestSL.InterlineName = cardPM.EnglishName;
                            manifestSL.InterlineCode = cardPM.Code;
                            
                        }

                    }


                }
                else
                {
                    cardPM = cardQuery.GetSinglePM(interlineId, tenant);
                    if (cardPM != null)
                    {
                        if (fieldName == "InterlineId")
                        {
                            manifestSL.InterlineName = cardPM.EnglishName;
                            manifestSL.InterlineCode = cardPM.Code;
                        }
        
                    }

                }

            }
        }

        private ShipmentPickUpDeliverySL BuildShipmentPickUpDeliverySL(ShipmentPickUpPM shipmentPickUpPM , ShipmentDeliveryPM shipmentDeliveryPM)
        {
            int tenant = shipmentPickUpPM!=null ? shipmentPickUpPM.Tenant: shipmentDeliveryPM.Tenant;
            CardRepository cardRepository = new CardRepository(tenant);
            ShipmentPickUpDeliverySL ShipmentPickUpDeliverySL = new ShipmentPickUpDeliverySL();

            ShipmentPickUpDeliverySL.PickUpDeliveryTypeCode = shipmentPickUpPM != null ? "PICK" : "DELV";
            ShipmentPickUpDeliverySL.MainCarriageATA = shipmentPickUpPM != null ? shipmentPickUpPM.ATA : shipmentDeliveryPM.ATA;
            ShipmentPickUpDeliverySL.MainCarriageATD = shipmentPickUpPM != null ? shipmentPickUpPM.ATD : shipmentDeliveryPM.ATD;
            ShipmentPickUpDeliverySL.MainCarriageETA = shipmentPickUpPM != null ? shipmentPickUpPM.ETA : shipmentDeliveryPM.ETA;
            ShipmentPickUpDeliverySL.MainCarriageETD = shipmentPickUpPM != null ? shipmentPickUpPM.ETD : shipmentDeliveryPM.ETD;
            ShipmentPickUpDeliverySL.TransportModeCode = shipmentPickUpPM != null ? shipmentPickUpPM.TransportModeCode : shipmentDeliveryPM.TransportModeCode;
            ShipmentPickUpDeliverySL.TransportModeName = shipmentPickUpPM != null ? shipmentPickUpPM.TransportModeName : shipmentDeliveryPM.TransportModeName;

            #region From
            string pickUpDeliveryFromTypeCode = shipmentPickUpPM != null ? shipmentPickUpPM.PickUpDeliveryFromTypeCode : shipmentDeliveryPM.PickUpDeliveryFromTypeCode;
            ShipmentPickUpDeliverySL.PickUpDeliveryFromTypeCode = pickUpDeliveryFromTypeCode;


            if (pickUpDeliveryFromTypeCode == "PORT")
            {
                string fromPortCode = shipmentPickUpPM!=null ? shipmentPickUpPM.FromPortCode: shipmentDeliveryPM.FromPortCode;
                string fromPortName = shipmentPickUpPM != null ? shipmentPickUpPM.FromPortName : shipmentDeliveryPM.FromPortName;
                string fromPortCountryCode = shipmentPickUpPM != null ? shipmentPickUpPM.FromPortCountryCode : shipmentDeliveryPM.FromPortCountryCode;
                PortSL fromPort = new PortSL()
                {
                    Code = fromPortCode,
                    EnglishName = fromPortName,
                    CountryCode = fromPortCountryCode,
                };
                ShipmentPickUpDeliverySL.FromPort = fromPort;
            }
            else if (pickUpDeliveryFromTypeCode == "PART")
            {
                string fromPartnerCardId = shipmentPickUpPM != null ? shipmentPickUpPM.FromPartnerCardId : shipmentDeliveryPM.FromPartnerCardId; 

                Card fromPartnerCard = fromPartnerCard = cardRepository.GetSingleCard(fromPartnerCardId, tenant);
                if (fromPartnerCard != null)
                {
                    PartnerSL fromPartner = new PartnerSL()
                    {
                        Id = fromPartnerCardId,
                        Code = fromPartnerCard != null ? fromPartnerCard.Code : "",
                        EnglishName = fromPartnerCard.EnglishName,
                        Address1 = fromPartnerCard.Address1,
                        Address2 = fromPartnerCard.Address2,
                        City = fromPartnerCard.CityName,
                        CountryCode = fromPartnerCard.CountryCode,
                        CountryName = fromPartnerCard.CountryName,

                    };

                    ShipmentPickUpDeliverySL.FromPartner = fromPartner;
                }
            }
            else if (pickUpDeliveryFromTypeCode == "CASL")
            {
                string fromAddressZipCode = shipmentPickUpPM != null ? shipmentPickUpPM.FromAddressZipCode : shipmentDeliveryPM.FromAddressZipCode; 
                string fromAddressCountryCode = shipmentPickUpPM != null ? shipmentPickUpPM.FromAddressCountryCode : shipmentDeliveryPM.FromAddressCountryCode;
                string fromAddressCountryName = shipmentPickUpPM != null ? shipmentPickUpPM.FromAddressCountryName : shipmentDeliveryPM.FromAddressCountryName;
                string fromAddressCity = shipmentPickUpPM != null ? shipmentPickUpPM.FromAddressCity : shipmentDeliveryPM.FromAddressCity; 

                ShipmentPickUpDeliverySL.FromAddressZipCode = fromAddressZipCode;
                ShipmentPickUpDeliverySL.FromAddressCountryCode = fromAddressCountryCode;
                ShipmentPickUpDeliverySL.FromAddressCountryName = fromAddressCountryName;
                ShipmentPickUpDeliverySL.FromAddressCity = fromAddressCity;
            
            }

            #endregion

            #region To
            string pickUpDeliveryToTypeCode = shipmentPickUpPM != null ? shipmentPickUpPM.PickUpDeliveryToTypeCode : shipmentDeliveryPM.PickUpDeliveryToTypeCode;
            ShipmentPickUpDeliverySL.PickUpDeliveryToTypeCode = pickUpDeliveryToTypeCode;

            if (pickUpDeliveryToTypeCode == "PORT")
            {
                string ToPortCode = shipmentPickUpPM != null ? shipmentPickUpPM.ToPortCode : shipmentDeliveryPM.ToPortCode;
                string ToPortName = shipmentPickUpPM != null ? shipmentPickUpPM.ToPortName : shipmentDeliveryPM.ToPortName;
                string ToPortCountryCode = shipmentPickUpPM != null ? shipmentPickUpPM.ToPortCountryCode : shipmentDeliveryPM.ToPortCountryCode;
                PortSL ToPort = new PortSL()
                {
                    Code = ToPortCode,
                    EnglishName = ToPortName,
                    CountryCode = ToPortCountryCode,
                };
                ShipmentPickUpDeliverySL.ToPort = ToPort;
            }
            else if (pickUpDeliveryToTypeCode == "PART")
            {
                string ToPartnerCardId = shipmentPickUpPM != null ? shipmentPickUpPM.ToPartnerCardId : shipmentDeliveryPM.ToPartnerCardId;

                Card ToPartnerCard = ToPartnerCard = cardRepository.GetSingleCard(ToPartnerCardId, tenant);
                if (ToPartnerCard != null)
                {
                    PartnerSL ToPartner = new PartnerSL()
                    {
                        Id = ToPartnerCardId,
                        Code = ToPartnerCard != null ? ToPartnerCard.Code : "",
                        EnglishName = ToPartnerCard.EnglishName,
                        Address1 = ToPartnerCard.Address1,
                        Address2 = ToPartnerCard.Address2,
                        City = ToPartnerCard.CityName,
                        CountryCode = ToPartnerCard.CountryCode,
                        CountryName = ToPartnerCard.CountryName,

                    };

                    ShipmentPickUpDeliverySL.ToPartner = ToPartner;
                }
            }
            else if (pickUpDeliveryToTypeCode == "CASL")
            {
                string ToAddressZipCode = shipmentPickUpPM != null ? shipmentPickUpPM.ToAddressZipCode : shipmentDeliveryPM.ToAddressZipCode;
                string ToAddressCountryCode = shipmentPickUpPM != null ? shipmentPickUpPM.ToAddressCountryCode : shipmentDeliveryPM.ToAddressCountryCode;
                string ToAddressCountryName = shipmentPickUpPM != null ? shipmentPickUpPM.ToAddressCountryName : shipmentDeliveryPM.ToAddressCountryName;
                string ToAddressCity = shipmentPickUpPM != null ? shipmentPickUpPM.ToAddressCity : shipmentDeliveryPM.ToAddressCity;

                ShipmentPickUpDeliverySL.ToAddressZipCode = ToAddressZipCode;
                ShipmentPickUpDeliverySL.ToAddressCountryCode = ToAddressCountryCode;
                ShipmentPickUpDeliverySL.ToAddressCountryName = ToAddressCountryName;
                ShipmentPickUpDeliverySL.ToAddressCity = ToAddressCity;
            }

            #endregion

            return ShipmentPickUpDeliverySL;
        }

        private static List<ShipmentPackagePM> ReBulidShipmentPackages(int tenant, List<ShipmentPackagePM> shipmentPackages)
        {
            PackageTypeRepository packageTypeRepository = new PackageTypeRepository(tenant);
            foreach (ShipmentPackagePM item in shipmentPackages)
            {
                item.MethodUsed = "";
                item.VGM = null;
  
                if (!string.IsNullOrEmpty(item.PackageTypeId))
                {
                    if (!item.IsPackageAddedManually)
                    {
                        PackageType packagetype = packageTypeRepository.GetSinglePackageTypeByCode(item.PackageTypeCode, tenant, true);
                        if (packagetype != null)
                        {
                            item.PackageTypeId = packagetype.Id;
                        }
                        else item.IsPackageAddedManually = true;
                    }
                    else item.IsPackageAddedManually = true;
                }
                else item.IsPackageAddedManually = false;


                if (item.InsideShipmentPackages != null)
                {
                    foreach (InsideShipmentPackagePM insideShipmentPackage in item.InsideShipmentPackages)
                    {
                        if (!string.IsNullOrEmpty(insideShipmentPackage.PackageTypeId))
                        {
                            item.MethodUsed = "";
                            item.VGM = null;
                            if (!insideShipmentPackage.IsPackageAddedManually)
                            {
                                PackageType packagetype = packageTypeRepository.GetSinglePackageTypeByCode(insideShipmentPackage.PackageTypeCode, tenant, true);
                                if (packagetype != null)
                                {
                                    insideShipmentPackage.PackageTypeId = packagetype.Id;
                                }
                                else insideShipmentPackage.IsPackageAddedManually = true;
                            }
                            else insideShipmentPackage.IsPackageAddedManually = true;
                        }

                    }
                }

            }

            return shipmentPackages;
        }



        public HttpResponseMessage GetCheckIfMasterShipmentHaveHouseWithOtherAgent(string entityId, string agentId, int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
                ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);

                List<ShipmentList> shipmentLists = shipmentQuery.GetShipmentListsByMasterIdAndTenant(entityId, tenant);
                bool isMasterShipmentHaveHouseWithOtherAgent = false;
                foreach (ShipmentList shipmentList in shipmentLists)
                {
                    if(!String.IsNullOrEmpty(shipmentList.AgentId) && shipmentList.AgentId != agentId)
                    {
                        isMasterShipmentHaveHouseWithOtherAgent = true;
                        break;
                    }
                }


                return Request.CreateResponse(HttpStatusCode.OK, isMasterShipmentHaveHouseWithOtherAgent);

            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }



        public HttpResponseMessage GetAgentSharedManifestsForDashBoard(int lastMonths, int lastDays  ,int selectedIndex)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                AgentSharedManifestQuery agentSharedManifestQuery = new AgentSharedManifestQuery(authToken.Tenant);
                List<SharedManifestsStatusClass> result = agentSharedManifestQuery.GetAgentSharedManifestsForDashBoard(lastMonths, lastDays, selectedIndex ,  authToken.Tenant);
                return Request.CreateResponse(HttpStatusCode.OK, result);

            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetIsAgentSharedManifests(string agentId , string entityid)
        {
            try
            {

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                bool result = false;

                AgentSharedManifestHelper agentSharedManifestHelper = new AgentSharedManifestHelper();
                List<ManifestSL> manifestSLLists = agentSharedManifestHelper.GetAgentShareManifestSLByEntityId(entityid, authToken.Tenant);

                if (manifestSLLists.Count> 0)
                {
                    var manifestSL = manifestSLLists.Where(d => d.OrginalAgentId == agentId).FirstOrDefault();
                    if (manifestSL != null)
                    {
                        result = true;
                    }
                }

                if (!result)
                {
                    foreach (ManifestSL manifestSL in manifestSLLists.Where(d => string.IsNullOrEmpty(d.OrginalAgentId)).ToList())
                    {
                        if (manifestSL != null)
                        {
                            AgentRepository agentRepository = new AgentRepository(authToken.Tenant);
                            Agent agent = agentRepository.GetSingleAgentBySharedKey(manifestSL.AgentSharedKey, authToken.Tenant);
                            if (agent != null)
                            {
                                if (agent.Id == agentId)
                                {
                                    result = true;
                                    return Request.CreateResponse(HttpStatusCode.OK, result);
                                }
                            }

                        }
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, result);

            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }





}