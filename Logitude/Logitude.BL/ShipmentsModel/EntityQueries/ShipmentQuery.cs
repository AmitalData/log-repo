using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.Data.Repositories;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Transactions;
using System.Xml;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ShipmentQuery : ShipmentCloudCustomDataDeserializer
    {

        ShipmentRepository repository;

        public ShipmentQuery(int tenant)
        {
            repository = new ShipmentRepository(tenant);
        }

        public ShipmentQuery(ShipmentRepository repository)
        {
            this.repository = repository;
        }
        
        public ShipmentPM GetSinglePMByShipmentNumber(string shipmentNumber, int tenant, bool withComposition = true)
        {
            if (!string.IsNullOrEmpty(shipmentNumber))
            {

                Shipment shipment = (from a in repository.context.Shipments.Include("EntityStatus").Include("ComputedEntityStatus").Include("ShipmentType").Include("Incoterm").Include("ShipmentReceivableStatus").Include("ShipmentPayableStatus").Include("ShipmentLevel").Include("NextLeg").Include("ShipmentType").Include("ShipmentMasterData").Include("SpecialServicesType").Include("MoveType")
                                     where a.ShipmentNumber == shipmentNumber && a.Tenant == tenant
                                     select a).FirstOrDefault();
                if (shipment != null)
                {
                    ShipmentMasterData masterData = (from a in repository.context.ShipmentMasterDatas
                                                     where a.Id == shipment.MasterShipmentDataId
                                                     select a).FirstOrDefault();

                    ShipmentPM shipmentPM = new ShipmentPM();

                    shipmentPM = MapShipmentToShipmentPM(shipmentPM, shipment, null, masterData, withComposition);
                    //shipmentPM.ToCountryCode = !string.IsNullOrEmpty(shipmentPM.MainCarriageFinalDestinationPortCountryCode) ? shipmentPM.MainCarriageFinalDestinationPortCountryCode : shipmentPM.ToPortCountryCode,
                    //shipmentPM.FromCountryCode = f.ShipmentLevelCode == "H" && string.IsNullOrEmpty(f.MasterShipmentDataId) ? f.FromPortCountryCode : f.MainCarriageFromPortCountryCode,
                    ShipmentPM securedPM = new ShipmentPM();
                    securedPM = SecuredMapping.GetMappedPM(shipmentPM, securedPM, "Shipment", tenant);

                    ShipmentPM returnShipment = BranchPermitionsFilter.AddUserBranchRestrictionFilters(new QueryOperations(), securedPM, tenant);//securedPM;
                    returnShipment = ProductPermitionsFilter.AddUserProductRestrictionFilters(new QueryOperations(), securedPM, tenant);
                    var CLoudData = (from a in repository.context.ShipmentAdditionalCloudDatas
                                     where a.Id == shipment.Id
                                     select a).FirstOrDefault();
                    if (CLoudData != null)
                    {
                        returnShipment.DeclarationXMLData = CLoudData.DeclarationXmlData;
                        returnShipment.DeclarationWCOXml = CLoudData.DeclarationWCOXml;
                        returnShipment.ApproveDateTime = CLoudData.ApproveDateTime;
                        returnShipment.IsImporterApprovalRequired = CLoudData.IsImporterApprovalRequried;
                        returnShipment.VersionApproved = CLoudData.VersionApproved;
                        returnShipment.ShipmentAddtionalDataXML = CLoudData.ShipmentAddtionalDataXML;
                        returnShipment.SendUpdatesToAgentEnabled = CLoudData.SendUpdatesToAgentEnabled;
                        returnShipment.DocsSentToAgent = CLoudData.DocsSentToAgent;
                        returnShipment.ApprovedBy = CLoudData.ApprovedByUserName;
                        returnShipment.DocumentsApprovedByUserName = CLoudData.DocumentsApprovedByUserName;
                        returnShipment.IsUserIDNumberRequired = CLoudData.IsUserIDNumberRequired;
                        returnShipment.UserIdNumberUpdateDate = CLoudData.UserIdNumberUpdateDate;
                        returnShipment.UserIdNumberXMLData = CLoudData.UserIdNumberXMLData;
                        returnShipment.UserIdNumber = CLoudData.UserIdNumber;
                        returnShipment.PaymentRequestXML = CLoudData.PaymentRequestXML;
                        returnShipment.PaymentDateTime = CLoudData.PaymentDateTime;
                        returnShipment.IsPaymentRequired = CLoudData.IsPaymentRequired;
                    }
                    return returnShipment;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        public ShipmentPM MapShipmentToShipmentPM(ShipmentPM shipmentPM, Shipment shipment, IQueryable<ShipmentMasterData> shipmentMasterDataList, ShipmentMasterData masterData, bool withComposition)
        {
            int tenant = shipment.Tenant;
            ICommonDataContext myCommonContext = CommonDataContext.GetContext(shipment.Tenant);
            PortRepository portsRep = new PortRepository(myCommonContext);
            VesselRepository vesselRep = new VesselRepository(myCommonContext);
            AddressRepository addressRepository = new AddressRepository(myCommonContext);

            IWebFreightContext webFreightContext = WebFreightContext.GetContext(shipment.Tenant);

            TransportModeRepository transmodeRep = new TransportModeRepository(webFreightContext);

            ShipmentLevelRepository shipmentLevelRep = new ShipmentLevelRepository(repository.context);
            ShipmentPickUpDeliveryRepository shipmentPickUpDeliveryRepository = new ShipmentPickUpDeliveryRepository(repository.context);
            ShipmentDeliveryQuery shipmentDeliveryQuery = new ShipmentDeliveryQuery(shipmentPickUpDeliveryRepository);
            ShipmentPickUpQuery shipmentPickUpQuery = new ShipmentPickUpQuery(shipmentPickUpDeliveryRepository);
            DirectionRepository directionRep = new DirectionRepository(webFreightContext);

            Currency awbCurrency = CurrencyRepository.GetSingleCurrency(shipment.AWBCurrencyId, shipment.Tenant, true);
            ShipmentLevel shipmentLevel = shipmentLevelRep.GetSingleShipmentLevel(shipment.ShipmentLevelCode);
            TransportMode transportmode = transmodeRep.GetSingleTransportMode(shipment.TransportModeId);
            Direction direction = directionRep.GetSingleDirection(shipment.DirectionId);

            PortQuery portQuery = new PortQuery(portsRep);
            
            bool isInlandDomesticShipment = (shipment.DirectionId == "D" && shipment.TransportModeId == "I");

            if (masterData == null)
            {
                if (shipment.MasterShipmentDataId != null && shipmentMasterDataList != null)
                {
                    masterData = (from a in shipmentMasterDataList where a.Id == shipment.MasterShipmentDataId select a).FirstOrDefault();
                }
            }

            shipmentPM.BookingId = shipment.BookingId;
            if (!string.IsNullOrEmpty(shipment.BookingId))
            {
                BookingRepository bookingRepository = new BookingRepository(shipment.Tenant);
                Booking connectedBooking = bookingRepository.GetSingle(shipment.BookingId, shipment.Tenant);
                if (connectedBooking != null)
                {
                    shipmentPM.BookingNumber = connectedBooking.BookingNumber;
                }
            }

            shipmentPM.ShipmentSubTypeId = shipment.ShipmentSubTypeId;
            if (!string.IsNullOrEmpty(shipment.ShipmentSubTypeId))
            {
                ShipmentSubTypeRepository subTypeRepository = new ShipmentSubTypeRepository(shipment.Tenant);
                ShipmentSubType subType = subTypeRepository.GetSingleShipmentSubType(shipment.ShipmentSubTypeId, shipment.Tenant);
                if (subType != null)
                {
                    shipmentPM.ShipmentSubTypeName = subType.Name;
                }
            }

            #region if (masterData != null)
            if (masterData != null)
            {
                shipmentPM.Master = masterData.Master;
                shipmentPM.AirlinePrefix = masterData.AirlinePrefix;
                shipmentPM.LongMaster = EntityFieldsHelper.GetLongMasterField(shipment, masterData);
                shipmentPM.ManifestReason = masterData.ManifestReason;
                shipmentPM.ManifestStatusCode = masterData.ManifestStatusCode;
                shipmentPM.ProrateReceivables = masterData.ProrateReceivables;

                #region Carrier

                shipmentPM.MainCarriageCarrierId = masterData.MainCarriageCarrierId;

                if (!string.IsNullOrEmpty(masterData.MainCarriageCarrierId))
                {
                    Card cardObject = CardRepository.GetSingleCard(masterData.MainCarriageCarrierId, shipment.Tenant, true);

                    if (cardObject != null)
                    {
                        shipmentPM.MainCarriageCarrierCode = cardObject.Code;
                        shipmentPM.MainCarriageCarrierName = cardObject.EnglishName;
                        shipmentPM.MainCarriageCarrierWebSite = cardObject.Website;

                        Address address = addressRepository.GetSingleAddressByCardIdAndTypeId(cardObject.Id, "M", tenant);
                        if (address != null)
                        {
                            shipmentPM.MainCarriageCarrierAddressId = address.Id;
                        }
                    }

                    if (shipment.TransportModeId == "A")
                    {
                        AirlineRepository airlineRepository = new AirlineRepository(myCommonContext);

                        if (!string.IsNullOrEmpty(masterData.MainCarriageCarrierId))
                        {
                            Airline airline = airlineRepository.GetSingleAirline(masterData.MainCarriageCarrierId, shipment.Tenant);
                            if (airline != null)
                            {
                                shipmentPM.CarrierIsChampRegistered = airline.IsChampRegistered;
                                shipmentPM.CarrierIsGLSHKRegistered = airline.IsGLSHKRegistered;
                                shipmentPM.CarrierIsCheckDigit = airline.CheckDigit;
                                shipmentPM.CarrierIsLimitedLength = airline.LimitedLength;

                                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                                {
                                    AirlineRepository tenantZeroAirlineRepository = new AirlineRepository(0);
                                    Airline tenantZeroAirline = tenantZeroAirlineRepository.GetSingleAirlineByCode(airline.Card.Code, 0);
                                    if (tenantZeroAirline != null)
                                    {
                                        shipmentPM.TenantZeroAirlineId = tenantZeroAirline.Id;
                                        shipmentPM.TenantZeroAirlineTTY = tenantZeroAirline.TTY;
                                        shipmentPM.TenantZeroAirlinePIMA = tenantZeroAirline.GLSHKPIMA;
                                        shipmentPM.TenantZeroAirlineChampFWB = tenantZeroAirline.ChampFWB;
                                        shipmentPM.TenantZeroAirlineChampFHL = tenantZeroAirline.ChampFHL;
                                        shipmentPM.TenantZeroAirlineChampFSU = tenantZeroAirline.ChampFSU;
                                        shipmentPM.TenantZeroAirlineChampFSRFSA = tenantZeroAirline.ChampFSRFSA;
                                        shipmentPM.TenantZeroAirlineChampFVRFVA = tenantZeroAirline.ChampFVRFVA;
                                        shipmentPM.TenantZeroAirlineChampNeedsRegistration = tenantZeroAirline.ChampNeedsRegistration;
                                        shipmentPM.TenantZeroAirlineGLSHKFWB = tenantZeroAirline.GLSHKFWB;
                                        shipmentPM.TenantZeroAirlineGLSHKFHL = tenantZeroAirline.GLSHKFHL;
                                        shipmentPM.TenantZeroAirlineGLSHKFSU = tenantZeroAirline.GLSHKFSU;
                                        shipmentPM.TenantZeroAirlineGLSHKFSRFSA = tenantZeroAirline.GLSHKFSRFSA;
                                        shipmentPM.TenantZeroAirlineGLSHKFVRFVA = tenantZeroAirline.GLSHKFVRFVA;
                                        shipmentPM.TenantZeroAirlineGLSHKNeedsRegistration = tenantZeroAirline.GLSHKNeedsRegistration;
                                    }

                                    scope.Complete();
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(masterData.InterlineId))
                        {
                            Airline airline = airlineRepository.GetSingleAirline(masterData.InterlineId, shipment.Tenant);
                            if (airline != null)
                            {
                                shipmentPM.CarrierIsCheckDigit = airline.CheckDigit;
                                shipmentPM.CarrierIsLimitedLength = airline.LimitedLength;
                            }
                        }
                    }
                }
                #endregion

                shipmentPM.ImportManifest = masterData.ImportManifest;
                shipmentPM.CarrierTransportDocumentNumber = masterData.CarrierTransportDocumentNumber;
                shipmentPM.MAWBOBLDate = masterData.MAWBOBLDate;
                shipmentPM.CutoffDate = masterData.CutoffDate;
                shipmentPM.BookingConfirmationNumber = masterData.BookingConfirmationNumber;
                shipmentPM.BookingConfirmationNotes = masterData.BookingConfirmationNotes;
                shipmentPM.BookingConfirmedBy = masterData.BookingConfirmedBy;
                shipmentPM.MainCarriageIsFromStack = masterData.MainCarriageIsFromStack;
                shipmentPM.MasterShipmentNumber = masterData.MasterShipmentNumber;
                shipmentPM.MainCarriageCarrierNumber = masterData.MainCarriageCarrierNumber;
                shipmentPM.MainCarriageETD = masterData.MainCarriageETD;
                shipmentPM.MainCarriageETA = masterData.MainCarriageETA;
                shipmentPM.MainCarriageATD = masterData.MainCarriageATD;
                shipmentPM.MainCarriageATA = masterData.MainCarriageATA;
                shipmentPM.MainCarriageFullCarrierNumber = masterData.MainCarriageCarrierPrefix + masterData.MainCarriageCarrierNumber;
                shipmentPM.Transshipment1FullCarrierNumber = masterData.Transshipment1CarrierPrefix + masterData.Transshipment1CarrierNumber;
                shipmentPM.Transshipment2FullCarrierNumber = masterData.Transshipment2CarrierPrefix + masterData.Transshipment2CarrierNumber;
                shipmentPM.Transshipment3FullCarrierNumber = masterData.Transshipment3CarrierPrefix + masterData.Transshipment3CarrierNumber;
                shipmentPM.MainCarriageSTA = masterData.MainCarriageSTA;
                shipmentPM.MainCarriageSTD = masterData.MainCarriageSTD;
                shipmentPM.Transshipment1STA = masterData.Transshipment1STA;
                shipmentPM.Transshipment1STD = masterData.Transshipment1STD;
                shipmentPM.Transshipment2STA = masterData.Transshipment2STA;
                shipmentPM.Transshipment2STD = masterData.Transshipment2STD;
                shipmentPM.Transshipment3STA = masterData.Transshipment3STA;
                shipmentPM.Transshipment3STD = masterData.Transshipment3STD;
                shipmentPM.IsKnownCargo = masterData.IsKnownCargo;
                shipmentPM.RegulatedAgentRANumber = masterData.RegulatedAgentRANumber;
                shipmentPM.KnownConsignorNumber = masterData.KnownConsignorNumber;
                shipmentPM.KCExpirationDate = masterData.KCExpirationDate;
                shipmentPM.ColoaderRANumber = masterData.ColoaderRANumber;
                shipmentPM.AWBPrintingSecurityStatusId = masterData.AWBPrintingSecurityStatusId;
                shipmentPM.AWBPrintingRANumber = masterData.AWBPrintingRANumber;
                shipmentPM.AdditionalHandlingInfo = masterData.AdditionalHandlingInfo;
                shipmentPM.AWBPrintingSecurityStatusEdited = masterData.AWBPrintingSecurityStatusEdited;
                shipmentPM.AWBPrintingRANumberEdited = masterData.AWBPrintingRANumberEdited;
                shipmentPM.AdditionalHandlingInfoEdited = masterData.AdditionalHandlingInfoEdited;
                shipmentPM.InterlineId = masterData.InterlineId;
                shipmentPM.DepartureArrivalFromDate = masterData.DepartureArrivalFromDate;
                shipmentPM.DepartureArrivalToDate = masterData.DepartureArrivalToDate;
                shipmentPM.MainCarriageFinalDestinationETA = masterData.MainCarriageFinalDestinationETA;
                shipmentPM.MainCarriageFinalDestinationATA = masterData.MainCarriageFinalDestinationATA;
                shipmentPM.OBLTypeCode = masterData.OBLTypeCode;
                shipmentPM.DocumentsClosingDate = masterData.DocumentsClosingDate;
                shipmentPM.FWBStatusCode = masterData.FWBStatusCode;
                shipmentPM.FWBStatusDate = masterData.FWBStatusDate;

                if (!string.IsNullOrEmpty(masterData.FWBStatusCode))
                {
                    FWBStatusRepository myRepository = new FWBStatusRepository(tenant);
                    FWBStatus myStatus = myRepository.GetSingleFWBStatus(masterData.FWBStatusCode);
                    if (myStatus != null)
                    {
                        shipmentPM.FWBStatusName = myStatus.Name;
                    }
                }

                shipmentPM.CargonautFWBStatusCode = masterData.CargonautFWBStatusCode;
                shipmentPM.CargonautFWBStatusDate = masterData.CargonautFWBStatusDate;
                if (!string.IsNullOrEmpty(masterData.CargonautFWBStatusCode))
                {
                    FWBStatusRepository myRepository = new FWBStatusRepository(tenant);
                    FWBStatus myStatus = myRepository.GetSingleFWBStatus(masterData.CargonautFWBStatusCode);
                    if (myStatus != null)
                    {
                        shipmentPM.CargonautFWBStatusName = myStatus.Name;
                    }
                }

                if (shipment.TransportModeId == "I")
                {
                    shipmentPM.TrailerNumber = masterData.TrailerNumber;
                }

                if (isInlandDomesticShipment)
                {
                    shipmentPM.MainCarriageFromPartnerId = masterData.MainCarriageFromPartnerId;
                    shipmentPM.MainCarriageFromAddressId = masterData.MainCarriageFromAddressId;
                    shipmentPM.MainCarriageToPartnerId = masterData.MainCarriageToPartnerId;
                    shipmentPM.MainCarriageToAddressId = masterData.MainCarriageToAddressId;
                    shipmentPM.Driver = masterData.Driver;
                    shipmentPM.TruckNumber = masterData.TruckNumber;

                    if (!string.IsNullOrEmpty(shipmentPM.MainCarriageFromAddressId))
                    {
                        Address fromAddress = addressRepository.GetSingleAddress(shipmentPM.MainCarriageFromAddressId, tenant);
                        if (fromAddress != null)
                        {
                            shipmentPM.FromCountryId = fromAddress.CountryId;
                            shipmentPM.FromPartnerCity = fromAddress.City;
                            shipmentPM.FromLocation = fromAddress.City;
                            if (fromAddress.Country != null)
                            {
                                shipmentPM.FromCountryIsEC = fromAddress.Country.EC;
                                shipmentPM.FromPartnerCountryCode = fromAddress.Country.Code;
                                shipmentPM.FromPartnerCountryName = fromAddress.Country.EnglishName;
                                shipmentPM.FromLocation += " " + fromAddress.Country.Code;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(shipmentPM.MainCarriageToAddressId))
                    {
                        Address toAddress = addressRepository.GetSingleAddress(shipmentPM.MainCarriageToAddressId, tenant);
                        if (toAddress != null)
                        {
                            shipmentPM.ToCountryId = toAddress.CountryId;
                            shipmentPM.ToPartnerCity = toAddress.City;
                            shipmentPM.ToLocation = toAddress.City;
                            if (toAddress.Country != null)
                            {
                                shipmentPM.ToCountryIsEC = toAddress.Country.EC;
                                shipmentPM.ToPartnerCountryCode = toAddress.Country.Code;
                                shipmentPM.ToPartnerCountryName = toAddress.Country.EnglishName;
                                shipmentPM.ToLocation += " " + toAddress.Country.Code;
                            }
                        }
                    }
                }

                if (!isInlandDomesticShipment)
                {
                    shipmentPM.FinalDistenationPortId = masterData.MainCarriageToPortId;
                    shipmentPM.MainCarriageFinalDestinationPortId = masterData.MainCarriageFinalDestinationPortId;
                    shipmentPM.MainCarriageFromPortId = masterData.MainCarriageFromPortId;
                    shipmentPM.OriginMainCarriageFromPortId = masterData.MainCarriageFromPortId;
                    shipmentPM.OriginFinalDestinationPortId = masterData.MainCarriageFinalDestinationPortId;
                    shipmentPM.MainCarriageToPortId = masterData.MainCarriageToPortId;

                    PortPM mainCarriageFromPort = portQuery.GetSinglePM(masterData.MainCarriageFromPortId, masterData.Tenant);
                    if (mainCarriageFromPort != null)
                    {
                        shipmentPM.FromCountryId = mainCarriageFromPort.CountryId;
                        shipmentPM.FromCountryIsEC = mainCarriageFromPort.CountryEC;
                        shipmentPM.FromPort = mainCarriageFromPort.Code;
                        shipmentPM.FromPortCountry = mainCarriageFromPort.CountryName;
                        shipmentPM.FromPortName = mainCarriageFromPort.EnglishName;
                        shipmentPM.MainCarriageFromPortCode = mainCarriageFromPort.Code;
                        shipmentPM.MainCarriageFromPortName = mainCarriageFromPort.EnglishName;
                        shipmentPM.MainCarriageFromPortCountryCode = mainCarriageFromPort.CountryCode;
                        shipmentPM.MainCarriageFromPortCountryName = mainCarriageFromPort.CountryName;
                        shipmentPM.FromCountryCode = mainCarriageFromPort.CountryCode;
                        shipmentPM.FromLocation = mainCarriageFromPort.Code + " " + mainCarriageFromPort.EnglishName;
                    }

                    PortPM mainCarriageToPort = portQuery.GetSinglePM(masterData.MainCarriageToPortId, masterData.Tenant);
                    if (mainCarriageToPort != null)
                    {
                        shipmentPM.ToCountryId = mainCarriageToPort.CountryId;
                        shipmentPM.ToCountryIsEC = mainCarriageToPort.CountryEC;
                        shipmentPM.ToPort = mainCarriageToPort.Code;
                        shipmentPM.ToPortName = mainCarriageToPort.EnglishName;
                        shipmentPM.ToPortCountry = mainCarriageToPort.CountryName;
                        shipmentPM.ToCountryCode = mainCarriageToPort.CountryCode;
                        shipmentPM.MainCarriageToPortCountryName = mainCarriageToPort.CountryName;
                        shipmentPM.MainCarriageToPortCode = mainCarriageToPort.Code;
                        shipmentPM.MainCarriageToPortName = mainCarriageToPort.EnglishName;
                        shipmentPM.MainCarriageToPortCountryCode = mainCarriageToPort.CountryCode;

                        shipmentPM.ToLocation = mainCarriageToPort.Code + " " + mainCarriageToPort.EnglishName;
                    }

                    PortPM mainCarriageFinalDestinationPort = portQuery.GetSinglePM(masterData.MainCarriageFinalDestinationPortId, masterData.Tenant);
                    if (mainCarriageFinalDestinationPort != null)
                    {
                        shipmentPM.MainCarriageFinalDestinationPortCode = mainCarriageFinalDestinationPort.Code;
                        shipmentPM.MainCarriageFinalDestinationPortName = mainCarriageFinalDestinationPort.EnglishName;
                        shipmentPM.MainCarriageFinalDestinationPortCountryCode = mainCarriageFinalDestinationPort.CountryCode;
                        shipmentPM.MainCarriageFinalDestinationPortCountryName = mainCarriageFinalDestinationPort.CountryName;
                    }

                    shipmentPM.MainCarriageVesselId = masterData.MainCarriageVesselId;
                    shipmentPM.Transshipment1VesselId = masterData.Transshipment1VesselId;
                    shipmentPM.Transshipment2VesselId = masterData.Transshipment2VesselId;
                    shipmentPM.Transshipment3VesselId = masterData.Transshipment3VesselId;

                    if (!string.IsNullOrEmpty(shipmentPM.MainCarriageVesselId))
                    {
                        Vessel vesselEntity = vesselRep.GetSingleVessel(masterData.MainCarriageVesselId, masterData.Tenant);
                        if (vesselEntity != null)
                        {
                            shipmentPM.MainCarriageVesselName = vesselEntity.EnglishName;
                        }
                    }

                    if (!string.IsNullOrEmpty(shipmentPM.Transshipment1VesselId))
                    {
                        Vessel vesselEntity = vesselRep.GetSingleVessel(masterData.Transshipment1VesselId, masterData.Tenant);
                        if (vesselEntity != null)
                        {
                            shipmentPM.Transshipment1VesselName = vesselEntity.EnglishName;
                        }
                    }

                    if (!string.IsNullOrEmpty(shipmentPM.Transshipment2VesselId))
                    {
                        Vessel vesselEntity = vesselRep.GetSingleVessel(masterData.Transshipment2VesselId, masterData.Tenant);
                        if (vesselEntity != null)
                        {
                            shipmentPM.Transshipment2VesselName = vesselEntity.EnglishName;
                        }
                    }

                    if (!string.IsNullOrEmpty(shipmentPM.Transshipment3VesselId))
                    {
                        Vessel vesselEntity = vesselRep.GetSingleVessel(masterData.Transshipment3VesselId, masterData.Tenant);
                        if (vesselEntity != null)
                        {
                            shipmentPM.Transshipment3VesselName = vesselEntity.EnglishName;
                        }
                    }

                    #region Transshipment 1
                    shipmentPM.Transshipment1ATA = masterData.Transshipment1ATA;
                    shipmentPM.Transshipment1ATD = masterData.Transshipment1ATD;
                    shipmentPM.Transshipment1ETA = masterData.Transshipment1ETA;
                    shipmentPM.Transshipment1ETD = masterData.Transshipment1ETD;

                    shipmentPM.Transshipment1AdditionalMAWBOBLBL = masterData.Transshipment1AdditionalMAWBOBLBL;
                    shipmentPM.Transshipment1FromPortId = masterData.Transshipment1FromPortId;
                    shipmentPM.Transshipment1ToPortId = masterData.Transshipment1ToPortId;
                    shipmentPM.Transshipment1CarrierId = masterData.Transshipment1CarrierId;
                    shipmentPM.Transshipment1CarrierNumber = masterData.Transshipment1CarrierNumber;

                    if (!string.IsNullOrEmpty(shipmentPM.Transshipment1FromPortId))
                    {
                        PortPM transshipment1FromPort = portQuery.GetSinglePM(masterData.Transshipment1FromPortId, masterData.Tenant);
                        if (transshipment1FromPort != null)
                        {
                            shipmentPM.Transshipment1FromPortCode = transshipment1FromPort.Code;
                            shipmentPM.Transshipment1FromPortName = transshipment1FromPort.EnglishName;
                            shipmentPM.Transshipment1FromPortCountryCode = transshipment1FromPort.CountryCode;
                            shipmentPM.Transshipment1FromPortCountryName = transshipment1FromPort.CountryName;
                        }
                    }

                    if (!string.IsNullOrEmpty(shipmentPM.Transshipment1ToPortId))
                    {
                        PortPM transshipment1ToPort = portQuery.GetSinglePM(masterData.Transshipment1ToPortId, masterData.Tenant);
                        if (transshipment1ToPort != null)
                        {
                            shipmentPM.Transshipment1ToPortCode = transshipment1ToPort.Code;
                            shipmentPM.Transshipment1ToPortName = transshipment1ToPort.EnglishName;
                            shipmentPM.Transshipment1ToPortCountryCode = transshipment1ToPort.CountryCode;
                            shipmentPM.Transshipment1ToPortCountryName = transshipment1ToPort.CountryName;
                            shipmentPM.Transshipment1ToPortStateCode = transshipment1ToPort.StateCode;

                            shipmentPM.FinalDistenationPortId = masterData.Transshipment1ToPortId;
                        }
                    }

                    if (!string.IsNullOrEmpty(masterData.Transshipment1CarrierId))
                    {
                        Card cardObject = CardRepository.GetSingleCard(masterData.Transshipment1CarrierId, shipment.Tenant, true);
                        if (cardObject != null)
                        {
                            shipmentPM.Transshipment1CarrierCode = cardObject.Code;
                            shipmentPM.Transshipment1CarrierName = cardObject.EnglishName;
                            shipmentPM.Transshipment1CarrierWebSite = cardObject.Website;
                        }
                    }
                    #endregion

                    #region Transshipment 2
                    shipmentPM.Transshipment2ATA = masterData.Transshipment2ATA;
                    shipmentPM.Transshipment2ATD = masterData.Transshipment2ATD;
                    shipmentPM.Transshipment2ETA = masterData.Transshipment2ETA;
                    shipmentPM.Transshipment2ETD = masterData.Transshipment2ETD;

                    shipmentPM.Transshipment2AdditionalMAWBOBLBL = masterData.Transshipment2AdditionalMAWBOBLBL;
                    shipmentPM.Transshipment2FromPortId = masterData.Transshipment2FromPortId;
                    shipmentPM.Transshipment2ToPortId = masterData.Transshipment2ToPortId;
                    shipmentPM.Transshipment2CarrierId = masterData.Transshipment2CarrierId;
                    shipmentPM.Transshipment2CarrierNumber = masterData.Transshipment2CarrierNumber;

                    if (!string.IsNullOrEmpty(shipmentPM.Transshipment2FromPortId))
                    {
                        PortPM transshipment2FromPort = portQuery.GetSinglePM(masterData.Transshipment2FromPortId, masterData.Tenant);
                        if (transshipment2FromPort != null)
                        {
                            shipmentPM.Transshipment2FromPortCode = transshipment2FromPort.Code;
                            shipmentPM.Transshipment2FromPortName = transshipment2FromPort.EnglishName;
                            shipmentPM.Transshipment2FromPortCountryCode = transshipment2FromPort.CountryCode;
                            shipmentPM.Transshipment2FromPortCountryName = transshipment2FromPort.CountryName;
                        }
                    }

                    if (!string.IsNullOrEmpty(shipmentPM.Transshipment2ToPortId))
                    {
                        PortPM transshipment2ToPort = portQuery.GetSinglePM(masterData.Transshipment2ToPortId, masterData.Tenant);
                        if (transshipment2ToPort != null)
                        {
                            shipmentPM.Transshipment2ToPortCode = transshipment2ToPort.Code;
                            shipmentPM.Transshipment2ToPortName = transshipment2ToPort.EnglishName;
                            shipmentPM.Transshipment2ToPortCountryCode = transshipment2ToPort.CountryCode;
                            shipmentPM.Transshipment2ToPortCountryName = transshipment2ToPort.CountryName;
                            shipmentPM.Transshipment2ToPortStateCode = transshipment2ToPort.StateCode;

                            shipmentPM.FinalDistenationPortId = masterData.Transshipment2ToPortId;
                        }
                    }

                    if (!string.IsNullOrEmpty(masterData.Transshipment2CarrierId))
                    {
                        Card cardObject = CardRepository.GetSingleCard(masterData.Transshipment2CarrierId, shipment.Tenant, true);
                        if (cardObject != null)
                        {
                            shipmentPM.Transshipment2CarrierCode = cardObject.Code;
                            shipmentPM.Transshipment2CarrierName = cardObject.EnglishName;
                            shipmentPM.Transshipment2CarrierWebSite = cardObject.Website;
                        }
                    }
                    #endregion

                    #region Transshipment 3
                    shipmentPM.Transshipment3ATA = masterData.Transshipment3ATA;
                    shipmentPM.Transshipment3ATD = masterData.Transshipment3ATD;
                    shipmentPM.Transshipment3ETA = masterData.Transshipment3ETA;
                    shipmentPM.Transshipment3ETD = masterData.Transshipment3ETD;

                    shipmentPM.Transshipment3AdditionalMAWBOBLBL = masterData.Transshipment3AdditionalMAWBOBLBL;
                    shipmentPM.Transshipment3FromPortId = masterData.Transshipment3FromPortId;
                    shipmentPM.Transshipment3ToPortId = masterData.Transshipment3ToPortId;
                    shipmentPM.Transshipment3CarrierId = masterData.Transshipment3CarrierId;
                    shipmentPM.Transshipment3CarrierNumber = masterData.Transshipment3CarrierNumber;

                    if (!string.IsNullOrEmpty(shipmentPM.Transshipment3FromPortId))
                    {
                        PortPM transshipment3FromPort = portQuery.GetSinglePM(masterData.Transshipment3FromPortId, masterData.Tenant);
                        if (transshipment3FromPort != null)
                        {
                            shipmentPM.Transshipment3FromPortCode = transshipment3FromPort.Code;
                            shipmentPM.Transshipment3FromPortName = transshipment3FromPort.EnglishName;
                            shipmentPM.Transshipment3FromPortCountryCode = transshipment3FromPort.CountryCode;
                            shipmentPM.Transshipment3FromPortCountryName = transshipment3FromPort.CountryName;
                        }
                    }


                    if (!string.IsNullOrEmpty(shipmentPM.Transshipment3ToPortId))
                    {
                        PortPM transshipment3ToPort = portQuery.GetSinglePM(masterData.Transshipment3ToPortId, masterData.Tenant);
                        if (transshipment3ToPort != null)
                        {
                            shipmentPM.Transshipment3ToPortCode = transshipment3ToPort.Code;
                            shipmentPM.Transshipment3ToPortName = transshipment3ToPort.EnglishName;
                            shipmentPM.Transshipment3ToPortCountryCode = transshipment3ToPort.CountryCode;
                            shipmentPM.Transshipment3ToPortCountryName = transshipment3ToPort.CountryName;
                            shipmentPM.Transshipment3ToPortStateCode = transshipment3ToPort.StateCode;

                            shipmentPM.FinalDistenationPortId = masterData.Transshipment3ToPortId;
                        }
                    }


                    if (!string.IsNullOrEmpty(masterData.Transshipment3CarrierId))
                    {
                        Card cardObject = CardRepository.GetSingleCard(masterData.Transshipment3CarrierId, shipment.Tenant, true);
                        if (cardObject != null)
                        {
                            shipmentPM.Transshipment3CarrierCode = cardObject.Code;
                            shipmentPM.Transshipment3CarrierName = cardObject.EnglishName;
                            shipmentPM.Transshipment3CarrierWebSite = cardObject.Website;
                        }
                    }
                    #endregion

                    #region Pre carriage
                    if (!string.IsNullOrEmpty(masterData.PreCarriageFromPortId) || !string.IsNullOrEmpty(masterData.PreCarriageToPortId))
                    {
                        shipmentPM.HasPreCarriage = true;
                    }

                    PortPM precarriageFromPort = null;
                    PortPM precarriageToPort = null;
                    if (!string.IsNullOrEmpty(masterData.PreCarriageFromPortId))
                    {
                        precarriageFromPort = portQuery.GetSinglePM(masterData.PreCarriageFromPortId, masterData.Tenant);
                    }

                    if (!string.IsNullOrEmpty(masterData.PreCarriageToPortId))
                    {
                        precarriageToPort = portQuery.GetSinglePM(masterData.PreCarriageToPortId, masterData.Tenant);
                    }

                    shipmentPM.OriginPreCarriageFromPortId = masterData.PreCarriageFromPortId;
                    shipmentPM.OriginPreCarriageToPortId = masterData.PreCarriageToPortId;
                    shipmentPM.PreCarriageFromPortId = masterData.PreCarriageFromPortId;
                    shipmentPM.PreCarriageToPortId = masterData.PreCarriageToPortId;
                    shipmentPM.PreCarriageCarrierId = masterData.PreCarriageCarrierId;
                    shipmentPM.PreCarriageCarrierNumber = masterData.PreCarriageCarrierNumber;
                    shipmentPM.PreCarriageATA = masterData.PreCarriageATA;
                    shipmentPM.PreCarriageATD = masterData.PreCarriageATD;
                    shipmentPM.PreCarriageETA = masterData.PreCarriageETA;
                    shipmentPM.PreCarriageETD = masterData.PreCarriageETD;
                    shipmentPM.PreCarriageTransportModeId = masterData.PreCarriageTransportModeId;
                    shipmentPM.PreCarriageVesselId = masterData.PreCarriageVesselId;

                    if (precarriageFromPort != null)
                    {
                        shipmentPM.PreCarriageFromPortCode = precarriageFromPort.Code;
                        shipmentPM.PreCarriageFromPortName = precarriageFromPort.EnglishName;
                        shipmentPM.PreCarriageFromPortCountryCode = precarriageFromPort.CountryCode;
                        shipmentPM.PreCarriageFromPortCountryName = precarriageFromPort.CountryName;
                    }

                    if (precarriageToPort != null)
                    {
                        shipmentPM.PreCarriageToPortCode = precarriageToPort.Code;
                        shipmentPM.PreCarriageToPortName = precarriageToPort.EnglishName;
                        shipmentPM.PreCarriageToPortCountryCode = precarriageToPort.CountryCode;
                        shipmentPM.PreCarriageToPortCountryName = precarriageToPort.CountryName;
                    }

                    if (!string.IsNullOrEmpty(masterData.PreCarriageCarrierId))
                    {
                        Card cardObject = CardRepository.GetSingleCard(masterData.PreCarriageCarrierId, masterData.Tenant, true);
                        if (cardObject != null)
                        {
                            shipmentPM.PreCarriageCarrierCode = cardObject.Code;
                            shipmentPM.PreCarriageCarrierName = cardObject.EnglishName;
                            shipmentPM.PreCarriageCarrierWebSite = cardObject.Website;
                        }
                    }

                    if (!string.IsNullOrEmpty(masterData.PreCarriageVesselId))
                    {
                        Vessel vesselEntity = vesselRep.GetSingleVessel(masterData.PreCarriageVesselId, masterData.Tenant);
                        if (vesselEntity != null)
                        {
                            shipmentPM.PreCarriageVesselName = vesselEntity.EnglishName;
                        }
                    }
                    #endregion

                    #region On carriage
                    if (!string.IsNullOrEmpty(masterData.OnCarriageFromPortId) || !string.IsNullOrEmpty(masterData.OnCarriageToPortId))
                    {
                        shipmentPM.HasOnCarriage = true;
                    }

                    PortPM oncarriageFromPort = null;
                    PortPM oncarriageToPort = null;

                    if (!string.IsNullOrEmpty(masterData.OnCarriageFromPortId))
                    {
                        oncarriageFromPort = portQuery.GetSinglePM(masterData.OnCarriageFromPortId, masterData.Tenant);
                    }

                    if (!string.IsNullOrEmpty(masterData.OnCarriageToPortId))
                    {
                        oncarriageToPort = portQuery.GetSinglePM(masterData.OnCarriageToPortId, masterData.Tenant);
                    }

                    shipmentPM.OnCarriageAdditionalTransportModeCode = masterData.OnCarriageAdditionalTransportModeCode;
                    shipmentPM.SplitOnCarriage = masterData.SplitOnCarriage;
                    shipmentPM.OnCarriageFromPortId = masterData.OnCarriageFromPortId;
                    shipmentPM.OnCarriageToPortId = masterData.OnCarriageToPortId;
                    shipmentPM.OriginOnCarriageToPortId = masterData.OnCarriageToPortId;
                    shipmentPM.OriginOnCarriageFromPortId = masterData.OnCarriageFromPortId;
                    shipmentPM.OnCarriageCarrierId = masterData.OnCarriageCarrierId;
                    shipmentPM.OnCarriageCarrierNumber = masterData.OnCarriageCarrierNumber;
                    shipmentPM.OnCarriageATA = masterData.OnCarriageATA;
                    shipmentPM.OnCarriageATD = masterData.OnCarriageATD;
                    shipmentPM.OnCarriageETA = masterData.OnCarriageETA;
                    shipmentPM.OnCarriageETD = masterData.OnCarriageETD;
                    shipmentPM.OnCarriageTransportModeId = masterData.OnCarriageTransportModeId;
                    shipmentPM.OnCarriageVesselId = masterData.OnCarriageVesselId;

                    if (oncarriageFromPort != null)
                    {
                        shipmentPM.OnCarriageFromPortCode = oncarriageFromPort.Code;
                        shipmentPM.OnCarriageFromPortName = oncarriageFromPort.EnglishName;
                        shipmentPM.OnCarriageFromPortCountryCode = oncarriageFromPort.CountryCode;
                        shipmentPM.OnCarriageFromPortCountryName = oncarriageFromPort.CountryName;
                    }

                    if (oncarriageToPort != null)
                    {
                        shipmentPM.OnCarriageToPortCode = oncarriageToPort.Code;
                        shipmentPM.OnCarriageToPortName = oncarriageToPort.EnglishName;
                        shipmentPM.OnCarriageToPortCountryCode = oncarriageToPort.CountryCode;
                        shipmentPM.OnCarriageToPortCountryName = oncarriageToPort.CountryName;
                    }

                    if (!string.IsNullOrEmpty(masterData.OnCarriageCarrierId))
                    {
                        Card cardObject = CardRepository.GetSingleCard(masterData.OnCarriageCarrierId, masterData.Tenant, true);
                        if (cardObject != null)
                        {
                            shipmentPM.OnCarriageCarrierCode = cardObject.Code;
                            shipmentPM.OnCarriageCarrierName = cardObject.EnglishName;
                            shipmentPM.OnCarriageCarrierWebSite = cardObject.Website;
                        }
                    }

                    if (!string.IsNullOrEmpty(masterData.OnCarriageVesselId))
                    {
                        Vessel vesselEntity = vesselRep.GetSingleVessel(masterData.OnCarriageVesselId, masterData.Tenant);
                        if (vesselEntity != null)
                        {
                            shipmentPM.OnCarriageVesselName = vesselEntity.EnglishName;
                        }
                    }

                    #endregion

                    shipmentPM.MainCarriageCarrierPrefix = masterData.MainCarriageCarrierPrefix;
                    shipmentPM.Transshipment1CarrierPrefix = masterData.Transshipment1CarrierPrefix;
                    shipmentPM.Transshipment2CarrierPrefix = masterData.Transshipment2CarrierPrefix;
                    shipmentPM.Transshipment3CarrierPrefix = masterData.Transshipment3CarrierPrefix;

                    shipmentPM.MasterPreCarriageCarrierNumber = masterData.PreCarriageCarrierNumber;
                    shipmentPM.MasterProjectNumber = shipment.ProjectNumber;
                    shipmentPM.MasterPreCarriageFromPortName = precarriageFromPort != null ? precarriageFromPort.EnglishName : null;

                    if (!string.IsNullOrEmpty(masterData.PreCarriageVesselId))
                    {
                        Vessel vesselEntity = vesselRep.GetSingleVessel(masterData.PreCarriageVesselId, tenant);
                        if (vesselEntity != null)
                        {
                            shipmentPM.MasterPreCarriageVesselName = vesselEntity.EnglishName;
                        }
                    }

                    if (!string.IsNullOrEmpty(masterData.PreCarriageFromPortId))
                    {
                        Port portEntity = portsRep.GetSinglePort(masterData.PreCarriageFromPortId, tenant);
                        if (portEntity != null)
                        {
                            shipmentPM.MasterPreCarriageFromPortName = portEntity.EnglishName;
                        }
                    }
                }
            }
            #endregion

            #region else House Shipment Ports
            else
            {
                if (!isInlandDomesticShipment)
                {
                    shipmentPM.MainCarriageFromPortId = shipment.FromPortId;
                    shipmentPM.MainCarriageToPortId = shipment.ToPortId;
                    shipmentPM.MainCarriageFinalDestinationPortId = shipment.ToPortId;
                    shipmentPM.MainCarriageTransportModeId = shipment.TransportModeId;

                    PortPM fromPort = portQuery.GetSinglePM(shipment.FromPortId, shipment.Tenant);
                    if (fromPort != null)
                    {
                        shipmentPM.MainCarriageFromPortCode = fromPort.Code;
                        shipmentPM.MainCarriageFromPortName = fromPort.EnglishName;
                        shipmentPM.MainCarriageFromPortCountryCode = fromPort.CountryCode;
                        shipmentPM.MainCarriageFromPortCountryName = fromPort.CountryName;
                        shipmentPM.FromPort = fromPort.Code;
                        shipmentPM.FromPortCountry = fromPort.CountryName;
                        shipmentPM.FromPortName = fromPort.EnglishName;
                        shipmentPM.FromCountryCode = fromPort.CountryCode;
                        shipmentPM.FromCountryId = fromPort.CountryId;
                        shipmentPM.FromCountryIsEC = fromPort.CountryEC;
                    }

                    PortPM toPort = portQuery.GetSinglePM(shipment.ToPortId, shipment.Tenant);
                    if (toPort != null)
                    {
                        shipmentPM.MainCarriageToPortCode = toPort.Code;
                        shipmentPM.MainCarriageToPortName = toPort.EnglishName;
                        shipmentPM.MainCarriageToPortCountryCode = toPort.CountryCode;
                        shipmentPM.MainCarriageToPortCountryName = toPort.CountryName;
                        shipmentPM.ToPort = toPort.Code;
                        shipmentPM.ToPortCountry = toPort.CountryName;
                        shipmentPM.ToPortName = toPort.EnglishName;
                        shipmentPM.ToCountryCode = toPort.CountryCode;
                        shipmentPM.ToCountryId = toPort.CountryId;
                        shipmentPM.ToCountryIsEC = toPort.CountryEC;

                        shipmentPM.MainCarriageFinalDestinationPortCode = toPort.Code;
                        shipmentPM.MainCarriageFinalDestinationPortName = toPort.EnglishName;
                        shipmentPM.MainCarriageFinalDestinationPortCountryCode = toPort.CountryCode;
                        shipmentPM.MainCarriageFinalDestinationPortCountryName = toPort.CountryName;
                    }
                }
            }
            shipmentPM.Routing = shipment.Routing;
            #endregion

            #region Partners
            shipmentPM.ShipmentCustomerTypeCode = shipment.ShipmentCustomerTypeCode;
            shipmentPM.ConsigneeAddressOneTime = shipment.ConsigneeAddressOneTime;
            shipmentPM.ShipperAddressOneTime = shipment.ShipperAddressOneTime;
            shipmentPM.WarehouseStorageFreeDays = shipment.WarehouseStorageFreeDays;

            CardRepository cardRepository = new CardRepository(myCommonContext);


            #region Customer
            shipmentPM.CustomerId = shipment.CustomerId;
            shipmentPM.CustomerAddressId = shipment.CustomerAddressId;
            shipmentPM.CustomerContactId = shipment.CustomerContactId;
            shipmentPM.CustomerReference1 = shipment.CustomerReference1;
            shipmentPM.CustomerReference2 = shipment.CustomerReference2;

            if (!string.IsNullOrEmpty(shipment.CustomerId))
            {
                Card card = cardRepository.GetSingleCard(shipment.CustomerId, shipment.Tenant);

                if (card != null)
                {
                    shipmentPM.CustomerName = card.EnglishName;
                    shipmentPM.CustomerNote = card.Notes;

                    if (card.PartnerTypeId == "CS")
                    {
                        CustomerRepository customerRepository = new CustomerRepository(shipment.Tenant);
                        Customer customer = customerRepository.GetSingleCustomer(shipment.CustomerId, shipment.Tenant, false);
                        if (customer != null)
                        {
                            if (!string.IsNullOrEmpty(customer.RankId))
                            {
                                RankRepository rankRepository = new RankRepository(shipment.Tenant);
                                Rank rank = rankRepository.GetSingleRank(customer.RankId, shipment.Tenant);
                                if (rank != null)
                                {
                                    shipmentPM.CustomerRankName = rank.Name;
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            #region FreightForwarder
            shipmentPM.FreightForwarderId = shipment.FreightForwarderId;
            shipmentPM.FreightForwarderAddressId = shipment.FreightForwarderAddressId;
            shipmentPM.FreightForwarderContactId = shipment.FreightForwarderContactId;
            shipmentPM.FreightForwarderReference = shipment.FreightForwarderReference;
            if (shipment.FreightForwarderId != null)
            {
                Card loadedCard = CardRepository.GetSingleCard(shipment.FreightForwarderId, shipment.Tenant, true);
                shipmentPM.FreightForwarderName = loadedCard.EnglishName;
                shipmentPM.FreightForwarderNote = loadedCard.Notes;
            }
            #endregion

            #region Shipper
            shipmentPM.ShipperId = shipment.ShipperId;
            shipmentPM.ShipperAddressId = shipment.ShipperAddressId;
            shipmentPM.ShipperContactId = shipment.ShipperContactId;
            shipmentPM.ShipperReference1 = shipment.ShipperReference1;
            shipmentPM.ShipperReference2 = shipment.ShipperReference2;

            if (shipment.ShipperId != null)
            {
                Card loadedCard = CardRepository.GetSingleCard(shipment.ShipperId, shipment.Tenant, true);
                shipmentPM.ShipperName = loadedCard.EnglishName;
                shipmentPM.ShipperNote = loadedCard.Notes;

                if (!string.IsNullOrEmpty(shipment.ShipperAddressId))
                {
                    Address shipperAddress = addressRepository.GetSingleAddress(shipmentPM.ShipperAddressId, tenant);
                    if (shipperAddress != null)
                    {
                        shipmentPM.ShipperAddressText = shipperAddress.City + (shipperAddress.Country == null ? "" : ", " + shipperAddress.Country.EnglishName);
                        shipmentPM.ShipperAddressCountryCode = shipperAddress.Country == null ? "" : shipperAddress.Country.Code;

                        shipmentPM.ShipperAddress1 = shipperAddress.Address1;
                        shipmentPM.ShipperAddress2 = shipperAddress.Address2;
                        shipmentPM.ShipperCity = shipperAddress.City;
                        shipmentPM.ShipperCountryId = shipperAddress.CountryId;
                        shipmentPM.ShipperStateId = shipperAddress.StateId;
                        shipmentPM.ShipperZipCode = shipperAddress.ZipCode;
                        shipmentPM.ShipperPhoneNumber = shipperAddress.PhoneNumber;
                        shipmentPM.ShipperFaxNumber = shipperAddress.FaxNumber;
                    }
                }
            }
            else
            {
                shipmentPM.ShipperName = shipment.ShipperName;
            }
            #endregion

            #region Consignee
            shipmentPM.ConsigneeId = shipment.ConsigneeId;
            shipmentPM.ConsigneeAddressId = shipment.ConsigneeAddressId;
            shipmentPM.ConsigneeContactId = shipment.ConsigneeContactId;
            shipmentPM.ConsigneeReference1 = shipment.ConsigneeReference1;
            shipmentPM.ConsigneeReference2 = shipment.ConsigneeReference2;
            if (shipment.ConsigneeId != null)
            {
                Card loadedCard = CardRepository.GetSingleCard(shipment.ConsigneeId, shipment.Tenant, true);
                shipmentPM.ConsigneeName = loadedCard.EnglishName;
                shipmentPM.ConsigneeNote = loadedCard.Notes;
                shipmentPM.ConsigneeVatNumber = loadedCard.VatNumber;

                if (!string.IsNullOrEmpty(shipment.ConsigneeAddressId))
                {
                    Address consigneeAddress = addressRepository.GetSingleAddress(shipmentPM.ConsigneeAddressId, tenant);
                    if (consigneeAddress != null)
                    {
                        shipmentPM.ConsigneeAddressText = consigneeAddress.City + (consigneeAddress.Country == null ? "" : ", " + consigneeAddress.Country.EnglishName);
                        shipmentPM.ConsigneeAddressCountryCode = consigneeAddress.Country == null ? "" : consigneeAddress.Country.Code;

                        shipmentPM.ConsigneeAddress1 = consigneeAddress.Address1;
                        shipmentPM.ConsigneeAddress2 = consigneeAddress.Address2;
                        shipmentPM.ConsigneeCity = consigneeAddress.City;
                        shipmentPM.ConsigneeCountryId = consigneeAddress.CountryId;
                        shipmentPM.ConsigneeStateId = consigneeAddress.StateId;
                        shipmentPM.ConsigneeZipCode = consigneeAddress.ZipCode;
                        shipmentPM.ConsigneePhoneNumber = consigneeAddress.PhoneNumber;
                        shipmentPM.ConsigneeFaxNumber = consigneeAddress.FaxNumber;
                    }
                }
            }
            #endregion

            #region Agent
            shipmentPM.AgentId = shipment.AgentId;
            shipmentPM.AgentComputed = shipment.AgentComputed;
            shipmentPM.AgentAddressId = shipment.AgentAddressId;
            shipmentPM.AgentContactId = shipment.AgentContactId;
            shipmentPM.AgentReference1 = shipment.AgentReference1;
            shipmentPM.AgentReference2 = shipment.AgentReference2;
            if (shipment.AgentId != null)
            {
                Card loadedCard = CardRepository.GetSingleCard(shipment.AgentId, shipment.Tenant, true);
                shipmentPM.AgentName = loadedCard.EnglishName;
                shipmentPM.AgentNote = loadedCard.Notes;

                if (!string.IsNullOrEmpty(shipment.AgentAddressId))
                {
                    Address agentAddress = addressRepository.GetSingleAddress(shipmentPM.AgentAddressId, tenant);
                    if (agentAddress != null)
                    {
                        shipmentPM.AgentAddressText = agentAddress.City + (agentAddress.Country == null ? "" : ", " + agentAddress.Country.EnglishName);
                        shipmentPM.AgentAddressCountryCode = agentAddress.Country == null ? "" : agentAddress.Country.Code;
                    }
                }
            }
            #endregion

            #region IssuingCarrierAgent
            shipmentPM.IssuingCarrierAgentId = shipment.IssuingCarrierAgentId;
            shipmentPM.IssuingCarrierAddressId = shipment.IssuingCarrierAddressId;

            if (shipment.IssuingCarrierAgentId != null)
            {
                Card loadedCard = CardRepository.GetSingleCard(shipment.IssuingCarrierAgentId, shipment.Tenant, true);
                if (loadedCard != null)
                {
                    shipmentPM.IssuingCarrierAgentName = loadedCard.EnglishName;
                    shipmentPM.IssuingCarrierAgentNote = loadedCard.Notes;
                }
                if (!string.IsNullOrEmpty(shipment.IssuingCarrierAddressId))
                {
                    Address myAddress = addressRepository.GetSingleAddress(shipmentPM.IssuingCarrierAddressId, tenant);
                    if (myAddress != null)
                    {
                        shipmentPM.IssuingCarrierCity = myAddress.City;
                    }
                }
            }
            #endregion

            #region CustomAgentExport
            shipmentPM.CustomAgentExportId = shipment.CustomAgentExportId;
            shipmentPM.CustomAgentExportAddressId = shipment.CustomAgentExportAddressId;
            shipmentPM.CustomAgentExportContactId = shipment.CustomAgentExportContactId;
            shipmentPM.CustomAgentExportReference = shipment.CustomAgentExportReference;
            if (shipment.CustomAgentExportId != null)
            {
                Card loadedCard = CardRepository.GetSingleCard(shipment.CustomAgentExportId, shipment.Tenant, true);
                shipmentPM.CustomAgentExportName = loadedCard.EnglishName;
                shipmentPM.CustomAgentExportNote = loadedCard.Notes;
            }
            #endregion

            #region CustomAgentImport
            shipmentPM.CustomAgentImportId = shipment.CustomAgentImportId;
            shipmentPM.CustomAgentImportAddressId = shipment.CustomAgentImportAddressId;
            shipmentPM.CustomAgentImportContactId = shipment.CustomAgentImportContactId;
            shipmentPM.CustomAgentImportReference = shipment.CustomAgentImportReference;
            if (shipment.CustomAgentImportId != null)
            {
                Card loadedCard = CardRepository.GetSingleCard(shipment.CustomAgentImportId, shipment.Tenant, true);
                shipmentPM.CustomAgentImportName = loadedCard.EnglishName;
                shipmentPM.CustomAgentImportNote = loadedCard.Notes;
            }
            #endregion

            #region Notify1
            shipmentPM.Notify1Id = shipment.Notify1Id;
            shipmentPM.Notify1AddressId = shipment.Notify1AddressId;
            shipmentPM.Notify1ContactId = shipment.Notify1ContactId;
            if (shipment.Notify1Id != null)
            {
                Card loadedCard = CardRepository.GetSingleCard(shipment.Notify1Id, shipment.Tenant, true);
                shipmentPM.Notify1Name = loadedCard.EnglishName;
                shipmentPM.Notify1Note = loadedCard.Notes;

                if (!string.IsNullOrEmpty(shipment.Notify1AddressId))
                {
                    Address notify1Address = addressRepository.GetSingleAddress(shipmentPM.Notify1AddressId, tenant);
                    if (notify1Address != null)
                    {
                        shipmentPM.Notify1Address1 = notify1Address.Address1;
                        shipmentPM.Notify1Address2 = notify1Address.Address2;
                        shipmentPM.Notify1City = notify1Address.City;
                        shipmentPM.Notify1CountryId = notify1Address.CountryId;
                        shipmentPM.Notify1StateId = notify1Address.StateId;
                        shipmentPM.Notify1ZipCode = notify1Address.ZipCode;
                        shipmentPM.Notify1PhoneNumber = notify1Address.PhoneNumber;
                        shipmentPM.Notify1FaxNumber = notify1Address.FaxNumber;
                        shipmentPM.Notify1AddressCountryCode = notify1Address.Country == null ? "" : notify1Address.Country.Code;
                    }
                }
            }
            #endregion

            #region Notify2
            shipmentPM.Notify2Id = shipment.Notify2Id;
            shipmentPM.Notify2AddressId = shipment.Notify2AddressId;
            shipmentPM.Notify2ContactId = shipment.Notify2ContactId;
            if (shipment.Notify2Id != null)
            {
                Card loadedCard = CardRepository.GetSingleCard(shipment.Notify2Id, shipment.Tenant, true);
                shipmentPM.Notify2Name = loadedCard.EnglishName;
                shipmentPM.Notify2Note = loadedCard.Notes;

                if (!string.IsNullOrEmpty(shipment.Notify2AddressId))
                {
                    Address notify2Address = addressRepository.GetSingleAddress(shipmentPM.Notify2AddressId, tenant);
                    if (notify2Address != null)
                    {
                        shipmentPM.Notify2Address1 = notify2Address.Address1;
                        shipmentPM.Notify2Address2 = notify2Address.Address2;
                        shipmentPM.Notify2City = notify2Address.City;
                        shipmentPM.Notify2StateId = notify2Address.StateId;
                        shipmentPM.Notify2ZipCode = notify2Address.ZipCode;
                        shipmentPM.Notify1AddressCountryCode = notify2Address.Country == null ? "" : notify2Address.Country.Code;
                    }
                }
            }
            #endregion

            #region ShipperNotExporter
            shipmentPM.ShipperNotExporterId = shipment.ShipperNotExporterId;
            shipmentPM.ShipperNotExporterAddressId = shipment.ShipperNotExporterAddressId;
            shipmentPM.ShipperNotExporterContactId = shipment.ShipperNotExporterContactId;
            if (shipment.ShipperNotExporterId != null)
            {
                Card loadedCard = CardRepository.GetSingleCard(shipment.ShipperNotExporterId, shipment.Tenant, true);
                shipmentPM.ShipperNotExporterName = loadedCard.EnglishName;
                shipmentPM.ShipperNotExporterNote = loadedCard.Notes;
            }
            #endregion

            #region ConsigneeNotImporter
            shipmentPM.ConsigneeNotImporterId = shipment.ConsigneeNotImporterId;
            shipmentPM.ConsigneeNotImporterAddressId = shipment.ConsigneeNotImporterAddressId;
            shipmentPM.ConsigneeNotImporterContactId = shipment.ConsigneeNotImporterContactId;
            if (shipment.ConsigneeNotImporterId != null)
            {
                Card loadedCard = CardRepository.GetSingleCard(shipment.ConsigneeNotImporterId, shipment.Tenant, true);
                shipmentPM.ConsigneeNotImporterName = loadedCard.EnglishName;
                shipmentPM.ConsigneeNotImporterNote = loadedCard.Notes;
            }
            #endregion

            #region CustomClearancePoint
            shipmentPM.CustomClearancePointId = shipment.CustomClearancePointId;
            shipmentPM.CustomClearancePointAddressId = shipment.CustomClearancePointAddressId;
            shipmentPM.CustomClearancePointContactId = shipment.CustomClearancePointContactId;
            shipmentPM.CustomClearancePointReference1 = shipment.CustomClearancePointReference1;
            if (shipment.CustomClearancePointId != null)
            {
                Card loadedCard = CardRepository.GetSingleCard(shipment.CustomClearancePointId, shipment.Tenant, true);
                shipmentPM.CustomClearancePointName = loadedCard.EnglishName;
                shipmentPM.CustomClearancePointNote = loadedCard.Notes;
            }
            #endregion

            #region Coloader
            shipmentPM.ColoaderId = shipment.ColoaderId;
            shipmentPM.ColoaderAddressId = shipment.ColoaderAddressId;
            shipmentPM.ColoaderContactId = shipment.ColoaderContactId;
            shipmentPM.ColoaderReference1 = shipment.ColoaderReference1;
            if (shipment.ColoaderId != null)
            {
                Card loadedCard = CardRepository.GetSingleCard(shipment.ColoaderId, shipment.Tenant, true);
                shipmentPM.ColoaderName = loadedCard.EnglishName;
                shipmentPM.ColoaderNote = loadedCard.Notes;
            }
            #endregion

            #region Freelancer
            shipmentPM.FreelancerId = shipment.FreelancerId;
            shipmentPM.FreelancerAddressId = shipment.FreelancerAddressId;
            shipmentPM.FreelancerContactId = shipment.FreelancerContactId;

            if (shipment.FreelancerId != null)
            {
                Card loadedCard = CardRepository.GetSingleCard(shipment.FreelancerId, shipment.Tenant, true);
                shipmentPM.FreelancerName = loadedCard.EnglishName;

            }
            #endregion

            #region Consolidator
            shipmentPM.ConsolidatorId = shipment.ConsolidatorId;
            shipmentPM.ConsolidatorAddressId = shipment.ConsolidatorAddressId;
            shipmentPM.ConsolidatorContactId = shipment.ConsolidatorContactId;
            shipmentPM.ConsolidatorReference = shipment.ConsolidatorReference;
            if (shipment.ConsolidatorId != null)
            {
                Card loadedCard = CardRepository.GetSingleCard(shipment.ConsolidatorId, shipment.Tenant, true);
                shipmentPM.ConsolidatorName = loadedCard.EnglishName;
                shipmentPM.ConsolidatorNote = loadedCard.Notes;
            }
            #endregion

            #region ReleasingAgent
            shipmentPM.ReleasingAgentId = shipment.ReleasingAgentId;
            shipmentPM.ReleasingAgentAddressId = shipment.ReleasingAgentAddressId;
            shipmentPM.ReleasingAgentContactId = shipment.ReleasingAgentContactId;
            shipmentPM.ReleasingAgentReference1 = shipment.ReleasingAgentReference1;
            shipmentPM.ReleasingAgentReference2 = shipment.ReleasingAgentReference2;
            if (shipment.ReleasingAgentId != null)
            {
                Card loadedCard = CardRepository.GetSingleCard(shipment.ReleasingAgentId, shipment.Tenant, true);
                shipmentPM.ReleasingAgentName = loadedCard.EnglishName;
                shipmentPM.ReleasingAgentNote = loadedCard.Notes;
            }
            #endregion
            #endregion

            #region Fields
            BranchRepository branchRep = new BranchRepository(myCommonContext);
            Branch branch = branchRep.GetSingleBranch(shipment.BranchId, shipment.Tenant);

            if (branch != null)
            {
                shipmentPM.BranchName = branch.EnglishName;

                if (!string.IsNullOrEmpty(branch.AddressId))
                {
                    Address branchAddress = addressRepository.GetSingleAddress(branch.AddressId, tenant);
                    if (branchAddress != null)
                    {
                        shipmentPM.BranchAddress = branchAddress.City + (branchAddress.Country == null ? "" : ", " + branchAddress.Country.EnglishName);
                    }
                }
            }

            shipmentPM.AWBPrint = shipment.AWBPrint;
            shipmentPM.LastFSRStatusRequestDate = shipment.LastFSRStatusRequestDate;
            shipmentPM.IsFSRSent = shipment.IsFSRSent;

            shipmentPM.FHLStatusCode = shipment.FHLStatusCode;
            shipmentPM.FHLStatusDate = shipment.FHLStatusDate;
            if (!string.IsNullOrEmpty(shipmentPM.FHLStatusCode))
            {
                FHLStatusRepository fHLStatusRepository = new FHLStatusRepository(tenant);
                FHLStatus fHLStatus = fHLStatusRepository.GetSingleFHLStatus(shipment.FHLStatusCode);
                if (fHLStatus != null)
                {
                    shipmentPM.FHLStatusName = fHLStatus.Name;
                }
            }

            shipmentPM.CargonautFHLStatusCode = shipment.CargonautFHLStatusCode;
            shipmentPM.CargonautFHLStatusDate = shipment.CargonautFHLStatusDate;
            if (!string.IsNullOrEmpty(shipmentPM.CargonautFHLStatusCode))
            {
                FHLStatusRepository fHLStatusRepository = new FHLStatusRepository(tenant);
                FHLStatus fHLStatus = fHLStatusRepository.GetSingleFHLStatus(shipment.CargonautFHLStatusCode);
                if (fHLStatus != null)
                {
                    shipmentPM.CargonautFHLStatusName = fHLStatus.Name;
                }
            }

            shipmentPM.CarrierLastStatusDate = shipment.CarrierLastStatusDate;
            shipmentPM.CarrierLastStatusCode = shipment.CarrierLastStatusCode;
            if (!string.IsNullOrEmpty(shipmentPM.CarrierLastStatusCode))
            {
                AWBStatusRepository aWBStatusRepository = new AWBStatusRepository(tenant);
                AWBStatus awbStatus = aWBStatusRepository.GetSingleAWBStatus(shipment.CarrierLastStatusCode);
                if (awbStatus != null)
                {
                    shipmentPM.CarrierLastStatusName = awbStatus.Name;
                }
            }

            shipmentPM.NumberOfInsidePackages = shipment.NumberOfInsidePackages;
            shipmentPM.NumberOfInsidePackagesDetails = shipment.NumberOfInsidePackagesDetails;
            shipmentPM.ComputedStatusName = shipment.ComputedEntityStatus != null ? shipment.ComputedEntityStatus.Name : "";
            shipmentPM.ComputedStatusDate = shipment.ComputedStatusDate;
            shipmentPM.ForeignPartnerCountryCode = shipment.ForeignPartnerCountryCode;
            shipmentPM.ComputedStatusId = shipment.ComputedStatusId;
            shipmentPM.MasterShipmentDataId = shipment.MasterShipmentDataId;
            shipmentPM.FNAReason = shipment.FNAReason;
            shipmentPM.AWBSpecialHandlingCodeId1 = shipment.AWBSpecialHandlingCodeId1;
            shipmentPM.AWBSpecialHandlingCodeId2 = shipment.AWBSpecialHandlingCodeId2;
            shipmentPM.AWBSpecialHandlingCodeId3 = shipment.AWBSpecialHandlingCodeId3;
            shipmentPM.AWBSpecialHandlingCodeId4 = shipment.AWBSpecialHandlingCodeId4;
            shipmentPM.AWBSpecialHandlingCodeId5 = shipment.AWBSpecialHandlingCodeId5;
            shipmentPM.AWBSpecialHandlingCodeId6 = shipment.AWBSpecialHandlingCodeId6;
            shipmentPM.AWBSpecialHandlingCodeId7 = shipment.AWBSpecialHandlingCodeId7;
            shipmentPM.AWBSpecialHandlingCodeId8 = shipment.AWBSpecialHandlingCodeId8;
            shipmentPM.AWBSpecialHandlingCodeId9 = shipment.AWBSpecialHandlingCodeId9;
            shipmentPM.IssuingCarrierIATACode = shipment.IssuingCarrierIATACode;
            shipmentPM.AWBPlace = shipment.AWBPlace;
            shipmentPM.AWBSignature = shipment.AWBSignature;
            shipmentPM.AWBChargesCodeCode = shipment.AWBChargesCodeCode;
            shipmentPM.ConcurrencyGUID = shipment.ConcurrencyGUID;
            shipmentPM.SCI = shipment.SCI;
            shipmentPM.AWBComments = shipment.AWBComments;
            shipmentPM.AWBPrintingComments = shipment.AWBPrintingComments;
            shipmentPM.TransportModeName = transportmode.Name;
            shipmentPM.ShipmentTypeName = shipment.ShipmentType != null ? shipment.ShipmentType.Name : "";
            shipmentPM.TransportModeId = shipment.TransportModeId;
            shipmentPM.OrderGrossWeight = shipment.OrderGrossWeight;
            shipmentPM.BookingVolume = shipment.BookingVolume;
            shipmentPM.OrderVolumetricWeight = shipment.OrderVolumetricWeight;
            shipmentPM.OrderChargeableWeight = shipment.OrderChargeableWeight;
            shipmentPM.BookingNumberOfPackages = shipment.BookingNumberOfPackages;
            shipmentPM.OrderIsDangerouseGoods = shipment.OrderIsDangerouseGoods;
            shipmentPM.OrderGrossWeightEdited = shipment.OrderGrossWeightEdited;
            shipmentPM.OrderChargeableWeightEdited = shipment.OrderChargeableWeightEdited;
            shipmentPM.AsAgreedFreight = shipment.AsAgreedFreight;
            shipmentPM.AsAgreedOtherCharges = shipment.AsAgreedOtherCharges;
            shipmentPM.AccountNumber = shipment.AccountNumber;
            shipmentPM.DirectionName = direction.Name;
            shipmentPM.ForeignPartnerCountryCode = shipment.ForeignPartnerCountryCode;
            shipmentPM.IsNewARInvoiceBlocked = shipment.IsNewARInvoiceBlocked;
            shipmentPM.OperationalDate = shipment.OperationalDate;
            shipmentPM.BasicFreightId = shipment.BasicFreightId;
            shipmentPM.DestinationPortChargesId = shipment.DestinationPortChargesId;
            shipmentPM.DestinationHaulageChargesId = shipment.DestinationHaulageChargesId;
            shipmentPM.AdditionalChargesId = shipment.AdditionalChargesId;
            shipmentPM.FreightPayerId = shipment.FreightPayerId;
            shipmentPM.FreightPayerAddressId = shipment.FreightPayerAddressId;
            shipmentPM.ARInvoices = shipment.ARInvoices;

            #region ppcc region
            string ppcc = "";
            if (shipment.FreightPrepaidCollectId == "P")
            {
                ppcc = "PP";
            }
            if (shipment.FreightPrepaidCollectId == "C")
            {
                ppcc = "CC";
            }
            shipmentPM.PPCC = ppcc;
            #endregion

            if (masterData != null)
            {
                if (masterData.MainCarriageATD != null)
                {
                    shipmentPM.FlightDate = masterData.MainCarriageATD;
                    shipmentPM.IsFlightDateActual = true;
                }
                else
                {
                    shipmentPM.FlightDate = masterData.MainCarriageETD;
                    shipmentPM.IsFlightDateActual = false;
                }

                shipmentPM.CutoffDate = masterData.CutoffDate;
            }

            /* Bills*/
            shipmentPM.House = shipment.House;
            shipmentPM.HAWBDate = shipment.HAWBDate;

            /* Ayman */
            shipmentPM.Id = shipment.Id;
            shipmentPM.IncotermId = shipment.IncotermId;
            if (shipment.IncotermId != null)
            {
                if (shipment.Incoterm == null)
                {
                    IncotermRepository incotermrep = new IncotermRepository(myCommonContext);
                    Incoterm incoterm = incotermrep.GetSingleIncoterm(shipment.IncotermId, shipment.Tenant);
                    shipmentPM.IncotermCode = incoterm.Code;
                    shipmentPM.IncotermName = incoterm.Name;
                }
                else
                {
                    shipmentPM.IncotermCode = shipment.Incoterm.Code;
                    shipmentPM.IncotermName = shipment.Incoterm.Name;
                }
            }

            shipmentPM.IsOperationalClosed = shipment.IsOperationalClosed;
            shipmentPM.OperationalCloseDate = shipment.OperationalCloseDate;
            shipmentPM.AccountingCloseDate = shipment.AccountingCloseDate;
            shipmentPM.AgentAddressId = shipment.AgentAddressId;
            shipmentPM.AgentContactId = shipment.AgentContactId;
            shipmentPM.AgentId = shipment.AgentId;
            shipmentPM.AgentComputed = shipment.AgentComputed;
            shipmentPM.BranchId = shipment.BranchId;
            shipmentPM.FreelancerId = shipment.FreelancerId;
            shipmentPM.FreelancerAddressId = shipment.FreelancerAddressId;
            shipmentPM.FreelancerContactId = shipment.FreelancerContactId;
            shipmentPM.ChargeableWeightInKG = shipment.ChargeableWeightInKG;
            shipmentPM.GrossWeightEdited = shipment.GrossWeightEdited;
            shipmentPM.ChargeableWeightEdited = shipment.ChargeableWeightEdited;
            shipmentPM.VolumeUnitCode = shipment.VolumeUnitCode;
            shipmentPM.CurrentUserId = shipment.CurrentUserId;
            shipmentPM.DepartmentId = shipment.DepartmentId;
            shipmentPM.DescriptionOfGoods = shipment.DescriptionOfGoods;
            shipmentPM.DirectionId = shipment.DirectionId;
            shipmentPM.Field1 = new CustomFieldClass("Field1", "Shipment", shipment.Field1);
            shipmentPM.Field2 = new CustomFieldClass("Field2", "Shipment", shipment.Field2);
            shipmentPM.Field3 = new CustomFieldClass("Field3", "Shipment", shipment.Field3);
            shipmentPM.Field4 = new CustomFieldClass("Field4", "Shipment", shipment.Field4);
            shipmentPM.Field5 = new CustomFieldClass("Field5", "Shipment", shipment.Field5);
            shipmentPM.Field6 = new CustomFieldClass("Field6", "Shipment", shipment.Field6);
            shipmentPM.Field7 = new CustomFieldClass("Field7", "Shipment", shipment.Field7);
            shipmentPM.Field8 = new CustomFieldClass("Field8", "Shipment", shipment.Field8);
            shipmentPM.Field9 = new CustomFieldClass("Field9", "Shipment", shipment.Field9);
            shipmentPM.Field10 = new CustomFieldClass("Field10", "Shipment", shipment.Field10);
            shipmentPM.Field11 = new CustomFieldClass("Field11", "Shipment", shipment.Field11);
            shipmentPM.Field12 = new CustomFieldClass("Field12", "Shipment", shipment.Field12);
            shipmentPM.Field13 = new CustomFieldClass("Field13", "Shipment", shipment.Field13);
            shipmentPM.Field14 = new CustomFieldClass("Field14", "Shipment", shipment.Field14);
            shipmentPM.Field15 = new CustomFieldClass("Field15", "Shipment", shipment.Field15);
            shipmentPM.Field16 = new CustomFieldClass("Field16", "Shipment", shipment.Field16);
            shipmentPM.Field17 = new CustomFieldClass("Field17", "Shipment", shipment.Field17);
            shipmentPM.Field18 = new CustomFieldClass("Field18", "Shipment", shipment.Field18);
            shipmentPM.Field19 = new CustomFieldClass("Field19", "Shipment", shipment.Field19);
            shipmentPM.Field20 = new CustomFieldClass("Field20", "Shipment", shipment.Field20);
            shipmentPM.Field21 = new CustomFieldClass("Field21", "Shipment", shipment.Field21);
            shipmentPM.Field22 = new CustomFieldClass("Field22", "Shipment", shipment.Field22);
            shipmentPM.Field23 = new CustomFieldClass("Field23", "Shipment", shipment.Field23);
            shipmentPM.Field24 = new CustomFieldClass("Field24", "Shipment", shipment.Field24);
            shipmentPM.Field25 = new CustomFieldClass("Field25", "Shipment", shipment.Field25);
            shipmentPM.Field26 = new CustomFieldClass("Field26", "Shipment", shipment.Field26);
            shipmentPM.Field27 = new CustomFieldClass("Field27", "Shipment", shipment.Field27);
            shipmentPM.Field28 = new CustomFieldClass("Field28", "Shipment", shipment.Field28);
            shipmentPM.Field29 = new CustomFieldClass("Field29", "Shipment", shipment.Field29);
            shipmentPM.Field30 = new CustomFieldClass("Field30", "Shipment", shipment.Field30);
            shipmentPM.Field31 = new CustomFieldClass("Field31", "Shipment", shipment.Field31);
            shipmentPM.Field32 = new CustomFieldClass("Field32", "Shipment", shipment.Field32);
            shipmentPM.Field33 = new CustomFieldClass("Field33", "Shipment", shipment.Field33);
            shipmentPM.Field34 = new CustomFieldClass("Field34", "Shipment", shipment.Field34);
            shipmentPM.Field35 = new CustomFieldClass("Field35", "Shipment", shipment.Field35);
            shipmentPM.Field36 = new CustomFieldClass("Field36", "Shipment", shipment.Field36);
            shipmentPM.Field37 = new CustomFieldClass("Field37", "Shipment", shipment.Field37);
            shipmentPM.Field38 = new CustomFieldClass("Field38", "Shipment", shipment.Field38);
            shipmentPM.Field39 = new CustomFieldClass("Field39", "Shipment", shipment.Field39);
            shipmentPM.Field40 = new CustomFieldClass("Field40", "Shipment", shipment.Field40);

            shipmentPM.GrossWeightInKG = shipment.GrossWeightInKG;
            shipmentPM.GrossWeightPerStorageDays = shipment.GrossWeightPerStorageDays;
            shipmentPM.GrossWeight = shipment.GrossWeight;
            shipmentPM.ChargeableWeight = shipment.ChargeableWeight;
            shipmentPM.Notes = shipment.Notes;
            shipmentPM.CreateDateTime = shipment.CreateDateTime;
            shipmentPM.CreatedByUserId = shipment.CreatedByUserId;
            shipmentPM.OperationalClosedByUserId = shipment.OperationalClosedByUserId;

            shipmentPM.SalesmanUserId = shipment.SalesmanUserId;
            shipmentPM.AccountManagerUserId = shipment.AccountManagerUserId;
            shipmentPM.ShipmentNumber = shipment.ShipmentNumber;
            shipmentPM.ShipmentTypeId = shipment.ShipmentTypeId;
            shipmentPM.LastStatusLogDate = shipment.LastStatusLogDate;
            shipmentPM.ExceptionDescription = shipment.ExceptionDescription;
            shipmentPM.ExceptionResolvedDescription = shipment.ExceptionResolvedDescription;
            shipmentPM.LastExceptionDescription = shipment.LastExceptionDescription;
            shipmentPM.GrossWeightPerTon = shipment.GrossWeightPerTon;

            shipmentPM.ExceptionDate = shipment.ExceptionDate;
            shipmentPM.HasException = shipment.HasException;
            shipmentPM.CustomConnectToShipment = shipment.CustomConnectToShipment;

            if (shipmentPM.HasException)
            {
                shipmentPM.HasExceptionMessage = "SHIPMENT HAS EXCEPTION";
            }

            shipmentPM.IsManifestSentToAgent = shipment.IsManifestSentToAgent;
            shipmentPM.AgentSharedManifestRef = shipment.AgentSharedManifestRef;
            shipmentPM.ManifestLastSharingDate = shipment.ManifestLastSharingDate;

            shipmentPM.IsAccrualsApproved = shipment.IsAccrualsApproved;
            shipmentPM.AccrualsApprovalDate = shipment.AccrualsApprovalDate;

            if (!string.IsNullOrEmpty(shipmentPM.SalesmanUserId))
            {
                Contact myContact = ContactRepository.GetSingleContact(shipmentPM.SalesmanUserId, tenant, true);
                if (myContact != null)
                {
                    shipmentPM.SalesmanUserName = myContact.EnglishName;
                }
            }

            if (!string.IsNullOrEmpty(shipmentPM.AccountManagerUserId))
            {
                Contact myContact = ContactRepository.GetSingleContact(shipmentPM.AccountManagerUserId, tenant, true);
                if (myContact != null)
                {
                    shipmentPM.AccountManagerUserName = myContact.EnglishName;
                }
            }



            string str = string.Empty;
            if (shipment.ShipmentType != null)
            {
                str = shipment.ShipmentType.Name;
            }

            str = string.IsNullOrEmpty(str) ? shipmentLevel.Name : str + " " + shipmentLevel.Name;
            shipmentPM.ShipmentType = str;
            shipmentPM.Tenant = shipment.Tenant;
            shipmentPM.ShipmentPMId = shipment.Id;
            shipmentPM.FreightPrepaidCollectId = shipment.FreightPrepaidCollectId;
            shipmentPM.OtherPrepaidCollectId = shipment.OtherPrepaidCollectId;
            shipmentPM.AWBCurrencyId = shipment.AWBCurrencyId;
            if (awbCurrency != null)
            {
                shipmentPM.AWBCurrencyCode = awbCurrency.Code;
            }

            shipmentPM.ShipperNotExporterId = shipment.ShipperNotExporterId;
            shipmentPM.ConsigneeNotImporterId = shipment.ConsigneeNotImporterId;
            shipmentPM.DimensionsUnitCode = shipment.DimensionsUnitCode;
            shipmentPM.GrossWeightUnitCode = shipment.GrossWeightUnitCode;
            shipmentPM.ChargeableWeightUnitCode = shipment.ChargeableWeightUnitCode;
            shipmentPM.Volume = shipment.Volume;
            shipmentPM.VolumeInCBM = shipment.VolumeInCBM;
            shipmentPM.VolumetricWeight = shipment.VolumetricWeight;
            shipmentPM.Ratio = shipment.Ratio;
            shipmentPM.DimFactor = shipment.DimFactor;
            shipmentPM.PackagesQuantity = shipment.PackagesQuantity;
            shipmentPM.NumberOfPackages = shipment.NumberOfPackages;
            shipmentPM.NumberOfContainers = shipment.NumberOfContainers;
            shipmentPM.AWBFreightAmountCollect = shipment.AWBFreightAmountCollect;
            shipmentPM.AWBFreightAmountPrepaid = shipment.AWBFreightAmountPrepaid;
            shipmentPM.MainHarmonize = shipment.MainHarmonize;
            shipmentPM.IsDangerous = shipment.IsDangerous;
            shipmentPM.DangerousClassNumber = shipment.DangerousClassNumber;
            shipmentPM.DangerousFlashPoint = shipment.DangerousFlashPoint;
            shipmentPM.DangerousIMDGCode = shipment.DangerousIMDGCode;
            shipmentPM.DangerousMaterialDescription = shipment.DangerousMaterialDescription;
            shipmentPM.DangerousPackagingGroup = shipment.DangerousPackagingGroup;
            shipmentPM.DangerousUnNumber = shipment.DangerousUnNumber;
            shipmentPM.LTCWEdited = shipment.LTCWEdited;
            shipmentPM.ShipmentDeliveryIndex = shipment.ShipmentDeliveryIndex;
            shipmentPM.ShipmentPickUpIndex = shipment.ShipmentPickUpIndex;
            shipmentPM.ShipmentContainerReturnIndex = shipment.ShipmentContainerReturnIndex;
            shipmentPM.IsAccountingClosed = shipment.IsAccountingClosed;
            shipmentPM.IsCancelled = shipment.IsCancelled;
            shipmentPM.CancelledDate = shipment.CancelledDate;
            shipmentPM.ShipmentPayableStatusCode = shipment.ShipmentPayableStatusCode;
            shipmentPM.ShipmentReceivableStatusCode = shipment.ShipmentReceivableStatusCode;
            shipmentPM.ShipmentPayableStatusName = shipment.ShipmentPayableStatus != null ? shipment.ShipmentPayableStatus.Name : null;
            shipmentPM.ShipmentReceivableStatusName = shipment.ShipmentReceivableStatus != null ? shipment.ShipmentReceivableStatus.Name : null;
            shipmentPM.LastUpdateDate = shipment.LastUpdateDate;
            shipmentPM.UpdatedByUserId = shipment.UpdatedByUserId;
            shipmentPM.OrderChargeableWeight = shipment.OrderChargeableWeight;
            shipmentPM.OrderVolumetricWeight = shipment.OrderVolumetricWeight;
            shipmentPM.EstimateProfitInLocalCurrency = shipment.EstimateProfitInLocalCurrency;
            shipmentPM.EstimateProfitInProfitCurrency = shipment.EstimateProfitInProfitCurrency;
            shipmentPM.AWBAccountingInformation = shipment.AWBAccountingInformation;
            shipmentPM.AWBCarrierTarrifReference = shipment.AWBCarrierTarrifReference;
            shipmentPM.AWBDeclaredValueForCarriage = shipment.AWBDeclaredValueForCarriage;
            shipmentPM.AWBDeclaredValueForCustoms = shipment.AWBDeclaredValueForCustoms;
            shipmentPM.AWBInsurrenceValue = shipment.AWBInsurrenceValue;
            shipmentPM.AWBHandlingInformation = shipment.AWBHandlingInformation;
            shipmentPM.NextLegCode = shipment.NextLegCode;
            shipmentPM.NextETA = shipment.NextETA;
            shipmentPM.NextETD = shipment.NextETD;
            shipmentPM.ShipmentLevelCode = shipment.ShipmentLevelCode;
            shipmentPM.ShipmentLevelName = shipment.ShipmentLevel != null ? shipment.ShipmentLevel.Name : null;
            shipmentPM.NextLegName = shipment.NextLeg != null ? shipment.NextLeg.Name : null;
            shipmentPM.ShipmentTypeViewField = (shipment.ShipmentType != null ? shipment.ShipmentType.Name : "") + " " + (shipment.ShipmentLevel != null ? shipment.ShipmentLevel.Name : "");
            shipmentPM.CASSCode = shipment.CASSCode;
            shipmentPM.SLAC = shipment.SLAC;
            shipmentPM.NoFreightFile = shipment.NoFreightFile;
            shipmentPM.DeliveryOrder = shipment.DeliveryOrder;
            shipmentPM.FreightLocationId = shipment.FreightLocationId;
            shipmentPM.TransportDocumentNumber = shipment.TransportDocumentNumber;
            shipmentPM.SpecialServicesTypeId = shipment.SpecialServicesTypeId;
            shipmentPM.SpecialServicesTypeName = shipment.SpecialServicesType != null ? shipment.SpecialServicesType.EnglishName : null;
            shipmentPM.ARInvoiceIssued = shipment.ARInvoiceIssued;
            shipmentPM.CreditNoteIssued = shipment.CreditNoteIssued;
            shipmentPM.NominatedHandlingPartyId = shipment.NominatedHandlingPartyId;
            shipmentPM.OtherParticipantIdCode1 = shipment.OtherParticipantIdCode1;
            shipmentPM.OtherParticipantIdCode2 = shipment.OtherParticipantIdCode2;
            shipmentPM.OtherParticipantIdCode3 = shipment.OtherParticipantIdCode3;
            shipmentPM.OtherParticipantInformationCode1 = shipment.OtherParticipantInformationCode1;
            shipmentPM.OtherParticipantInformationCode2 = shipment.OtherParticipantInformationCode2;
            shipmentPM.OtherParticipantInformationCode3 = shipment.OtherParticipantInformationCode3;
            shipmentPM.OtherParticipantInformationPortCode1 = shipment.OtherParticipantInformationPortCode1;
            shipmentPM.OtherParticipantInformationPortCode2 = shipment.OtherParticipantInformationPortCode2;
            shipmentPM.OtherParticipantInformationPortCode3 = shipment.OtherParticipantInformationPortCode3;
            shipmentPM.OtherParticipantInformationName1 = shipment.OtherParticipantInformationName1;
            shipmentPM.OtherParticipantInformationName2 = shipment.OtherParticipantInformationName2;
            shipmentPM.OtherParticipantInformationName3 = shipment.OtherParticipantInformationName3;
            shipmentPM.OtherParticipantInformationReference1 = shipment.OtherParticipantInformationReference1;
            shipmentPM.OtherParticipantInformationReference2 = shipment.OtherParticipantInformationReference2;
            shipmentPM.OtherParticipantInformationReference3 = shipment.OtherParticipantInformationReference3;
            shipmentPM.AccountingInformation1 = shipment.AccountingInformation1;
            shipmentPM.AccountingInformation2 = shipment.AccountingInformation2;
            shipmentPM.AccountingInformation3 = shipment.AccountingInformation3;
            shipmentPM.AccountingInformation4 = shipment.AccountingInformation4;
            shipmentPM.AccountingInformation5 = shipment.AccountingInformation5;
            shipmentPM.AccountingInformation6 = shipment.AccountingInformation6;
            shipmentPM.AccountingInformationIdentifierCode1 = shipment.AccountingInformationIdentifierCode1;
            shipmentPM.AccountingInformationIdentifierCode2 = shipment.AccountingInformationIdentifierCode2;
            shipmentPM.AccountingInformationIdentifierCode3 = shipment.AccountingInformationIdentifierCode3;
            shipmentPM.AccountingInformationIdentifierCode4 = shipment.AccountingInformationIdentifierCode4;
            shipmentPM.AccountingInformationIdentifierCode5 = shipment.AccountingInformationIdentifierCode5;
            shipmentPM.AccountingInformationIdentifierCode6 = shipment.AccountingInformationIdentifierCode6;
            shipmentPM.ReferenceNumber = shipment.ReferenceNumber;
            shipmentPM.SupplementaryShipmentInformation1 = shipment.SupplementaryShipmentInformation1;
            shipmentPM.SupplementaryShipmentInformation2 = shipment.SupplementaryShipmentInformation2;
            shipmentPM.ViaColoader = shipment.ViaColoader;
            shipmentPM.IssuingCarrierReference1 = shipment.IssuingCarrierReference1;
            shipmentPM.CustomsDeclarationNumber = shipment.CustomsDeclarationNumber;
            shipmentPM.CustomerShipmentNumber = shipment.CustomerShipmentNumber;
            shipmentPM.CustomerTenantNumber = shipment.CustomerTenantNumber;
            shipmentPM.ForwarderShipmentNumber = shipment.ForwarderShipmentNumber;
            shipmentPM.ForwarderPartnerId = shipment.ForwarderPartnerId;
            shipmentPM.ForwardingPartnerId = shipment.ForwardingPartnerId;
            shipmentPM.NumberOfFollowUps = shipment.NumberOfFollowUps;
            #endregion

            #region Pre Forwarding
            if (!string.IsNullOrEmpty(shipment.PreForwardingFromPortId) || !string.IsNullOrEmpty(shipment.PreForwardingToPortId))
            {
                shipmentPM.HasPreForwarding = true;
            }

            PortPM preForwardingFromPort = null;
            PortPM preForwardingToPort = null;

            if (!string.IsNullOrEmpty(shipment.PreForwardingFromPortId))
            {
                preForwardingFromPort = portQuery.GetSinglePM(shipment.PreForwardingFromPortId, shipment.Tenant);
            }

            if (!string.IsNullOrEmpty(shipment.PreForwardingToPortId))
            {
                preForwardingToPort = portQuery.GetSinglePM(shipment.PreForwardingToPortId, shipment.Tenant);
            }

            shipmentPM.PreForwardingFromPortId = shipment.PreForwardingFromPortId;
            shipmentPM.PreForwardingToPortId = shipment.PreForwardingToPortId;
            shipmentPM.PreForwardingCarrierId = shipment.PreForwardingCarrierId;
            shipmentPM.PreForwardingCarrierNumber = shipment.PreForwardingCarrierNumber;
            shipmentPM.PreForwardingATA = shipment.PreForwardingATA;
            shipmentPM.PreForwardingATD = shipment.PreForwardingATD;
            shipmentPM.PreForwardingETA = shipment.PreForwardingETA;
            shipmentPM.PreForwardingETD = shipment.PreForwardingETD;
            shipmentPM.PreForwardingTransportModeId = shipment.PreForwardingTransportModeId;
            shipmentPM.PreForwardingVesselId = shipment.PreForwardingVesselId;

            if (preForwardingFromPort != null)
            {
                shipmentPM.PreForwardingFromPortCode = preForwardingFromPort.Code;
                shipmentPM.PreForwardingFromPortName = preForwardingFromPort.EnglishName;
                shipmentPM.PreForwardingFromPortCountryCode = preForwardingFromPort.CountryCode;
                shipmentPM.PreForwardingFromPortCountryName = preForwardingFromPort.CountryName;
            }

            if (preForwardingToPort != null)
            {
                shipmentPM.PreForwardingToPortCode = preForwardingToPort.Code;
                shipmentPM.PreForwardingToPortName = preForwardingToPort.EnglishName;
                shipmentPM.PreForwardingToPortCountryCode = preForwardingToPort.CountryCode;
                shipmentPM.PreForwardingToPortCountryName = preForwardingToPort.CountryName;
            }

            if (!string.IsNullOrEmpty(shipment.PreForwardingCarrierId))
            {
                Card cardObject = CardRepository.GetSingleCard(shipment.PreForwardingCarrierId, shipment.Tenant, true);
                if (cardObject != null)
                {
                    shipmentPM.PreForwardingCarrierCode = cardObject.Code;
                    shipmentPM.PreForwardingCarrierName = cardObject.EnglishName;
                    shipmentPM.PreForwardingCarrierWebSite = cardObject.Website;
                }
            }

            if (!string.IsNullOrEmpty(shipment.PreForwardingVesselId))
            {
                Vessel vesselEntity = vesselRep.GetSingleVessel(shipment.PreForwardingVesselId, shipment.Tenant);
                if (vesselEntity != null)
                {
                    shipmentPM.PreForwardingVesselName = vesselEntity.EnglishName;
                }
            }
            #endregion

            #region On Forwarding
            if (!string.IsNullOrEmpty(shipment.OnForwardingFromPortId) || !string.IsNullOrEmpty(shipment.OnForwardingToPortId))
            {
                shipmentPM.HasOnForwarding = true;
            }

            PortPM onForwardingFromPort = null;
            PortPM onForwardingToPort = null;

            if (!string.IsNullOrEmpty(shipment.OnForwardingFromPortId))
            {
                onForwardingFromPort = portQuery.GetSinglePM(shipment.OnForwardingFromPortId, shipment.Tenant);
            }

            if (!string.IsNullOrEmpty(shipment.OnForwardingToPortId))
            {
                onForwardingToPort = portQuery.GetSinglePM(shipment.OnForwardingToPortId, shipment.Tenant);
            }

            shipmentPM.OnForwardingAdditionalTransportModeCode = shipment.OnForwardingAdditionalTransportModeCode;
            shipmentPM.SplitOnForwarding = shipment.SplitOnForwarding;
            shipmentPM.OnForwardingFromPortId = shipment.OnForwardingFromPortId;
            shipmentPM.OnForwardingToPortId = shipment.OnForwardingToPortId;
            shipmentPM.OnForwardingCarrierId = shipment.OnForwardingCarrierId;
            shipmentPM.OnForwardingCarrierNumber = shipment.OnForwardingCarrierNumber;
            shipmentPM.OnForwardingATA = shipment.OnForwardingATA;
            shipmentPM.OnForwardingATD = shipment.OnForwardingATD;
            shipmentPM.OnForwardingETA = shipment.OnForwardingETA;
            shipmentPM.OnForwardingETD = shipment.OnForwardingETD;
            shipmentPM.OnForwardingTransportModeId = shipment.OnForwardingTransportModeId;
            shipmentPM.OnForwardingVesselId = shipment.OnForwardingVesselId;

            if (onForwardingFromPort != null)
            {
                shipmentPM.OnForwardingFromPortCode = onForwardingFromPort.Code;
                shipmentPM.OnForwardingFromPortName = onForwardingFromPort.EnglishName;
                shipmentPM.OnForwardingFromPortCountryCode = onForwardingFromPort.CountryCode;
                shipmentPM.OnForwardingFromPortCountryName = onForwardingFromPort.CountryName;
            }

            if (onForwardingToPort != null)
            {
                shipmentPM.OnForwardingToPortCode = onForwardingToPort.Code;
                shipmentPM.OnForwardingToPortName = onForwardingToPort.EnglishName;
                shipmentPM.OnForwardingToPortCountryCode = onForwardingToPort.CountryCode;
                shipmentPM.OnForwardingToPortCountryName = onForwardingToPort.CountryName;
            }

            if (!string.IsNullOrEmpty(shipment.OnForwardingCarrierId))
            {
                Card cardObject = CardRepository.GetSingleCard(shipment.OnForwardingCarrierId, shipment.Tenant, true);
                if (cardObject != null)
                {
                    shipmentPM.OnForwardingCarrierCode = cardObject.Code;
                    shipmentPM.OnForwardingCarrierName = cardObject.EnglishName;
                    shipmentPM.OnForwardingCarrierWebSite = cardObject.Website;
                }
            }

            if (!string.IsNullOrEmpty(shipment.OnForwardingVesselId))
            {
                Vessel vesselEntity = vesselRep.GetSingleVessel(shipment.OnForwardingVesselId, shipment.Tenant);
                if (vesselEntity != null)
                {
                    shipmentPM.OnForwardingVesselName = vesselEntity.EnglishName;
                }
            }
            #endregion

            // Warehouse Leg 
            shipmentPM.WarehouseLegWarehouseId = shipment.WarehouseLegWarehouseId;
            shipmentPM.WarehouseLegAddressId = shipment.WarehouseLegAddressId;
            shipmentPM.WarehouseLegTerminalCode = shipment.WarehouseLegTerminalCode;
            shipmentPM.WarehouseLegExpectedEntryDate = shipment.WarehouseLegExpectedEntryDate;
            shipmentPM.WarehouseLegActualEntryDate = shipment.WarehouseLegActualEntryDate;
            shipmentPM.WarehouseLegExpectedReleaseDate = shipment.WarehouseLegExpectedReleaseDate;
            shipmentPM.WarehouseLegActualReleaseDate = shipment.WarehouseLegActualReleaseDate;
            shipmentPM.WarehouseLegLastFreeDate = shipment.WarehouseLegLastFreeDate;
            shipmentPM.WarehouseLegRemarks = shipment.WarehouseLegRemarks;
            shipmentPM.WarehouseLegReference = shipment.WarehouseLegReference;
            shipmentPM.WarehouseLegVGMCutOffDate = shipment.WarehouseLegVGMCutOffDate;
            shipmentPM.WarehouseLegCutOffDate = shipment.WarehouseLegCutOffDate;
            shipmentPM.WarehouseLegEntryDate = shipment.WarehouseLegActualEntryDate != null ? shipment.WarehouseLegActualEntryDate : shipment.WarehouseLegExpectedEntryDate;
            shipmentPM.WarehouseLegReleaseDate = shipment.WarehouseLegActualReleaseDate != null ? shipment.WarehouseLegActualReleaseDate : shipment.WarehouseLegExpectedReleaseDate;
            shipmentPM.ChargeStorage = shipment.ChargeStorage;
            shipmentPM.ChargeStorageCurrencyId = shipment.ChargeStorageCurrencyId;
            shipmentPM.WeightMeasurementCode = shipment.WeightMeasurementCode;
            shipmentPM.WeightRoundingCode = shipment.WeightRoundingCode;
            shipmentPM.IsCFSWarehouse = shipment.IsCFSWarehouse;
            shipmentPM.IsCFSWarehouseChanged = shipment.IsCFSWarehouseChanged;

            if (!string.IsNullOrEmpty(shipment.WarehouseLegWarehouseId))
            {
                Card cardObject = CardRepository.GetSingleCard(shipment.WarehouseLegWarehouseId, shipment.Tenant, true);
                if (cardObject != null)
                {
                    shipmentPM.WarehouseLegTerminalName = cardObject.EnglishName;
                    shipmentPM.WarehouseLegAddressCountryCode = cardObject.CountryCode;
                    shipmentPM.WarehouseLegAddressCountryName = cardObject.CountryName;
                }
            }

            shipmentPM.IsHTSMissing = shipment.IsHTSMissing;
            shipmentPM.FirstARInvoiceApprovalDate = shipment.FirstARInvoiceApprovalDate;
            shipmentPM.RegistryDate = shipment.RegistryDate;
            shipmentPM.IsAssembly = shipment.IsAssembly;
            shipmentPM.LocalCustomsTransmissionsStatusCode = shipment.LocalCustomsTransmissionsStatusCode;
            shipmentPM.LocalCustomsTransmissionsStatusError = shipment.LocalCustomsTransmissionsStatusError;
            shipmentPM.LocalCustomsTransmissionsStatusDate = shipment.LocalCustomsTransmissionsStatusDate;
            shipmentPM.IncludesCustoms = shipment.IncludesCustoms;
            shipmentPM.DeclarationNumber = shipment.DeclarationNumber;
            shipmentPM.DeclarationDate = shipment.DeclarationDate;
            shipmentPM.CustomsClearanceDate = shipment.CustomsClearanceDate;
            shipmentPM.LastSharedEventId = shipment.LastSharedEventId;
            shipmentPM.LastSharedEventLocation = shipment.LastSharedEventLocation;
            shipmentPM.LastSharedEventNotes = shipment.LastSharedEventNotes;
            shipmentPM.LastSharedEventDate = shipment.LastSharedEventDate;
            shipmentPM.LocalCustomsSentByUserId = shipment.LocalCustomsSentByUserId;
            shipmentPM.FirstOperationalCloseDate = shipment.FirstOperationalCloseDate;
            shipmentPM.FirstAccountingCloseDate = shipment.FirstAccountingCloseDate;
            shipmentPM.LastFinalDestination = shipment.LastFinalDestination;
            shipmentPM.FirstPickupETA = shipment.FirstPickupETA;
            shipmentPM.FirstPickupETD = shipment.FirstPickupETD;        
            shipmentPM.From = shipment.From;
            shipmentPM.To = shipment.To;
            shipmentPM.Origin = shipment.Origin;
            shipmentPM.AgentComputed = shipment.AgentComputed;
            shipmentPM.ComputedShipmentNumber = shipment.ComputedShipmentNumber;
            shipmentPM.TruckerId = shipment.TruckerId;
            shipmentPM.AssignedToTruckerDate = shipment.AssignedToTruckerDate;
            shipmentPM.AssginedToCustomsAgentDate = shipment.AssginedToCustomsAgentDate;
            shipmentPM.AssginedtoCustomsAgentId = shipment.AssginedtoCustomsAgentId;

            if (!string.IsNullOrEmpty(shipmentPM.UpdatedByUserId))
            {
                Contact myContact = ContactRepository.GetSingleContact(shipmentPM.UpdatedByUserId, tenant, true);
                if (myContact != null)
                {
                    shipmentPM.UpdatedByUserName = myContact.EnglishName;
                }
            }

            if (!string.IsNullOrEmpty(shipment.LastSharedEventId))
            {
                EventTypeRepository myRepository = new EventTypeRepository(tenant);
                EventType myEvent = myRepository.GetSingleEventType(shipment.LastSharedEventId, tenant);
                if (myEvent != null)
                {
                    shipmentPM.LastSharedEventName = myEvent.EnglishName;
                }
            }

            if (!string.IsNullOrEmpty(shipment.LocalCustomsTransmissionsStatusCode))
            {
                CustomsTransmissionsStatusRepository myRepository = new CustomsTransmissionsStatusRepository(tenant);
                CustomsTransmissionsStatus myStatus = myRepository.GetSingleCustomsTransmissionsStatus(shipment.LocalCustomsTransmissionsStatusCode);
                if (myStatus != null)
                {
                    shipmentPM.LocalCustomsTransmissionsStatusName = myStatus.Name;
                }
            }

            if (!string.IsNullOrEmpty(shipment.LocalCustomsSentByUserId))
            {
                Contact myContact = ContactRepository.GetSingleContact(shipmentPM.LocalCustomsSentByUserId, tenant, true);
                if (myContact != null)
                {
                    shipmentPM.LocalCustomsSentByUserName = myContact.EnglishName;
                }
            }

            shipmentPM.OriginShipmentId = shipment.OriginShipmentId;
            shipmentPM.FBLIsFromStock = shipment.FBLIsFromStock;
            shipmentPM.StatusId = shipment.StatusId;
            shipmentPM.StatusName = shipment.EntityStatus.Name;
            shipmentPM.StatusLocation = shipment.StatusLocation;
            shipmentPM.StatusDate = shipment.StatusDate;
            shipmentPM.StatusWeight = shipment.EntityStatus.StatusWeight;
            shipmentPM.LastSentByUserId = shipment.LastSentByUserId;
            shipmentPM.ProfitCurrencyId = shipment.ProfitCurrencyId;
            shipmentPM.ProfitExchangeRate = shipment.ProfitExchangeRate;

            if (shipmentPM.ProfitCurrencyId != null)
            {
                Currency myCurrency = CurrencyRepository.GetSingleCurrency(shipmentPM.ProfitCurrencyId, tenant, true);
                if (myCurrency != null)
                {
                    shipmentPM.ProfitCurrencyCode = myCurrency.Code;
                }
            }

            if (shipment.ShipmentLevelCode == "H" && !string.IsNullOrEmpty(shipment.MasterShipmentDataId))
            {
                if (masterData != null)
                {
                    if (!string.IsNullOrEmpty(masterData.StatusId))
                    {
                        string statusName = null;
                        shipmentPM.StatusId = EntityStatusHelper.GetHighestStatusId(shipment.StatusId, masterData.StatusId, shipment.Tenant, ref statusName);
                        shipmentPM.StatusName = statusName;

                        if (shipmentPM.StatusId == masterData.StatusId)
                        {
                            shipmentPM.StatusDate = masterData.StatusDate;
                            shipmentPM.StatusLocation = masterData.StatusLocation;
                        }
                    }
                }
            }

            #region Totals
            shipmentPM.OpenPayablesInLocalCurrency = shipment.OpenPayablesInLocalCurrency;
            shipmentPM.AccountedPayablesInLocalCurrency = shipment.AccountedPayablesInLocalCurrency;
            shipmentPM.OpenReceivablesInLocalCurrency = shipment.OpenReceivablesInLocalCurrency;
            shipmentPM.AccountedReceivablesInLocalCurrency = shipment.AccountedReceivablesInLocalCurrency;
            shipmentPM.ProfitInLocalCurrency = shipment.ProfitInLocalCurrency;

            shipmentPM.OpenPayablesInProfitCurrency = shipment.OpenPayablesInProfitCurrency;
            shipmentPM.AccountedPayablesInProfitCurrency = shipment.AccountedPayablesInProfitCurrency;
            shipmentPM.OpenReceivablesInProfitCurrency = shipment.OpenReceivablesInProfitCurrency;
            shipmentPM.AccountedReceivablesInProfitCurrency = shipment.AccountedReceivablesInProfitCurrency;
            shipmentPM.ProfitInProfitCurrency = shipment.ProfitInProfitCurrency;
            shipmentPM.NotInvoicedReceivablesAmount = shipment.NotInvoicedReceivablesAmount;

            shipmentPM.HousesOpenPayablesInLocal = shipment.HousesOpenPayablesInLocal;
            shipmentPM.HousesOpenPayablesInProfit = shipment.HousesOpenPayablesInProfit;
            shipmentPM.HousesACCTPayablesInLocal = shipment.HousesACCTPayablesInLocal;
            shipmentPM.HousesACCTPayablesInProfit = shipment.HousesACCTPayablesInProfit;
            shipmentPM.HousesOpenReceivablesInLocal = shipment.HousesOpenReceivablesInLocal;
            shipmentPM.HousesOpenReceivablesInProfit = shipment.HousesOpenReceivablesInProfit;
            shipmentPM.HousesACCTReceivablesInLocal = shipment.HousesACCTReceivablesInLocal;
            shipmentPM.HousesACCTReceivablesInProfit = shipment.HousesACCTReceivablesInProfit;

            #endregion

            #region Routings
            shipmentPM.FromPortId = shipment.FromPortId;
            shipmentPM.ToPortId = shipment.ToPortId;
            shipmentPM.QuoteId = shipment.QuoteId;
            shipmentPM.QuoteNumber = shipment.QuoteNumber;
            #endregion

            #region follow ups

            FollowUpRepository followUpsRepository = new FollowUpRepository(webFreightContext);

            if (shipmentPM.FollowUps.Count != 0)
            {
                shipmentPM.FollowUps.Clear();
            }

            List<FollowUp> followupList = followUpsRepository.GetFollowUpsByShipmentId(shipment.Id, shipment.Tenant);
            foreach (FollowUp follow in followupList)
            {
                ShipmentFollowUpPM followUpPM = new ShipmentFollowUpPM()
                {
                    Tenant = follow.Tenant,
                    Date = follow.Date,
                    Done = follow.Done,
                    DoneDateTime = follow.DoneDateTime,
                    DoneNote = follow.DoneNote,
                    //EntityTypeId = follow.EntityTypeId,
                    ExternalDocumentId = follow.DocumentsFilingId,
                    //FollowUpTypeId = follow.FollowUpTypeId,
                    Id = follow.Id,
                    InternalDocumentId = follow.InternalDocumentId,
                    IsNew = follow.IsNew,
                    JobId = follow.JobId,
                    LegType = follow.LegType,
                    Note = follow.Notes,
                    ShipmentId = follow.ShipmentId,
                    //FollowUpTypeName = follow.FollowUpType.Name,
                    //EntityDateId = follow.FollowUpType.EntityDateId
                    EventTypeId = follow.EventTypeId,
                    EventTypeFollowUpName = follow.EventType.FollowUpEnglishName,
                    ManualActivatedFollowUp = follow.EventType.ManualActivatedFollowUp,
                    OwnerUserId = follow.OwnerUserId,
                    OwnerUserName = follow.OwnerUser.Contact.EnglishName,
                    Area = follow.Area,
                    DocumentTypeId = follow.DocumentTypeId,
                    AutomationId = follow.AutomationId,
                };
                shipmentPM.FollowUps.Add(followUpPM);
            }
            #endregion

            shipmentPM.IsStandalonePickupDelivery = shipment.IsStandalonePickupDelivery;
            shipmentPM.ProductCode = shipment.ProductCode;
            shipmentPM.LastStatusLogDate = shipment.LastStatusLogDate;
            shipmentPM.ComputedStatusId = shipment.ComputedStatusId;
            shipmentPM.ComputedStatusDate = shipment.ComputedStatusDate;
            shipmentPM.FinalArrivalDate = shipment.FinalArrivalDate;
            shipmentPM.EstimatedFinalArrivalDate = shipment.EstimatedFinalArrivalDate;
            shipmentPM.CreateDateTime = shipment.CreateDateTime;
            shipmentPM.ActualFinalArrivalDate = shipment.ActualFinalArrivalDate;
            shipmentPM.CustomFileId = shipment.CustomFileId;
            shipmentPM.CustomFileNumber = shipment.CustomFileNumber;
            shipmentPM.IsMultipleCommodities = shipment.IsMultipleCommodities;
            shipmentPM.AMSBL = shipment.AMSBL;
            shipmentPM.MoveTypeId = shipment.MoveTypeId;
            shipmentPM.HasContainerException = shipment.HasContainerException;
            shipmentPM.WarehouseStorageFreeDays = shipment.WarehouseStorageFreeDays;
            shipmentPM.OrderIsDangerouseGoods = shipment.OrderIsDangerouseGoods;
            shipmentPM.FirstPickupETA = shipment.FirstPickupETA;
            shipmentPM.FirstPickupETD = shipment.FirstPickupETD;
            shipmentPM.WarehouseLegLastFreeDate = shipment.WarehouseLegLastFreeDate;

            if (shipment.MoveTypeId != null)
            {
                MoveTypeRepository moveTyperep = new MoveTypeRepository(webFreightContext);
                MoveType moveType = moveTyperep.GetSingleMoveType(shipment.MoveTypeId, shipment.Tenant);

                if (moveType != null)
                {
                    shipmentPM.MoveTypeCode = moveType.Code;
                    shipmentPM.MoveTypeName = moveType.MoveTypeEnglishName;
                }
            }

            if (withComposition)
            {
                #region ShipmentCarrierStatuses
                ShipmentCarrierStatusQuery shipmentCarrierStatusQuery = new ShipmentCarrierStatusQuery(tenant);
                shipmentPM.ShipmentCarrierStatuses = shipmentCarrierStatusQuery.GetShipmentCarrierStatusPMsByShipmentId(shipment).ToList();
                #endregion

                #region Order Packages
                ShipmentOrderPackageRepository shipmentOrderPackageRepository = new ShipmentOrderPackageRepository(repository.context);
                ShipmentOrderPackageQuery shipmentOrderPackageQuery = new ShipmentOrderPackageQuery(shipmentOrderPackageRepository);
                shipmentPM.ShipmentOrderPackages = shipmentOrderPackageQuery.GetShipmentOrderPackagesByShipment(shipment.Id, shipment.Tenant);
                #endregion

                #region Shipment Assembleies
                ShipmentAssemblyRepository shipmentAssemblyRepository = new ShipmentAssemblyRepository(repository.context);
                ShipmentAssemblyQuery shipmentAssemblyQuery = new ShipmentAssemblyQuery(shipmentAssemblyRepository);

                shipmentPM.ShipmentAssemblies = shipmentAssemblyQuery.GetShipmentAssemblies(shipment.Id, shipment.Tenant);
                #endregion

                #region Packages | Commodities
                ShipmentPackageRepository shipmentPackageRepository = new ShipmentPackageRepository(repository.context);
                ShipmentPackageQuery shipmentPackageQuery = new ShipmentPackageQuery(shipmentPackageRepository);

                ShipmentCommodityRepository shipmentCommodityRepository = new ShipmentCommodityRepository(repository.context);
                ShipmentCommodityQuery shipmentCommodityQuery = new ShipmentCommodityQuery(shipmentCommodityRepository);
                #endregion

                #region shipment order packages
                shipmentPM.ShipmentOrderPackages = shipmentOrderPackageQuery.GetShipmentOrderPackagesByShipment(shipment.Id, shipment.Tenant);
                #endregion

                #region shipment receivables
                ShipmentReceivableRepository shipmentReceivablesRepository = new ShipmentReceivableRepository(repository.context);
                ShipmentReceivableQuery shipmentReceivablesQuery = new ShipmentReceivableQuery(shipmentReceivablesRepository);
                shipmentPM.ShipmentReceivables = shipmentReceivablesQuery.GetShipmentReceivablePMsByShipmentId(shipment.Id, shipment.Tenant);

                foreach (ShipmentReceivablePM item in shipmentPM.ShipmentReceivables)
                {
                    item.ShipmentNumber = shipment.ShipmentNumber;
                }
                #endregion

                #region shipment payables
                ShipmentPayableRepository shipmentPayableRepository = new ShipmentPayableRepository(repository.context);
                ShipmentPayableQuery shipmentPayableQuery = new ShipmentPayableQuery(shipmentPayableRepository);
                shipmentPM.ShipmentPayables = shipmentPayableQuery.GetShipmentPayablePMsByShipment(shipment.Id, shipment.Tenant);

                foreach (ShipmentPayablePM item in shipmentPM.ShipmentPayables)
                {
                    item.ShipmentNumber = shipment.ShipmentNumber;
                }
                #endregion

                #region Shipment packages | Commodities

                if (shipment.TransportModeId == "A")
                {
                    shipmentPM.ShipmentCommodities = shipmentCommodityQuery.GetCommoditiesByShipmentId(shipment.Id, shipment.Tenant);

                    if (shipment.IsMultipleCommodities)
                    {
                        shipmentPM.AWBChargeAmount = shipmentPM.ShipmentCommodities.Sum(s => s.ChargeAmount);
                    }

                    else
                    {
                        ShipmentCommodityPM myShipmentCommodityPM = shipmentPM.ShipmentCommodities.OrderBy(d => d.Id).FirstOrDefault();

                        if (myShipmentCommodityPM != null)
                        {
                            shipmentPM.RateClassCode = myShipmentCommodityPM.RateClassCode;
                            shipmentPM.AWBChargeRate = myShipmentCommodityPM.ChargeRate;
                            shipmentPM.AWBChargeAmount = myShipmentCommodityPM.ChargeAmount;
                            shipmentPM.AWBCommodityItemNumber = myShipmentCommodityPM.CommodityNumber;
                        }
                    }
                }

                shipmentPM.ShipmentPackages = shipmentPackageQuery.GetShipmentPackages(shipment.Id, shipment.ShipmentNumber, shipment.Tenant);
                if (shipment.ShipmentLevelCode == "H" && !string.IsNullOrEmpty(shipment.MasterShipmentDataId) && masterData != null)
                {
                    shipmentPM.ConnectedMasterPackages = shipmentPackageQuery.GetShipmentPackages(masterData.Id, shipment.ShipmentNumber, shipment.Tenant);
                }
                #endregion

                #region AWB Print Onlies
                ShipmentAWBPrintOnlyRepository shipmentAWBPrintOnlyRepository = new ShipmentAWBPrintOnlyRepository(repository.context);
                ShipmentAWBPrintOnlyQuery shipmentAwbPrintOnlyQuery = new ShipmentAWBPrintOnlyQuery(shipmentAWBPrintOnlyRepository);
                shipmentPM.ShipmentAWBPrintOnlies = shipmentAwbPrintOnlyQuery.GetShipmentAWBPrintOnlyPMsByShipment(shipment.Id, shipment.Tenant);
                #endregion

                #region AWBOCIPMs
                AWBOCIRepository aWBOCIRepository = new AWBOCIRepository(repository.context);
                AWBOCIQuery aWBOCIQuery = new AWBOCIQuery(aWBOCIRepository);
                shipmentPM.AWBOCIPMs = aWBOCIQuery.GetAWBOCIPMsByShipmentId(shipmentPM.Id, shipmentPM.Tenant).ToList();
                #endregion

                #region Shipment ARInvoices
                if (shipmentPM.ShipmentARInvoices != null)
                {
                    ARInvoiceStatusRepository statusRepository = new ARInvoiceStatusRepository(tenant);
                    List<ARInvoiceStatus> allStatuses = statusRepository.GetARInvoiceStatus().ToList();

                    ARInvoiceRepository aRInvoiceRepository = new ARInvoiceRepository(shipment.Tenant);
                    List<ARInvoice> invoices = aRInvoiceRepository.GetInvoicesByShipmentId(shipmentPM.Id, shipmentPM.Tenant);

                    if (shipmentPM.ShipmentLevelCode == "H" && shipmentPM.MasterShipmentDataId != null)
                    {
                        List<ARInvoice> invoices_Childs = aRInvoiceRepository.GetInvoicesByShipmentId(shipmentPM.MasterShipmentDataId, shipmentPM.Tenant);
                        foreach (ARInvoice item in invoices_Childs)
                        {
                            if (!invoices.Where(d => d.Id == item.Id).Any())
                            {
                                invoices.Add(item);
                            }
                        }
                    }

                    foreach (ARInvoice item in invoices)
                    {
                        ShipmentARInvoicePM entityPM = new ShipmentARInvoicePM()
                        {
                            Id = item.Id,
                            ShipmentId = shipmentPM.Id,
                            InvoiceNumber = item.InvoiceNumber,
                            InvoiceCurrencyId = item.InvoiceCurrencyId,
                            DueDate = item.DueDate,
                            StatusCode = item.StatusCode,
                            InvoiceTypeCode = item.ARInvoiceTypeCode,
                            AmountInLocalCurrency = item.AmountInLocalCurrency,
                            AmountInProfitCurrency = item.AmountInProfitCurrency,
                            AmountInInvoiceCurrency = item.AmountInInvoiceCurrency,
                            AmountDue = item.AmountDue,
                            IsAutoCredit = item.IsAutoCredit,
                            IsCancelled = item.IsCancelled,
                            InvoiceDate = item.InvoiceDate,
                            IsConstituentInvoice = item.IsConstituentInvoice,
                            IsConsolidationInvoice = item.IsConsolidationInvoice,
                            ConsolidationInvoiceId = item.ConsolidationInvoiceId,
                        };

                        Currency myCurrency = CurrencyRepository.GetSingleCurrency(entityPM.InvoiceCurrencyId, tenant, true);
                        if (myCurrency != null)
                        {
                            entityPM.InvoiceCurrencyCode = myCurrency.Code;
                        }

                        if (item.Id == item.InvoiceNumber)
                        {
                            entityPM.InvoiceNumber = item.DraftNumber;
                        }

                        if (!string.IsNullOrEmpty(item.ConsolidationInvoiceId))
                        {
                            entityPM.ConsolidationInvoiceNumber = aRInvoiceRepository.GetInvoiceNumber(item.ConsolidationInvoiceId, tenant);
                        }

                        ARInvoiceStatus myStatus = allStatuses.Where(d => d.Code == entityPM.StatusCode).FirstOrDefault();
                        if (myStatus != null)
                        {
                            entityPM.StatusName = myStatus.Name;
                        }

                        shipmentPM.ShipmentARInvoices.Add(entityPM);
                    }
                }
                #endregion

                #region Shipment APInvoices
                if (shipmentPM.ShipmentAPInvoices != null)
                {
                    APInvoiceStatusRepository statusRepository = new APInvoiceStatusRepository(tenant);
                    List<APInvoiceStatus> allStatuses = statusRepository.GetAPInvoiceStatus().ToList();

                    APInvoiceRepository apInvoiceReps = new APInvoiceRepository(shipment.Tenant);
                    List<APInvoice> invoices = apInvoiceReps.GetInvoicesByShipmentId(shipmentPM.Id, shipmentPM.Tenant);

                    if (shipmentPM.ShipmentLevelCode == "H" && shipmentPM.MasterShipmentDataId != null)
                    {
                        List<APInvoice> invoices_Childs = apInvoiceReps.GetInvoicesByShipmentId(shipmentPM.MasterShipmentDataId, shipmentPM.Tenant);
                        foreach (APInvoice item in invoices_Childs)
                        {
                            if (!invoices.Where(d => d.Id == item.Id).Any())
                            {
                                invoices.Add(item);
                            }
                        }
                    }

                    foreach (APInvoice invoice in invoices)
                    {
                        ShipmentAPInvoicePM entityPM = new ShipmentAPInvoicePM()
                        {
                            Id = invoice.Id,
                            ShipmentId = shipmentPM.Id,
                            InvoiceNumber = invoice.InvoiceNumber,
                            InvoiceCurrencyId = invoice.InvoiceCurrencyId,
                            DueDate = invoice.DueDate,
                            StatusCode = invoice.StatusCode,
                            GrandTotalInLocalCurrency = invoice.AmountInLocalCurrency,
                            GrandTotalInProfitCurrency = invoice.AmountInProfitCurrency,
                            GrandTotalInInvoiceCurrency = invoice.AmountInInvoiceCurrency,
                        };

                        APInvoiceStatus myStatus = allStatuses.Where(d => d.Code == entityPM.StatusCode).FirstOrDefault();
                        if (myStatus != null)
                        {
                            entityPM.StatusName = myStatus.Name;
                        }

                        shipmentPM.ShipmentAPInvoices.Add(entityPM);
                    }
                }
                #endregion

                #region ShipmenConsoleShipments
                if (shipmentPM.ShipmentLevelCode == "C")
                {
                    ShipmentConsoleShipmentQuery shipmentConsoleShipmentQuery = new ShipmentConsoleShipmentQuery(this.repository.context);
                    shipmentConsoleShipmentQuery.BuildConsoleShipments(shipmentPM);
                }
                #endregion

                #region ShipmentStoragePricings
                ShipmentStoragePricingRepository shipmentStoragePricingRepository = new ShipmentStoragePricingRepository(repository.context);
                ShipmentStoragePricingQuery shipmentStoragePricingQuery = new ShipmentStoragePricingQuery(shipmentStoragePricingRepository);
                shipmentPM.ShipmentStoragePricings = shipmentStoragePricingQuery.GetShipmentStoragePricingsByShipmentId(shipment.Id, shipment.Tenant);
                #endregion

                #region ShipmentProductItems
                ShipmentProductItemRepository shipmentProductItemRepository = new ShipmentProductItemRepository(repository.context);
                ShipmentProductItemQuery shipmentProductItemQuery = new ShipmentProductItemQuery(shipmentProductItemRepository);

                shipmentPM.ShipmentProductItems = shipmentProductItemQuery.GetShipmentProductItems(shipment.Id, shipment.Tenant);
                #endregion
            }

            #region Pickups & Deliveries

            shipmentPM.ShipmentPickUps = shipmentPickUpQuery.GetShipmentPickUpPMsByTenantAndShipment(shipment.Id, shipment.Tenant).ToList();
            shipmentPM.ShipmentDeliveries = shipmentDeliveryQuery.GetShipmentDeliveryPMsByTenantAndShipment(shipment.Id, shipment.Tenant, true).ToList();

            foreach (ShipmentDeliveryPM item in shipmentPM.ShipmentDeliveries)
            {
                ShipmentPackagePM myPackage = shipmentPM.ShipmentPackages.Where(d => d.DeliveryId == item.Id || d.EmptyContainerReturnId == item.Id).FirstOrDefault();
                if (myPackage != null)
                {
                    item.ConnectedPackageId = myPackage.Id;
                }

                item.AllConnectedPackagesId = new List<string>();

                foreach (ShipmentPackagePM ConnectedPackage in shipmentPM.ShipmentPackages.Where(d => d.DeliveryId == item.Id))
                {
                    item.AllConnectedPackagesId.Add(ConnectedPackage.Id);
                }
            }

            if (shipmentPM.ShipmentPickUps.Count > 0)
            {
                ShipmentPickUpPM myFirstPickup = shipmentPM.ShipmentPickUps.OrderBy(s => s.PickUpDeliveryNumber).FirstOrDefault();

                if (myFirstPickup != null)
                {
                    shipmentPM.FirstPickupATA = myFirstPickup.ATA;
                    shipmentPM.FirstPickupATD = myFirstPickup.ATD;

                    #region
                    switch (myFirstPickup.PickUpDeliveryFromTypeCode)
                    {
                        case "PART":
                            {
                                if (!string.IsNullOrEmpty(myFirstPickup.FromPartnerCardId))
                                {
                                    if (!string.IsNullOrEmpty(myFirstPickup.FromAddressId))
                                    {
                                        Address myPartnerAddress = addressRepository.GetSingleAddress(myFirstPickup.FromAddressId, tenant);
                                        if (myPartnerAddress != null)
                                        {
                                            shipmentPM.FirstPickupLocation = myPartnerAddress.City;
                                        }
                                    }
                                    else
                                    {
                                        Address myPartnerAddress = addressRepository.GetMainAddressByCardId(myFirstPickup.FromPartnerCardId, tenant);
                                        if (myPartnerAddress != null)
                                        {
                                            shipmentPM.FirstPickupLocation = myPartnerAddress.City;
                                        }
                                    }
                                }


                                break;
                            }

                        case "PORT":
                            {
                                if (!string.IsNullOrEmpty(myFirstPickup.FromPortId))
                                {
                                    PortPM myPort = PortQuery.GetSinglePort(tenant, myFirstPickup.FromPortId, true);
                                    if (myPort != null)
                                    {
                                        shipmentPM.FirstPickupLocation = myPort.EnglishName;
                                    }
                                }

                                break;
                            }

                        case "CASL":
                            {
                                shipmentPM.FirstPickupLocation = myFirstPickup.FromAddressCity;
                                break;
                            }
                    }
                    #endregion
                }
            }

            if (shipmentPM.ShipmentDeliveries.Count > 0)
            {
                ShipmentDeliveryPM myFinalDelivery = shipmentPM.ShipmentDeliveries.Where(d => d.PickUpDeliveryTypeCode == "DELV").OrderByDescending(s => s.PickUpDeliveryNumber).FirstOrDefault();

                if (myFinalDelivery != null)
                {
                    shipmentPM.FinalDeliveryATA = myFinalDelivery.ATA;
                    shipmentPM.FinalDeliveryATD = myFinalDelivery.ATD;
                    shipmentPM.FinalDeliveryETA = myFinalDelivery.ETA;
                    shipmentPM.FinalDeliveryETD = myFinalDelivery.ETD;

                    #region
                    switch (myFinalDelivery.PickUpDeliveryToTypeCode)
                    {
                        case "PART":
                            {
                                if (!string.IsNullOrEmpty(myFinalDelivery.ToPartnerCardId))
                                {
                                    Address myPartnerAddress = addressRepository.GetSingleAddress(myFinalDelivery.ToAddressId, tenant);
                                    if (myPartnerAddress != null)
                                    {
                                        shipmentPM.FinalDeliveryLocation = myPartnerAddress.City;
                                    }
                                }

                                break;
                            }

                        case "PORT":
                            {
                                if (!string.IsNullOrEmpty(myFinalDelivery.ToPortId))
                                {
                                    PortPM myPort = PortQuery.GetSinglePort(tenant, myFinalDelivery.ToPortId, true);
                                    if (myPort != null)
                                    {
                                        shipmentPM.FinalDeliveryLocation = myPort.EnglishName;
                                    }
                                }

                                break;
                            }

                        case "CASL":
                            {
                                shipmentPM.FinalDeliveryLocation = myFinalDelivery.ToAddressCity;
                                break;
                            }
                    }
                    #endregion
                }
            }
            #endregion

            // Edited by Ayman
            if (shipmentPM.ShipmentPackages != null)
            {
                // By Samar: for message variables
                string myContainersNumbers = null;
                string myPackagesNames = null;
                string myPackagesPrintAs = null;
                foreach (ShipmentPackagePM packagePM in shipmentPM.ShipmentPackages)
                {
                    if (string.IsNullOrEmpty(myContainersNumbers))
                    {
                        myContainersNumbers = packagePM.ContainerNumber;
                    }

                    else
                    {
                        myContainersNumbers += ", " + packagePM.ContainerNumber;
                    }

                    if (string.IsNullOrEmpty(myPackagesNames))
                    {
                        myPackagesNames = packagePM.PackageTypeName;
                    }

                    else
                    {
                        myPackagesNames += ", " + packagePM.PackageTypeName;
                    }

                    if (string.IsNullOrEmpty(myPackagesPrintAs))
                    {
                        myPackagesPrintAs = packagePM.PrintAs;
                    }

                    else
                    {
                        myPackagesPrintAs += ", " + packagePM.PrintAs;
                    }
                }

                if (!string.IsNullOrEmpty(myPackagesNames) && myPackagesNames.Length > 1000)
                {
                    myPackagesNames = myPackagesNames.Substring(0, 1000);
                }

                if (!string.IsNullOrEmpty(myPackagesPrintAs) && myPackagesPrintAs.Length > 1000)
                {
                    myPackagesPrintAs = myPackagesPrintAs.Substring(0, 1000);
                }

                if (!string.IsNullOrEmpty(myContainersNumbers) && myContainersNumbers.Length > 1000)
                {
                    myContainersNumbers = myContainersNumbers.Substring(0, 1000);
                }

                shipmentPM.PackagesTypesNames = myPackagesNames;
                shipmentPM.PackagesTypesPrintAs = myPackagesPrintAs;
                shipmentPM.ContainersNumbers = myContainersNumbers;
                // end 

                var myGroup = (from a in shipmentPM.ShipmentPackages
                               where a.IsContainer && a.PackageTypeId != null
                               group a by a.PackageTypeId into g
                               select new
                               {
                                   PackageTypeId = g.Key,
                                   Count = g.Count(),
                               });

                string myLineText = "";
                string myTotalContainers = "";
                PackageTypeRepository packageTypeRepository = new PackageTypeRepository(tenant);

                foreach (var item in myGroup)
                {
                    myLineText = "";

                    PackageType myPackageType = packageTypeRepository.GetSinglePackageType(item.PackageTypeId, tenant);
                    if (myPackageType != null)
                    {
                        if (!string.IsNullOrEmpty(myPackageType.PrintAs))
                        {
                            myLineText = item.Count + " x " + myPackageType.PrintAs;
                        }

                        else if (!string.IsNullOrEmpty(myPackageType.Code))
                        {
                            myLineText = item.Count + " x " + myPackageType.Code;
                        }
                    }

                    if (!string.IsNullOrEmpty(myLineText))
                    {
                        if (string.IsNullOrEmpty(myTotalContainers))
                        {
                            myTotalContainers = myLineText;
                        }

                        else
                        {
                            myTotalContainers += ", " + myLineText;
                        }
                    }
                }

                shipmentPM.TotalContainers = myTotalContainers;
            }

            if (shipmentPM.ShipmentConsoleShipments != null)
            {
                this.ComputeHousesNumbersField(shipmentPM);

            }

            this.ComputeHousesDescriptionofGoodsField(shipmentPM);

            shipmentPM.TEU = shipment.TEU;
            shipmentPM.SecurityKey = shipment.SecurityKey;
            shipmentPM.NewConcurrencyGUID = Guid.NewGuid().ToString();
            shipmentPM.ValueOfGoods = shipment.ValueOfGoods;
            shipmentPM.ValueOfGoodsCurrencyId = shipment.ValueOfGoodsCurrencyId;
            shipmentPM.ISFDate = shipment.ISFDate;
            shipmentPM.ISFNumber = shipment.ISFNumber;
            shipmentPM.ITDate = shipment.ITDate;
            shipmentPM.ITNumber = shipment.ITNumber;
            shipmentPM.FreightRelease = shipment.FreightRelease;
            shipmentPM.TerminalAvailable = shipment.TerminalAvailable;
            shipmentPM.ENSNumber = shipment.ENSNumber;
            shipmentPM.ENSDate = shipment.ENSDate;
            shipmentPM.AMSClosingDate = shipment.AMSClosingDate;
            shipmentPM.UpdatedByPartner = shipment.UpdatedByPartner;

            shipmentPM.INTTRASIStatusCode = shipment.INTTRASIStatusCode;
            shipmentPM.INTTRASIStatusDate = shipment.INTTRASIStatusDate;
            shipmentPM.INTTRASIError = shipment.INTTRASIError;
            shipmentPM.EmergencyContactId = shipment.EmergencyContactId;
            shipmentPM.INTTRAContractNumber = shipment.INTTRAContractNumber;
            shipmentPM.INTTRAInstructions = shipment.INTTRAInstructions;
            shipmentPM.INTTRAComments = shipment.INTTRAComments;
            shipmentPM.INTTRADocumentQTY = shipment.INTTRADocumentQTY;
            shipmentPM.SIHasAttachList = shipment.SIHasAttachList;
            shipmentPM.INTTRAIsFreighted = shipment.INTTRAIsFreighted;
            shipmentPM.INTTRADocumentTypeCode = shipment.INTTRADocumentTypeCode;
            shipmentPM.INTTRALastStatusDate = shipment.INTTRALastStatusDate;
            shipmentPM.ContainerLastStatusDate = shipment.ContainerLastStatusDate;
            shipmentPM.INTTRABookingStatusCode = shipment.INTTRABookingStatusCode;
            shipmentPM.INTTRABookingTransStatusCode = shipment.INTTRABookingTransStatusCode;
            shipmentPM.INTTRABookingError = shipment.INTTRABookingError;
            shipmentPM.INTTRALastBookingResponse = shipment.INTTRALastBookingResponse;
            shipmentPM.INTTRALastEBbookingSendDate = shipment.INTTRALastEBbookingSendDate;

            if (!string.IsNullOrEmpty(shipmentPM.INTTRALastBookingResponse))
            {
                this.MapINTTRABookingXMLFields(shipmentPM);
            }

            INTTRABookingStatusRepository iNTTRABookingStatusRepository = new INTTRABookingStatusRepository(repository.context);
            if (!string.IsNullOrEmpty(shipmentPM.INTTRABookingStatusCode))
                shipmentPM.INTTRABookingStatusName = iNTTRABookingStatusRepository.GetSingleINTTRABookingStatus(shipmentPM.INTTRABookingStatusCode).Name;


            INTTRABookingTransStatusRepository iNTTRABookingTransStatusRepository = new INTTRABookingTransStatusRepository(repository.context);
            if (!string.IsNullOrEmpty(shipmentPM.INTTRABookingTransStatusCode))
                shipmentPM.INTTRABookingTransStatusName = iNTTRABookingTransStatusRepository.GetSingleINTTRABookingTransStatus(shipmentPM.INTTRABookingTransStatusCode).Name;

            shipmentPM.Notify1Reference = shipment.Notify1Reference;
            shipmentPM.Notify2Reference = shipment.Notify2Reference;
            shipmentPM.ShipperNotExporterReference = shipment.ShipperNotExporterReference;
            shipmentPM.ConsigneeNotImporterReference = shipment.ConsigneeNotImporterReference;
            shipmentPM.ProjectNumber = shipment.ProjectNumber;
            shipmentPM.CreatedByPartner = shipment.CreatedByPartner;

            bool iDangerousShipmentPackages = true;
            if (shipment.IsDangerous)
            {
                foreach (ShipmentPackagePM item in shipmentPM.ShipmentPackages)
                {
                    if (!item.IsDangerous) iDangerousShipmentPackages = false;
                }
            }

            if (shipment.IsDangerous && iDangerousShipmentPackages) shipmentPM.ShipmentContanisDangerousGoods = true;


            MapShipmentComputedFields(shipmentPM);


            this.MapAnalyzerConcurrencyFields(shipmentPM);
            this.MapMainCarriageLegsForAPI(shipmentPM);

            ShipmentPM returnShipment = BranchPermitionsFilter.AddUserBranchRestrictionFilters(new QueryOperations(), shipmentPM, tenant);
            returnShipment = ProductPermitionsFilter.AddUserProductRestrictionFilters(new QueryOperations(), shipmentPM, tenant);

            return returnShipment;
        }

        private void MapMainCarriageLegsForAPI(ShipmentPM shipmentPM)
        {
            shipmentPM.MainCarriageLegs = new List<TransshipmentLeg>();

            shipmentPM.MainCarriageLegs.Add(new TransshipmentLeg()
            {
                LegIndex = 1,
                ATA = shipmentPM.MainCarriageATA,
                ATD = shipmentPM.MainCarriageATD,
                ETA = shipmentPM.MainCarriageETA,
                ETD = shipmentPM.MainCarriageETD,
                FromPortId = shipmentPM.MainCarriageFromPortId,
                ToPortId = shipmentPM.MainCarriageToPortId,
                VesselId = shipmentPM.MainCarriageVesselId,
                CarrierId = shipmentPM.MainCarriageCarrierId,
                CarrierNumber = shipmentPM.MainCarriageCarrierNumber,
                MasterNumber = shipmentPM.Master,
            });

            if(!string.IsNullOrEmpty(shipmentPM.Transshipment1FromPortId))
            {
                shipmentPM.MainCarriageLegs.Add(new TransshipmentLeg()
                {
                    LegIndex = 2,
                    ATA = shipmentPM.Transshipment1ATA,
                    ATD = shipmentPM.Transshipment1ATD,
                    ETA = shipmentPM.Transshipment1ETA,
                    ETD = shipmentPM.Transshipment1ETD,
                    FromPortId = shipmentPM.Transshipment1FromPortId,
                    ToPortId = shipmentPM.Transshipment1ToPortId,
                    VesselId = shipmentPM.Transshipment1VesselId,
                    CarrierId = shipmentPM.Transshipment1CarrierId,
                    CarrierNumber = shipmentPM.Transshipment1CarrierNumber,
                    MasterNumber = shipmentPM.Transshipment1AdditionalMAWBOBLBL,
                });
            }

            if (!string.IsNullOrEmpty(shipmentPM.Transshipment2FromPortId))
            {
                shipmentPM.MainCarriageLegs.Add(new TransshipmentLeg()
                {
                    LegIndex = 2,
                    ATA = shipmentPM.Transshipment2ATA,
                    ATD = shipmentPM.Transshipment2ATD,
                    ETA = shipmentPM.Transshipment2ETA,
                    ETD = shipmentPM.Transshipment2ETD,
                    FromPortId = shipmentPM.Transshipment2FromPortId,
                    ToPortId = shipmentPM.Transshipment2ToPortId,
                    VesselId = shipmentPM.Transshipment2VesselId,
                    CarrierId = shipmentPM.Transshipment2CarrierId,
                    CarrierNumber = shipmentPM.Transshipment2CarrierNumber,
                    MasterNumber = shipmentPM.Transshipment2AdditionalMAWBOBLBL,
                });
            }

            if (!string.IsNullOrEmpty(shipmentPM.Transshipment3FromPortId))
            {
                shipmentPM.MainCarriageLegs.Add(new TransshipmentLeg()
                {
                    LegIndex = 2,
                    ATA = shipmentPM.Transshipment3ATA,
                    ATD = shipmentPM.Transshipment3ATD,
                    ETA = shipmentPM.Transshipment3ETA,
                    ETD = shipmentPM.Transshipment3ETD,
                    FromPortId = shipmentPM.Transshipment3FromPortId,
                    ToPortId = shipmentPM.Transshipment3ToPortId,
                    VesselId = shipmentPM.Transshipment3VesselId,
                    CarrierId = shipmentPM.Transshipment3CarrierId,
                    CarrierNumber = shipmentPM.Transshipment3CarrierNumber,
                    MasterNumber = shipmentPM.Transshipment3AdditionalMAWBOBLBL,
                });
            }
        }

        private void ComputeHousesNumbersField(ShipmentPM shipmentPM)
        {
            var myHousesNumbers = "";
            var myMasterHousesNumbers = "";
            
            foreach (ConsoleShipmentPM console in shipmentPM.ShipmentConsoleShipments)
            {
                if (string.IsNullOrEmpty(myHousesNumbers))
                {
                    myHousesNumbers = console.ShipmentNumber;
                }
                else
                {
                    myHousesNumbers += ", " + console.ShipmentNumber;
                }

                if (string.IsNullOrEmpty(myMasterHousesNumbers))
                {
                    myMasterHousesNumbers = console.House;
                }
                else
                {
                    myMasterHousesNumbers += ", " + console.House;
                }

            }

            if (!string.IsNullOrEmpty(myHousesNumbers) && myHousesNumbers.Length > 1000)
            {
                myHousesNumbers = myHousesNumbers.Substring(0, 1000);
            }

            if (!string.IsNullOrEmpty(myMasterHousesNumbers) && myMasterHousesNumbers.Length > 1000)
            {
                myMasterHousesNumbers = myMasterHousesNumbers.Substring(0, 1000);
            }

            shipmentPM.HousesNumbers = myHousesNumbers;
            shipmentPM.MasterHousesNumbers = myMasterHousesNumbers;
        }
        private void ComputeHousesDescriptionofGoodsField(ShipmentPM shipmentPM)
        {
            var myHousesDescriptionofGoods = "";

            if (shipmentPM.ShipmentLevelCode == "C")
            {
                foreach (ConsoleShipmentPM console in shipmentPM.ShipmentConsoleShipments)
                {
                    if (string.IsNullOrEmpty(myHousesDescriptionofGoods))
                    {
                        myHousesDescriptionofGoods = console.DescriptionOfGoods;
                    }
                    else
                    {
                        myHousesDescriptionofGoods += ", " + console.DescriptionOfGoods;
                    }
                }
            }
            else
            {
                myHousesDescriptionofGoods = shipmentPM.DescriptionOfGoods;
            }
            
            if (!string.IsNullOrEmpty(myHousesDescriptionofGoods) && myHousesDescriptionofGoods.Length > 2000)
            {
                myHousesDescriptionofGoods = myHousesDescriptionofGoods.Substring(0, 2000);
            }

            shipmentPM.HousesDescriptionofGoods = myHousesDescriptionofGoods;
        }

        private void MapAnalyzerConcurrencyFields(ShipmentPM shipmentPM)
        {
            shipmentPM.FHLStatusCode_Original = shipmentPM.FHLStatusCode;
            shipmentPM.FHLStatusDate_Original = shipmentPM.FHLStatusDate;
            shipmentPM.FWBStatusCode_Original = shipmentPM.FWBStatusCode;
            shipmentPM.FWBStatusDate_Original = shipmentPM.FWBStatusDate;
            shipmentPM.CarrierLastStatusCode_Original = shipmentPM.CarrierLastStatusCode;
            shipmentPM.CarrierLastStatusDate_Original = shipmentPM.CarrierLastStatusDate;
            shipmentPM.NumberOfPackages_Original = shipmentPM.NumberOfPackages;
            shipmentPM.GrossWeight_Original = shipmentPM.GrossWeight;
            shipmentPM.ChargeableWeight_Original = shipmentPM.ChargeableWeight;
            shipmentPM.GrossWeightUnitCode_Original = shipmentPM.GrossWeightUnitCode;
            shipmentPM.MAN_FromPortId_Original = shipmentPM.MainCarriageFromPortId;
            shipmentPM.MainCarriageToPortId_Original = shipmentPM.MainCarriageToPortId;
            shipmentPM.TR1_ToPortId_Original = shipmentPM.Transshipment1ToPortId;
            shipmentPM.TR2_ToPortId_Original = shipmentPM.Transshipment2ToPortId;
            shipmentPM.TR3_ToPortId_Original = shipmentPM.Transshipment3ToPortId;
            shipmentPM.FIN_PortId_Original = shipmentPM.MainCarriageFinalDestinationPortId;
            shipmentPM.MainCarriageATD_Original = shipmentPM.MainCarriageATD;
            shipmentPM.MainCarriageETD_Original = shipmentPM.MainCarriageETD;
            shipmentPM.MainCarriageSTD_Original = shipmentPM.MainCarriageSTD;
            shipmentPM.MainCarriageATA_Original = shipmentPM.MainCarriageATA;
            shipmentPM.MainCarriageETA_Original = shipmentPM.MainCarriageETA;
            shipmentPM.MainCarriageSTA_Original = shipmentPM.MainCarriageSTA;
            shipmentPM.Transshipment1ATD_Original = shipmentPM.Transshipment1ATD;
            shipmentPM.Transshipment1ETD_Original = shipmentPM.Transshipment1ETD;
            shipmentPM.Transshipment1STD_Original = shipmentPM.Transshipment1STD;
            shipmentPM.Transshipment1ATA_Original = shipmentPM.Transshipment1ATA;
            shipmentPM.Transshipment1ETA_Original = shipmentPM.Transshipment1ETA;
            shipmentPM.Transshipment1STA_Original = shipmentPM.Transshipment1STA;
            shipmentPM.Transshipment2ATD_Original = shipmentPM.Transshipment2ATD;
            shipmentPM.Transshipment2ETD_Original = shipmentPM.Transshipment2ETD;
            shipmentPM.Transshipment2STD_Original = shipmentPM.Transshipment2STD;
            shipmentPM.Transshipment2ATA_Original = shipmentPM.Transshipment2ATA;
            shipmentPM.Transshipment2ETA_Original = shipmentPM.Transshipment2ETA;
            shipmentPM.Transshipment2STA_Original = shipmentPM.Transshipment2STA;
            shipmentPM.Transshipment3ATD_Original = shipmentPM.Transshipment3ATD;
            shipmentPM.Transshipment3ETD_Original = shipmentPM.Transshipment3ETD;
            shipmentPM.Transshipment3STD_Original = shipmentPM.Transshipment3STD;
            shipmentPM.Transshipment3ATA_Original = shipmentPM.Transshipment3ATA;
            shipmentPM.Transshipment3ETA_Original = shipmentPM.Transshipment3ETA;
            shipmentPM.Transshipment3STA_Original = shipmentPM.Transshipment3STA;
            shipmentPM.OnCarriageATD_Original = shipmentPM.OnCarriageATD;
            shipmentPM.OnCarriageETD_Original = shipmentPM.OnCarriageETD;
            shipmentPM.OnCarriageATA_Original = shipmentPM.OnCarriageATA;
            shipmentPM.OnCarriageETA_Original = shipmentPM.OnCarriageETA;
            shipmentPM.PreCarriageATA_Original = shipmentPM.PreCarriageATA;
            shipmentPM.PreCarriageETA_Original = shipmentPM.PreCarriageETA;
            shipmentPM.PreCarriageATD_Original = shipmentPM.PreCarriageATD;
            shipmentPM.PreCarriageETD_Original = shipmentPM.PreCarriageETD;
            shipmentPM.OnForwardingATD_Original = shipmentPM.OnForwardingATD;
            shipmentPM.OnForwardingETD_Original = shipmentPM.OnForwardingETD;
            shipmentPM.OnForwardingATA_Original = shipmentPM.OnForwardingATA;
            shipmentPM.OnForwardingETA_Original = shipmentPM.OnForwardingETA;
            shipmentPM.PreForwardingATA_Original = shipmentPM.PreForwardingATA;
            shipmentPM.PreForwardingETA_Original = shipmentPM.PreForwardingETA;
            shipmentPM.PreForwardingATD_Original = shipmentPM.PreForwardingATD;
            shipmentPM.PreForwardingETD_Original = shipmentPM.PreForwardingETD;
            shipmentPM.INTTRABookingStatusCode_Original = shipmentPM.INTTRABookingStatusCode;
            shipmentPM.BookingConfirmedBy_Original = shipmentPM.BookingConfirmedBy;
            shipmentPM.BookingConfNumber_Original = shipmentPM.BookingConfirmationNumber;
            shipmentPM.MAN_CarrierNumber_Original = shipmentPM.MainCarriageCarrierNumber;
        }

        private void MapINTTRABookingXMLFields(ShipmentPM shipmentPM)
        {
            this.ReadINTTRABookingXMLVoyage(shipmentPM);
            this.ReadINTTRABookingXMLVessel(shipmentPM);
            this.ReadINTTRABookingXMLDates(shipmentPM);
            this.ReadINTTRABookingXMLShippingLine(shipmentPM);
        }

        private void ReadINTTRABookingXMLVoyage(ShipmentPM shipmentPM)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(shipmentPM.INTTRALastBookingResponse);
            XmlNodeList xnList = xmlDoc.SelectNodes("//ConveyanceInformation");

            foreach (XmlNode xn in xnList)
            {
                foreach (XmlNode item in xn.ChildNodes)
                {
                    if (item.Attributes != null &&  item.Attributes["Type"] != null && item.Attributes["Type"].Value == "VoyageNumber")
                    {
                        shipmentPM.INTTRABookingResponse_Voyage = item.FirstChild.InnerText;
                    }
                }
            }
        }
        private void ReadINTTRABookingXMLVessel(ShipmentPM shipmentPM)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(shipmentPM.INTTRALastBookingResponse);
            XmlNodeList xnList = xmlDoc.SelectNodes("//ConveyanceInformation");

            foreach (XmlNode xn in xnList)
            {

                if (xn.ChildNodes != null)
                {
                    foreach (XmlNode item in xn.ChildNodes)
                    {
                        if (item.Attributes != null && item.Attributes["Type"] != null && item.Attributes["Type"].Value == "VesselName")
                        {
                            shipmentPM.INTTRABookingResponse_Vessel = item.FirstChild.InnerText;
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(shipmentPM.INTTRABookingResponse_Vessel))
            {
                this.GetSingleVesselByName(shipmentPM);
            }
        }

        private void GetSingleVesselByName(ShipmentPM shipmentPM)
        {
            VesselRepository vesselRepository = new VesselRepository(shipmentPM.Tenant);
            var vessel = vesselRepository.GetSingleVesselByName(shipmentPM.INTTRABookingResponse_Vessel, shipmentPM.Tenant);
            if(vessel != null)
            {
                shipmentPM.INTTRABookingResponse_VesselId = vessel.Id;
            }
        }

        private void ReadINTTRABookingXMLDates(ShipmentPM shipmentPM)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(shipmentPM.INTTRALastBookingResponse);
            var path = "//Location";
            XmlNodeList xnList = xmlDoc.SelectNodes(path);
            ICommonDataContext commonContext = CommonDataContext.GetContext(shipmentPM.Tenant);
            PortRepository portsRep = new PortRepository(commonContext);
            foreach (XmlNode xn in xnList)
            {
                if (xn["Type"] != null && xn["Type"].InnerText == "PortOfLoad")
                {
                    Port port = portsRep.GetSinglePortIdByCombinedCode(xn["Identifier"].InnerText, shipmentPM.Tenant);
                    if (port != null)
                    {
                        shipmentPM.INTTRABookingResponse_POFPort = port.Id;
                        shipmentPM.INTTRABookingResponse_POFPortCode = port.Code;
                        shipmentPM.INTTRABookingResponse_POFCCode = port.Country != null ? port.Country.Code : "";
                        shipmentPM.INTTRABookingResponse_POFCName = port.Country != null ? port.Country.EnglishName : "";
                    }
                    shipmentPM.INTTRABookingResponse_POLDate = xn["DateTime"] != null ? DateTime.Parse(xn["DateTime"].InnerText) : shipmentPM.INTTRABookingResponse_POLDate;
                }

                if (xn["Type"] != null && xn["Type"].InnerText == "PortOfDischarge")
                {
                    Port port = portsRep.GetSinglePortIdByCombinedCode(xn["Identifier"].InnerText, shipmentPM.Tenant);
                    if (port != null)
                    {
                        shipmentPM.INTTRABookingResponse_PODPort = port.Id;
                        shipmentPM.INTTRABookingResponse_PODPortCode = port.Code;
                        shipmentPM.INTTRABookingResponse_PODCCode = port.Country != null ? port.Country.Code : "";
                        shipmentPM.INTTRABookingResponse_PODCName = port.Country != null ? port.Country.EnglishName : "";
                    }
                    shipmentPM.INTTRABookingResponse_PODDate = xn["DateTime"] != null ? DateTime.Parse(xn["DateTime"].InnerText) : shipmentPM.INTTRABookingResponse_PODDate;
                }
            }
        }
        private void ReadINTTRABookingXMLShippingLine(ShipmentPM shipmentPM)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(shipmentPM.INTTRALastBookingResponse);
            var path = "//Party";
            XmlNodeList xnList = xmlDoc.SelectNodes(path);
            foreach (XmlNode xn in xnList)
            {
                if (xn["Role"] != null && xn["Role"].InnerText == "Carrier")
                {
                    shipmentPM.INTTRABookingResponse_ShippingLine = xn["Identifier"] != null ? xn["Identifier"].InnerText : "";
                }
            }
        }

        public ShipmentPM MapShipmentToShipmentPMForMobile(ShipmentPM shipmentPM, Shipment shipment, IQueryable<ShipmentMasterData> shipmentMasterDataList, ShipmentMasterData masterData, bool withComposition)
        {
            int tenant = shipment.Tenant;
            ICommonDataContext commonContext = CommonDataContext.GetContext(shipment.Tenant);
            IWebFreightContext webFreightContext = WebFreightContext.GetContext(shipment.Tenant);
            PortRepository portsRep = new PortRepository(commonContext);
            TransportModeRepository transmodeRep = new TransportModeRepository(webFreightContext);
            VesselRepository vesselRep = new VesselRepository(commonContext);
            ShipmentLevelRepository shipmentLevelRep = new ShipmentLevelRepository(repository.context);
            ShipmentPickUpDeliveryRepository shipmentPickUpDeliveryRepository = new ShipmentPickUpDeliveryRepository(repository.context);
            ShipmentDeliveryQuery shipmentDeliveryQuery = new ShipmentDeliveryQuery(shipmentPickUpDeliveryRepository);
            ShipmentPickUpQuery shipmentPickUpQuery = new ShipmentPickUpQuery(shipmentPickUpDeliveryRepository);
            DirectionRepository directionRep = new DirectionRepository(webFreightContext);

            Currency awbCurrency = CurrencyRepository.GetSingleCurrency(shipment.AWBCurrencyId, shipment.Tenant, true);
            ShipmentLevel shipmentLevel = shipmentLevelRep.GetSingleShipmentLevel(shipment.ShipmentLevelCode);
            TransportMode transportmode = transmodeRep.GetSingleTransportMode(shipment.TransportModeId);
            EntityStatus status = EntityStatusRepository.GetSingleEntityStatus(shipment.StatusId, shipment.Tenant, true);
            Direction direction = directionRep.GetSingleDirection(shipment.DirectionId);

            if (masterData == null)
            {
                if (shipment.MasterShipmentDataId != null && shipmentMasterDataList != null)
                {
                    masterData = (from a in shipmentMasterDataList where a.Id == shipment.MasterShipmentDataId select a).FirstOrDefault();
                }
            }

            PortQuery portQuery = new PortQuery(portsRep);
            bool isInlandDomesticShipment = (shipment.DirectionId == "D" && shipment.TransportModeId == "I");

            #region if (masterData != null)
            if (masterData != null)
            {
                shipmentPM.Master = masterData.Master;
                shipmentPM.LongMaster = EntityFieldsHelper.GetLongMasterField(shipment, masterData);
                shipmentPM.MasterShipmentNumber = masterData.MasterShipmentNumber;
                shipmentPM.MainCarriageCarrierNumber = masterData.MainCarriageCarrierNumber;
                shipmentPM.MainCarriageETD = masterData.MainCarriageETD;
                shipmentPM.MainCarriageETA = masterData.MainCarriageETA;
                shipmentPM.MainCarriageATD = masterData.MainCarriageATD;
                shipmentPM.MainCarriageATA = masterData.MainCarriageATA;

                #region Carrier

                shipmentPM.MainCarriageCarrierId = masterData.MainCarriageCarrierId;

                if (!string.IsNullOrEmpty(masterData.MainCarriageCarrierId))
                {
                    Card cardObject = CardRepository.GetSingleCard(masterData.MainCarriageCarrierId, shipment.Tenant, true);

                    if (cardObject != null)
                    {
                        shipmentPM.MainCarriageCarrierCode = cardObject.Code;
                        shipmentPM.MainCarriageCarrierName = cardObject.EnglishName;
                        shipmentPM.MainCarriageCarrierWebSite = cardObject.Website;

                        AddressRepository addressRepository = new AddressRepository(tenant);
                        Address address = addressRepository.GetSingleAddressByCardIdAndTypeId(cardObject.Id, "M", tenant);
                        if (address != null)
                        {
                            shipmentPM.MainCarriageCarrierAddressId = address.Id;
                        }
                    }
                }
                #endregion

                if (!isInlandDomesticShipment)
                {
                    PortPM mainCarriageFromPort = portQuery.GetSinglePM(masterData.MainCarriageFromPortId, masterData.Tenant);
                    shipmentPM.FromPortName = mainCarriageFromPort.EnglishName;
                    shipmentPM.MainCarriageFromPortCode = mainCarriageFromPort.Code;
                    shipmentPM.MainCarriageFromPortName = mainCarriageFromPort.EnglishName;
                    shipmentPM.MainCarriageFromPortCountryCode = mainCarriageFromPort.CountryCode;

                    PortPM mainCarriageToPort = portQuery.GetSinglePM(masterData.MainCarriageToPortId, masterData.Tenant);
                    shipmentPM.ToPortName = mainCarriageToPort.EnglishName;
                    shipmentPM.ToPortCountry = mainCarriageToPort.CountryName;
                    shipmentPM.MainCarriageToPortCode = mainCarriageToPort.Code;
                    shipmentPM.MainCarriageToPortName = mainCarriageToPort.EnglishName;
                    shipmentPM.MainCarriageToPortCountryCode = mainCarriageToPort.CountryCode;

                    #region Transshipment 1
                    shipmentPM.Transshipment1ATA = masterData.Transshipment1ATA;
                    shipmentPM.Transshipment1ATD = masterData.Transshipment1ATD;
                    shipmentPM.Transshipment1ETA = masterData.Transshipment1ETA;
                    shipmentPM.Transshipment1ETD = masterData.Transshipment1ETD;


                    shipmentPM.Transshipment1FromPortId = masterData.Transshipment1FromPortId;
                    shipmentPM.Transshipment1ToPortId = masterData.Transshipment1ToPortId;

                    shipmentPM.Transshipment1CarrierNumber = masterData.Transshipment1CarrierNumber;

                    shipmentPM.MainCarriageVesselId = masterData.MainCarriageVesselId;
                    shipmentPM.Transshipment1VesselId = masterData.Transshipment1VesselId;
                    shipmentPM.Transshipment2VesselId = masterData.Transshipment2VesselId;
                    shipmentPM.Transshipment3VesselId = masterData.Transshipment3VesselId;

                    if (!string.IsNullOrEmpty(shipmentPM.MainCarriageVesselId))
                    {
                        Vessel vesselEntity = vesselRep.GetSingleVessel(masterData.MainCarriageVesselId, masterData.Tenant);
                        if (vesselEntity != null)
                        {
                            shipmentPM.MainCarriageVesselName = vesselEntity.EnglishName;
                        }
                    }


                    if (!string.IsNullOrEmpty(shipmentPM.Transshipment1VesselId))
                    {
                        Vessel vesselEntity = vesselRep.GetSingleVessel(masterData.Transshipment1VesselId, masterData.Tenant);
                        if (vesselEntity != null)
                        {
                            shipmentPM.Transshipment1VesselName = vesselEntity.EnglishName;
                        }
                    }

                    if (!string.IsNullOrEmpty(shipmentPM.Transshipment2VesselId))
                    {
                        Vessel vesselEntity = vesselRep.GetSingleVessel(masterData.Transshipment2VesselId, masterData.Tenant);
                        if (vesselEntity != null)
                        {
                            shipmentPM.Transshipment2VesselName = vesselEntity.EnglishName;
                        }
                    }

                    if (!string.IsNullOrEmpty(shipmentPM.Transshipment3VesselId))
                    {
                        Vessel vesselEntity = vesselRep.GetSingleVessel(masterData.Transshipment3VesselId, masterData.Tenant);
                        if (vesselEntity != null)
                        {
                            shipmentPM.Transshipment3VesselName = vesselEntity.EnglishName;
                        }
                    }

                    if (!string.IsNullOrEmpty(shipmentPM.Transshipment1FromPortId))
                    {
                        PortPM transshipment1FromPort = portQuery.GetSinglePM(masterData.Transshipment1FromPortId, masterData.Tenant);
                        if (transshipment1FromPort != null)
                        {
                            shipmentPM.Transshipment1FromPortCode = transshipment1FromPort.Code;
                            shipmentPM.Transshipment1FromPortName = transshipment1FromPort.EnglishName;
                            shipmentPM.Transshipment1FromPortCountryCode = transshipment1FromPort.CountryCode;
                        }
                    }

                    if (!string.IsNullOrEmpty(shipmentPM.Transshipment1ToPortId))
                    {
                        PortPM transshipment1ToPort = portQuery.GetSinglePM(masterData.Transshipment1ToPortId, masterData.Tenant);
                        if (transshipment1ToPort != null)
                        {
                            shipmentPM.Transshipment1ToPortCode = transshipment1ToPort.Code;
                            shipmentPM.Transshipment1ToPortName = transshipment1ToPort.EnglishName;
                            shipmentPM.Transshipment1ToPortCountryCode = transshipment1ToPort.CountryCode;
                        }
                    }

                    if (!string.IsNullOrEmpty(masterData.Transshipment1CarrierId))
                    {
                        Card cardObject = CardRepository.GetSingleCard(masterData.Transshipment1CarrierId, shipment.Tenant, true);
                        if (cardObject != null)
                        {
                            shipmentPM.Transshipment1CarrierCode = cardObject.Code;
                            shipmentPM.Transshipment1CarrierName = cardObject.EnglishName;
                        }
                    }
                    #endregion

                    #region Transshipment 2
                    shipmentPM.Transshipment2ATA = masterData.Transshipment2ATA;
                    shipmentPM.Transshipment2ATD = masterData.Transshipment2ATD;
                    shipmentPM.Transshipment2ETA = masterData.Transshipment2ETA;
                    shipmentPM.Transshipment2ETD = masterData.Transshipment2ETD;
                    shipmentPM.Transshipment2FromPortId = masterData.Transshipment2FromPortId;
                    shipmentPM.Transshipment2ToPortId = masterData.Transshipment2ToPortId;
                    shipmentPM.Transshipment2CarrierNumber = masterData.Transshipment2CarrierNumber;

                    if (!string.IsNullOrEmpty(shipmentPM.Transshipment2FromPortId))
                    {
                        PortPM transshipment2FromPort = portQuery.GetSinglePM(masterData.Transshipment2FromPortId, masterData.Tenant);
                        if (transshipment2FromPort != null)
                        {
                            shipmentPM.Transshipment2FromPortCode = transshipment2FromPort.Code;
                            shipmentPM.Transshipment2FromPortName = transshipment2FromPort.EnglishName;
                            shipmentPM.Transshipment2FromPortCountryCode = transshipment2FromPort.CountryCode;
                        }
                    }

                    if (!string.IsNullOrEmpty(shipmentPM.Transshipment2ToPortId))
                    {
                        PortPM transshipment2ToPort = portQuery.GetSinglePM(masterData.Transshipment2ToPortId, masterData.Tenant);
                        if (transshipment2ToPort != null)
                        {
                            shipmentPM.Transshipment2ToPortCode = transshipment2ToPort.Code;
                            shipmentPM.Transshipment2ToPortName = transshipment2ToPort.EnglishName;
                            shipmentPM.Transshipment2ToPortCountryCode = transshipment2ToPort.CountryCode;
                        }
                    }

                    if (!string.IsNullOrEmpty(masterData.Transshipment2CarrierId))
                    {
                        Card cardObject = CardRepository.GetSingleCard(masterData.Transshipment2CarrierId, shipment.Tenant, true);
                        if (cardObject != null)
                        {
                            shipmentPM.Transshipment2CarrierCode = cardObject.Code;
                            shipmentPM.Transshipment2CarrierName = cardObject.EnglishName;
                        }
                    }
                    #endregion

                    #region Transshipment 3
                    shipmentPM.Transshipment3ATA = masterData.Transshipment3ATA;
                    shipmentPM.Transshipment3ATD = masterData.Transshipment3ATD;
                    shipmentPM.Transshipment3ETA = masterData.Transshipment3ETA;
                    shipmentPM.Transshipment3ETD = masterData.Transshipment3ETD;
                    shipmentPM.Transshipment3FromPortId = masterData.Transshipment3FromPortId;
                    shipmentPM.Transshipment3ToPortId = masterData.Transshipment3ToPortId;
                    shipmentPM.Transshipment3CarrierNumber = masterData.Transshipment3CarrierNumber;

                    if (!string.IsNullOrEmpty(shipmentPM.Transshipment3FromPortId))
                    {
                        PortPM transshipment3FromPort = portQuery.GetSinglePM(masterData.Transshipment3FromPortId, masterData.Tenant);
                        if (transshipment3FromPort != null)
                        {
                            shipmentPM.Transshipment3FromPortCode = transshipment3FromPort.Code;
                            shipmentPM.Transshipment3FromPortName = transshipment3FromPort.EnglishName;
                            shipmentPM.Transshipment3FromPortCountryCode = transshipment3FromPort.CountryCode;
                        }
                    }

                    if (!string.IsNullOrEmpty(shipmentPM.Transshipment3ToPortId))
                    {
                        PortPM transshipment3ToPort = portQuery.GetSinglePM(masterData.Transshipment3ToPortId, masterData.Tenant);
                        if (transshipment3ToPort != null)
                        {
                            shipmentPM.Transshipment3ToPortCode = transshipment3ToPort.Code;
                            shipmentPM.Transshipment3ToPortName = transshipment3ToPort.EnglishName;
                            shipmentPM.Transshipment3ToPortCountryCode = transshipment3ToPort.CountryCode;
                        }
                    }

                    if (!string.IsNullOrEmpty(masterData.Transshipment3CarrierId))
                    {
                        Card cardObject = CardRepository.GetSingleCard(masterData.Transshipment3CarrierId, shipment.Tenant, true);
                        if (cardObject != null)
                        {
                            shipmentPM.Transshipment3CarrierCode = cardObject.Code;
                            shipmentPM.Transshipment3CarrierName = cardObject.EnglishName;
                        }
                    }
                    #endregion

                    #region Pre carriage
                    if (!string.IsNullOrEmpty(masterData.PreCarriageFromPortId) || !string.IsNullOrEmpty(masterData.PreCarriageToPortId))
                    {
                        shipmentPM.HasPreCarriage = true;
                    }

                    PortPM precarriageFromPort = null;
                    PortPM precarriageToPort = null;
                    if (!string.IsNullOrEmpty(masterData.PreCarriageFromPortId))
                    {
                        precarriageFromPort = portQuery.GetSinglePM(masterData.PreCarriageFromPortId, masterData.Tenant);
                    }

                    if (!string.IsNullOrEmpty(masterData.PreCarriageToPortId))
                    {
                        precarriageToPort = portQuery.GetSinglePM(masterData.PreCarriageToPortId, masterData.Tenant);
                    }

                    shipmentPM.PreCarriageFromPortId = masterData.PreCarriageFromPortId;
                    shipmentPM.PreCarriageToPortId = masterData.PreCarriageToPortId;
                    shipmentPM.PreCarriageCarrierId = masterData.PreCarriageCarrierId;
                    shipmentPM.PreCarriageCarrierNumber = masterData.PreCarriageCarrierNumber;
                    shipmentPM.PreCarriageATA = masterData.PreCarriageATA;
                    shipmentPM.PreCarriageATD = masterData.PreCarriageATD;
                    shipmentPM.PreCarriageETA = masterData.PreCarriageETA;
                    shipmentPM.PreCarriageETD = masterData.PreCarriageETD;
                    shipmentPM.PreCarriageTransportModeId = masterData.PreCarriageTransportModeId;
                    shipmentPM.PreCarriageVesselId = masterData.PreCarriageVesselId;

                    if (precarriageFromPort != null)
                    {
                        shipmentPM.PreCarriageFromPortCode = precarriageFromPort.Code;
                        shipmentPM.PreCarriageFromPortName = precarriageFromPort.EnglishName;
                        shipmentPM.PreCarriageFromPortCountryCode = precarriageFromPort.CountryCode;
                        shipmentPM.PreCarriageFromPortCountryName = precarriageFromPort.CountryName;
                    }

                    if (precarriageToPort != null)
                    {
                        shipmentPM.PreCarriageToPortCode = precarriageToPort.Code;
                        shipmentPM.PreCarriageToPortName = precarriageToPort.EnglishName;
                        shipmentPM.PreCarriageToPortCountryCode = precarriageToPort.CountryCode;
                        shipmentPM.PreCarriageToPortCountryName = precarriageToPort.CountryName;
                    }

                    if (!string.IsNullOrEmpty(masterData.PreCarriageCarrierId))
                    {
                        Card cardObject = CardRepository.GetSingleCard(masterData.PreCarriageCarrierId, masterData.Tenant, true);
                        if (cardObject != null)
                        {
                            shipmentPM.PreCarriageCarrierCode = cardObject.Code;
                            shipmentPM.PreCarriageCarrierName = cardObject.EnglishName;
                            shipmentPM.PreCarriageCarrierWebSite = cardObject.Website;
                        }
                    }

                    if (!string.IsNullOrEmpty(masterData.PreCarriageVesselId))
                    {
                        Vessel vesselEntity = vesselRep.GetSingleVessel(masterData.PreCarriageVesselId, masterData.Tenant);
                        if (vesselEntity != null)
                        {
                            shipmentPM.PreCarriageVesselName = vesselEntity.EnglishName;
                        }
                    }
                    #endregion

                    #region On carriage
                    if (!string.IsNullOrEmpty(masterData.OnCarriageFromPortId) || !string.IsNullOrEmpty(masterData.OnCarriageToPortId))
                    {
                        shipmentPM.HasOnCarriage = true;
                    }

                    PortPM oncarriageFromPort = null;
                    PortPM oncarriageToPort = null;

                    if (!string.IsNullOrEmpty(masterData.OnCarriageFromPortId))
                    {
                        oncarriageFromPort = portQuery.GetSinglePM(masterData.OnCarriageFromPortId, masterData.Tenant);
                    }

                    if (!string.IsNullOrEmpty(masterData.OnCarriageToPortId))
                    {
                        oncarriageToPort = portQuery.GetSinglePM(masterData.OnCarriageToPortId, masterData.Tenant);
                    }

                    shipmentPM.OnCarriageAdditionalTransportModeCode = masterData.OnCarriageAdditionalTransportModeCode;
                    shipmentPM.SplitOnCarriage = masterData.SplitOnCarriage;
                    shipmentPM.OnCarriageFromPortId = masterData.OnCarriageFromPortId;
                    shipmentPM.OnCarriageToPortId = masterData.OnCarriageToPortId;
                    shipmentPM.OnCarriageCarrierId = masterData.OnCarriageCarrierId;
                    shipmentPM.OnCarriageCarrierNumber = masterData.OnCarriageCarrierNumber;
                    shipmentPM.OnCarriageATA = masterData.OnCarriageATA;
                    shipmentPM.OnCarriageATD = masterData.OnCarriageATD;
                    shipmentPM.OnCarriageETA = masterData.OnCarriageETA;
                    shipmentPM.OnCarriageETD = masterData.OnCarriageETD;
                    shipmentPM.OnCarriageTransportModeId = masterData.OnCarriageTransportModeId;
                    shipmentPM.OnCarriageVesselId = masterData.OnCarriageVesselId;

                    if (oncarriageFromPort != null)
                    {
                        shipmentPM.OnCarriageFromPortCode = oncarriageFromPort.Code;
                        shipmentPM.OnCarriageFromPortName = oncarriageFromPort.EnglishName;
                        shipmentPM.OnCarriageFromPortCountryCode = oncarriageFromPort.CountryCode;
                        shipmentPM.OnCarriageFromPortCountryName = oncarriageFromPort.CountryName;
                    }

                    if (oncarriageToPort != null)
                    {
                        shipmentPM.OnCarriageToPortCode = oncarriageToPort.Code;
                        shipmentPM.OnCarriageToPortName = oncarriageToPort.EnglishName;
                        shipmentPM.OnCarriageToPortCountryCode = oncarriageToPort.CountryCode;
                        shipmentPM.OnCarriageToPortCountryName = oncarriageToPort.CountryName;
                    }

                    if (!string.IsNullOrEmpty(masterData.OnCarriageCarrierId))
                    {
                        Card cardObject = CardRepository.GetSingleCard(masterData.OnCarriageCarrierId, masterData.Tenant, true);
                        if (cardObject != null)
                        {
                            shipmentPM.OnCarriageCarrierCode = cardObject.Code;
                            shipmentPM.OnCarriageCarrierName = cardObject.EnglishName;
                            shipmentPM.OnCarriageCarrierWebSite = cardObject.Website;
                        }
                    }

                    if (!string.IsNullOrEmpty(masterData.OnCarriageVesselId))
                    {
                        Vessel vesselEntity = vesselRep.GetSingleVessel(masterData.OnCarriageVesselId, masterData.Tenant);
                        if (vesselEntity != null)
                        {
                            shipmentPM.OnCarriageVesselName = vesselEntity.EnglishName;
                        }
                    }

                    #endregion
                }
            }
            #endregion

            #region else House Shipment Ports
            else
            {
                if (!isInlandDomesticShipment)
                {
                    PortPM fromPort = portQuery.GetSinglePM(shipment.FromPortId, shipment.Tenant);
                    shipmentPM.MainCarriageFromPortCode = fromPort.Code;
                    shipmentPM.MainCarriageFromPortName = fromPort.EnglishName;
                    shipmentPM.MainCarriageFromPortCountryCode = fromPort.CountryCode;
                    shipmentPM.FromPortName = fromPort.EnglishName;
                    PortPM toPort = portQuery.GetSinglePM(shipment.ToPortId, shipment.Tenant);
                    shipmentPM.MainCarriageToPortCode = toPort.Code;
                    shipmentPM.MainCarriageToPortName = toPort.EnglishName;
                    shipmentPM.MainCarriageToPortCountryCode = toPort.CountryCode;
                    shipmentPM.ToPortName = toPort.EnglishName;

                    #region Pre Forwarding
                    if (!string.IsNullOrEmpty(shipment.PreForwardingFromPortId) || !string.IsNullOrEmpty(shipment.PreForwardingToPortId))
                    {
                        shipmentPM.HasPreForwarding = true;
                    }

                    PortPM preForwardingFromPort = null;
                    PortPM preForwardingToPort = null;

                    if (!string.IsNullOrEmpty(shipment.PreForwardingFromPortId))
                    {
                        preForwardingFromPort = portQuery.GetSinglePM(shipment.PreForwardingFromPortId, shipment.Tenant);
                    }

                    if (!string.IsNullOrEmpty(shipment.PreForwardingToPortId))
                    {
                        preForwardingToPort = portQuery.GetSinglePM(shipment.PreForwardingToPortId, shipment.Tenant);
                    }

                    shipmentPM.PreForwardingFromPortId = shipment.PreForwardingFromPortId;
                    shipmentPM.PreForwardingToPortId = shipment.PreForwardingToPortId;
                    shipmentPM.PreForwardingCarrierId = shipment.PreForwardingCarrierId;
                    shipmentPM.PreForwardingCarrierNumber = shipment.PreForwardingCarrierNumber;
                    shipmentPM.PreForwardingATA = shipment.PreForwardingATA;
                    shipmentPM.PreForwardingATD = shipment.PreForwardingATD;
                    shipmentPM.PreForwardingETA = shipment.PreForwardingETA;
                    shipmentPM.PreForwardingETD = shipment.PreForwardingETD;
                    shipmentPM.PreForwardingTransportModeId = shipment.PreForwardingTransportModeId;
                    shipmentPM.PreForwardingVesselId = shipment.PreForwardingVesselId;

                    if (preForwardingFromPort != null)
                    {
                        shipmentPM.PreForwardingFromPortCode = preForwardingFromPort.Code;
                        shipmentPM.PreForwardingFromPortName = preForwardingFromPort.EnglishName;
                        shipmentPM.PreForwardingFromPortCountryCode = preForwardingFromPort.CountryCode;
                        shipmentPM.PreForwardingFromPortCountryName = preForwardingFromPort.CountryName;
                    }

                    if (preForwardingToPort != null)
                    {
                        shipmentPM.PreForwardingToPortCode = preForwardingToPort.Code;
                        shipmentPM.PreForwardingToPortName = preForwardingToPort.EnglishName;
                        shipmentPM.PreForwardingToPortCountryCode = preForwardingToPort.CountryCode;
                        shipmentPM.PreForwardingToPortCountryName = preForwardingToPort.CountryName;
                    }

                    if (!string.IsNullOrEmpty(shipment.PreForwardingCarrierId))
                    {
                        Card cardObject = CardRepository.GetSingleCard(shipment.PreForwardingCarrierId, shipment.Tenant, true);
                        if (cardObject != null)
                        {
                            shipmentPM.PreForwardingCarrierCode = cardObject.Code;
                            shipmentPM.PreForwardingCarrierName = cardObject.EnglishName;
                            shipmentPM.PreForwardingCarrierWebSite = cardObject.Website;
                        }
                    }

                    if (!string.IsNullOrEmpty(shipment.PreForwardingVesselId))
                    {
                        Vessel vesselEntity = vesselRep.GetSingleVessel(shipment.PreForwardingVesselId, shipment.Tenant);
                        if (vesselEntity != null)
                        {
                            shipmentPM.PreForwardingVesselName = vesselEntity.EnglishName;
                        }
                    }
                    #endregion

                    #region On Forwarding
                    if (!string.IsNullOrEmpty(shipment.OnForwardingFromPortId) || !string.IsNullOrEmpty(shipment.OnForwardingToPortId))
                    {
                        shipmentPM.HasOnForwarding = true;
                    }

                    PortPM onForwardingFromPort = null;
                    PortPM onForwardingToPort = null;

                    if (!string.IsNullOrEmpty(shipment.OnForwardingFromPortId))
                    {
                        onForwardingFromPort = portQuery.GetSinglePM(shipment.OnForwardingFromPortId, shipment.Tenant);
                    }

                    if (!string.IsNullOrEmpty(shipment.OnForwardingToPortId))
                    {
                        onForwardingToPort = portQuery.GetSinglePM(shipment.OnForwardingToPortId, shipment.Tenant);
                    }

                    shipmentPM.OnForwardingAdditionalTransportModeCode = shipment.OnForwardingAdditionalTransportModeCode;
                    shipmentPM.SplitOnForwarding = shipment.SplitOnForwarding;
                    shipmentPM.OnForwardingFromPortId = shipment.OnForwardingFromPortId;
                    shipmentPM.OnForwardingToPortId = shipment.OnForwardingToPortId;
                    shipmentPM.OnForwardingCarrierId = shipment.OnForwardingCarrierId;
                    shipmentPM.OnForwardingCarrierNumber = shipment.OnForwardingCarrierNumber;
                    shipmentPM.OnForwardingATA = shipment.OnForwardingATA;
                    shipmentPM.OnForwardingATD = shipment.OnForwardingATD;
                    shipmentPM.OnForwardingETA = shipment.OnForwardingETA;
                    shipmentPM.OnForwardingETD = shipment.OnForwardingETD;
                    shipmentPM.OnForwardingTransportModeId = shipment.OnForwardingTransportModeId;
                    shipmentPM.OnForwardingVesselId = shipment.OnForwardingVesselId;

                    if (onForwardingFromPort != null)
                    {
                        shipmentPM.OnForwardingFromPortCode = onForwardingFromPort.Code;
                        shipmentPM.OnForwardingFromPortName = onForwardingFromPort.EnglishName;
                        shipmentPM.OnForwardingFromPortCountryCode = onForwardingFromPort.CountryCode;
                        shipmentPM.OnForwardingFromPortCountryName = onForwardingFromPort.CountryName;
                    }

                    if (onForwardingToPort != null)
                    {
                        shipmentPM.OnForwardingToPortCode = onForwardingToPort.Code;
                        shipmentPM.OnForwardingToPortName = onForwardingToPort.EnglishName;
                        shipmentPM.OnForwardingToPortCountryCode = onForwardingToPort.CountryCode;
                        shipmentPM.OnForwardingToPortCountryName = onForwardingToPort.CountryName;
                    }

                    if (!string.IsNullOrEmpty(shipment.OnForwardingCarrierId))
                    {
                        Card cardObject = CardRepository.GetSingleCard(shipment.OnForwardingCarrierId, shipment.Tenant, true);
                        if (cardObject != null)
                        {
                            shipmentPM.OnForwardingCarrierCode = cardObject.Code;
                            shipmentPM.OnForwardingCarrierName = cardObject.EnglishName;
                            shipmentPM.OnForwardingCarrierWebSite = cardObject.Website;
                        }
                    }

                    if (!string.IsNullOrEmpty(shipment.OnForwardingVesselId))
                    {
                        Vessel vesselEntity = vesselRep.GetSingleVessel(shipment.OnForwardingVesselId, shipment.Tenant);
                        if (vesselEntity != null)
                        {
                            shipmentPM.OnForwardingVesselName = vesselEntity.EnglishName;
                        }
                    }
                    #endregion
                }
            }
            #endregion

            #region Partners
            shipmentPM.CustomerId = shipment.CustomerId;

            #region FreightForwarder
            shipmentPM.FreightForwarderId = shipment.FreightForwarderId;
            shipmentPM.FreightForwarderAddressId = shipment.FreightForwarderAddressId;
            shipmentPM.FreightForwarderContactId = shipment.FreightForwarderContactId;
            shipmentPM.FreightForwarderReference = shipment.FreightForwarderReference;

            #endregion

            shipmentPM.ShipperId = shipment.ShipperId;
            shipmentPM.ShipperName = shipment.ShipperName;
            shipmentPM.ShipperAddressId = shipment.ShipperAddressId;
            shipmentPM.ShipperContactId = shipment.ShipperContactId;
            shipmentPM.ShipperReference1 = shipment.ShipperReference1;
            shipmentPM.ShipperReference2 = shipment.ShipperReference2;

            #region Consignee
            shipmentPM.ConsigneeId = shipment.ConsigneeId;
            shipmentPM.ConsigneeName = shipment.ConsigneeName;
            shipmentPM.ConsigneeAddressId = shipment.ConsigneeAddressId;
            shipmentPM.ConsigneeContactId = shipment.ConsigneeContactId;
            shipmentPM.ConsigneeReference1 = shipment.ConsigneeReference1;
            shipmentPM.ConsigneeReference2 = shipment.ConsigneeReference2;

            #endregion

            #region Agent
            shipmentPM.AgentId = shipment.AgentId;
            shipmentPM.AgentComputed = shipment.AgentComputed;
            shipmentPM.AgentAddressId = shipment.AgentAddressId;
            shipmentPM.AgentContactId = shipment.AgentContactId;
            shipmentPM.AgentReference1 = shipment.AgentReference1;
            shipmentPM.AgentReference2 = shipment.AgentReference2;

            #endregion

            #region IssuingCarrierAgent
            shipmentPM.IssuingCarrierAgentId = shipment.IssuingCarrierAgentId;
            shipmentPM.IssuingCarrierAddressId = shipment.IssuingCarrierAddressId;


            #endregion

            #region CustomAgentExport
            shipmentPM.CustomAgentExportId = shipment.CustomAgentExportId;
            shipmentPM.CustomAgentExportAddressId = shipment.CustomAgentExportAddressId;
            shipmentPM.CustomAgentExportContactId = shipment.CustomAgentExportContactId;
            shipmentPM.CustomAgentExportReference = shipment.CustomAgentExportReference;

            #endregion

            #region CustomAgentImport
            shipmentPM.CustomAgentImportId = shipment.CustomAgentImportId;
            shipmentPM.CustomAgentImportAddressId = shipment.CustomAgentImportAddressId;
            shipmentPM.CustomAgentImportContactId = shipment.CustomAgentImportContactId;
            shipmentPM.CustomAgentImportReference = shipment.CustomAgentImportReference;

            #endregion

            #region Notify1
            shipmentPM.Notify1Id = shipment.Notify1Id;
            shipmentPM.Notify1AddressId = shipment.Notify1AddressId;
            shipmentPM.Notify1ContactId = shipment.Notify1ContactId;

            #endregion

            #region Notify2
            shipmentPM.Notify2Id = shipment.Notify2Id;
            shipmentPM.Notify2AddressId = shipment.Notify2AddressId;
            shipmentPM.Notify2ContactId = shipment.Notify2ContactId;

            #endregion

            #region ShipperNotExporter
            shipmentPM.ShipperNotExporterId = shipment.ShipperNotExporterId;
            shipmentPM.ShipperNotExporterAddressId = shipment.ShipperNotExporterAddressId;
            shipmentPM.ShipperNotExporterContactId = shipment.ShipperNotExporterContactId;

            #endregion

            #region ConsigneeNotImporter
            shipmentPM.ConsigneeNotImporterId = shipment.ConsigneeNotImporterId;
            shipmentPM.ConsigneeNotImporterAddressId = shipment.ConsigneeNotImporterAddressId;
            shipmentPM.ConsigneeNotImporterContactId = shipment.ConsigneeNotImporterContactId;

            #endregion

            #region CustomClearancePoint
            shipmentPM.CustomClearancePointId = shipment.CustomClearancePointId;
            shipmentPM.CustomClearancePointAddressId = shipment.CustomClearancePointAddressId;
            shipmentPM.CustomClearancePointContactId = shipment.CustomClearancePointContactId;
            shipmentPM.CustomClearancePointReference1 = shipment.CustomClearancePointReference1;

            #endregion

            #region Coloader
            shipmentPM.ColoaderId = shipment.ColoaderId;
            shipmentPM.ColoaderAddressId = shipment.ColoaderAddressId;
            shipmentPM.ColoaderContactId = shipment.ColoaderContactId;
            shipmentPM.ColoaderReference1 = shipment.ColoaderReference1;

            #endregion

            #region Freelancer
            shipmentPM.FreelancerId = shipment.FreelancerId;
            shipmentPM.FreelancerAddressId = shipment.FreelancerAddressId;
            shipmentPM.FreelancerContactId = shipment.FreelancerContactId;


            #endregion

            #region Consolidator
            shipmentPM.ConsolidatorId = shipment.ConsolidatorId;
            shipmentPM.ConsolidatorAddressId = shipment.ConsolidatorAddressId;
            shipmentPM.ConsolidatorContactId = shipment.ConsolidatorContactId;
            shipmentPM.ConsolidatorReference = shipment.ConsolidatorReference;

            #endregion

            #region ReleasingAgent
            shipmentPM.ReleasingAgentId = shipment.ReleasingAgentId;
            shipmentPM.ReleasingAgentAddressId = shipment.ReleasingAgentAddressId;
            shipmentPM.ReleasingAgentContactId = shipment.ReleasingAgentContactId;
            shipmentPM.ReleasingAgentReference1 = shipment.ReleasingAgentReference1;
            shipmentPM.ReleasingAgentReference2 = shipment.ReleasingAgentReference2;

            #endregion
            #endregion

            #region Properties

            shipmentPM.StatusName = shipment.EntityStatus.Name;
            shipmentPM.ComputedStatusName = shipment.ComputedEntityStatus != null ? shipment.ComputedEntityStatus.Name : "";
            shipmentPM.ComputedStatusDate = shipment.ComputedStatusDate;
            shipmentPM.ForeignPartnerCountryCode = shipment.ForeignPartnerCountryCode;
            shipmentPM.ComputedStatusId = shipment.ComputedStatusId;

            shipmentPM.TransportModeId = shipment.TransportModeId;

            shipmentPM.TransportModeName = transportmode.Name;
            shipmentPM.DirectionName = direction.Name;
            shipmentPM.ForeignPartnerCountryCode = shipment.ForeignPartnerCountryCode;

            /* Bills*/
            shipmentPM.House = shipment.House;


            /* Ayman */
            shipmentPM.Id = shipment.Id;


            shipment.FreelancerId = shipment.FreelancerId;
            shipment.FreelancerAddressId = shipment.FreelancerAddressId;
            shipment.FreelancerContactId = shipment.FreelancerContactId;

            shipmentPM.ChargeableWeightInKG = shipment.ChargeableWeightInKG;
            shipmentPM.GrossWeightEdited = shipment.GrossWeightEdited;

            shipmentPM.VolumeUnitCode = shipment.VolumeUnitCode;
            shipmentPM.CurrentUserId = shipment.CurrentUserId;

            shipmentPM.DirectionId = shipment.DirectionId;
            shipmentPM.AccountManagerUserId = shipment.AccountManagerUserId;
            shipmentPM.GrossWeightInKG = shipment.GrossWeightInKG;
            shipmentPM.GrossWeightPerStorageDays = shipment.GrossWeightPerStorageDays;

            shipmentPM.GrossWeight = shipment.GrossWeight;
            shipmentPM.ChargeableWeight = shipment.ChargeableWeight;
            shipmentPM.GrossWeightPerTon = shipment.GrossWeightPerTon;

            shipmentPM.CreateDateTime = shipment.CreateDateTime;


            shipmentPM.ShipmentNumber = shipment.ShipmentNumber;
            shipmentPM.ShipmentTypeId = shipment.ShipmentTypeId;



            shipmentPM.ExceptionDescription = shipment.ExceptionDescription;
            shipmentPM.ExceptionResolvedDescription = shipment.ExceptionResolvedDescription;
            shipmentPM.LastExceptionDescription = shipment.LastExceptionDescription;
            shipmentPM.ExceptionDate = shipment.ExceptionDate;
            shipmentPM.HasException = shipment.HasException;
            shipmentPM.CustomConnectToShipment = shipment.CustomConnectToShipment;
            shipmentPM.IsManifestSentToAgent = shipment.IsManifestSentToAgent;
            shipmentPM.AgentSharedManifestRef = shipment.AgentSharedManifestRef;
            shipmentPM.ManifestLastSharingDate = shipment.ManifestLastSharingDate;

            string str = string.Empty;
            if (shipment.ShipmentType != null)
            {
                str = shipment.ShipmentType.Name;
            }

            str = string.IsNullOrEmpty(str) ? shipmentLevel.Name : str + " " + shipmentLevel.Name;

            shipmentPM.Tenant = shipment.Tenant;



            shipmentPM.DimensionsUnitCode = shipment.DimensionsUnitCode;
            shipmentPM.GrossWeightUnitCode = shipment.GrossWeightUnitCode;
            shipmentPM.ChargeableWeightUnitCode = shipment.ChargeableWeightUnitCode;

            shipmentPM.Volume = shipment.Volume;

            shipmentPM.VolumetricWeight = shipment.VolumetricWeight;

            shipmentPM.NumberOfPackages = shipment.NumberOfPackages;
            shipmentPM.NumberOfContainers = shipment.NumberOfContainers;

            shipmentPM.StatusName = status.Name;

            shipmentPM.ShipmentLevelCode = shipment.ShipmentLevelCode;

            shipmentPM.NumberOfFollowUps = shipment.NumberOfFollowUps;
            #endregion

            #region shipmentpickup deliveries

            shipmentPM.ShipmentPickUps = shipmentPickUpQuery.GetShipmentPickUpPMsByTenantAndShipment(shipment.Id, shipment.Tenant).ToList();
            shipmentPM.ShipmentDeliveries = shipmentDeliveryQuery.GetShipmentDeliveryPMsByTenantAndShipment(shipment.Id, shipment.Tenant).ToList();

            #endregion

            //Warehouse Leg
            shipmentPM.WarehouseLegWarehouseId = shipment.WarehouseLegWarehouseId;
            shipmentPM.WarehouseLegAddressId = shipment.WarehouseLegAddressId;
            shipmentPM.WarehouseLegTerminalCode = shipment.WarehouseLegTerminalCode;
            shipmentPM.WarehouseLegExpectedEntryDate = shipment.WarehouseLegExpectedEntryDate;
            shipmentPM.WarehouseLegActualEntryDate = shipment.WarehouseLegActualEntryDate;
            shipmentPM.WarehouseLegExpectedReleaseDate = shipment.WarehouseLegExpectedReleaseDate;
            shipmentPM.WarehouseLegActualReleaseDate = shipment.WarehouseLegActualReleaseDate;
            shipmentPM.WarehouseLegLastFreeDate = shipment.WarehouseLegLastFreeDate;
            shipmentPM.WarehouseLegRemarks = shipment.WarehouseLegRemarks;
            shipmentPM.WarehouseLegReference = shipment.WarehouseLegReference;
            shipmentPM.WarehouseLegEntryDate = shipment.WarehouseLegActualEntryDate != null ? shipment.WarehouseLegActualEntryDate : shipment.WarehouseLegExpectedEntryDate;
            shipmentPM.WarehouseLegReleaseDate = shipment.WarehouseLegActualReleaseDate != null ? shipment.WarehouseLegActualReleaseDate : shipment.WarehouseLegExpectedReleaseDate;
            if (!string.IsNullOrEmpty(shipment.WarehouseLegWarehouseId))
            {
                Card cardObject = CardRepository.GetSingleCard(shipment.WarehouseLegWarehouseId, shipment.Tenant, true);
                if (cardObject != null)
                {
                    shipmentPM.WarehouseLegTerminalName = cardObject.EnglishName;
                }
            }

            shipmentPM.IsStandalonePickupDelivery = shipment.IsStandalonePickupDelivery;
            shipmentPM.RegistryDate = shipment.RegistryDate;
            shipmentPM.IsAssembly = shipment.IsAssembly;
            shipmentPM.MasterShipmentDataId = shipment.MasterShipmentDataId;
            shipmentPM.ComputedStatusId = shipment.ComputedStatusId;
            shipmentPM.ComputedStatusDate = shipment.ComputedStatusDate;
            shipmentPM.CustomFileId = shipment.CustomFileId;
            shipmentPM.CustomFileNumber = shipment.CustomFileNumber;
            shipmentPM.CustomsDeclarationNumber = shipment.CustomsDeclarationNumber;
            shipmentPM.ValueOfGoods = shipment.ValueOfGoods;
            shipmentPM.ValueOfGoodsCurrencyId = shipment.ValueOfGoodsCurrencyId;
            shipmentPM.IsNewARInvoiceBlocked = shipment.IsNewARInvoiceBlocked;
            shipmentPM.LastSharedEventId = shipment.LastSharedEventId;
            shipmentPM.LastSharedEventLocation = shipment.LastSharedEventLocation;
            shipmentPM.LastSharedEventNotes = shipment.LastSharedEventNotes;
            shipmentPM.LastSharedEventDate = shipment.LastSharedEventDate;
            shipmentPM.FirstOperationalCloseDate = shipment.FirstOperationalCloseDate;
            shipmentPM.FirstAccountingCloseDate = shipment.FirstAccountingCloseDate;
            shipmentPM.LastFinalDestination = shipment.LastFinalDestination;
            shipmentPM.FirstPickupETA = shipment.FirstPickupETA;
            shipmentPM.FirstPickupETD = shipment.FirstPickupETD;            
            shipmentPM.SplitOnForwarding = shipment.SplitOnForwarding;
            shipmentPM.From = shipment.From;
            shipmentPM.To = shipment.To;
            shipmentPM.Origin = shipment.Origin;

            if (!string.IsNullOrEmpty(shipment.LastSharedEventId))
            {
                EventTypeRepository myRepository = new EventTypeRepository(tenant);
                EventType myEvent = myRepository.GetSingleEventType(shipment.LastSharedEventId, tenant);
                if (myEvent != null)
                {
                    shipmentPM.LastSharedEventName = myEvent.EnglishName;
                }
            }

            if (withComposition)
            {
                #region Collections

                ShipmentOrderPackageRepository shipmentOrderPackageRepository = new ShipmentOrderPackageRepository(repository.context);
                ShipmentOrderPackageQuery shipmentOrderPackageQuery = new ShipmentOrderPackageQuery(shipmentOrderPackageRepository);

                ShipmentPackageRepository shipmentPackageRepository = new ShipmentPackageRepository(repository.context);
                ShipmentPackageQuery shipmentPackageQuery = new ShipmentPackageQuery(shipmentPackageRepository);

                #region shipment order packages
                shipmentPM.ShipmentOrderPackages = shipmentOrderPackageQuery.GetShipmentOrderPackagesByShipment(shipment.Id, shipment.Tenant);
                #endregion

                #region Shipment packages | Commodities

                shipmentPM.ShipmentPackages = shipmentPackageQuery.GetShipmentPackages(shipment.Id, shipment.ShipmentNumber, shipment.Tenant);
                #endregion
                #endregion
            }

            // Edited by Ayman
            if (shipmentPM.ShipmentPackages != null)
            {
                var myGroup = (from a in shipmentPM.ShipmentPackages
                               where a.IsContainer && a.PackageTypeId != null
                               group a by a.PackageTypeId into g
                               select new
                               {
                                   PackageTypeId = g.Key,
                                   Count = g.Count(),
                               });

                string myLineText = "";
                string myTotalContainers = "";
                PackageTypeRepository packageTypeRepository = new PackageTypeRepository(tenant);

                foreach (var item in myGroup)
                {
                    myLineText = "";

                    PackageType myPackageType = packageTypeRepository.GetSinglePackageType(item.PackageTypeId, tenant);
                    if (myPackageType != null)
                    {
                        if (!string.IsNullOrEmpty(myPackageType.PrintAs))
                        {
                            myLineText = item.Count + " x " + myPackageType.PrintAs;
                        }

                        else if (!string.IsNullOrEmpty(myPackageType.Code))
                        {
                            myLineText = item.Count + " x " + myPackageType.Code;
                        }
                    }

                    if (!string.IsNullOrEmpty(myLineText))
                    {
                        if (string.IsNullOrEmpty(myTotalContainers))
                        {
                            myTotalContainers = myLineText;
                        }

                        else
                        {
                            myTotalContainers += ", " + myLineText;
                        }
                    }
                }


            }

            ShipmentPM returnShipment = BranchPermitionsFilter.AddUserBranchRestrictionFilters(new QueryOperations(), shipmentPM, tenant);
            returnShipment = ProductPermitionsFilter.AddUserProductRestrictionFilters(new QueryOperations(), shipmentPM, tenant);

            return returnShipment;
        }

        public ShipmentPM MapShipmentToShipmentPMForAutomation(ShipmentPM shipmentPM, Shipment shipment, IQueryable<ShipmentMasterData> shipmentMasterDataList, ShipmentMasterData masterData)
        {
            if (masterData == null)
            {
                if (shipment.MasterShipmentDataId != null && shipmentMasterDataList != null)
                {
                    masterData = (from a in shipmentMasterDataList where a.Id == shipment.MasterShipmentDataId select a).FirstOrDefault();
                }
            }

            if (EntityChangeHelper.IsShowLogBoxAutomationFields())
            {
                #region ShipmentAdditionalCloudDatas
                shipmentPM.IsImporterApprovalRequired = GetIsImporterApprovalRequried(shipment.Id, shipment.Tenant);
                #endregion
            }

            shipmentPM.IsStandalonePickupDelivery = shipment.IsStandalonePickupDelivery;
            shipmentPM.CreatedByPartner = shipment.CreatedByPartner;
            shipmentPM.Tenant = shipment.Tenant;
            shipmentPM.Id = shipment.Id;
            shipmentPM.StatusId = shipment.StatusId;
            shipmentPM.UpdatedByUserId = shipment.UpdatedByUserId;
            shipmentPM.CustomerId = shipment.CustomerId;
            shipmentPM.DirectionId = shipment.DirectionId;
            shipmentPM.AgentId = shipment.AgentId;
            shipmentPM.AgentComputed = shipment.AgentComputed;
            shipmentPM.ComputedShipmentNumber = shipment.AgentComputed;
            shipmentPM.BranchId = shipment.BranchId;
            shipmentPM.CreatedByUserId = shipment.CreatedByUserId;
            shipmentPM.OperationalClosedByUserId = shipment.OperationalClosedByUserId;
            shipmentPM.SalesmanUserId = shipment.SalesmanUserId;
            shipmentPM.AccountManagerUserId = shipment.AccountManagerUserId;
            shipmentPM.MainCarriageTransportModeId = shipment.TransportModeId;
            shipmentPM.DepartmentId = shipment.DepartmentId;
            shipmentPM.ShipmentLevelCode = shipment.ShipmentLevelCode;
            shipmentPM.IsOperationalClosed = shipment.IsOperationalClosed;
            shipmentPM.IsAccountingClosed = shipment.IsAccountingClosed;
            shipmentPM.OriginShipmentId = shipment.OriginShipmentId;
            shipmentPM.TransportModeId = shipment.TransportModeId;
            shipmentPM.IncotermId = shipment.IncotermId;
            shipmentPM.ShipmentTypeId = shipment.ShipmentTypeId;
            shipmentPM.IsCancelled = shipment.IsCancelled;
            shipmentPM.FirstARInvoiceApprovalDate = shipment.FirstARInvoiceApprovalDate;
            shipmentPM.ActualFinalArrivalDate = shipment.ActualFinalArrivalDate;
            shipmentPM.EstimatedFinalArrivalDate = shipment.EstimatedFinalArrivalDate;
            shipmentPM.CreateDateTime = shipment.CreateDateTime;
            shipmentPM.WarehouseStorageFreeDays = shipment.WarehouseStorageFreeDays;
            shipmentPM.OrderIsDangerouseGoods = shipment.OrderIsDangerouseGoods;
            shipmentPM.FirstPickupETA = shipment.FirstPickupETA;
            shipmentPM.FirstPickupETD = shipment.FirstPickupETD;
            shipmentPM.PreForwardingETA = shipment.PreForwardingETA;
            shipmentPM.PreForwardingETD = shipment.PreForwardingETD;
            shipmentPM.OnForwardingETA = shipment.OnForwardingETA;
            shipmentPM.OnForwardingETD = shipment.OnForwardingETD;
            shipmentPM.WarehouseLegLastFreeDate = shipment.WarehouseLegLastFreeDate;
            shipmentPM.LastSharedEventId = shipment.LastSharedEventId;
            shipmentPM.LastSharedEventDate = shipment.LastSharedEventDate;
            shipmentPM.IsAccrualsApproved = shipment.IsAccrualsApproved; 
            shipmentPM.LastUpdateDate = shipment.LastUpdateDate;

            if (masterData != null)
            {
                shipmentPM.MainCarriageFinalDestinationETA = masterData.MainCarriageFinalDestinationETA;
                shipmentPM.MainCarriageFinalDestinationATA = masterData.MainCarriageFinalDestinationATA;
                shipmentPM.MainCarriageCarrierId = masterData.MainCarriageCarrierId;
                shipmentPM.MainCarriageETD = masterData.MainCarriageETD;
                shipmentPM.MainCarriageETA = masterData.MainCarriageETA;
                shipmentPM.MainCarriageATD = masterData.MainCarriageATD;
                shipmentPM.MainCarriageATA = masterData.MainCarriageATA;
                shipmentPM.FinalDistenationPortId = masterData.Transshipment3ToPortId != null ? masterData.Transshipment3ToPortId : masterData.Transshipment2ToPortId != null ? masterData.Transshipment2ToPortId : masterData.Transshipment1ToPortId != null ? masterData.Transshipment1ToPortId : masterData.MainCarriageToPortId;
                shipmentPM.CutoffDate = masterData.CutoffDate;
                shipmentPM.DocumentsClosingDate = masterData.DocumentsClosingDate;
                shipmentPM.PreCarriageETA = masterData.PreCarriageETA;
                shipmentPM.PreCarriageETD = masterData.PreCarriageETD;
                shipmentPM.OnCarriageETA = masterData.OnCarriageETA;
                shipmentPM.OnCarriageETD = masterData.OnCarriageETD;
            }

            shipmentPM.Field1 = new CustomFieldClass("Field1", "Shipment", shipment.Field1);
            shipmentPM.Field2 = new CustomFieldClass("Field2", "Shipment", shipment.Field2);
            shipmentPM.Field3 = new CustomFieldClass("Field3", "Shipment", shipment.Field3);
            shipmentPM.Field4 = new CustomFieldClass("Field4", "Shipment", shipment.Field4);
            shipmentPM.Field5 = new CustomFieldClass("Field5", "Shipment", shipment.Field5);
            shipmentPM.Field6 = new CustomFieldClass("Field6", "Shipment", shipment.Field6);
            shipmentPM.Field7 = new CustomFieldClass("Field7", "Shipment", shipment.Field7);
            shipmentPM.Field8 = new CustomFieldClass("Field8", "Shipment", shipment.Field8);
            shipmentPM.Field9 = new CustomFieldClass("Field9", "Shipment", shipment.Field9);
            shipmentPM.Field10 = new CustomFieldClass("Field10", "Shipment", shipment.Field10);
            shipmentPM.Field11 = new CustomFieldClass("Field11", "Shipment", shipment.Field11);
            shipmentPM.Field12 = new CustomFieldClass("Field12", "Shipment", shipment.Field12);
            shipmentPM.Field13 = new CustomFieldClass("Field13", "Shipment", shipment.Field13);
            shipmentPM.Field14 = new CustomFieldClass("Field14", "Shipment", shipment.Field14);
            shipmentPM.Field15 = new CustomFieldClass("Field15", "Shipment", shipment.Field15);
            shipmentPM.Field16 = new CustomFieldClass("Field16", "Shipment", shipment.Field16);
            shipmentPM.Field17 = new CustomFieldClass("Field17", "Shipment", shipment.Field17);
            shipmentPM.Field18 = new CustomFieldClass("Field18", "Shipment", shipment.Field18);
            shipmentPM.Field19 = new CustomFieldClass("Field19", "Shipment", shipment.Field19);
            shipmentPM.Field20 = new CustomFieldClass("Field20", "Shipment", shipment.Field20);
            shipmentPM.Field21 = new CustomFieldClass("Field21", "Shipment", shipment.Field21);
            shipmentPM.Field22 = new CustomFieldClass("Field22", "Shipment", shipment.Field22);
            shipmentPM.Field23 = new CustomFieldClass("Field23", "Shipment", shipment.Field23);
            shipmentPM.Field24 = new CustomFieldClass("Field24", "Shipment", shipment.Field24);
            shipmentPM.Field25 = new CustomFieldClass("Field25", "Shipment", shipment.Field25);
            shipmentPM.Field26 = new CustomFieldClass("Field26", "Shipment", shipment.Field26);
            shipmentPM.Field27 = new CustomFieldClass("Field27", "Shipment", shipment.Field27);
            shipmentPM.Field28 = new CustomFieldClass("Field28", "Shipment", shipment.Field28);
            shipmentPM.Field29 = new CustomFieldClass("Field29", "Shipment", shipment.Field29);
            shipmentPM.Field30 = new CustomFieldClass("Field30", "Shipment", shipment.Field30);
            shipmentPM.Field31 = new CustomFieldClass("Field31", "Shipment", shipment.Field31);
            shipmentPM.Field32 = new CustomFieldClass("Field32", "Shipment", shipment.Field32);
            shipmentPM.Field33 = new CustomFieldClass("Field33", "Shipment", shipment.Field33);
            shipmentPM.Field34 = new CustomFieldClass("Field34", "Shipment", shipment.Field34);
            shipmentPM.Field35 = new CustomFieldClass("Field35", "Shipment", shipment.Field35);
            shipmentPM.Field36 = new CustomFieldClass("Field36", "Shipment", shipment.Field36);
            shipmentPM.Field37 = new CustomFieldClass("Field37", "Shipment", shipment.Field37);
            shipmentPM.Field38 = new CustomFieldClass("Field38", "Shipment", shipment.Field38);
            shipmentPM.Field39 = new CustomFieldClass("Field39", "Shipment", shipment.Field39);
            shipmentPM.Field40 = new CustomFieldClass("Field40", "Shipment", shipment.Field40);

            #region ShipmentComputedFields
            MapShipmentComputedFields(shipmentPM);
            #endregion

            return null;
        }

        private static void MapShipmentComputedFields(ShipmentPM shipmentPM)
        {
            ShipmentComputedFieldsRepository shipmentComputedFieldsRepository = new ShipmentComputedFieldsRepository(shipmentPM.Tenant);
            ShipmentComputedFields entityComputedFields = shipmentComputedFieldsRepository.GetSingleShipmentComputedFields(shipmentPM.Id, shipmentPM.Tenant);

            if (entityComputedFields != null)
            {
                shipmentPM.IsDepositionRequired = entityComputedFields.IsDepositionRequired;
                shipmentPM.IsRequestedDocuments = entityComputedFields.IsRequestedDocuments;
                shipmentPM.IsDigitalSignRequired = entityComputedFields.IsDigitalSignRequired;
                shipmentPM.IsMissingDocuments = entityComputedFields.IsMissingDocuments;
                shipmentPM.DocumentsSearchFields = entityComputedFields.DocumentsSearchFields;
                shipmentPM.MissingDocumentsCount = entityComputedFields.MissingDocumentsCount;
                shipmentPM.MissingDocumentsNames = entityComputedFields.MissingDocumentsNames;
                shipmentPM.RequestedDocumentsCount = entityComputedFields.RequestedDocumentsCount;
                shipmentPM.NumberOfHouses = entityComputedFields.NumberOfHouses;
                shipmentPM.ImporterDepositionRequestDetails = entityComputedFields.ImporterDepositionRequestDetails;
                shipmentPM.LastDocumentDateTime = entityComputedFields.LastDocumentDateTime;
                shipmentPM.CreatedFromDigital = entityComputedFields.CreatedFromDigital;
                shipmentPM.BookingConfirmationSentDate = entityComputedFields.BookingConfirmationSent;
                shipmentPM.PreAlertSentDate = entityComputedFields.PreAlertSent;
                shipmentPM.DeliveryNoticeSentDate = entityComputedFields.DeliveryNoticeSent;
                shipmentPM.ExpectedArrivalNoticeSentDate = entityComputedFields.ExpectedArrivalNoticeSent;
                shipmentPM.ArrivalNoticeSentDate = entityComputedFields.ArrivalNoticeSent;
                shipmentPM.T1ReceivedDate = entityComputedFields.T1Received;
                shipmentPM.FirstPickupATD = entityComputedFields.FirstPickupATD;
            }
        }

        private bool GetIsImporterApprovalRequried(string shipmentId , int tenant , List<ShipmentAdditionalCloudData> shipmentAdditionalCloudDataLists = null)
        {
            bool isImporterApprovalRequried = false;
            ShipmentAdditionalCloudData shipmentAdditionalCloudDatas = null; 
            if (shipmentAdditionalCloudDataLists == null)
            {
                shipmentAdditionalCloudDatas = (from a in repository.context.ShipmentAdditionalCloudDatas
                                                    where a.Id == shipmentId && a.Tenant == tenant
                                                    select a).FirstOrDefault();

            }
            else  shipmentAdditionalCloudDatas = shipmentAdditionalCloudDataLists.Where(d => d.Id == shipmentId && d.Tenant == tenant).FirstOrDefault();

            if (shipmentAdditionalCloudDatas != null) isImporterApprovalRequried = shipmentAdditionalCloudDatas.IsImporterApprovalRequried;

            return isImporterApprovalRequried;
        }

        private string GetNewStatusId(string currentStatusId, string newStatusId, int tenant, ref string statusName)
        {
            EntityStatus currentStatus = EntityStatusRepository.GetSingleEntityStatus(currentStatusId, tenant, true);
            EntityStatus newStatus = EntityStatusRepository.GetSingleEntityStatus(newStatusId, tenant, true);

            if (currentStatus == null)
            {
                return newStatusId;
            }
            else if (newStatus == null)
            {
                return currentStatusId;
            }

            string id = null;

            if (currentStatus.StatusWeight > newStatus.StatusWeight)
            {
                id = currentStatus.Id;
                statusName = currentStatus.Name;
            }
            else
            {
                id = newStatus.Id;
                statusName = newStatus.Name;
            }
            return id;
        }

        public ShipmentPM GetSinglePMBySecurityKey(string key, string id, int tenant)
        {
            if (!string.IsNullOrEmpty(key))
            {
                Shipment shipment = (from a in repository.context.Shipments.Include("EntityStatus").Include("ShipmentType").Include("Incoterm").Include("ShipmentReceivableStatus").Include("ShipmentPayableStatus").Include("ShipmentLevel").Include("NextLeg").Include("ShipmentType").Include("MoveType")
                                     where a.SecurityKey == key && a.Tenant == tenant && a.Id == id
                                     select a).FirstOrDefault();

                if (shipment != null)
                {
                    ShipmentPM returnShipment = MapShipmentToSecuredShipmentPMWithRestrictionFilters(shipment, tenant);
                    return returnShipment;
                }
            }
            return null;
        }

        public ShipmentPM GetSinglePMBySecurityKeyAndTenant(string key, int tenant)
        {
            if (!string.IsNullOrEmpty(key))
            {
                Shipment shipment = (from a in repository.context.Shipments.Include("EntityStatus").Include("ShipmentType").Include("Incoterm").Include("ShipmentReceivableStatus").Include("ShipmentPayableStatus").Include("ShipmentLevel").Include("NextLeg").Include("ShipmentType").Include("MoveType")
                                     where a.SecurityKey == key && a.Tenant == tenant
                                     select a).FirstOrDefault();

                if (shipment != null)
                {
                    ShipmentPM returnShipment = MapShipmentToSecuredShipmentPMWithRestrictionFilters(shipment, tenant);
                    return returnShipment;
                }
            }
            return null;
        }

        private ShipmentPM MapShipmentToSecuredShipmentPMWithRestrictionFilters(Shipment shipment, int tenant)
        {
            ShipmentMasterData masterData = (from a in repository.context.ShipmentMasterDatas
                                             where a.Id == shipment.MasterShipmentDataId
                                             select a).FirstOrDefault();

            ShipmentPM shipmentPM = new ShipmentPM();

            shipmentPM = MapShipmentToShipmentPM(shipmentPM, shipment, null, masterData, true);

            ShipmentPM securedPM = new ShipmentPM();
            SecuredMapping.GetMappedPM(shipmentPM, securedPM, "Shipment", tenant);

            ShipmentPM returnShipment = BranchPermitionsFilter.AddUserBranchRestrictionFilters(new QueryOperations(), securedPM, tenant);//securedPM;
            returnShipment = ProductPermitionsFilter.AddUserProductRestrictionFilters(new QueryOperations(), securedPM, tenant);

            return returnShipment;
        }

        public ShipmentPM GetSinglePmForMobile(string id, int tenant)
        {
            Shipment shipment = (from a in repository.context.Shipments.Include("EntityStatus").Include("ComputedEntityStatus").Include("ShipmentMasterData")
                                 where a.Id == id && a.Tenant == tenant
                                 select a).FirstOrDefault();

            ShipmentMasterData masterData = (from a in repository.context.ShipmentMasterDatas
                                             where a.Id == shipment.MasterShipmentDataId
                                             select a).FirstOrDefault();

            ShipmentPM shipmentPM = new ShipmentPM();
            shipmentPM = MapShipmentToShipmentPMForMobile(shipmentPM, shipment, null, masterData, true);
            ShipmentPM securedPM = new ShipmentPM();
            securedPM = SecuredMapping.GetMappedPM(shipmentPM, securedPM, "Shipment", tenant);

            ShipmentPM returnShipment = BranchPermitionsFilter.AddUserBranchRestrictionFilters(new QueryOperations(), securedPM, tenant);//securedPM;
            returnShipment = ProductPermitionsFilter.AddUserProductRestrictionFilters(new QueryOperations(), securedPM, tenant);

            return returnShipment;
        }

        public string GetShipmentIdIfOneShipmentHaveHouseNumber(string house, int tenant)
        {
            if (string.IsNullOrEmpty(house)) { return null; }

            List<Shipment> shipments = repository.GetAllShipmentsByHouseNumber(house, tenant);

            if (shipments.Count == 0) { throw new Exception("There is no Shipment found with house " + house);}

            if (shipments.Count == 1) { return shipments[0].Id;}
            return null;
        }
      

        public ShipmentPM GetSinglePM(string id, int tenant)
        {
            if (!string.IsNullOrEmpty(id))
            {
                Shipment shipment = (from a in repository.context.Shipments.Include("EntityStatus").Include("ComputedEntityStatus").Include("ShipmentType").Include("Incoterm").Include("ShipmentReceivableStatus").Include("ShipmentPayableStatus").Include("ShipmentLevel").Include("NextLeg").Include("ShipmentType").Include("ShipmentMasterData").Include("SpecialServicesType").Include("MoveType")
                                     where a.Id == id && a.Tenant == tenant
                                     select a).FirstOrDefault();

                if (shipment != null)
                {
                    ShipmentMasterData masterData = (from a in repository.context.ShipmentMasterDatas
                                                     where a.Id == shipment.MasterShipmentDataId
                                                     select a).FirstOrDefault();

                    ShipmentPM shipmentPM = new ShipmentPM();

                    shipmentPM = MapShipmentToShipmentPM(shipmentPM, shipment, null, masterData, true);                   
                    ShipmentPM securedPM = new ShipmentPM();
                    securedPM = SecuredMapping.GetMappedPM(shipmentPM, securedPM, "Shipment", tenant);

                    ShipmentPM returnShipment = BranchPermitionsFilter.AddUserBranchRestrictionFilters(new QueryOperations(), securedPM, tenant);//securedPM;
                    returnShipment = ProductPermitionsFilter.AddUserProductRestrictionFilters(new QueryOperations(), securedPM, tenant);
                    var CLoudData = (from a in repository.context.ShipmentAdditionalCloudDatas
                                     where a.Id == shipment.Id
                                     select a).FirstOrDefault();

                    if (CLoudData != null)
                    {
                        returnShipment.DeclarationXMLData = CLoudData.DeclarationXmlData;
                        returnShipment.DeclarationWCOXml = CLoudData.DeclarationWCOXml;
                        returnShipment.ApproveDateTime = CLoudData.ApproveDateTime;
                        returnShipment.IsImporterApprovalRequired = CLoudData.IsImporterApprovalRequried;
                        returnShipment.VersionApproved = CLoudData.VersionApproved;
                        returnShipment.ShipmentAddtionalDataXML = CLoudData.ShipmentAddtionalDataXML;
                        returnShipment.SendUpdatesToAgentEnabled = CLoudData.SendUpdatesToAgentEnabled;
                        returnShipment.DocsSentToAgent = CLoudData.DocsSentToAgent;
                        returnShipment.ApprovedBy = CLoudData.ApprovedByUserName;
                        returnShipment.DocumentsApprovedByUserName = CLoudData.DocumentsApprovedByUserName;
                        returnShipment.IsUserIDNumberRequired = CLoudData.IsUserIDNumberRequired;
                        returnShipment.UserIdNumberUpdateDate = CLoudData.UserIdNumberUpdateDate;
                        returnShipment.UserIdNumberXMLData = CLoudData.UserIdNumberXMLData;
                        returnShipment.UserIdNumber = CLoudData.UserIdNumber;
                        returnShipment.PaymentRequestXML = CLoudData.PaymentRequestXML;
                        returnShipment.PaymentDateTime = CLoudData.PaymentDateTime;
                        returnShipment.IsPaymentRequired = CLoudData.IsPaymentRequired;
                    }

                    MapShipmentComputedFields(returnShipment);


                    return returnShipment;
                }

                else
                {
                    return null;
                }
            }
            return null;
        }

        public ShipmentPM GetSinglePMWithLists(string id, int tenant)
        {
            if (!string.IsNullOrEmpty(id))
            {
                Shipment shipment = (from a in repository.context.Shipments.Include("EntityStatus").Include("ComputedEntityStatus").Include("ShipmentType").Include("Incoterm").Include("ShipmentReceivableStatus").Include("ShipmentPayableStatus").Include("ShipmentLevel").Include("NextLeg").Include("ShipmentType").Include("ShipmentMasterData").Include("SpecialServicesType").Include("MoveType")
                                     where a.Id == id && a.Tenant == tenant
                                     select a).FirstOrDefault();

                ShipmentMasterData masterData = (from a in repository.context.ShipmentMasterDatas
                                                 where a.Id == shipment.MasterShipmentDataId
                                                 select a).FirstOrDefault();

                ShipmentPM shipmentPM = new ShipmentPM();

                shipmentPM = MapShipmentToShipmentPM(shipmentPM, shipment, null, masterData, true);

                ShipmentPM securedPM = new ShipmentPM();
                securedPM = SecuredMapping.GetMappedPM(shipmentPM, securedPM, "Shipment", tenant);

                ShipmentPM returnShipment = BranchPermitionsFilter.AddUserBranchRestrictionFilters(new QueryOperations(), securedPM, tenant);//securedPM;
                returnShipment = ProductPermitionsFilter.AddUserProductRestrictionFilters(new QueryOperations(), securedPM, tenant);

                #region Documents Filing Lists
                DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(tenant);
                List<DocumentsFilingPM> DocumentsFilingLists = documentsFilingQuery.GetDocumentsFilingPMsByEntityId(returnShipment.Id, tenant);
                returnShipment.RequiredDocuments = DocumentsFilingLists.Where(d => d.IsRequested).ToList();
                returnShipment.ReceivedDocuments = DocumentsFilingLists.Where(d => d.Received).ToList();
                returnShipment.MissingDocuments = DocumentsFilingLists.Where(d => d.IsRequested == true && !d.Received).ToList();
                #endregion

                return returnShipment;
            }

            return null;
        }

        public ShipmentPM GetSingleShipmentPM(string id, int tenant)
        {
            if (!string.IsNullOrEmpty(id))
            {
                Shipment shipment = (from a in repository.context.Shipments.Include("EntityStatus").Include("ShipmentType").Include("Incoterm").Include("ShipmentReceivableStatus").Include("ShipmentPayableStatus").Include("ShipmentLevel").Include("NextLeg").Include("ShipmentType").Include("ShipmentMasterData").Include("SpecialServicesType")
                                     where a.Id == id && a.Tenant == tenant
                                     select a).FirstOrDefault();
                if (shipment == null)
                {
                    return null;
                }
                ShipmentMasterData masterData = (from a in repository.context.ShipmentMasterDatas
                                                 where a.Id == shipment.MasterShipmentDataId
                                                 select a).FirstOrDefault();

                ShipmentPM shipmentPM = new ShipmentPM();

                shipmentPM = MapShipmentToShipmentPM(shipmentPM, shipment, null, masterData, true);

                ShipmentPM securedPM = new ShipmentPM();
                securedPM = SecuredMapping.GetMappedPM(shipmentPM, securedPM, "Shipment", tenant);

                ShipmentPM returnShipment = BranchPermitionsFilter.AddUserBranchRestrictionFilters(new QueryOperations(), securedPM, tenant);//securedPM;
                returnShipment = ProductPermitionsFilter.AddUserProductRestrictionFilters(new QueryOperations(), securedPM, tenant);

                return returnShipment;
            }

            return null;
        }

        public ShipmentPM GetSingleShipmentPMByNumber(string shipmentNumber, int tenant)
        {
            if (!string.IsNullOrEmpty(shipmentNumber))
            {
                Shipment shipment = (from a in repository.context.Shipments.Include("EntityStatus").Include("ShipmentType").Include("Incoterm").Include("ShipmentReceivableStatus").Include("ShipmentPayableStatus").Include("ShipmentLevel").Include("NextLeg").Include("ShipmentType").Include("ShipmentMasterData").Include("MoveType")
                                     where a.ShipmentNumber == shipmentNumber && a.Tenant == tenant
                                     select a).FirstOrDefault();

                if (shipment != null)
                {
                    ShipmentMasterData masterData = (from a in repository.context.ShipmentMasterDatas
                                                     where a.Id == shipment.MasterShipmentDataId
                                                     select a).FirstOrDefault();

                    ShipmentPM shipmentPM = new ShipmentPM();

                    shipmentPM = MapShipmentToShipmentPM(shipmentPM, shipment, null, masterData, true);
                    var CLoudData = (from a in repository.context.ShipmentAdditionalCloudDatas
                                     where a.Id == shipment.Id
                                     select a).FirstOrDefault();
                    if (CLoudData != null)
                    {
                        shipmentPM.DeclarationXMLData = CLoudData.DeclarationXmlData;
                        shipmentPM.DeclarationWCOXml = CLoudData.DeclarationWCOXml;
                        shipmentPM.ApproveDateTime = CLoudData.ApproveDateTime;
                        shipmentPM.IsImporterApprovalRequired = CLoudData.IsImporterApprovalRequried;
                        shipmentPM.VersionApproved = CLoudData.VersionApproved;
                        shipmentPM.ShipmentAddtionalDataXML = CLoudData.ShipmentAddtionalDataXML;
                        shipmentPM.SendUpdatesToAgentEnabled = CLoudData.SendUpdatesToAgentEnabled;
                        shipmentPM.DocsSentToAgent = CLoudData.DocsSentToAgent;
                        shipmentPM.ApprovedBy = CLoudData.ApprovedByUserName;
                        shipmentPM.DocumentsApprovedByUserName = CLoudData.DocumentsApprovedByUserName;
                        shipmentPM.IsUserIDNumberRequired = CLoudData.IsUserIDNumberRequired;
                        shipmentPM.UserIdNumberUpdateDate = CLoudData.UserIdNumberUpdateDate;
                        shipmentPM.UserIdNumberXMLData = CLoudData.UserIdNumberXMLData;
                        shipmentPM.UserIdNumber = CLoudData.UserIdNumber;
                        shipmentPM.PaymentRequestXML = CLoudData.PaymentRequestXML;
                        shipmentPM.PaymentDateTime = CLoudData.PaymentDateTime;
                        shipmentPM.IsPaymentRequired = CLoudData.IsPaymentRequired;
                    }
                    return shipmentPM;
                }
            }

            return null;
        }

        public ShipmentPM GetSingleShipmentPMByForwarderNumber(string FWshipmentNumber, int tenant, int PartnerTenant)
        {
            if (!string.IsNullOrEmpty(FWshipmentNumber))
            {
                HybridPartnerRepository myRepo = new HybridPartnerRepository(tenant);
                var Partner = myRepo.GetHybridPartnersByPartnerTenant(PartnerTenant).FirstOrDefault();
                Shipment shipment = null;
                if (Partner != null)
                {
                    shipment = (from a in repository.context.Shipments.Include("EntityStatus").Include("ShipmentType").Include("Incoterm").Include("ShipmentReceivableStatus").Include("ShipmentPayableStatus").Include("ShipmentLevel").Include("NextLeg").Include("ShipmentType").Include("ShipmentMasterData")
                                where a.ForwarderShipmentNumber == FWshipmentNumber && a.Tenant == tenant && !a.IsCancelled && a.ForwarderPartnerId == Partner.Id
                                select a).FirstOrDefault();
                }
                else
                {
                    shipment = (from a in repository.context.Shipments.Include("EntityStatus").Include("ShipmentType").Include("Incoterm").Include("ShipmentReceivableStatus").Include("ShipmentPayableStatus").Include("ShipmentLevel").Include("NextLeg").Include("ShipmentType").Include("ShipmentMasterData")
                                where a.ForwarderShipmentNumber == FWshipmentNumber && a.Tenant == tenant && !a.IsCancelled
                                select a).FirstOrDefault();
                }


                if (shipment != null)
                {
                    ShipmentMasterData masterData = (from a in repository.context.ShipmentMasterDatas
                                                     where a.Id == shipment.MasterShipmentDataId
                                                     select a).FirstOrDefault();

                    ShipmentPM shipmentPM = new ShipmentPM();

                    shipmentPM = MapShipmentToShipmentPM(shipmentPM, shipment, null, masterData, true);
                    var CLoudData = (from a in repository.context.ShipmentAdditionalCloudDatas
                                     where a.Id == shipment.Id
                                     select a).FirstOrDefault();
                    if (CLoudData != null)
                    {
                        shipmentPM.DeclarationXMLData = CLoudData.DeclarationXmlData;
                        shipmentPM.DeclarationWCOXml = CLoudData.DeclarationWCOXml;
                        shipmentPM.ApproveDateTime = CLoudData.ApproveDateTime;
                        shipmentPM.IsImporterApprovalRequired = CLoudData.IsImporterApprovalRequried;
                        shipmentPM.VersionApproved = CLoudData.VersionApproved;
                        shipmentPM.ShipmentAddtionalDataXML = CLoudData.ShipmentAddtionalDataXML;
                        shipmentPM.SendUpdatesToAgentEnabled = CLoudData.SendUpdatesToAgentEnabled;
                        shipmentPM.DocsSentToAgent = CLoudData.DocsSentToAgent;
                        shipmentPM.ApprovedBy = CLoudData.ApprovedByUserName;
                        shipmentPM.DocumentsApprovedByUserName = CLoudData.DocumentsApprovedByUserName;
                        shipmentPM.IsUserIDNumberRequired = CLoudData.IsUserIDNumberRequired;
                        shipmentPM.UserIdNumberUpdateDate = CLoudData.UserIdNumberUpdateDate;
                        shipmentPM.UserIdNumberXMLData = CLoudData.UserIdNumberXMLData;
                        shipmentPM.UserIdNumber = CLoudData.UserIdNumber;
                        shipmentPM.PaymentRequestXML = CLoudData.PaymentRequestXML;
                        shipmentPM.PaymentDateTime = CLoudData.PaymentDateTime;
                        shipmentPM.IsPaymentRequired = CLoudData.IsPaymentRequired;
                    }
                    return shipmentPM;
                }
            }

            return null;
        }


        public ShipmentPM GetSinglePMWithoutComposition(string id, int tenant)
        {
            Shipment shipment = (from a in repository.context.Shipments.Include("EntityStatus").Include("ShipmentType").Include("Incoterm").Include("ShipmentReceivableStatus").Include("ShipmentPayableStatus").Include("ShipmentLevel").Include("NextLeg").Include("ShipmentType").Include("MoveType").Include("SalesmanUser").Include("SalesmanUser.Contact")
                                 where a.Id == id && a.Tenant == tenant
                                 select a).FirstOrDefault();
            if (shipment == null)
            {
                return null;
            }
            ShipmentMasterData masterData = (from a in repository.context.ShipmentMasterDatas
                                             where a.Id == shipment.MasterShipmentDataId
                                             select a).FirstOrDefault();

            ShipmentPM shipmentPM = new ShipmentPM();

            shipmentPM = MapShipmentToShipmentPM(shipmentPM, shipment, null, masterData, false);

            ShipmentPM securedPM = new ShipmentPM();
            SecuredMapping.GetMappedPM(shipmentPM, securedPM, "Shipment", tenant);

            ShipmentPM returnShipment = BranchPermitionsFilter.AddUserBranchRestrictionFilters(new QueryOperations(), securedPM, tenant);//securedPM;
            returnShipment = ProductPermitionsFilter.AddUserProductRestrictionFilters(new QueryOperations(), securedPM, tenant);
            var CLoudData = (from a in repository.context.ShipmentAdditionalCloudDatas
                             where a.Id == shipment.Id
                             select a).FirstOrDefault();
            if (CLoudData != null)
            {
                returnShipment.DeclarationXMLData = CLoudData.DeclarationXmlData;
                returnShipment.DeclarationWCOXml = CLoudData.DeclarationWCOXml;
                returnShipment.ApproveDateTime = CLoudData.ApproveDateTime;
                returnShipment.IsImporterApprovalRequired = CLoudData.IsImporterApprovalRequried;
                returnShipment.VersionApproved = CLoudData.VersionApproved;
                returnShipment.ShipmentAddtionalDataXML = CLoudData.ShipmentAddtionalDataXML;
                returnShipment.SendUpdatesToAgentEnabled = CLoudData.SendUpdatesToAgentEnabled;
                returnShipment.DocsSentToAgent = CLoudData.DocsSentToAgent;
                returnShipment.ApprovedBy = CLoudData.ApprovedByUserName;
                returnShipment.DocumentsApprovedByUserName = CLoudData.DocumentsApprovedByUserName;
                returnShipment.IsUserIDNumberRequired = CLoudData.IsUserIDNumberRequired;
                returnShipment.UserIdNumberUpdateDate = CLoudData.UserIdNumberUpdateDate;
                returnShipment.UserIdNumberXMLData = CLoudData.UserIdNumberXMLData;
                returnShipment.UserIdNumber = CLoudData.UserIdNumber;
                returnShipment.PaymentRequestXML = CLoudData.PaymentRequestXML;
                returnShipment.PaymentDateTime = CLoudData.PaymentDateTime;
                returnShipment.IsPaymentRequired = CLoudData.IsPaymentRequired;
            }
            return returnShipment;
        }

        public List<ShipmentPM> GetShipmentPMsByMasterIdAndTenant(string masterId, int tenant)
        {
            #region shipmentpm temp code
            IQueryable<ShipmentPM> shipmentPMList = from s in repository.context.Shipments.Include("MainCarriageFromPort").Include("MainCarriageFromPort.Country").Include("Incoterm").Include("MainCarriageCarrierCard").Include("MainCarriageToPort").Include("MainCarriageToPort.Country").Include("NextLeg").Include("Notify1Card").Include("Notify2Card").Include("Notify2Card").Include("FreelancerCard")
.Include("OnCarriageCarrierCard").Include("OnCarriageFromPort").Include("OnCarriageFromPort.Country").Include("OnCarriageToPort").Include("OnCarriageToPort.Country").Include("PreCarriageCarrierCard").Include("PreCarriageFromPort").Include("PreCarriageFromPort.Country").Include("PreCarriageToPort").Include("PreCarriageToPort.Country").Include("MainCarriageToPort").Include("MainCarriageToPort.Country").Include("Transshipment1CarrierCard").Include("Transshipment1FromPort").Include("Transshipment1FromPort.Country").Include("Transshipment1FromPort").Include("Transshipment1ToPort").Include("Transshipment1ToPort.Country").Include("Transshipment2CarrierCard").Include("Transshipment2FromPort").Include("Transshipment2FromPort.Country").Include("Transshipment2ToPort").Include("Transshipment2ToPort.Country").Include("Transshipment3CarrierCard").Include("Transshipment3FromPort").Include("Transshipment3FromPort.Country").Include("ShipmentPayableStatus").Include("ShipmentReceivableStatus").Include("ShipperCard").Include("ShipperNotExporterCard").Include("EntityStatus").Include("UpdatedByUser.Contact").Include("Coloader").Include("CustomClearancePoint")
                                                    join sm in repository.context.ShipmentMasterDatas.Include("Port").Include("Port.Country")
                                                    on s.MasterShipmentDataId equals sm.Id into shipmentJoin
                                                    from m in shipmentJoin.DefaultIfEmpty()
                                                    where s.Tenant == tenant && s.MasterShipmentDataId == masterId && s.Id != masterId && ((s.ShipmentLevelCode == "H") || (s.ShipmentLevelCode == "D") || s.ShipmentLevelCode == "A")
                                                    select new ShipmentPM()
                                                    {
                                                        ProfitCurrencyId = s.ProfitCurrencyId,
                                                        ProfitExchangeRate = s.ProfitExchangeRate,
                                                        OpenPayablesInLocalCurrency = s.OpenPayablesInLocalCurrency,
                                                        AccountedPayablesInLocalCurrency = s.AccountedPayablesInLocalCurrency,
                                                        OpenReceivablesInLocalCurrency = s.OpenReceivablesInLocalCurrency,
                                                        AccountedReceivablesInLocalCurrency = s.AccountedReceivablesInLocalCurrency,
                                                        ProfitInLocalCurrency = s.ProfitInLocalCurrency,
                                                        OpenPayablesInProfitCurrency = s.OpenPayablesInProfitCurrency,
                                                        AccountedPayablesInProfitCurrency = s.AccountedPayablesInProfitCurrency,
                                                        OpenReceivablesInProfitCurrency = s.OpenReceivablesInProfitCurrency,
                                                        AccountedReceivablesInProfitCurrency = s.AccountedReceivablesInProfitCurrency,
                                                        ProfitInProfitCurrency = s.ProfitInProfitCurrency,
                                                        AgentAddressId = s.AgentAddressId,
                                                        AgentContactId = s.AgentContactId,
                                                        AgentId = s.AgentId,
                                                        AgentComputed = s.AgentComputed,
                                                        ComputedShipmentNumber=s.ComputedShipmentNumber,
                                                        AgentName = s.AgentCard != null ? s.AgentCard.EnglishName : null,
                                                        AgentNote = s.AgentCard != null ? s.AgentCard.Notes : null,
                                                        AgentReference1 = s.AgentReference1,
                                                        AgentReference2 = s.AgentReference2,
                                                        AWBAccountingInformation = s.AWBAccountingInformation,
                                                        AWBCarrierTarrifReference = s.AWBCarrierTarrifReference,
                                                        AWBCurrencyId = s.AWBCurrencyId,
                                                        AWBDeclaredValueForCarriage = s.AWBDeclaredValueForCarriage,
                                                        AWBDeclaredValueForCustoms = s.AWBDeclaredValueForCustoms,
                                                        AWBFreightAmountCollect = s.AWBFreightAmountCollect,
                                                        AWBFreightAmountPrepaid = s.AWBFreightAmountPrepaid,
                                                        AWBHandlingInformation = s.AWBHandlingInformation,
                                                        AWBInsurrenceValue = s.AWBInsurrenceValue,
                                                        BookingConfirmationNotes = m.BookingConfirmationNotes,
                                                        BookingConfirmationNumber = m.BookingConfirmationNumber,
                                                        BookingConfirmedBy = m.BookingConfirmedBy,
                                                        BookingNumberOfPackages = s.BookingNumberOfPackages,
                                                        BookingVolume = s.BookingVolume,
                                                        BranchId = s.BranchId,
                                                        ChargeableWeightInKG = s.ChargeableWeightInKG,
                                                        ChargeableWeightEdited = s.ChargeableWeightEdited,
                                                        ConsigneeAddressId = s.ConsigneeAddressId,
                                                        ConsigneeAddressOneTime = s.ConsigneeAddressOneTime,
                                                        ConsigneeContactId = s.ConsigneeContactId,
                                                        ConsigneeId = s.ConsigneeId,
                                                        ConsigneeName = s.ConsigneeCard != null ? s.ConsigneeCard.EnglishName : null,
                                                        ConsigneeNote = s.ConsigneeCard != null ? s.ConsigneeCard.Notes : null,
                                                        ConsigneeNotImporterAddressId = s.ConsigneeNotImporterAddressId,
                                                        ConsigneeNotImporterContactId = s.ConsigneeNotImporterContactId,
                                                        ConsigneeNotImporterId = s.ConsigneeNotImporterId,
                                                        ConsigneeNotImporterName = s.ConsigneeNotImporterCard != null ? s.ConsigneeNotImporterCard.EnglishName : null,
                                                        ConsigneeNotImporterNote = s.ConsigneeNotImporterCard != null ? s.ConsigneeNotImporterCard.Notes : null,
                                                        ConsigneeReference1 = s.ConsigneeReference1,
                                                        ConsigneeReference2 = s.ConsigneeReference2,
                                                        CreateDateTime = s.CreateDateTime,
                                                        CustomAgentExportAddressId = s.CustomAgentExportAddressId,
                                                        CustomAgentExportContactId = s.CustomAgentExportAddressId,
                                                        CustomAgentExportId = s.CustomAgentExportId,
                                                        CustomAgentExportName = s.CustomAgentExportCard.EnglishName,
                                                        AWBCurrencyCode = s.AWBCurrency != null ? s.AWBCurrency.Code : null,
                                                        CustomAgentExportNote = s.CustomAgentExportCard != null ? s.CustomAgentExportCard.Notes : null,
                                                        CustomAgentExportReference = s.CustomAgentExportReference,
                                                        CustomAgentImportAddressId = s.CustomAgentImportAddressId,
                                                        CustomAgentImportContactId = s.CustomAgentImportContactId,
                                                        CustomAgentImportId = s.CustomAgentImportId,
                                                        CustomAgentImportName = s.CustomAgentImportCard != null ? s.CustomAgentImportCard.EnglishName : null,
                                                        CustomAgentImportNote = s.CustomAgentImportCard != null ? s.CustomAgentImportCard.Notes : null,
                                                        CustomAgentImportReference = s.CustomAgentImportReference,
                                                        CustomerAddressId = s.CustomerAddressId,
                                                        CustomerContactId = s.CustomerContactId,
                                                        CustomerId = s.CustomerId,
                                                        CustomerName = s.CustomerCard != null ? s.CustomerCard.EnglishName : null,
                                                        CustomerNote = s.CustomerCard != null ? s.CustomerCard.Notes : null,
                                                        CustomerReference1 = s.CustomerReference1,
                                                        CustomerReference2 = s.CustomerReference2,
                                                        DangerousClassNumber = s.DangerousClassNumber,
                                                        DangerousFlashPoint = s.DangerousFlashPoint,
                                                        DangerousIMDGCode = s.DangerousIMDGCode,
                                                        DangerousMaterialDescription = s.DangerousMaterialDescription,
                                                        DangerousPackagingGroup = s.DangerousPackagingGroup,
                                                        DangerousUnNumber = s.DangerousUnNumber,
                                                        DepartmentId = s.DepartmentId,
                                                        DescriptionOfGoods = s.DescriptionOfGoods,
                                                        DimensionsUnitCode = s.DimensionsUnitCode,
                                                        ChargeableWeightUnitCode = s.ChargeableWeightUnitCode,
                                                        DirectionId = s.DirectionId,
                                                        DirectionName = s.Direction.Name,
                                                        EstimateProfitInLocalCurrency = s.EstimateProfitInLocalCurrency,
                                                        EstimateProfitInProfitCurrency = s.EstimateProfitInProfitCurrency,
                                                        FinalDistenationPortId = m.Transshipment3ToPortId != null ? m.Transshipment3ToPortId : m.Transshipment2ToPortId != null ? m.Transshipment2ToPortId : m.Transshipment1ToPortId != null ? m.Transshipment1ToPortId : m.MainCarriageToPortId,
                                                        MainCarriageFinalDestinationETA = m.MainCarriageFinalDestinationETA,
                                                        MainCarriageFinalDestinationATA = m.MainCarriageFinalDestinationATA,
                                                        FreightForwarderAddressId = s.FreightForwarderAddressId,
                                                        FreightForwarderContactId = s.FreightForwarderContactId,
                                                        FreightForwarderId = s.FreightForwarderId,
                                                        FreightForwarderName = s.FreightForwarderCard != null ? s.FreightForwarderCard.EnglishName : null,
                                                        FreightForwarderReference = s.FreightForwarderReference,
                                                        FreightForwarderNote = s.FreightForwarderCard != null ? s.FreightForwarderCard.Notes : null,
                                                        FreightPrepaidCollectId = s.FreightPrepaidCollectId,
                                                        FromPort = m.MainCarriageFromPort.Code,
                                                        FromPortCountry = m.MainCarriageFromPort.Country.Code,
                                                        FromPortName = m.MainCarriageFromPort.EnglishName,
                                                        GrossWeightInKG = s.GrossWeightInKG,
                                                        GrossWeightPerStorageDays = s.GrossWeightPerStorageDays,
                                                        GrossWeightPerTon = s.GrossWeightPerTon,
                                                        GrossWeightEdited = s.GrossWeightEdited,
                                                        HAWBDate = s.HAWBDate,
                                                        House = s.House,
                                                        Id = s.Id,
                                                        IncotermId = s.IncotermId,
                                                        IncotermCode = s.Incoterm != null ? s.Incoterm.Code : null,
                                                        IncotermName = s.Incoterm != null ? s.Incoterm.Name : null,
                                                        IsAccountingClosed = s.IsAccountingClosed,
                                                        IsCancelled = s.IsCancelled,
                                                        CancelledDate = s.CancelledDate,
                                                        IsDangerous = s.IsDangerous,
                                                        ShipmentLevelCode = s.ShipmentLevelCode,
                                                        IsOperationalClosed = s.IsOperationalClosed,
                                                        AccountingCloseDate = s.AccountingCloseDate,
                                                        OperationalCloseDate = s.AccountingCloseDate,
                                                        LastUpdateDate = s.LastUpdateDate,
                                                        LTCWEdited = s.LTCWEdited,
                                                        MainCarriageATA = m.MainCarriageATA,
                                                        MainCarriageATD = m.MainCarriageATD,
                                                        MainCarriageCarrierCode = m.MainCarriageCarrierCard != null ? m.MainCarriageCarrierCard.Code : null,
                                                        MainCarriageCarrierId = m.MainCarriageCarrierId,
                                                        MainCarriageCarrierName = m.MainCarriageCarrierCard != null ? m.MainCarriageCarrierCard.EnglishName : null,
                                                        MainCarriageCarrierNumber = m.MainCarriageCarrierNumber,
                                                        MainCarriageCarrierWebSite = m.MainCarriageCarrierCard != null ? m.MainCarriageCarrierCard.Website : null,
                                                        MainCarriageETA = m.MainCarriageETA,
                                                        MainCarriageETD = m.MainCarriageETD,
                                                        MainCarriageFinalDestinationPortId = m.Transshipment3ToPortId != null ? m.Transshipment3ToPortId : m.Transshipment2ToPortId != null ? m.Transshipment2ToPortId : m.Transshipment1ToPortId != null ? m.Transshipment1ToPortId : m.MainCarriageToPortId,
                                                        MainCarriageFromPortCode = m.MainCarriageFromPort.Code,
                                                        MainCarriageFromPortCountryCode = m.MainCarriageFromPort.Country.Code,
                                                        MainCarriageFromPortCountryName = m.MainCarriageFromPort.Country.EnglishName,
                                                        MainCarriageFromPortId = m.MainCarriageFromPortId,
                                                        MainCarriageFromPortName = m.MainCarriageFromPort.EnglishName,
                                                        MainCarriageIsFromStack = m.MainCarriageIsFromStack,
                                                        MainCarriageToPortCode = m.MainCarriageToPort.Code,
                                                        MainCarriageToPortCountryCode = m.MainCarriageToPort.Country.Code,
                                                        MainCarriageToPortCountryName = m.MainCarriageToPort.Country.EnglishName,
                                                        MainCarriageToPortId = m.MainCarriageToPortId,
                                                        MainCarriageToPortName = m.MainCarriageToPort.EnglishName,
                                                        MainCarriageVesselId = m.MainCarriageVesselId,
                                                        MainHarmonize = s.MainHarmonize,
                                                        Master = m.Master,
                                                        MAWBOBLDate = m.MAWBOBLDate,
                                                        NextETA = s.NextETA,
                                                        NextETD = s.NextETD,
                                                        NextLegCode = s.NextLegCode,
                                                        NextLegName = s.NextLeg != null ? s.NextLeg.Name : null,
                                                        Notes = s.Notes,
                                                        Notify1AddressId = s.Notify1AddressId,
                                                        Notify1ContactId = s.Notify1ContactId,
                                                        Notify1Id = s.Notify1Id,
                                                        Notify1Name = s.Notify1Card != null ? s.Notify1Card.EnglishName : null,
                                                        Notify1Note = s.Notify1Card != null ? s.Notify1Card.Notes : null,
                                                        Notify2AddressId = s.Notify2AddressId,
                                                        Notify2ContactId = s.Notify2ContactId,
                                                        Notify2Id = s.Notify2Id,
                                                        Notify2Name = s.Notify2Card != null ? s.Notify2Card.EnglishName : null,
                                                        Notify2Note = s.Notify2Card != null ? s.Notify2Card.Notes : null,
                                                        PackagesQuantity = s.PackagesQuantity,
                                                        NumberOfContainers = s.NumberOfContainers,
                                                        NumberOfFollowUps = s.NumberOfFollowUps,
                                                        NumberOfPackages = s.NumberOfPackages,
                                                        OnCarriageATA = m.OnCarriageATA,
                                                        OnCarriageATD = m.OnCarriageATD,
                                                        OnCarriageCarrierCode = m.OnCarriageCarrierCard != null ? m.OnCarriageCarrierCard.Code : null,
                                                        OnCarriageCarrierId = m.OnCarriageCarrierId,
                                                        OnCarriageCarrierName = m.OnCarriageCarrierCard != null ? m.OnCarriageCarrierCard.EnglishName : null,
                                                        OnCarriageCarrierNumber = m.OnCarriageCarrierNumber,
                                                        OnCarriageCarrierWebSite = m.OnCarriageCarrierCard != null ? m.OnCarriageCarrierCard.Website : null,
                                                        OnCarriageETA = m.OnCarriageETA,
                                                        OnCarriageETD = m.OnCarriageETD,
                                                        OnCarriageFromPortCode = m.OnCarriageFromPort != null ? m.OnCarriageFromPort.Code : null,
                                                        OnCarriageFromPortCountryCode = m.OnCarriageFromPort != null ? m.OnCarriageFromPort.Country.Code : null,
                                                        OnCarriageFromPortCountryName = m.OnCarriageFromPort != null ? m.OnCarriageFromPort.Country.EnglishName : null,
                                                        OnCarriageFromPortId = m.OnCarriageFromPortId,
                                                        OnCarriageFromPortName = m.OnCarriageToPort != null ? m.OnCarriageToPort.EnglishName : null,
                                                        OnCarriageToPortCode = m.OnCarriageToPort != null ? m.OnCarriageToPort.Code : null,
                                                        OnCarriageToPortCountryCode = m.OnCarriageToPort != null ? m.OnCarriageToPort.Country.Code : null,
                                                        OnCarriageToPortCountryName = m.OnCarriageToPort != null ? m.OnCarriageToPort.Country.EnglishName : null,
                                                        OnCarriageToPortId = m.OnCarriageToPortId,
                                                        OnCarriageToPortName = m.OnCarriageToPort != null ? m.OnCarriageToPort.EnglishName : null,
                                                        OnCarriageTransportModeId = m.OnCarriageTransportModeId,
                                                        OnCarriageVesselId = m.OnCarriageVesselId,
                                                        PreCarriageATA = m.PreCarriageATA,
                                                        PreCarriageATD = m.PreCarriageATD,
                                                        PreCarriageCarrierCode = m.PreCarriageCarrierCard != null ? m.PreCarriageCarrierCard.Code : null,
                                                        PreCarriageCarrierId = m.PreCarriageCarrierId,
                                                        PreCarriageCarrierName = m.PreCarriageCarrierCard != null ? m.PreCarriageCarrierCard.EnglishName : null,
                                                        PreCarriageCarrierNumber = m.PreCarriageCarrierNumber,
                                                        PreCarriageCarrierWebSite = m.PreCarriageCarrierCard != null ? m.PreCarriageCarrierCard.Website : null,
                                                        PreCarriageETA = m.PreCarriageETA,
                                                        PreCarriageETD = m.PreCarriageETD,
                                                        PreCarriageFromPortCode = m.PreCarriageFromPort != null ? m.PreCarriageFromPort.Code : null,
                                                        PreCarriageFromPortCountryCode = m.PreCarriageFromPort != null ? m.PreCarriageFromPort.Country.Code : null,
                                                        PreCarriageFromPortCountryName = m.PreCarriageFromPort != null ? m.PreCarriageFromPort.Country.EnglishName : null,
                                                        PreCarriageFromPortId = m.PreCarriageFromPortId,
                                                        PreCarriageFromPortName = m.PreCarriageToPort != null ? m.PreCarriageToPort.EnglishName : null,
                                                        PreCarriageToPortCode = m.PreCarriageToPort != null ? m.PreCarriageToPort.Code : null,
                                                        PreCarriageToPortCountryCode = m.PreCarriageToPort != null ? m.PreCarriageToPort.Country.Code : null,
                                                        PreCarriageToPortCountryName = m.PreCarriageToPort != null ? m.PreCarriageToPort.Country.EnglishName : null,
                                                        PreCarriageToPortId = m.PreCarriageToPortId,
                                                        PreCarriageToPortName = m.PreCarriageToPort != null ? m.PreCarriageToPort.EnglishName : null,
                                                        PreCarriageTransportModeId = m.PreCarriageTransportModeId,
                                                        PreCarriageVesselId = m.PreCarriageVesselId,
                                                        CreatedByUserId = s.CreatedByUserId,
                                                        OrderChargeableWeight = s.OrderChargeableWeight,
                                                        OrderGrossWeight = s.OrderGrossWeight,
                                                        OrderIsDangerouseGoods = s.OrderIsDangerouseGoods,
                                                        OrderVolumetricWeight = s.OrderVolumetricWeight,
                                                        OtherPrepaidCollectId = s.OtherPrepaidCollectId,
                                                        TransportModeId = s.TransportModeId,
                                                        Tenant = s.Tenant,
                                                        ToPort = m.MainCarriageToPort.Code,
                                                        ToPortCountry = m.MainCarriageToPort.Country.Code,
                                                        ToPortName = m.MainCarriageToPort.EnglishName,
                                                        TransportModeName = s.TransportMode.Name,
                                                        Transshipment1AdditionalMAWBOBLBL = m.Transshipment1AdditionalMAWBOBLBL,
                                                        Transshipment1ATA = m.Transshipment1ATA,
                                                        Transshipment1ATD = m.Transshipment1ATD,
                                                        Transshipment1CarrierCode = m.Transshipment1CarrierCard != null ? m.Transshipment1CarrierCard.Code : null,
                                                        Transshipment1CarrierId = m.Transshipment1CarrierId,
                                                        Transshipment1CarrierName = m.Transshipment1CarrierCard != null ? m.Transshipment1CarrierCard.EnglishName : null,
                                                        Transshipment1CarrierNumber = m.Transshipment1CarrierNumber,
                                                        Transshipment1CarrierWebSite = m.Transshipment1CarrierCard != null ? m.Transshipment1CarrierCard.Website : null,
                                                        Transshipment1ETA = m.Transshipment1ETA,
                                                        Transshipment1ETD = m.Transshipment1ETD,
                                                        Transshipment1FromPortCode = m.Transshipment1FromPort != null ? m.Transshipment1FromPort.Code : null,
                                                        Transshipment1FromPortCountryCode = m.Transshipment1FromPort != null ? m.Transshipment1FromPort.Country.Code : null,
                                                        Transshipment1FromPortCountryName = m.Transshipment1FromPort != null ? m.Transshipment1FromPort.Country.EnglishName : null,
                                                        Transshipment1FromPortId = m.Transshipment1FromPortId,
                                                        Transshipment1FromPortName = m.Transshipment1FromPort != null ? m.Transshipment1FromPort.EnglishName : null,
                                                        Transshipment1ToPortCode = m.Transshipment1ToPort != null ? m.Transshipment1ToPort.Code : null,
                                                        Transshipment1ToPortCountryCode = m.Transshipment1ToPort != null ? m.Transshipment1ToPort.Country.Code : null,
                                                        Transshipment1ToPortCountryName = m.Transshipment1ToPort != null ? m.Transshipment1ToPort.Country.EnglishName : null,
                                                        Transshipment1ToPortId = m.Transshipment1ToPortId,
                                                        Transshipment1ToPortName = m.Transshipment1ToPort != null ? m.Transshipment1ToPort.EnglishName : null,
                                                        Transshipment1VesselId = m.Transshipment1VesselId,
                                                        Transshipment2AdditionalMAWBOBLBL = m.Transshipment2AdditionalMAWBOBLBL,
                                                        Transshipment2ATA = m.Transshipment2ATA,
                                                        Transshipment2ATD = m.Transshipment2ATD,
                                                        Transshipment2CarrierCode = m.Transshipment2CarrierCard != null ? m.Transshipment2CarrierCard.Code : null,
                                                        Transshipment2CarrierId = m.Transshipment2CarrierId,
                                                        Transshipment2CarrierName = m.Transshipment2CarrierCard != null ? m.Transshipment2CarrierCard.EnglishName : null,
                                                        Transshipment2CarrierNumber = m.Transshipment2CarrierNumber,
                                                        Transshipment2CarrierWebSite = m.Transshipment2CarrierCard != null ? m.Transshipment2CarrierCard.Website : null,
                                                        Transshipment2ETA = m.Transshipment2ETA,
                                                        Transshipment2ETD = m.Transshipment2ETD,
                                                        Transshipment2FromPortCode = m.Transshipment2FromPort != null ? m.Transshipment2FromPort.Code : null,
                                                        Transshipment2FromPortCountryCode = m.Transshipment2FromPort != null ? m.Transshipment2FromPort.Country.Code : null,
                                                        Transshipment2FromPortCountryName = m.Transshipment2FromPort != null ? m.Transshipment2FromPort.Country.EnglishName : null,
                                                        Transshipment2FromPortId = m.Transshipment2FromPortId,
                                                        Transshipment2FromPortName = m.Transshipment2FromPort != null ? m.Transshipment2FromPort.EnglishName : null,
                                                        Transshipment2ToPortCode = m.Transshipment2ToPort != null ? m.Transshipment2ToPort.Code : null,
                                                        Transshipment2ToPortCountryCode = m.Transshipment2ToPort != null ? m.Transshipment2ToPort.Country.Code : null,
                                                        Transshipment2ToPortCountryName = m.Transshipment2ToPort != null ? m.Transshipment2ToPort.Country.EnglishName : null,
                                                        Transshipment2ToPortId = m.Transshipment2ToPortId,
                                                        Transshipment2ToPortName = m.Transshipment2ToPort != null ? m.Transshipment2ToPort.EnglishName : null,
                                                        Transshipment2VesselId = m.Transshipment2VesselId,
                                                        Transshipment3AdditionalMAWBOBLBL = m.Transshipment3AdditionalMAWBOBLBL,
                                                        Transshipment3ATA = m.Transshipment3ATA,
                                                        Transshipment3ATD = m.Transshipment3ATD,
                                                        Transshipment3CarrierCode = m.Transshipment3CarrierCard != null ? m.Transshipment3CarrierCard.Code : null,
                                                        Transshipment3CarrierId = m.Transshipment3CarrierId,
                                                        Transshipment3CarrierName = m.Transshipment3CarrierCard != null ? m.Transshipment3CarrierCard.EnglishName : null,
                                                        Transshipment3CarrierNumber = m.Transshipment3CarrierNumber,
                                                        Transshipment3CarrierWebSite = m.Transshipment3CarrierCard != null ? m.Transshipment3CarrierCard.Website : null,
                                                        Transshipment3ETA = m.Transshipment3ETA,
                                                        Transshipment3ETD = m.Transshipment3ETD,
                                                        Transshipment3FromPortCode = m.Transshipment3FromPort != null ? m.Transshipment3FromPort.Code : null,
                                                        Transshipment3FromPortCountryCode = m.Transshipment3FromPort != null ? m.Transshipment3FromPort.Country.Code : null,
                                                        Transshipment3FromPortCountryName = m.Transshipment3FromPort != null ? m.Transshipment3FromPort.Country.EnglishName : null,
                                                        Transshipment3FromPortId = m.Transshipment3FromPortId,
                                                        Transshipment3FromPortName = m.Transshipment3FromPort != null ? m.Transshipment3FromPort.EnglishName : null,
                                                        Transshipment3ToPortCode = m.Transshipment3ToPort != null ? m.Transshipment3ToPort.Code : null,
                                                        Transshipment3ToPortCountryCode = m.Transshipment3ToPort != null ? m.Transshipment3ToPort.Country.Code : null,
                                                        Transshipment3ToPortCountryName = m.Transshipment3ToPort != null ? m.Transshipment3ToPort.Country.EnglishName : null,
                                                        Transshipment3ToPortId = m.Transshipment3ToPortId,
                                                        Transshipment3ToPortName = m.Transshipment3ToPort != null ? m.Transshipment2ToPort.EnglishName : null,
                                                        Transshipment3VesselId = m.Transshipment3VesselId,
                                                        SalesmanUserId = s.SalesmanUserId,
                                                        AccountManagerUserId = s.AccountManagerUserId,
                                                        SCI = s.SCI,
                                                        ShipmentCustomerTypeCode = s.ShipmentCustomerTypeCode,
                                                        ShipmentDeliveryIndex = s.ShipmentDeliveryIndex,
                                                        ShipmentContainerReturnIndex = s.ShipmentContainerReturnIndex,
                                                        MasterShipmentDataId = s.MasterShipmentDataId,
                                                        ShipmentPayableStatusCode = s.ShipmentPayableStatusCode,
                                                        ShipmentPayableStatusName = s.ShipmentPayableStatus != null ? s.ShipmentPayableStatus.Name : null,
                                                        ShipmentNumber = s.ShipmentNumber,
                                                        ShipmentPickUpIndex = s.ShipmentPickUpIndex,
                                                        ShipmentReceivableStatusCode = s.ShipmentReceivableStatusCode,
                                                        ShipmentReceivableStatusName = s.ShipmentReceivableStatus != null ? s.ShipmentReceivableStatus.Name : null,
                                                        ShipmentTypeId = s.ShipmentTypeId,
                                                        ShipmentTypeName = s.ShipmentType.Name,
                                                        ShipperAddressId = s.ShipperAddressId,
                                                        ShipperAddressOneTime = s.ShipperAddressOneTime,
                                                        ShipperId = s.ShipperId,
                                                        ShipperContactId = s.ShipperContactId,
                                                        ShipperName = s.ShipperCard != null ? s.ShipperCard.EnglishName : null,
                                                        ShipperNote = s.ShipperCard != null ? s.ShipperCard.Notes : null,
                                                        ShipperNotExporterAddressId = s.ShipperNotExporterAddressId,
                                                        ShipperNotExporterContactId = s.ShipperNotExporterContactId,
                                                        ShipperNotExporterId = s.ShipperNotExporterId,
                                                        ShipperNotExporterName = s.ShipperNotExporterCard != null ? s.ShipperNotExporterCard.EnglishName : null,
                                                        ShipperNotExporterNote = s.ShipperNotExporterCard != null ? s.ShipperNotExporterCard.Notes : null,
                                                        ShipperReference1 = s.ShipperReference1,
                                                        ShipperReference2 = s.ShipperReference2,
                                                        UpdatedByUserId = s.UpdatedByUserId,
                                                        UpdatedByUserName = s.UpdatedByUser != null ? s.UpdatedByUser.Contact.EnglishName : null,
                                                        VolumeInCBM = s.VolumeInCBM,
                                                        Volume = s.Volume,
                                                        VolumetricWeight = s.VolumetricWeight,
                                                        VolumeUnitCode = s.VolumeUnitCode,
                                                        GrossWeightUnitCode = s.GrossWeightUnitCode,
                                                        QuoteId = s.QuoteId,
                                                        QuoteNumber = s.QuoteNumber,
                                                        Ratio = s.Ratio,
                                                        DimFactor = s.DimFactor,
                                                        MainCarriageFullCarrierNumber = (m.MainCarriageCarrierNumber != null && m.MainCarriageCarrierCard != null) ? m.MainCarriageCarrierCard.Code + m.MainCarriageCarrierNumber : null,
                                                        Transshipment1FullCarrierNumber = (m.Transshipment1CarrierNumber != null && m.Transshipment1CarrierPrefix != null) ? m.Transshipment1CarrierPrefix + m.Transshipment1CarrierNumber : null,
                                                        Transshipment2FullCarrierNumber = (m.Transshipment2CarrierNumber != null && m.Transshipment2CarrierPrefix != null) ? m.Transshipment2CarrierPrefix + m.Transshipment2CarrierNumber : null,
                                                        Transshipment3FullCarrierNumber = (m.Transshipment3CarrierNumber != null && m.Transshipment3CarrierPrefix != null) ? m.Transshipment3CarrierPrefix + m.Transshipment3CarrierNumber : null,
                                                        AsAgreedFreight = s.AsAgreedFreight,
                                                        AsAgreedOtherCharges = s.AsAgreedOtherCharges,
                                                        AccountNumber = s.AccountNumber,
                                                        FinalArrivalDate = s.FinalArrivalDate,
                                                        EstimatedFinalArrivalDate = s.EstimatedFinalArrivalDate,
                                                        ActualFinalArrivalDate = s.ActualFinalArrivalDate,
                                                        MoveTypeId = s.MoveTypeId,
                                                        AMSBL = s.AMSBL,
                                                        ColoaderId = s.ColoaderId,
                                                        ColoaderAddressId = s.ColoaderAddressId,
                                                        ColoaderContactId = s.ColoaderContactId,
                                                        ColoaderReference1 = s.ColoaderReference1,
                                                        ColoaderName = s.Coloader != null ? s.Coloader.EnglishName : null,
                                                        ColoaderNote = s.Coloader != null ? s.Coloader.Notes : null,
                                                        CustomClearancePointName = s.CustomClearancePoint != null ? s.CustomClearancePoint.EnglishName : null,
                                                        CustomClearancePointNote = s.CustomClearancePoint != null ? s.CustomClearancePoint.Notes : null,
                                                        CustomClearancePointId = s.CustomClearancePointId,
                                                        CustomClearancePointAddressId = s.CustomClearancePointAddressId,
                                                        CustomClearancePointContactId = s.CustomClearancePointContactId,
                                                        CustomClearancePointReference1 = s.CustomClearancePointReference1,
                                                        CASSCode = s.CASSCode,
                                                        SLAC = s.SLAC,
                                                        FreelancerId = s.FreelancerId,
                                                        FreelancerAddressId = s.FreelancerAddressId,
                                                        FreelancerContactId = s.FreelancerContactId,
                                                        FreelancerName = s.FreelancerCard != null ? s.FreelancerCard.EnglishName : null,
                                                        ARInvoiceIssued = s.ARInvoiceIssued,
                                                        CreditNoteIssued = s.CreditNoteIssued,
                                                        ProductCode = s.ProductCode,
                                                        LastStatusLogDate = s.LastStatusLogDate,
                                                        ExceptionDescription = s.ExceptionDescription,
                                                        ExceptionDate = s.ExceptionDate,
                                                        HasException = s.HasException,
                                                        ExceptionResolvedDescription = s.ExceptionResolvedDescription,
                                                        LastExceptionDescription = s.LastExceptionDescription,
                                                        CustomConnectToShipment = s.CustomConnectToShipment,
                                                        CustomsDeclarationNumber = s.CustomsDeclarationNumber,
                                                        NumberOfInsidePackages = s.NumberOfInsidePackages,
                                                        NumberOfInsidePackagesDetails = s.NumberOfInsidePackagesDetails,
                                                        ComputedStatusId = s.ComputedStatusId,
                                                        ComputedStatusDate = s.ComputedStatusDate,
                                                        StatusId = s.StatusId,
                                                        StatusName = s.EntityStatus != null ? s.EntityStatus.Name : null,
                                                        StatusDate = s.StatusDate,
                                                        StatusLocation = s.StatusLocation,
                                                        ForeignPartnerCountryCode = s.ForeignPartnerCountryCode,
                                                        AgentSharedManifestRef = s.AgentSharedManifestRef,
                                                        IsManifestSentToAgent = s.IsManifestSentToAgent,
                                                        GrossWeight = s.GrossWeight,
                                                        ChargeableWeight = s.ChargeableWeight,
                                                        IsNewARInvoiceBlocked = s.IsNewARInvoiceBlocked,
                                                        OperationalDate = s.OperationalDate,
                                                        ReleasingAgentId = s.ConsigneeId,
                                                        ReleasingAgentAddressId = s.ConsigneeAddressId,
                                                        ReleasingAgentContactId = s.ConsigneeContactId,
                                                        ReleasingAgentReference1 = s.ReleasingAgentReference1,
                                                        ReleasingAgentReference2 = s.ReleasingAgentReference2,
                                                        ReleasingAgentName = s.ReleasingAgentCard != null ? s.ReleasingAgentCard.EnglishName : null,
                                                        ReleasingAgentNote = s.ReleasingAgentCard != null ? s.ReleasingAgentCard.Notes : null,
                                                        ValueOfGoods = s.ValueOfGoods,
                                                        ManifestLastSharingDate = s.ManifestLastSharingDate,
                                                        SpecialServicesTypeId = s.SpecialServicesTypeId,
                                                        OnForwardingATA = s.OnForwardingATA,
                                                        OnForwardingATD = s.OnForwardingATD,
                                                        OnForwardingCarrierCode = s.OnForwardingCarrierCard != null ? s.OnForwardingCarrierCard.Code : null,
                                                        OnForwardingCarrierId = s.OnForwardingCarrierId,
                                                        OnForwardingCarrierName = s.OnForwardingCarrierCard != null ? s.OnForwardingCarrierCard.EnglishName : null,
                                                        OnForwardingCarrierNumber = s.OnForwardingCarrierNumber,
                                                        OnForwardingCarrierWebSite = s.OnForwardingCarrierCard != null ? s.OnForwardingCarrierCard.Website : null,
                                                        OnForwardingETA = s.OnForwardingETA,
                                                        OnForwardingETD = s.OnForwardingETD,
                                                        OnForwardingFromPortCode = s.OnForwardingFromPort != null ? s.OnForwardingFromPort.Code : null,
                                                        OnForwardingFromPortCountryCode = s.OnForwardingFromPort != null ? s.OnForwardingFromPort.Country.Code : null,
                                                        OnForwardingFromPortCountryName = s.OnForwardingFromPort != null ? s.OnForwardingFromPort.Country.EnglishName : null,
                                                        OnForwardingFromPortId = s.OnForwardingFromPortId,
                                                        OnForwardingFromPortName = s.OnForwardingToPort != null ? s.OnForwardingToPort.EnglishName : null,
                                                        OnForwardingToPortCode = s.OnForwardingToPort != null ? s.OnForwardingToPort.Code : null,
                                                        OnForwardingToPortCountryCode = s.OnForwardingToPort != null ? s.OnForwardingToPort.Country.Code : null,
                                                        OnForwardingToPortCountryName = s.OnForwardingToPort != null ? s.OnForwardingToPort.Country.EnglishName : null,
                                                        OnForwardingToPortId = s.OnForwardingToPortId,
                                                        OnForwardingToPortName = s.OnForwardingToPort != null ? s.OnForwardingToPort.EnglishName : null,
                                                        OnForwardingTransportModeId = s.OnForwardingTransportModeId,
                                                        OnForwardingVesselId = s.OnForwardingVesselId,
                                                        PreForwardingATA = s.PreForwardingATA,
                                                        PreForwardingATD = s.PreForwardingATD,
                                                        PreForwardingCarrierCode = s.PreForwardingCarrierCard != null ? s.PreForwardingCarrierCard.Code : null,
                                                        PreForwardingCarrierId = s.PreForwardingCarrierId,
                                                        PreForwardingCarrierName = s.PreForwardingCarrierCard != null ? s.PreForwardingCarrierCard.EnglishName : null,
                                                        PreForwardingCarrierNumber = s.PreForwardingCarrierNumber,
                                                        PreForwardingCarrierWebSite = s.PreForwardingCarrierCard != null ? s.PreForwardingCarrierCard.Website : null,
                                                        PreForwardingETA = s.PreForwardingETA,
                                                        PreForwardingETD = s.PreForwardingETD,
                                                        PreForwardingFromPortCode = s.PreForwardingFromPort != null ? s.PreForwardingFromPort.Code : null,
                                                        PreForwardingFromPortCountryCode = s.PreForwardingFromPort != null ? s.PreForwardingFromPort.Country.Code : null,
                                                        PreForwardingFromPortCountryName = s.PreForwardingFromPort != null ? s.PreForwardingFromPort.Country.EnglishName : null,
                                                        PreForwardingFromPortId = s.PreForwardingFromPortId,
                                                        PreForwardingFromPortName = s.PreForwardingToPort != null ? s.PreForwardingToPort.EnglishName : null,
                                                        PreForwardingToPortCode = s.PreForwardingToPort != null ? s.PreForwardingToPort.Code : null,
                                                        PreForwardingToPortCountryCode = s.PreForwardingToPort != null ? s.PreForwardingToPort.Country.Code : null,
                                                        PreForwardingToPortCountryName = s.PreForwardingToPort != null ? s.PreForwardingToPort.Country.EnglishName : null,
                                                        PreForwardingToPortId = s.PreForwardingToPortId,
                                                        PreForwardingToPortName = s.PreForwardingToPort != null ? s.PreForwardingToPort.EnglishName : null,
                                                        PreForwardingTransportModeId = s.PreForwardingTransportModeId,
                                                        PreForwardingVesselId = s.PreForwardingVesselId,
                                                    };

            List<ShipmentPM> securedShipmentPMs = new List<ShipmentPM>();
            foreach (ShipmentPM shipmentPM in shipmentPMList)
            {
                Shipment ship = (from s in repository.context.Shipments
                                 where s.Id == shipmentPM.Id
                                 select s).FirstOrDefault();

                shipmentPM.Field1 = new CustomFieldClass("Field1", "Shipment", ship.Field1);
                shipmentPM.Field2 = new CustomFieldClass("Field2", "Shipment", ship.Field2);
                shipmentPM.Field3 = new CustomFieldClass("Field3", "Shipment", ship.Field3);
                shipmentPM.Field4 = new CustomFieldClass("Field4", "Shipment", ship.Field4);
                shipmentPM.Field5 = new CustomFieldClass("Field5", "Shipment", ship.Field5);
                shipmentPM.Field6 = new CustomFieldClass("Field6", "Shipment", ship.Field6);
                shipmentPM.Field7 = new CustomFieldClass("Field7", "Shipment", ship.Field7);
                shipmentPM.Field8 = new CustomFieldClass("Field8", "Shipment", ship.Field8);
                shipmentPM.Field9 = new CustomFieldClass("Field9", "Shipment", ship.Field9);
                shipmentPM.Field10 = new CustomFieldClass("Field10", "Shipment", ship.Field10);
                shipmentPM.Field11 = new CustomFieldClass("Field11", "Shipment", ship.Field11);
                shipmentPM.Field12 = new CustomFieldClass("Field12", "Shipment", ship.Field12);
                shipmentPM.Field13 = new CustomFieldClass("Field13", "Shipment", ship.Field13);
                shipmentPM.Field14 = new CustomFieldClass("Field14", "Shipment", ship.Field14);
                shipmentPM.Field15 = new CustomFieldClass("Field15", "Shipment", ship.Field15);
                shipmentPM.Field16 = new CustomFieldClass("Field16", "Shipment", ship.Field16);
                shipmentPM.Field17 = new CustomFieldClass("Field17", "Shipment", ship.Field17);
                shipmentPM.Field18 = new CustomFieldClass("Field18", "Shipment", ship.Field18);
                shipmentPM.Field19 = new CustomFieldClass("Field19", "Shipment", ship.Field19);
                shipmentPM.Field20 = new CustomFieldClass("Field20", "Shipment", ship.Field20);
                shipmentPM.Field21 = new CustomFieldClass("Field21", "Shipment", ship.Field21);
                shipmentPM.Field22 = new CustomFieldClass("Field22", "Shipment", ship.Field22);
                shipmentPM.Field23 = new CustomFieldClass("Field23", "Shipment", ship.Field23);
                shipmentPM.Field24 = new CustomFieldClass("Field24", "Shipment", ship.Field24);
                shipmentPM.Field25 = new CustomFieldClass("Field25", "Shipment", ship.Field25);
                shipmentPM.Field26 = new CustomFieldClass("Field26", "Shipment", ship.Field26);
                shipmentPM.Field27 = new CustomFieldClass("Field27", "Shipment", ship.Field27);
                shipmentPM.Field28 = new CustomFieldClass("Field28", "Shipment", ship.Field28);
                shipmentPM.Field29 = new CustomFieldClass("Field29", "Shipment", ship.Field29);
                shipmentPM.Field30 = new CustomFieldClass("Field30", "Shipment", ship.Field30);
                shipmentPM.Field31 = new CustomFieldClass("Field31", "Shipment", ship.Field31);
                shipmentPM.Field32 = new CustomFieldClass("Field32", "Shipment", ship.Field32);
                shipmentPM.Field33 = new CustomFieldClass("Field33", "Shipment", ship.Field33);
                shipmentPM.Field34 = new CustomFieldClass("Field34", "Shipment", ship.Field34);
                shipmentPM.Field35 = new CustomFieldClass("Field35", "Shipment", ship.Field35);
                shipmentPM.Field36 = new CustomFieldClass("Field36", "Shipment", ship.Field36);
                shipmentPM.Field37 = new CustomFieldClass("Field37", "Shipment", ship.Field37);
                shipmentPM.Field38 = new CustomFieldClass("Field38", "Shipment", ship.Field38);
                shipmentPM.Field39 = new CustomFieldClass("Field39", "Shipment", ship.Field39);
                shipmentPM.Field40 = new CustomFieldClass("Field40", "Shipment", ship.Field40);


                ShipmentPM securedPM = new ShipmentPM();
                SecuredMapping.GetMappedPM(shipmentPM, securedPM, "Shipment", tenant);

                securedShipmentPMs.Add(securedPM);

                securedShipmentPMs = BranchPermitionsFilter.AddUserBranchRestrictionFilters<ShipmentPM>(new QueryOperations(), securedShipmentPMs.AsQueryable<ShipmentPM>(), tenant).ToList();
                securedShipmentPMs = ProductPermitionsFilter.AddUserProductRestrictionFilters<ShipmentPM>(new QueryOperations(), securedShipmentPMs.AsQueryable<ShipmentPM>(), tenant).ToList();

            }

            return securedShipmentPMs;
            #endregion
        }

        public List<ShipmentPM> GetShipmentPMsByMasterIdAndTenantForAutomation(string masterId, int tenant)
        {
            #region shipmentpm temp code

            List<Shipment> shipmentLists = (from s in repository.context.Shipments
                                            where s.Tenant == tenant && s.MasterShipmentDataId == masterId && s.Id != masterId && ((s.ShipmentLevelCode == "H") || (s.ShipmentLevelCode == "D") || s.ShipmentLevelCode == "A")
                                            select s).ToList();


            List<ShipmentPM> securedShipmentPMs = new List<ShipmentPM>();
            if (shipmentLists.Count() > 0)
            {

                List<ShipmentAdditionalCloudData> shipmentAdditionalCloudDataLists = new List<ShipmentAdditionalCloudData>();
                List<ShipmentComputedFields> entityComputedFieldsLists = new List<ShipmentComputedFields>();
                ShipmentMasterData m = (from a in repository.context.ShipmentMasterDatas
                                        where a.Id == masterId
                                        select a).FirstOrDefault();

            
                    List<string> shipmentIds = shipmentLists.GroupBy(d => d.Id).Select(d => d.First().Id).ToList();
                    ShipmentComputedFieldsRepository shipmentComputedFieldsRepository = new ShipmentComputedFieldsRepository(tenant);
                    entityComputedFieldsLists = shipmentComputedFieldsRepository.GetShipmentComputedFieldsByIds(shipmentIds, tenant).ToList();


                if (EntityChangeHelper.IsShowLogBoxAutomationFields())
                {
                    shipmentAdditionalCloudDataLists = (from a in repository.context.ShipmentAdditionalCloudDatas
                                                        where shipmentIds.Contains(a.Id) && a.Tenant == tenant
                                                        select a).ToList();
                }


                foreach (Shipment shipment in shipmentLists)
                {
                    var shipmentPM = new ShipmentPM();
                    shipmentPM.Id = shipment.Id;
                    shipmentPM.Tenant = shipment.Tenant;
                    shipmentPM.CreatedByUserId = shipment.CreatedByUserId;
                    shipmentPM.UpdatedByUserId = shipment.UpdatedByUserId;
                    shipmentPM.DirectionId = shipment.DirectionId;
                    shipmentPM.ShipmentLevelCode = shipment.ShipmentLevelCode;
                    shipmentPM.CustomerId = shipment.CustomerId;
                    shipmentPM.AccountManagerUserId = shipment.AccountManagerUserId;
                    shipmentPM.BranchId = shipment.BranchId;
                    shipmentPM.DepartmentId = shipment.DepartmentId;
                    shipmentPM.AgentId = shipment.AgentId;
                    shipmentPM.AgentComputed = shipment.AgentComputed;
                    shipmentPM.ComputedShipmentNumber = shipment.ComputedShipmentNumber;
                    shipmentPM.IsAccountingClosed = shipment.IsAccountingClosed;
                    shipmentPM.IsOperationalClosed = shipment.IsOperationalClosed;
                    shipmentPM.OriginShipmentId = shipment.OriginShipmentId;
                    shipmentPM.SalesmanUserId = shipment.SalesmanUserId;
                    shipmentPM.StatusId = shipment.StatusId;
                    shipmentPM.TransportModeId = shipment.TransportModeId;
                    shipmentPM.MainCarriageTransportModeId = shipment.TransportModeId;
                    shipmentPM.IncotermId = shipment.IncotermId;
                    shipmentPM.CustomerContactId = shipment.CustomerContactId;
                    shipmentPM.AgentContactId = shipment.AgentContactId;
                    shipmentPM.ShipmentTypeId = shipment.ShipmentTypeId;
                    shipmentPM.MasterShipmentDataId = shipment.MasterShipmentDataId;
                    shipmentPM.LastSharedEventDate = shipment.LastSharedEventDate;
                    shipmentPM.LastSharedEventId = shipment.LastSharedEventId;
                    shipmentPM.IsAccrualsApproved = shipment.IsAccrualsApproved;

                    

                    if (m != null)
                    {
                        shipmentPM.MainCarriageETD = m.MainCarriageETD;
                        shipmentPM.MainCarriageATD = m.MainCarriageATD;
                        shipmentPM.MainCarriageETA = m.MainCarriageETA;
                        shipmentPM.MainCarriageATA = m.MainCarriageATA;
                        shipmentPM.MainCarriageCarrierId = m.MainCarriageCarrierId;
                        shipmentPM.FinalDistenationPortId = m.Transshipment3ToPortId != null ? m.Transshipment3ToPortId : m.Transshipment2ToPortId != null ? m.Transshipment2ToPortId : m.Transshipment1ToPortId != null ? m.Transshipment1ToPortId : m.MainCarriageToPortId;
                        shipmentPM.CutoffDate = m.CutoffDate;
 
                    }


                    shipmentPM.Field1 = new CustomFieldClass("Field1", "Shipment", shipment.Field1);
                    shipmentPM.Field2 = new CustomFieldClass("Field2", "Shipment", shipment.Field2);
                    shipmentPM.Field3 = new CustomFieldClass("Field3", "Shipment", shipment.Field3);
                    shipmentPM.Field4 = new CustomFieldClass("Field4", "Shipment", shipment.Field4);
                    shipmentPM.Field5 = new CustomFieldClass("Field5", "Shipment", shipment.Field5);
                    shipmentPM.Field6 = new CustomFieldClass("Field6", "Shipment", shipment.Field6);
                    shipmentPM.Field7 = new CustomFieldClass("Field7", "Shipment", shipment.Field7);
                    shipmentPM.Field8 = new CustomFieldClass("Field8", "Shipment", shipment.Field8);
                    shipmentPM.Field9 = new CustomFieldClass("Field9", "Shipment", shipment.Field9);
                    shipmentPM.Field10 = new CustomFieldClass("Field10", "Shipment", shipment.Field10);
                    shipmentPM.Field11 = new CustomFieldClass("Field11", "Shipment", shipment.Field11);
                    shipmentPM.Field12 = new CustomFieldClass("Field12", "Shipment", shipment.Field12);
                    shipmentPM.Field13 = new CustomFieldClass("Field13", "Shipment", shipment.Field13);
                    shipmentPM.Field14 = new CustomFieldClass("Field14", "Shipment", shipment.Field14);
                    shipmentPM.Field15 = new CustomFieldClass("Field15", "Shipment", shipment.Field15);
                    shipmentPM.Field16 = new CustomFieldClass("Field16", "Shipment", shipment.Field16);
                    shipmentPM.Field17 = new CustomFieldClass("Field17", "Shipment", shipment.Field17);
                    shipmentPM.Field18 = new CustomFieldClass("Field18", "Shipment", shipment.Field18);
                    shipmentPM.Field19 = new CustomFieldClass("Field19", "Shipment", shipment.Field19);
                    shipmentPM.Field20 = new CustomFieldClass("Field20", "Shipment", shipment.Field20);
                    shipmentPM.Field21 = new CustomFieldClass("Field21", "Shipment", shipment.Field21);
                    shipmentPM.Field22 = new CustomFieldClass("Field22", "Shipment", shipment.Field22);
                    shipmentPM.Field23 = new CustomFieldClass("Field23", "Shipment", shipment.Field23);
                    shipmentPM.Field24 = new CustomFieldClass("Field24", "Shipment", shipment.Field24);
                    shipmentPM.Field25 = new CustomFieldClass("Field25", "Shipment", shipment.Field25);
                    shipmentPM.Field26 = new CustomFieldClass("Field26", "Shipment", shipment.Field26);
                    shipmentPM.Field27 = new CustomFieldClass("Field27", "Shipment", shipment.Field27);
                    shipmentPM.Field28 = new CustomFieldClass("Field28", "Shipment", shipment.Field28);
                    shipmentPM.Field29 = new CustomFieldClass("Field29", "Shipment", shipment.Field29);
                    shipmentPM.Field30 = new CustomFieldClass("Field30", "Shipment", shipment.Field30);
                    shipmentPM.Field31 = new CustomFieldClass("Field31", "Shipment", shipment.Field31);
                    shipmentPM.Field32 = new CustomFieldClass("Field32", "Shipment", shipment.Field32);
                    shipmentPM.Field33 = new CustomFieldClass("Field33", "Shipment", shipment.Field33);
                    shipmentPM.Field34 = new CustomFieldClass("Field34", "Shipment", shipment.Field34);
                    shipmentPM.Field35 = new CustomFieldClass("Field35", "Shipment", shipment.Field35);
                    shipmentPM.Field36 = new CustomFieldClass("Field36", "Shipment", shipment.Field36);
                    shipmentPM.Field37 = new CustomFieldClass("Field37", "Shipment", shipment.Field37);
                    shipmentPM.Field38 = new CustomFieldClass("Field38", "Shipment", shipment.Field38);
                    shipmentPM.Field39 = new CustomFieldClass("Field39", "Shipment", shipment.Field39);
                    shipmentPM.Field40 = new CustomFieldClass("Field40", "Shipment", shipment.Field40);


                   
                    var entityComputedFields = entityComputedFieldsLists.Where(d => d.Id == shipment.Id).FirstOrDefault();
                    if (entityComputedFields != null)
                    {
                        shipmentPM.IsDepositionRequired = entityComputedFields.IsDepositionRequired;
                        shipmentPM.IsDigitalSignRequired = entityComputedFields.IsDigitalSignRequired;
                        shipmentPM.IsRequestedDocuments = entityComputedFields.IsRequestedDocuments;
                        shipmentPM.BookingConfirmationSentDate = entityComputedFields.BookingConfirmationSent;
                        shipmentPM.PreAlertSentDate = entityComputedFields.PreAlertSent;
                        shipmentPM.DeliveryNoticeSentDate = entityComputedFields.DeliveryNoticeSent;
                        shipmentPM.ExpectedArrivalNoticeSentDate = entityComputedFields.ExpectedArrivalNoticeSent;
                        shipmentPM.ArrivalNoticeSentDate = entityComputedFields.ArrivalNoticeSent;
                        shipmentPM.T1ReceivedDate = entityComputedFields.T1Received;
                    }

                        #region ShipmentAdditionalCloudDatas
                        shipmentPM.IsImporterApprovalRequired = GetIsImporterApprovalRequried(shipment.Id, shipment.Tenant, shipmentAdditionalCloudDataLists);
                        #endregion

                 //   }


                    ShipmentPM securedPM = new ShipmentPM();
                    SecuredMapping.GetMappedPM(shipmentPM, securedPM, "Shipment", tenant);

                    securedShipmentPMs.Add(securedPM);

                    securedShipmentPMs = BranchPermitionsFilter.AddUserBranchRestrictionFilters<ShipmentPM>(new QueryOperations(), securedShipmentPMs.AsQueryable<ShipmentPM>(), tenant).ToList();
                    securedShipmentPMs = ProductPermitionsFilter.AddUserProductRestrictionFilters<ShipmentPM>(new QueryOperations(), securedShipmentPMs.AsQueryable<ShipmentPM>(), tenant).ToList();

                }




            }




















            return securedShipmentPMs;
            #endregion
        }



        public List<ShipmentPM> GetShipmentPMsByMasterIdAndTenantForSharedManifest(string masterId, int tenant)
        {
            #region shipmentpm temp code
            IQueryable<ShipmentPM> shipmentPMList = from s in repository.context.Shipments.Include("MainCarriageFromPort").Include("MainCarriageFromPort.Country").Include("Incoterm").Include("MainCarriageCarrierCard").Include("MainCarriageToPort").Include("MainCarriageToPort.Country").Include("Notify1Card").Include("Notify2Card").Include("Notify2Card").Include("MainCarriageToPort.Country").Include("Transshipment1CarrierCard").Include("Transshipment1FromPort").Include("Transshipment1FromPort.Country").Include("Transshipment1FromPort").Include("Transshipment1ToPort").Include("Transshipment1ToPort.Country").Include("Transshipment2CarrierCard").Include("Transshipment2FromPort").Include("Transshipment2FromPort.Country").Include("Transshipment2ToPort").Include("Transshipment2ToPort.Country").Include("Transshipment3CarrierCard").Include("Transshipment3FromPort").Include("Transshipment3FromPort.Country").Include("ShipperCard").Include("MoveType").Include("AgentCard").Include("ConsigneeCard").Include("CustomerCard")
                                                    join sm in repository.context.ShipmentMasterDatas.Include("Port").Include("Port.Country")
                                                    on s.MasterShipmentDataId equals sm.Id into shipmentJoin
                                                    from m in shipmentJoin.DefaultIfEmpty()
                                                    where s.Tenant == tenant && s.MasterShipmentDataId == masterId && s.Id != masterId && ((s.ShipmentLevelCode == "H") || (s.ShipmentLevelCode == "D") || s.ShipmentLevelCode == "A")
                                                    select new ShipmentPM()
                                                    {
                                                        MoveTypeCode = s.MoveType != null ? s.MoveType.Code : null,
                                                        MoveTypeName = s.MoveType != null ? s.MoveType.MoveTypeEnglishName : null,


                                                        AgentAddressId = s.AgentAddressId,
                                                        AgentContactId = s.AgentContactId,
                                                        AgentId = s.AgentId,
                                                        AgentComputed = s.AgentComputed,
                                                        ComputedShipmentNumber = s.ComputedShipmentNumber,

                                                        AgentName = s.AgentCard != null ? s.AgentCard.EnglishName : null,
                                                        AgentNote = s.AgentCard != null ? s.AgentCard.Notes : null,
                                                        AgentReference1 = s.AgentReference1,
                                                        AgentReference2 = s.AgentReference2,


                                                        BranchId = s.BranchId,
                                                        ChargeableWeightInKG = s.ChargeableWeightInKG,
                                                        ChargeableWeightEdited = s.ChargeableWeightEdited,
                                                        ConsigneeAddressId = s.ConsigneeAddressId,
                                                        ConsigneeAddressOneTime = s.ConsigneeAddressOneTime,
                                                        ConsigneeContactId = s.ConsigneeContactId,
                                                        ConsigneeId = s.ConsigneeId,
                                                        ConsigneeName = s.ConsigneeCard != null ? s.ConsigneeCard.EnglishName : null,
                                                        ConsigneeNote = s.ConsigneeCard != null ? s.ConsigneeCard.Notes : null,

                                                        ConsigneeReference1 = s.ConsigneeReference1,
                                                        ConsigneeReference2 = s.ConsigneeReference2,
                                                        CreateDateTime = s.CreateDateTime,


                                                        CustomerAddressId = s.CustomerAddressId,
                                                        CustomerContactId = s.CustomerContactId,
                                                        CustomerId = s.CustomerId,
                                                        CustomerName = s.CustomerCard != null ? s.CustomerCard.EnglishName : null,
                                                        CustomerNote = s.CustomerCard != null ? s.CustomerCard.Notes : null,
                                                        CustomerReference1 = s.CustomerReference1,
                                                        CustomerReference2 = s.CustomerReference2,


                                                        DescriptionOfGoods = s.DescriptionOfGoods,
                                                        DimensionsUnitCode = s.DimensionsUnitCode,
                                                        ChargeableWeightUnitCode = s.ChargeableWeightUnitCode,
                                                        DirectionId = s.DirectionId,
                                                        DirectionName = s.Direction.Name,

                                                        FinalDistenationPortId = m.Transshipment3ToPortId != null ? m.Transshipment3ToPortId : m.Transshipment2ToPortId != null ? m.Transshipment2ToPortId : m.Transshipment1ToPortId != null ? m.Transshipment1ToPortId : m.MainCarriageToPortId,
                                                        MainCarriageFinalDestinationETA = m.MainCarriageFinalDestinationETA,
                                                        MainCarriageFinalDestinationATA = m.MainCarriageFinalDestinationATA,

                                                        FreightPrepaidCollectId = s.FreightPrepaidCollectId,
                                                        FromPort = m.MainCarriageFromPort.Code,
                                                        FromPortCountry = m.MainCarriageFromPort.Country.Code,
                                                        FromPortName = m.MainCarriageFromPort.EnglishName,
                                                        GrossWeightInKG = s.GrossWeightInKG,
                                                        GrossWeightPerStorageDays = s.GrossWeightPerStorageDays,
                                                        GrossWeightPerTon = s.GrossWeightPerTon,
                                                        GrossWeightEdited = s.GrossWeightEdited,
                                                        HAWBDate = s.HAWBDate,
                                                        House = s.House,
                                                        Id = s.Id,
                                                        IncotermId = s.IncotermId,
                                                        IncotermCode = s.Incoterm != null ? s.Incoterm.Code : null,
                                                        IncotermName = s.Incoterm != null ? s.Incoterm.Name : null,
                                                        IsAccountingClosed = s.IsAccountingClosed,
                                                        IsCancelled = s.IsCancelled,
                                                        CancelledDate = s.CancelledDate,
                                                        IsDangerous = s.IsDangerous,
                                                        ShipmentLevelCode = s.ShipmentLevelCode,
                                                        IsOperationalClosed = s.IsOperationalClosed,
                                                        AccountingCloseDate = s.AccountingCloseDate,
                                                        OperationalCloseDate = s.AccountingCloseDate,
                                                        LastUpdateDate = s.LastUpdateDate,
                                                        LTCWEdited = s.LTCWEdited,
                                                        MainCarriageATA = m.MainCarriageATA,
                                                        MainCarriageATD = m.MainCarriageATD,
                                                        MainCarriageCarrierCode = m.MainCarriageCarrierCard != null ? m.MainCarriageCarrierCard.Code : null,
                                                        MainCarriageCarrierId = m.MainCarriageCarrierId,
                                                        MainCarriageCarrierName = m.MainCarriageCarrierCard != null ? m.MainCarriageCarrierCard.EnglishName : null,
                                                        MainCarriageCarrierNumber = m.MainCarriageCarrierNumber,
                                                        MainCarriageCarrierWebSite = m.MainCarriageCarrierCard != null ? m.MainCarriageCarrierCard.Website : null,
                                                        MainCarriageETA = m.MainCarriageETA,
                                                        MainCarriageETD = m.MainCarriageETD,
                                                        MainCarriageFinalDestinationPortId = m.Transshipment3ToPortId != null ? m.Transshipment3ToPortId : m.Transshipment2ToPortId != null ? m.Transshipment2ToPortId : m.Transshipment1ToPortId != null ? m.Transshipment1ToPortId : m.MainCarriageToPortId,
                                                        MainCarriageFromPortCode = m.MainCarriageFromPort.Code,
                                                        MainCarriageFromPortCountryCode = m.MainCarriageFromPort.Country.Code,
                                                        MainCarriageFromPortCountryName = m.MainCarriageFromPort.Country.EnglishName,
                                                        MainCarriageFromPortId = m.MainCarriageFromPortId,
                                                        MainCarriageFromPortName = m.MainCarriageFromPort.EnglishName,
                                                        MainCarriageIsFromStack = m.MainCarriageIsFromStack,
                                                        MainCarriageToPortCode = m.MainCarriageToPort.Code,
                                                        MainCarriageToPortCountryCode = m.MainCarriageToPort.Country.Code,
                                                        MainCarriageToPortCountryName = m.MainCarriageToPort.Country.EnglishName,
                                                        MainCarriageToPortId = m.MainCarriageToPortId,
                                                        MainCarriageToPortName = m.MainCarriageToPort.EnglishName,
                                                        MainCarriageVesselId = m.MainCarriageVesselId,
                                                        MainHarmonize = s.MainHarmonize,
                                                        Master = m.Master,
                                                        MAWBOBLDate = m.MAWBOBLDate,

                                                        Notes = s.Notes,
                                                        Notify1AddressId = s.Notify1AddressId,
                                                        Notify1ContactId = s.Notify1ContactId,
                                                        Notify1Id = s.Notify1Id,
                                                        Notify1Name = s.Notify1Card != null ? s.Notify1Card.EnglishName : null,
                                                        Notify1Note = s.Notify1Card != null ? s.Notify1Card.Notes : null,
                                                        Notify2AddressId = s.Notify2AddressId,
                                                        Notify2ContactId = s.Notify2ContactId,
                                                        Notify2Id = s.Notify2Id,
                                                        Notify2Name = s.Notify2Card != null ? s.Notify2Card.EnglishName : null,
                                                        Notify2Note = s.Notify2Card != null ? s.Notify2Card.Notes : null,
                                                        PackagesQuantity = s.PackagesQuantity,
                                                        NumberOfContainers = s.NumberOfContainers,

                                                        NumberOfPackages = s.NumberOfPackages,

                                                        CreatedByUserId = s.CreatedByUserId,

                                                        OtherPrepaidCollectId = s.OtherPrepaidCollectId,
                                                        TransportModeId = s.TransportModeId,
                                                        Tenant = s.Tenant,
                                                        ToPort = m.MainCarriageToPort.Code,
                                                        ToPortCountry = m.MainCarriageToPort.Country.Code,
                                                        ToPortName = m.MainCarriageToPort.EnglishName,
                                                        TransportModeName = s.TransportMode.Name,
                                                        Transshipment1AdditionalMAWBOBLBL = m.Transshipment1AdditionalMAWBOBLBL,
                                                        Transshipment1ATA = m.Transshipment1ATA,
                                                        Transshipment1ATD = m.Transshipment1ATD,
                                                        Transshipment1CarrierCode = m.Transshipment1CarrierCard != null ? m.Transshipment1CarrierCard.Code : null,
                                                        Transshipment1CarrierId = m.Transshipment1CarrierId,
                                                        Transshipment1CarrierName = m.Transshipment1CarrierCard != null ? m.Transshipment1CarrierCard.EnglishName : null,
                                                        Transshipment1CarrierNumber = m.Transshipment1CarrierNumber,
                                                        Transshipment1CarrierWebSite = m.Transshipment1CarrierCard != null ? m.Transshipment1CarrierCard.Website : null,
                                                        Transshipment1ETA = m.Transshipment1ETA,
                                                        Transshipment1ETD = m.Transshipment1ETD,
                                                        Transshipment1FromPortCode = m.Transshipment1FromPort != null ? m.Transshipment1FromPort.Code : null,
                                                        Transshipment1FromPortCountryCode = m.Transshipment1FromPort != null ? m.Transshipment1FromPort.Country.Code : null,
                                                        Transshipment1FromPortCountryName = m.Transshipment1FromPort != null ? m.Transshipment1FromPort.Country.EnglishName : null,
                                                        Transshipment1FromPortId = m.Transshipment1FromPortId,
                                                        Transshipment1FromPortName = m.Transshipment1FromPort != null ? m.Transshipment1FromPort.EnglishName : null,
                                                        Transshipment1ToPortCode = m.Transshipment1ToPort != null ? m.Transshipment1ToPort.Code : null,
                                                        Transshipment1ToPortCountryCode = m.Transshipment1ToPort != null ? m.Transshipment1ToPort.Country.Code : null,
                                                        Transshipment1ToPortCountryName = m.Transshipment1ToPort != null ? m.Transshipment1ToPort.Country.EnglishName : null,
                                                        Transshipment1ToPortId = m.Transshipment1ToPortId,
                                                        Transshipment1ToPortName = m.Transshipment1ToPort != null ? m.Transshipment1ToPort.EnglishName : null,
                                                        Transshipment1VesselId = m.Transshipment1VesselId,
                                                        Transshipment2AdditionalMAWBOBLBL = m.Transshipment2AdditionalMAWBOBLBL,
                                                        Transshipment2ATA = m.Transshipment2ATA,
                                                        Transshipment2ATD = m.Transshipment2ATD,
                                                        Transshipment2CarrierCode = m.Transshipment2CarrierCard != null ? m.Transshipment2CarrierCard.Code : null,
                                                        Transshipment2CarrierId = m.Transshipment2CarrierId,
                                                        Transshipment2CarrierName = m.Transshipment2CarrierCard != null ? m.Transshipment2CarrierCard.EnglishName : null,
                                                        Transshipment2CarrierNumber = m.Transshipment2CarrierNumber,
                                                        Transshipment2CarrierWebSite = m.Transshipment2CarrierCard != null ? m.Transshipment2CarrierCard.Website : null,
                                                        Transshipment2ETA = m.Transshipment2ETA,
                                                        Transshipment2ETD = m.Transshipment2ETD,
                                                        Transshipment2FromPortCode = m.Transshipment2FromPort != null ? m.Transshipment2FromPort.Code : null,
                                                        Transshipment2FromPortCountryCode = m.Transshipment2FromPort != null ? m.Transshipment2FromPort.Country.Code : null,
                                                        Transshipment2FromPortCountryName = m.Transshipment2FromPort != null ? m.Transshipment2FromPort.Country.EnglishName : null,
                                                        Transshipment2FromPortId = m.Transshipment2FromPortId,
                                                        Transshipment2FromPortName = m.Transshipment2FromPort != null ? m.Transshipment2FromPort.EnglishName : null,
                                                        Transshipment2ToPortCode = m.Transshipment2ToPort != null ? m.Transshipment2ToPort.Code : null,
                                                        Transshipment2ToPortCountryCode = m.Transshipment2ToPort != null ? m.Transshipment2ToPort.Country.Code : null,
                                                        Transshipment2ToPortCountryName = m.Transshipment2ToPort != null ? m.Transshipment2ToPort.Country.EnglishName : null,
                                                        Transshipment2ToPortId = m.Transshipment2ToPortId,
                                                        Transshipment2ToPortName = m.Transshipment2ToPort != null ? m.Transshipment2ToPort.EnglishName : null,
                                                        Transshipment2VesselId = m.Transshipment2VesselId,
                                                        Transshipment3AdditionalMAWBOBLBL = m.Transshipment3AdditionalMAWBOBLBL,
                                                        Transshipment3ATA = m.Transshipment3ATA,
                                                        Transshipment3ATD = m.Transshipment3ATD,
                                                        Transshipment3CarrierCode = m.Transshipment3CarrierCard != null ? m.Transshipment3CarrierCard.Code : null,
                                                        Transshipment3CarrierId = m.Transshipment3CarrierId,
                                                        Transshipment3CarrierName = m.Transshipment3CarrierCard != null ? m.Transshipment3CarrierCard.EnglishName : null,
                                                        Transshipment3CarrierNumber = m.Transshipment3CarrierNumber,
                                                        Transshipment3CarrierWebSite = m.Transshipment3CarrierCard != null ? m.Transshipment3CarrierCard.Website : null,
                                                        Transshipment3ETA = m.Transshipment3ETA,
                                                        Transshipment3ETD = m.Transshipment3ETD,
                                                        Transshipment3FromPortCode = m.Transshipment3FromPort != null ? m.Transshipment3FromPort.Code : null,
                                                        Transshipment3FromPortCountryCode = m.Transshipment3FromPort != null ? m.Transshipment3FromPort.Country.Code : null,
                                                        Transshipment3FromPortCountryName = m.Transshipment3FromPort != null ? m.Transshipment3FromPort.Country.EnglishName : null,
                                                        Transshipment3FromPortId = m.Transshipment3FromPortId,
                                                        Transshipment3FromPortName = m.Transshipment3FromPort != null ? m.Transshipment3FromPort.EnglishName : null,
                                                        Transshipment3ToPortCode = m.Transshipment3ToPort != null ? m.Transshipment3ToPort.Code : null,
                                                        Transshipment3ToPortCountryCode = m.Transshipment3ToPort != null ? m.Transshipment3ToPort.Country.Code : null,
                                                        Transshipment3ToPortCountryName = m.Transshipment3ToPort != null ? m.Transshipment3ToPort.Country.EnglishName : null,
                                                        Transshipment3ToPortId = m.Transshipment3ToPortId,
                                                        Transshipment3ToPortName = m.Transshipment3ToPort != null ? m.Transshipment2ToPort.EnglishName : null,
                                                        Transshipment3VesselId = m.Transshipment3VesselId,
                                                        SalesmanUserId = s.SalesmanUserId,


                                                        ShipmentCustomerTypeCode = s.ShipmentCustomerTypeCode,
                                                        ShipmentDeliveryIndex = s.ShipmentDeliveryIndex,
                                                        ShipmentContainerReturnIndex = s.ShipmentContainerReturnIndex,
                                                        MasterShipmentDataId = s.MasterShipmentDataId,

                                                        ShipmentNumber = s.ShipmentNumber,

                                                        ShipmentTypeId = s.ShipmentTypeId,
                                                        ShipmentTypeName = s.ShipmentType.Name,
                                                        ShipperAddressId = s.ShipperAddressId,
                                                        ShipperAddressOneTime = s.ShipperAddressOneTime,
                                                        ShipperId = s.ShipperId,
                                                        ShipperContactId = s.ShipperContactId,
                                                        ShipperName = s.ShipperCard != null ? s.ShipperCard.EnglishName : null,
                                                        ShipperNote = s.ShipperCard != null ? s.ShipperCard.Notes : null,

                                                        ShipperReference1 = s.ShipperReference1,
                                                        ShipperReference2 = s.ShipperReference2,
                                                        UpdatedByUserId = s.UpdatedByUserId,

                                                        VolumeInCBM = s.VolumeInCBM,
                                                        Volume = s.Volume,
                                                        VolumetricWeight = s.VolumetricWeight,
                                                        VolumeUnitCode = s.VolumeUnitCode,
                                                        GrossWeightUnitCode = s.GrossWeightUnitCode,

                                                        MoveTypeId = s.MoveTypeId,
                                                        AgentSharedManifestRef = s.AgentSharedManifestRef,
                                                        IsManifestSentToAgent = s.IsManifestSentToAgent,
                                                        GrossWeight = s.GrossWeight,
                                                        ChargeableWeight = s.ChargeableWeight,
                                                        IsNewARInvoiceBlocked = s.IsNewARInvoiceBlocked,
                                                        OperationalDate = s.OperationalDate,

                                                        ValueOfGoods = s.ValueOfGoods,
                                                        ManifestLastSharingDate = s.ManifestLastSharingDate,
                                                    };

            List<ShipmentPM> securedShipmentPMs = new List<ShipmentPM>();
            foreach (ShipmentPM shipmentPM in shipmentPMList)
            {

                ShipmentPM securedPM = new ShipmentPM();
                SecuredMapping.GetMappedPM(shipmentPM, securedPM, "Shipment", tenant);

                securedShipmentPMs.Add(securedPM);

                securedShipmentPMs = BranchPermitionsFilter.AddUserBranchRestrictionFilters<ShipmentPM>(new QueryOperations(), securedShipmentPMs.AsQueryable<ShipmentPM>(), tenant).ToList();
                securedShipmentPMs = ProductPermitionsFilter.AddUserProductRestrictionFilters<ShipmentPM>(new QueryOperations(), securedShipmentPMs.AsQueryable<ShipmentPM>(), tenant).ToList();

            }

            return securedShipmentPMs;
            #endregion
        }



        public ShipmentPM GetSinglePMByForwarderShipmentNumber(string forwarderShipmentNumber, int tenant)
        {
            if (!string.IsNullOrEmpty(forwarderShipmentNumber))
            {
                Shipment shipment = (from a in repository.context.Shipments.Include("EntityStatus").Include("ComputedEntityStatus").Include("ShipmentType").Include("Incoterm").Include("ShipmentReceivableStatus").Include("ShipmentPayableStatus").Include("ShipmentLevel").Include("NextLeg").Include("ShipmentType").Include("ShipmentMasterData").Include("SpecialServicesType").Include("MoveType")
                                     where a.ForwarderShipmentNumber == forwarderShipmentNumber && a.Tenant == tenant && !a.IsCancelled
                                     select a).FirstOrDefault();

                if (shipment != null)
                {
                    ShipmentMasterData masterData = (from a in repository.context.ShipmentMasterDatas
                                                     where a.Id == shipment.MasterShipmentDataId
                                                     select a).FirstOrDefault();

                    ShipmentPM shipmentPM = new ShipmentPM();

                    shipmentPM = MapShipmentToShipmentPM(shipmentPM, shipment, null, masterData, true);
                    //shipmentPM.ToCountryCode = !string.IsNullOrEmpty(shipmentPM.MainCarriageFinalDestinationPortCountryCode) ? shipmentPM.MainCarriageFinalDestinationPortCountryCode : shipmentPM.ToPortCountryCode,
                    //shipmentPM.FromCountryCode = f.ShipmentLevelCode == "H" && string.IsNullOrEmpty(f.MasterShipmentDataId) ? f.FromPortCountryCode : f.MainCarriageFromPortCountryCode,
                    ShipmentPM securedPM = new ShipmentPM();
                    securedPM = SecuredMapping.GetMappedPM(shipmentPM, securedPM, "Shipment", tenant);

                    ShipmentPM returnShipment = BranchPermitionsFilter.AddUserBranchRestrictionFilters(new QueryOperations(), securedPM, tenant);//securedPM;
                    returnShipment = ProductPermitionsFilter.AddUserProductRestrictionFilters(new QueryOperations(), securedPM, tenant);
                    var CLoudData = (from a in repository.context.ShipmentAdditionalCloudDatas
                                     where a.Id == shipment.Id
                                     select a).FirstOrDefault();

                    if (CLoudData != null)
                    {
                        returnShipment.DeclarationXMLData = CLoudData.DeclarationXmlData;
                        returnShipment.DeclarationWCOXml = CLoudData.DeclarationWCOXml;
                        returnShipment.ApproveDateTime = CLoudData.ApproveDateTime;
                        returnShipment.IsImporterApprovalRequired = CLoudData.IsImporterApprovalRequried;
                        returnShipment.VersionApproved = CLoudData.VersionApproved;
                        returnShipment.ShipmentAddtionalDataXML = CLoudData.ShipmentAddtionalDataXML;
                        returnShipment.SendUpdatesToAgentEnabled = CLoudData.SendUpdatesToAgentEnabled;
                        returnShipment.DocsSentToAgent = CLoudData.DocsSentToAgent;
                        returnShipment.ApprovedBy = CLoudData.ApprovedByUserName;
                        returnShipment.DocumentsApprovedByUserName = CLoudData.DocumentsApprovedByUserName;
                        returnShipment.IsUserIDNumberRequired = CLoudData.IsUserIDNumberRequired;
                        returnShipment.UserIdNumberUpdateDate = CLoudData.UserIdNumberUpdateDate;
                        returnShipment.UserIdNumberXMLData = CLoudData.UserIdNumberXMLData;
                        returnShipment.UserIdNumber = CLoudData.UserIdNumber;
                        returnShipment.PaymentRequestXML = CLoudData.PaymentRequestXML;
                        returnShipment.PaymentDateTime = CLoudData.PaymentDateTime;
                        returnShipment.IsPaymentRequired = CLoudData.IsPaymentRequired;
                    }

                    return returnShipment;
                }

                else
                {
                    return null;
                }
            }
            return null;
        }


        #region DashBoard Region

        public IQueryable<ShipmentPM> GetShipmentsForDashBoard(int tenant, string customerId)
        {
            IQueryable<ShipmentDataView> dataViews = repository.GetShipmentViewsByTenant(tenant);
            IQueryable<ShipmentPM> shipmentPMList;
            IQueryable<ShipmentPM> shipmentPMQuery = (from a in dataViews
                                                      where a.Tenant == tenant && a.IsCancelled == false && a.ShipmentLevelCode != "C"
                                                      select new ShipmentPM()
                                                      {
                                                          CreateDateTime = a.CreateDateTime,
                                                          DirectionId = a.DirectionId,
                                                          DirectionName = a.DirectionName,
                                                          TransportModeId = a.TransportModeId,
                                                          CustomerId = a.CustomerId,
                                                          FromPort = a.MainCarriageFromPortCode,
                                                          FromPortCountry = a.FromPortCountryCode,
                                                          FromPortCountryName = a.FromPortCountryName,
                                                          FromPortName = a.MainCarriageFromPortName,
                                                          Id = a.Id,
                                                          ShipmentLevelCode = a.ShipmentLevelCode,
                                                          ShipmentLevelName = a.ShipmentLevelName,
                                                          MainCarriageFinalDestinationPortId = a.Transshipment3ToPortId != null ? a.Transshipment3ToPortId : a.Transshipment2ToPortId != null ? a.Transshipment2ToPortId : a.Transshipment1ToPortId != null ? a.Transshipment1ToPortId : a.MainCarriageToPortId,
                                                          MainCarriageFinalDestinationETA = a.MainCarriageFinalDestinationETA,
                                                          MainCarriageFinalDestinationATA = a.MainCarriageFinalDestinationATA,
                                                          MainCarriageFromPortCode = a.MainCarriageFromPortCode,
                                                          MainCarriageFromPortCountryCode = a.MainCarriageFromPortCountryCode,
                                                          MainCarriageFromPortCountryName = a.MainCarriageFromPortCountryName,
                                                          MainCarriageFromPortId = a.MainCarriageFromPortId,
                                                          MainCarriageFromPortName = a.MainCarriageFromPortName,
                                                          MainCarriageToPortCode = a.MainCarriageToPortCode,
                                                          MainCarriageToPortCountryCode = a.MainCarriageToPortCountryCode,
                                                          MainCarriageToPortCountryName = a.MainCarriageToPortCountryName,
                                                          MainCarriageToPortId = a.MainCarriageToPortId,
                                                          MainCarriageToPortName = a.MainCarriageToPortName,
                                                          Tenant = a.Tenant,
                                                          ToPort = a.MainCarriageToPortCode,
                                                          ToPortCountry = a.ToPortCountryCode,
                                                          ToPortCountryName = a.ToPortCountryName,
                                                          ToPortName = a.MainCarriageToPortName,
                                                          TransportModeName = a.TransportModeName,
                                                          GrossWeightInKG = a.GrossWeightInKG,
                                                          GrossWeightPerStorageDays = a.GrossWeightPerStorageDays,
                                                          GrossWeightPerTon = a.GrossWeightPerTon,
                                                          ChargeableWeightInKG = a.ChargeableWeightInKG,
                                                          ProfitInProfitCurrency = a.ProfitInProfitCurrency,
                                                          ProfitInLocalCurrency = a.ProfitInLocalCurrency,
                                                          OpenReceivablesInLocalCurrency = a.OpenReceivablesInLocalCurrency,
                                                          MainCarriageFullCarrierNumber = (a.MainCarriageCarrierNumber != null) ? a.MainCarriageCarrierCode + a.MainCarriageCarrierNumber : null,
                                                          IsCancelled = a.IsCancelled,
                                                          CancelledDate = a.CancelledDate,
                                                          CustomerName = a.CustomerName,
                                                          AccountedReceivablesInLocalCurrency = a.AccountedReceivablesInLocalCurrency,
                                                          AccountedReceivablesInProfitCurrency = a.AccountedReceivablesInProfitCurrency,
                                                          AccountedPayablesInLocalCurrency = a.AccountedPayablesInLocalCurrency,
                                                          AccountedPayablesInProfitCurrency = a.AccountedPayablesInProfitCurrency,
                                                          OpenReceivablesInProfitCurrency = a.OpenReceivablesInProfitCurrency,
                                                          OpenPayablesInLocalCurrency = a.OpenPayablesInLocalCurrency,
                                                          OpenPayablesInProfitCurrency = a.OpenPayablesInProfitCurrency,
                                                          NumberOfInsidePackages = a.NumberOfInsidePackages,
                                                          NumberOfInsidePackagesDetails = a.NumberOfInsidePackagesDetails,
                                                          CustomConnectToShipment = a.CustomConnectToShipment,
                                                          ComputedStatusId = a.ComputedStatusId,
                                                          ComputedStatusDate = a.ComputedStatusDate,
                                                          IsNewARInvoiceBlocked = a.IsNewARInvoiceBlocked,
                                                      });
            if (!string.IsNullOrEmpty(customerId))
            {
                shipmentPMQuery = from a in shipmentPMQuery
                                  where a.CustomerId == customerId
                                  select a;
            }
#if false
                        List<ShipmentPM> shipmentPMList = (from s in repository.context.Shipments.Include("Direction").Include("FromPort.Country").Include("ShipmentLevel").Include("ToPort.Country").Include("TransportMode")
                                                           join sm in repository.context.ShipmentMasterDatas.Include("MainCarriageFromPort").Include("MainCarriageFromPort.Country").Include("MainCarriageToPort").Include("MainCarriageToPort.Country")
                                                           on s.MasterShipmentDataId equals sm.Id into shipmentJoin
                                                           from m in shipmentJoin.DefaultIfEmpty()
                                                           where s.Tenant == tenant && s.IsCancelled == false && ((s.ShipmentLevelCode != "C"))
                                                           select new ShipmentPM()
                                                           {
                                                               CreateDateTime = s.CreateDateTime,
                                                               DirectionId = s.DirectionId,
                                                               DirectionName = s.Direction.Name,
                                                               TransportModeId = s.TransportModeId,
                                                               CustomerId = s.CustomerId,
                                                               FromPort = m.MainCarriageFromPort.Code,
                                                               FromPortCountry = m != null ? m.MainCarriageFromPort.Country.Code : s.FromPort.Country.Code,
                                                               FromPortCountryName = m != null ? m.MainCarriageFromPort.Country.EnglishName : s.FromPort.Country.EnglishName,
                                                               FromPortName = m.MainCarriageFromPort.EnglishName,
                                                               Id = s.Id,
                                                               ShipmentLevelCode = s.ShipmentLevelCode,
                                                               ShipmentLevelName = s.ShipmentLevel != null ? s.ShipmentLevel.Name : null,
                                                               MainCarriageFinalDestinationPortId = m.Transshipment3ToPortId != null ? m.Transshipment3ToPortId : m.Transshipment2ToPortId != null ? m.Transshipment2ToPortId : m.Transshipment1ToPortId != null ? m.Transshipment1ToPortId : m.MainCarriageToPortId,
                                                               MainCarriageFinalDestinationETA = m.MainCarriageFinalDestinationETA,
                                                               MainCarriageFinalDestinationATA = m.MainCarriageFinalDestinationATA,                      
                                                               MainCarriageFromPortCode = m.MainCarriageFromPort.Code,
                                                               MainCarriageFromPortCountryCode = m.MainCarriageFromPort.Country.Code,
                                                               MainCarriageFromPortCountryName = m.MainCarriageFromPort.Country.EnglishName,
                                                               MainCarriageFromPortId = m.MainCarriageFromPortId,
                                                               MainCarriageFromPortName = m.MainCarriageFromPort.EnglishName,
                                                               MainCarriageToPortCode = m.MainCarriageToPort.Code,
                                                               MainCarriageToPortCountryCode = m.MainCarriageToPort.Country.Code,
                                                               MainCarriageToPortCountryName = m.MainCarriageToPort.Country.EnglishName,
                                                               MainCarriageToPortId = m.MainCarriageToPortId,
                                                               MainCarriageToPortName = m.MainCarriageToPort.EnglishName,
                                                               Tenant = s.Tenant,
                                                               ToPort = m.MainCarriageToPort.Code,
                                                               ToPortCountry = m != null ? m.MainCarriageToPort.Country.Code : s.ToPort.Country.Code,
                                                               ToPortCountryName = m != null ? m.MainCarriageToPort.Country.EnglishName : s.ToPort.Country.EnglishName,
                                                               ToPortName = m.MainCarriageToPort.EnglishName,
                                                               TransportModeName = s.TransportMode.Name,
                                                               GrossWeightInKG = s.GrossWeightInKG,
                                                               GrossWeightPerStorageDays = s.GrossWeightPerStorageDays,
                                                               GrossWeightPerTon = s.GrossWeightPerTon,
                                                               ChargeableWeightInKG = s.ChargeableWeightInKG,
                                                               ProfitInProfitCurrency = s.ProfitInProfitCurrency,
                                                               ProfitInLocalCurrency = s.ProfitInLocalCurrency,
                                                               OpenReceivablesInLocalCurrency = s.OpenReceivablesInLocalCurrency,
                                                               MainCarriageFullCarrierNumber = (m.MainCarriageCarrierNumber != null && m.MainCarriageCarrierCard != null) ? m.MainCarriageCarrierCard.Code + m.MainCarriageCarrierNumber : null,
                                                               Transshipment1FullCarrierNumber = (m.Transshipment1CarrierNumber != null && s.Transshipment1CarrierPrefix != null) ? s.Transshipment1CarrierPrefix + m.Transshipment1CarrierNumber : null,
                                                               Transshipment2FullCarrierNumber = (m.Transshipment2CarrierNumber != null && s.Transshipment2CarrierPrefix != null) ? s.Transshipment2CarrierPrefix + m.Transshipment2CarrierNumber : null,
                                                               Transshipment3FullCarrierNumber = (m.Transshipment3CarrierNumber != null && s.Transshipment3CarrierPrefix != null) ? s.Transshipment3CarrierPrefix + m.Transshipment3CarrierNumber : null,
                                                           }).ToList();


#endif

            shipmentPMList = BranchPermitionsFilter.AddUserBranchRestrictionFilters<ShipmentPM>(new QueryOperations(), shipmentPMQuery, tenant);
            shipmentPMList = ProductPermitionsFilter.AddUserProductRestrictionFilters<ShipmentPM>(new QueryOperations(), shipmentPMQuery, tenant);
            return shipmentPMList;
        }

        /* DashBoard By Months */
        public List<DashBoardClass> GetShipmentsByMonthDashBoard(string type, int lastMonths, int lastDays, int currentTenant, string customerId)
        {
            IQueryable<Shipment> shipments = repository.GetShipments(currentTenant);
            if (string.IsNullOrEmpty(customerId))
            {
                shipments = from s in shipments
                            where s.Tenant == currentTenant && s.IsCancelled == false && s.CustomerId != null
                            select s;
            }
            else
            {
                shipments = from s in shipments
                            where s.CustomerId == customerId && s.Tenant == currentTenant && s.IsCancelled == false
                            select s;
            }

            shipments = BranchPermitionsFilter.AddUserBranchRestrictionFilters<Shipment>(new QueryOperations(), shipments, currentTenant);
            shipments = ProductPermitionsFilter.AddUserProductRestrictionFilters<Shipment>(new QueryOperations(), shipments, currentTenant);

            List<DashBoardClass> resulList = null;
            // int lastDays;
            DateTime fiXedDateDays;

            int days = 0;
            int months = 0;
            days = lastDays;
            DateTime lastDate = DateTime.Today.Date.AddDays(days);

            if (lastMonths != 0)
            {
                months = lastMonths + 1;
                lastDate = DateTime.Today.Date.AddMonths(months);
            }
            DateTime FromDateQuery = TenantServerConfigration.GetCurrentDateTime(currentTenant);


            if (type == "CreateDate" || type == null)
            {
                resulList = (from s in shipments
                             where s.CreateDateTime > lastDate

                             group s by new
                             {
                                 s.CreateDateTime.Day,
                                 s.CreateDateTime.Month,
                                 s.CreateDateTime.Year,
                                 s.DirectionId,
                                 s.TransportModeId,
                             } into m

                             orderby m.Key.Year, m.Key.Month, m.Key.Day
                             select new DashBoardClass()
                             {
                                 day = m.Key.Day,
                                 month = m.Key.Month,
                                 year = m.Key.Year,
                                 directionID = m.Key.DirectionId,
                                 transportModeID = m.Key.TransportModeId,
                                 total = m.Count(),
                                 sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                 sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                 totalProfitInLocalCurrency = m.Sum(s => s.ProfitInLocalCurrency),
                                 totalProfitInProfitCurrency = m.Sum(s => s.ProfitInProfitCurrency),
                                 ReceivablesInLocalCurrency = (m.Sum(s => s.OpenReceivablesInLocalCurrency) + m.Sum(s => s.AccountedReceivablesInLocalCurrency)),
                                 ReceivablesInProfitCurrency = (m.Sum(s => s.OpenReceivablesInProfitCurrency) + m.Sum(s => s.AccountedReceivablesInProfitCurrency)),
                             }).ToList();
            }
            else
            {
                resulList = (from s in shipments
                             where s.OperationalDate > lastDate && s.OperationalDate <= FromDateQuery

                             group s by new
                             {
                                 s.OperationalDate.Value.Day,
                                 s.OperationalDate.Value.Month,
                                 s.OperationalDate.Value.Year,
                                 s.DirectionId,
                                 s.TransportModeId,
                             } into m

                             orderby m.Key.Year, m.Key.Month, m.Key.Day
                             select new DashBoardClass()
                             {
                                 day = m.Key.Day,
                                 month = m.Key.Month,
                                 year = m.Key.Year,
                                 directionID = m.Key.DirectionId,
                                 transportModeID = m.Key.TransportModeId,
                                 total = m.Count(),
                                 sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                 sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                 totalProfitInLocalCurrency = m.Sum(s => s.ProfitInLocalCurrency),
                                 totalProfitInProfitCurrency = m.Sum(s => s.ProfitInProfitCurrency),
                                 ReceivablesInLocalCurrency = (m.Sum(s => s.OpenReceivablesInLocalCurrency) + m.Sum(s => s.AccountedReceivablesInLocalCurrency)),
                                 ReceivablesInProfitCurrency = (m.Sum(s => s.OpenReceivablesInProfitCurrency) + m.Sum(s => s.AccountedReceivablesInProfitCurrency)),
                             }).ToList();

            }

            foreach (DashBoardClass d in resulList)
            {
                d.Date = new DateTime(d.year, d.month, d.day);

                int day = Convert.ToInt32(d.Date.DayOfWeek);
                DateTime startOfWeek = d.Date.AddDays((-1 * day));
                DateTime endOfWeek = d.Date.AddDays((6 - day));

                d.StartOfTheWeek = startOfWeek;
                d.EndOfTheWeek = endOfWeek;
                if (days == -364 || lastMonths != 0)
                {
                    d.DateRange = startOfWeek.Month + "/" + startOfWeek.Year;
                }
                else
                {
                    d.DateRange = startOfWeek.Day + "/" + startOfWeek.Month;

                    //  d.DateRange=d.Date.Day+"/"+d.Date.Month+" - "+
                }
            }

            return resulList;
        }

        public List<DashBoardClass> GetShipmentsByCreateOperationalDate(string type, DateTime? FromDate, DateTime? ToDate, int currentTenant, string customerId, string directionId, string transportmodeid)
        {
            bool AddYearFlag = false;
            if (FromDate.Value.Year != ToDate.Value.Year)
                AddYearFlag = true;
            IQueryable<Shipment> shipments = repository.GetShipments(currentTenant);

            if (string.IsNullOrEmpty(customerId))
            {
                shipments = from s in shipments
                            where s.Tenant == currentTenant && s.ShipmentLevelCode != "C" && s.IsCancelled == false
                            select s;
            }
            else
            {
                shipments = from s in shipments
                            where s.CustomerId == customerId && s.Tenant == currentTenant && s.ShipmentLevelCode != "C" && s.IsCancelled == false
                            select s;
            }

            if (!string.IsNullOrEmpty(directionId) && directionId != "All")
            {
                shipments = (from f in shipments where f.DirectionId == directionId select f);
            }
            if (!string.IsNullOrEmpty(transportmodeid) && transportmodeid != "All")
            {
                shipments = (from f in shipments where f.TransportModeId == transportmodeid select f);
            }




            shipments = BranchPermitionsFilter.AddUserBranchRestrictionFilters<Shipment>(new QueryOperations(), shipments, currentTenant);
            shipments = ProductPermitionsFilter.AddUserProductRestrictionFilters<Shipment>(new QueryOperations(), shipments, currentTenant);

            List<DashBoardClass> resulList = null;
            int days = 0;
            int lastDays = (FromDate.Value - ToDate.Value).Days;
            DateTime QueryFromDate = new DateTime(FromDate.Value.Year, FromDate.Value.Month, FromDate.Value.Day, 0, 0, 0);

            DateTime QueryToDate = ToDate.Value.AddDays(1);
            lastDays = lastDays *= -1;
            int Perdio = lastDays <= 6 ? 1 : lastDays / 5;

            var dates = new List<DateTime?>();
            Dictionary<string, DashBoardClass> Listt = new Dictionary<string, DashBoardClass>();

            string DatePeriod = "";
            var Entity = new DashBoardClass();
            var dateString = "";
            for (DateTime? dt = FromDate; dt < ToDate; dt = dt.Value.AddDays(Perdio))
            {
                dates.Add(dt);
                DatePeriod = dt.Value.Day + "/" + dt.Value.Month + (AddYearFlag == true ? "/" + dt.Value.Year + "" : "");
                Entity = new DashBoardClass();
                Entity.total = 0;
                Entity.sumChargeableWeight = 0;
                Entity.sumGrossWeight = 0;
                Entity.totalProfitInLocalCurrency = 0;
                Entity.totalProfitInProfitCurrency = 0;
                Entity.ReceivablesInLocalCurrency = 0;
                Entity.ReceivablesInProfitCurrency = 0;
                dateString = dt.Value.Day + "/" + dt.Value.Month + (AddYearFlag == true ? "/" + dt.Value.Year + "" : ""); ;
                Listt.Add(dateString, Entity);

            }


            dates.Add(ToDate.Value);
            DatePeriod = ToDate.Value.Day + "/" + ToDate.Value.Month + (AddYearFlag == true ? "/" + ToDate.Value.Year + "" : "");
            Entity = new DashBoardClass();
            Entity.total = 0;
            Entity.sumChargeableWeight = 0;
            Entity.sumGrossWeight = 0;
            Entity.totalProfitInLocalCurrency = 0;
            Entity.totalProfitInProfitCurrency = 0;
            Entity.ReceivablesInLocalCurrency = 0;
            Entity.ReceivablesInProfitCurrency = 0;
            dateString = ToDate.Value.Day + "/" + ToDate.Value.Month + (AddYearFlag == true ? "/" + ToDate.Value.Year + "" : ""); ;
            Listt.Add(dateString, Entity);

            dates.Sort();


            days = lastDays + 1;


            if (type == "CreateDate" || type == null)
            {
                resulList = (from s in shipments
                             where (s.CreateDateTime >= QueryFromDate) && (s.CreateDateTime < QueryToDate)
                             orderby s.CreateDateTime

                             group s by new
                             {
                                 s.CreateDateTime.Day,
                                 s.CreateDateTime.Month,
                                 s.CreateDateTime.Year,
                                 s.DirectionId,
                                 s.CreateDateTime,
                                 s.TransportModeId,
                             } into m

                             orderby m.Key.Year, m.Key.Month, m.Key.Day
                             select new DashBoardClass()
                             {
                                 day = m.Key.Day,
                                 month = m.Key.Month,
                                 year = m.Key.Year,
                                 directionID = m.Key.DirectionId,
                                 transportModeID = m.Key.TransportModeId,
                                 total = m.Count(),
                                 FullDate = m.Key.CreateDateTime,
                                 sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                 sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                 totalProfitInLocalCurrency = m.Sum(s => s.ProfitInLocalCurrency),
                                 totalProfitInProfitCurrency = m.Sum(s => s.ProfitInProfitCurrency),
                                 ReceivablesInLocalCurrency = (m.Sum(s => s.OpenReceivablesInLocalCurrency) + m.Sum(s => s.AccountedReceivablesInLocalCurrency)),
                                 ReceivablesInProfitCurrency = (m.Sum(s => s.OpenReceivablesInProfitCurrency) + m.Sum(s => s.AccountedReceivablesInProfitCurrency)),
                             }).ToList();
            }
            else
            {
                resulList = (from s in shipments
                             where s.OperationalDate >= QueryFromDate && s.CreateDateTime < QueryToDate
                             orderby s.OperationalDate

                             group s by new
                             {
                                 s.OperationalDate.Value.Day,
                                 s.OperationalDate.Value.Month,
                                 s.OperationalDate.Value.Year,
                                 s.OperationalDate,
                                 s.DirectionId,
                                 s.TransportModeId,
                             } into m

                             orderby m.Key.Year, m.Key.Month, m.Key.Day
                             select new DashBoardClass()
                             {
                                 day = m.Key.Day,
                                 month = m.Key.Month,
                                 year = m.Key.Year,
                                 directionID = m.Key.DirectionId,
                                 transportModeID = m.Key.TransportModeId,
                                 total = m.Count(),
                                 FullDate = m.Key.OperationalDate,
                                 sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                 sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                 totalProfitInLocalCurrency = m.Sum(s => s.ProfitInLocalCurrency),
                                 totalProfitInProfitCurrency = m.Sum(s => s.ProfitInProfitCurrency),
                                 ReceivablesInLocalCurrency = (m.Sum(s => s.OpenReceivablesInLocalCurrency) + m.Sum(s => s.AccountedReceivablesInLocalCurrency)),
                                 ReceivablesInProfitCurrency = (m.Sum(s => s.OpenReceivablesInProfitCurrency) + m.Sum(s => s.AccountedReceivablesInProfitCurrency)),
                             }).ToList();

            }
            foreach (DashBoardClass d in resulList)
            {
                DatePeriod = "";
                var dt = d.FullDate.Value.Date;
                int count = dates.Count;

                if (count >= 1 ? dt <= dates[0] : false)
                {
                    DatePeriod = dates[0].Value.Day + "/" + dates[0].Value.Month + (AddYearFlag == true ? "/" + dates[0].Value.Year + "" : "");
                    if (!Listt.ContainsKey(DatePeriod))
                    {
                        Listt.Add(DatePeriod, d);
                    }
                    else
                    {
                        Listt[DatePeriod].total += d.total;
                        Listt[DatePeriod].sumChargeableWeight += d.sumChargeableWeight != null ? d.sumChargeableWeight : 0;
                        Listt[DatePeriod].sumGrossWeight += d.sumGrossWeight != null ? d.sumGrossWeight : 0;
                        Listt[DatePeriod].totalProfitInLocalCurrency += d.totalProfitInLocalCurrency != null ? d.totalProfitInLocalCurrency : 0;
                        Listt[DatePeriod].totalProfitInProfitCurrency += d.totalProfitInProfitCurrency != null ? d.totalProfitInProfitCurrency : 0;
                        Listt[DatePeriod].ReceivablesInLocalCurrency += d.ReceivablesInLocalCurrency != null ? d.ReceivablesInLocalCurrency : 0;
                        Listt[DatePeriod].ReceivablesInProfitCurrency += d.ReceivablesInProfitCurrency != null ? d.ReceivablesInProfitCurrency : 0;
                    }
                }

                else if (count >= 2 ? dt <= dates[1] : false)
                {
                    DatePeriod = dates[1].Value.Day + "/" + dates[1].Value.Month + (AddYearFlag == true ? "/" + dates[1].Value.Year + "" : "");
                    if (!Listt.ContainsKey(DatePeriod))
                    {
                        Listt.Add(DatePeriod, d);
                    }
                    else
                    {
                        Listt[DatePeriod].total += d.total;
                        Listt[DatePeriod].sumChargeableWeight += d.sumChargeableWeight != null ? d.sumChargeableWeight : 0;
                        Listt[DatePeriod].sumGrossWeight += d.sumGrossWeight != null ? d.sumGrossWeight : 0;
                        Listt[DatePeriod].totalProfitInLocalCurrency += d.totalProfitInLocalCurrency != null ? d.totalProfitInLocalCurrency : 0;
                        Listt[DatePeriod].totalProfitInProfitCurrency += d.totalProfitInProfitCurrency != null ? d.totalProfitInProfitCurrency : 0;
                        Listt[DatePeriod].ReceivablesInLocalCurrency += d.ReceivablesInLocalCurrency != null ? d.ReceivablesInLocalCurrency : 0;
                        Listt[DatePeriod].ReceivablesInProfitCurrency += d.ReceivablesInProfitCurrency != null ? d.ReceivablesInProfitCurrency : 0;
                    }
                }


                else if (count >= 3 ? dt <= dates[2] : false)
                {
                    DatePeriod = dates[2].Value.Day + "/" + dates[2].Value.Month + (AddYearFlag == true ? "/" + dates[2].Value.Year + "" : "");
                    if (!Listt.ContainsKey(DatePeriod))
                    {
                        Listt.Add(DatePeriod, d);
                    }
                    else
                    {
                        Listt[DatePeriod].total += d.total;
                        Listt[DatePeriod].sumChargeableWeight += d.sumChargeableWeight != null ? d.sumChargeableWeight : 0;
                        Listt[DatePeriod].sumGrossWeight += d.sumGrossWeight != null ? d.sumGrossWeight : 0;
                        Listt[DatePeriod].totalProfitInLocalCurrency += d.totalProfitInLocalCurrency != null ? d.totalProfitInLocalCurrency : 0;
                        Listt[DatePeriod].totalProfitInProfitCurrency += d.totalProfitInProfitCurrency != null ? d.totalProfitInProfitCurrency : 0;
                        Listt[DatePeriod].ReceivablesInLocalCurrency += d.ReceivablesInLocalCurrency != null ? d.ReceivablesInLocalCurrency : 0;
                        Listt[DatePeriod].ReceivablesInProfitCurrency += d.ReceivablesInProfitCurrency != null ? d.ReceivablesInProfitCurrency : 0;
                    }
                }


                else if (count >= 4 ? dt <= dates[3] : false)
                {
                    DatePeriod = dates[3].Value.Day + "/" + dates[3].Value.Month + (AddYearFlag == true ? "/" + dates[3].Value.Year + "" : "");
                    if (!Listt.ContainsKey(DatePeriod))
                    {
                        Listt.Add(DatePeriod, d);
                    }
                    else
                    {
                        Listt[DatePeriod].total += d.total;
                        Listt[DatePeriod].sumChargeableWeight += d.sumChargeableWeight != null ? d.sumChargeableWeight : 0;
                        Listt[DatePeriod].sumGrossWeight += d.sumGrossWeight != null ? d.sumGrossWeight : 0;
                        Listt[DatePeriod].totalProfitInLocalCurrency += d.totalProfitInLocalCurrency != null ? d.totalProfitInLocalCurrency : 0;
                        Listt[DatePeriod].totalProfitInProfitCurrency += d.totalProfitInProfitCurrency != null ? d.totalProfitInProfitCurrency : 0;
                        Listt[DatePeriod].ReceivablesInLocalCurrency += d.ReceivablesInLocalCurrency != null ? d.ReceivablesInLocalCurrency : 0;
                        Listt[DatePeriod].ReceivablesInProfitCurrency += d.ReceivablesInProfitCurrency != null ? d.ReceivablesInProfitCurrency : 0;
                    }
                }


                else if (count >= 5 ? dt <= dates[4] : false)
                {
                    DatePeriod = dates[4].Value.Day + "/" + dates[4].Value.Month + (AddYearFlag == true ? "/" + dates[4].Value.Year + "" : "");
                    if (!Listt.ContainsKey(DatePeriod))
                    {
                        Listt.Add(DatePeriod, d);
                    }
                    else
                    {
                        Listt[DatePeriod].total += d.total;
                        Listt[DatePeriod].sumChargeableWeight += d.sumChargeableWeight != null ? d.sumChargeableWeight : 0;
                        Listt[DatePeriod].sumGrossWeight += d.sumGrossWeight != null ? d.sumGrossWeight : 0;
                        Listt[DatePeriod].totalProfitInLocalCurrency += d.totalProfitInLocalCurrency != null ? d.totalProfitInLocalCurrency : 0;
                        Listt[DatePeriod].totalProfitInProfitCurrency += d.totalProfitInProfitCurrency != null ? d.totalProfitInProfitCurrency : 0;
                        Listt[DatePeriod].ReceivablesInLocalCurrency += d.ReceivablesInLocalCurrency != null ? d.ReceivablesInLocalCurrency : 0;
                        Listt[DatePeriod].ReceivablesInProfitCurrency += d.ReceivablesInProfitCurrency != null ? d.ReceivablesInProfitCurrency : 0;
                    }
                }


                else if (count >= 6 ? dt <= dates[5] : false)
                {
                    DatePeriod = dates[5].Value.Day + "/" + dates[5].Value.Month + (AddYearFlag == true ? "/" + dates[5].Value.Year + "" : "");
                    if (!Listt.ContainsKey(DatePeriod))
                    {
                        Listt.Add(DatePeriod, d);
                    }
                    else
                    {
                        Listt[DatePeriod].total += d.total;
                        Listt[DatePeriod].sumChargeableWeight += d.sumChargeableWeight != null ? d.sumChargeableWeight : 0;
                        Listt[DatePeriod].sumGrossWeight += d.sumGrossWeight != null ? d.sumGrossWeight : 0;
                        Listt[DatePeriod].totalProfitInLocalCurrency += d.totalProfitInLocalCurrency != null ? d.totalProfitInLocalCurrency : 0;
                        Listt[DatePeriod].totalProfitInProfitCurrency += d.totalProfitInProfitCurrency != null ? d.totalProfitInProfitCurrency : 0;
                        Listt[DatePeriod].ReceivablesInLocalCurrency += d.ReceivablesInLocalCurrency != null ? d.ReceivablesInLocalCurrency : 0;
                        Listt[DatePeriod].ReceivablesInProfitCurrency += d.ReceivablesInProfitCurrency != null ? d.ReceivablesInProfitCurrency : 0;
                    }
                }


                else if (count >= 7 ? dt <= dates[6] : false)
                {
                    DatePeriod = dates[6].Value.Day + "/" + dates[6].Value.Month + (AddYearFlag == true ? "/" + dates[6].Value.Year + "" : "");
                    if (!Listt.ContainsKey(DatePeriod))
                    {
                        Listt.Add(DatePeriod, d);
                    }
                    else
                    {
                        Listt[DatePeriod].total += d.total;
                        Listt[DatePeriod].sumChargeableWeight += d.sumChargeableWeight != null ? d.sumChargeableWeight : 0;
                        Listt[DatePeriod].sumGrossWeight += d.sumGrossWeight != null ? d.sumGrossWeight : 0;
                        Listt[DatePeriod].totalProfitInLocalCurrency += d.totalProfitInLocalCurrency != null ? d.totalProfitInLocalCurrency : 0;
                        Listt[DatePeriod].totalProfitInProfitCurrency += d.totalProfitInProfitCurrency != null ? d.totalProfitInProfitCurrency : 0;
                        Listt[DatePeriod].ReceivablesInLocalCurrency += d.ReceivablesInLocalCurrency != null ? d.ReceivablesInLocalCurrency : 0;
                        Listt[DatePeriod].ReceivablesInProfitCurrency += d.ReceivablesInProfitCurrency != null ? d.ReceivablesInProfitCurrency : 0;
                    }
                }

                //else if (d.FullDate >= dates[6] && d.FullDate <= dates[7])
                //{
                //    DatePeriod = dates[6].Value.Day + "/" + dates[6].Value.Month + (AddYearFlag == true ? "/" + dates[6].Value.Year + "" : "") + "-" + dates[7].Value.Day + "/" + dates[7].Value.Month+ (AddYearFlag == true ? "/" + dates[7].Value.Year + "" : "") ;
                //    if (!Listt.ContainsKey(DatePeriod))
                //    {
                //        Listt.Add(DatePeriod, d);
                //    }
                //    else
                //    {
                //        Listt[DatePeriod].total += d.total;
                //        Listt[DatePeriod].sumChargeableWeight += d.sumChargeableWeight != null ? d.sumChargeableWeight : 0;
                //        Listt[DatePeriod].sumGrossWeight += d.sumGrossWeight != null ? d.sumGrossWeight : 0;
                //        Listt[DatePeriod].totalProfitInLocalCurrency += d.totalProfitInLocalCurrency != null ? d.totalProfitInLocalCurrency : 0;
                //        Listt[DatePeriod].totalProfitInProfitCurrency += d.totalProfitInProfitCurrency != null ? d.totalProfitInProfitCurrency : 0;
                //        Listt[DatePeriod].ReceivablesInLocalCurrency += d.ReceivablesInLocalCurrency != null ? d.ReceivablesInLocalCurrency : 0;
                //        Listt[DatePeriod].ReceivablesInProfitCurrency += d.ReceivablesInProfitCurrency != null ? d.ReceivablesInProfitCurrency : 0;
                //    }
                //}           
            }
            resulList = new List<DashBoardClass>();

            for (int i = 0; i < Listt.Count; i++)
            {
                Listt.Values.ElementAt(i).DateRange = Listt.Keys.ElementAt(i);
                resulList.Add(Listt.Values.ElementAt(i));
            }
            return resulList;
        }



        /* DashBoard By Activites */
        public IQueryable<DashBoardClass> GetShipmentsByActivitesDashBoard(int last, int currentTenant)
        {
            IQueryable<DashBoardClass> resulList = null;
            DateTime lastDate = DateTime.Today.Date.AddMonths(last + 1);
            IQueryable<Shipment> shipments = from s in repository.context.Shipments.Include("Direction").Include("TransportMode")
                                             where s.Tenant == currentTenant && s.ShipmentLevelCode != "C"
                                             && s.CreateDateTime >= lastDate
                                             select s;
            shipments = BranchPermitionsFilter.AddUserBranchRestrictionFilters<Shipment>(new QueryOperations(), shipments, currentTenant);
            shipments = ProductPermitionsFilter.AddUserProductRestrictionFilters<Shipment>(new QueryOperations(), shipments, currentTenant);

            resulList = from s in shipments

                        group s by new
                        {
                            direction = s.Direction.Name,
                            transport = s.TransportMode.Name,
                        } into m

                        select new DashBoardClass()
                        {
                            directionID = m.Key.direction,
                            transportModeID = m.Key.transport,
                            total = m.Count(),
                            sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                            sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                        };

            return resulList;
        }

        /* DashBoard By Top 10 Countries */
        public List<DashBoardClass> GetShipmentsByTop10CountriesDashBoard(string type, int lastMonths, int lastDays, int measurment, int currentTenant, int top, bool includeOthers, string customerid, string directionId, string transportmodeId)
        {
            if (top < 0)
            {
                top = 0;
            }

            int days = 0;
            int months = 0;
            days = lastDays;
            DateTime lastDate = DateTime.Today.Date.AddDays(days);

            if (lastMonths != 0)
            {
                months = lastMonths + 1;
                lastDate = DateTime.Today.Date.AddMonths(months);
            }

            List<DashBoardClass> unionList = null;
            IQueryable<ShipmentCountryDashboardView> shipments = repository.GetShipmentDataViewsForCountriesDashboard(currentTenant, customerid, directionId, transportmodeId);
            DateTime FromDateQuery = TenantServerConfigration.GetCurrentDateTime(currentTenant);
            List<DashBoardClass> resultList = null;
            List<DashBoardClass> othersResultList = null;

            switch (measurment)
            {
                #region if the measurement is shipment count
                case 0:
                    {

                        if (type == "CreateDate" || type == null)
                        {
                            resultList = (from s in shipments
                                          where s.CreateDateTime > lastDate
                                          group s by new
                                          {
                                              s.CountryForStatisticsCode,
                                              s.CountryForStatisticsName

                                          } into m
                                          select new DashBoardClass()
                                          {
                                              countryCode = m.Key.CountryForStatisticsCode,
                                              countryName = m.Key.CountryForStatisticsName,
                                              country = m.Key.CountryForStatisticsCode,
                                              total = m.Count(),
                                              sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                              sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                              totalLastMonth = 0,
                                              sumChargeableWeightLastMonth = 0,
                                              sumGrossWeightLastMonth = 0,
                                          }).OrderByDescending(d => d.total).Take(top).ToList();


                            if (includeOthers)
                            {

                                List<DashBoardClass> allCountriesResult = (from s in shipments
                                                                           where s.CreateDateTime > lastDate
                                                                           group s by new
                                                                           {
                                                                               s.CountryForStatisticsCode,
                                                                               s.CountryForStatisticsName
                                                                           } into m
                                                                           select new DashBoardClass()
                                                                           {
                                                                               countryCode = m.Key.CountryForStatisticsCode,
                                                                               countryName = m.Key.CountryForStatisticsName,
                                                                               country = m.Key.CountryForStatisticsCode,
                                                                               total = m.Count(),
                                                                               sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                                                               sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                                                               totalLastMonth = 0,
                                                                               sumChargeableWeightLastMonth = 0,
                                                                               sumGrossWeightLastMonth = 0,
                                                                           }).ToList();

                                othersResultList = (from a in allCountriesResult
                                                    where !(from r in resultList where r.countryCode == a.countryCode select r).Any()
                                                    select a).ToList();
                                foreach (DashBoardClass d in othersResultList)
                                {
                                    d.countryCode = "Others";
                                    d.countryName = "Others";
                                    d.country = "Others";
                                }

                                unionList = resultList.Union(othersResultList).OrderByDescending(d => d.total).ToList();
                            }

                            else
                            {
                                unionList = resultList.ToList();
                            }
                        }
                        else
                        {
                            resultList = (from s in shipments
                                          where s.OperationalDate > lastDate && s.OperationalDate <= FromDateQuery
                                          group s by new
                                          {
                                              s.CountryForStatisticsCode,
                                              s.CountryForStatisticsName

                                          } into m
                                          select new DashBoardClass()
                                          {
                                              countryCode = m.Key.CountryForStatisticsCode,
                                              countryName = m.Key.CountryForStatisticsName,
                                              country = m.Key.CountryForStatisticsCode,
                                              total = m.Count(),
                                              sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                              sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                              totalLastMonth = 0,
                                              sumChargeableWeightLastMonth = 0,
                                              sumGrossWeightLastMonth = 0,
                                          }).OrderByDescending(d => d.total).Take(top).ToList();


                            if (includeOthers)
                            {
                                List<DashBoardClass> allCountriesResult = (from s in shipments
                                                                           where s.OperationalDate > lastDate && s.OperationalDate <= FromDateQuery
                                                                           group s by new
                                                                           {
                                                                               s.CountryForStatisticsCode,
                                                                               s.CountryForStatisticsName

                                                                           } into m
                                                                           select new DashBoardClass()
                                                                           {
                                                                               countryCode = m.Key.CountryForStatisticsCode,
                                                                               countryName = m.Key.CountryForStatisticsName,
                                                                               country = m.Key.CountryForStatisticsCode,
                                                                               total = m.Count(),
                                                                               sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                                                               sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                                                               totalLastMonth = 0,
                                                                               sumChargeableWeightLastMonth = 0,
                                                                               sumGrossWeightLastMonth = 0,
                                                                           }).ToList();

                                othersResultList = (from a in allCountriesResult
                                                    where !(from r in resultList where r.countryCode == a.countryCode select r).Any()
                                                    select a).ToList();
                                foreach (DashBoardClass d in othersResultList)
                                {
                                    d.countryCode = "Others";
                                    d.countryName = "Others";
                                    d.country = "Others";
                                }

                                unionList = resultList.Union(othersResultList).OrderByDescending(d => d.total).ToList();
                            }


                            else
                            {
                                unionList = resultList.ToList();
                            }
                        }


                        break;
                    }
                #endregion

                #region if the measurement is chargeable weight
                case 1:
                    {
                        if (type == "CreateDate" || type == null)
                        {

                            resultList = (from s in shipments
                                          where s.CreateDateTime > lastDate
                                          group s by new
                                          {
                                              s.CountryForStatisticsCode,
                                              s.CountryForStatisticsName

                                          } into m
                                          select new DashBoardClass()
                                          {
                                              countryCode = m.Key.CountryForStatisticsCode,
                                              countryName = m.Key.CountryForStatisticsName,
                                              country = m.Key.CountryForStatisticsCode,
                                              total = m.Count(),
                                              sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                              sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                              totalLastMonth = 0,
                                              sumChargeableWeightLastMonth = 0,
                                              sumGrossWeightLastMonth = 0,
                                          }).OrderByDescending(d => d.sumChargeableWeight).Take(top).ToList();


                            if (includeOthers)
                            {
                                List<DashBoardClass> allCountriesResult = (from s in shipments
                                                                           where s.CreateDateTime > lastDate
                                                                           group s by new
                                                                           {
                                                                               s.CountryForStatisticsCode,
                                                                               s.CountryForStatisticsName

                                                                           } into m
                                                                           select new DashBoardClass()
                                                                           {
                                                                               countryCode = m.Key.CountryForStatisticsCode,
                                                                               countryName = m.Key.CountryForStatisticsName,
                                                                               country = m.Key.CountryForStatisticsCode,
                                                                               total = m.Count(),
                                                                               sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                                                               sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                                                               totalLastMonth = 0,
                                                                               sumChargeableWeightLastMonth = 0,
                                                                               sumGrossWeightLastMonth = 0,
                                                                           }).ToList();

                                othersResultList = (from a in allCountriesResult
                                                    where !(from r in resultList where r.countryCode == a.countryCode select r).Any()
                                                    select a).ToList();
                                foreach (DashBoardClass d in othersResultList)
                                {
                                    d.countryCode = "Others";
                                    d.countryName = "Others";
                                    d.country = "Others";
                                }

                                unionList = resultList.Union(othersResultList).OrderByDescending(d => d.sumChargeableWeight).ToList();

                            }
                            else
                            {
                                unionList = resultList.OrderByDescending(d => d.sumChargeableWeight).ToList();
                            }
                            break;
                        }
                        else
                        {
                            resultList = (from s in shipments
                                          where s.CreateDateTime > lastDate
                                          group s by new
                                          {
                                              s.CountryForStatisticsCode,
                                              s.CountryForStatisticsName

                                          } into m
                                          select new DashBoardClass()
                                          {
                                              countryCode = m.Key.CountryForStatisticsCode,
                                              countryName = m.Key.CountryForStatisticsName,
                                              country = m.Key.CountryForStatisticsCode,
                                              total = m.Count(),
                                              sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                              sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                              totalLastMonth = 0,
                                              sumChargeableWeightLastMonth = 0,
                                              sumGrossWeightLastMonth = 0,
                                          }).OrderByDescending(d => d.sumChargeableWeight).Take(top).ToList();


                            if (includeOthers)
                            {
                                List<DashBoardClass> allCountriesResult = (from s in shipments
                                                                           where s.CreateDateTime > lastDate
                                                                           group s by new
                                                                           {
                                                                               s.CountryForStatisticsCode,
                                                                               s.CountryForStatisticsName

                                                                           } into m
                                                                           select new DashBoardClass()
                                                                           {
                                                                               countryCode = m.Key.CountryForStatisticsCode,
                                                                               countryName = m.Key.CountryForStatisticsName,
                                                                               country = m.Key.CountryForStatisticsCode,
                                                                               total = m.Count(),
                                                                               sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                                                               sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                                                               totalLastMonth = 0,
                                                                               sumChargeableWeightLastMonth = 0,
                                                                               sumGrossWeightLastMonth = 0,
                                                                           }).ToList();

                                othersResultList = (from a in allCountriesResult
                                                    where !(from r in resultList where r.countryCode == a.countryCode select r).Any()
                                                    select a).ToList();
                                foreach (DashBoardClass d in othersResultList)
                                {
                                    d.countryCode = "Others";
                                    d.countryName = "Others";
                                    d.country = "Others";
                                }

                                unionList = resultList.Union(othersResultList).OrderByDescending(d => d.sumChargeableWeight).ToList();

                            }
                            else
                            {
                                unionList = resultList.OrderByDescending(d => d.sumChargeableWeight).ToList();
                            }
                            break;
                        }

                    }
                #endregion

                #region if the measurement is gross weight
                case 2:
                    {
                        if (type == "CreateDate" || type == null)
                        {
                            resultList = (from s in shipments
                                          where s.CreateDateTime > lastDate
                                          group s by new
                                          {
                                              s.CountryForStatisticsCode,
                                              s.CountryForStatisticsName

                                          } into m
                                          select new DashBoardClass()
                                          {
                                              countryCode = m.Key.CountryForStatisticsCode,
                                              countryName = m.Key.CountryForStatisticsName,
                                              country = m.Key.CountryForStatisticsCode,
                                              total = m.Count(),
                                              sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                              sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                              totalLastMonth = 0,
                                              sumChargeableWeightLastMonth = 0,
                                              sumGrossWeightLastMonth = 0,
                                          }).OrderByDescending(d => d.sumGrossWeight).Take(top).ToList();


                            if (includeOthers)
                            {
                                List<DashBoardClass> allCountriesResult = (from s in shipments
                                                                           where s.CreateDateTime > lastDate
                                                                           group s by new
                                                                           {
                                                                               s.CountryForStatisticsCode,
                                                                               s.CountryForStatisticsName

                                                                           } into m
                                                                           select new DashBoardClass()
                                                                           {
                                                                               countryCode = m.Key.CountryForStatisticsCode,
                                                                               countryName = m.Key.CountryForStatisticsName,
                                                                               country = m.Key.CountryForStatisticsCode,
                                                                               total = m.Count(),
                                                                               sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                                                               sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                                                               totalLastMonth = 0,
                                                                               sumChargeableWeightLastMonth = 0,
                                                                               sumGrossWeightLastMonth = 0,
                                                                           }).ToList();

                                othersResultList = (from a in allCountriesResult
                                                    where !(from r in resultList where r.countryCode == a.countryCode select r).Any()
                                                    select a).ToList();
                                foreach (DashBoardClass d in othersResultList)
                                {
                                    d.countryCode = "Others";
                                    d.countryName = "Others";
                                    d.country = "Others";
                                }

                                unionList = resultList.Union(othersResultList).OrderByDescending(d => d.sumGrossWeight).ToList();

                            }
                            else
                            {
                                unionList = resultList.OrderByDescending(d => d.sumGrossWeight).ToList();//ResulList;//ResulListMonth.Union(ResulList);
                            }
                        }
                        else
                        {
                            resultList = (from s in shipments
                                          where s.OperationalDate > lastDate && s.OperationalDate <= FromDateQuery
                                          group s by new
                                          {
                                              s.CountryForStatisticsCode,
                                              s.CountryForStatisticsName

                                          } into m
                                          select new DashBoardClass()
                                          {
                                              countryCode = m.Key.CountryForStatisticsCode,
                                              countryName = m.Key.CountryForStatisticsName,
                                              country = m.Key.CountryForStatisticsCode,
                                              total = m.Count(),
                                              sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                              sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                              totalLastMonth = 0,
                                              sumChargeableWeightLastMonth = 0,
                                              sumGrossWeightLastMonth = 0,
                                          }).OrderByDescending(d => d.sumGrossWeight).Take(top).ToList();


                            if (includeOthers)
                            {
                                List<DashBoardClass> allCountriesResult = (from s in shipments
                                                                           where s.OperationalDate > lastDate && s.OperationalDate <= FromDateQuery
                                                                           group s by new
                                                                           {
                                                                               s.CountryForStatisticsCode,
                                                                               s.CountryForStatisticsName

                                                                           } into m
                                                                           select new DashBoardClass()
                                                                           {
                                                                               countryCode = m.Key.CountryForStatisticsCode,
                                                                               countryName = m.Key.CountryForStatisticsName,
                                                                               country = m.Key.CountryForStatisticsCode,
                                                                               total = m.Count(),
                                                                               sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                                                               sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                                                               totalLastMonth = 0,
                                                                               sumChargeableWeightLastMonth = 0,
                                                                               sumGrossWeightLastMonth = 0,
                                                                           }).ToList();

                                othersResultList = (from a in allCountriesResult
                                                    where !(from r in resultList where r.countryCode == a.countryCode select r).Any()
                                                    select a).ToList();
                                foreach (DashBoardClass d in othersResultList)
                                {
                                    d.countryCode = "Others";
                                    d.countryName = "Others";
                                    d.country = "Others";
                                }

                                unionList = resultList.Union(othersResultList).OrderByDescending(d => d.sumGrossWeight).ToList();

                            }
                            else
                            {
                                unionList = resultList.OrderByDescending(d => d.sumGrossWeight).ToList();//ResulList;//ResulListMonth.Union(ResulList);
                            }
                        }

                        break;
                    }
                #endregion

                #region if the measurement is profit in local
                case 3:
                    {
                        if (type == "CreateDate" || type == null)
                        {
                            resultList = (from s in shipments
                                          where s.CreateDateTime > lastDate
                                          group s by new
                                          {
                                              s.CountryForStatisticsCode,
                                              s.CountryForStatisticsName

                                          } into m
                                          select new DashBoardClass()
                                          {
                                              countryCode = m.Key.CountryForStatisticsCode,
                                              countryName = m.Key.CountryForStatisticsName,
                                              country = m.Key.CountryForStatisticsCode,
                                              total = m.Count(),
                                              sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                              sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                              totalLastMonth = 0,
                                              sumChargeableWeightLastMonth = 0,
                                              sumGrossWeightLastMonth = 0,
                                              ReceivablesInProfitCurrency = m.Sum(d => d.OpenReceivablesInLocalCurrency),
                                              totalProfitInLocalCurrency = m.Sum(d => d.ProfitInLocalCurrency),
                                              totalProfitInProfitCurrency = m.Sum(d => d.ProfitInProfitCurrency),
                                          }).OrderByDescending(d => d.totalProfitInLocalCurrency).Take(top).ToList();


                            if (includeOthers)
                            {
                                List<DashBoardClass> allCountriesResult = (from s in shipments
                                                                           where s.CreateDateTime > lastDate
                                                                           group s by new
                                                                           {
                                                                               s.CountryForStatisticsCode,
                                                                               s.CountryForStatisticsName

                                                                           } into m
                                                                           select new DashBoardClass()
                                                                           {
                                                                               countryCode = m.Key.CountryForStatisticsCode,
                                                                               countryName = m.Key.CountryForStatisticsName,
                                                                               country = m.Key.CountryForStatisticsCode,
                                                                               total = m.Count(),
                                                                               sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                                                               sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                                                               totalLastMonth = 0,
                                                                               sumChargeableWeightLastMonth = 0,
                                                                               sumGrossWeightLastMonth = 0,
                                                                               ReceivablesInProfitCurrency = m.Sum(d => d.OpenReceivablesInLocalCurrency),
                                                                               totalProfitInLocalCurrency = m.Sum(d => d.ProfitInLocalCurrency),
                                                                               totalProfitInProfitCurrency = m.Sum(d => d.ProfitInProfitCurrency),
                                                                           }).ToList();



                                othersResultList = (from a in allCountriesResult
                                                    where !(from r in resultList where r.countryCode == a.countryCode select r).Any()
                                                    select a).ToList();
                                foreach (DashBoardClass d in othersResultList)
                                {
                                    d.countryCode = "Others";
                                    d.countryName = "Others";
                                    d.country = "Others";
                                }
                                unionList = resultList.Union(othersResultList).OrderByDescending(d => d.totalProfitInLocalCurrency).ToList();

                            }
                            else
                            {
                                unionList = resultList.OrderByDescending(d => d.totalProfitInLocalCurrency).ToList();//ResulList;//ResulListMonth.Union(ResulList);
                            }

                        }
                        else
                        {
                            resultList = (from s in shipments
                                          where s.OperationalDate > lastDate && s.OperationalDate <= FromDateQuery
                                          group s by new
                                          {
                                              s.CountryForStatisticsCode,
                                              s.CountryForStatisticsName

                                          } into m
                                          select new DashBoardClass()
                                          {
                                              countryCode = m.Key.CountryForStatisticsCode,
                                              countryName = m.Key.CountryForStatisticsName,
                                              country = m.Key.CountryForStatisticsCode,
                                              total = m.Count(),
                                              sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                              sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                              totalLastMonth = 0,
                                              sumChargeableWeightLastMonth = 0,
                                              sumGrossWeightLastMonth = 0,
                                              ReceivablesInProfitCurrency = m.Sum(d => d.OpenReceivablesInLocalCurrency),
                                              totalProfitInLocalCurrency = m.Sum(d => d.ProfitInLocalCurrency),
                                              totalProfitInProfitCurrency = m.Sum(d => d.ProfitInProfitCurrency),
                                          }).OrderByDescending(d => d.totalProfitInLocalCurrency).Take(top).ToList();


                            if (includeOthers)
                            {
                                List<DashBoardClass> allCountriesResult = (from s in shipments
                                                                           where s.OperationalDate > lastDate && s.OperationalDate <= FromDateQuery
                                                                           group s by new
                                                                           {
                                                                               s.CountryForStatisticsCode,
                                                                               s.CountryForStatisticsName

                                                                           } into m
                                                                           select new DashBoardClass()
                                                                           {
                                                                               countryCode = m.Key.CountryForStatisticsCode,
                                                                               countryName = m.Key.CountryForStatisticsName,
                                                                               country = m.Key.CountryForStatisticsCode,
                                                                               total = m.Count(),
                                                                               sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                                                               sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                                                               totalLastMonth = 0,
                                                                               sumChargeableWeightLastMonth = 0,
                                                                               sumGrossWeightLastMonth = 0,
                                                                               ReceivablesInProfitCurrency = m.Sum(d => d.OpenReceivablesInLocalCurrency),
                                                                               totalProfitInLocalCurrency = m.Sum(d => d.ProfitInLocalCurrency),
                                                                               totalProfitInProfitCurrency = m.Sum(d => d.ProfitInProfitCurrency),
                                                                           }).ToList();



                                othersResultList = (from a in allCountriesResult
                                                    where !(from r in resultList where r.countryCode == a.countryCode select r).Any()
                                                    select a).ToList();
                                foreach (DashBoardClass d in othersResultList)
                                {
                                    d.countryCode = "Others";
                                    d.countryName = "Others";
                                    d.country = "Others";
                                }
                                unionList = resultList.Union(othersResultList).OrderByDescending(d => d.totalProfitInLocalCurrency).ToList();

                            }
                            else
                            {
                                unionList = resultList.OrderByDescending(d => d.totalProfitInLocalCurrency).ToList();//ResulList;//ResulListMonth.Union(ResulList);
                            }
                        }

                        break;
                    }
                #endregion

                #region if the measurement is profit in profit
                case 4:
                    {

                        if (type == "CreateDate" || type == null)
                        {
                            resultList = (from s in shipments
                                          where s.CreateDateTime > lastDate
                                          group s by new
                                          {
                                              s.CountryForStatisticsCode,
                                              s.CountryForStatisticsName

                                          } into m
                                          select new DashBoardClass()
                                          {
                                              countryCode = m.Key.CountryForStatisticsCode,
                                              countryName = m.Key.CountryForStatisticsName,
                                              country = m.Key.CountryForStatisticsCode,
                                              total = m.Count(),
                                              sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                              sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                              totalLastMonth = 0,
                                              sumChargeableWeightLastMonth = 0,
                                              sumGrossWeightLastMonth = 0,
                                              ReceivablesInProfitCurrency = m.Sum(d => d.OpenReceivablesInLocalCurrency),
                                              totalProfitInLocalCurrency = m.Sum(d => d.ProfitInLocalCurrency),
                                              totalProfitInProfitCurrency = m.Sum(d => d.ProfitInProfitCurrency),
                                          }).OrderByDescending(d => d.totalProfitInProfitCurrency).Take(top).ToList();


                            if (includeOthers)
                            {
                                List<DashBoardClass> allCountriesResult = (from s in shipments
                                                                           where s.CreateDateTime > lastDate
                                                                           group s by new
                                                                           {
                                                                               s.CountryForStatisticsCode,
                                                                               s.CountryForStatisticsName

                                                                           } into m
                                                                           select new DashBoardClass()
                                                                           {
                                                                               countryCode = m.Key.CountryForStatisticsCode,
                                                                               countryName = m.Key.CountryForStatisticsName,
                                                                               country = m.Key.CountryForStatisticsCode,
                                                                               total = m.Count(),
                                                                               sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                                                               sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                                                               totalLastMonth = 0,
                                                                               sumChargeableWeightLastMonth = 0,
                                                                               sumGrossWeightLastMonth = 0,
                                                                               ReceivablesInProfitCurrency = m.Sum(d => d.OpenReceivablesInLocalCurrency),
                                                                               totalProfitInLocalCurrency = m.Sum(d => d.ProfitInLocalCurrency),
                                                                               totalProfitInProfitCurrency = m.Sum(d => d.ProfitInProfitCurrency),
                                                                           }).ToList();


                                othersResultList = (from a in allCountriesResult
                                                    where !(from r in resultList where r.countryCode == a.countryCode select r).Any()
                                                    select a).ToList();
                                foreach (DashBoardClass d in othersResultList)
                                {
                                    d.countryCode = "Others";
                                    d.countryName = "Others";
                                    d.country = "Others";
                                }
                                unionList = resultList.Union(othersResultList).OrderByDescending(d => d.totalProfitInProfitCurrency).ToList();

                            }
                            else
                            {
                                unionList = resultList.OrderByDescending(d => d.totalProfitInProfitCurrency).ToList();
                            }
                        }

                        else
                        {
                            resultList = (from s in shipments
                                          where s.OperationalDate > lastDate && s.OperationalDate <= FromDateQuery
                                          group s by new
                                          {
                                              s.CountryForStatisticsCode,
                                              s.CountryForStatisticsName

                                          } into m
                                          select new DashBoardClass()
                                          {
                                              countryCode = m.Key.CountryForStatisticsCode,
                                              countryName = m.Key.CountryForStatisticsName,
                                              country = m.Key.CountryForStatisticsCode,
                                              total = m.Count(),
                                              sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                              sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                              totalLastMonth = 0,
                                              sumChargeableWeightLastMonth = 0,
                                              sumGrossWeightLastMonth = 0,
                                              ReceivablesInProfitCurrency = m.Sum(d => d.OpenReceivablesInLocalCurrency),
                                              totalProfitInLocalCurrency = m.Sum(d => d.ProfitInLocalCurrency),
                                              totalProfitInProfitCurrency = m.Sum(d => d.ProfitInProfitCurrency),
                                          }).OrderByDescending(d => d.totalProfitInProfitCurrency).Take(top).ToList();


                            if (includeOthers)
                            {
                                List<DashBoardClass> allCountriesResult = (from s in shipments
                                                                           where s.OperationalDate > lastDate && s.OperationalDate <= FromDateQuery
                                                                           group s by new
                                                                           {
                                                                               s.CountryForStatisticsCode,
                                                                               s.CountryForStatisticsName

                                                                           } into m
                                                                           select new DashBoardClass()
                                                                           {
                                                                               countryCode = m.Key.CountryForStatisticsCode,
                                                                               countryName = m.Key.CountryForStatisticsName,
                                                                               country = m.Key.CountryForStatisticsCode,
                                                                               total = m.Count(),
                                                                               sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                                                               sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                                                               totalLastMonth = 0,
                                                                               sumChargeableWeightLastMonth = 0,
                                                                               sumGrossWeightLastMonth = 0,
                                                                               ReceivablesInProfitCurrency = m.Sum(d => d.OpenReceivablesInLocalCurrency),
                                                                               totalProfitInLocalCurrency = m.Sum(d => d.ProfitInLocalCurrency),
                                                                               totalProfitInProfitCurrency = m.Sum(d => d.ProfitInProfitCurrency),
                                                                           }).ToList();


                                othersResultList = (from a in allCountriesResult
                                                    where !(from r in resultList where r.countryCode == a.countryCode select r).Any()
                                                    select a).ToList();
                                foreach (DashBoardClass d in othersResultList)
                                {
                                    d.countryCode = "Others";
                                    d.countryName = "Others";
                                    d.country = "Others";
                                }
                                unionList = resultList.Union(othersResultList).OrderByDescending(d => d.totalProfitInProfitCurrency).ToList();

                            }
                            else
                            {
                                unionList = resultList.OrderByDescending(d => d.totalProfitInProfitCurrency).ToList();
                            }
                        }

                        break;
                    }
                #endregion

                #region if the measurement is receivables in local
                case 5:
                    {
                        if (type == "CreateDate" || type == null)
                        {
                            resultList = (from s in shipments
                                          where s.CreateDateTime > lastDate
                                          group s by new
                                          {
                                              s.CountryForStatisticsCode,
                                              s.CountryForStatisticsName

                                          } into m
                                          select new DashBoardClass()
                                          {
                                              countryCode = m.Key.CountryForStatisticsCode,
                                              countryName = m.Key.CountryForStatisticsName,
                                              country = m.Key.CountryForStatisticsCode,
                                              total = m.Count(),
                                              sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                              sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                              totalLastMonth = 0,
                                              sumChargeableWeightLastMonth = 0,
                                              sumGrossWeightLastMonth = 0,
                                              ReceivablesInProfitCurrency = m.Sum(d => d.OpenReceivablesInProfitCurrency),
                                              ReceivablesInLocalCurrency = m.Sum(d => d.OpenReceivablesInLocalCurrency),
                                              totalProfitInLocalCurrency = m.Sum(d => d.ProfitInLocalCurrency),
                                              totalProfitInProfitCurrency = m.Sum(d => d.ProfitInProfitCurrency),
                                          }).OrderByDescending(d => d.ReceivablesInLocalCurrency).Take(top).ToList();


                            if (includeOthers)
                            {
                                List<DashBoardClass> allCountriesResult = (from s in shipments
                                                                           where s.CreateDateTime > lastDate
                                                                           group s by new
                                                                           {
                                                                               s.CountryForStatisticsCode,
                                                                               s.CountryForStatisticsName

                                                                           } into m
                                                                           select new DashBoardClass()
                                                                           {
                                                                               countryCode = m.Key.CountryForStatisticsCode,
                                                                               countryName = m.Key.CountryForStatisticsName,
                                                                               country = m.Key.CountryForStatisticsCode,
                                                                               total = m.Count(),
                                                                               sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                                                               sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                                                               totalLastMonth = 0,
                                                                               sumChargeableWeightLastMonth = 0,
                                                                               sumGrossWeightLastMonth = 0,
                                                                               ReceivablesInProfitCurrency = m.Sum(d => d.OpenReceivablesInProfitCurrency),
                                                                               ReceivablesInLocalCurrency = m.Sum(d => d.OpenReceivablesInLocalCurrency),
                                                                               totalProfitInLocalCurrency = m.Sum(d => d.ProfitInLocalCurrency),
                                                                               totalProfitInProfitCurrency = m.Sum(d => d.ProfitInProfitCurrency),
                                                                           }).ToList();



                                othersResultList = (from a in allCountriesResult
                                                    where !(from r in resultList where r.countryCode == a.countryCode select r).Any()
                                                    select a).ToList();
                                foreach (DashBoardClass d in othersResultList)
                                {
                                    d.countryCode = "Others";
                                    d.countryName = "Others";
                                    d.country = "Others";
                                }
                                unionList = resultList.Union(othersResultList).OrderByDescending(d => d.ReceivablesInLocalCurrency).ToList();

                            }
                            else
                            {
                                unionList = resultList.OrderByDescending(d => d.ReceivablesInLocalCurrency).ToList();
                            }
                        }

                        else
                        {
                            resultList = (from s in shipments
                                          where s.OperationalDate > lastDate && s.OperationalDate <= FromDateQuery
                                          group s by new
                                          {
                                              s.CountryForStatisticsCode,
                                              s.CountryForStatisticsName

                                          } into m
                                          select new DashBoardClass()
                                          {
                                              countryCode = m.Key.CountryForStatisticsCode,
                                              countryName = m.Key.CountryForStatisticsName,
                                              country = m.Key.CountryForStatisticsCode,
                                              total = m.Count(),
                                              sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                              sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                              totalLastMonth = 0,
                                              sumChargeableWeightLastMonth = 0,
                                              sumGrossWeightLastMonth = 0,
                                              ReceivablesInProfitCurrency = m.Sum(d => d.OpenReceivablesInProfitCurrency),
                                              ReceivablesInLocalCurrency = m.Sum(d => d.OpenReceivablesInLocalCurrency),
                                              totalProfitInLocalCurrency = m.Sum(d => d.ProfitInLocalCurrency),
                                              totalProfitInProfitCurrency = m.Sum(d => d.ProfitInProfitCurrency),
                                          }).OrderByDescending(d => d.ReceivablesInLocalCurrency).Take(top).ToList();


                            if (includeOthers)
                            {
                                List<DashBoardClass> allCountriesResult = (from s in shipments
                                                                           where s.OperationalDate > lastDate && s.OperationalDate <= FromDateQuery
                                                                           group s by new
                                                                           {
                                                                               s.CountryForStatisticsCode,
                                                                               s.CountryForStatisticsName

                                                                           } into m
                                                                           select new DashBoardClass()
                                                                           {
                                                                               countryCode = m.Key.CountryForStatisticsCode,
                                                                               countryName = m.Key.CountryForStatisticsName,
                                                                               country = m.Key.CountryForStatisticsCode,
                                                                               total = m.Count(),
                                                                               sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                                                               sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                                                               totalLastMonth = 0,
                                                                               sumChargeableWeightLastMonth = 0,
                                                                               sumGrossWeightLastMonth = 0,
                                                                               ReceivablesInProfitCurrency = m.Sum(d => d.OpenReceivablesInProfitCurrency),
                                                                               ReceivablesInLocalCurrency = m.Sum(d => d.OpenReceivablesInLocalCurrency),
                                                                               totalProfitInLocalCurrency = m.Sum(d => d.ProfitInLocalCurrency),
                                                                               totalProfitInProfitCurrency = m.Sum(d => d.ProfitInProfitCurrency),
                                                                           }).ToList();



                                othersResultList = (from a in allCountriesResult
                                                    where !(from r in resultList where r.countryCode == a.countryCode select r).Any()
                                                    select a).ToList();
                                foreach (DashBoardClass d in othersResultList)
                                {
                                    d.countryCode = "Others";
                                    d.countryName = "Others";
                                    d.country = "Others";
                                }
                                unionList = resultList.Union(othersResultList).OrderByDescending(d => d.ReceivablesInLocalCurrency).ToList();

                            }
                            else
                            {
                                unionList = resultList.OrderByDescending(d => d.ReceivablesInLocalCurrency).ToList();
                            }
                        }

                        break;
                    }
                #endregion

                #region if the measurement is receivables in profit
                case 6:
                    {

                        if (type == "CreateDate" || type == null)
                        {

                            resultList = (from s in shipments
                                          where s.CreateDateTime > lastDate
                                          group s by new
                                          {
                                              s.CountryForStatisticsCode,
                                              s.CountryForStatisticsName

                                          } into m
                                          select new DashBoardClass()
                                          {
                                              countryCode = m.Key.CountryForStatisticsCode,
                                              countryName = m.Key.CountryForStatisticsName,
                                              country = m.Key.CountryForStatisticsCode,
                                              total = m.Count(),
                                              sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                              sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                              totalLastMonth = 0,
                                              sumChargeableWeightLastMonth = 0,
                                              sumGrossWeightLastMonth = 0,
                                              ReceivablesInProfitCurrency = m.Sum(d => d.OpenReceivablesInProfitCurrency),
                                              ReceivablesInLocalCurrency = m.Sum(d => d.OpenReceivablesInLocalCurrency),
                                              totalProfitInLocalCurrency = m.Sum(d => d.ProfitInLocalCurrency),
                                              totalProfitInProfitCurrency = m.Sum(d => d.ProfitInProfitCurrency),
                                          }).OrderByDescending(d => d.ReceivablesInProfitCurrency).Take(top).ToList();


                            if (includeOthers)
                            {
                                List<DashBoardClass> allCountriesResult = (from s in shipments
                                                                           where s.CreateDateTime > lastDate
                                                                           group s by new
                                                                           {
                                                                               s.CountryForStatisticsCode,
                                                                               s.CountryForStatisticsName

                                                                           } into m
                                                                           select new DashBoardClass()
                                                                           {
                                                                               countryCode = m.Key.CountryForStatisticsCode,
                                                                               countryName = m.Key.CountryForStatisticsName,
                                                                               country = m.Key.CountryForStatisticsCode,
                                                                               total = m.Count(),
                                                                               sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                                                               sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                                                               totalLastMonth = 0,
                                                                               sumChargeableWeightLastMonth = 0,
                                                                               sumGrossWeightLastMonth = 0,
                                                                               ReceivablesInProfitCurrency = m.Sum(d => d.OpenReceivablesInProfitCurrency),
                                                                               ReceivablesInLocalCurrency = m.Sum(d => d.OpenReceivablesInLocalCurrency),
                                                                               totalProfitInLocalCurrency = m.Sum(d => d.ProfitInLocalCurrency),
                                                                               totalProfitInProfitCurrency = m.Sum(d => d.ProfitInProfitCurrency),
                                                                           }).ToList();



                                othersResultList = (from a in allCountriesResult
                                                    where !(from r in resultList where r.countryCode == a.countryCode select r).Any()
                                                    select a).ToList();
                                foreach (DashBoardClass d in othersResultList)
                                {
                                    d.countryCode = "Others";
                                    d.countryName = "Others";
                                    d.country = "Others";
                                }
                                unionList = resultList.Union(othersResultList).OrderByDescending(d => d.ReceivablesInProfitCurrency).ToList();

                            }
                            else
                            {
                                unionList = resultList.OrderByDescending(d => d.ReceivablesInProfitCurrency).ToList();
                            }
                        }

                        else
                        {
                            resultList = (from s in shipments
                                          where s.OperationalDate > lastDate && s.OperationalDate <= FromDateQuery
                                          group s by new
                                          {
                                              s.CountryForStatisticsCode,
                                              s.CountryForStatisticsName

                                          } into m
                                          select new DashBoardClass()
                                          {
                                              countryCode = m.Key.CountryForStatisticsCode,
                                              countryName = m.Key.CountryForStatisticsName,
                                              country = m.Key.CountryForStatisticsCode,
                                              total = m.Count(),
                                              sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                              sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                              totalLastMonth = 0,
                                              sumChargeableWeightLastMonth = 0,
                                              sumGrossWeightLastMonth = 0,
                                              ReceivablesInProfitCurrency = m.Sum(d => d.OpenReceivablesInProfitCurrency),
                                              ReceivablesInLocalCurrency = m.Sum(d => d.OpenReceivablesInLocalCurrency),
                                              totalProfitInLocalCurrency = m.Sum(d => d.ProfitInLocalCurrency),
                                              totalProfitInProfitCurrency = m.Sum(d => d.ProfitInProfitCurrency),
                                          }).OrderByDescending(d => d.ReceivablesInProfitCurrency).Take(top).ToList();


                            if (includeOthers)
                            {
                                List<DashBoardClass> allCountriesResult = (from s in shipments
                                                                           where s.OperationalDate > lastDate && s.OperationalDate <= FromDateQuery
                                                                           group s by new
                                                                           {
                                                                               s.CountryForStatisticsCode,
                                                                               s.CountryForStatisticsName

                                                                           } into m
                                                                           select new DashBoardClass()
                                                                           {
                                                                               countryCode = m.Key.CountryForStatisticsCode,
                                                                               countryName = m.Key.CountryForStatisticsName,
                                                                               country = m.Key.CountryForStatisticsCode,
                                                                               total = m.Count(),
                                                                               sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                                                               sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                                                               totalLastMonth = 0,
                                                                               sumChargeableWeightLastMonth = 0,
                                                                               sumGrossWeightLastMonth = 0,
                                                                               ReceivablesInProfitCurrency = m.Sum(d => d.OpenReceivablesInProfitCurrency),
                                                                               ReceivablesInLocalCurrency = m.Sum(d => d.OpenReceivablesInLocalCurrency),
                                                                               totalProfitInLocalCurrency = m.Sum(d => d.ProfitInLocalCurrency),
                                                                               totalProfitInProfitCurrency = m.Sum(d => d.ProfitInProfitCurrency),
                                                                           }).ToList();



                                othersResultList = (from a in allCountriesResult
                                                    where !(from r in resultList where r.countryCode == a.countryCode select r).Any()
                                                    select a).ToList();
                                foreach (DashBoardClass d in othersResultList)
                                {
                                    d.countryCode = "Others";
                                    d.countryName = "Others";
                                    d.country = "Others";
                                }
                                unionList = resultList.Union(othersResultList).OrderByDescending(d => d.ReceivablesInProfitCurrency).ToList();

                            }
                            else
                            {
                                unionList = resultList.OrderByDescending(d => d.ReceivablesInProfitCurrency).ToList();
                            }
                        }

                        break;
                    }
                    #endregion


            }

            return unionList;
        }
        public List<DashBoardClass> GetShipmentsByTop10CountriesDashBoardCustom2(string type, DateTime? FromDate, DateTime? ToDate, int measurment, int currentTenant, int top, bool includeOthers, string customerid, string directionId, string transportmodeId)
        {
            if (top < 0)
            {
                top = 0;
            }

            FromDate = new DateTime(FromDate.Value.Year, FromDate.Value.Month, FromDate.Value.Day, 0, 0, 0);
            ToDate = ToDate.Value.AddDays(1);



            List<DashBoardClass> unionList = null;
            IQueryable<ShipmentCountryDashboardView> shipments = repository.GetShipmentDataViewsForCountriesDashboard(currentTenant, customerid, directionId, transportmodeId);

            List<DashBoardClass> resultList = null;
            List<DashBoardClass> othersResultList = null;

            switch (measurment)
            {
                #region if the measurement is shipment count
                case 0:
                    {

                        if (type == "CreateDate" || type == null)
                        {
                            resultList = (from s in shipments
                                          where s.CreateDateTime >= FromDate && s.CreateDateTime < ToDate
                                          group s by new
                                          {
                                              s.CountryForStatisticsCode,
                                              s.CountryForStatisticsName

                                          } into m
                                          select new DashBoardClass()
                                          {
                                              countryCode = m.Key.CountryForStatisticsCode,
                                              countryName = m.Key.CountryForStatisticsName,
                                              country = m.Key.CountryForStatisticsCode,
                                              total = m.Count(),
                                              sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                              sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                              totalLastMonth = 0,
                                              sumChargeableWeightLastMonth = 0,
                                              sumGrossWeightLastMonth = 0,
                                          }).OrderByDescending(d => d.total).Take(top).ToList();


                            if (includeOthers)
                            {

                                List<DashBoardClass> allCountriesResult = (from s in shipments
                                                                           where s.CreateDateTime >= FromDate && s.CreateDateTime < ToDate
                                                                           group s by new
                                                                           {
                                                                               s.CountryForStatisticsCode,
                                                                               s.CountryForStatisticsName

                                                                           } into m
                                                                           select new DashBoardClass()
                                                                           {
                                                                               countryCode = m.Key.CountryForStatisticsCode,
                                                                               countryName = m.Key.CountryForStatisticsName,
                                                                               country = m.Key.CountryForStatisticsCode,
                                                                               total = m.Count(),
                                                                               sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                                                               sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                                                               totalLastMonth = 0,
                                                                               sumChargeableWeightLastMonth = 0,
                                                                               sumGrossWeightLastMonth = 0,
                                                                           }).ToList();




                                othersResultList = (from a in allCountriesResult
                                                    where !(from r in resultList where r.countryCode == a.countryCode select r).Any()
                                                    select a).ToList();
                                foreach (DashBoardClass d in othersResultList)
                                {
                                    d.countryCode = "Others";
                                    d.countryName = "Others";
                                    d.country = "Others";
                                }

                                unionList = resultList.Union(othersResultList).OrderByDescending(d => d.total).ToList();
                            }


                            else
                            {
                                unionList = resultList.ToList();
                            }
                        }
                        else
                        {
                            resultList = (from s in shipments
                                          where s.OperationalDate >= FromDate && s.OperationalDate < ToDate
                                          group s by new
                                          {
                                              s.CountryForStatisticsCode,
                                              s.CountryForStatisticsName

                                          } into m
                                          select new DashBoardClass()
                                          {
                                              countryCode = m.Key.CountryForStatisticsCode,
                                              countryName = m.Key.CountryForStatisticsName,
                                              country = m.Key.CountryForStatisticsCode,
                                              total = m.Count(),
                                              sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                              sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                              totalLastMonth = 0,
                                              sumChargeableWeightLastMonth = 0,
                                              sumGrossWeightLastMonth = 0,
                                          }).OrderByDescending(d => d.total).Take(top).ToList();


                            if (includeOthers)
                            {

                                List<DashBoardClass> allCountriesResult = (from s in shipments
                                                                           where s.OperationalDate >= FromDate && s.OperationalDate < ToDate
                                                                           group s by new
                                                                           {
                                                                               s.CountryForStatisticsCode,
                                                                               s.CountryForStatisticsName

                                                                           } into m
                                                                           select new DashBoardClass()
                                                                           {
                                                                               countryCode = m.Key.CountryForStatisticsCode,
                                                                               countryName = m.Key.CountryForStatisticsName,
                                                                               country = m.Key.CountryForStatisticsCode,
                                                                               total = m.Count(),
                                                                               sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                                                               sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                                                               totalLastMonth = 0,
                                                                               sumChargeableWeightLastMonth = 0,
                                                                               sumGrossWeightLastMonth = 0,
                                                                           }).ToList();




                                othersResultList = (from a in allCountriesResult
                                                    where !(from r in resultList where r.countryCode == a.countryCode select r).Any()
                                                    select a).ToList();
                                foreach (DashBoardClass d in othersResultList)
                                {
                                    d.countryCode = "Others";
                                    d.countryName = "Others";
                                    d.country = "Others";
                                }

                                unionList = resultList.Union(othersResultList).OrderByDescending(d => d.total).ToList();
                            }


                            else
                            {
                                unionList = resultList.ToList();
                            }
                        }


                        break;
                    }
                #endregion

                #region if the measurement is chargeable weight
                case 1:
                    {

                        if (type == "CreateDate" || type == null)
                        {
                            resultList = (from s in shipments
                                          where s.CreateDateTime >= FromDate && s.CreateDateTime < ToDate
                                          group s by new
                                          {
                                              s.CountryForStatisticsCode,
                                              s.CountryForStatisticsName

                                          } into m
                                          select new DashBoardClass()
                                          {
                                              countryCode = m.Key.CountryForStatisticsCode,
                                              countryName = m.Key.CountryForStatisticsName,
                                              country = m.Key.CountryForStatisticsCode,
                                              total = m.Count(),
                                              sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                              sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                              totalLastMonth = 0,
                                              sumChargeableWeightLastMonth = 0,
                                              sumGrossWeightLastMonth = 0,
                                          }).OrderByDescending(d => d.sumChargeableWeight).Take(top).ToList();


                            if (includeOthers)
                            {
                                List<DashBoardClass> allCountriesResult = (from s in shipments
                                                                           where s.CreateDateTime >= FromDate && s.CreateDateTime < ToDate
                                                                           group s by new
                                                                           {
                                                                               s.CountryForStatisticsCode,
                                                                               s.CountryForStatisticsName

                                                                           } into m
                                                                           select new DashBoardClass()
                                                                           {
                                                                               countryCode = m.Key.CountryForStatisticsCode,
                                                                               countryName = m.Key.CountryForStatisticsName,
                                                                               country = m.Key.CountryForStatisticsCode,
                                                                               total = m.Count(),
                                                                               sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                                                               sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                                                               totalLastMonth = 0,
                                                                               sumChargeableWeightLastMonth = 0,
                                                                               sumGrossWeightLastMonth = 0,
                                                                           }).ToList();




                                othersResultList = (from a in allCountriesResult
                                                    where !(from r in resultList where r.countryCode == a.countryCode select r).Any()
                                                    select a).ToList();
                                foreach (DashBoardClass d in othersResultList)
                                {
                                    d.countryCode = "Others";
                                    d.countryName = "Others";
                                    d.country = "Others";
                                }

                                unionList = resultList.Union(othersResultList).OrderByDescending(d => d.sumChargeableWeight).ToList();

                            }
                            else
                            {
                                unionList = resultList.OrderByDescending(d => d.sumChargeableWeight).ToList();
                            }
                        }
                        else
                        {
                            resultList = (from s in shipments
                                          where s.OperationalDate >= FromDate && s.OperationalDate < ToDate
                                          group s by new
                                          {
                                              s.CountryForStatisticsCode,
                                              s.CountryForStatisticsName

                                          } into m
                                          select new DashBoardClass()
                                          {
                                              countryCode = m.Key.CountryForStatisticsCode,
                                              countryName = m.Key.CountryForStatisticsName,
                                              country = m.Key.CountryForStatisticsCode,
                                              total = m.Count(),
                                              sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                              sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                              totalLastMonth = 0,
                                              sumChargeableWeightLastMonth = 0,
                                              sumGrossWeightLastMonth = 0,
                                          }).OrderByDescending(d => d.sumChargeableWeight).Take(top).ToList();


                            if (includeOthers)
                            {
                                List<DashBoardClass> allCountriesResult = (from s in shipments
                                                                           where s.OperationalDate >= FromDate && s.OperationalDate < ToDate
                                                                           group s by new
                                                                           {
                                                                               s.CountryForStatisticsCode,
                                                                               s.CountryForStatisticsName

                                                                           } into m
                                                                           select new DashBoardClass()
                                                                           {
                                                                               countryCode = m.Key.CountryForStatisticsCode,
                                                                               countryName = m.Key.CountryForStatisticsName,
                                                                               country = m.Key.CountryForStatisticsCode,
                                                                               total = m.Count(),
                                                                               sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                                                               sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                                                               totalLastMonth = 0,
                                                                               sumChargeableWeightLastMonth = 0,
                                                                               sumGrossWeightLastMonth = 0,
                                                                           }).ToList();




                                othersResultList = (from a in allCountriesResult
                                                    where !(from r in resultList where r.countryCode == a.countryCode select r).Any()
                                                    select a).ToList();
                                foreach (DashBoardClass d in othersResultList)
                                {
                                    d.countryCode = "Others";
                                    d.countryName = "Others";
                                    d.country = "Others";
                                }

                                unionList = resultList.Union(othersResultList).OrderByDescending(d => d.sumChargeableWeight).ToList();

                            }
                            else
                            {
                                unionList = resultList.OrderByDescending(d => d.sumChargeableWeight).ToList();
                            }
                        }

                        break;
                    }
                #endregion

                #region if the measurement is gross weight
                case 2:
                    {
                        if (type == "CreateDate" || type == null)
                        {
                            resultList = (from s in shipments
                                          where s.CreateDateTime >= FromDate && s.CreateDateTime < ToDate
                                          group s by new
                                          {
                                              s.CountryForStatisticsCode,
                                              s.CountryForStatisticsName

                                          } into m
                                          select new DashBoardClass()
                                          {
                                              countryCode = m.Key.CountryForStatisticsCode,
                                              countryName = m.Key.CountryForStatisticsName,
                                              country = m.Key.CountryForStatisticsCode,
                                              total = m.Count(),
                                              sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                              sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                              totalLastMonth = 0,
                                              sumChargeableWeightLastMonth = 0,
                                              sumGrossWeightLastMonth = 0,
                                          }).OrderByDescending(d => d.sumGrossWeight).Take(top).ToList();


                            if (includeOthers)
                            {
                                List<DashBoardClass> allCountriesResult = (from s in shipments
                                                                           where s.CreateDateTime >= FromDate && s.CreateDateTime < ToDate
                                                                           group s by new
                                                                           {
                                                                               s.CountryForStatisticsCode,
                                                                               s.CountryForStatisticsName

                                                                           } into m
                                                                           select new DashBoardClass()
                                                                           {
                                                                               countryCode = m.Key.CountryForStatisticsCode,
                                                                               countryName = m.Key.CountryForStatisticsName,
                                                                               country = m.Key.CountryForStatisticsCode,
                                                                               total = m.Count(),
                                                                               sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                                                               sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                                                               totalLastMonth = 0,
                                                                               sumChargeableWeightLastMonth = 0,
                                                                               sumGrossWeightLastMonth = 0,
                                                                           }).ToList();

                                othersResultList = (from a in allCountriesResult
                                                    where !(from r in resultList where r.countryCode == a.countryCode select r).Any()
                                                    select a).ToList();
                                foreach (DashBoardClass d in othersResultList)
                                {
                                    d.countryCode = "Others";
                                    d.countryName = "Others";
                                    d.country = "Others";
                                }

                                unionList = resultList.Union(othersResultList).OrderByDescending(d => d.sumGrossWeight).ToList();

                            }
                            else
                            {
                                unionList = resultList.OrderByDescending(d => d.sumGrossWeight).ToList();
                            }
                        }

                        else
                        {
                            resultList = (from s in shipments
                                          where s.OperationalDate >= FromDate && s.OperationalDate < ToDate
                                          group s by new
                                          {
                                              s.CountryForStatisticsCode,
                                              s.CountryForStatisticsName

                                          } into m
                                          select new DashBoardClass()
                                          {
                                              countryCode = m.Key.CountryForStatisticsCode,
                                              countryName = m.Key.CountryForStatisticsName,
                                              country = m.Key.CountryForStatisticsCode,
                                              total = m.Count(),
                                              sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                              sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                              totalLastMonth = 0,
                                              sumChargeableWeightLastMonth = 0,
                                              sumGrossWeightLastMonth = 0,
                                          }).OrderByDescending(d => d.sumGrossWeight).Take(top).ToList();


                            if (includeOthers)
                            {
                                List<DashBoardClass> allCountriesResult = (from s in shipments
                                                                           where s.OperationalDate >= FromDate && s.OperationalDate < ToDate
                                                                           group s by new
                                                                           {
                                                                               s.CountryForStatisticsCode,
                                                                               s.CountryForStatisticsName

                                                                           } into m
                                                                           select new DashBoardClass()
                                                                           {
                                                                               countryCode = m.Key.CountryForStatisticsCode,
                                                                               countryName = m.Key.CountryForStatisticsName,
                                                                               country = m.Key.CountryForStatisticsCode,
                                                                               total = m.Count(),
                                                                               sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                                                               sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                                                               totalLastMonth = 0,
                                                                               sumChargeableWeightLastMonth = 0,
                                                                               sumGrossWeightLastMonth = 0,
                                                                           }).ToList();

                                othersResultList = (from a in allCountriesResult
                                                    where !(from r in resultList where r.countryCode == a.countryCode select r).Any()
                                                    select a).ToList();
                                foreach (DashBoardClass d in othersResultList)
                                {
                                    d.countryCode = "Others";
                                    d.countryName = "Others";
                                    d.country = "Others";
                                }

                                unionList = resultList.Union(othersResultList).OrderByDescending(d => d.sumGrossWeight).ToList();

                            }
                            else
                            {
                                unionList = resultList.OrderByDescending(d => d.sumGrossWeight).ToList();
                            }
                        }

                        break;
                    }
                #endregion

                #region if the measurement is profit in local
                case 3:
                    {
                        if (type == "CreateDate" || type == null)
                        {
                            resultList = (from s in shipments
                                          where s.CreateDateTime >= FromDate && s.CreateDateTime < ToDate
                                          group s by new
                                          {
                                              s.CountryForStatisticsCode,
                                              s.CountryForStatisticsName

                                          } into m
                                          select new DashBoardClass()
                                          {
                                              countryCode = m.Key.CountryForStatisticsCode,
                                              countryName = m.Key.CountryForStatisticsName,
                                              country = m.Key.CountryForStatisticsCode,
                                              total = m.Count(),
                                              sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                              sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                              totalLastMonth = 0,
                                              sumChargeableWeightLastMonth = 0,
                                              sumGrossWeightLastMonth = 0,
                                              ReceivablesInProfitCurrency = m.Sum(d => d.OpenReceivablesInLocalCurrency),
                                              totalProfitInLocalCurrency = m.Sum(d => d.ProfitInLocalCurrency),
                                              totalProfitInProfitCurrency = m.Sum(d => d.ProfitInProfitCurrency),
                                          }).OrderByDescending(d => d.totalProfitInLocalCurrency).Take(top).ToList();


                            if (includeOthers)
                            {
                                List<DashBoardClass> allCountriesResult = (from s in shipments
                                                                           where s.CreateDateTime >= FromDate && s.CreateDateTime < ToDate
                                                                           group s by new
                                                                           {
                                                                               s.CountryForStatisticsCode,
                                                                               s.CountryForStatisticsName

                                                                           } into m
                                                                           select new DashBoardClass()
                                                                           {
                                                                               countryCode = m.Key.CountryForStatisticsCode,
                                                                               countryName = m.Key.CountryForStatisticsName,
                                                                               country = m.Key.CountryForStatisticsCode,
                                                                               total = m.Count(),
                                                                               sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                                                               sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                                                               totalLastMonth = 0,
                                                                               sumChargeableWeightLastMonth = 0,
                                                                               sumGrossWeightLastMonth = 0,
                                                                               ReceivablesInProfitCurrency = m.Sum(d => d.OpenReceivablesInLocalCurrency),
                                                                               totalProfitInLocalCurrency = m.Sum(d => d.ProfitInLocalCurrency),
                                                                               totalProfitInProfitCurrency = m.Sum(d => d.ProfitInProfitCurrency),
                                                                           }).ToList();




                                othersResultList = (from a in allCountriesResult
                                                    where !(from r in resultList where r.countryCode == a.countryCode select r).Any()
                                                    select a).ToList();
                                foreach (DashBoardClass d in othersResultList)
                                {
                                    d.countryCode = "Others";
                                    d.countryName = "Others";
                                    d.country = "Others";
                                }
                                unionList = resultList.Union(othersResultList).OrderByDescending(d => d.totalProfitInLocalCurrency).ToList();

                            }
                            else
                            {
                                unionList = resultList.OrderByDescending(d => d.totalProfitInLocalCurrency).ToList();//ResulList;//ResulListMonth.Union(ResulList);
                            }
                        }
                        else
                        {
                            resultList = (from s in shipments
                                          where s.OperationalDate >= FromDate && s.OperationalDate < ToDate
                                          group s by new
                                          {
                                              s.CountryForStatisticsCode,
                                              s.CountryForStatisticsName

                                          } into m
                                          select new DashBoardClass()
                                          {
                                              countryCode = m.Key.CountryForStatisticsCode,
                                              countryName = m.Key.CountryForStatisticsName,
                                              country = m.Key.CountryForStatisticsCode,
                                              total = m.Count(),
                                              sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                              sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                              totalLastMonth = 0,
                                              sumChargeableWeightLastMonth = 0,
                                              sumGrossWeightLastMonth = 0,
                                              ReceivablesInProfitCurrency = m.Sum(d => d.OpenReceivablesInLocalCurrency),
                                              totalProfitInLocalCurrency = m.Sum(d => d.ProfitInLocalCurrency),
                                              totalProfitInProfitCurrency = m.Sum(d => d.ProfitInProfitCurrency),
                                          }).OrderByDescending(d => d.totalProfitInLocalCurrency).Take(top).ToList();


                            if (includeOthers)
                            {
                                List<DashBoardClass> allCountriesResult = (from s in shipments
                                                                           where s.OperationalDate >= FromDate && s.OperationalDate < ToDate
                                                                           group s by new
                                                                           {
                                                                               s.CountryForStatisticsCode,
                                                                               s.CountryForStatisticsName

                                                                           } into m
                                                                           select new DashBoardClass()
                                                                           {
                                                                               countryCode = m.Key.CountryForStatisticsCode,
                                                                               countryName = m.Key.CountryForStatisticsName,
                                                                               country = m.Key.CountryForStatisticsCode,
                                                                               total = m.Count(),
                                                                               sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                                                               sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                                                               totalLastMonth = 0,
                                                                               sumChargeableWeightLastMonth = 0,
                                                                               sumGrossWeightLastMonth = 0,
                                                                               ReceivablesInProfitCurrency = m.Sum(d => d.OpenReceivablesInLocalCurrency),
                                                                               totalProfitInLocalCurrency = m.Sum(d => d.ProfitInLocalCurrency),
                                                                               totalProfitInProfitCurrency = m.Sum(d => d.ProfitInProfitCurrency),
                                                                           }).ToList();




                                othersResultList = (from a in allCountriesResult
                                                    where !(from r in resultList where r.countryCode == a.countryCode select r).Any()
                                                    select a).ToList();
                                foreach (DashBoardClass d in othersResultList)
                                {
                                    d.countryCode = "Others";
                                    d.countryName = "Others";
                                    d.country = "Others";
                                }
                                unionList = resultList.Union(othersResultList).OrderByDescending(d => d.totalProfitInLocalCurrency).ToList();

                            }
                            else
                            {
                                unionList = resultList.OrderByDescending(d => d.totalProfitInLocalCurrency).ToList();//ResulList;//ResulListMonth.Union(ResulList);
                            }
                        }

                        break;
                    }
                #endregion

                #region if the measurement is profit in profit
                case 4:
                    {
                        if (type == "CreateDate" || type == null)
                        {
                            resultList = (from s in shipments
                                          where s.CreateDateTime >= FromDate && s.CreateDateTime < ToDate
                                          group s by new
                                          {
                                              s.CountryForStatisticsCode,
                                              s.CountryForStatisticsName

                                          } into m
                                          select new DashBoardClass()
                                          {
                                              countryCode = m.Key.CountryForStatisticsCode,
                                              countryName = m.Key.CountryForStatisticsName,
                                              country = m.Key.CountryForStatisticsCode,
                                              total = m.Count(),
                                              sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                              sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                              totalLastMonth = 0,
                                              sumChargeableWeightLastMonth = 0,
                                              sumGrossWeightLastMonth = 0,
                                              ReceivablesInProfitCurrency = m.Sum(d => d.OpenReceivablesInLocalCurrency),
                                              totalProfitInLocalCurrency = m.Sum(d => d.ProfitInLocalCurrency),
                                              totalProfitInProfitCurrency = m.Sum(d => d.ProfitInProfitCurrency),
                                          }).OrderByDescending(d => d.totalProfitInProfitCurrency).Take(top).ToList();


                            if (includeOthers)
                            {
                                List<DashBoardClass> allCountriesResult = (from s in shipments
                                                                           where s.CreateDateTime >= FromDate && s.CreateDateTime < ToDate
                                                                           group s by new
                                                                           {
                                                                               s.CountryForStatisticsCode,
                                                                               s.CountryForStatisticsName

                                                                           } into m
                                                                           select new DashBoardClass()
                                                                           {
                                                                               countryCode = m.Key.CountryForStatisticsCode,
                                                                               countryName = m.Key.CountryForStatisticsName,
                                                                               country = m.Key.CountryForStatisticsCode,
                                                                               total = m.Count(),
                                                                               sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                                                               sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                                                               totalLastMonth = 0,
                                                                               sumChargeableWeightLastMonth = 0,
                                                                               sumGrossWeightLastMonth = 0,
                                                                               ReceivablesInProfitCurrency = m.Sum(d => d.OpenReceivablesInLocalCurrency),
                                                                               totalProfitInLocalCurrency = m.Sum(d => d.ProfitInLocalCurrency),
                                                                               totalProfitInProfitCurrency = m.Sum(d => d.ProfitInProfitCurrency),
                                                                           }).ToList();



                                othersResultList = (from a in allCountriesResult
                                                    where !(from r in resultList where r.countryCode == a.countryCode select r).Any()
                                                    select a).ToList();
                                foreach (DashBoardClass d in othersResultList)
                                {
                                    d.countryCode = "Others";
                                    d.countryName = "Others";
                                    d.country = "Others";
                                }
                                unionList = resultList.Union(othersResultList).OrderByDescending(d => d.totalProfitInProfitCurrency).ToList();

                            }
                            else
                            {
                                unionList = resultList.OrderByDescending(d => d.totalProfitInProfitCurrency).ToList();
                            }
                        }
                        else
                        {
                            resultList = (from s in shipments
                                          where s.OperationalDate >= FromDate && s.OperationalDate < ToDate
                                          group s by new
                                          {
                                              s.CountryForStatisticsCode,
                                              s.CountryForStatisticsName

                                          } into m
                                          select new DashBoardClass()
                                          {
                                              countryCode = m.Key.CountryForStatisticsCode,
                                              countryName = m.Key.CountryForStatisticsName,
                                              country = m.Key.CountryForStatisticsCode,
                                              total = m.Count(),
                                              sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                              sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                              totalLastMonth = 0,
                                              sumChargeableWeightLastMonth = 0,
                                              sumGrossWeightLastMonth = 0,
                                              ReceivablesInProfitCurrency = m.Sum(d => d.OpenReceivablesInLocalCurrency),
                                              totalProfitInLocalCurrency = m.Sum(d => d.ProfitInLocalCurrency),
                                              totalProfitInProfitCurrency = m.Sum(d => d.ProfitInProfitCurrency),
                                          }).OrderByDescending(d => d.totalProfitInProfitCurrency).Take(top).ToList();


                            if (includeOthers)
                            {
                                List<DashBoardClass> allCountriesResult = (from s in shipments
                                                                           where s.OperationalDate >= FromDate && s.OperationalDate < ToDate
                                                                           group s by new
                                                                           {
                                                                               s.CountryForStatisticsCode,
                                                                               s.CountryForStatisticsName

                                                                           } into m
                                                                           select new DashBoardClass()
                                                                           {
                                                                               countryCode = m.Key.CountryForStatisticsCode,
                                                                               countryName = m.Key.CountryForStatisticsName,
                                                                               country = m.Key.CountryForStatisticsCode,
                                                                               total = m.Count(),
                                                                               sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                                                               sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                                                               totalLastMonth = 0,
                                                                               sumChargeableWeightLastMonth = 0,
                                                                               sumGrossWeightLastMonth = 0,
                                                                               ReceivablesInProfitCurrency = m.Sum(d => d.OpenReceivablesInLocalCurrency),
                                                                               totalProfitInLocalCurrency = m.Sum(d => d.ProfitInLocalCurrency),
                                                                               totalProfitInProfitCurrency = m.Sum(d => d.ProfitInProfitCurrency),
                                                                           }).ToList();



                                othersResultList = (from a in allCountriesResult
                                                    where !(from r in resultList where r.countryCode == a.countryCode select r).Any()
                                                    select a).ToList();
                                foreach (DashBoardClass d in othersResultList)
                                {
                                    d.countryCode = "Others";
                                    d.countryName = "Others";
                                    d.country = "Others";
                                }
                                unionList = resultList.Union(othersResultList).OrderByDescending(d => d.totalProfitInProfitCurrency).ToList();

                            }
                            else
                            {
                                unionList = resultList.OrderByDescending(d => d.totalProfitInProfitCurrency).ToList();
                            }
                        }

                        break;
                    }
                #endregion

                #region if the measurement is receivables in local
                case 5:
                    {
                        if (type == "CreateDate" || type == null)
                        {
                            resultList = (from s in shipments
                                          where s.CreateDateTime >= FromDate && s.CreateDateTime < ToDate
                                          group s by new
                                          {
                                              s.CountryForStatisticsCode,
                                              s.CountryForStatisticsName

                                          } into m
                                          select new DashBoardClass()
                                          {
                                              countryCode = m.Key.CountryForStatisticsCode,
                                              countryName = m.Key.CountryForStatisticsName,
                                              country = m.Key.CountryForStatisticsCode,
                                              total = m.Count(),
                                              sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                              sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                              totalLastMonth = 0,
                                              sumChargeableWeightLastMonth = 0,
                                              sumGrossWeightLastMonth = 0,
                                              ReceivablesInProfitCurrency = m.Sum(d => d.OpenReceivablesInProfitCurrency),
                                              ReceivablesInLocalCurrency = m.Sum(d => d.OpenReceivablesInLocalCurrency),
                                              totalProfitInLocalCurrency = m.Sum(d => d.ProfitInLocalCurrency),
                                              totalProfitInProfitCurrency = m.Sum(d => d.ProfitInProfitCurrency),
                                          }).OrderByDescending(d => d.ReceivablesInLocalCurrency).Take(top).ToList();


                            if (includeOthers)
                            {
                                List<DashBoardClass> allCountriesResult = (from s in shipments
                                                                           where s.CreateDateTime >= FromDate && s.CreateDateTime < ToDate
                                                                           group s by new
                                                                           {
                                                                               s.CountryForStatisticsCode,
                                                                               s.CountryForStatisticsName

                                                                           } into m
                                                                           select new DashBoardClass()
                                                                           {
                                                                               countryCode = m.Key.CountryForStatisticsCode,
                                                                               countryName = m.Key.CountryForStatisticsName,
                                                                               country = m.Key.CountryForStatisticsCode,
                                                                               total = m.Count(),
                                                                               sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                                                               sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                                                               totalLastMonth = 0,
                                                                               sumChargeableWeightLastMonth = 0,
                                                                               sumGrossWeightLastMonth = 0,
                                                                               ReceivablesInProfitCurrency = m.Sum(d => d.OpenReceivablesInProfitCurrency),
                                                                               ReceivablesInLocalCurrency = m.Sum(d => d.OpenReceivablesInLocalCurrency),
                                                                               totalProfitInLocalCurrency = m.Sum(d => d.ProfitInLocalCurrency),
                                                                               totalProfitInProfitCurrency = m.Sum(d => d.ProfitInProfitCurrency),
                                                                           }).ToList();



                                othersResultList = (from a in allCountriesResult
                                                    where !(from r in resultList where r.countryCode == a.countryCode select r).Any()
                                                    select a).ToList();
                                foreach (DashBoardClass d in othersResultList)
                                {
                                    d.countryCode = "Others";
                                    d.countryName = "Others";
                                    d.country = "Others";
                                }
                                unionList = resultList.Union(othersResultList).OrderByDescending(d => d.ReceivablesInLocalCurrency).ToList();

                            }
                            else
                            {
                                unionList = resultList.OrderByDescending(d => d.ReceivablesInLocalCurrency).ToList();
                            }
                        }
                        else
                        {
                            resultList = (from s in shipments
                                          where s.OperationalDate >= FromDate && s.OperationalDate < ToDate
                                          group s by new
                                          {
                                              s.CountryForStatisticsCode,
                                              s.CountryForStatisticsName

                                          } into m
                                          select new DashBoardClass()
                                          {
                                              countryCode = m.Key.CountryForStatisticsCode,
                                              countryName = m.Key.CountryForStatisticsName,
                                              country = m.Key.CountryForStatisticsCode,
                                              total = m.Count(),
                                              sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                              sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                              totalLastMonth = 0,
                                              sumChargeableWeightLastMonth = 0,
                                              sumGrossWeightLastMonth = 0,
                                              ReceivablesInProfitCurrency = m.Sum(d => d.OpenReceivablesInProfitCurrency),
                                              ReceivablesInLocalCurrency = m.Sum(d => d.OpenReceivablesInLocalCurrency),
                                              totalProfitInLocalCurrency = m.Sum(d => d.ProfitInLocalCurrency),
                                              totalProfitInProfitCurrency = m.Sum(d => d.ProfitInProfitCurrency),
                                          }).OrderByDescending(d => d.ReceivablesInLocalCurrency).Take(top).ToList();


                            if (includeOthers)
                            {
                                List<DashBoardClass> allCountriesResult = (from s in shipments
                                                                           where s.OperationalDate >= FromDate && s.OperationalDate < ToDate
                                                                           group s by new
                                                                           {
                                                                               s.CountryForStatisticsCode,
                                                                               s.CountryForStatisticsName

                                                                           } into m
                                                                           select new DashBoardClass()
                                                                           {
                                                                               countryCode = m.Key.CountryForStatisticsCode,
                                                                               countryName = m.Key.CountryForStatisticsName,
                                                                               country = m.Key.CountryForStatisticsCode,
                                                                               total = m.Count(),
                                                                               sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                                                               sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                                                               totalLastMonth = 0,
                                                                               sumChargeableWeightLastMonth = 0,
                                                                               sumGrossWeightLastMonth = 0,
                                                                               ReceivablesInProfitCurrency = m.Sum(d => d.OpenReceivablesInProfitCurrency),
                                                                               ReceivablesInLocalCurrency = m.Sum(d => d.OpenReceivablesInLocalCurrency),
                                                                               totalProfitInLocalCurrency = m.Sum(d => d.ProfitInLocalCurrency),
                                                                               totalProfitInProfitCurrency = m.Sum(d => d.ProfitInProfitCurrency),
                                                                           }).ToList();



                                othersResultList = (from a in allCountriesResult
                                                    where !(from r in resultList where r.countryCode == a.countryCode select r).Any()
                                                    select a).ToList();
                                foreach (DashBoardClass d in othersResultList)
                                {
                                    d.countryCode = "Others";
                                    d.countryName = "Others";
                                    d.country = "Others";
                                }
                                unionList = resultList.Union(othersResultList).OrderByDescending(d => d.ReceivablesInLocalCurrency).ToList();

                            }
                            else
                            {
                                unionList = resultList.OrderByDescending(d => d.ReceivablesInLocalCurrency).ToList();
                            }
                        }

                        break;
                    }
                #endregion

                #region if the measurement is receivables in profit
                case 6:
                    {
                        if (type == "CreateDate" || type == null)
                        {
                            resultList = (from s in shipments
                                          where s.CreateDateTime >= FromDate && s.CreateDateTime < ToDate
                                          group s by new
                                          {
                                              s.CountryForStatisticsCode,
                                              s.CountryForStatisticsName

                                          } into m
                                          select new DashBoardClass()
                                          {
                                              countryCode = m.Key.CountryForStatisticsCode,
                                              countryName = m.Key.CountryForStatisticsName,
                                              country = m.Key.CountryForStatisticsCode,
                                              total = m.Count(),
                                              sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                              sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                              totalLastMonth = 0,
                                              sumChargeableWeightLastMonth = 0,
                                              sumGrossWeightLastMonth = 0,
                                              ReceivablesInProfitCurrency = m.Sum(d => d.OpenReceivablesInProfitCurrency),
                                              ReceivablesInLocalCurrency = m.Sum(d => d.OpenReceivablesInLocalCurrency),
                                              totalProfitInLocalCurrency = m.Sum(d => d.ProfitInLocalCurrency),
                                              totalProfitInProfitCurrency = m.Sum(d => d.ProfitInProfitCurrency),
                                          }).OrderByDescending(d => d.ReceivablesInProfitCurrency).Take(top).ToList();


                            if (includeOthers)
                            {
                                List<DashBoardClass> allCountriesResult = (from s in shipments
                                                                           where s.CreateDateTime >= FromDate && s.CreateDateTime < ToDate
                                                                           group s by new
                                                                           {
                                                                               s.CountryForStatisticsCode,
                                                                               s.CountryForStatisticsName

                                                                           } into m
                                                                           select new DashBoardClass()
                                                                           {
                                                                               countryCode = m.Key.CountryForStatisticsCode,
                                                                               countryName = m.Key.CountryForStatisticsName,
                                                                               country = m.Key.CountryForStatisticsCode,
                                                                               total = m.Count(),
                                                                               sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                                                               sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                                                               totalLastMonth = 0,
                                                                               sumChargeableWeightLastMonth = 0,
                                                                               sumGrossWeightLastMonth = 0,
                                                                               ReceivablesInProfitCurrency = m.Sum(d => d.OpenReceivablesInProfitCurrency),
                                                                               ReceivablesInLocalCurrency = m.Sum(d => d.OpenReceivablesInLocalCurrency),
                                                                               totalProfitInLocalCurrency = m.Sum(d => d.ProfitInLocalCurrency),
                                                                               totalProfitInProfitCurrency = m.Sum(d => d.ProfitInProfitCurrency),
                                                                           }).ToList();



                                othersResultList = (from a in allCountriesResult
                                                    where !(from r in resultList where r.countryCode == a.countryCode select r).Any()
                                                    select a).ToList();
                                foreach (DashBoardClass d in othersResultList)
                                {
                                    d.countryCode = "Others";
                                    d.countryName = "Others";
                                    d.country = "Others";
                                }
                                unionList = resultList.Union(othersResultList).OrderByDescending(d => d.ReceivablesInProfitCurrency).ToList();

                            }
                            else
                            {
                                unionList = resultList.OrderByDescending(d => d.ReceivablesInProfitCurrency).ToList();
                            }
                        }

                        else
                        {
                            resultList = (from s in shipments
                                          where s.OperationalDate >= FromDate && s.OperationalDate < ToDate
                                          group s by new
                                          {
                                              s.CountryForStatisticsCode,
                                              s.CountryForStatisticsName

                                          } into m
                                          select new DashBoardClass()
                                          {
                                              countryCode = m.Key.CountryForStatisticsCode,
                                              countryName = m.Key.CountryForStatisticsName,
                                              country = m.Key.CountryForStatisticsCode,
                                              total = m.Count(),
                                              sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                              sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                              totalLastMonth = 0,
                                              sumChargeableWeightLastMonth = 0,
                                              sumGrossWeightLastMonth = 0,
                                              ReceivablesInProfitCurrency = m.Sum(d => d.OpenReceivablesInProfitCurrency),
                                              ReceivablesInLocalCurrency = m.Sum(d => d.OpenReceivablesInLocalCurrency),
                                              totalProfitInLocalCurrency = m.Sum(d => d.ProfitInLocalCurrency),
                                              totalProfitInProfitCurrency = m.Sum(d => d.ProfitInProfitCurrency),
                                          }).OrderByDescending(d => d.ReceivablesInProfitCurrency).Take(top).ToList();


                            if (includeOthers)
                            {
                                List<DashBoardClass> allCountriesResult = (from s in shipments
                                                                           where s.OperationalDate >= FromDate && s.OperationalDate < ToDate
                                                                           group s by new
                                                                           {
                                                                               s.CountryForStatisticsCode,
                                                                               s.CountryForStatisticsName

                                                                           } into m
                                                                           select new DashBoardClass()
                                                                           {
                                                                               countryCode = m.Key.CountryForStatisticsCode,
                                                                               countryName = m.Key.CountryForStatisticsName,
                                                                               country = m.Key.CountryForStatisticsCode,
                                                                               total = m.Count(),
                                                                               sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                                                                               sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                                                                               totalLastMonth = 0,
                                                                               sumChargeableWeightLastMonth = 0,
                                                                               sumGrossWeightLastMonth = 0,
                                                                               ReceivablesInProfitCurrency = m.Sum(d => d.OpenReceivablesInProfitCurrency),
                                                                               ReceivablesInLocalCurrency = m.Sum(d => d.OpenReceivablesInLocalCurrency),
                                                                               totalProfitInLocalCurrency = m.Sum(d => d.ProfitInLocalCurrency),
                                                                               totalProfitInProfitCurrency = m.Sum(d => d.ProfitInProfitCurrency),
                                                                           }).ToList();



                                othersResultList = (from a in allCountriesResult
                                                    where !(from r in resultList where r.countryCode == a.countryCode select r).Any()
                                                    select a).ToList();
                                foreach (DashBoardClass d in othersResultList)
                                {
                                    d.countryCode = "Others";
                                    d.countryName = "Others";
                                    d.country = "Others";
                                }
                                unionList = resultList.Union(othersResultList).OrderByDescending(d => d.ReceivablesInProfitCurrency).ToList();

                            }
                            else
                            {
                                unionList = resultList.OrderByDescending(d => d.ReceivablesInProfitCurrency).ToList();
                            }
                        }

                        break;
                    }
                    #endregion


            }

            return unionList;
        }

        /* DashBoard Top 10 Customers */
        public IQueryable<DashBoardClass> GetTop10DashBoard(string type, int lastMonths, int lastDays, int measurment, int currentTenant, int top, bool includeOthers, string directionId, string transportmodeId)
        {
            if (top < 0)
            {
                top = 0;
            }

            int days = 0;
            int months = 0;
            days = lastDays;
            DateTime lastDate = DateTime.Today.Date.AddDays(days);

            if (lastMonths != 0)
            {
                months = lastMonths + 1;
                lastDate = DateTime.Today.Date.AddMonths(months);
            }

            IQueryable<DashBoardClass> resulList = null;

            IQueryable<ShipmentsCustomersDashboardView> allShipments = repository.GetShipmentDataViewsForCustomersDashboard(currentTenant, directionId, transportmodeId);

            allShipments = BranchPermitionsFilter.AddUserBranchRestrictionFilters<ShipmentsCustomersDashboardView>(new QueryOperations(), allShipments, currentTenant);
            allShipments = ProductPermitionsFilter.AddUserProductRestrictionFilters<ShipmentsCustomersDashboardView>(new QueryOperations(), allShipments, currentTenant);
            DateTime FromDateQuery = TenantServerConfigration.GetCurrentDateTime(currentTenant);
            IQueryable<DashBoardClass> resultList1 = null;
            IQueryable<DashBoardClass> resultList2 = null;

            switch (measurment)
            {
                #region measurement is shipment count
                case 0:
                    {
                        List<string> topCustomerIds;
                        if (type == "CreateDate" || type == null)
                        {
                            topCustomerIds = (from t in allShipments
                                              where t.CreateDateTime > lastDate
                                              group t by t.CustomerId into g
                                              select new
                                              {
                                                  id = g.Key,
                                                  c = g.Count(),
                                              }).OrderByDescending(r => r.c).Select(a => a.id).Skip(0).Take(top).ToList();

                            resultList1 = (from s in allShipments
                                           where s.CreateDateTime > lastDate && topCustomerIds.Contains(s.CustomerId)

                                           group s by new
                                           {
                                               s.CustomerId,
                                               s.CustomerName,
                                               s.DirectionId,
                                               s.TransportModeId,
                                           } into c

                                           select new DashBoardClass()
                                           {
                                               CustomerID = c.Key.CustomerId,
                                               CustomerName = c.Key.CustomerName,
                                               directionID = c.Key.DirectionId,
                                               transportModeID = c.Key.TransportModeId,
                                               total = c.Count(),
                                               sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                               sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                           });
                        }
                        else
                        {
                            topCustomerIds = (from t in allShipments
                                              where t.OperationalDate > lastDate && t.OperationalDate <= FromDateQuery
                                              group t by t.CustomerId into g
                                              select new
                                              {
                                                  id = g.Key,
                                                  c = g.Count(),
                                              }).OrderByDescending(r => r.c).Select(a => a.id).Skip(0).Take(top).ToList();

                            resultList1 = (from s in allShipments
                                           where s.OperationalDate > lastDate && s.OperationalDate <= FromDateQuery && topCustomerIds.Contains(s.CustomerId)
                                           group s by new
                                           {
                                               s.CustomerId,
                                               s.CustomerName,
                                               s.DirectionId,
                                               s.TransportModeId,
                                           } into c

                                           select new DashBoardClass()
                                           {
                                               CustomerID = c.Key.CustomerId,
                                               CustomerName = c.Key.CustomerName,
                                               directionID = c.Key.DirectionId,
                                               transportModeID = c.Key.TransportModeId,
                                               total = c.Count(),
                                               sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                               sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                           });
                        }


                        if (includeOthers)
                        {
                            if (type == "CreateDate" || type == null)
                            {
                                resultList2 = (from s in allShipments
                                               where s.CreateDateTime > lastDate && !topCustomerIds.Contains(s.CustomerId)
                                               group s by new
                                               {
                                                   s.DirectionId,
                                                   s.TransportModeId,
                                               } into c

                                               select new DashBoardClass()
                                               {
                                                   CustomerID = "Others" + c.Key.DirectionId + c.Key.TransportModeId,
                                                   CustomerName = "Others",
                                                   directionID = c.Key.DirectionId,
                                                   transportModeID = c.Key.TransportModeId,
                                                   total = c.Count(),
                                                   sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                                   sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                               });
                            }
                            else
                            {
                                resultList2 = (from s in allShipments
                                               where s.OperationalDate > lastDate && s.OperationalDate <= FromDateQuery && !topCustomerIds.Contains(s.CustomerId)
                                               group s by new
                                               {
                                                   s.DirectionId,
                                                   s.TransportModeId,
                                               } into c

                                               select new DashBoardClass()
                                               {
                                                   CustomerID = "Others" + c.Key.DirectionId + c.Key.TransportModeId,
                                                   CustomerName = "Others",
                                                   directionID = c.Key.DirectionId,
                                                   transportModeID = c.Key.TransportModeId,
                                                   total = c.Count(),
                                                   sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                                   sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                               });


                            }

                            resulList = resultList1.Concat(resultList2).OrderByDescending(d => d.total);
                        }
                        else
                        {
                            resulList = resultList1.OrderByDescending(d => d.total);
                        }




                        break;
                    }
                #endregion

                #region measurement is chargeable weight
                case 1:
                    {

                        if (type == "CreateDate" || type == null)
                        {
                            var top10 = (from t in allShipments
                                         where t.CreateDateTime > lastDate
                                         group t by t.CustomerId into g
                                         select new
                                         {
                                             id = g.Key,
                                             c = g.Sum(d => d.ChargeableWeightInKG),
                                         }).OrderByDescending(r => r.c).Skip(0).Take(top);

                            resultList1 = (from s in allShipments
                                           where s.CreateDateTime > lastDate
                                           && top10.Any(q => q.id == s.CustomerId)
                                           group s by new
                                           {
                                               s.CustomerId,
                                               s.CustomerName,
                                               s.DirectionId,
                                               s.TransportModeId,
                                           } into c
                                           select new DashBoardClass()
                                           {
                                               CustomerID = c.Key.CustomerId,
                                               CustomerName = c.Key.CustomerName,
                                               directionID = c.Key.DirectionId,
                                               transportModeID = c.Key.TransportModeId,
                                               total = c.Count(),
                                               sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                               sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                           });
                            if (includeOthers)
                            {
                                resultList2 = (from s in allShipments
                                               where s.CreateDateTime > lastDate
                                               && !top10.Any(q => q.id == s.CustomerId)
                                               group s by new
                                               {

                                                   s.DirectionId,
                                                   s.TransportModeId,
                                               } into c

                                               select new DashBoardClass()
                                               {
                                                   CustomerID = "Others" + c.Key.DirectionId + c.Key.TransportModeId,
                                                   CustomerName = "Others",
                                                   directionID = c.Key.DirectionId,
                                                   transportModeID = c.Key.TransportModeId,
                                                   total = c.Count(),
                                                   sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                                   sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                               });

                                resulList = resultList1.Concat(resultList2).OrderByDescending(d => d.sumChargeableWeight);
                            }
                            else
                            {
                                resulList = resultList1.OrderByDescending(d => d.sumChargeableWeight);
                            }
                        }
                        else
                        {
                            var top10 = (from t in allShipments
                                         where t.OperationalDate > lastDate
                                         group t by t.CustomerId into g
                                         select new
                                         {
                                             id = g.Key,
                                             c = g.Sum(d => d.ChargeableWeightInKG),
                                         }).OrderByDescending(r => r.c).Skip(0).Take(top);

                            resultList1 = (from s in allShipments
                                           where s.OperationalDate > lastDate
                                           && top10.Any(q => q.id == s.CustomerId)
                                           group s by new
                                           {
                                               s.CustomerId,
                                               s.CustomerName,
                                               s.DirectionId,
                                               s.TransportModeId,
                                           } into c
                                           select new DashBoardClass()
                                           {
                                               CustomerID = c.Key.CustomerId,
                                               CustomerName = c.Key.CustomerName,
                                               directionID = c.Key.DirectionId,
                                               transportModeID = c.Key.TransportModeId,
                                               total = c.Count(),
                                               sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                               sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                           });

                            if (includeOthers)
                            {
                                resultList2 = (from s in allShipments
                                               where s.OperationalDate > lastDate && s.OperationalDate <= FromDateQuery
                                               && !top10.Any(q => q.id == s.CustomerId)
                                               group s by new
                                               {

                                                   s.DirectionId,
                                                   s.TransportModeId,
                                               } into c

                                               select new DashBoardClass()
                                               {
                                                   CustomerID = "Others" + c.Key.DirectionId + c.Key.TransportModeId,
                                                   CustomerName = "Others",
                                                   directionID = c.Key.DirectionId,
                                                   transportModeID = c.Key.TransportModeId,
                                                   total = c.Count(),
                                                   sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                                   sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                               });

                                resulList = resultList1.Concat(resultList2).OrderByDescending(d => d.sumChargeableWeight);
                            }
                            else
                            {
                                resulList = resultList1.OrderByDescending(d => d.sumChargeableWeight);
                            }

                        }


                        break;
                    }
                #endregion

                #region measurement is Gross weight
                case 2:
                    {

                        if (type == "CreateDate" || type == null)
                        {
                            var top10 = (from t in allShipments
                                         where t.CreateDateTime > lastDate
                                         group t by t.CustomerId into g
                                         select new
                                         {
                                             id = g.Key,
                                             c = g.Sum(d => d.GrossWeightInKG),
                                         }).OrderByDescending(r => r.c).Skip(0).Take(top);

                            resultList1 = (from s in allShipments
                                           where s.CreateDateTime > lastDate
                                           && top10.Any(q => q.id == s.CustomerId)
                                           group s by new
                                           {
                                               s.CustomerId,
                                               s.CustomerName,
                                               s.DirectionId,
                                               s.TransportModeId,
                                           } into c

                                           select new DashBoardClass()
                                           {
                                               CustomerID = c.Key.CustomerId,
                                               CustomerName = c.Key.CustomerName,
                                               directionID = c.Key.DirectionId,
                                               transportModeID = c.Key.TransportModeId,
                                               total = c.Count(),
                                               sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                               sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                           });


                            if (includeOthers)
                            {
                                resultList2 = (from s in allShipments
                                               where s.CreateDateTime > lastDate
                                               && !top10.Any(q => q.id == s.CustomerId)
                                               group s by new
                                               {

                                                   s.DirectionId,
                                                   s.TransportModeId,
                                               } into c
                                               select new DashBoardClass()
                                               {
                                                   CustomerID = "Others" + c.Key.DirectionId + c.Key.TransportModeId,
                                                   CustomerName = "Others",
                                                   directionID = c.Key.DirectionId,
                                                   transportModeID = c.Key.TransportModeId,
                                                   total = c.Count(),
                                                   sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                                   sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                               });

                                resulList = resultList1.Concat(resultList2).OrderByDescending(d => d.sumGrossWeight);
                            }
                            else
                            {
                                resulList = resultList1.OrderByDescending(d => d.sumGrossWeight);
                            }
                        }

                        else
                        {
                            var top10 = (from t in allShipments
                                         where t.OperationalDate > lastDate
                                         group t by t.CustomerId into g
                                         select new
                                         {
                                             id = g.Key,
                                             c = g.Sum(d => d.GrossWeightInKG),
                                         }).OrderByDescending(r => r.c).Skip(0).Take(top);

                            resultList1 = (from s in allShipments
                                           where s.OperationalDate > lastDate && s.OperationalDate <= FromDateQuery
                                           && top10.Any(q => q.id == s.CustomerId)
                                           group s by new
                                           {
                                               s.CustomerId,
                                               s.CustomerName,
                                               s.DirectionId,
                                               s.TransportModeId,
                                           } into c

                                           select new DashBoardClass()
                                           {
                                               CustomerID = c.Key.CustomerId,
                                               CustomerName = c.Key.CustomerName,
                                               directionID = c.Key.DirectionId,
                                               transportModeID = c.Key.TransportModeId,
                                               total = c.Count(),
                                               sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                               sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                           });
                            if (includeOthers)
                            {
                                resultList2 = (from s in allShipments
                                               where s.OperationalDate > lastDate && s.OperationalDate <= FromDateQuery
                                               && !top10.Any(q => q.id == s.CustomerId)
                                               group s by new
                                               {
                                                   s.DirectionId,
                                                   s.TransportModeId,
                                               } into c
                                               select new DashBoardClass()
                                               {
                                                   CustomerID = "Others" + c.Key.DirectionId + c.Key.TransportModeId,
                                                   CustomerName = "Others",
                                                   directionID = c.Key.DirectionId,
                                                   transportModeID = c.Key.TransportModeId,
                                                   total = c.Count(),
                                                   sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                                   sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                               });

                                resulList = resultList1.Concat(resultList2).OrderByDescending(d => d.sumGrossWeight);
                            }
                            else
                            {
                                resulList = resultList1.OrderByDescending(d => d.sumGrossWeight);
                            }

                        }


                        break;
                    }
                #endregion

                #region measurement is profit in local
                case 3:
                    {
                        if (type == "CreateDate" || type == null)
                        {
                            var top10 = (from t in allShipments
                                         where t.CreateDateTime > lastDate
                                         group t by t.CustomerId into g
                                         select new
                                         {
                                             id = g.Key,
                                             c = g.Sum(d => d.ProfitInLocalCurrency),
                                         }).OrderByDescending(r => r.c).Skip(0).Take(top);

                            resultList1 = (from s in allShipments
                                           where s.CreateDateTime > lastDate
                                           && top10.Any(q => q.id == s.CustomerId)
                                           group s by new
                                           {
                                               s.CustomerId,
                                               s.CustomerName,
                                               s.DirectionId,
                                               s.TransportModeId,
                                           } into c
                                           select new DashBoardClass()
                                           {
                                               CustomerID = c.Key.CustomerId,
                                               CustomerName = c.Key.CustomerName,
                                               directionID = c.Key.DirectionId,
                                               transportModeID = c.Key.TransportModeId,
                                               total = c.Count(),
                                               sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                               sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                               totalProfitInLocalCurrency = c.Sum(s => s.ProfitInLocalCurrency),
                                               totalProfitInProfitCurrency = c.Sum(s => s.ProfitInProfitCurrency),
                                           });
                            if (includeOthers)
                            {
                                resultList2 = (from s in allShipments
                                               where s.CreateDateTime > lastDate
                                               && !top10.Any(q => q.id == s.CustomerId)
                                               group s by new
                                               {
                                                   s.DirectionId,
                                                   s.TransportModeId,
                                               } into c

                                               select new DashBoardClass()
                                               {
                                                   CustomerID = "Others" + c.Key.DirectionId + c.Key.TransportModeId,
                                                   CustomerName = "Others",
                                                   directionID = c.Key.DirectionId,
                                                   transportModeID = c.Key.TransportModeId,
                                                   total = c.Count(),
                                                   sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                                   sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                                   totalProfitInLocalCurrency = c.Sum(s => s.ProfitInLocalCurrency),
                                                   totalProfitInProfitCurrency = c.Sum(s => s.ProfitInProfitCurrency),
                                               });

                                resulList = resultList1.Concat(resultList2).OrderByDescending(d => d.totalProfitInLocalCurrency);
                            }
                            else
                            {
                                resulList = resultList1.OrderByDescending(d => d.totalProfitInLocalCurrency);
                            }
                        }
                        else
                        {
                            var top10 = (from t in allShipments
                                         where t.OperationalDate > lastDate
                                         group t by t.CustomerId into g
                                         select new
                                         {
                                             id = g.Key,
                                             c = g.Sum(d => d.ProfitInLocalCurrency),
                                         }).OrderByDescending(r => r.c).Skip(0).Take(top);

                            resultList1 = (from s in allShipments
                                           where s.OperationalDate > lastDate && s.OperationalDate <= FromDateQuery
                                           && top10.Any(q => q.id == s.CustomerId)
                                           group s by new
                                           {
                                               s.CustomerId,
                                               s.CustomerName,
                                               s.DirectionId,
                                               s.TransportModeId,
                                           } into c

                                           select new DashBoardClass()
                                           {
                                               CustomerID = c.Key.CustomerId,
                                               CustomerName = c.Key.CustomerName,
                                               directionID = c.Key.DirectionId,
                                               transportModeID = c.Key.TransportModeId,
                                               total = c.Count(),
                                               sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                               sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                               totalProfitInLocalCurrency = c.Sum(s => s.ProfitInLocalCurrency),
                                               totalProfitInProfitCurrency = c.Sum(s => s.ProfitInProfitCurrency),
                                           });
                            if (includeOthers)
                            {
                                resultList2 = (from s in allShipments
                                               where s.OperationalDate > lastDate && s.OperationalDate <= FromDateQuery
                                               && !top10.Any(q => q.id == s.CustomerId)
                                               group s by new
                                               {
                                                   s.DirectionId,
                                                   s.TransportModeId,
                                               } into c

                                               select new DashBoardClass()
                                               {
                                                   CustomerID = "Others" + c.Key.DirectionId + c.Key.TransportModeId,
                                                   CustomerName = "Others",
                                                   directionID = c.Key.DirectionId,
                                                   transportModeID = c.Key.TransportModeId,
                                                   total = c.Count(),
                                                   sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                                   sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                                   totalProfitInLocalCurrency = c.Sum(s => s.ProfitInLocalCurrency),
                                                   totalProfitInProfitCurrency = c.Sum(s => s.ProfitInProfitCurrency),
                                               });

                                resulList = resultList1.Concat(resultList2).OrderByDescending(d => d.totalProfitInLocalCurrency);
                            }
                            else
                            {
                                resulList = resultList1.OrderByDescending(d => d.totalProfitInLocalCurrency);
                            }
                        }



                        break;
                    }
                #endregion

                #region measurement is profit in profit
                case 4:
                    {
                        if (type == "CreateDate" || type == null)
                        {
                            var top10 = (from t in allShipments
                                         where t.CreateDateTime > lastDate
                                         group t by t.CustomerId into g
                                         select new
                                         {
                                             id = g.Key,
                                             c = g.Sum(d => d.ProfitInProfitCurrency),
                                         }).OrderByDescending(r => r.c).Skip(0).Take(top);


                            resultList1 = (from s in allShipments
                                           where s.CreateDateTime > lastDate
                                           && top10.Any(q => q.id == s.CustomerId)
                                           group s by new
                                           {
                                               s.CustomerId,
                                               s.CustomerName,
                                               s.DirectionId,
                                               s.TransportModeId,
                                           } into c

                                           select new DashBoardClass()
                                           {
                                               CustomerID = c.Key.CustomerId,
                                               CustomerName = c.Key.CustomerName,
                                               directionID = c.Key.DirectionId,
                                               transportModeID = c.Key.TransportModeId,
                                               total = c.Count(),
                                               sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                               sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                               totalProfitInLocalCurrency = c.Sum(s => s.ProfitInLocalCurrency),
                                               totalProfitInProfitCurrency = c.Sum(s => s.ProfitInProfitCurrency),
                                           });
                            if (includeOthers)
                            {
                                resultList2 = (from s in allShipments
                                               where s.CreateDateTime > lastDate
                                               && !top10.Any(q => q.id == s.CustomerId)

                                               group s by new
                                               {

                                                   s.DirectionId,
                                                   s.TransportModeId,
                                               } into c

                                               select new DashBoardClass()
                                               {
                                                   CustomerID = "Others" + c.Key.DirectionId + c.Key.TransportModeId,
                                                   CustomerName = "Others",
                                                   directionID = c.Key.DirectionId,
                                                   transportModeID = c.Key.TransportModeId,
                                                   total = c.Count(),
                                                   sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                                   sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                                   totalProfitInLocalCurrency = c.Sum(s => s.ProfitInLocalCurrency),
                                                   totalProfitInProfitCurrency = c.Sum(s => s.ProfitInProfitCurrency),
                                               });

                                resulList = resultList1.Concat(resultList2).OrderByDescending(d => d.totalProfitInProfitCurrency);
                            }
                            else
                            {
                                resulList = resultList1.OrderByDescending(d => d.totalProfitInProfitCurrency);
                            }

                        }
                        else
                        {
                            var top10 = (from t in allShipments
                                         where t.OperationalDate > lastDate
                                         group t by t.CustomerId into g
                                         select new
                                         {
                                             id = g.Key,
                                             c = g.Sum(d => d.ProfitInProfitCurrency),
                                         }).OrderByDescending(r => r.c).Skip(0).Take(top);

                            resultList1 = (from s in allShipments
                                           where s.OperationalDate > lastDate && s.OperationalDate <= FromDateQuery
                                           && top10.Any(q => q.id == s.CustomerId)
                                           group s by new
                                           {
                                               s.CustomerId,
                                               s.CustomerName,
                                               s.DirectionId,
                                               s.TransportModeId,
                                           } into c

                                           select new DashBoardClass()
                                           {
                                               CustomerID = c.Key.CustomerId,
                                               CustomerName = c.Key.CustomerName,
                                               directionID = c.Key.DirectionId,
                                               transportModeID = c.Key.TransportModeId,
                                               total = c.Count(),
                                               sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                               sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                               totalProfitInLocalCurrency = c.Sum(s => s.ProfitInLocalCurrency),
                                               totalProfitInProfitCurrency = c.Sum(s => s.ProfitInProfitCurrency),
                                           });
                            if (includeOthers)
                            {
                                resultList2 = (from s in allShipments
                                               where s.OperationalDate > lastDate && s.OperationalDate <= FromDateQuery
                                               && !top10.Any(q => q.id == s.CustomerId)
                                               group s by new
                                               {
                                                   s.DirectionId,
                                                   s.TransportModeId,
                                               } into c

                                               select new DashBoardClass()
                                               {
                                                   CustomerID = "Others" + c.Key.DirectionId + c.Key.TransportModeId,
                                                   CustomerName = "Others",
                                                   directionID = c.Key.DirectionId,
                                                   transportModeID = c.Key.TransportModeId,
                                                   total = c.Count(),
                                                   sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                                   sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                                   totalProfitInLocalCurrency = c.Sum(s => s.ProfitInLocalCurrency),
                                                   totalProfitInProfitCurrency = c.Sum(s => s.ProfitInProfitCurrency),
                                               });

                                resulList = resultList1.Concat(resultList2).OrderByDescending(d => d.totalProfitInProfitCurrency);
                            }
                            else
                            {
                                resulList = resultList1.OrderByDescending(d => d.totalProfitInProfitCurrency);
                            }

                        }


                        break;
                    }
                #endregion

                #region measurement is receivable in local
                case 5:
                    {
                        if (type == "CreateDate" || type == null)
                        {
                            var top10 = (from t in allShipments
                                         where t.CreateDateTime > lastDate
                                         group t by t.CustomerId into g
                                         select new
                                         {
                                             id = g.Key,
                                             c = g.Sum(d => d.OpenReceivablesInLocalCurrency),
                                         }).OrderByDescending(r => r.c).Skip(0).Take(top);


                            resultList1 = (from s in allShipments
                                           where s.CreateDateTime > lastDate
                                           && top10.Any(q => q.id == s.CustomerId)
                                           group s by new
                                           {
                                               s.CustomerId,
                                               s.CustomerName,
                                               s.DirectionId,
                                               s.TransportModeId,
                                           } into c
                                           select new DashBoardClass()
                                           {
                                               CustomerID = c.Key.CustomerId,
                                               CustomerName = c.Key.CustomerName,
                                               directionID = c.Key.DirectionId,
                                               transportModeID = c.Key.TransportModeId,
                                               total = c.Count(),
                                               sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                               sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                               totalProfitInLocalCurrency = c.Sum(s => s.ProfitInLocalCurrency),
                                               totalProfitInProfitCurrency = c.Sum(s => s.ProfitInProfitCurrency),
                                               ReceivablesInLocalCurrency = c.Sum(s => s.OpenReceivablesInLocalCurrency),
                                               ReceivablesInProfitCurrency = c.Sum(s => s.OpenReceivablesInProfitCurrency),
                                           });

                            if (includeOthers)
                            {
                                resultList2 = (from s in allShipments
                                               where s.CreateDateTime > lastDate
                                               && !top10.Any(q => q.id == s.CustomerId)
                                               group s by new
                                               {
                                                   s.DirectionId,
                                                   s.TransportModeId,
                                               } into c

                                               select new DashBoardClass()
                                               {
                                                   CustomerID = "Others" + c.Key.DirectionId + c.Key.TransportModeId,
                                                   CustomerName = "Others",
                                                   directionID = c.Key.DirectionId,
                                                   transportModeID = c.Key.TransportModeId,
                                                   total = c.Count(),
                                                   sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                                   sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                                   totalProfitInLocalCurrency = c.Sum(s => s.ProfitInLocalCurrency),
                                                   totalProfitInProfitCurrency = c.Sum(s => s.ProfitInProfitCurrency),
                                                   ReceivablesInLocalCurrency = c.Sum(s => s.OpenReceivablesInLocalCurrency),
                                                   ReceivablesInProfitCurrency = c.Sum(s => s.OpenReceivablesInProfitCurrency),
                                               });

                                resulList = resultList1.Concat(resultList2).OrderByDescending(d => d.ReceivablesInLocalCurrency);
                            }
                            else
                            {
                                resulList = resultList1.OrderByDescending(d => d.ReceivablesInLocalCurrency);
                            }

                        }
                        else
                        {
                            var top10 = (from t in allShipments
                                         where t.OperationalDate > lastDate
                                         group t by t.CustomerId into g
                                         select new
                                         {
                                             id = g.Key,
                                             c = g.Sum(d => d.OpenReceivablesInLocalCurrency),
                                         }).OrderByDescending(r => r.c).Skip(0).Take(top);

                            resultList1 = (from s in allShipments
                                           where s.OperationalDate > lastDate && s.OperationalDate <= FromDateQuery
                                           && top10.Any(q => q.id == s.CustomerId)
                                           group s by new
                                           {
                                               s.CustomerId,
                                               s.CustomerName,
                                               s.DirectionId,
                                               s.TransportModeId,
                                           } into c

                                           select new DashBoardClass()
                                           {
                                               CustomerID = c.Key.CustomerId,
                                               CustomerName = c.Key.CustomerName,
                                               directionID = c.Key.DirectionId,
                                               transportModeID = c.Key.TransportModeId,
                                               total = c.Count(),
                                               sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                               sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                               totalProfitInLocalCurrency = c.Sum(s => s.ProfitInLocalCurrency),
                                               totalProfitInProfitCurrency = c.Sum(s => s.ProfitInProfitCurrency),
                                               ReceivablesInLocalCurrency = c.Sum(s => s.OpenReceivablesInLocalCurrency),
                                               ReceivablesInProfitCurrency = c.Sum(s => s.OpenReceivablesInProfitCurrency),
                                           });
                            if (includeOthers)
                            {
                                resultList2 = (from s in allShipments
                                               where s.OperationalDate > lastDate && s.OperationalDate <= FromDateQuery
                                               && !top10.Any(q => q.id == s.CustomerId)
                                               group s by new
                                               {
                                                   s.DirectionId,
                                                   s.TransportModeId,
                                               } into c

                                               select new DashBoardClass()
                                               {
                                                   CustomerID = "Others" + c.Key.DirectionId + c.Key.TransportModeId,
                                                   CustomerName = "Others",
                                                   directionID = c.Key.DirectionId,
                                                   transportModeID = c.Key.TransportModeId,
                                                   total = c.Count(),
                                                   sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                                   sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                                   totalProfitInLocalCurrency = c.Sum(s => s.ProfitInLocalCurrency),
                                                   totalProfitInProfitCurrency = c.Sum(s => s.ProfitInProfitCurrency),
                                                   ReceivablesInLocalCurrency = c.Sum(s => s.OpenReceivablesInLocalCurrency),
                                                   ReceivablesInProfitCurrency = c.Sum(s => s.OpenReceivablesInProfitCurrency),
                                               });

                                resulList = resultList1.Concat(resultList2).OrderByDescending(d => d.ReceivablesInLocalCurrency);
                            }
                            else
                            {
                                resulList = resultList1.OrderByDescending(d => d.ReceivablesInLocalCurrency);
                            }

                        }


                        break;
                    }
                #endregion

                #region measurement is receivable in profit
                case 6:
                    {
                        if (type == "CreateDate" || type == null)
                        {
                            var top10 = (from t in allShipments
                                         where t.CreateDateTime > lastDate
                                         group t by t.CustomerId into g
                                         select new
                                         {
                                             id = g.Key,
                                             c = g.Sum(d => d.OpenReceivablesInProfitCurrency),
                                         }).OrderByDescending(r => r.c).Skip(0).Take(top);



                            resultList1 = (from s in allShipments
                                           where s.CreateDateTime > lastDate
                                           && top10.Any(q => q.id == s.CustomerId)
                                           group s by new
                                           {
                                               s.CustomerId,
                                               s.CustomerName,
                                               s.DirectionId,
                                               s.TransportModeId,
                                           } into c

                                           select new DashBoardClass()
                                           {
                                               CustomerID = c.Key.CustomerId,
                                               CustomerName = c.Key.CustomerName,
                                               directionID = c.Key.DirectionId,
                                               transportModeID = c.Key.TransportModeId,
                                               total = c.Count(),
                                               sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                               sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                               totalProfitInLocalCurrency = c.Sum(s => s.ProfitInLocalCurrency),
                                               totalProfitInProfitCurrency = c.Sum(s => s.ProfitInProfitCurrency),
                                               ReceivablesInLocalCurrency = c.Sum(s => s.OpenReceivablesInLocalCurrency),
                                               ReceivablesInProfitCurrency = c.Sum(s => s.OpenReceivablesInProfitCurrency),
                                           });
                            if (includeOthers)
                            {
                                resultList2 = (from s in allShipments
                                               where s.CreateDateTime > lastDate
                                               && !top10.Any(q => q.id == s.CustomerId)
                                               group s by new
                                               {
                                                   s.DirectionId,
                                                   s.TransportModeId,
                                               } into c

                                               select new DashBoardClass()
                                               {
                                                   CustomerID = "Others" + c.Key.DirectionId + c.Key.TransportModeId,
                                                   CustomerName = "Others",
                                                   directionID = c.Key.DirectionId,
                                                   transportModeID = c.Key.TransportModeId,
                                                   total = c.Count(),
                                                   sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                                   sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                                   totalProfitInLocalCurrency = c.Sum(s => s.ProfitInLocalCurrency),
                                                   totalProfitInProfitCurrency = c.Sum(s => s.ProfitInProfitCurrency),
                                                   ReceivablesInLocalCurrency = c.Sum(s => s.OpenReceivablesInLocalCurrency),
                                                   ReceivablesInProfitCurrency = c.Sum(s => s.OpenReceivablesInProfitCurrency),
                                               });

                                resulList = resultList1.Concat(resultList2).OrderByDescending(d => d.ReceivablesInProfitCurrency);
                            }
                            else
                            {
                                resulList = resultList1.OrderByDescending(d => d.ReceivablesInProfitCurrency);
                            }
                        }
                        else
                        {
                            var top10 = (from t in allShipments
                                         where t.OperationalDate > lastDate
                                         group t by t.CustomerId into g
                                         select new
                                         {
                                             id = g.Key,
                                             c = g.Sum(d => d.OpenReceivablesInProfitCurrency),
                                         }).OrderByDescending(r => r.c).Skip(0).Take(top);


                            resultList1 = (from s in allShipments
                                           where s.OperationalDate > lastDate && s.OperationalDate <= FromDateQuery
                                           && top10.Any(q => q.id == s.CustomerId)
                                           group s by new
                                           {
                                               s.CustomerId,
                                               s.CustomerName,
                                               s.DirectionId,
                                               s.TransportModeId,
                                           } into c

                                           select new DashBoardClass()
                                           {
                                               CustomerID = c.Key.CustomerId,
                                               CustomerName = c.Key.CustomerName,
                                               directionID = c.Key.DirectionId,
                                               transportModeID = c.Key.TransportModeId,
                                               total = c.Count(),
                                               sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                               sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                               totalProfitInLocalCurrency = c.Sum(s => s.ProfitInLocalCurrency),
                                               totalProfitInProfitCurrency = c.Sum(s => s.ProfitInProfitCurrency),
                                               ReceivablesInLocalCurrency = c.Sum(s => s.OpenReceivablesInLocalCurrency),
                                               ReceivablesInProfitCurrency = c.Sum(s => s.OpenReceivablesInProfitCurrency),
                                           });
                            if (includeOthers)
                            {
                                resultList2 = (from s in allShipments
                                               where s.OperationalDate > lastDate && s.OperationalDate <= FromDateQuery
                                               && !top10.Any(q => q.id == s.CustomerId)
                                               group s by new
                                               {
                                                   s.DirectionId,
                                                   s.TransportModeId,
                                               } into c

                                               select new DashBoardClass()
                                               {
                                                   CustomerID = "Others" + c.Key.DirectionId + c.Key.TransportModeId,
                                                   CustomerName = "Others",
                                                   directionID = c.Key.DirectionId,
                                                   transportModeID = c.Key.TransportModeId,
                                                   total = c.Count(),
                                                   sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                                   sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                                   totalProfitInLocalCurrency = c.Sum(s => s.ProfitInLocalCurrency),
                                                   totalProfitInProfitCurrency = c.Sum(s => s.ProfitInProfitCurrency),
                                                   ReceivablesInLocalCurrency = c.Sum(s => s.OpenReceivablesInLocalCurrency),
                                                   ReceivablesInProfitCurrency = c.Sum(s => s.OpenReceivablesInProfitCurrency),
                                               });

                                resulList = resultList1.Concat(resultList2).OrderByDescending(d => d.ReceivablesInProfitCurrency);
                            }
                            else
                            {
                                resulList = resultList1.OrderByDescending(d => d.ReceivablesInProfitCurrency);
                            }
                        }



                        break;
                    }
                #endregion

                default: break;
            }

            return resulList;
        }
        public IQueryable<DashBoardClass> GetTop10DashBoardCustom(string type, DateTime? FromDate, DateTime? ToDate, int measurment, int currentTenant, int top, bool includeOthers, string directionId, string transportmodeId)
        {
            if (top < 0)
            {
                top = 0;
            }
            FromDate = new DateTime(FromDate.Value.Year, FromDate.Value.Month, FromDate.Value.Day, 0, 0, 0);
            ToDate = ToDate.Value.AddDays(1);

            IQueryable<DashBoardClass> resulList = null;

            IQueryable<ShipmentsCustomersDashboardView> allShipments = repository.GetShipmentDataViewsForCustomersDashboard(currentTenant, directionId, transportmodeId);

            allShipments = BranchPermitionsFilter.AddUserBranchRestrictionFilters<ShipmentsCustomersDashboardView>(new QueryOperations(), allShipments, currentTenant);
            allShipments = ProductPermitionsFilter.AddUserProductRestrictionFilters<ShipmentsCustomersDashboardView>(new QueryOperations(), allShipments, currentTenant);
            IQueryable<DashBoardClass> resultList1 = null;
            IQueryable<DashBoardClass> resultList2 = null;

            switch (measurment)
            {
                #region measurement is shipment count
                case 0:
                    {
                        if (type == "CreateDate" || type == null)
                        {
                            var top10 = (from t in allShipments
                                         where t.CreateDateTime > FromDate && t.CreateDateTime < ToDate
                                         group t by t.CustomerId into g
                                         select new
                                         {
                                             id = g.Key,
                                             c = g.Count(),
                                         }).OrderByDescending(r => r.c);

                            List<string> topCustomerIds = (from a in top10
                                                           select a.id).Skip(0).Take(top).ToList();

                            resultList1 = (from s in allShipments
                                           where s.CreateDateTime > FromDate && s.CreateDateTime < ToDate && topCustomerIds.Contains(s.CustomerId)

                                           group s by new
                                           {
                                               s.CustomerId,
                                               s.CustomerName,
                                               s.DirectionId,
                                               s.TransportModeId,
                                           } into c

                                           select new DashBoardClass()
                                           {
                                               CustomerID = c.Key.CustomerId,
                                               CustomerName = c.Key.CustomerName,
                                               directionID = c.Key.DirectionId,
                                               transportModeID = c.Key.TransportModeId,
                                               total = c.Count(),
                                               sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                               sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                           });
                            if (includeOthers)
                            {
                                resultList2 = (from s in allShipments
                                               where s.CreateDateTime > FromDate && s.CreateDateTime < ToDate && !topCustomerIds.Contains(s.CustomerId)
                                               group s by new
                                               {
                                                   s.DirectionId,
                                                   s.TransportModeId,
                                               } into c

                                               select new DashBoardClass()
                                               {
                                                   CustomerID = "Others" + c.Key.DirectionId + c.Key.TransportModeId,
                                                   CustomerName = "Others",
                                                   directionID = c.Key.DirectionId,
                                                   transportModeID = c.Key.TransportModeId,
                                                   total = c.Count(),
                                                   sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                                   sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                               });
                                resulList = resultList1.Concat(resultList2).OrderByDescending(d => d.total);
                            }
                            else
                            {
                                resulList = resultList1.OrderByDescending(d => d.total);
                            }
                        }
                        else
                        {
                            var top10 = (from t in allShipments
                                         where t.CreateDateTime > FromDate && t.CreateDateTime < ToDate
                                         group t by t.CustomerId into g
                                         select new
                                         {
                                             id = g.Key,
                                             c = g.Count(),
                                         }).OrderByDescending(r => r.c);

                            List<string> topCustomerIds = (from a in top10
                                                           select a.id).Skip(0).Take(top).ToList();

                            resultList1 = (from s in allShipments
                                           where s.OperationalDate > FromDate && s.OperationalDate < ToDate && topCustomerIds.Contains(s.CustomerId)
                                           group s by new
                                           {
                                               s.CustomerId,
                                               s.CustomerName,
                                               s.DirectionId,
                                               s.TransportModeId,
                                           } into c

                                           select new DashBoardClass()
                                           {
                                               CustomerID = c.Key.CustomerId,
                                               CustomerName = c.Key.CustomerName,
                                               directionID = c.Key.DirectionId,
                                               transportModeID = c.Key.TransportModeId,
                                               total = c.Count(),
                                               sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                               sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                           });
                            if (includeOthers)
                            {
                                resultList2 = (from s in allShipments
                                               where s.OperationalDate > FromDate && s.OperationalDate < ToDate && !topCustomerIds.Contains(s.CustomerId)
                                               group s by new
                                               {

                                                   s.DirectionId,
                                                   s.TransportModeId,
                                               } into c

                                               select new DashBoardClass()
                                               {
                                                   CustomerID = "Others" + c.Key.DirectionId + c.Key.TransportModeId,
                                                   CustomerName = "Others",
                                                   directionID = c.Key.DirectionId,
                                                   transportModeID = c.Key.TransportModeId,
                                                   total = c.Count(),
                                                   sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                                   sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                               });
                                resulList = resultList1.Concat(resultList2).OrderByDescending(d => d.total);
                            }
                            else
                            {
                                resulList = resultList1.OrderByDescending(d => d.total);
                            }
                        }






                        break;
                    }
                #endregion

                #region measurement is chargeable weight
                case 1:
                    {
                        if (type == "CreateDate" || type == null)
                        {
                            var top10 = (from t in allShipments
                                         where t.CreateDateTime > FromDate && t.CreateDateTime < ToDate
                                         group t by t.CustomerId into g
                                         select new
                                         {
                                             id = g.Key,
                                             c = g.Sum(d => d.ChargeableWeightInKG),
                                         }).OrderByDescending(r => r.c).Skip(0).Take(top);

                            resultList1 = (from s in allShipments
                                           where s.CreateDateTime > FromDate && s.CreateDateTime < ToDate
                                           && top10.Any(q => q.id == s.CustomerId)
                                           group s by new
                                           {
                                               s.CustomerId,
                                               s.CustomerName,
                                               s.DirectionId,
                                               s.TransportModeId,
                                           } into c

                                           select new DashBoardClass()
                                           {
                                               CustomerID = c.Key.CustomerId,
                                               CustomerName = c.Key.CustomerName,
                                               directionID = c.Key.DirectionId,
                                               transportModeID = c.Key.TransportModeId,
                                               total = c.Count(),
                                               sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                               sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                           });
                            if (includeOthers)
                            {
                                resultList2 = (from s in allShipments
                                               where s.CreateDateTime > FromDate && s.CreateDateTime < ToDate
                                               && !top10.Any(q => q.id == s.CustomerId)
                                               group s by new
                                               {
                                                   s.DirectionId,
                                                   s.TransportModeId,
                                               } into c

                                               select new DashBoardClass()
                                               {
                                                   CustomerID = "Others" + c.Key.DirectionId + c.Key.TransportModeId,
                                                   CustomerName = "Others",
                                                   directionID = c.Key.DirectionId,
                                                   transportModeID = c.Key.TransportModeId,
                                                   total = c.Count(),
                                                   sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                                   sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                               });

                                resulList = resultList1.Concat(resultList2).OrderByDescending(d => d.sumChargeableWeight);
                            }
                            else
                            {
                                resulList = resultList1.OrderByDescending(d => d.sumChargeableWeight);
                            }
                        }
                        else
                        {
                            var top10 = (from t in allShipments
                                         where t.OperationalDate > FromDate && t.OperationalDate < ToDate
                                         group t by t.CustomerId into g
                                         select new
                                         {
                                             id = g.Key,
                                             c = g.Sum(d => d.ChargeableWeightInKG),
                                         }).OrderByDescending(r => r.c).Skip(0).Take(top);

                            resultList1 = (from s in allShipments
                                           where s.OperationalDate > FromDate && s.OperationalDate < ToDate
                                           && top10.Any(q => q.id == s.CustomerId)
                                           group s by new
                                           {
                                               s.CustomerId,
                                               s.CustomerName,
                                               s.DirectionId,
                                               s.TransportModeId,
                                           } into c

                                           select new DashBoardClass()
                                           {
                                               CustomerID = c.Key.CustomerId,
                                               CustomerName = c.Key.CustomerName,
                                               directionID = c.Key.DirectionId,
                                               transportModeID = c.Key.TransportModeId,
                                               total = c.Count(),
                                               sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                               sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                           });
                            if (includeOthers)
                            {
                                resultList2 = (from s in allShipments
                                               where s.OperationalDate > FromDate && s.OperationalDate < ToDate
                                               && !top10.Any(q => q.id == s.CustomerId)
                                               group s by new
                                               {
                                                   s.DirectionId,
                                                   s.TransportModeId,
                                               } into c

                                               select new DashBoardClass()
                                               {
                                                   CustomerID = "Others" + c.Key.DirectionId + c.Key.TransportModeId,
                                                   CustomerName = "Others",
                                                   directionID = c.Key.DirectionId,
                                                   transportModeID = c.Key.TransportModeId,
                                                   total = c.Count(),
                                                   sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                                   sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                               });

                                resulList = resultList1.Concat(resultList2).OrderByDescending(d => d.sumChargeableWeight);
                            }
                            else
                            {
                                resulList = resultList1.OrderByDescending(d => d.sumChargeableWeight);
                            }

                        }


                        break;
                    }
                #endregion

                #region measurement is Gross weight
                case 2:
                    {
                        if (type == "CreateDate" || type == null)
                        {
                            var top10 = (from t in allShipments
                                         where t.CreateDateTime > FromDate && t.CreateDateTime < ToDate
                                         group t by t.CustomerId into g
                                         select new
                                         {
                                             id = g.Key,
                                             c = g.Sum(d => d.GrossWeightInKG),
                                         }).OrderByDescending(r => r.c).Skip(0).Take(top);

                            resultList1 = (from s in allShipments
                                           where s.CreateDateTime > FromDate && s.CreateDateTime < ToDate
                                           && top10.Any(q => q.id == s.CustomerId)
                                           group s by new
                                           {
                                               s.CustomerId,
                                               s.CustomerName,
                                               s.DirectionId,
                                               s.TransportModeId,
                                           } into c

                                           select new DashBoardClass()
                                           {
                                               CustomerID = c.Key.CustomerId,
                                               CustomerName = c.Key.CustomerName,
                                               directionID = c.Key.DirectionId,
                                               transportModeID = c.Key.TransportModeId,
                                               total = c.Count(),
                                               sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                               sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                           });
                            if (includeOthers)
                            {
                                resultList2 = (from s in allShipments
                                               where s.CreateDateTime > FromDate && s.CreateDateTime < ToDate
                                               && !top10.Any(q => q.id == s.CustomerId)
                                               group s by new
                                               {
                                                   s.DirectionId,
                                                   s.TransportModeId,
                                               } into c

                                               select new DashBoardClass()
                                               {
                                                   CustomerID = "Others" + c.Key.DirectionId + c.Key.TransportModeId,
                                                   CustomerName = "Others",
                                                   directionID = c.Key.DirectionId,
                                                   transportModeID = c.Key.TransportModeId,
                                                   total = c.Count(),
                                                   sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                                   sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                               });

                                resulList = resultList1.Concat(resultList2).OrderByDescending(d => d.sumGrossWeight);
                            }
                            else
                            {
                                resulList = resultList1.OrderByDescending(d => d.sumGrossWeight);
                            }

                        }
                        else
                        {
                            var top10 = (from t in allShipments
                                         where t.OperationalDate > FromDate && t.OperationalDate < ToDate
                                         group t by t.CustomerId into g
                                         select new
                                         {
                                             id = g.Key,
                                             c = g.Sum(d => d.GrossWeightInKG),
                                         }).OrderByDescending(r => r.c).Skip(0).Take(top);

                            resultList1 = (from s in allShipments
                                           where s.OperationalDate > FromDate && s.OperationalDate < ToDate
                                           && top10.Any(q => q.id == s.CustomerId)
                                           group s by new
                                           {
                                               s.CustomerId,
                                               s.CustomerName,
                                               s.DirectionId,
                                               s.TransportModeId,
                                           } into c

                                           select new DashBoardClass()
                                           {
                                               CustomerID = c.Key.CustomerId,
                                               CustomerName = c.Key.CustomerName,
                                               directionID = c.Key.DirectionId,
                                               transportModeID = c.Key.TransportModeId,
                                               total = c.Count(),
                                               sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                               sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                           });

                            if (includeOthers)
                            {
                                resultList2 = (from s in allShipments
                                               where s.OperationalDate > FromDate && s.OperationalDate < ToDate
                                               && !top10.Any(q => q.id == s.CustomerId)
                                               group s by new
                                               {
                                                   s.DirectionId,
                                                   s.TransportModeId,
                                               } into c

                                               select new DashBoardClass()
                                               {
                                                   CustomerID = "Others" + c.Key.DirectionId + c.Key.TransportModeId,
                                                   CustomerName = "Others",
                                                   directionID = c.Key.DirectionId,
                                                   transportModeID = c.Key.TransportModeId,
                                                   total = c.Count(),
                                                   sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                                   sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                               });

                                resulList = resultList1.Concat(resultList2).OrderByDescending(d => d.sumGrossWeight);
                            }
                            else
                            {
                                resulList = resultList1.OrderByDescending(d => d.sumGrossWeight);
                            }

                        }


                        break;
                    }
                #endregion

                #region measurement is profit in local
                case 3:
                    {
                        if (type == "CreateDate" || type == null)
                        {
                            var top10 = (from t in allShipments
                                         where t.CreateDateTime > FromDate && t.CreateDateTime < ToDate
                                         group t by t.CustomerId into g
                                         select new
                                         {
                                             id = g.Key,
                                             c = g.Sum(d => d.ProfitInLocalCurrency),
                                         }).OrderByDescending(r => r.c).Skip(0).Take(top);


                            resultList1 = (from s in allShipments
                                           where s.CreateDateTime > FromDate && s.CreateDateTime < ToDate
                                           && top10.Any(q => q.id == s.CustomerId)
                                           group s by new
                                           {
                                               s.CustomerId,
                                               s.CustomerName,
                                               s.DirectionId,
                                               s.TransportModeId,
                                           } into c

                                           select new DashBoardClass()
                                           {
                                               CustomerID = c.Key.CustomerId,
                                               CustomerName = c.Key.CustomerName,
                                               directionID = c.Key.DirectionId,
                                               transportModeID = c.Key.TransportModeId,
                                               total = c.Count(),
                                               sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                               sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                               totalProfitInLocalCurrency = c.Sum(s => s.ProfitInLocalCurrency),
                                               totalProfitInProfitCurrency = c.Sum(s => s.ProfitInProfitCurrency),
                                           });
                            if (includeOthers)
                            {
                                resultList2 = (from s in allShipments
                                               where s.CreateDateTime > FromDate && s.CreateDateTime < ToDate
                                               && !top10.Any(q => q.id == s.CustomerId)
                                               group s by new
                                               {
                                                   s.DirectionId,
                                                   s.TransportModeId,
                                               } into c

                                               select new DashBoardClass()
                                               {
                                                   CustomerID = "Others" + c.Key.DirectionId + c.Key.TransportModeId,
                                                   CustomerName = "Others",
                                                   directionID = c.Key.DirectionId,
                                                   transportModeID = c.Key.TransportModeId,
                                                   total = c.Count(),
                                                   sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                                   sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                                   totalProfitInLocalCurrency = c.Sum(s => s.ProfitInLocalCurrency),
                                                   totalProfitInProfitCurrency = c.Sum(s => s.ProfitInProfitCurrency),
                                               });

                                resulList = resultList1.Concat(resultList2).OrderByDescending(d => d.totalProfitInLocalCurrency);
                            }
                            else
                            {
                                resulList = resultList1.OrderByDescending(d => d.totalProfitInLocalCurrency);
                            }

                        }
                        else
                        {
                            var top10 = (from t in allShipments
                                         where t.OperationalDate > FromDate && t.OperationalDate < ToDate
                                         group t by t.CustomerId into g
                                         select new
                                         {
                                             id = g.Key,
                                             c = g.Sum(d => d.ProfitInLocalCurrency),
                                         }).OrderByDescending(r => r.c).Skip(0).Take(top);


                            resultList1 = (from s in allShipments
                                           where s.OperationalDate > FromDate && s.OperationalDate < ToDate
                                           && top10.Any(q => q.id == s.CustomerId)
                                           group s by new
                                           {
                                               s.CustomerId,
                                               s.CustomerName,
                                               s.DirectionId,
                                               s.TransportModeId,
                                           } into c

                                           select new DashBoardClass()
                                           {
                                               CustomerID = c.Key.CustomerId,
                                               CustomerName = c.Key.CustomerName,
                                               directionID = c.Key.DirectionId,
                                               transportModeID = c.Key.TransportModeId,
                                               total = c.Count(),
                                               sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                               sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                               totalProfitInLocalCurrency = c.Sum(s => s.ProfitInLocalCurrency),
                                               totalProfitInProfitCurrency = c.Sum(s => s.ProfitInProfitCurrency),
                                           });
                            if (includeOthers)
                            {
                                resultList2 = (from s in allShipments
                                               where s.OperationalDate > FromDate && s.OperationalDate < ToDate
                                               && !top10.Any(q => q.id == s.CustomerId)
                                               group s by new
                                               {
                                                   s.DirectionId,
                                                   s.TransportModeId,
                                               } into c

                                               select new DashBoardClass()
                                               {
                                                   CustomerID = "Others" + c.Key.DirectionId + c.Key.TransportModeId,
                                                   CustomerName = "Others",
                                                   directionID = c.Key.DirectionId,
                                                   transportModeID = c.Key.TransportModeId,
                                                   total = c.Count(),
                                                   sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                                   sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                                   totalProfitInLocalCurrency = c.Sum(s => s.ProfitInLocalCurrency),
                                                   totalProfitInProfitCurrency = c.Sum(s => s.ProfitInProfitCurrency),
                                               });

                                resulList = resultList1.Concat(resultList2).OrderByDescending(d => d.totalProfitInLocalCurrency);
                            }
                            else
                            {
                                resulList = resultList1.OrderByDescending(d => d.totalProfitInLocalCurrency);
                            }

                        }


                        break;
                    }
                #endregion

                #region measurement is profit in profit
                case 4:
                    {
                        if (type == "CreateDate" || type == null)
                        {
                            var top10 = (from t in allShipments
                                         where t.CreateDateTime > FromDate && t.CreateDateTime < ToDate
                                         group t by t.CustomerId into g
                                         select new
                                         {
                                             id = g.Key,
                                             c = g.Sum(d => d.ProfitInProfitCurrency),
                                         }).OrderByDescending(r => r.c).Skip(0).Take(top);

                            resultList1 = (from s in allShipments
                                           where s.CreateDateTime > FromDate && s.CreateDateTime < ToDate
                                           && top10.Any(q => q.id == s.CustomerId)
                                           group s by new
                                           {
                                               s.CustomerId,
                                               s.CustomerName,
                                               s.DirectionId,
                                               s.TransportModeId,
                                           } into c

                                           select new DashBoardClass()
                                           {
                                               CustomerID = c.Key.CustomerId,
                                               CustomerName = c.Key.CustomerName,
                                               directionID = c.Key.DirectionId,
                                               transportModeID = c.Key.TransportModeId,
                                               total = c.Count(),
                                               sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                               sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                               totalProfitInLocalCurrency = c.Sum(s => s.ProfitInLocalCurrency),
                                               totalProfitInProfitCurrency = c.Sum(s => s.ProfitInProfitCurrency),
                                           });
                            if (includeOthers)
                            {
                                resultList2 = (from s in allShipments
                                               where s.CreateDateTime > FromDate && s.CreateDateTime < ToDate
                                               && !top10.Any(q => q.id == s.CustomerId)
                                               group s by new
                                               {
                                                   s.DirectionId,
                                                   s.TransportModeId,
                                               } into c

                                               select new DashBoardClass()
                                               {
                                                   CustomerID = "Others" + c.Key.DirectionId + c.Key.TransportModeId,
                                                   CustomerName = "Others",
                                                   directionID = c.Key.DirectionId,
                                                   transportModeID = c.Key.TransportModeId,
                                                   total = c.Count(),
                                                   sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                                   sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                                   totalProfitInLocalCurrency = c.Sum(s => s.ProfitInLocalCurrency),
                                                   totalProfitInProfitCurrency = c.Sum(s => s.ProfitInProfitCurrency),
                                               });

                                resulList = resultList1.Concat(resultList2).OrderByDescending(d => d.totalProfitInProfitCurrency);
                            }
                            else
                            {
                                resulList = resultList1.OrderByDescending(d => d.totalProfitInProfitCurrency);
                            }

                        }
                        else
                        {
                            var top10 = (from t in allShipments
                                         where t.OperationalDate > FromDate && t.OperationalDate < ToDate
                                         group t by t.CustomerId into g
                                         select new
                                         {
                                             id = g.Key,
                                             c = g.Sum(d => d.ProfitInProfitCurrency),
                                         }).OrderByDescending(r => r.c).Skip(0).Take(top);



                            resultList1 = (from s in allShipments
                                           where s.OperationalDate > FromDate && s.OperationalDate < ToDate
                                           && top10.Any(q => q.id == s.CustomerId)
                                           group s by new
                                           {
                                               s.CustomerId,
                                               s.CustomerName,
                                               s.DirectionId,
                                               s.TransportModeId,
                                           } into c

                                           select new DashBoardClass()
                                           {
                                               CustomerID = c.Key.CustomerId,
                                               CustomerName = c.Key.CustomerName,
                                               directionID = c.Key.DirectionId,
                                               transportModeID = c.Key.TransportModeId,
                                               total = c.Count(),
                                               sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                               sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                               totalProfitInLocalCurrency = c.Sum(s => s.ProfitInLocalCurrency),
                                               totalProfitInProfitCurrency = c.Sum(s => s.ProfitInProfitCurrency),
                                           });
                            if (includeOthers)
                            {
                                resultList2 = (from s in allShipments
                                               where s.OperationalDate > FromDate && s.OperationalDate < ToDate
                                               && !top10.Any(q => q.id == s.CustomerId)
                                               group s by new
                                               {
                                                   s.DirectionId,
                                                   s.TransportModeId,
                                               } into c

                                               select new DashBoardClass()
                                               {
                                                   CustomerID = "Others" + c.Key.DirectionId + c.Key.TransportModeId,
                                                   CustomerName = "Others",
                                                   directionID = c.Key.DirectionId,
                                                   transportModeID = c.Key.TransportModeId,
                                                   total = c.Count(),
                                                   sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                                   sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                                   totalProfitInLocalCurrency = c.Sum(s => s.ProfitInLocalCurrency),
                                                   totalProfitInProfitCurrency = c.Sum(s => s.ProfitInProfitCurrency),
                                               });

                                resulList = resultList1.Concat(resultList2).OrderByDescending(d => d.totalProfitInProfitCurrency);
                            }
                            else
                            {
                                resulList = resultList1.OrderByDescending(d => d.totalProfitInProfitCurrency);
                            }

                        }


                        break;
                    }
                #endregion

                #region measurement is receivable in local
                case 5:
                    {
                        if (type == "CreateDate" || type == null)
                        {
                            var top10 = (from t in allShipments
                                         where t.CreateDateTime > FromDate && t.CreateDateTime < ToDate
                                         group t by t.CustomerId into g
                                         select new
                                         {
                                             id = g.Key,
                                             c = g.Sum(d => d.OpenReceivablesInLocalCurrency),
                                         }).OrderByDescending(r => r.c).Skip(0).Take(top);


                            resultList1 = (from s in allShipments
                                           where s.CreateDateTime > FromDate && s.CreateDateTime < ToDate
                                           && top10.Any(q => q.id == s.CustomerId)
                                           group s by new
                                           {
                                               s.CustomerId,
                                               s.CustomerName,
                                               s.DirectionId,
                                               s.TransportModeId,
                                           } into c

                                           select new DashBoardClass()
                                           {
                                               CustomerID = c.Key.CustomerId,
                                               CustomerName = c.Key.CustomerName,
                                               directionID = c.Key.DirectionId,
                                               transportModeID = c.Key.TransportModeId,
                                               total = c.Count(),
                                               sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                               sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                               totalProfitInLocalCurrency = c.Sum(s => s.ProfitInLocalCurrency),
                                               totalProfitInProfitCurrency = c.Sum(s => s.ProfitInProfitCurrency),
                                               ReceivablesInLocalCurrency = c.Sum(s => s.OpenReceivablesInLocalCurrency),
                                               ReceivablesInProfitCurrency = c.Sum(s => s.OpenReceivablesInProfitCurrency),
                                           });
                            if (includeOthers)
                            {
                                resultList2 = (from s in allShipments
                                               where s.CreateDateTime > FromDate && s.CreateDateTime < ToDate
                                               && !top10.Any(q => q.id == s.CustomerId)
                                               group s by new
                                               {
                                                   s.DirectionId,
                                                   s.TransportModeId,
                                               } into c

                                               select new DashBoardClass()
                                               {
                                                   CustomerID = "Others" + c.Key.DirectionId + c.Key.TransportModeId,
                                                   CustomerName = "Others",
                                                   directionID = c.Key.DirectionId,
                                                   transportModeID = c.Key.TransportModeId,
                                                   total = c.Count(),
                                                   sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                                   sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                                   totalProfitInLocalCurrency = c.Sum(s => s.ProfitInLocalCurrency),
                                                   totalProfitInProfitCurrency = c.Sum(s => s.ProfitInProfitCurrency),
                                                   ReceivablesInLocalCurrency = c.Sum(s => s.OpenReceivablesInLocalCurrency),
                                                   ReceivablesInProfitCurrency = c.Sum(s => s.OpenReceivablesInProfitCurrency),
                                               });

                                resulList = resultList1.Concat(resultList2).OrderByDescending(d => d.ReceivablesInLocalCurrency);
                            }
                            else
                            {
                                resulList = resultList1.OrderByDescending(d => d.ReceivablesInLocalCurrency);
                            }
                        }
                        else
                        {
                            var top10 = (from t in allShipments
                                         where t.OperationalDate > FromDate && t.OperationalDate < ToDate
                                         group t by t.CustomerId into g
                                         select new
                                         {
                                             id = g.Key,
                                             c = g.Sum(d => d.OpenReceivablesInLocalCurrency),
                                         }).OrderByDescending(r => r.c).Skip(0).Take(top);

                            resultList1 = (from s in allShipments
                                           where s.OperationalDate > FromDate && s.OperationalDate < ToDate
                                           && top10.Any(q => q.id == s.CustomerId)
                                           group s by new
                                           {
                                               s.CustomerId,
                                               s.CustomerName,
                                               s.DirectionId,
                                               s.TransportModeId,
                                           } into c

                                           select new DashBoardClass()
                                           {
                                               CustomerID = c.Key.CustomerId,
                                               CustomerName = c.Key.CustomerName,
                                               directionID = c.Key.DirectionId,
                                               transportModeID = c.Key.TransportModeId,
                                               total = c.Count(),
                                               sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                               sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                               totalProfitInLocalCurrency = c.Sum(s => s.ProfitInLocalCurrency),
                                               totalProfitInProfitCurrency = c.Sum(s => s.ProfitInProfitCurrency),
                                               ReceivablesInLocalCurrency = c.Sum(s => s.OpenReceivablesInLocalCurrency),
                                               ReceivablesInProfitCurrency = c.Sum(s => s.OpenReceivablesInProfitCurrency),
                                           });
                            if (includeOthers)
                            {
                                resultList2 = (from s in allShipments
                                               where s.OperationalDate > FromDate && s.OperationalDate < ToDate
                                               && !top10.Any(q => q.id == s.CustomerId)
                                               group s by new
                                               {
                                                   s.DirectionId,
                                                   s.TransportModeId,
                                               } into c

                                               select new DashBoardClass()
                                               {
                                                   CustomerID = "Others" + c.Key.DirectionId + c.Key.TransportModeId,
                                                   CustomerName = "Others",
                                                   directionID = c.Key.DirectionId,
                                                   transportModeID = c.Key.TransportModeId,
                                                   total = c.Count(),
                                                   sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                                   sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                                   totalProfitInLocalCurrency = c.Sum(s => s.ProfitInLocalCurrency),
                                                   totalProfitInProfitCurrency = c.Sum(s => s.ProfitInProfitCurrency),
                                                   ReceivablesInLocalCurrency = c.Sum(s => s.OpenReceivablesInLocalCurrency),
                                                   ReceivablesInProfitCurrency = c.Sum(s => s.OpenReceivablesInProfitCurrency),
                                               });

                                resulList = resultList1.Concat(resultList2).OrderByDescending(d => d.ReceivablesInLocalCurrency);
                            }
                            else
                            {
                                resulList = resultList1.OrderByDescending(d => d.ReceivablesInLocalCurrency);
                            }
                        }



                        break;
                    }
                #endregion

                #region measurement is receivable in profit
                case 6:
                    {
                        if (type == "CreateDate" || type == null)
                        {

                            var top10 = (from t in allShipments
                                         where t.CreateDateTime > FromDate && t.CreateDateTime < ToDate
                                         group t by t.CustomerId into g
                                         select new
                                         {
                                             id = g.Key,
                                             c = g.Sum(d => d.OpenReceivablesInProfitCurrency),
                                         }).OrderByDescending(r => r.c).Skip(0).Take(top);

                            resultList1 = (from s in allShipments
                                           where s.CreateDateTime > FromDate && s.CreateDateTime < ToDate
                                           && top10.Any(q => q.id == s.CustomerId)
                                           group s by new
                                           {
                                               s.CustomerId,
                                               s.CustomerName,
                                               s.DirectionId,
                                               s.TransportModeId,
                                           } into c

                                           select new DashBoardClass()
                                           {
                                               CustomerID = c.Key.CustomerId,
                                               CustomerName = c.Key.CustomerName,
                                               directionID = c.Key.DirectionId,
                                               transportModeID = c.Key.TransportModeId,
                                               total = c.Count(),
                                               sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                               sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                               totalProfitInLocalCurrency = c.Sum(s => s.ProfitInLocalCurrency),
                                               totalProfitInProfitCurrency = c.Sum(s => s.ProfitInProfitCurrency),
                                               ReceivablesInLocalCurrency = c.Sum(s => s.OpenReceivablesInLocalCurrency),
                                               ReceivablesInProfitCurrency = c.Sum(s => s.OpenReceivablesInProfitCurrency),
                                           });
                            if (includeOthers)
                            {
                                resultList2 = (from s in allShipments
                                               where s.CreateDateTime > FromDate && s.CreateDateTime < ToDate
                                               && !top10.Any(q => q.id == s.CustomerId)
                                               group s by new
                                               {
                                                   s.DirectionId,
                                                   s.TransportModeId,
                                               } into c

                                               select new DashBoardClass()
                                               {
                                                   CustomerID = "Others" + c.Key.DirectionId + c.Key.TransportModeId,
                                                   CustomerName = "Others",
                                                   directionID = c.Key.DirectionId,
                                                   transportModeID = c.Key.TransportModeId,
                                                   total = c.Count(),
                                                   sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                                   sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                                   totalProfitInLocalCurrency = c.Sum(s => s.ProfitInLocalCurrency),
                                                   totalProfitInProfitCurrency = c.Sum(s => s.ProfitInProfitCurrency),
                                                   ReceivablesInLocalCurrency = c.Sum(s => s.OpenReceivablesInLocalCurrency),
                                                   ReceivablesInProfitCurrency = c.Sum(s => s.OpenReceivablesInProfitCurrency),
                                               });

                                resulList = resultList1.Concat(resultList2).OrderByDescending(d => d.ReceivablesInProfitCurrency);
                            }
                            else
                            {
                                resulList = resultList1.OrderByDescending(d => d.ReceivablesInProfitCurrency);
                            }
                        }

                        else
                        {

                            var top10 = (from t in allShipments
                                         where t.OperationalDate > FromDate && t.OperationalDate < ToDate
                                         group t by t.CustomerId into g
                                         select new
                                         {
                                             id = g.Key,
                                             c = g.Sum(d => d.OpenReceivablesInProfitCurrency),
                                         }).OrderByDescending(r => r.c).Skip(0).Take(top);

                            resultList1 = (from s in allShipments
                                           where s.OperationalDate > FromDate && s.OperationalDate < ToDate
                                           && top10.Any(q => q.id == s.CustomerId)
                                           group s by new
                                           {
                                               s.CustomerId,
                                               s.CustomerName,
                                               s.DirectionId,
                                               s.TransportModeId,
                                           } into c

                                           select new DashBoardClass()
                                           {
                                               CustomerID = c.Key.CustomerId,
                                               CustomerName = c.Key.CustomerName,
                                               directionID = c.Key.DirectionId,
                                               transportModeID = c.Key.TransportModeId,
                                               total = c.Count(),
                                               sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                               sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                               totalProfitInLocalCurrency = c.Sum(s => s.ProfitInLocalCurrency),
                                               totalProfitInProfitCurrency = c.Sum(s => s.ProfitInProfitCurrency),
                                               ReceivablesInLocalCurrency = c.Sum(s => s.OpenReceivablesInLocalCurrency),
                                               ReceivablesInProfitCurrency = c.Sum(s => s.OpenReceivablesInProfitCurrency),
                                           });
                            if (includeOthers)
                            {
                                resultList2 = (from s in allShipments
                                               where s.OperationalDate > FromDate && s.OperationalDate < ToDate
                                               && !top10.Any(q => q.id == s.CustomerId)
                                               group s by new
                                               {

                                                   s.DirectionId,
                                                   s.TransportModeId,
                                               } into c

                                               select new DashBoardClass()
                                               {
                                                   CustomerID = "Others" + c.Key.DirectionId + c.Key.TransportModeId,
                                                   CustomerName = "Others",
                                                   directionID = c.Key.DirectionId,
                                                   transportModeID = c.Key.TransportModeId,
                                                   total = c.Count(),
                                                   sumChargeableWeight = c.Sum(s => s.ChargeableWeightInKG),
                                                   sumGrossWeight = c.Sum(s => s.GrossWeightInKG),
                                                   totalProfitInLocalCurrency = c.Sum(s => s.ProfitInLocalCurrency),
                                                   totalProfitInProfitCurrency = c.Sum(s => s.ProfitInProfitCurrency),
                                                   ReceivablesInLocalCurrency = c.Sum(s => s.OpenReceivablesInLocalCurrency),
                                                   ReceivablesInProfitCurrency = c.Sum(s => s.OpenReceivablesInProfitCurrency),
                                               });

                                resulList = resultList1.Concat(resultList2).OrderByDescending(d => d.ReceivablesInProfitCurrency);
                            }
                            else
                            {
                                resulList = resultList1.OrderByDescending(d => d.ReceivablesInProfitCurrency);
                            }
                        }


                        break;
                    }
                #endregion

                default: break;
            }

            return resulList;
        }


        /* DashBoard Top 10 Customers */
        //public List<DashBoardClass> GetTop10DashBoardCustom(string type, DateTime? FromDate, DateTime? ToDate, int measurment, int currentTenant, int top, bool includeOthers)
        //{
        //    if (top < 0)
        //    {
        //        top = 0;
        //    }         

        //    List<DashBoardClass> datalist = null;
        //    List<ShipmentsCustomersDashboardView> shipments = repository.GetShipmentDataViewsForCustomersDashboard(currentTenant).Take(top).ToList();

        //    Dictionary<string, DashBoardClass> Listt = new Dictionary<string, DashBoardClass>();

        //    foreach (ShipmentsCustomersDashboardView d in shipments)
        //    {
        //        string Index = "";
        //        Index = d.CustomerId;                
        //        if (!Listt.ContainsKey(Index))
        //        {
        //            DashBoardClass Item = new DashBoardClass();
        //            Item.countryName = d.CustomerName;
        //            Item.sumChargeableWeight = d.ChargeableWeightInKG;
        //            Item.sumGrossWeight = d.GrossWeightInKG;
        //            Item.totalProfitInLocalCurrency = d.ProfitInLocalCurrency;
        //            Item.totalProfitInProfitCurrency = d.ProfitInProfitCurrency;
        //            Item.ReceivablesInLocalCurrency = d.OpenReceivablesInLocalCurrency;
        //            Item.ReceivablesInProfitCurrency = d.OpenReceivablesInProfitCurrency;
        //            Item.directionID = d.DirectionId;
        //            Item.transportModeID = d.TransportModeId;
        //            Item.total = 1;
        //            Listt.Add(Index, Item);
        //        }
        //        else
        //        {
        //            Listt[Index].countryName = d.CustomerName;
        //            Listt[Index].sumChargeableWeight += d.ChargeableWeightInKG != null ? d.ChargeableWeightInKG : 0;
        //            Listt[Index].sumGrossWeight += d.GrossWeightInKG != null ? d.GrossWeightInKG : 0;
        //            Listt[Index].totalProfitInLocalCurrency += d.ProfitInLocalCurrency != null ? d.ProfitInLocalCurrency : 0;
        //            Listt[Index].totalProfitInProfitCurrency += d.ProfitInProfitCurrency != null ? d.ProfitInProfitCurrency : 0;
        //            Listt[Index].ReceivablesInLocalCurrency += d.OpenReceivablesInLocalCurrency != null ? d.OpenReceivablesInLocalCurrency : 0;
        //            Listt[Index].ReceivablesInProfitCurrency += d.OpenReceivablesInProfitCurrency != null ? d.OpenReceivablesInProfitCurrency : 0;
        //            Listt[Index].directionID = d.DirectionId;
        //            Listt[Index].transportModeID = d.TransportModeId;
        //            Listt[Index].total = Listt[Index].total + 1;


        //        }
        //    }

        //    datalist = new List<DashBoardClass>();

        //    for (int i = 0; i < Listt.Count; i++)
        //    {
        //        datalist.Add(Listt.Values.ElementAt(i));
        //    }
        //    return datalist;

        //}





        /* DashBoard By Country */
        public IQueryable<DashBoardClass> GetShipmentsByCountryDashBoard(int last, int currentTenant)
        {
            IQueryable<DashBoardClass> resulList = null;
            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(currentTenant);
            DateTime lastDate = todayDate.Date.AddMonths(last);

            IQueryable<Shipment> allShipments = (from a in repository.context.Shipments
                                                 where a.Tenant == currentTenant && a.IsCancelled == false
                                                 select a);
            allShipments = BranchPermitionsFilter.AddUserBranchRestrictionFilters<Shipment>(new QueryOperations(), allShipments, currentTenant);
            allShipments = ProductPermitionsFilter.AddUserProductRestrictionFilters<Shipment>(new QueryOperations(), allShipments, currentTenant);

            resulList = from s in allShipments
                        where s.Tenant == currentTenant && s.IsCancelled == false
                        && s.CreateDateTime <= todayDate
                        && s.CreateDateTime >= lastDate

                        group s by new
                        {
                            s.DirectionId,
                            s.TransportModeId,
                            s.ToPort.Country.Code,
                            s.ToPort.Country.EnglishName,
                        } into m


                        select new DashBoardClass()
                        {
                            directionID = m.Key.DirectionId,
                            transportModeID = m.Key.TransportModeId,
                            countryCode = m.Key.Code,
                            countryName = m.Key.EnglishName,
                            country = m.Key.Code,
                            total = m.Count(),
                            sumChargeableWeight = m.Sum(s => s.ChargeableWeightInKG),
                            sumGrossWeight = m.Sum(s => s.GrossWeightInKG),
                        };

            return resulList;
        }

        /*DashBoard By direction and transportmode*/
        public IQueryable<DashBoardClass> GetShipmentsByDirectionAndTransMode(string type, int lastMonths, int lastDays, int tenant, string customerid)
        {
            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            DateTime lastDate = todayDate.Date.AddDays(lastDays);
            if (lastMonths != 0)
            {
                lastDate = todayDate.Date.AddMonths(lastMonths);
            }


            IQueryable<ShipmentDirectionTransmodeView> allShipments = repository.GetShipmentDataViewsForDirectionAndTransmodeDashboard(tenant, customerid);
            allShipments = BranchPermitionsFilter.AddUserBranchRestrictionFilters<ShipmentDirectionTransmodeView>(new QueryOperations(), allShipments, tenant);
            allShipments = ProductPermitionsFilter.AddUserProductRestrictionFilters<ShipmentDirectionTransmodeView>(new QueryOperations(), allShipments, tenant);
            IQueryable<DashBoardClass> datalist;

            DateTime FromDateQuery = TenantServerConfigration.GetCurrentDateTime(tenant);

            if (type == "CreateDate" || type == null)
            {
                datalist = from a in allShipments
                           where a.Tenant == tenant && a.IsCancelled == false
                             && (a.CreateDateTime < FromDateQuery)
                             && (a.CreateDateTime > lastDate)
                           group a by
                                                       new
                                                       {
                                                           a.TransportModeId,
                                                           transmodeName = a.TransportModeName,
                                                           a.DirectionId,
                                                           directionName = a.DirectionName,
                                                           a.CreateDateTime.Day,
                                                           a.CreateDateTime.Month,
                                                           a.CreateDateTime.Year
                                                       }
                                                       into g
                           select new DashBoardClass()
                           {
                               day = g.Key.Day,
                               year = g.Key.Year,
                               month = g.Key.Month,
                               directionID = g.Key.DirectionId,
                               transportModeID = g.Key.TransportModeId,
                               TransportModeName = g.Key.transmodeName,
                               DirectionName = g.Key.directionName,
                               total = g.Count(),
                               sumGrossWeight = g.Sum(d => d.GrossWeightInKG),
                               sumChargeableWeight = g.Sum(d => d.ChargeableWeightInKG),
                               totalProfitInLocalCurrency = g.Sum(d => d.ProfitInLocalCurrency),
                               totalProfitInProfitCurrency = g.Sum(d => d.ProfitInProfitCurrency),
                               ReceivablesInLocalCurrency = (g.Sum(d => d.OpenReceivablesInLocalCurrency) + g.Sum(d => d.AccountedReceivablesInLocalCurrency)),
                               ReceivablesInProfitCurrency = (g.Sum(d => d.OpenReceivablesInProfitCurrency) + g.Sum(d => d.AccountedReceivablesInProfitCurrency)),
                           };
            }
            else
            {
                datalist = from a in allShipments
                           where a.Tenant == tenant && a.IsCancelled == false
                             && a.OperationalDate < FromDateQuery
                             && a.OperationalDate > lastDate
                           group a by
                           new
                           {
                               a.TransportModeId,
                               transmodeName = a.TransportModeName,
                               a.DirectionId,
                               directionName = a.DirectionName,
                               a.CreateDateTime.Day,
                               a.CreateDateTime.Month,
                               a.CreateDateTime.Year
                           }
                                                     into g
                           select new DashBoardClass()
                           {
                               day = g.Key.Day,
                               year = g.Key.Year,
                               month = g.Key.Month,
                               directionID = g.Key.DirectionId,
                               transportModeID = g.Key.TransportModeId,
                               TransportModeName = g.Key.transmodeName,
                               DirectionName = g.Key.directionName,
                               total = g.Count(),
                               sumGrossWeight = g.Sum(d => d.GrossWeightInKG),
                               sumChargeableWeight = g.Sum(d => d.ChargeableWeightInKG),
                               totalProfitInLocalCurrency = g.Sum(d => d.ProfitInLocalCurrency),
                               totalProfitInProfitCurrency = g.Sum(d => d.ProfitInProfitCurrency),
                               ReceivablesInLocalCurrency = (g.Sum(d => d.OpenReceivablesInLocalCurrency) + g.Sum(d => d.AccountedReceivablesInLocalCurrency)),
                               ReceivablesInProfitCurrency = (g.Sum(d => d.OpenReceivablesInProfitCurrency) + g.Sum(d => d.AccountedReceivablesInProfitCurrency)),
                           };

            }
            return datalist;
        }

        public List<DashBoardClass> GetShipmentsByDirectionAndTransModeCustom(string type, DateTime? FromDate, DateTime? ToDate, int tenant, string customerid)
        {
            bool AddYearFlag = false;
            if (FromDate.Value.Year != ToDate.Value.Year)
                AddYearFlag = true;


            IQueryable<ShipmentDirectionTransmodeView> allShipments = repository.GetShipmentDataViewsForDirectionAndTransmodeDashboard(tenant, customerid);
            allShipments = BranchPermitionsFilter.AddUserBranchRestrictionFilters<ShipmentDirectionTransmodeView>(new QueryOperations(), allShipments, tenant);
            allShipments = ProductPermitionsFilter.AddUserProductRestrictionFilters<ShipmentDirectionTransmodeView>(new QueryOperations(), allShipments, tenant);

            DateTime FromDateQuery = FromDate.Value.AddDays(-1);
            DateTime ToDateQuery = ToDate.Value.AddDays(1);





            List<DashBoardClass> datalist;
            if (type == "CreateDate" || type == null)
            {
                datalist = (from a in allShipments
                            where a.Tenant == tenant && a.IsCancelled == false
                              && (a.CreateDateTime < ToDateQuery)
                              && (a.CreateDateTime > FromDateQuery)
                            orderby a.CreateDateTime

                            group a by
                                                       new
                                                       {
                                                           a.TransportModeId,
                                                           transmodeName = a.TransportModeName,
                                                           a.DirectionId,
                                                           directionName = a.DirectionName,
                                                           a.CreateDateTime.Day,
                                                           a.CreateDateTime.Month,
                                                           a.CreateDateTime.Year,
                                                       }
                                                       into g
                            select new DashBoardClass()
                            {
                                day = g.Key.Day,
                                year = g.Key.Year,
                                month = g.Key.Month,
                                directionID = g.Key.DirectionId,
                                transportModeID = g.Key.TransportModeId,
                                TransportModeName = g.Key.transmodeName,
                                DirectionName = g.Key.directionName,
                                total = g.Count(),
                                sumGrossWeight = g.Sum(d => d.GrossWeightInKG),
                                sumChargeableWeight = g.Sum(d => d.ChargeableWeightInKG),
                                totalProfitInLocalCurrency = g.Sum(d => d.ProfitInLocalCurrency),
                                totalProfitInProfitCurrency = g.Sum(d => d.ProfitInProfitCurrency),
                                ReceivablesInLocalCurrency = (g.Sum(d => d.OpenReceivablesInLocalCurrency) + g.Sum(d => d.AccountedReceivablesInLocalCurrency)),
                                ReceivablesInProfitCurrency = (g.Sum(d => d.OpenReceivablesInProfitCurrency) + g.Sum(d => d.AccountedReceivablesInProfitCurrency)),
                            }).ToList();
            }
            else
            {
                datalist = (from a in allShipments
                            where a.Tenant == tenant && a.IsCancelled == false
                              && a.OperationalDate < ToDateQuery
                              && a.OperationalDate > FromDateQuery
                            orderby a.OperationalDate

                            group a by
                           new
                           {
                               a.TransportModeId,
                               transmodeName = a.TransportModeName,
                               a.DirectionId,
                               directionName = a.DirectionName,
                               a.OperationalDate.Day,
                               a.OperationalDate.Month,
                               a.OperationalDate.Year,
                           }
                                                     into g
                            select new DashBoardClass()
                            {
                                day = g.Key.Day,
                                year = g.Key.Year,
                                month = g.Key.Month,
                                directionID = g.Key.DirectionId,
                                transportModeID = g.Key.TransportModeId,
                                TransportModeName = g.Key.transmodeName,
                                DirectionName = g.Key.directionName,
                                total = g.Count(),
                                sumGrossWeight = g.Sum(d => d.GrossWeightInKG),
                                sumChargeableWeight = g.Sum(d => d.ChargeableWeightInKG),
                                totalProfitInLocalCurrency = g.Sum(d => d.ProfitInLocalCurrency),
                                totalProfitInProfitCurrency = g.Sum(d => d.ProfitInProfitCurrency),
                                ReceivablesInLocalCurrency = (g.Sum(d => d.OpenReceivablesInLocalCurrency) + g.Sum(d => d.AccountedReceivablesInLocalCurrency)),
                                ReceivablesInProfitCurrency = (g.Sum(d => d.OpenReceivablesInProfitCurrency) + g.Sum(d => d.AccountedReceivablesInProfitCurrency)),
                            }).ToList();

            }

            Dictionary<string, DashBoardClass> Listt = new Dictionary<string, DashBoardClass>();

            foreach (DashBoardClass d in datalist)
            {
                string Index = "";
                Index = d.transportModeID + "|" + d.directionID;
                if (!Listt.ContainsKey(Index))
                {
                    Listt.Add(Index, d);
                }
                else
                {
                    Listt[Index].total += d.total;
                    Listt[Index].sumChargeableWeight += d.sumChargeableWeight != null ? d.sumChargeableWeight : 0;
                    Listt[Index].sumGrossWeight += d.sumGrossWeight != null ? d.sumGrossWeight : 0;
                    Listt[Index].totalProfitInLocalCurrency += d.totalProfitInLocalCurrency != null ? d.totalProfitInLocalCurrency : 0;
                    Listt[Index].totalProfitInProfitCurrency += d.totalProfitInProfitCurrency != null ? d.totalProfitInProfitCurrency : 0;
                    Listt[Index].ReceivablesInLocalCurrency += d.ReceivablesInLocalCurrency != null ? d.ReceivablesInLocalCurrency : 0;
                    Listt[Index].ReceivablesInProfitCurrency += d.ReceivablesInProfitCurrency != null ? d.ReceivablesInProfitCurrency : 0;
                    Listt[Index].DirectionName = d.DirectionName;
                    Listt[Index].TransportModeName = d.TransportModeName;

                }
            }

            datalist = new List<DashBoardClass>();

            for (int i = 0; i < Listt.Count; i++)
            {
                datalist.Add(Listt.Values.ElementAt(i));
            }
            return datalist;
        }


        public IQueryable<DashBoardClass> GetShipmentsByDirectionForCustomer(int last, int tenant, string customerid)
        {
            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            DateTime lastDate = todayDate.Date.AddMonths(last);

            IQueryable<Shipment> shipments = null;

            IQueryable<Shipment> allShipments = (from a in repository.context.Shipments
                                                 where a.Tenant == tenant && a.IsCancelled == false
                                                 select a);
            allShipments = BranchPermitionsFilter.AddUserBranchRestrictionFilters<Shipment>(new QueryOperations(), allShipments, tenant);
            allShipments = ProductPermitionsFilter.AddUserProductRestrictionFilters<Shipment>(new QueryOperations(), allShipments, tenant);

            shipments = from a in allShipments
                        where a.Tenant == tenant && a.IsCancelled == false && a.CustomerId == customerid && a.ShipmentLevelCode != "C"
                        select a;

            IQueryable<DashBoardClass> datalist = from a in shipments.Include("Direction")
                                                  where a.Tenant == tenant && a.IsCancelled == false
                                                    && a.CreateDateTime <= todayDate
                                                    && a.CreateDateTime >= lastDate
                                                  group a by
                                                  new
                                                  {
                                                      a.DirectionId,
                                                      directionName = a.Direction.Name,
                                                      a.CreateDateTime.Day,
                                                      a.CreateDateTime.Month,
                                                      a.CreateDateTime.Year
                                                  }
                                                      into g
                                                  select new DashBoardClass()
                                                  {
                                                      day = g.Key.Day,
                                                      year = g.Key.Year,
                                                      month = g.Key.Month,
                                                      directionID = g.Key.DirectionId,
                                                      DirectionName = g.Key.directionName,
                                                      total = g.Count(),
                                                      sumGrossWeight = g.Sum(d => d.GrossWeightInKG),
                                                      sumChargeableWeight = g.Sum(d => d.ChargeableWeightInKG),
                                                      totalProfitInLocalCurrency = g.Sum(d => d.ProfitInLocalCurrency),
                                                      totalProfitInProfitCurrency = g.Sum(d => d.ProfitInProfitCurrency),
                                                      ReceivablesInLocalCurrency = (g.Sum(d => d.OpenReceivablesInLocalCurrency) + g.Sum(d => d.AccountedReceivablesInLocalCurrency)),
                                                      ReceivablesInProfitCurrency = (g.Sum(d => d.OpenReceivablesInProfitCurrency) + g.Sum(d => d.AccountedReceivablesInProfitCurrency)),
                                                  };
            return datalist;
        }

        public IQueryable<DashBoardClass> GetShipmentsByTransModeForCustomer(int last, int tenant, string customerid)
        {
            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            DateTime lastDate = todayDate.Date.AddMonths(last);


            IQueryable<Shipment> shipments = null;

            shipments = from a in repository.context.Shipments
                        where a.Tenant == tenant && a.IsCancelled == false && a.CustomerId == customerid && a.ShipmentLevelCode != "C"
                        select a;

            shipments = BranchPermitionsFilter.AddUserBranchRestrictionFilters<Shipment>(new QueryOperations(), shipments, tenant);
            shipments = ProductPermitionsFilter.AddUserProductRestrictionFilters<Shipment>(new QueryOperations(), shipments, tenant);

            IQueryable<DashBoardClass> datalist = from a in shipments.Include("TransportMode")
                                                  where a.Tenant == tenant && a.IsCancelled == false
                                                    && a.CreateDateTime <= todayDate
                                                    && a.CreateDateTime >= lastDate
                                                  group a by
                                                  new
                                                  {
                                                      a.TransportModeId,
                                                      transmodeName = a.TransportMode.Name,

                                                      a.CreateDateTime.Day,
                                                      a.CreateDateTime.Month,
                                                      a.CreateDateTime.Year
                                                  }
                                                      into g
                                                  select new DashBoardClass()
                                                  {
                                                      day = g.Key.Day,
                                                      year = g.Key.Year,
                                                      month = g.Key.Month,

                                                      transportModeID = g.Key.TransportModeId,
                                                      TransportModeName = g.Key.transmodeName,

                                                      total = g.Count(),
                                                      sumGrossWeight = g.Sum(d => d.GrossWeightInKG),
                                                      sumChargeableWeight = g.Sum(d => d.ChargeableWeightInKG),
                                                      totalProfitInLocalCurrency = g.Sum(d => d.ProfitInLocalCurrency),
                                                      totalProfitInProfitCurrency = g.Sum(d => d.ProfitInProfitCurrency),
                                                      ReceivablesInLocalCurrency = (g.Sum(d => d.OpenReceivablesInLocalCurrency) + g.Sum(d => d.AccountedReceivablesInLocalCurrency)),
                                                      ReceivablesInProfitCurrency = (g.Sum(d => d.OpenReceivablesInProfitCurrency) + g.Sum(d => d.AccountedReceivablesInProfitCurrency)),
                                                  };
            return datalist;
        }
        #endregion

        public ShipmentsSummary GetShipmentsDashBoardSummary(int tenant, string directionId, string transportModeId, string loggedContactId, bool hasETDFeature, bool hasFollowupsFeature, bool hasExpDepNotTransmittedFeature, bool hasShippingInstructionsLast7DaysFeature, bool hasContainerStatusLast7DaysFeature, bool hasEBookingInProgress)
        {
            ShipmentsSummary myResult = new ShipmentsSummary() { Id = 1 };

            IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(tenant);
            IWebFreightContext myFreightContext = WebFreightContext.GetContext(tenant);
            ICommonDataContext myCommonContext = CommonDataContext.GetContext(tenant);

            IQueryable<Shipment> iQueryable_Shipments = (from f in shipmentsContext.Shipments where f.Tenant == tenant && f.IsCancelled == false select f);

            iQueryable_Shipments = BranchPermitionsFilter.AddUserBranchRestrictionFilters<Shipment>(new QueryOperations(), iQueryable_Shipments, tenant);
            iQueryable_Shipments = ProductPermitionsFilter.AddUserProductRestrictionFilters<Shipment>(new QueryOperations(), iQueryable_Shipments, tenant);

            if (!string.IsNullOrEmpty(directionId))
            {
                iQueryable_Shipments = iQueryable_Shipments.Where(d => d.DirectionId == directionId);
            }

            if (!string.IsNullOrEmpty(transportModeId))
            {
                iQueryable_Shipments = iQueryable_Shipments.Where(d => d.TransportModeId == transportModeId);
            }

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime lastWeekDate = todayDate.AddDays(-7);

            // Operational Open
            IQueryable<Shipment> iQueryable_OperationalOpen = iQueryable_Shipments.Where(d => d.IsOperationalClosed == false);
            myResult.OperationalOpenCount_DH = iQueryable_OperationalOpen.Where(d => d.ShipmentLevelCode == "D" || d.ShipmentLevelCode == "H" || d.ShipmentLevelCode == "A").Take(1001).Count();
            myResult.OperationalOpenCount_DC = iQueryable_OperationalOpen.Where(d => d.ShipmentLevelCode == "D" || d.ShipmentLevelCode == "C").Take(1001).Count();
            myResult.ImportShipmentsCount = iQueryable_OperationalOpen.Where(d => d.ShipmentLevelCode != "C" && ((d.DirectionId == "C" && d.NoFreightFile == true) || d.DirectionId == "I")).Take(1001).Count();

            // Accounting Open
            IQueryable<Shipment> iQueryable_AccountingOpen = iQueryable_Shipments.Where(d => d.IsAccountingClosed == false);
            myResult.AccountingOpenCount_DH = iQueryable_AccountingOpen.Where(d => (d.ShipmentLevelCode == "D" || d.ShipmentLevelCode == "H") && d.ShipmentReceivableStatusCode == "OPEN").Take(1001).Count();
            myResult.AccountingOpenCount_DC = iQueryable_AccountingOpen.Where(d => (d.ShipmentLevelCode == "D" || d.ShipmentLevelCode == "C") && d.ShipmentPayableStatusCode == "OPEN").Take(1001).Count();

            // FSR
            myResult.LastSentFSRCount = iQueryable_Shipments.Where(d => d.ShipmentLevelCode != "H" && d.TransportModeId == "A" && d.LastFSRStatusRequestDate >= lastWeekDate).Take(1001).Count();


            // EAWB
            myResult.OperationalOpenCount_LWU = iQueryable_Shipments.Where(d => d.ShipmentLevelCode != "H" && d.CarrierLastStatusDate >= lastWeekDate).Take(1001).Count();

            if (hasETDFeature)
            {
                List<string> unwantedStatuesId = (from d in myFreightContext.EntityStatus where d.Tenant == tenant && (d.Code == "SARR" || d.Code == "SDLD") select d.Id).ToList();
                string statusId1 = unwantedStatuesId[0];
                string statusId2 = unwantedStatuesId[1];

                myResult.OperationalOpenCount_ETD = (from myShipment in iQueryable_Shipments
                                                     join db_Masters in shipmentsContext.ShipmentMasterDatas on myShipment.MasterShipmentDataId equals db_Masters.Id into ShipmentsMasters
                                                     from myMasterData in ShipmentsMasters
                                                     where myShipment.Tenant == tenant
                                                     && myShipment.ShipmentLevelCode != "H"
                                                     && myShipment.DirectionId == "E"
                                                     && myShipment.TransportModeId == "A"
                                                     && myShipment.StatusId != statusId1
                                                     && myShipment.StatusId != statusId2
                                                     && myMasterData.Tenant == tenant
                                                     && myMasterData.MainCarriageATD == null
                                                     && myMasterData.MainCarriageATA == null
                                                     && myMasterData.MainCarriageETD != null
                                                     && System.Data.Entity.DbFunctions.TruncateTime(myMasterData.MainCarriageETD) > lastWeekDate
                                                     select myShipment).Take(1001).Count();
            }

            if (hasExpDepNotTransmittedFeature)
            {
                myResult.ExpectedDeparturesNotTransmittedCount = (from myShipment in iQueryable_Shipments
                                                                  join db_Masters in shipmentsContext.ShipmentMasterDatas on myShipment.MasterShipmentDataId equals db_Masters.Id into ShipmentsMasters
                                                                  from myMasterData in ShipmentsMasters
                                                                  where myShipment.Tenant == tenant
                                                                  && myShipment.ShipmentLevelCode != "H"
                                                                  && myShipment.DirectionId == "E"
                                                                  && myShipment.TransportModeId == "O"
                                                                  && myShipment.INTTRASIStatusCode == "NSEN"
                                                                  && (myShipment.ShipmentTypeId == "FCLD" || myShipment.ShipmentTypeId == "MYGO")
                                                                  && myMasterData.Tenant == tenant
                                                                  && myMasterData.MainCarriageATD == null
                                                                  && myMasterData.MainCarriageETD != null
                                                                  select myShipment).Take(1001).Count();
            }

            if (hasShippingInstructionsLast7DaysFeature)
            {
                myResult.ShippingInstructionsLast7DaysCount = iQueryable_Shipments.Where(d => d.INTTRASIStatusCode != "NSEN" && (d.INTTRASIStatusDate >= lastWeekDate || d.INTTRALastStatusDate >= lastWeekDate)).Take(1001).Count();
            }

            if (hasContainerStatusLast7DaysFeature)
            {
                myResult.ContainerStatusLast7DaysCount = iQueryable_Shipments.Where(d => d.INTTRASIStatusCode != "NSEN" && (d.INTTRALastStatusDate >= lastWeekDate)).Take(1001).Count();
            }

            if (hasEBookingInProgress)
            {
                myResult.EBookingInProgressCount = (from myShipment in iQueryable_Shipments
                                                    join db_Masters in shipmentsContext.ShipmentMasterDatas on myShipment.MasterShipmentDataId equals db_Masters.Id into ShipmentsMasters
                                                    from myMasterData in ShipmentsMasters
                                                    where myShipment.Tenant == tenant
                                                    && (myShipment.ShipmentLevelCode == "H" || myShipment.ShipmentLevelCode == "D")
                                                    && myShipment.DirectionId == "E"
                                                    && myShipment.TransportModeId == "O"
                                                    && myShipment.INTTRABookingTransStatusCode != "NST" && myShipment.INTTRABookingStatusCode != "SI"
                                                    && myMasterData.Tenant == tenant
                                                    && myMasterData.MainCarriageATD == null
                                                    select myShipment).Take(1001).Count();

            }
            // Others
            myResult.CreditLimitBlockedCount = iQueryable_Shipments.Where(d => d.IsNewARInvoiceBlocked == true).Take(1001).Count();

            if (hasFollowupsFeature)
            {
                int allFollowUpsCount = 0;
                int myFollowUpsCount = 0;

                IQueryable<ShipmentFollowUpDataView> allFollowups = repository.GetShipmentFollowUpDataViewByTenant(tenant);
                allFollowups = BranchPermitionsFilter.AddUserBranchRestrictionFilters<ShipmentFollowUpDataView>(new QueryOperations(), allFollowups, tenant);
                allFollowups = ProductPermitionsFilter.AddUserProductRestrictionFilters<ShipmentFollowUpDataView>(new QueryOperations(), allFollowups, tenant);

                if (!string.IsNullOrEmpty(directionId))
                {
                    allFollowups = allFollowups.Where(d => d.DirectionId == directionId);
                }

                if (!string.IsNullOrEmpty(transportModeId))
                {
                    allFollowups = allFollowups.Where(d => d.TransportModeId == transportModeId);
                }

                allFollowUpsCount = allFollowups.Take(1001).Count();
                myFollowUpsCount = allFollowups.Where(d => d.FollowUpOwnerId == loggedContactId).Take(1001).Count();

                myResult.AllFollowUpsCount = allFollowUpsCount;
                myResult.MyFollowUpsCount = myFollowUpsCount;
            }

            return myResult;
        }
        public List<FlightSummary> GetShipmentsDashBoardDeparturesArrivals(int tenant, string directionId, string transportModeId)
        {
            List<FlightSummary> myResult = new List<FlightSummary>();

            DateTime? currentDateTime = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime? firstDateTime = currentDateTime.Value.AddDays(-7).Date;
            DateTime? lastDateTime = currentDateTime.Value.AddDays(7).Date;

            IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(tenant);

            IQueryable<Shipment> iQueryable_Shipments = (from f in shipmentsContext.Shipments where f.Tenant == tenant && f.IsCancelled == false && f.ShipmentLevelCode != "H" select f);

            iQueryable_Shipments = BranchPermitionsFilter.AddUserBranchRestrictionFilters<Shipment>(new QueryOperations(), iQueryable_Shipments, tenant);
            iQueryable_Shipments = ProductPermitionsFilter.AddUserProductRestrictionFilters<Shipment>(new QueryOperations(), iQueryable_Shipments, tenant);

            if (!string.IsNullOrEmpty(directionId))
            {
                iQueryable_Shipments = iQueryable_Shipments.Where(d => d.DirectionId == directionId);
            }

            if (!string.IsNullOrEmpty(transportModeId))
            {
                iQueryable_Shipments = iQueryable_Shipments.Where(d => d.TransportModeId == transportModeId);
            }

            IQueryable<DeparturesArrivalsDataItem> iQuery
                = (from myShipment in iQueryable_Shipments
                   join db_Masters in shipmentsContext.ShipmentMasterDatas on myShipment.MasterShipmentDataId equals db_Masters.Id into ShipmentsMasters
                   from myMasterData in ShipmentsMasters.DefaultIfEmpty()
                   where myMasterData.Tenant == tenant
                   && (myMasterData.DepartureArrivalFromDate >= firstDateTime || myMasterData.DepartureArrivalToDate >= firstDateTime)
                   && (myMasterData.DepartureArrivalFromDate <= lastDateTime || myMasterData.DepartureArrivalToDate <= lastDateTime)
                   select new DeparturesArrivalsDataItem
                   {
                       Id = myShipment.Id,
                       DirectionId = myShipment.DirectionId,
                       TransportModeId = myShipment.TransportModeId,
                       MainCarriageCarrierId = myMasterData.MainCarriageCarrierId,
                       MainCarriageCarrierNumber = myMasterData.MainCarriageCarrierNumber,
                       TruckNumber = myMasterData.TruckNumber,
                       MainCarriageETD = myMasterData.MainCarriageETD,
                       MainCarriageATD = myMasterData.MainCarriageATD,
                       MainCarriageETA = myMasterData.MainCarriageETA,
                       MainCarriageATA = myMasterData.MainCarriageATA,
                       DepartureArrivalToDate = myMasterData.DepartureArrivalToDate,
                       DepartureArrivalFromDate = myMasterData.DepartureArrivalFromDate,
                       MainCarriageFinalDestinationETA = myMasterData.MainCarriageFinalDestinationETA,
                       MainCarriageFinalDestinationATA = myMasterData.MainCarriageFinalDestinationATA,
                   });

            List<FlightSummary> arrivals
                = (from r in iQuery
                   where r.DirectionId == "I"
                   && r.DepartureArrivalToDate >= firstDateTime
                   && r.DepartureArrivalToDate <= lastDateTime
                   select new FlightSummary()
                   {
                       Id = r.Id + ":ARR",
                       ShipmentId = r.Id,
                       DirectionId = r.DirectionId,
                       TransportModeId = r.TransportModeId,
                       CarrierId = !string.IsNullOrEmpty(r.MainCarriageCarrierId) ? r.MainCarriageCarrierId : "No_Data",
                       //CarrierCode = !string.IsNullOrEmpty(r.MainCarriageCarrierCode) ? r.MainCarriageCarrierCode : "No_Data",
                       //CarrierName = !string.IsNullOrEmpty(r.MainCarriageCarrierName) ? r.MainCarriageCarrierName : "No_Data",
                       CarrierNumber = !string.IsNullOrEmpty(r.MainCarriageCarrierNumber) ? r.MainCarriageCarrierNumber : "No_Data",
                       ExpectedDate = r.MainCarriageFinalDestinationETA,
                       ActualDate = r.MainCarriageFinalDestinationATA,
                       DateFilterField = "DepartureArrivalToDate",
                   }).ToList();


            List<FlightSummary> departures =
                (from r in iQuery
                 where (r.DirectionId == "E" || r.DirectionId == "R" || (r.DirectionId == "D" && r.TransportModeId != "I"))
                 && r.DepartureArrivalFromDate >= firstDateTime
                 && r.DepartureArrivalFromDate <= lastDateTime
                 select new FlightSummary()
                 {
                     Id = r.Id + ":DEP",
                     ShipmentId = r.Id,
                     DirectionId = r.DirectionId,
                     TransportModeId = r.TransportModeId,
                     CarrierId = !string.IsNullOrEmpty(r.MainCarriageCarrierId) ? r.MainCarriageCarrierId : "No_Data",
                     //CarrierCode = !string.IsNullOrEmpty(r.MainCarriageCarrierCode) ? r.MainCarriageCarrierCode : "No_Data",
                     //CarrierName = !string.IsNullOrEmpty(r.MainCarriageCarrierName) ? r.MainCarriageCarrierName : "No_Data",
                     CarrierNumber = !string.IsNullOrEmpty(r.MainCarriageCarrierNumber) ? r.MainCarriageCarrierNumber : "No_Data",
                     ExpectedDate = r.MainCarriageETD,
                     ActualDate = r.MainCarriageATD,
                     DateFilterField = "DepartureArrivalFromDate",
                 }).ToList();


            List<FlightSummary> inlandDomestic_DEP =
                (from r in iQuery
                 where r.DirectionId == "D"
                 && r.TransportModeId == "I"
                 && r.DepartureArrivalFromDate >= firstDateTime
                 && r.DepartureArrivalFromDate <= lastDateTime
                 select new FlightSummary()
                 {
                     Id = r.Id + ":INDEP",
                     ShipmentId = r.Id,
                     DirectionId = r.DirectionId,
                     TransportModeId = r.TransportModeId,
                     CarrierId = !string.IsNullOrEmpty(r.MainCarriageCarrierId) ? r.MainCarriageCarrierId : "No_Data",
                     //CarrierCode = !string.IsNullOrEmpty(r.MainCarriageCarrierCode) ? r.MainCarriageCarrierCode : "No_Data",
                     //CarrierName = !string.IsNullOrEmpty(r.MainCarriageCarrierName) ? r.MainCarriageCarrierName : "No_Data",
                     CarrierNumber = !string.IsNullOrEmpty(r.TruckNumber) ? r.TruckNumber : "No_Data",
                     ExpectedDate = r.MainCarriageETD,
                     ActualDate = r.MainCarriageATD,
                     DateFilterField = "DepartureArrivalFromDate",
                 }).ToList();

            // Inland Domestic got 1 leg only: Main
            // So no need to check for FinalDestination
            List<FlightSummary> inlandDomestic_ARR =
                (from r in iQuery
                 where r.DirectionId == "D"
                 && r.TransportModeId == "I"
                 && r.DepartureArrivalToDate >= firstDateTime
                 && r.DepartureArrivalToDate <= lastDateTime
                 select new FlightSummary()
                 {
                     Id = r.Id + ":INARR",
                     ShipmentId = r.Id,
                     DirectionId = r.DirectionId,
                     TransportModeId = r.TransportModeId,
                     CarrierId = !string.IsNullOrEmpty(r.MainCarriageCarrierId) ? r.MainCarriageCarrierId : "No_Data",
                     //CarrierCode = !string.IsNullOrEmpty(r.MainCarriageCarrierCode) ? r.MainCarriageCarrierCode : "No_Data",
                     //CarrierName = !string.IsNullOrEmpty(r.MainCarriageCarrierName) ? r.MainCarriageCarrierName : "No_Data",
                     CarrierNumber = !string.IsNullOrEmpty(r.TruckNumber) ? r.TruckNumber : "No_Data",
                     ExpectedDate = r.MainCarriageETA,
                     ActualDate = r.MainCarriageATA,
                     DateFilterField = "DepartureArrivalToDate",
                 }).ToList();


            myResult.AddRange(arrivals);
            myResult.AddRange(departures);
            myResult.AddRange(inlandDomestic_DEP);
            myResult.AddRange(inlandDomestic_ARR);

            if (myResult.Count > 0)
            {
                List<string> allCarriersIds = (from d in myResult where d.CarrierId != null group d by d.CarrierId into g select g.Key).ToList();

                ICommonDataContext myCommonContext = CommonDataContext.GetContext(tenant);

                List<Card> allCarriers = (from d in myCommonContext.Cards
                                          where d.Tenant == tenant
                                          && allCarriersIds.Contains(d.Id)
                                          select d).ToList();

                foreach (FlightSummary item in myResult)
                {
                    item.ActualDateCode = this.GetFlightSummaryDateCode(item.ActualDate, tenant);
                    item.ExpectedDateCode = this.GetFlightSummaryDateCode(item.ExpectedDate, tenant);

                    if (item.CarrierId != null)
                    {
                        Card myCarrier = allCarriers.Where(d => d.Id == item.CarrierId).FirstOrDefault();
                        if (myCarrier != null)
                        {
                            item.CarrierCode = myCarrier.Code;
                            item.CarrierName = myCarrier.EnglishName;
                        }

                        else
                        {
                            item.CarrierCode = "No_Data";
                            item.CarrierName = "No_Data";
                        }
                    }
                }
            }

            return myResult;
        }

        private string GetFlightSummaryDateCode(DateTime? date, int tenant)
        {
            string myResult = null;

            if (date != null)
            {
                DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                DateTime? yesterdayDate = todayDate.Value.AddDays(-1);
                DateTime? tomorrowDate = todayDate.Value.AddDays(1);
                DateTime? afterTomorrowDate = todayDate.Value.AddDays(2);

                DateTime? lastWeekDate = todayDate.Value.AddDays(-7);
                DateTime? nextWeekDate = todayDate.Value.AddDays(7);

                //queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageATD) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageATD) <= date2);
                if (date.Value.Date == todayDate)
                {
                    myResult = "TOD";
                }
                
                else if(date.Value.Date == tomorrowDate)
                {
                    myResult = "TOM";
                }

                else if (date.Value.Date >= lastWeekDate && date.Value.Date <= yesterdayDate)
                {
                    myResult = "LSW";
                }

                else if (date.Value.Date >= afterTomorrowDate && date.Value.Date <= nextWeekDate)
                {
                    myResult = "NXW";
                }
            }

            return myResult;
        }

        public EntityLastUpdatedByInfo GetLastUpdatedByInfo(string shipmentId, int tenant)
        {
            Shipment shipment = (from a in repository.context.Shipments
                                 where a.Id == shipmentId && a.Tenant == tenant
                                 select a).FirstOrDefault();

            EntityLastUpdatedByInfo info = new EntityLastUpdatedByInfo()
            {
                UpdateDate = shipment.LastUpdateDate != null ? shipment.LastUpdateDate : TenantServerConfigration.GetCurrentDateTime(tenant),
                UserName = shipment.UpdatedByUser.Contact.EnglishName,

            };
            return info;
        }

        public InvoiceEntityFields GetInvoiceEntityFields(string entityId, int tenant)
        {
            InvoiceEntityFields result =
                (from record in repository.context.Shipments
                 where record.Id == entityId && record.Tenant == tenant
                 select new InvoiceEntityFields()
                 {
                     Id = record.Id,
                     EntityLevelCode = record.ShipmentLevelCode,
                     EntityTransportModeId = record.TransportModeId
                 }).FirstOrDefault();

            return result;
        }

        public List<ShipmentList> GetLastActivityShipments(int tenant, string userId, string objectTableId)
        {
            EntityLastActivityRepository entityLastActivityRepository = new EntityLastActivityRepository(tenant);
            List<EntityLastActivity> lastActivities = entityLastActivityRepository.GetTopEntityLastActivities(tenant, userId, objectTableId).ToList();

            List<string> ids = new List<string>();
            foreach (EntityLastActivity activity in lastActivities)
            {
                ids.Add(activity.EntityId);
            }

            List<ShipmentDataView> shipments = repository.GetShipmentsFromIdList(ids, tenant);
            List<ShipmentList> shipmetnlists = new List<ShipmentList>();

            shipments = BranchPermitionsFilter.AddUserBranchRestrictionFilters<ShipmentDataView>(new QueryOperations(), shipments.AsQueryable<ShipmentDataView>(), tenant).ToList();
            shipments = ProductPermitionsFilter.AddUserProductRestrictionFilters<ShipmentDataView>(new QueryOperations(), shipments.AsQueryable<ShipmentDataView>(), tenant).ToList();

            shipmetnlists = BranchPermitionsFilter.AddUserBranchRestrictionFilters<ShipmentList>(new QueryOperations(), shipmetnlists.AsQueryable<ShipmentList>(), tenant).ToList();
            shipmetnlists = ProductPermitionsFilter.AddUserProductRestrictionFilters<ShipmentList>(new QueryOperations(), shipmetnlists.AsQueryable<ShipmentList>(), tenant).ToList();

            foreach (EntityLastActivity activity in lastActivities)
            {
                ShipmentDataView view = shipments.Where(d => d.Id == activity.EntityId).FirstOrDefault();
                if (view != null)
                {
                    ShipmentList list = new ShipmentList()
                    {
                        ShipmentViewId = view.Id + activity.Id,
                        Id = view.Id,
                        DirectionId = view.DirectionId,
                        DirectionName = view.DirectionName,
                        TransportModeName = view.TransportModeName,
                        ShipmentNumber = view.ShipmentNumber,
                        ShipmentTypeId = view.ShipmentTypeId,
                        ShipmentType = !string.IsNullOrEmpty(view.ShipmentTypeName) ? view.ShipmentTypeName + " " + view.ShipmentLevelName : view.ShipmentLevelName,
                        TransportModeId = view.TransportModeId,
                        CustomerName = view.CustomerName,
                        AgentName = view.AgentName,
                        ShipmentLevelCode = view.ShipmentLevelCode,
                        MasterShipmentDataId = view.MasterShipmentDataId,
                        ActivityDate = activity.ActivityDate,
                        ActivityTypeName = activity.ActivityType.Name,
                        ActivityByUserName = activity.User.Contact.EnglishName,
                        Routing = view.Routing,
                        House = view.House,
                        Master = view.Master,
                        MainCarriageCarrierName = view.MainCarriageCarrierName,
                        BranchId = view.BranchId,
                        TEU = view.TEU,
                        LastFSRStatusRequestDate = view.LastFSRStatusRequestDate,
                        ARInvoiceIssued = view.ARInvoiceIssued,
                        CreditNoteIssued = view.CreditNoteIssued,
                        ProductCode = view.ProductCode,
                        PackagesQuantity = view.PackagesQuantity,
                        IsAccountingClosed = view.IsAccountingClosed,
                        IsOperationalClosed = view.IsOperationalClosed,
                        AccountingCloseDate = view.AccountingCloseDate,
                        OperationalCloseDate = view.AccountingCloseDate,
                        FHLStatusCode = view.FHLStatusCode,
                        FHLStatusName = view.FHLStatusName,
                        FHLStatusDate = view.FHLStatusDate,
                        FWBStatusCode = view.FWBStatusCode,
                        FWBStatusName = view.FWBStatusName,
                        FWBStatusDate = view.FWBStatusDate,
                        LocalCustomsTransmissionsStatusCode = view.LocalCustomsTransmissionsStatusCode,
                        LocalCustomsTransmissionsStatusName = view.LocalCustomsTransmissionsStatusName,
                        LocalCustomsTransmissionsStatusError = view.LocalCustomsTransmissionsStatusError,
                        LocalCustomsTransmissionsStatusDate = view.LocalCustomsTransmissionsStatusDate,
                        LocalCustomsSentByUserId = view.LocalCustomsSentByUserId,
                        LocalCustomsSentByUserName = view.LocalCustomsSentByUserName,
                        CargonautFHLStatusCode = view.CargonautFHLStatusCode,
                        CargonautFHLStatusName = view.CargonautFHLStatusName,
                        CargonautFHLStatusDate = view.CargonautFHLStatusDate,
                        CargonautFWBStatusCode = view.CargonautFWBStatusCode,
                        CargonautFWBStatusName = view.CargonautFWBStatusName,
                        CargonautFWBStatusDate = view.CargonautFWBStatusDate,
                        NumberOfInsidePackages = view.NumberOfInsidePackages,
                        NumberOfInsidePackagesDetails = view.NumberOfInsidePackagesDetails,
                        ManifestReason = view.ManifestReason,
                        ManifestStatusCode = view.ManifestStatusCode,
                        AirlinePrefix = view.AirlinePrefix,
                        CustomConnectToShipment = view.CustomConnectToShipment,
                        ComputedStatusDate = view.ComputedStatusDate,
                        ComputedStatusId = view.ComputedStatusId,
                        ForeignPartnerCountryCode = view.ForeignPartnerCountryCode,
                        StatusId = view.MasterShipmentDataId != null ? (view.ShipmentMasterDataStatusWeight > view.ShipmentStatusWeight ? view.ShipmentMasterDataStatusId : view.ShipmentStatusId) : (view.ShipmentStatusId),
                        StatusDate = view.MasterShipmentDataId != null ? (view.ShipmentMasterDataStatusWeight > view.ShipmentStatusWeight ? view.ShipmentMasterDataStatusDate : view.ShipmentStatusDate) : (view.ShipmentStatusDate),
                        StatusName = view.MasterShipmentDataId != null ? (view.ShipmentMasterDataStatusWeight > view.ShipmentStatusWeight ? view.ShipmentMasterDataStatusName : view.ShipmentStatusName) : (view.ShipmentStatusName),
                        StatusLocation = view.MasterShipmentDataId != null ? (view.ShipmentMasterDataStatusWeight > view.ShipmentStatusWeight ? view.ShipmentMasterDataStatusLocation : view.ShipmentStatusLocation) : (view.ShipmentStatusLocation),
                        CustomsDeclarationNumber = view.CustomsDeclarationNumber,
                        MainCarriageVesselId = view.MainCarriageVesselId,
                        MainCarriageVesselName = view.MainCarriageVesselName,
                        BookingConfirmationNumber = view.BookingConfirmationNumber,
                        DescriptionOfGoods = view.DescriptionOfGoods,
                        IsNewARInvoiceBlocked = view.IsNewARInvoiceBlocked,
                        OperationalDate = view.OperationalDate,
                        NumberOfHouses = view.NumberOfHouses,
                        ValueOfGoods = view.ValueOfGoods,
                        WarehouseLegWarehouseId = view.WarehouseLegWarehouseId,
                        WarehouseLegAddressId = view.WarehouseLegAddressId,
                        WarehouseLegTerminalCode = view.WarehouseLegTerminalCode,
                        WarehouseLegExpectedEntryDate = view.WarehouseLegExpectedEntryDate,
                        WarehouseLegActualEntryDate = view.WarehouseLegActualEntryDate,
                        WarehouseLegExpectedReleaseDate = view.WarehouseLegExpectedReleaseDate,
                        WarehouseLegActualReleaseDate = view.WarehouseLegActualReleaseDate,
                        WarehouseLegLastFreeDate = view.WarehouseLegLastFreeDate,
                        WarehouseLegRemarks = view.WarehouseLegRemarks,
                        WarehouseLegReference = view.WarehouseLegReference,
                        WarehouseLegTerminalName = view.WarehouseLegTerminalName,
                        WarehouseLegEntryDate = view.WarehouseLegActualEntryDate != null ? view.WarehouseLegActualEntryDate : view.WarehouseLegExpectedEntryDate,
                        WarehouseLegReleaseDate = view.WarehouseLegActualReleaseDate != null ? view.WarehouseLegActualReleaseDate : view.WarehouseLegExpectedReleaseDate,
                        RegistryDate = view.RegistryDate,
                        IsAssembly = view.IsAssembly,
                        LastSharedEventId = view.LastSharedEventId,
                        LastSharedEventName = view.LastSharedEventName,
                        LastSharedEventLocation = view.LastSharedEventLocation,
                        LastSharedEventNotes = view.LastSharedEventNotes,
                        LastSharedEventDate = view.LastSharedEventDate,
                        FirstOperationalCloseDate = view.FirstOperationalCloseDate,
                        FirstAccountingCloseDate = view.FirstAccountingCloseDate,
                        DeclarationNumber = view.DeclarationNumber,
                        CustomsClearanceDate = view.CustomsClearanceDate,
                        IncludesCustoms = view.IncludesCustoms,
                        DeclarationDate = view.DeclarationDate,
                        IsDangerous = view.IsDangerous,
                        DangerousUnNumber = view.DangerousUnNumber,
                        IsStandalonePickupDelivery = view.IsStandalonePickupDelivery,
                    };

                    list.LongMaster = EntityFieldsHelper.GetLongMasterField(view);

                    shipmetnlists.Add(list);
                }
            }
            return shipmetnlists;
        }

        public IQueryable<ShipmentJoinPackageList> GetShipmentsJoinPackagesByTenant(int tenant)
        {
            IQueryable<ShipmentJoinPackageList> dataList =
                (from shipment in repository.context.Shipments.Include("Direction").Include("ShipmentLevel").Include("TransportMode").Include("CustomerCard").Include("ShipperCard").Include("ShipmentType").Include("OnCarriageToPort")
                 join shipmentPackage in repository.context.ShipmentPackages.Include("PackageType")
                 on shipment.Id equals shipmentPackage.ShipmentId into JoinedData
                 join sm in repository.context.ShipmentMasterDatas.Include("MainCarriageFromPort").Include("MainCarriageToPort").Include("MainCarriageFinalDestinationPort").Include("MainCarriageCarrierCard").Include("Transshipment1ToPort").Include("Transshipment2ToPort").Include("Transshipment3ToPort")
                 on shipment.MasterShipmentDataId equals sm.Id into shipmentJoin
                 from jd in JoinedData.DefaultIfEmpty()
                 from m in shipmentJoin.DefaultIfEmpty()
                 where shipment.Tenant == tenant && shipment.TransportModeId == "O" //&& (shipment.ShipmentLevelCode == "D" || shipment.ShipmentLevelCode == "C")
                 select new ShipmentJoinPackageList()
                 {
                     Id = shipment.Id + (!string.IsNullOrEmpty(jd.Id) ? jd.Id : ""),
                     ShipmentId = shipment.Id,
                     PackageId = jd.Id,
                     DirectionId = shipment.DirectionId,
                     ShipmentNumber = shipment.ShipmentNumber,
                     CreateDateTime = shipment.CreateDateTime,
                     MainCarriageETD = m.MainCarriageETD,
                     CustomerId = shipment.CustomerId,
                     CustomerName = shipment.CustomerCard != null ? shipment.CustomerCard.EnglishName : null,
                     AgentName = shipment.AgentCard != null ? shipment.AgentCard.EnglishName : null,
                     MainCarriageFromPortName = m.MainCarriageFromPort.EnglishName,
                     MainCarriageFinalDestinationPortName = m.MainCarriageFinalDestinationPort != null ? m.MainCarriageFinalDestinationPort.EnglishName : null,
                     ShipperName = shipment.ShipperCard != null ? shipment.ShipperCard.EnglishName : null,
                     MainCarriageCarrierName = m.MainCarriageCarrierCard != null ? m.MainCarriageCarrierCard.EnglishName : null,                    
                     ContainerNumber = jd.ContainerNumber,
                     ShipmentTypeId = shipment.ShipmentTypeId,
                     ShipmentTypeName = shipment.ShipmentType != null ? shipment.ShipmentType.Name : null,
                     MainCarriageFinalDestinationETA = m.MainCarriageFinalDestinationETA,
                     MasterNumber = m.Master,
                     Voyage = m.MainCarriageCarrierNumber,
                     StatusName = shipment.EntityStatus.Name,
                     AgentReference1 = shipment.AgentReference1,
                     AgentReference2 = shipment.AgentReference2,
                     CustomerReference1 = shipment.CustomerReference1,
                     CustomerReference2 = shipment.CustomerReference2,
                     ContainerCode = jd.PackageType != null ? jd.PackageType.PrintAs : null,
                     TransportModeId = shipment.TransportModeId,
                     MainCarriageCarrierPrefix = m.MainCarriageCarrierPrefix,
                     AgentId = shipment.AgentId,
                     ShipmentLevelCode = shipment.ShipmentLevelCode,
                     VesselId = m.MainCarriageVesselId,
                     Field1 = shipment.Field1,
                     Field2 = shipment.Field2,
                     Field3 = shipment.Field3,
                     Field4 = shipment.Field4,
                     Field5 = shipment.Field5,
                     Field6 = shipment.Field6,
                     Field7 = shipment.Field7,
                     Field8 = shipment.Field8,
                     Field9 = shipment.Field9,
                     Field10 = shipment.Field10,
                     Field11 = shipment.Field11,
                     Field12 = shipment.Field12,
                     Field13 = shipment.Field13,
                     Field14 = shipment.Field14,
                     Field15 = shipment.Field15,
                     Field16 = shipment.Field16,
                     Field17 = shipment.Field17,
                     Field18 = shipment.Field18,
                     Field19 = shipment.Field19,
                     Field20 = shipment.Field20,
                     Field21 = shipment.Field21,
                     Field22 = shipment.Field22,
                     Field23 = shipment.Field23,
                     Field24 = shipment.Field24,
                     Field25 = shipment.Field25,
                     Field26 = shipment.Field26,
                     Field27 = shipment.Field27,
                     Field28 = shipment.Field28,
                     Field29 = shipment.Field29,
                     Field30 = shipment.Field30,
                     Field31 = shipment.Field31,
                     Field32 = shipment.Field32,
                     Field33 = shipment.Field33,
                     Field34 = shipment.Field34,
                     Field35 = shipment.Field35,
                     Field36 = shipment.Field36,
                     Field37 = shipment.Field37,
                     Field38 = shipment.Field38,
                     Field39 = shipment.Field39,
                     Field40 = shipment.Field40,
                     IsCancelled = shipment.IsCancelled,
                     DescriptionofGoods = shipment.DescriptionOfGoods,
                     House = shipment.House,
                     ConsigneeName = shipment.ConsigneeName,
                     ShipmentPackageReference1 = jd.Reference1,
                     ShipmentPackageReference2 = jd.Reference2,
                     ShipmentPackageReference3 = jd.Reference3,
                     ShipmentPackageReference4 = jd.Reference4,
                     ContainerTypeName = jd.PackageType != null ? jd.PackageType.EnglishName : null,
                     OnCarriageToPortId = m.OnCarriageToPortId,
                     OnCarriageTo = m.OnCarriageToPort != null ? m.OnCarriageToPort.EnglishName : null,
                     OnCarriageToPortCode = m.OnCarriageToPort != null ? m.OnCarriageToPort.Code : null,
                     ATA = m.MainCarriageATA,
                     ATD = m.MainCarriageATD,
                     OnCarriageATD = m.OnCarriageATD,
                     OnCarriageATA = m.OnCarriageATA,
                     OnCarriageETA = m.OnCarriageETA,
                     ContainerNotes = jd.Notes,
                     MainCarriageToPortId = m.MainCarriageToPortId,
                     Transshipment1ToPortId = m.Transshipment1ToPortId,
                     Transshipment2ToPortId = m.Transshipment2ToPortId,
                     Transshipment3ToPortId = m.Transshipment3ToPortId,
                     MainCarriageToPortName = m.MainCarriageToPort != null ? m.MainCarriageToPort.EnglishName : null,
                     Transshipment1ToPortName = m.Transshipment1ToPort != null ? m.Transshipment1ToPort.EnglishName : null,
                     Transshipment2ToPortName = m.Transshipment2ToPort != null ? m.Transshipment2ToPort.EnglishName : null,
                     Transshipment3ToPortName = m.Transshipment3ToPort != null ? m.Transshipment3ToPort.EnglishName : null,
                     PackagesGrossWeight = jd.Weight,
                     ContainerFollowUp = jd.IsDeliveryFU,
                     SplitOnCarriage = m.SplitOnCarriage,
                     PackageOnCarriageATA = jd.OnCarriageATA,
                     PackageOnCarriageATD = jd.OnCarriageATD,
                     PackageOnCarriageETA = jd.OnCarriageETA,
                     PackageDliveryId = jd.DeliveryId,
                     MainCarriageFromPortId = m.MainCarriageFromPortId,
                     Transshipment1FromPortId = m.Transshipment1FromPortId,
                     Transshipment2FromPortId = m.Transshipment2FromPortId,
                     Transshipment3FromPortId = m.Transshipment3FromPortId,
                     MainCarriageATA = m.MainCarriageATA,
                     MainCarriageETA = m.MainCarriageETA,
                     Transshipment1ATA = m.Transshipment1ATA,
                     Transshipment1ETA = m.Transshipment1ETA,
                     Transshipment2ATA = m.Transshipment2ATA,
                     Transshipment2ETA = m.Transshipment2ETA,
                     Transshipment3ATA = m.Transshipment3ATA,
                     Transshipment3ETA = m.Transshipment3ETA,
                     MainCarriageVesselId = m.MainCarriageVesselId,
                     Transshipment1VesselId = m.Transshipment1VesselId,
                     Transshipment2VesselId = m.Transshipment2VesselId,
                     Transshipment3VesselId = m.Transshipment3VesselId,
                     BookingConfirmationNumber = m.BookingConfirmationNumber,
                     IncotermId = shipment.IncotermId,
                     ShipperAddressId = shipment.ShipperAddressId,
                     ConsigneeAddressId = shipment.ConsigneeAddressId,
                     Volume = shipment.Volume,
                     PackageVolume = jd.Volume,
                     MainCarriageCarrierId = m.MainCarriageCarrierId,
                     NumberOfContainers = shipment.NumberOfContainers,
                     OnForwardingTo = shipment.OnForwardingToPort != null ? shipment.OnForwardingToPort.EnglishName : null,
                     OnForwardingATD = shipment.OnForwardingATD,
                     OnForwardingATA = shipment.OnForwardingATA,
                     OnForwardingETA = shipment.OnForwardingETA,
                     OnForwardingToPortCode = shipment.OnForwardingToPort != null ? shipment.OnForwardingToPort.Code : null,
                     OnForwardingToPortId = shipment.OnForwardingToPortId,
                 });

            return dataList;
        }

        public List<ShipmentPM> GetShipmentsForUnpaidInvoicesReport(List<string> shipmentIds)
        {
            IQueryable<Shipment> shipments = repository.GetShipmentsForUnpaidInvoicesReport(shipmentIds);
            List<ShipmentPM> shipmentPMs = (from a in shipments
                                            select new ShipmentPM()
                                            {
                                                Id = a.Id,
                                                TransportModeId = a.TransportModeId,
                                                MainCarriageCarrierPrefix = a.ShipmentMasterData == null ? "" : a.ShipmentMasterData.MainCarriageCarrierPrefix,
                                                Master = a.ShipmentMasterData == null ? "" : a.ShipmentMasterData.Master,
                                                AirlinePrefix = a.ShipmentMasterData == null ? "" : a.ShipmentMasterData.AirlinePrefix,
                                                ShipmentNumber = a.ShipmentNumber,
                                                DescriptionOfGoods = a.DescriptionOfGoods,
                                                ShipperReference1 = a.ShipperReference1,
                                                ShipperReference2 = a.ShipperReference2,
                                                ShipperName = a.ShipperCard != null ? a.ShipperCard.EnglishName : null,
                                                DirectionId = a.DirectionId,
                                                MainCarriageFromPortCode = a.ShipmentMasterData == null ? null : (a.ShipmentMasterData.MainCarriageFromPort == null ? null : a.ShipmentMasterData.MainCarriageFromPort.Code),
                                                MainCarriageFinalDestinationPortCode = a.ShipmentMasterData == null ? null : (a.ShipmentMasterData.MainCarriageFinalDestinationPort == null ? null : a.ShipmentMasterData.MainCarriageFinalDestinationPort.Code),
                                                ToPartnerCity = a.ShipmentMasterData == null ? null : (a.ShipmentMasterData.ToPartnerAddress == null ? null : a.ShipmentMasterData.ToPartnerAddress.City),
                                                ToPort = a.ToPort == null ? "" : a.ToPort.Code,
                                                ShipmentLevelCode = a.ShipmentLevelCode,
                                            }).ToList();
            return shipmentPMs;
        }

        public List<FlightSummary> GetShipmentsDashBoardDeparturesArrivalsFroMobile(int tenant, string transportModeId, string directionId, IQueryable<ShipmentList> iQuery)
        {
            List<FlightSummary> result = new List<FlightSummary>();
            DateTime? currentDateTime = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime? firstDateTime = currentDateTime.Value.AddDays(-7).Date;
            DateTime? lastDateTime = currentDateTime.Value.AddDays(7).Date;

            iQuery = BranchPermitionsFilter.AddUserBranchRestrictionFilters<ShipmentList>(new QueryOperations(), iQuery, tenant);
            iQuery = ProductPermitionsFilter.AddUserProductRestrictionFilters<ShipmentList>(new QueryOperations(), iQuery, tenant);

            if (iQuery != null)
            {
                IQueryable<ShipmentList> iQueryTransportMode = string.IsNullOrEmpty(transportModeId) ? iQuery : iQuery.Where(d => d.TransportModeId == transportModeId);
                if (iQueryTransportMode != null)
                {

                    IQueryable<ShipmentList> iQueryableShipments;
                    if (string.IsNullOrEmpty(directionId))
                    {
                        iQueryableShipments = iQueryTransportMode;
                    }
                    else if (directionId == "I")
                    {
                        iQueryableShipments = iQueryTransportMode.Where(d => d.DirectionId == "I" || d.DirectionId == "C");
                    }
                    else
                    {
                        iQueryableShipments = iQueryTransportMode.Where(d => d.DirectionId == directionId);
                    }



                    if (iQueryableShipments != null)
                    {
                        List<FlightSummary> arrivals =
                            (from r in iQueryableShipments
                             where r.DirectionId == "I" || r.DirectionId == "C"
                             && (r.MainCarriageETA >= firstDateTime || r.MainCarriageATA >= firstDateTime)
                             && (r.MainCarriageETA <= lastDateTime || r.MainCarriageATA <= lastDateTime)
                             select new FlightSummary()
                             {
                                 Id = r.Id,
                                 DirectionId = r.DirectionId,
                                 TransportModeId = r.TransportModeId,
                                 CarrierId = !string.IsNullOrEmpty(r.MainCarriageCarrierId) ? r.MainCarriageCarrierId : "No_Data",
                                 CarrierCode = !string.IsNullOrEmpty(r.MainCarriageCarrierCode) ? r.MainCarriageCarrierCode : "No_Data",
                                 CarrierName = !string.IsNullOrEmpty(r.MainCarriageCarrierName) ? r.MainCarriageCarrierName : "No_Data",
                                 CarrierNumber = !string.IsNullOrEmpty(r.MainCarriageCarrierNumber) ? r.MainCarriageCarrierNumber : "No_Data",
                                 ExpectedDate = r.MainCarriageETA,
                                 ActualDate = r.MainCarriageATA,
                                 MainCarriageATA = r.MainCarriageATA,
                                 ComputedStatusId = r.ComputedStatusId

                             }).ToList();


                        List<FlightSummary> departures =
                            (from r in iQueryableShipments
                             where r.DirectionId == "E" || r.DirectionId == "R" || (r.DirectionId == "D" && r.TransportModeId != "I")

                             && (r.MainCarriageETD >= firstDateTime || r.MainCarriageATD >= firstDateTime)
                             && (r.MainCarriageETD <= lastDateTime || r.MainCarriageATD <= lastDateTime)
                             select new FlightSummary()
                             {
                                 Id = r.Id,
                                 DirectionId = r.DirectionId,
                                 TransportModeId = r.TransportModeId,
                                 CarrierId = !string.IsNullOrEmpty(r.MainCarriageCarrierId) ? r.MainCarriageCarrierId : "No_Data",
                                 CarrierCode = !string.IsNullOrEmpty(r.MainCarriageCarrierCode) ? r.MainCarriageCarrierCode : "No_Data",
                                 CarrierName = !string.IsNullOrEmpty(r.MainCarriageCarrierName) ? r.MainCarriageCarrierName : "No_Data",
                                 CarrierNumber = !string.IsNullOrEmpty(r.MainCarriageCarrierNumber) ? r.MainCarriageCarrierNumber : "No_Data",
                                 ExpectedDate = r.MainCarriageETD,
                                 ActualDate = r.MainCarriageATD,
                                 MainCarriageATA = r.MainCarriageATA,
                                 ComputedStatusId = r.ComputedStatusId
                             }).ToList();


                        List<FlightSummary> domestics =
                            (from r in iQueryableShipments
                             where (r.DirectionId == "D" && r.TransportModeId == "I")

                             && (r.MainCarriageETD >= firstDateTime || r.MainCarriageATD >= firstDateTime)
                             && (r.MainCarriageETD <= lastDateTime || r.MainCarriageATD <= lastDateTime)
                             select new FlightSummary()
                             {
                                 Id = r.Id,
                                 DirectionId = r.DirectionId,
                                 TransportModeId = r.TransportModeId,
                                 CarrierId = !string.IsNullOrEmpty(r.MainCarriageCarrierId) ? r.MainCarriageCarrierId : "No_Data",
                                 CarrierCode = !string.IsNullOrEmpty(r.MainCarriageCarrierCode) ? r.MainCarriageCarrierCode : "No_Data",
                                 CarrierName = !string.IsNullOrEmpty(r.MainCarriageCarrierName) ? r.MainCarriageCarrierName : "No_Data",
                                 CarrierNumber = !string.IsNullOrEmpty(r.TruckNumber) ? r.TruckNumber : "No_Data",
                                 ExpectedDate = r.MainCarriageETD,
                                 ActualDate = r.MainCarriageATD,
                                 MainCarriageATA = r.MainCarriageATA,
                                 ComputedStatusId = r.ComputedStatusId

                             }).ToList();

                        result.AddRange(domestics);
                        result.AddRange(departures);
                        result.AddRange(arrivals);
                    }
                }
            }


            return result;

        }

        public ImporterQueriesDataCounts GetShipmentsQueriesCounts(ShipmentsQueriesCountsArgs shipmentsQueriesCountsArgs)
        {
            ImporterQueriesDataCounts myResult = new ImporterQueriesDataCounts() { Id = 1 };
            IShipmentDataViewContext dataViewContext = ShipmentDataViewContext.GetContext(shipmentsQueriesCountsArgs.Tenant);
            IQueryable<ShipmentDataView> allShipments = (from f in dataViewContext.ShipmentDataViews where f.Tenant == shipmentsQueriesCountsArgs.Tenant && f.IsCancelled == false select f);
            allShipments = BranchPermitionsFilter.AddUserBranchRestrictionFilters<ShipmentDataView>(new QueryOperations(), allShipments, shipmentsQueriesCountsArgs.Tenant);
            allShipments = ProductPermitionsFilter.AddUserProductRestrictionFilters<ShipmentDataView>(new QueryOperations(), allShipments, shipmentsQueriesCountsArgs.Tenant);
            allShipments = GetAllFliteredShipments(allShipments, shipmentsQueriesCountsArgs);

            myResult.AllShipmentsCount = allShipments.Take(1001).Count();
            MapOpenShipmentsCount(myResult, allShipments);
            MapArchivedShipmentsCount(myResult, allShipments);
            MapMissingDocsCount(myResult, allShipments);
            MapAgentShipmentsCount(myResult, allShipments);
            MapImporterShipmentsCount(myResult, allShipments);
            MapRequestedDocsCount(myResult, allShipments);
            MapRequiredActionsCount(myResult, allShipments);

            return myResult;
        }

        private void MapRequiredActionsCount(ImporterQueriesDataCounts myResult, IQueryable<ShipmentDataView> allShipments)
        {
            IQueryable<ShipmentDataView> allReqActionsShipments = allShipments.Where(d => d.IsRequestedDocuments || d.RequestedDocumentsCount > 0 || d.IsDigitalSignRequired == true || d.IsDepositionRequired == true || (d.IsImporterApprovalRequried == true && string.IsNullOrEmpty(d.ApprovedByUserName)));
            if (allReqActionsShipments != null)
            {
                myResult.RequiredActionsCount = (from r in allReqActionsShipments select r).Take(1001).Count();
            }
        }

        private void MapRequestedDocsCount(ImporterQueriesDataCounts myResult, IQueryable<ShipmentDataView> allShipments)
        {
            IQueryable<ShipmentDataView> allReqDocsShipments = allShipments.Where(d => d.IsRequestedDocuments == true || d.RequestedDocumentsCount > 0 || d.IsDigitalSignRequired == true || d.IsDepositionRequired == true || (d.IsImporterApprovalRequried == true && string.IsNullOrEmpty(d.ApprovedByUserName)));
            if (allReqDocsShipments != null)
            {
                myResult.RequestedDocsCount = (from r in allReqDocsShipments select r).Take(1001).Count();
            }
        }

        private void MapImporterShipmentsCount(ImporterQueriesDataCounts myResult, IQueryable<ShipmentDataView> allShipments)
        {
            IQueryable<ShipmentDataView> allImporterShipments = allShipments.Where(d => d.ForwarderShipmentNumber == null || d.ForwarderShipmentNumber == string.Empty);
            if (allImporterShipments != null)
            {
                myResult.ImporterShipmentsCount = (from r in allImporterShipments select r).Take(1001).Count();
            }
        }

        private void MapAgentShipmentsCount(ImporterQueriesDataCounts myResult, IQueryable<ShipmentDataView> allShipments)
        {
            IQueryable<ShipmentDataView> allAgentShipments = allShipments.Where(d => d.ForwarderShipmentNumber != null && d.ForwarderShipmentNumber != string.Empty);
            if (allAgentShipments != null)
            {
                myResult.AgentShipmentsCount = (from r in allAgentShipments select r).Take(1001).Count();
            }
        }

        private void MapMissingDocsCount(ImporterQueriesDataCounts myResult, IQueryable<ShipmentDataView> allShipments)
        {
            IQueryable<ShipmentDataView> allMissingDocs = allShipments.Where(d => d.IsMissingDocument == true);
            if (allMissingDocs != null)
            {
                myResult.MissingDocsCount = (from r in allMissingDocs select r).Take(1001).Count();
            }
        }

        private void MapArchivedShipmentsCount(ImporterQueriesDataCounts myResult, IQueryable<ShipmentDataView> allShipments)
        {
            IQueryable<ShipmentDataView> allClosedShipments = allShipments.Where(d => d.IsOperationalClosed == true);
            if (allClosedShipments != null)
            {
                myResult.ArchivedShipmentsCount = (from r in allClosedShipments select r).Take(1001).Count();
            }
        }

        private void MapOpenShipmentsCount(ImporterQueriesDataCounts myResult, IQueryable<ShipmentDataView> allShipments)
        {
            IQueryable<ShipmentDataView> allOpenShipments = allShipments.Where(d => d.IsOperationalClosed == false);
            if (allOpenShipments != null)
            {
                myResult.OpenShipmentsCount = (from r in allOpenShipments select r).Take(1001).Count();
            }
        }

        private IQueryable<ShipmentDataView> GetAllFliteredShipments(IQueryable<ShipmentDataView> allShipmentsDataViews, ShipmentsQueriesCountsArgs shipmentsQueriesCountsArgs)
        {
            IQueryable <ShipmentDataView> allShipments = allShipmentsDataViews;
            if (!string.IsNullOrEmpty(shipmentsQueriesCountsArgs.ForwarderPartnerId))
            {
                allShipments = allShipments.Where(d => d.ForwarderPartnerId == shipmentsQueriesCountsArgs.ForwarderPartnerId);
            }
            if (!string.IsNullOrEmpty(shipmentsQueriesCountsArgs.TransportModeId))
            {
                allShipments = allShipments.Where(d => d.TransportModeId == shipmentsQueriesCountsArgs.TransportModeId);
            }
            if (!string.IsNullOrEmpty(shipmentsQueriesCountsArgs.DirectionId))
            {
                allShipments = allShipments.Where(d => d.DirectionId == shipmentsQueriesCountsArgs.DirectionId);
            }
            if (!string.IsNullOrEmpty(shipmentsQueriesCountsArgs.SearchFilter))
            {
                allShipments = allShipments.Where(d => (d.SearchFields.ToUpper().Contains(shipmentsQueriesCountsArgs.SearchFilter.ToUpper()) || d.DocumentsSearchFields.ToUpper().Contains(shipmentsQueriesCountsArgs.SearchFilter.ToUpper())));
            }
            if (!string.IsNullOrEmpty(shipmentsQueriesCountsArgs.TypeCode))
            {
                if (shipmentsQueriesCountsArgs.TypeCode.ToUpper() == "O")
                {
                    allShipments = allShipments.Where(d => (d.IsOperationalClosed == false));
                }
                else
                {
                    allShipments = allShipments.Where(d => (d.IsOperationalClosed == true));
                }
            }

            return allShipments;
        }

        public List<string> GetShipmentsByTenantCreateDateCustomer(int tenant, DateTime StartDate, DateTime EndDate, string CustomerId, string CustomerTenantAccessId)
        {
            IQueryable<Shipment> shipments = repository.GetByCreateDate(StartDate, EndDate);
            CustomerTenantAccessCardBatchQuery BQuery = new CustomerTenantAccessCardBatchQuery(tenant);
            //var temp = BQuery.GetOldestCustomerTenantAccessCardsBatch(CustomerId, CustomerTenantAccessId, tenant);
            List<string> shipmentsIds = (from s in shipments
                                         where s.Tenant == tenant && s.CustomerId == CustomerId
                                         select s.Id).ToList();//s.CreateDateTime >= temp.FromDatetime &&
            return shipmentsIds;

        }

        public IQueryable<ShipmentList> GetShipmentListTenant(int tenant)
        {
            IQueryable<ShipmentList> shipmentsList = from s in repository.context.Shipments.Include("MainCarriageFromPort").Include("MainCarriageToPort").Include("MainCarriageFinalDestinationPort")
                                                     join sm in repository.context.ShipmentMasterDatas
                                                     on s.MasterShipmentDataId equals sm.Id into shipmentJoin
                                                     from m in shipmentJoin.DefaultIfEmpty()
                                                     where s.Tenant == tenant && s.ShipmentLevelCode != "C" && !s.IsCancelled
                                                     select new ShipmentList()
                                                     {
                                                         ProfitExchangeRate = s.ProfitExchangeRate,
                                                         OpenPayablesInLocalCurrency = s.OpenPayablesInLocalCurrency,
                                                         AccountedPayablesInLocalCurrency = s.AccountedPayablesInLocalCurrency,
                                                         OpenReceivablesInLocalCurrency = s.OpenReceivablesInLocalCurrency,
                                                         AccountedReceivablesInLocalCurrency = s.AccountedReceivablesInLocalCurrency,
                                                         ProfitInLocalCurrency = s.ProfitInLocalCurrency,
                                                         OpenPayablesInProfitCurrency = s.OpenPayablesInProfitCurrency,
                                                         AccountedPayablesInProfitCurrency = s.AccountedPayablesInProfitCurrency,
                                                         OpenReceivablesInProfitCurrency = s.OpenReceivablesInProfitCurrency,
                                                         AccountedReceivablesInProfitCurrency = s.AccountedReceivablesInProfitCurrency,
                                                         ProfitInProfitCurrency = s.ProfitInProfitCurrency,
                                                         AgentId = s.AgentId,
                                                         AgentComputed = s.AgentComputed,
                                                         ComputedShipmentNumber = s.ComputedShipmentNumber,
                                                         AgentName = s.AgentCard != null ? s.AgentCard.EnglishName : null,
                                                         AgentReference1 = s.AgentReference1,
                                                         AgentReference2 = s.AgentReference2,
                                                         BookingNumberOfPackages = s.BookingNumberOfPackages,
                                                         BranchId = s.BranchId,
                                                         ChargeableWeightInKG = s.ChargeableWeightInKG,
                                                         ConsigneeId = s.ConsigneeId,
                                                         ConsigneeName = s.ConsigneeCard != null ? s.ConsigneeCard.EnglishName : null,
                                                         ConsigneeReference1 = s.ConsigneeReference1,
                                                         ConsigneeReference2 = s.ConsigneeReference2,
                                                         CreateDateTime = s.CreateDateTime,
                                                         AWBCurrencyCode = s.AWBCurrency != null ? s.AWBCurrency.Code : null,
                                                         CustomerId = s.CustomerId,
                                                         CustomerName = s.CustomerCard != null ? s.CustomerCard.EnglishName : null,
                                                         CustomerReference1 = s.CustomerReference1,
                                                         CustomerReference2 = s.CustomerReference2,
                                                         DepartmentId = s.DepartmentId,
                                                         ChargeableWeightUnitCode = s.ChargeableWeightUnitCode,
                                                         DirectionId = s.DirectionId,
                                                         DirectionName = s.Direction.Name,
                                                         EstimateProfitInLocalCurrency = s.EstimateProfitInLocalCurrency,
                                                         EstimateProfitInProfitCurrency = s.EstimateProfitInProfitCurrency,
                                                         MainCarriageFinalDestinationETA = m.MainCarriageFinalDestinationETA,
                                                         MainCarriageFinalDestinationATA = m.MainCarriageFinalDestinationATA,
                                                         MainCarriageFinalDestinationPortCode = m.MainCarriageFinalDestinationPort == null ? null : m.MainCarriageFinalDestinationPort.Code,
                                                         FreightForwarderId = s.FreightForwarderId,
                                                         FreightForwarderName = s.FreightForwarderCard != null ? s.FreightForwarderCard.EnglishName : null,
                                                         FromPort = m.MainCarriageFromPort.Code,
                                                         FromPortCountry = m.MainCarriageFromPort.Country.Code,
                                                         FromPortName = m.MainCarriageFromPort.EnglishName,
                                                         GrossWeightInKG = s.GrossWeightInKG,
                                                         GrossWeightPerStorageDays = s.GrossWeightPerStorageDays,
                                                         GrossWeightPerTon = s.GrossWeightPerTon,
                                                         House = s.House,
                                                         Id = s.Id,
                                                         IncotermId = s.IncotermId,
                                                         IncotermCode = s.Incoterm != null ? s.Incoterm.Code : null,
                                                         IsAccountingClosed = s.IsAccountingClosed,
                                                         IsCancelled = s.IsCancelled,
                                                         CancelledDate = s.CancelledDate,
                                                         ShipmentLevelCode = s.ShipmentLevelCode,
                                                         IsOperationalClosed = s.IsOperationalClosed,
                                                         AccountingCloseDate = s.AccountingCloseDate,
                                                         OperationalCloseDate = s.AccountingCloseDate,
                                                         LastUpdateDate = s.LastUpdateDate,
                                                         MainCarriageATA = m.MainCarriageATA,
                                                         MainCarriageATD = m.MainCarriageATD,
                                                         MainCarriageCarrierCode = m.MainCarriageCarrierCard != null ? m.MainCarriageCarrierCard.Code : null,
                                                         MainCarriageCarrierId = m.MainCarriageCarrierId,
                                                         MainCarriageCarrierName = m.MainCarriageCarrierCard != null ? m.MainCarriageCarrierCard.EnglishName : null,
                                                         MainCarriageCarrierNumber = m.MainCarriageCarrierNumber,
                                                         MainCarriageETA = m.MainCarriageETA,
                                                         MainCarriageETD = m.MainCarriageETD,
                                                         MainCarriageFromPortCode = m.MainCarriageFromPort.Code,
                                                         MainCarriageFromPortCountryCode = m.MainCarriageFromPort.Country.Code,
                                                         MainCarriageFromPortCountryName = m.MainCarriageFromPort.Country.EnglishName,
                                                         MainCarriageFromPortId = m.MainCarriageFromPortId,
                                                         MainCarriageFromPortName = m.MainCarriageFromPort.EnglishName,
                                                         MainCarriageToPortCode = m.MainCarriageToPort.Code,
                                                         MainCarriageToPortCountryCode = m.MainCarriageToPort.Country.Code,
                                                         MainCarriageToPortCountryName = m.MainCarriageToPort.Country.EnglishName,
                                                         MainCarriageToPortName = m.MainCarriageToPort.EnglishName,
                                                         Master = m.Master,
                                                         NextETA = s.NextETA,
                                                         NextETD = s.NextETD,
                                                         NextLegCode = s.NextLegCode,
                                                         NextLegName = s.NextLeg != null ? s.NextLeg.Name : null,
                                                         PackagesQuantity = s.PackagesQuantity,
                                                         NumberOfContainers = s.NumberOfContainers,
                                                         NumberOfFollowUps = s.NumberOfFollowUps,
                                                         NumberOfPackages = s.NumberOfPackages,
                                                         PreCarriageETD = m.PreCarriageETD,
                                                         OrderChargeableWeight = s.OrderChargeableWeight,
                                                         OrderVolumetricWeight = s.OrderVolumetricWeight,
                                                         TransportModeId = s.TransportModeId,
                                                         Tenant = s.Tenant,
                                                         ToPort = m.MainCarriageToPort.Code,
                                                         ToPortCountry = m.MainCarriageToPort.Country.Code,
                                                         ToPortName = m.MainCarriageToPort.EnglishName,
                                                         TransportModeName = s.TransportMode.Name,
                                                         SalesmanUserId = s.SalesmanUserId,
                                                         AccountManagerUserId = s.AccountManagerUserId,
                                                         MasterShipmentDataId = s.MasterShipmentDataId,
                                                         ShipmentPayableStatusCode = s.ShipmentPayableStatusCode,
                                                         ShipmentPayableStatusName = s.ShipmentPayableStatus != null ? s.ShipmentPayableStatus.Name : null,
                                                         ShipmentNumber = s.ShipmentNumber,
                                                         ShipmentReceivableStatusCode = s.ShipmentReceivableStatusCode,
                                                         ShipmentReceivableStatusName = s.ShipmentReceivableStatus != null ? s.ShipmentReceivableStatus.Name : null,
                                                         ShipperId = s.ShipperId,
                                                         ShipperName = s.ShipperCard != null ? s.ShipperCard.EnglishName : null,
                                                         ShipperReference1 = s.ShipperReference1,
                                                         ShipperReference2 = s.ShipperReference2,
                                                         UpdatedByUserId = s.UpdatedByUserId,
                                                         VolumeInCBM = s.VolumeInCBM,
                                                         VolumetricWeight = s.VolumetricWeight,
                                                         QuoteId = s.QuoteId,
                                                         QuoteNumber = s.QuoteNumber,
                                                         MainCarriageFullCarrierNumber = (m.MainCarriageCarrierNumber != null && m.MainCarriageCarrierCard != null) ? m.MainCarriageCarrierCard.Code + m.MainCarriageCarrierNumber : null,
                                                         Transshipment1FullCarrierNumber = (m.Transshipment1CarrierNumber != null && m.Transshipment1CarrierPrefix != null) ? m.Transshipment1CarrierPrefix + m.Transshipment1CarrierNumber : null,
                                                         Transshipment2FullCarrierNumber = (m.Transshipment2CarrierNumber != null && m.Transshipment2CarrierPrefix != null) ? m.Transshipment2CarrierPrefix + m.Transshipment2CarrierNumber : null,
                                                         Transshipment3FullCarrierNumber = (m.Transshipment3CarrierNumber != null && m.Transshipment3CarrierPrefix != null) ? m.Transshipment3CarrierPrefix + m.Transshipment3CarrierNumber : null,
                                                         AsAgreedFreight = s.AsAgreedFreight,
                                                         AsAgreedOtherCharges = s.AsAgreedOtherCharges,
                                                         AccountNumber = s.AccountNumber,
                                                         FinalArrivalDate = s.FinalArrivalDate,
                                                         EstimatedFinalArrivalDate = s.EstimatedFinalArrivalDate,
                                                         ActualFinalArrivalDate = s.ActualFinalArrivalDate,
                                                         AMSBL = s.AMSBL,
                                                         CASSCode = s.CASSCode,
                                                         SLAC = s.SLAC,
                                                         FreelancerId = s.FreelancerId,
                                                         FreelancerAddressId = s.FreelancerAddressId,
                                                         FreelancerContactId = s.FreelancerContactId,
                                                         ARInvoiceIssued = s.ARInvoiceIssued,
                                                         CreditNoteIssued = s.CreditNoteIssued,
                                                         ProductCode = s.ProductCode,
                                                         LastStatusLogDate = s.LastStatusLogDate,
                                                         ExceptionDescription = s.ExceptionDescription,
                                                         ExceptionDate = s.ExceptionDate,
                                                         HasException = s.HasException,
                                                         ExceptionResolvedDescription = s.ExceptionResolvedDescription,
                                                         LastExceptionDescription = s.LastExceptionDescription,
                                                         CustomConnectToShipment = s.CustomConnectToShipment,
                                                         CustomsDeclarationNumber = s.CustomsDeclarationNumber,
                                                         NumberOfInsidePackages = s.NumberOfInsidePackages,
                                                         NumberOfInsidePackagesDetails = s.NumberOfInsidePackagesDetails,
                                                         ComputedStatusId = s.ComputedStatusId,
                                                         ComputedStatusDate = s.ComputedStatusDate,
                                                         StatusId = s.StatusId,
                                                         StatusName = s.EntityStatus != null ? s.EntityStatus.Name : null,
                                                         StatusDate = s.StatusDate,
                                                         StatusLocation = s.StatusLocation,
                                                         ForeignPartnerCountryCode = s.ForeignPartnerCountryCode,
                                                         DescriptionOfGoods = s.DescriptionOfGoods,
                                                         AgentSharedManifestRef = s.AgentSharedManifestRef,
                                                         IsManifestSentToAgent = s.IsManifestSentToAgent,
                                                         IsNewARInvoiceBlocked = s.IsNewARInvoiceBlocked,
                                                         OperationalDate = s.OperationalDate,
                                                         CutoffDate = m.CutoffDate,
                                                         ValueOfGoods = s.ValueOfGoods,
                                                         LocalCustomsTransmissionsStatusCode = s.LocalCustomsTransmissionsStatusCode,
                                                         LocalCustomsTransmissionsStatusName = s.CustomsTransmissionsStatus == null ? null : s.CustomsTransmissionsStatus.Name,
                                                         LocalCustomsTransmissionsStatusError = s.LocalCustomsTransmissionsStatusError,
                                                         LocalCustomsTransmissionsStatusDate = s.LocalCustomsTransmissionsStatusDate,
                                                         LocalCustomsSentByUserId = s.LocalCustomsSentByUserId,
                                                         LocalCustomsSentByUserName = s.LocalCustomsSentByUser != null ? s.LocalCustomsSentByUser.Contact != null ? s.LocalCustomsSentByUser.Contact.EnglishName : "" : "",
                                                         ISFDate = s.ISFDate,
                                                         ISFNumber = s.ISFNumber,
                                                         ITDate = s.ITDate,
                                                         ITNumber = s.ITNumber,
                                                         FreightRelease = s.FreightRelease,
                                                         TerminalAvailable = s.TerminalAvailable,
                                                         OBLTypeCode = m.OBLTypeCode,
                                                         DocumentsClosingDate = m.DocumentsClosingDate,
                                                         TEU = s.TEU,
                                                         ENSNumber = s.ENSNumber,
                                                         ENSDate = s.ENSDate,
                                                         WarehouseLegWarehouseId = s.WarehouseLegWarehouseId,
                                                         WarehouseLegAddressId = s.WarehouseLegAddressId,
                                                         WarehouseLegTerminalCode = s.WarehouseLegTerminalCode,
                                                         WarehouseLegExpectedEntryDate = s.WarehouseLegExpectedEntryDate,
                                                         WarehouseLegActualEntryDate = s.WarehouseLegActualEntryDate,
                                                         WarehouseLegExpectedReleaseDate = s.WarehouseLegExpectedReleaseDate,
                                                         WarehouseLegActualReleaseDate = s.WarehouseLegActualReleaseDate,
                                                         WarehouseLegLastFreeDate = s.WarehouseLegLastFreeDate,
                                                         WarehouseLegRemarks = s.WarehouseLegRemarks,
                                                         WarehouseLegReference = s.WarehouseLegReference,
                                                         RegistryDate = s.RegistryDate,
                                                         IsAssembly = s.IsAssembly,
                                                         LastSharedEventId = s.LastSharedEventId,
                                                         LastSharedEventName = s.LastSharedEvent == null ? null : s.LastSharedEvent.EnglishName,
                                                         LastSharedEventLocation = s.LastSharedEventLocation,
                                                         LastSharedEventNotes = s.LastSharedEventNotes,
                                                         LastSharedEventDate = s.LastSharedEventDate,
                                                         WarehouseLegTerminalName = s.WarehouseLegCard == null ? null : s.WarehouseLegCard.EnglishName,
                                                         WarehouseLegEntryDate = s.WarehouseLegActualEntryDate != null ? s.WarehouseLegActualEntryDate : s.WarehouseLegExpectedEntryDate,
                                                         WarehouseLegReleaseDate = s.WarehouseLegActualReleaseDate != null ? s.WarehouseLegActualReleaseDate : s.WarehouseLegExpectedReleaseDate,
                                                         ManifestLastSharingDate = s.ManifestLastSharingDate,
                                                         FirstOperationalCloseDate = s.FirstOperationalCloseDate,
                                                         FirstAccountingCloseDate = s.FirstAccountingCloseDate,
                                                         ShipmentTypeId = s.ShipmentTypeId,
                                                         LastFinalDestination = s.LastFinalDestination,
                                                         FirstPickupETA = s.FirstPickupETA,
                                                         FirstPickupETD = s.FirstPickupETD,
                                                         From = s.From,
                                                         To = s.To,
                                                         Origin = s.Origin,
                                                         LongMaster = s.TransportModeId == "A" ? (m.AirlinePrefix != null && m.Master != null ? m.AirlinePrefix + "-" + m.Master : m.Master) : m.Master,
                                                         IsDangerous = s.IsDangerous,
                                                         DangerousUnNumber = s.DangerousUnNumber,
                                                         BookingConfirmationNumber = m.BookingConfirmationNumber,
                                                         IsStandalonePickupDelivery = s.IsStandalonePickupDelivery,
                                                     };

            return shipmentsList;
        }

        public string GetEntitiyIdByShipmentNumber(string shipmentNumber, int tenant)
        {
            string entityId = (from a in repository.context.Shipments
                               where a.ShipmentNumber == shipmentNumber && a.Tenant == tenant
                               select a.Id).FirstOrDefault();

            return entityId;
        }

        public List<ShipmentPackagePM> GetShipmentConsolidationPackages(string masterId, int tenant)
        {
            List<ShipmentPackagePM> myResult = new List<ShipmentPackagePM>();

            ShipmentPM masterPM = this.GetSinglePM(masterId, tenant);
            if (masterPM != null)
            {
                foreach (ConsoleShipmentPM consoleShipmentPM in masterPM.ShipmentConsoleShipments)
                {
                    ShipmentPM consoleShipment = this.GetSinglePM(consoleShipmentPM.Id, tenant);
                    if (consoleShipment != null)
                    {
                        foreach (ShipmentPackagePM item in consoleShipment.ShipmentPackages)
                        {
                            item.ShipmentNumber = consoleShipment.ShipmentNumber;
                            myResult.Add(item);
                        }
                    }
                }
            }

            return myResult;
        }

        public bool CheckPODSecurityKeyValidation(string shipmentNumber, string securityKey, int tenant)
        {
            return (from a in repository.context.Shipments
                    where a.ShipmentNumber == shipmentNumber && a.Tenant == tenant && a.SecurityKey == securityKey
                    select a.Id).Any();



        }

        public string GetShipmentIdBySecurityKeyAndShipmentNumber(string shipmentNumber, string securityKey, int tenant)
        {
            string shipmentId = (from a in repository.context.Shipments
                                 where a.ShipmentNumber == shipmentNumber && a.Tenant == tenant && a.SecurityKey == securityKey
                                 select a.Id).FirstOrDefault();

            return shipmentId;

        }


        public IQueryable<ShipmentJoinPayablesList> GetShipmentsJoinPayablesByTenant(int tenant)
        {
            IQueryable<ShipmentJoinPayablesList> dataList =
                (from shipment in repository.context.Shipments
                 join shipmentPayable in repository.context.ShipmentPayables.Include("ChargesType")
                 on shipment.Id equals shipmentPayable.ShipmentId into JoinedPayableData

                 join ms in repository.context.ShipmentMasterDatas.Include("MainCarriageFromPort")
                 on shipment.MasterShipmentDataId equals ms.Id into JoinedMasterData

                 from payable in JoinedPayableData.DefaultIfEmpty()
                 from master in JoinedMasterData.DefaultIfEmpty()

                 where shipment.Tenant == tenant && master.MAWBOBLDate != null && shipment.ShipmentLevelCode != "C"
                 select new ShipmentJoinPayablesList()
                 {
                     //Id = shipment.Id + (!string.IsNullOrEmpty(jd.Id) ? jd.Id : ""),
                     //ShipmentId = shipment.Id,
                     //DirectionId = shipment.DirectionId,
                     //ShipmentNumber = shipment.ShipmentNumber,
                     //CreateDateTime = shipment.CreateDateTime,
                     //MainCarriageETD = m.MainCarriageETD,
                     //CustomerId = shipment.CustomerId,
                     //CustomerName = shipment.CustomerCard != null ? shipment.CustomerCard.EnglishName : null,
                     //AgentName = shipment.AgentCard != null ? shipment.AgentCard.EnglishName : null,
                     //MainCarriageFromPortName = m.MainCarriageFromPort.EnglishName,
                     //MainCarriageFinalDestinationPortName = m.MainCarriageFinalDestinationPort != null ? m.MainCarriageFinalDestinationPort.EnglishName : null,
                     //ShipperName = shipment.ShipperCard != null ? shipment.ShipperCard.EnglishName : null,
                     //MainCarriageCarrierName = m.MainCarriageCarrierCard != null ? m.MainCarriageCarrierCard.EnglishName : null,
                     //ContainerNumber = jd.ContainerNumber,
                     //ShipmentTypeId = shipment.ShipmentTypeId,
                     //ShipmentTypeName = shipment.ShipmentType != null ? shipment.ShipmentType.Name : null,
                     //MainCarriageFinalDestinationETA = m.MainCarriageFinalDestinationETA,
                     //MasterNumber = m.Master,
                     //Vessel_Voyage = m.MainCarriageCarrierNumber,
                     //StatusName = shipment.EntityStatus.Name,
                     //AgentReference1 = shipment.AgentReference1,
                     //AgentReference2 = shipment.AgentReference2,
                     //ContainerCode = jd.PackageType != null ? jd.PackageType.PrintAs : null,
                     //TransportModeId = shipment.TransportModeId,
                     //MainCarriageCarrierPrefix = m.MainCarriageCarrierPrefix,
                     //AgentId = shipment.AgentId,
                     //ShipmentLevelCode = shipment.ShipmentLevelCode,
                     //VesselId = m.MainCarriageVesselId,
                 });

            return dataList;
        }


        public List<AgentSharedManifesRefShipment> GetAgentSharedManifesRefShipmentLists(List<string> agentManifestSharedRefIds, int tenant)
        {

            List<AgentSharedManifesRefShipment> agentSharedManifesShipmentRefList = (from a in repository.context.Shipments
                                                                                     where a.Tenant == tenant && agentManifestSharedRefIds.Contains(a.AgentSharedManifestRef)
                                                                                     select new AgentSharedManifesRefShipment()
                                                                                     {
                                                                                         ShipmentId = a.Id,
                                                                                         AgentSharedManifestRef = a.AgentSharedManifestRef,
                                                                                         ShipmentLevelCode = a.ShipmentLevelCode,
                                                                                         ShipmentNumber = a.ShipmentNumber,
                                                                                     }).ToList();

            return agentSharedManifesShipmentRefList;

        }


        public string GetShipmentNumberByMaster(string master, string longMaster, int tenant)
        {
            string result = "";

            ShipmentMasterData masterData = (from a in repository.context.ShipmentMasterDatas
                                             where a.Master == master && a.Tenant == tenant
                                             select a).FirstOrDefault();
            if (masterData != null)
            {
                Shipment shipment = (from a in repository.context.Shipments
                                     where a.Tenant == tenant && a.Id == masterData.Id && !a.IsCancelled && a.ShipmentLevelCode != "H" && a.ShipmentLevelCode != "A" && a.TransportModeId == "A"
                                     select a).FirstOrDefault();
                if (shipment != null)
                {
                    string shipmentlongMaser = EntityFieldsHelper.GetLongMasterField(shipment, masterData);
                    if (shipmentlongMaser == longMaster) result = shipment.ShipmentNumber;
                }
            }


            return result;

        }

        public ShipmentPM GetSinglePMByCustomerReference1(string CustomerReference1, int tenant, bool IsForwarderShipment)
        {
            Shipment shipment = (from a in repository.context.Shipments.Include("EntityStatus").Include("ShipmentType").Include("Incoterm").Include("ShipmentReceivableStatus").Include("ShipmentPayableStatus").Include("ShipmentLevel").Include("NextLeg").Include("ShipmentType").Include("MoveType").Include("SalesmanUser").Include("SalesmanUser.Contact")
                                 where a.CustomerReference1 == CustomerReference1 && (IsForwarderShipment ? a.ForwarderShipmentNumber != null : true) && a.Tenant == tenant
                                 select a).FirstOrDefault();
            if (shipment == null)
            {
                return null;
            }
            ShipmentMasterData masterData = (from a in repository.context.ShipmentMasterDatas
                                             where a.Id == shipment.MasterShipmentDataId
                                             select a).FirstOrDefault();

            ShipmentPM shipmentPM = new ShipmentPM();

            shipmentPM = MapShipmentToShipmentPM(shipmentPM, shipment, null, masterData, false);


            return shipmentPM;
        }

        public ShipmentPM GetByCustomerReference1(string CustomerReference1, int tenant, bool IsForwarderShipment)
        {
            var shipments = (from a in repository.context.Shipments.Include("EntityStatus").Include("ShipmentType").Include("Incoterm").Include("ShipmentReceivableStatus").Include("ShipmentPayableStatus").Include("ShipmentLevel").Include("NextLeg").Include("ShipmentType").Include("MoveType").Include("SalesmanUser").Include("SalesmanUser.Contact")
                             where a.CustomerReference1 == CustomerReference1 && (IsForwarderShipment ? a.ForwarderShipmentNumber != null : true) && a.Tenant == tenant
                             select a);
            if (shipments == null || shipments.Count() < 1)
            {
                return null;
            }
            Shipment shipment = shipments.FirstOrDefault();
            ShipmentMasterData masterData = (from a in repository.context.ShipmentMasterDatas
                                             where a.Id == shipment.MasterShipmentDataId
                                             select a).FirstOrDefault();

            ShipmentPM shipmentPM = new ShipmentPM();

            shipmentPM = MapShipmentToShipmentPM(shipmentPM, shipment, null, masterData, false);


            return shipmentPM;
        }

        public ShipmentPM GetByCustomerReference1ForUpdate(string CustomerReference1, string ShipmentId, int tenant, bool IsForwarderShipment)
        {
            var shipments = (from a in repository.context.Shipments.Include("EntityStatus").Include("ShipmentType").Include("Incoterm").Include("ShipmentReceivableStatus").Include("ShipmentPayableStatus").Include("ShipmentLevel").Include("NextLeg").Include("ShipmentType").Include("MoveType").Include("SalesmanUser").Include("SalesmanUser.Contact")
                             where a.CustomerReference1 == CustomerReference1 && a.Id != ShipmentId && (IsForwarderShipment ? a.ForwarderShipmentNumber != null : true) && a.Tenant == tenant
                             select a);
            if (shipments == null || shipments.Count() < 1)
            {
                return null;
            }
            Shipment shipment = shipments.FirstOrDefault();
            ShipmentMasterData masterData = (from a in repository.context.ShipmentMasterDatas
                                             where a.Id == shipment.MasterShipmentDataId
                                             select a).FirstOrDefault();

            ShipmentPM shipmentPM = new ShipmentPM();

            shipmentPM = MapShipmentToShipmentPM(shipmentPM, shipment, null, masterData, false);


            return shipmentPM;
        }

        public IQueryable<ShipmentList> GetIQueryableShipmentList(IQueryable<ShipmentDataView> shipments, int tenant)
        {
            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM currentTenant = tenantQuery.GetSinglePM(tenant);

            var myResult = from f in shipments
                           select new ShipmentList()
                           {
                               CarrierLastStatusDate = f.CarrierLastStatusDate,
                               CarrierLastStatusName = f.CarrierLastStatusName,
                               CarrierLastStatusCode = f.CarrierLastStatusCode,
                               ShipmentViewId = f.Id,
                               Id = f.Id,
                               Shipper = f.Shipper,
                               Consignee = f.Consignee,
                               DirectionId = f.DirectionId,
                               DirectionName = f.DirectionName,
                               TransportModeName = f.TransportModeName,
                               MasterShipmentNumber = f.MasterShipmentNumber,
                               House = f.House,
                               CreateDateTime = f.CreateDateTime,
                               ShipmentNumber = f.ShipmentNumber,
                               ShipmentType = f.ShipmentType,
                               ShipmentTypeId = f.ShipmentTypeId,
                               TransportModeId = f.TransportModeId,
                               Field1 = f.Field1,
                               Field2 = f.Field2,
                               Field3 = f.Field3,
                               Field4 = f.Field4,
                               Field5 = f.Field5,
                               Field6 = f.Field6,
                               Field7 = f.Field7,
                               Field9 = f.Field9,
                               Field8 = f.Field8,
                               Field10 = f.Field10,
                               Field11 = f.Field11,
                               Field12 = f.Field12,
                               Field13 = f.Field13,
                               Field14 = f.Field14,
                               Field15 = f.Field15,
                               Field16 = f.Field16,
                               Field17 = f.Field17,
                               Field18 = f.Field18,
                               Field19 = f.Field19,
                               Field20 = f.Field20,
                               Field21 = f.Field21,
                               Field22 = f.Field22,
                               Field23 = f.Field23,
                               Field24 = f.Field24,
                               Field25 = f.Field25,
                               Field26 = f.Field26,
                               Field27 = f.Field27,
                               Field28 = f.Field28,
                               Field29 = f.Field29,
                               Field30 = f.Field30,
                               Field31 = f.Field31,
                               Field32 = f.Field32,
                               Field33 = f.Field33,
                               Field34 = f.Field34,
                               Field35 = f.Field35,
                               Field36 = f.Field36,
                               Field37 = f.Field37,
                               Field38 = f.Field38,
                               Field39 = f.Field39,
                               Field40 = f.Field40,
                               CarrierTransportDocumentNumber = f.CarrierTransportDocumentNumber,
                               ChargeableWeightInKG = f.ChargeableWeightInKG,
                               ChargeableWeight = f.ChargeableWeight,
                               GrossWeight = f.GrossWeight,
                               ShipperReference1 = f.ShipperReference1/*, Master = f.Master*/,
                               Master = f.Master,
                               OpenReceivablesInLocalCurrency = f.OpenReceivablesInLocalCurrency,
                               OpenReceivablesInProfitCurrency = f.OpenReceivablesInProfitCurrency,
                               AccountedReceivablesInLocalCurrency = f.AccountedReceivablesInLocalCurrency,
                               OpenPayablesInLocalCurrency = f.OpenPayablesInLocalCurrency,
                               ShipmentPayableStatusCode = f.ShipmentPayableStatusCode,
                               ShipmentReceivableStatusCode = f.ShipmentReceivableStatusCode,
                               ShipmentPayableStatusName = f.ShipmentPayableStatusName,
                               ShipmentReceivableStatusName = f.ShipmentReceivableStatusName,
                               ProfitInLocalCurrency = f.ProfitInLocalCurrency,
                               ProfitInProfitCurrency = f.ProfitInProfitCurrency,
                               EstimateProfitInLocalCurrency = f.EstimateProfitInLocalCurrency,
                               EstimateProfitInProfitCurrency = f.EstimateProfitInProfitCurrency,
                               BranchId = f.BranchId,
                               DepartmentId = f.DepartmentId,
                               MainCarriageETA = f.MainCarriageETA,
                               MainCarriageATD = f.MainCarriageATD,
                               LocalCurrencyCode = currentTenant.CurrencyCode,
                               ProfitCurrencyCode = currentTenant.ProfitCurrencyCode,
                               NextETA = f.NextETA,
                               NextETD = f.NextETD,
                               NextLegName = f.NextLegName,
                               Routing = f.Routing,//(f.DirectionId!="D"&&f.TransportModeId!="I")?((f.PreCarriageFromPortCode != null ? f.PreCarriageFromPortCode + " > " : "") + (f.MasterShipmentDataId != null ? (f.MainCarriageFromPortCode != null ? f.MainCarriageFromPortCode + " > " : "") + (f.MainCarriageFinalDestinationPortCode != null ? (f.OnCarriageToPortCode != null ? f.MainCarriageFinalDestinationPortCode + " > " + f.OnCarriageToPortCode : f.MainCarriageFinalDestinationPortCode) : "") : ((f.FromPortCode != null ? f.FromPortCode + " > " : "") + (f.ToPortCode != null ? (f.OnCarriageToPortCode != null ? f.ToPortCode + " > " + f.OnCarriageToPortCode : f.ToPortCode) : "")))):(""),
                               FromPortId = f.FromPortId,
                               ToPortId = f.ToPortId,

                               FromPortCode = f.FromPortCode,
                               FromPort = f.FromPort,
                               FromPortName = f.FromPortName,
                               FromPortCountry = f.MainCarriageFromPortCountryName,

                               // Column: To
                               ToPortCode = f.ToPortCode,
                               ToPort = (f.TransportModeId == "I" && f.DirectionId == "D") ? f.MainCarriageToCity : f.ToPort,
                               ToPortName = f.ToPortName,
                               ToPortCountry = f.MainCarriageToPortCountryName,
                               MasterShipmentDataId = f.MasterShipmentDataId,
                               BranchName = f.BranchName,
                               MoveTypeName = f.MoveTypeName,
                               CustomerName = f.CustomerName,
                               GrossWeightInKG = f.GrossWeightInKG,
                               GrossWeightPerStorageDays = f.GrossWeightPerStorageDays,
                               GrossWeightPerTon = f.GrossWeightPerTon,
                               ShipmentLevelCode = f.ShipmentLevelCode,
                               ShipmentLevelName = f.ShipmentLevelName,
                               VolumetricWeight = f.VolumetricWeight,
                               AirlinePrefix = f.AirlinePrefix,
                               AccountedPayablesInLocalCurrency = f.AccountedPayablesInLocalCurrency,
                               AccountedPayablesInProfitCurrency = f.AccountedPayablesInProfitCurrency,
                               AccountedReceivablesInProfitCurrency = f.AccountedReceivablesInProfitCurrency,
                               MainCarriageFromPortId = f.MainCarriageFromPortId,

                               // Column: Origin
                               MainCarriageFromPortName = (f.TransportModeId == "I" && f.DirectionId == "D") ? f.MainCarriageFromCity : f.FromPortName,
                               MainCarriageATA = f.MainCarriageATA,
                               MainCarriageETD = f.MainCarriageETD,
                               IncotermId = f.IncotermId,
                               OpenPayablesInProfitCurrency = f.OpenPayablesInProfitCurrency,
                               CustomerReference1 = f.CustomerReference1,
                               CustomerReference2 = f.CustomerReference2,
                               IssuingCarrierAgentId = f.IssuingCarrierAgentId,
                               IncotermCode = f.IncotermCode,
                               MainCarriageCarrierId = f.MainCarriageCarrierId,
                               AsAgreedFreight = f.AsAgreedFreight,
                               AsAgreedOtherCharges = f.AsAgreedOtherCharges,
                               AccountNumber = f.AccountNumber,
                               AWBPrint = f.AWBPrint,
                               FNAReason = f.FNAReason,
                               AgentName = f.AgentName,
                               MainCarriageCarrierName = f.MainCarriageCarrierName,
                               FinalArrivalDate = f.FinalArrivalDate,
                               EstimatedFinalArrivalDate = f.EstimatedFinalArrivalDate,
                               ActualFinalArrivalDate = f.ActualFinalArrivalDate,
                               VolumeInCBM = f.VolumeInCBM,
                               TEU = f.TEU,
                               ProfitExchangeRate = f.ProfitExchangeRate,
                               LastFSRStatusRequestDate = f.LastFSRStatusRequestDate,
                               CustomFieldId = f.CustomFileId,
                               SpecialServicesTypeName = f.SpecialServicesTypeName,
                               AMSBL = f.AMSBL,
                               FromPortCountryCode = f.FromPortCountryCode,
                               FromPortCountryName = f.FromPortCountryName,
                               ToPortCountryCode = f.ToPortCountryCode,
                               ToPortCountryName = f.ToPortCountryName,
                               CarrierNumber = f.CarrierNumber,
                               AgentId = f.AgentId,
                               AgentComputed = f.AgentComputed,
                               // ComputedShipmentNumber = f.ComputedShipmentNumber,
                               ARInvoiceIssued = f.ARInvoiceIssued,
                               CreditNoteIssued = f.CreditNoteIssued,
                               CustomFileNumber = f.CustomFileNumber,
                               FreightForwarderId = f.FreightForwarderId,
                               FreightForwarderName = f.FreightForwarderName,
                               ProductCode = f.ProductCode,
                               IsAccountingClosed = f.IsAccountingClosed,
                               IsOperationalClosed = f.IsOperationalClosed,
                               OperationalCloseDate = f.OperationalCloseDate,
                               AccountingCloseDate = f.AccountingCloseDate,
                               PackagesQuantity = f.PackagesQuantity,
                               FHLStatusCode = f.FHLStatusCode,
                               FHLStatusName = f.FHLStatusName,
                               FHLStatusDate = f.FHLStatusDate,
                               FWBStatusCode = f.FWBStatusCode,
                               FWBStatusName = f.FWBStatusName,
                               FWBStatusDate = f.FWBStatusDate,
                               CargonautFHLStatusCode = f.CargonautFHLStatusCode,
                               CargonautFHLStatusName = f.CargonautFHLStatusName,
                               CargonautFHLStatusDate = f.CargonautFHLStatusDate,
                               CargonautFWBStatusCode = f.CargonautFWBStatusCode,
                               CargonautFWBStatusName = f.CargonautFWBStatusName,
                               CargonautFWBStatusDate = f.CargonautFWBStatusDate,
                               NumberOfInsidePackages = f.NumberOfInsidePackages,
                               NumberOfInsidePackagesDetails = f.NumberOfInsidePackagesDetails,
                               ConsolidatorId = f.ConsolidatorId,
                               ConsolidatorName = f.ConsolidatorName,
                               ConsolidatorNote = f.ConsolidatorNote,
                               ConsolidatorAddressId = f.ConsolidatorAddressId,
                               ConsolidatorContactId = f.ConsolidatorContactId,
                               ConsolidatorReference = f.ConsolidatorReference,
                               PreCarriageETD = f.PreCarriageETD,
                               AccountManagerUserName = f.AccountManagerUserName,
                               SalesmanUserName = f.SalesmanUserName,
                               CreatedByUserName = f.CreatedByUserName,
                               ManifestReason = f.ManifestReason,
                               ManifestStatusCode = f.ManifestStatusCode,
                               IsMissingDocument = f.IsMissingDocument,
                               DocumentsSearchFields = f.DocumentsSearchFields,
                               ConsigneeName = f.ConsigneeName,
                               ShipperName = f.ShipperName,
                               ForwarderShipmentNumber = f.ForwarderShipmentNumber,
                               CustomerShipmentNumber = f.CustomerShipmentNumber,
                               CustomsDeclarationNumber = f.CustomsDeclarationNumber,
                               HasException = f.HasException,
                               ExceptionDescription = f.ExceptionDescription,
                               ExceptionResolvedDescription = f.ExceptionResolvedDescription,
                               LastExceptionDescription = f.LastExceptionDescription,
                               ExceptionDate = f.ExceptionDate,
                               LastDocumentDateTime = f.LastDocumentDateTime,
                               MainCarriageExpectedOrActual = f.MainCarriageExpectedOrActual,
                               MainCarriageETAOrATA = f.MainCarriageETAOrATA,
                               MainCarriageFromPortCountryCode = f.MainCarriageFromPortCountryCode,
                               MainCarriageToPortCountryCode = f.MainCarriageToPortCountryCode,
                               MainCarriageFromPortCode = f.MainCarriageFromPortCode,
                               MainCarriageFromPortCountryName = f.MainCarriageFromPortCountryName,
                               MainCarriageToPortCode = f.MainCarriageToPortCode,
                               MainCarriageToPortCountryName = f.MainCarriageToPortCountryName,
                               MainCarriageToPortName = f.MainCarriageToPortName,
                               MainHarmonize = f.MainHarmonize,
                               StatusId = f.StatusId,
                               StatusDate = f.StatusDate,
                               //StatusName = f.StatusName,
                               StatusLocation = f.StatusLocation,
                               LongMaster = f.LongMaster,
                               PartnerLogoId = f.PartnerLogoId,
                               MissingDocumentsCount = f.MissingDocumentsCount,
                               MissingDocumentsCountWords = f.MissingDocumentsCountWords,
                               CancelledDate = f.CancelledDate,
                               PartnerName = f.PartnerName,
                               MissingDocsNames = f.MissingDocsNames,
                               NumberOfFollowUps = f.NumberOfFollowUps,
                               ArchivedText = f.ArchivedText,
                               CustomerId = f.CustomerId,
                               IsRequestedDocuments = f.IsRequestedDocuments,
                               IsDigitalSignRequired = f.IsDigitalSignRequired,
                               RequestedDocumentsCount = f.RequestedDocumentsCount,
                               MainCarriageVesselId = f.MainCarriageVesselId,
                               MainCarriageVesselName = f.MainCarriageVesselName,
                               BookingConfirmationNumber = f.BookingConfirmationNumber,
                               DescriptionOfGoods = f.DescriptionOfGoods,
                               Notes = f.Notes,
                               IsNewARInvoiceBlocked = f.IsNewARInvoiceBlocked,
                               OperationalDate = f.OperationalDate,
                               CutoffDate = f.CutoffDate,
                               IsImporterApprovalRequried = f.IsImporterApprovalRequried,
                               ApprovedByUserName = f.ApprovedByUserName,
                               IsManifestSentToAgent = f.IsManifestSentToAgent,
                               AgentSharedManifestRef = f.AgentSharedManifestRef,
                               ValueOfGoods = f.ValueOfGoods,
                               NumberOfHouses = f.NumberOfHouses,
                               LocalCustomsTransmissionsStatusCode = f.LocalCustomsTransmissionsStatusCode,
                               LocalCustomsTransmissionsStatusName = f.LocalCustomsTransmissionsStatusName,
                               LocalCustomsTransmissionsStatusError = f.LocalCustomsTransmissionsStatusError,
                               LocalCustomsTransmissionsStatusDate = f.LocalCustomsTransmissionsStatusDate,
                               LocalCustomsSentByUserName = f.LocalCustomsSentByUserName,
                               ISFDate = f.ISFDate,
                               ISFNumber = f.ISFNumber,
                               ITDate = f.ITDate,
                               ITNumber = f.ITNumber,
                               FreightRelease = f.FreightRelease,
                               TerminalAvailable = f.TerminalAvailable,
                               OBLTypeCode = f.OBLTypeCode,
                               DocumentsClosingDate = f.DocumentsClosingDate,
                               ENSNumber = f.ENSNumber,
                               ENSDate = f.ENSDate,
                               WarehouseLegWarehouseId = f.WarehouseLegWarehouseId,
                               WarehouseLegAddressId = f.WarehouseLegAddressId,
                               WarehouseLegTerminalCode = f.WarehouseLegTerminalCode,
                               WarehouseLegExpectedEntryDate = f.WarehouseLegExpectedEntryDate,
                               WarehouseLegActualEntryDate = f.WarehouseLegActualEntryDate,
                               WarehouseLegExpectedReleaseDate = f.WarehouseLegExpectedReleaseDate,
                               WarehouseLegActualReleaseDate = f.WarehouseLegActualReleaseDate,
                               WarehouseLegLastFreeDate = f.WarehouseLegLastFreeDate,
                               WarehouseLegRemarks = f.WarehouseLegRemarks,
                               WarehouseLegReference = f.WarehouseLegReference,
                               WarehouseLegTerminalName = f.WarehouseLegTerminalName,
                               WarehouseLegAddressCountryCode = f.WarehouseLegAddressCountryCode,
                               WarehouseLegAddressCountryName = f.WarehouseLegAddressCountryName,
                               WarehouseLegEntryDate = f.WarehouseLegActualEntryDate != null ? f.WarehouseLegActualEntryDate : f.WarehouseLegExpectedEntryDate,
                               WarehouseLegReleaseDate = f.WarehouseLegActualReleaseDate != null ? f.WarehouseLegActualReleaseDate : f.WarehouseLegExpectedReleaseDate,
                               RegistryDate = f.RegistryDate,
                               TrailerNumber = f.TrailerNumber,
                               IsAssembly = f.IsAssembly,
                               LastSharedEventId = f.LastSharedEventId,
                               LastSharedEventName = f.LastSharedEventName,
                               LastSharedEventLocation = f.LastSharedEventLocation,
                               LastSharedEventNotes = f.LastSharedEventNotes,
                               LastSharedEventDate = f.LastSharedEventDate,
                               ManifestLastSharingDate = f.ManifestLastSharingDate,
                               NumberOfPackages = f.NumberOfPackages,
                               NumberOfContainers = f.NumberOfContainers,
                               FirstOperationalCloseDate = f.FirstOperationalCloseDate,
                               FirstAccountingCloseDate = f.FirstAccountingCloseDate,
                               DeclarationNumber = f.DeclarationNumber,
                               CustomsClearanceDate = f.CustomsClearanceDate,
                               IncludesCustoms = f.IncludesCustoms,
                               DeclarationDate = f.DeclarationDate,
                               INTTRASIError = f.INTTRASIError,
                               INTTRASIStatusCode = f.INTTRASIStatusCode,
                               INTTRASIStatusName = f.INTTRASIStatusName,
                               INTTRASIStatusDate = f.INTTRASIStatusDate,
                               INTTRABookingStatusCode = f.INTTRABookingStatusCode,
                               INTTRABookingStatusName = f.INTTRABookingStatusName,
                               INTTRABookingTransStatusName = f.INTTRABookingTransStatusName,
                               INTTRABookingTransStatusCode = f.INTTRABookingTransStatusCode,
                               INTTRABookingError = f.INTTRABookingError,
                               INTTRALastBookingResponse = f.INTTRALastBookingResponse,
                               LastFinalDestination = f.LastFinalDestination,
                               FirstPickupETA = f.FirstPickupETA,
                               FirstPickupETD = f.FirstPickupETD,
                               INTTRALastStatusDate = f.INTTRALastStatusDate,
                               Notify1Reference = f.Notify1Reference,
                               Notify2Reference = f.Notify2Reference,
                               ShipperNotExporterReference = f.ShipperNotExporterReference,
                               ConsigneeNotImporterReference = f.ConsigneeNotImporterReference,
                               ProjectNumber = f.ProjectNumber,
                               ContainerLastStatusDate = f.ContainerLastStatusDate,
                               IsDepositionRequired = f.IsDepositionRequired,
                               CreatedFromDigital = f.CreatedFromDigital,
                               ImporterDepositionRequestDetails = f.ImporterDepositionRequestDetails,
                               ForwarderPartnerId = f.ForwarderPartnerId,
                               From = f.From,
                               To = f.To,
                               Origin = f.Origin,
                               ARInvoices = f.ARInvoices,
                               NotInvoicedReceivablesAmount = f.NotInvoicedReceivablesAmount,
                               CreatedByPartner = f.CreatedByPartner,
                               FirstARInvoiceApprovalDate = f.FirstARInvoiceApprovalDate,
                               MainCarriageFinalDestinationATA = f.MainCarriageFinalDestinationATA,
                               MainCarriageFinalDestinationETA = f.MainCarriageFinalDestinationETA,
                               ShipmentSubTypeId = f.ShipmentSubTypeId,
                               ShipmentSubTypeName = f.ShipmentSubTypeName,
                               ImportManifest = f.ImportManifest,
                               IsDangerous = f.IsDangerous,
                               DangerousUnNumber = f.DangerousUnNumber,
                               ComputedStatusId = f.ComputedStatusId,
                               ComputedStatusDate = f.ComputedStatusDate,
                               QuoteId = f.QuoteId,
                               QuoteNumber = f.QuoteNumber,
                               StatusName = !string.IsNullOrEmpty(f.StatusLocation) ? f.StatusName + " (" + f.StatusLocation + ")" : f.StatusName,
                               ExactStatusName = f.StatusName,
                               PreForwardingETD = f.PreForwardingETD,
                               IsStandalonePickupDelivery = f.IsStandalonePickupDelivery,
                           };
            return myResult;
        }

        public ShipmentPM GetSinglePMByCustomerReference1(string CustomerReference1, int tenant)
        {
            Shipment shipment = (from a in repository.context.Shipments.Include("EntityStatus").Include("ShipmentType").Include("Incoterm").Include("ShipmentReceivableStatus").Include("ShipmentPayableStatus").Include("ShipmentLevel").Include("NextLeg").Include("ShipmentType").Include("MoveType").Include("SalesmanUser").Include("SalesmanUser.Contact")
                                 where a.CustomerReference1 == CustomerReference1 && a.ForwarderShipmentNumber != null && a.Tenant == tenant
                                 select a).FirstOrDefault();
            if (shipment == null)
            {
                return null;
            }
            ShipmentMasterData masterData = (from a in repository.context.ShipmentMasterDatas
                                             where a.Id == shipment.MasterShipmentDataId
                                             select a).FirstOrDefault();

            ShipmentPM shipmentPM = new ShipmentPM();

            shipmentPM = MapShipmentToShipmentPM(shipmentPM, shipment, null, masterData, false);


            return shipmentPM;
        }
        public ShipmentList GetSingleShipmentList(ShipmentDataView f, int tenant)
        {
            ShipmentList myResult = null;

            if (f != null)
            {
                TenantQuery tenantQuery = new TenantQuery(tenant);
                TenantPM currentTenant = tenantQuery.GetSinglePM(tenant);

                myResult = new ShipmentList()
                {
                    CarrierLastStatusDate = f.CarrierLastStatusDate,
                    CarrierLastStatusName = f.CarrierLastStatusName,
                    CarrierLastStatusCode = f.CarrierLastStatusCode,
                    ShipmentViewId = f.Id,
                    Id = f.Id,
                    Shipper = f.ShipperName,
                    Consignee = f.ConsigneeName,
                    DirectionId = f.DirectionId,
                    DirectionName = f.DirectionName,
                    TransportModeName = f.TransportModeName,
                    MasterShipmentNumber = f.MasterShipmentNumber,
                    House = f.House,
                    CreateDateTime = f.CreateDateTime,
                    ShipmentNumber = f.ShipmentNumber,
                    ShipmentType = f.ShipmentType,
                    ShipmentTypeId = f.ShipmentTypeId,
                    TransportModeId = f.TransportModeId,
                    Field1 = f.Field1,
                    Field2 = f.Field2,
                    Field3 = f.Field3,
                    Field4 = f.Field4,
                    Field5 = f.Field5,
                    Field6 = f.Field6,
                    Field7 = f.Field7,
                    Field9 = f.Field9,
                    Field8 = f.Field8,
                    Field10 = f.Field10,
                    Field11 = f.Field11,
                    Field12 = f.Field12,
                    Field13 = f.Field13,
                    Field14 = f.Field14,
                    Field15 = f.Field15,
                    Field16 = f.Field16,
                    Field17 = f.Field17,
                    Field18 = f.Field18,
                    Field19 = f.Field19,
                    Field20 = f.Field20,
                    Field21 = f.Field21,
                    Field22 = f.Field22,
                    Field23 = f.Field23,
                    Field24 = f.Field24,
                    Field25 = f.Field25,
                    Field26 = f.Field26,
                    Field27 = f.Field27,
                    Field28 = f.Field28,
                    Field29 = f.Field29,
                    Field30 = f.Field30,
                    Field31 = f.Field31,
                    Field32 = f.Field32,
                    Field33 = f.Field33,
                    Field34 = f.Field34,
                    Field35 = f.Field35,
                    Field36 = f.Field36,
                    Field37 = f.Field37,
                    Field38 = f.Field38,
                    Field39 = f.Field39,
                    Field40 = f.Field40,
                    CarrierTransportDocumentNumber = f.CarrierTransportDocumentNumber,
                    ChargeableWeightInKG = f.ChargeableWeightInKG,
                    ChargeableWeight = f.ChargeableWeight,
                    GrossWeight = f.GrossWeight,
                    ShipperReference1 = f.ShipperReference1/*, Master = f.Master*/,
                    Master = f.Master,
                    OpenReceivablesInLocalCurrency = f.OpenReceivablesInLocalCurrency,
                    OpenReceivablesInProfitCurrency = f.OpenReceivablesInProfitCurrency,
                    AccountedReceivablesInLocalCurrency = f.AccountedReceivablesInLocalCurrency,
                    OpenPayablesInLocalCurrency = f.OpenPayablesInLocalCurrency,
                    ShipmentPayableStatusCode = f.ShipmentPayableStatusCode,
                    ShipmentReceivableStatusCode = f.ShipmentReceivableStatusCode,
                    ShipmentPayableStatusName = f.ShipmentPayableStatusName,
                    ShipmentReceivableStatusName = f.ShipmentReceivableStatusName,
                    ProfitInLocalCurrency = f.ProfitInLocalCurrency,
                    ProfitInProfitCurrency = f.ProfitInProfitCurrency,
                    EstimateProfitInLocalCurrency = f.EstimateProfitInLocalCurrency,
                    EstimateProfitInProfitCurrency = f.EstimateProfitInProfitCurrency,
                    BranchId = f.BranchId,
                    DepartmentId = f.DepartmentId,
                    MainCarriageETA = f.MainCarriageETA,
                    MainCarriageATD = f.MainCarriageATD,
                    LocalCurrencyCode = currentTenant.CurrencyCode,
                    ProfitCurrencyCode = currentTenant.ProfitCurrencyCode,
                    NextETA = f.NextETA,
                    NextETD = f.NextETD,
                    NextLegName = f.NextLegName,
                    Routing = f.Routing,//(f.DirectionId!="D"&&f.TransportModeId!="I")?((f.PreCarriageFromPortCode != null ? f.PreCarriageFromPortCode + " > " : "") + (f.MasterShipmentDataId != null ? (f.MainCarriageFromPortCode != null ? f.MainCarriageFromPortCode + " > " : "") + (f.MainCarriageFinalDestinationPortCode != null ? (f.OnCarriageToPortCode != null ? f.MainCarriageFinalDestinationPortCode + " > " + f.OnCarriageToPortCode : f.MainCarriageFinalDestinationPortCode) : "") : ((f.FromPortCode != null ? f.FromPortCode + " > " : "") + (f.ToPortCode != null ? (f.OnCarriageToPortCode != null ? f.ToPortCode + " > " + f.OnCarriageToPortCode : f.ToPortCode) : "")))):(""),
                    FromPortId = f.FromPortId,
                    ToPortId = f.ToPortId,
                    FromPort = f.FromPort,
                    FromPortName = f.FromPortName,
                    FromPortCountry = f.MainCarriageFromPortCountryName,

                    // Column: To
                    ToPort = (f.TransportModeId == "I" && f.DirectionId == "D") ? f.MainCarriageToCity : f.ToPortName,


                    ToPortName = f.ToPortName,
                    ToPortCountry = f.MainCarriageToPortCountryName,
                    MasterShipmentDataId = f.MasterShipmentDataId,
                    BranchName = f.BranchName,
                    MoveTypeName = f.MoveTypeName,
                    CustomerName = f.CustomerName,
                    GrossWeightInKG = f.GrossWeightInKG,
                    GrossWeightPerStorageDays = f.GrossWeightPerStorageDays,
                    GrossWeightPerTon = f.GrossWeightPerTon,
                    ShipmentLevelCode = f.ShipmentLevelCode,
                    ShipmentLevelName = f.ShipmentLevelName,
                    VolumetricWeight = f.VolumetricWeight,
                    AirlinePrefix = f.AirlinePrefix,
                    AccountedPayablesInLocalCurrency = f.AccountedPayablesInLocalCurrency,
                    AccountedPayablesInProfitCurrency = f.AccountedPayablesInProfitCurrency,
                    AccountedReceivablesInProfitCurrency = f.AccountedReceivablesInProfitCurrency,
                    MainCarriageFromPortId = f.MainCarriageFromPortId,

                    // Column: Origin
                    MainCarriageFromPortName = (f.TransportModeId == "I" && f.DirectionId == "D") ? f.MainCarriageFromCity : f.FromPortName,

                    MainCarriageATA = f.MainCarriageATA,
                    MainCarriageETD = f.MainCarriageETD,
                    IncotermId = f.IncotermId,
                    OpenPayablesInProfitCurrency = f.OpenPayablesInProfitCurrency,
                    CustomerReference1 = f.CustomerReference1,
                    CustomerReference2 = f.CustomerReference2,
                    IssuingCarrierAgentId = f.IssuingCarrierAgentId,
                    IncotermCode = f.IncotermCode,
                    MainCarriageCarrierId = f.MainCarriageCarrierId,
                    AsAgreedFreight = f.AsAgreedFreight,
                    AsAgreedOtherCharges = f.AsAgreedOtherCharges,
                    AccountNumber = f.AccountNumber,
                    AWBPrint = f.AWBPrint,
                    FNAReason = f.FNAReason,
                    AgentName = f.AgentName,
                    MainCarriageCarrierName = f.MainCarriageCarrierName,
                    FinalArrivalDate = f.FinalArrivalDate,
                    EstimatedFinalArrivalDate = f.EstimatedFinalArrivalDate,
                    ActualFinalArrivalDate = f.ActualFinalArrivalDate,
                    VolumeInCBM = f.VolumeInCBM,
                    TEU = f.TEU,
                    ProfitExchangeRate = f.ProfitExchangeRate,
                    LastFSRStatusRequestDate = f.LastFSRStatusRequestDate,
                    CustomFieldId = f.CustomFileId,
                    SpecialServicesTypeName = f.SpecialServicesTypeName,
                    AMSBL = f.AMSBL,
                    FromPortCountryCode = f.FromPortCountryCode,
                    FromPortCountryName = f.FromPortCountryName,
                    ToPortCountryCode = f.ToPortCountryCode,
                    ToPortCountryName = f.ToPortCountryName,
                    CarrierNumber = f.CarrierNumber,
                    AgentId = f.AgentId,
                    AgentComputed = f.AgentComputed,
                    //ComputedShipmentNumber = f.ComputedShipmentNumber,

                    ARInvoiceIssued = f.ARInvoiceIssued,
                    CreditNoteIssued = f.CreditNoteIssued,
                    CustomFileNumber = f.CustomFileNumber,
                    FreightForwarderId = f.FreightForwarderId,
                    FreightForwarderName = f.FreightForwarderName,
                    ProductCode = f.ProductCode,
                    IsAccountingClosed = f.IsAccountingClosed,
                    IsOperationalClosed = f.IsOperationalClosed,
                    OperationalCloseDate = f.OperationalCloseDate,
                    AccountingCloseDate = f.AccountingCloseDate,
                    PackagesQuantity = f.PackagesQuantity,
                    FHLStatusCode = f.FHLStatusCode,
                    FHLStatusName = f.FHLStatusName,
                    FHLStatusDate = f.FHLStatusDate,
                    FWBStatusCode = f.FWBStatusCode,
                    FWBStatusName = f.FWBStatusName,
                    FWBStatusDate = f.FWBStatusDate,
                    CargonautFHLStatusCode = f.CargonautFHLStatusCode,
                    CargonautFHLStatusName = f.CargonautFHLStatusName,
                    CargonautFHLStatusDate = f.CargonautFHLStatusDate,
                    CargonautFWBStatusCode = f.CargonautFWBStatusCode,
                    CargonautFWBStatusName = f.CargonautFWBStatusName,
                    CargonautFWBStatusDate = f.CargonautFWBStatusDate,
                    NumberOfInsidePackages = f.NumberOfInsidePackages,
                    NumberOfInsidePackagesDetails = f.NumberOfInsidePackagesDetails,
                    ConsolidatorId = f.ConsolidatorId,
                    ConsolidatorName = f.ConsolidatorName,
                    ConsolidatorNote = f.ConsolidatorNote,
                    ConsolidatorAddressId = f.ConsolidatorAddressId,
                    ConsolidatorContactId = f.ConsolidatorContactId,
                    ConsolidatorReference = f.ConsolidatorReference,
                    PreCarriageETD = f.PreCarriageETD,
                    AccountManagerUserName = f.AccountManagerUserName,
                    SalesmanUserName = f.SalesmanUserName,
                    CreatedByUserName = f.CreatedByUserName,
                    ManifestReason = f.ManifestReason,
                    ManifestStatusCode = f.ManifestStatusCode,
                    IsMissingDocument = f.IsMissingDocument,
                    DocumentsSearchFields = f.DocumentsSearchFields,
                    ConsigneeName = f.ConsigneeName,
                    ShipperName = f.ShipperName,
                    ForwarderShipmentNumber = f.ForwarderShipmentNumber,
                    CustomerShipmentNumber = f.CustomerShipmentNumber,
                    CustomsDeclarationNumber = f.CustomsDeclarationNumber,
                    HasException = f.HasException,
                    ExceptionDescription = f.ExceptionDescription,
                    ExceptionResolvedDescription = f.ExceptionResolvedDescription,
                    LastExceptionDescription = f.LastExceptionDescription,
                    ExceptionDate = f.ExceptionDate,
                    LastDocumentDateTime = f.LastDocumentDateTime,
                    MainCarriageExpectedOrActual = f.MainCarriageExpectedOrActual,
                    MainCarriageETAOrATA = f.MainCarriageETAOrATA,
                    MainCarriageFromPortCountryCode = f.MainCarriageFromPortCountryCode,
                    MainCarriageToPortCountryCode = f.MainCarriageToPortCountryCode,
                    MainCarriageFromPortCode = f.MainCarriageFromPortCode,
                    MainCarriageFromPortCountryName = f.MainCarriageFromPortCountryName,
                    MainCarriageToPortCode = f.MainCarriageToPortCode,
                    MainCarriageToPortCountryName = f.MainCarriageToPortCountryName,
                    MainCarriageToPortName = f.MainCarriageToPortName,
                    StatusId = f.StatusId,
                    StatusDate = f.StatusDate,
                    //StatusName = f.StatusName,
                    StatusLocation = f.StatusLocation,
                    LongMaster = f.LongMaster,
                    PartnerLogoId = f.PartnerLogoId,
                    MissingDocumentsCount = f.MissingDocumentsCount,
                    MissingDocumentsCountWords = f.MissingDocumentsCountWords,
                    CancelledDate = f.CancelledDate,
                    PartnerName = f.PartnerName,
                    MissingDocsNames = f.MissingDocsNames,
                    NumberOfFollowUps = f.NumberOfFollowUps,
                    ArchivedText = f.ArchivedText,
                    CustomerId = f.CustomerId,
                    IsRequestedDocuments = f.IsRequestedDocuments,
                    RequestedDocumentsCount = f.RequestedDocumentsCount,
                    MainCarriageVesselId = f.MainCarriageVesselId,
                    MainCarriageVesselName = f.MainCarriageVesselName,
                    BookingConfirmationNumber = f.BookingConfirmationNumber,
                    DescriptionOfGoods = f.DescriptionOfGoods,
                    Notes = f.Notes,
                    IsNewARInvoiceBlocked = f.IsNewARInvoiceBlocked,
                    OperationalDate = f.OperationalDate,
                    CutoffDate = f.CutoffDate,
                    IsImporterApprovalRequried = f.IsImporterApprovalRequried,
                    ApprovedByUserName = f.ApprovedByUserName,
                    IsManifestSentToAgent = f.IsManifestSentToAgent,
                    AgentSharedManifestRef = f.AgentSharedManifestRef,
                    ValueOfGoods = f.ValueOfGoods,
                    NumberOfHouses = f.NumberOfHouses,
                    LocalCustomsTransmissionsStatusCode = f.LocalCustomsTransmissionsStatusCode,
                    LocalCustomsTransmissionsStatusName = f.LocalCustomsTransmissionsStatusName,
                    LocalCustomsTransmissionsStatusError = f.LocalCustomsTransmissionsStatusError,
                    LocalCustomsTransmissionsStatusDate = f.LocalCustomsTransmissionsStatusDate,
                    LocalCustomsSentByUserId = f.LocalCustomsSentByUserId,
                    ISFDate = f.ISFDate,
                    ISFNumber = f.ISFNumber,
                    ITDate = f.ITDate,
                    ITNumber = f.ITNumber,
                    FreightRelease = f.FreightRelease,
                    TerminalAvailable = f.TerminalAvailable,
                    OBLTypeCode = f.OBLTypeCode,
                    DocumentsClosingDate = f.DocumentsClosingDate,
                    ENSNumber = f.ENSNumber,
                    ENSDate = f.ENSDate,

                    WarehouseLegWarehouseId = f.WarehouseLegWarehouseId,
                    WarehouseLegAddressId = f.WarehouseLegAddressId,
                    WarehouseLegTerminalCode = f.WarehouseLegTerminalCode,
                    WarehouseLegExpectedEntryDate = f.WarehouseLegExpectedEntryDate,
                    WarehouseLegActualEntryDate = f.WarehouseLegActualEntryDate,
                    WarehouseLegExpectedReleaseDate = f.WarehouseLegExpectedReleaseDate,
                    WarehouseLegActualReleaseDate = f.WarehouseLegActualReleaseDate,
                    WarehouseLegLastFreeDate = f.WarehouseLegLastFreeDate,
                    WarehouseLegRemarks = f.WarehouseLegRemarks,
                    WarehouseLegReference = f.WarehouseLegReference,
                    WarehouseLegAddressCountryCode = f.WarehouseLegAddressCountryCode,
                    WarehouseLegAddressCountryName = f.WarehouseLegAddressCountryName,
                    WarehouseLegTerminalName = f.WarehouseLegTerminalName,
                    WarehouseLegEntryDate = f.WarehouseLegActualEntryDate != null ? f.WarehouseLegActualEntryDate : f.WarehouseLegExpectedEntryDate,
                    WarehouseLegReleaseDate = f.WarehouseLegActualReleaseDate != null ? f.WarehouseLegActualReleaseDate : f.WarehouseLegExpectedReleaseDate,
                    RegistryDate = f.RegistryDate,
                    TrailerNumber = f.TrailerNumber,
                    IsAssembly = f.IsAssembly,
                    LastSharedEventId = f.LastSharedEventId,
                    LastSharedEventName = f.LastSharedEventName,
                    LastSharedEventLocation = f.LastSharedEventLocation,
                    LastSharedEventNotes = f.LastSharedEventNotes,
                    LastSharedEventDate = f.LastSharedEventDate,
                    ManifestLastSharingDate = f.ManifestLastSharingDate,
                    FirstOperationalCloseDate = f.FirstOperationalCloseDate,
                    FirstAccountingCloseDate = f.FirstAccountingCloseDate,
                    DeclarationNumber = f.DeclarationNumber,
                    CustomsClearanceDate = f.CustomsClearanceDate,
                    IncludesCustoms = f.IncludesCustoms,
                    DeclarationDate = f.DeclarationDate,
                    INTTRASIError = f.INTTRASIError,
                    INTTRASIStatusCode = f.INTTRASIStatusCode,
                    INTTRASIStatusName = f.INTTRASIStatusName,
                    INTTRASIStatusDate = f.INTTRASIStatusDate,
                    INTTRABookingStatusCode = f.INTTRABookingStatusCode,
                    INTTRABookingStatusName = f.INTTRABookingStatusName,
                    INTTRABookingTransStatusName = f.INTTRABookingTransStatusName,
                    INTTRABookingTransStatusCode = f.INTTRABookingTransStatusCode,
                    INTTRABookingError = f.INTTRABookingError,
                    INTTRALastBookingResponse = f.INTTRALastBookingResponse,
                    LastFinalDestination = f.LastFinalDestination,
                    FirstPickupETA = f.FirstPickupETA,
                    FirstPickupETD = f.FirstPickupETD,
                    INTTRALastStatusDate = f.INTTRALastStatusDate,
                    Notify1Reference = f.Notify1Reference,
                    Notify2Reference = f.Notify2Reference,
                    ShipperNotExporterReference = f.ShipperNotExporterReference,
                    ConsigneeNotImporterReference = f.ConsigneeNotImporterReference,
                    ProjectNumber = f.ProjectNumber,
                    ContainerLastStatusDate = f.ContainerLastStatusDate,
                    From = f.From,
                    To = f.To,
                    Origin = f.Origin,
                    ARInvoices = f.ARInvoices,
                    NotInvoicedReceivablesAmount = f.NotInvoicedReceivablesAmount,
                    FirstARInvoiceApprovalDate = f.FirstARInvoiceApprovalDate,
                    CreatedByPartner= f.CreatedByPartner,
                    MainCarriageFinalDestinationATA = f.MainCarriageFinalDestinationATA,
                    MainCarriageFinalDestinationETA = f.MainCarriageFinalDestinationETA,
                    CreatedFromDigital = f.CreatedFromDigital,
                    ShipmentSubTypeId = f.ShipmentSubTypeId,
                    ShipmentSubTypeName = f.ShipmentSubTypeName,
                    ImportManifest = f.ImportManifest,
                    IsDangerous =f.IsDangerous,
                    DangerousUnNumber = f.DangerousUnNumber,
                    QuoteId = f.QuoteId,
                    QuoteNumber = f.QuoteNumber,
                    StatusName = !string.IsNullOrEmpty(f.StatusLocation) ? f.StatusName + "(" + f.StatusLocation + ")" : f.StatusName,
                    ExactStatusName = f.StatusName,
                    PreForwardingETD = f.PreForwardingETD,
                    IsStandalonePickupDelivery = f.IsStandalonePickupDelivery,
                };

                List<ObjectField> customFields = ObjectFieldRepository.GetCustomObjectFieldsByObjectTableName("Shipment", tenant).ToList();
                CustomFieldResolver customFieldResolver = new CustomFieldResolver();

                foreach (ObjectField field in customFields)
                {
                    PropertyInfo propInfo = typeof(ShipmentList).GetProperty(field.FieldName);
                    object newValue = customFieldResolver.GetFieldValue(myResult, field, tenant);
                    propInfo.SetValue(myResult, newValue, null);
                }
            }

            return myResult;
        }

        public ShipmentList GetSingleShipmentListForFollowup(ShipmentFollowUpDataView f, int tenant)
        {
            ShipmentList myResult = null;

            if (f != null)
            {
                TenantQuery tenantQuery = new TenantQuery(tenant);
                TenantPM currentTenant = tenantQuery.GetSinglePM(tenant);

                myResult = new ShipmentList()
                {
                    FNAReason = f.FNAReason,
                    CarrierLastStatusDate = f.CarrierLastStatusDate,
                    CarrierLastStatusName = f.CarrierLastStatusName,
                    CarrierLastStatusCode = f.CarrierLastStatusCode,
                    ShipmentViewId = f.Id + f.FollowUpId,
                    Id = f.Id,
                    Shipper = f.ShipperName,
                    Consignee = f.ConsigneeName,
                    DirectionId = f.DirectionId,
                    DirectionName = f.DirectionName,
                    TransportModeName = f.TransportModeName,
                    MasterShipmentNumber = f.MasterShipmentNumber,
                    House = f.House,
                    CreateDateTime = f.CreateDateTime,
                    ShipmentNumber = f.ShipmentNumber,
                    ShipmentType = !string.IsNullOrEmpty(f.ShipmentTypeName) ? f.ShipmentTypeName + " " + f.ShipmentLevelName : f.ShipmentLevelName,
                    ShipmentTypeId = f.ShipmentTypeId,
                    TransportModeId = f.TransportModeId,
                    Field1 = f.Field1,
                    Field2 = f.Field2,
                    Field3 = f.Field3,
                    Field4 = f.Field4,
                    Field5 = f.Field5,
                    Field6 = f.Field6,
                    Field7 = f.Field7,
                    Field9 = f.Field9,
                    Field8 = f.Field8,
                    Field10 = f.Field10,
                    Field11 = f.Field11,
                    Field12 = f.Field12,
                    Field13 = f.Field13,
                    Field14 = f.Field14,
                    Field15 = f.Field15,
                    Field16 = f.Field16,
                    Field17 = f.Field17,
                    Field18 = f.Field18,
                    Field19 = f.Field19,
                    Field20 = f.Field20,
                    Field21 = f.Field21,
                    Field22 = f.Field22,
                    Field23 = f.Field23,
                    Field24 = f.Field24,
                    Field25 = f.Field25,
                    Field26 = f.Field26,
                    Field27 = f.Field27,
                    Field28 = f.Field28,
                    Field29 = f.Field29,
                    Field30 = f.Field30,
                    Field31 = f.Field31,
                    Field32 = f.Field32,
                    Field33 = f.Field33,
                    Field34 = f.Field34,
                    Field35 = f.Field35,
                    Field36 = f.Field36,
                    Field37 = f.Field37,
                    Field38 = f.Field38,
                    Field39 = f.Field39,
                    Field40 = f.Field40,
                    ChargeableWeightInKG = f.ChargeableWeightInKG,
                    ChargeableWeight = f.ChargeableWeight,
                    GrossWeight = f.GrossWeight,
                    ShipperReference1 = f.ShipperReference1/*, Master = f.Master*/,
                    Master = f.Master,
                    OpenReceivablesInLocalCurrency = f.OpenReceivablesInLocalCurrency,
                    OpenReceivablesInProfitCurrency = f.OpenReceivablesInProfitCurrency,
                    AccountedReceivablesInLocalCurrency = f.AccountedReceivablesInLocalCurrency,
                    OpenPayablesInLocalCurrency = f.OpenPayablesInLocalCurrency,
                    ShipmentPayableStatusCode = f.ShipmentPayableStatusCode,
                    ShipmentReceivableStatusCode = f.ShipmentReceivableStatusCode,
                    ShipmentPayableStatusName = f.ShipmentPayableStatusName,
                    ShipmentReceivableStatusName = f.ShipmentReceivableStatusName,
                    ProfitInLocalCurrency = f.ProfitInLocalCurrency,
                    ProfitInProfitCurrency = f.ProfitInProfitCurrency,
                    EstimateProfitInLocalCurrency = f.EstimateProfitInLocalCurrency,
                    EstimateProfitInProfitCurrency = f.EstimateProfitInProfitCurrency,
                    BranchId = f.BranchId,
                    DepartmentId = f.DepartmentId,
                    MainCarriageETA = f.MainCarriageETA,
                    MainCarriageATD = f.MainCarriageATD,
                    LocalCurrencyCode = currentTenant.CurrencyCode,
                    NextETA = f.NextETA,
                    NextETD = f.NextETD,
                    NextLegName = f.NextLegName,
                    Routing = f.Routing,//(f.DirectionId!="D"&&f.TransportModeId!="I")?((f.PreCarriageFromPortCode != null ? f.PreCarriageFromPortCode + " > " : "") + (f.MasterShipmentDataId != null ? (f.MainCarriageFromPortCode != null ? f.MainCarriageFromPortCode + " > " : "") + (f.MainCarriageFinalDestinationPortCode != null ? (f.OnCarriageToPortCode != null ? f.MainCarriageFinalDestinationPortCode + " > " + f.OnCarriageToPortCode : f.MainCarriageFinalDestinationPortCode) : "") : ((f.FromPortCode != null ? f.FromPortCode + " > " : "") + (f.ToPortCode != null ? (f.OnCarriageToPortCode != null ? f.ToPortCode + " > " + f.OnCarriageToPortCode : f.ToPortCode) : "")))):(""),
                    FromPortId = !string.IsNullOrEmpty(f.MainCarriageFromPortId) ? f.MainCarriageFromPortId : f.FromPortId,
                    ToPortId = !string.IsNullOrEmpty(f.MainCarriageToPortId) ? f.MainCarriageToPortId : f.ToPortId,
                    FromPort = !string.IsNullOrEmpty(f.MainCarriageFromPortCode) ? f.MainCarriageFromPortCode : f.FromPortCode,
                    FromPortName = !string.IsNullOrEmpty(f.MainCarriageFromPortName) ? f.MainCarriageFromPortName : f.FromPortName,
                    FromPortCountry = f.MainCarriageFromPortCountryName,

                    //ToPort = !string.IsNullOrEmpty(f.MainCarriageFinalDestinationPortCode) ? f.MainCarriageFinalDestinationPortCode : f.ToPortCode,

                    // Column: To
                    ToPort = (f.TransportModeId == "I" && f.DirectionId == "D") ? f.MainCarriageToCity : f.ToPort,

                    ToPortName = !string.IsNullOrEmpty(f.MainCarriageFinalDestinationPortName) ? f.MainCarriageFinalDestinationPortName : f.ToPortName,
                    ToPortCountry = f.MainCarriageToPortCountryName,
                    MasterShipmentDataId = f.MasterShipmentDataId,
                    BranchName = f.BranchName,
                    MoveTypeName = f.MoveTypeName,
                    CustomerName = f.CustomerName,
                    GrossWeightInKG = f.GrossWeightInKG,
                    GrossWeightPerStorageDays = f.GrossWeightPerStorageDays,
                    GrossWeightPerTon = f.GrossWeightPerTon,
                    ShipmentLevelCode = f.ShipmentLevelCode,
                    ShipmentLevelName = f.ShipmentLevelName,
                    VolumetricWeight = f.VolumetricWeight,
                    AirlinePrefix = f.AirlinePrefix,
                    AccountedPayablesInLocalCurrency = f.AccountedPayablesInLocalCurrency,
                    AccountedPayablesInProfitCurrency = f.AccountedPayablesInProfitCurrency,
                    AccountedReceivablesInProfitCurrency = f.AccountedReceivablesInProfitCurrency,
                    MainCarriageFromPortId = f.MainCarriageFromPortId,

                    // Column: Origin
                    MainCarriageFromPortName = (f.TransportModeId == "I" && f.DirectionId == "D") ? f.MainCarriageFromCity : f.MainCarriageFromPortName,

                    MainCarriageATA = f.MainCarriageATA,
                    MainCarriageETD = f.MainCarriageETD,
                    IncotermId = f.IncotermId,
                    OpenPayablesInProfitCurrency = f.OpenPayablesInProfitCurrency,
                    CustomerReference1 = f.CustomerReference1,
                    CustomerReference2 = f.CustomerReference2,
                    IssuingCarrierAgentId = f.IssuingCarrierAgentId,
                    IncotermCode = f.IncotermCode,
                    AsAgreedFreight = f.AsAgreedFreight,
                    AsAgreedOtherCharges = f.AsAgreedOtherCharges,
                    AccountNumber = f.AccountNumber,
                    FollowUpDate = f.FollowUpDate,
                    FollowUpId = f.FollowUpId,
                    FollowUpNotes = f.FollowUpNotes,
                    FollowUpOwner = f.FollowUpOwner,
                    FollowUpOwnerId = f.FollowUpOwnerId,
                    FollowUpType = f.FollowUpType,
                    FollowUpTypeId = f.FollowUpTypeId,
                    VolumeInCBM = f.VolumeInCBM,
                    AgentId = f.AgentId,
                    AgentComputed = f.AgentComputed,
                    ARInvoiceIssued = f.ARInvoiceIssued,
                    CreditNoteIssued = f.CreditNoteIssued,
                    FreightForwarderId = f.FreightForwarderId,
                    FreightForwarderName = f.FreightForwarderName,
                    ProductCode = f.ProductCode,
                    IsAccountingClosed = f.IsAccountingClosed,
                    IsOperationalClosed = f.IsOperationalClosed,
                    OperationalCloseDate = f.OperationalCloseDate,
                    AccountingCloseDate = f.AccountingCloseDate,
                    PackagesQuantity = f.PackagesQuantity,
                    FHLStatusCode = f.FHLStatusCode,
                    FHLStatusName = f.FHLStatusName,
                    FHLStatusDate = f.FHLStatusDate,
                    FWBStatusCode = f.FWBStatusCode,
                    FWBStatusName = f.FWBStatusName,
                    FWBStatusDate = f.FWBStatusDate,
                    CargonautFHLStatusCode = f.CargonautFHLStatusCode,
                    CargonautFHLStatusName = f.CargonautFHLStatusName,
                    CargonautFHLStatusDate = f.CargonautFHLStatusDate,
                    CargonautFWBStatusCode = f.CargonautFWBStatusCode,
                    CargonautFWBStatusName = f.CargonautFWBStatusName,
                    CargonautFWBStatusDate = f.CargonautFWBStatusDate,
                    NumberOfInsidePackages = f.NumberOfInsidePackages,
                    NumberOfInsidePackagesDetails = f.NumberOfInsidePackagesDetails,
                    ConsolidatorId = f.ConsolidatorId,
                    ConsolidatorName = f.ConsolidatorName,
                    ConsolidatorNote = f.ConsolidatorNote,
                    ConsolidatorAddressId = f.ConsolidatorAddressId,
                    ConsolidatorContactId = f.ConsolidatorContactId,
                    ConsolidatorReference = f.ConsolidatorReference,
                    ManifestReason = f.ManifestReason,
                    ManifestStatusCode = f.ManifestStatusCode,
                    FromPortCountryCode = f.FromPortCountryCode,
                    ToPortCountryCode = f.ToPortCountryCode,
                    StatusId = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusId : f.ShipmentStatusId) : (f.ShipmentStatusId),
                    StatusDate = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusDate : f.ShipmentStatusDate) : (f.ShipmentStatusDate),
                    StatusName = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusName : f.ShipmentStatusName) : (f.ShipmentStatusName),
                    StatusLocation = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusLocation : f.ShipmentStatusLocation) : (f.ShipmentStatusLocation),
                    LongMaster = f.TransportModeId == "A" ? (!string.IsNullOrEmpty(f.AirlinePrefix) && !string.IsNullOrEmpty(f.Master) ? f.AirlinePrefix + "-" + f.Master : "") : f.Master,
                    CustomsDeclarationNumber = f.CustomsDeclarationNumber,
                    OperationalDate = f.OperationalDate,
                    CutoffDate = f.CutoffDate,
                    NumberOfHouses = f.NumberOfHouses,
                    ValueOfGoods = f.ValueOfGoods,
                    ISFDate = f.ISFDate,
                    ISFNumber = f.ISFNumber,
                    ITDate = f.ITDate,
                    ITNumber = f.ITNumber,
                    FreightRelease = f.FreightRelease,
                    TerminalAvailable = f.TerminalAvailable,
                    OBLTypeCode = f.OBLTypeCode,
                    DocumentsClosingDate = f.DocumentsClosingDate,
                    ENSNumber = f.ENSNumber,
                    ENSDate = f.ENSDate,
                    WarehouseLegWarehouseId = f.WarehouseLegWarehouseId,
                    WarehouseLegTerminalName = f.WarehouseLegTerminalName,
                    WarehouseLegAddressId = f.WarehouseLegAddressId,
                    WarehouseLegTerminalCode = f.WarehouseLegTerminalCode,
                    WarehouseLegExpectedEntryDate = f.WarehouseLegExpectedEntryDate,
                    WarehouseLegActualEntryDate = f.WarehouseLegActualEntryDate,
                    WarehouseLegExpectedReleaseDate = f.WarehouseLegExpectedReleaseDate,
                    WarehouseLegActualReleaseDate = f.WarehouseLegActualReleaseDate,
                    WarehouseLegLastFreeDate = f.WarehouseLegLastFreeDate,
                    WarehouseLegRemarks = f.WarehouseLegRemarks,
                    WarehouseLegReference = f.WarehouseLegReference,
                    RegistryDate = f.RegistryDate,
                    IsAssembly = f.IsAssembly,
                    LastSharedEventId = f.LastSharedEventId,
                    LastSharedEventName = f.LastSharedEventName,
                    LastSharedEventLocation = f.LastSharedEventLocation,
                    LastSharedEventNotes = f.LastSharedEventNotes,
                    LastSharedEventDate = f.LastSharedEventDate,
                    FirstOperationalCloseDate = f.FirstOperationalCloseDate,
                    FirstAccountingCloseDate = f.FirstAccountingCloseDate,
                    INTTRASIStatusName = f.INTTRASIStatusName,
                    LastFinalDestination = f.LastFinalDestination,
                    FirstPickupETA = f.FirstPickupETA,
                    FirstPickupETD = f.FirstPickupETD,
                    INTTRALastStatusDate = f.INTTRALastStatusDate,
                    Notify1Reference = f.Notify1Reference,
                    Notify2Reference = f.Notify2Reference,
                    ShipperNotExporterReference = f.ShipperNotExporterReference,
                    ConsigneeNotImporterReference = f.ConsigneeNotImporterReference,
                    ProjectNumber = f.ProjectNumber,
                    ContainerLastStatusDate = f.ContainerLastStatusDate,
                    From = f.From,
                    To = f.To,
                    Origin = f.Origin,
                    DeclarationDate = f.DeclarationDate,
                    DeclarationNumber = f.DeclarationNumber,
                    ARInvoices = f.ARInvoices,
                    Notes = f.Notes,
                    EstimatedFinalArrivalDate = f.EstimatedFinalArrivalDate,
                    NotInvoicedReceivablesAmount = f.NotInvoicedReceivablesAmount,
                    IsDangerous = f.IsDangerous,
                    DangerousUnNumber = f.DangerousUnNumber,
                    MainCarriageVesselName = f.MainCarriageVesselName,
                    BookingConfirmationNumber = f.BookingConfirmationNumber,
                    IsStandalonePickupDelivery = f.IsStandalonePickupDelivery,
                };

                List<ObjectField> customFields = ObjectFieldRepository.GetCustomObjectFieldsByObjectTableName("Shipment", tenant).ToList();
                CustomFieldResolver customFieldResolver = new CustomFieldResolver();

                foreach (ObjectField field in customFields)
                {
                    PropertyInfo propInfo = typeof(ShipmentList).GetProperty(field.FieldName);
                    object newValue = customFieldResolver.GetFieldValue(myResult, field, tenant);
                    propInfo.SetValue(myResult, newValue, null);
                }
            }

            return myResult;
        }

        public ShipmentList GetShipmentListBycustomFileIdForMobile(string customFileId, int tenant)
        {

            ShipmentList shipmentList = (from entity in repository.context.Shipments
                                         where entity.Id == customFileId && entity.Tenant == tenant && !entity.IsCancelled
                                         select new ShipmentList()
                                         {
                                             Id = entity.Id,
                                             DirectionId = entity.DirectionId,
                                             ShipmentLevelCode = entity.ShipmentLevelCode,
                                             ShipperReference1 = entity.ShipperReference1,
                                             ShipperReference2 = entity.ShipperReference2,
                                             AgentReference1 = entity.AgentReference1,
                                             AgentReference2 = entity.AgentReference2,
                                             ConsigneeReference1 = entity.ConsigneeReference1,
                                             ConsigneeReference2 = entity.ConsigneeReference2,

                                         }).FirstOrDefault();

            return shipmentList;
        }



        public List<ShipmentList> GetShipmentListsByMasterIdAndTenant(string masterId, int tenant)
        {

            List<ShipmentList> shipmentLists = (from s in repository.context.Shipments
                                                where s.Tenant == tenant && s.MasterShipmentDataId == masterId && s.Id != masterId && ((s.ShipmentLevelCode == "H") || (s.ShipmentLevelCode == "D") || s.ShipmentLevelCode == "A")
                                                select new ShipmentList()
                                                {
                                                    Id = s.Id,
                                                    ShipmentNumber = s.ShipmentNumber,
                                                    ShipmentLevelCode = s.ShipmentLevelCode,
                                                    AgentId = s.AgentId,

                                                }).ToList();

            return shipmentLists;

        }

        public bool CheckIfShipmentExistByAgentSharedManifestRef(string agentSharedManifestRef, int tenant)
        {

            string shipmentId = (from a in repository.context.Shipments
                                 where a.Tenant == tenant && a.AgentSharedManifestRef == agentSharedManifestRef
                                 select a.Id).FirstOrDefault();

            return !string.IsNullOrEmpty(shipmentId) ? true : false;

        }

        public string GetShipmentIdByShipmentNumber(string agentRef, int tenant)
        {

            string shipmentId = (from a in repository.context.Shipments
                                 where a.Tenant == tenant && a.ShipmentNumber == agentRef && !a.IsCancelled
                                 select a.Id).FirstOrDefault();

            return shipmentId;

        }


        public string GetShipmentIdByForwarderShipmentNumber(string forwarderShipmentNumber, int tenant)
        {
            string shipmentId = (from a in repository.context.Shipments
                                 where a.Tenant == tenant && a.ForwarderShipmentNumber == forwarderShipmentNumber && !a.IsCancelled
                                 select a.Id).FirstOrDefault();

            return shipmentId;

        }


        public string GetShipmentByAgentSharedManifestRef(string agentSharedManifestRef, int tenant)
        {

            string shipmentId = (from a in repository.context.Shipments
                                 where a.Tenant == tenant && a.AgentSharedManifestRef == agentSharedManifestRef
                                 select a.Id).FirstOrDefault();

            return shipmentId;

        }

        public string GetShipmentNumberByShipmentId(string shipmentId, int tenant)
        {

            string shipmentNumber = (from a in repository.context.Shipments
                                     where a.Tenant == tenant && a.Id == shipmentId
                                     select a.ShipmentNumber).FirstOrDefault();

            return shipmentNumber;

        }

        public List<ShipmentPM> GetShipmentsForInventoryReport(List<string> shipmentIds)
        {
            IQueryable<Shipment> shipments = repository.GetShipmentsForUnpaidInvoicesReport(shipmentIds);
            List<ShipmentPM> shipmentPMs = (from a in shipments
                                            select new ShipmentPM()
                                            {
                                                Id = a.Id,
                                                TransportModeId = a.TransportModeId,
                                                ShipperName = a.ShipperCard != null ? a.ShipperCard.EnglishName : null,
                                                DirectionId = a.DirectionId,
                                                ConsigneeName = a.ConsigneeCard != null ? a.ConsigneeCard.EnglishName : null,
                                                ShipmentLevelCode = a.ShipmentLevelCode,
                                                ShipmentNumber = a.ShipmentNumber,
                                            }).ToList();
            return shipmentPMs;
        }

        public ShipmentList GetShipmentListByIdForInventoryReport(string id, int tenant)
        {

            ShipmentList shipmentList = (from a in repository.context.Shipments.Include("Ports")
                                         join sm in repository.context.ShipmentMasterDatas.Include("Port")
                                         on a.MasterShipmentDataId equals sm.Id into shipmentJoin
                                         from m in shipmentJoin.DefaultIfEmpty()
                                         where a.Tenant == tenant && a.Id == id
                                         select new ShipmentList
                                         {
                                             Id = a.Id,
                                             Tenant = a.Tenant,
                                             CustomerId = a.CustomerId,
                                             ShipmentNumber = a.ShipmentNumber,
                                             CustomerReference1 = a.CustomerReference1,
                                             CustomerReference2 = a.CustomerReference2,
                                             FromPortName = !string.IsNullOrEmpty(m.MainCarriageFromPort.Code) ? m.MainCarriageFromPort.Code : a.FromPort.Code,
                                             ToPortName = !string.IsNullOrEmpty(m.MainCarriageToPort.Code) ? m.MainCarriageToPort.Code : a.ToPort.Code,
                                             WarehouseLegWarehouseId = a.WarehouseLegWarehouseId,
                                             LongMaster = a.TransportModeId == "A" ? (m.AirlinePrefix != null && m.Master != null ? m.AirlinePrefix + "-" + m.Master : m.Master) : m.Master,
                                             House = a.House,
                                         }).FirstOrDefault();





            return shipmentList;
        }


        public bool CheckIfShipmentExistByShipmentNumber(string shipmentNumber, int tenant)
        {

            bool exist = (from a in repository.context.Shipments
                          where a.ShipmentNumber == shipmentNumber && a.Tenant == tenant
                          select a.Id).Any();
            return exist;

        }

        public ShipmentPM GetSingleShipmentPMBySecurityKeyTenant(string key, int tenant)
        {
            if (!string.IsNullOrEmpty(key))
            {

                Shipment shipment = null;

                shipment = (from a in repository.context.Shipments.Include("EntityStatus").Include("ShipmentType").Include("Incoterm").Include("ShipmentReceivableStatus").Include("ShipmentPayableStatus").Include("ShipmentLevel").Include("NextLeg").Include("ShipmentType").Include("ShipmentMasterData")
                            where a.SecurityKey == key && !a.IsCancelled && a.Tenant == tenant
                            select a).FirstOrDefault();



                if (shipment != null)
                {
                    ShipmentMasterData masterData = (from a in repository.context.ShipmentMasterDatas
                                                     where a.Id == shipment.MasterShipmentDataId
                                                     select a).FirstOrDefault();

                    ShipmentPM shipmentPM = new ShipmentPM();

                    shipmentPM = MapShipmentToShipmentPM(shipmentPM, shipment, null, masterData, true);
                    var CLoudData = (from a in repository.context.ShipmentAdditionalCloudDatas
                                     where a.Id == shipment.Id
                                     select a).FirstOrDefault();
                    if (CLoudData != null)
                    {
                        shipmentPM.DeclarationXMLData = CLoudData.DeclarationXmlData;
                        shipmentPM.DeclarationWCOXml = CLoudData.DeclarationWCOXml;
                        shipmentPM.ApproveDateTime = CLoudData.ApproveDateTime;
                        shipmentPM.IsImporterApprovalRequired = CLoudData.IsImporterApprovalRequried;
                        shipmentPM.VersionApproved = CLoudData.VersionApproved;
                        shipmentPM.ShipmentAddtionalDataXML = CLoudData.ShipmentAddtionalDataXML;
                        shipmentPM.SendUpdatesToAgentEnabled = CLoudData.SendUpdatesToAgentEnabled;
                        shipmentPM.DocsSentToAgent = CLoudData.DocsSentToAgent;
                        shipmentPM.ApprovedBy = CLoudData.ApprovedByUserName;
                        shipmentPM.DocumentsApprovedByUserName = CLoudData.DocumentsApprovedByUserName;
                        shipmentPM.IsUserIDNumberRequired = CLoudData.IsUserIDNumberRequired;
                        shipmentPM.UserIdNumberUpdateDate = CLoudData.UserIdNumberUpdateDate;
                        shipmentPM.UserIdNumberXMLData = CLoudData.UserIdNumberXMLData;
                        shipmentPM.UserIdNumber = CLoudData.UserIdNumber;
                        shipmentPM.PaymentRequestXML = CLoudData.PaymentRequestXML;
                        shipmentPM.PaymentDateTime = CLoudData.PaymentDateTime;
                        shipmentPM.IsPaymentRequired = CLoudData.IsPaymentRequired;

                    }
                    return shipmentPM;
                }
            }

            return null;
        }

        public ShipmentAdditionalCloudCustomData GetSingleShipmentAdditionalCloudCustomData(string key, int tenant)
        {
            if (!string.IsNullOrEmpty(key))
            {

                ShipmentAdditionalCloudCustomData data = null;

                data = (from a in repository.context.Shipments
                            join b in repository.context.ShipmentAdditionalCloudDatas on a.Id equals b.Id
                            where a.SecurityKey == key && !a.IsCancelled && a.Tenant == tenant
                            select new ShipmentAdditionalCloudCustomData
                            {
                                ShipmentNumber = a.ShipmentNumber,
                                IsPaymentRequired = b.IsPaymentRequired,
                                PaymentDateTime = b.PaymentDateTime,
                                IsImporterApprovalRequried = b.IsImporterApprovalRequried,
                                ApprovedByUserName = b.ApprovedByUserName,
                                VersionApproved = b.VersionApproved,
                                ApproveDateTime = b.ApproveDateTime,
                                DenyReason = b.DenyReason,
                                DeclarationXmlData = b.DeclarationXmlData,
                                PaymentRequestXML = b.PaymentRequestXML,
                                DocumentsApprovedByUserName = b.DocumentsApprovedByUserName,
                            }).FirstOrDefault();

                return data;

                
            }

            return null;
        }

        public IQueryable<ShipmentList> GetShipmentListsByCustomerIdsAndDates(List<string> customerIds, DateTime? fromDate, DateTime? toDate, int tenant)
        {

            IQueryable<ShipmentList> result = Enumerable.Empty<ShipmentList>().AsQueryable();

            IQueryable<ShipmentList> shipmentLists = (from s in repository.context.Shipments.Include("EntityStatus")
                                                      where s.Tenant == tenant && customerIds.Contains(s.CustomerId) && !string.IsNullOrEmpty(s.CustomerShipmentNumber) && !s.IsCancelled && s.CreateDateTime >= fromDate && s.CreateDateTime <= toDate
                                                      select new ShipmentList()
                                                      {
                                                          Id = s.Id,
                                                          ShipmentNumber = s.ShipmentNumber,
                                                          CreateDateTime = s.CreateDateTime,
                                                          StatusName = s.EntityStatus.Name,
                                                          CustomerTenantNumber = s.CustomerTenantNumber,
                                                          CustomerId = s.CustomerId,
                                                      });

            if (shipmentLists.Count() > 0)
            {
                CustomerTenantAccessQuery customerTenantAccessQuery = new CustomerTenantAccessQuery(tenant);
                List<int?> customerTenantNumbers = shipmentLists.GroupBy(d => d.CustomerTenantNumber).Select(d => d.FirstOrDefault().CustomerTenantNumber).ToList();
                if (customerTenantNumbers.Count > 0)
                {
                    List<int> customerTenants = customerTenantAccessQuery.GetCustomerTenantAccessListsByCustomer(customerTenantNumbers, "A").ToList();
                    result = shipmentLists.Where(d => customerTenants.Contains((int)d.CustomerTenantNumber));
                }
            }



            return result;

        }



        public bool CheckIfShipmentCreateFromManinfest(string agentSharedManifestId, int tenant)
        {
            bool result = (from a in repository.context.Shipments
                           where a.Tenant == tenant && a.AgentSharedManifestRef == agentSharedManifestId
                           select a).Any();

            return result;

        }


        public int? GetCustomerTenantByShipmentNumber(string shipmentNumber, int tenant)
        {

            int? result = (from a in repository.context.Shipments
                           where a.Tenant == tenant && a.ShipmentNumber == shipmentNumber
                           select a.CustomerTenantNumber).FirstOrDefault();


            return result;

        }
        public IQueryable<Shipment> GetAllShipments()
        {
            return (from d in repository.context.Shipments  select d);
        }
        public IQueryable<ShipmentList> GetAllShipmentListTenant(int tenant)
        {
            IQueryable<ShipmentList> shipmentsList = from s in repository.context.Shipments.Include("ShipmentType").Include("ShipmentLevel")
                                                     join sm in repository.context.ShipmentMasterDatas
                                                     on s.MasterShipmentDataId equals sm.Id into shipmentJoin
                                                     from m in shipmentJoin.DefaultIfEmpty()
                                                     where s.Tenant == tenant
                                                     select new ShipmentList()
                                                     {
                                                         BookingConfirmationNumber =m.BookingConfirmationNumber,
                                                         ProfitExchangeRate = s.ProfitExchangeRate,
                                                         OpenPayablesInLocalCurrency = s.OpenPayablesInLocalCurrency,
                                                         AccountedPayablesInLocalCurrency = s.AccountedPayablesInLocalCurrency,
                                                         OpenReceivablesInLocalCurrency = s.OpenReceivablesInLocalCurrency,
                                                         AccountedReceivablesInLocalCurrency = s.AccountedReceivablesInLocalCurrency,
                                                         ProfitInLocalCurrency = s.ProfitInLocalCurrency,
                                                         OpenPayablesInProfitCurrency = s.OpenPayablesInProfitCurrency,
                                                         AccountedPayablesInProfitCurrency = s.AccountedPayablesInProfitCurrency,
                                                         OpenReceivablesInProfitCurrency = s.OpenReceivablesInProfitCurrency,
                                                         AccountedReceivablesInProfitCurrency = s.AccountedReceivablesInProfitCurrency,
                                                         ProfitInProfitCurrency = s.ProfitInProfitCurrency,
                                                         AgentId = s.AgentId,
                                                         AgentComputed = s.AgentComputed,
                                                         AgentName = s.AgentCard != null ? s.AgentCard.EnglishName : null,
                                                         AgentReference1 = s.AgentReference1,
                                                         AgentReference2 = s.AgentReference2,
                                                         BookingNumberOfPackages = s.BookingNumberOfPackages,
                                                         BranchId = s.BranchId,
                                                         ChargeableWeightInKG = s.ChargeableWeightInKG,
                                                         ConsigneeId = s.ConsigneeId,
                                                         ConsigneeName = s.ConsigneeCard != null ? s.ConsigneeCard.EnglishName : null,
                                                         ConsigneeReference1 = s.ConsigneeReference1,
                                                         ConsigneeReference2 = s.ConsigneeReference2,
                                                         CreateDateTime = s.CreateDateTime,
                                                         AWBCurrencyCode = s.AWBCurrency != null ? s.AWBCurrency.Code : null,
                                                         CustomerId = s.CustomerId,
                                                         CustomerName = s.CustomerCard != null ? s.CustomerCard.EnglishName : null,
                                                         CustomerReference1 = s.CustomerReference1,
                                                         CustomerReference2 = s.CustomerReference2,
                                                         DepartmentId = s.DepartmentId,
                                                         ChargeableWeightUnitCode = s.ChargeableWeightUnitCode,
                                                         DirectionId = s.DirectionId,
                                                         DirectionName = s.Direction.Name,
                                                         EstimateProfitInLocalCurrency = s.EstimateProfitInLocalCurrency,
                                                         EstimateProfitInProfitCurrency = s.EstimateProfitInProfitCurrency,
                                                         MainCarriageFinalDestinationETA = m.MainCarriageFinalDestinationETA,
                                                         MainCarriageFinalDestinationATA = m.MainCarriageFinalDestinationATA,
                                                         MainCarriageFinalDestinationPortCode = m.MainCarriageFinalDestinationPort == null ? null : m.MainCarriageFinalDestinationPort.Code,
                                                         FreightForwarderId = s.FreightForwarderId,
                                                         FreightForwarderName = s.FreightForwarderCard != null ? s.FreightForwarderCard.EnglishName : null,
                                                         FromPort = m.MainCarriageFromPort.Code,
                                                         FromPortCountry = m.MainCarriageFromPort.Country.Code,
                                                         FromPortName = m.MainCarriageFromPort.EnglishName,
                                                         GrossWeightInKG = s.GrossWeightInKG,
                                                         GrossWeightPerStorageDays = s.GrossWeightPerStorageDays,
                                                         GrossWeightPerTon = s.GrossWeightPerTon,
                                                         House = s.House,
                                                         Id = s.Id,
                                                         IncotermId = s.IncotermId,
                                                         IncotermCode = s.Incoterm != null ? s.Incoterm.Code : null,
                                                         IsAccountingClosed = s.IsAccountingClosed,
                                                         IsCancelled = s.IsCancelled,
                                                         CancelledDate = s.CancelledDate,
                                                         ShipmentLevelCode = s.ShipmentLevelCode,
                                                         IsOperationalClosed = s.IsOperationalClosed,
                                                         AccountingCloseDate = s.AccountingCloseDate,
                                                         OperationalCloseDate = s.AccountingCloseDate,
                                                         LastUpdateDate = s.LastUpdateDate,
                                                         MainCarriageATA = m.MainCarriageATA,
                                                         MainCarriageATD = m.MainCarriageATD,
                                                         MainCarriageCarrierCode = m.MainCarriageCarrierCard != null ? m.MainCarriageCarrierCard.Code : null,
                                                         MainCarriageCarrierId = m.MainCarriageCarrierId,
                                                         MainCarriageCarrierName = m.MainCarriageCarrierCard != null ? m.MainCarriageCarrierCard.EnglishName : null,
                                                         MainCarriageCarrierNumber = m.MainCarriageCarrierNumber,
                                                         MainCarriageETA = m.MainCarriageETA,
                                                         MainCarriageETD = m.MainCarriageETD,
                                                         MainCarriageFromPortCode = m.MainCarriageFromPort.Code,
                                                         MainCarriageFromPortCountryCode = m.MainCarriageFromPort.Country.Code,
                                                         MainCarriageFromPortCountryName = m.MainCarriageFromPort.Country.EnglishName,
                                                         MainCarriageFromPortId = m.MainCarriageFromPortId,
                                                         MainCarriageFromPortName = m.MainCarriageFromPort.EnglishName,
                                                         MainCarriageToPortCode = m.MainCarriageToPort.Code,
                                                         MainCarriageToPortCountryCode = m.MainCarriageToPort.Country.Code,
                                                         MainCarriageToPortCountryName = m.MainCarriageToPort.Country.EnglishName,
                                                         MainCarriageToPortName = m.MainCarriageToPort.EnglishName,
                                                         Master = m.Master,
                                                         NextETA = s.NextETA,
                                                         NextETD = s.NextETD,
                                                         NextLegCode = s.NextLegCode,
                                                         NextLegName = s.NextLeg != null ? s.NextLeg.Name : null,
                                                         PackagesQuantity = s.PackagesQuantity,
                                                         NumberOfContainers = s.NumberOfContainers,
                                                         NumberOfFollowUps = s.NumberOfFollowUps,
                                                         NumberOfPackages = s.NumberOfPackages,
                                                         PreCarriageETD = m.PreCarriageETD,
                                                         OrderChargeableWeight = s.OrderChargeableWeight,
                                                         OrderVolumetricWeight = s.OrderVolumetricWeight,
                                                         TransportModeId = s.TransportModeId,
                                                         Tenant = s.Tenant,
                                                         ToPort = m.MainCarriageToPort.Code,
                                                         ToPortCountry = m.MainCarriageToPort.Country.Code,
                                                         ToPortName = m.MainCarriageToPort.EnglishName,
                                                         TransportModeName = s.TransportMode.Name,
                                                         SalesmanUserId = s.SalesmanUserId,
                                                         AccountManagerUserId = s.AccountManagerUserId,
                                                         MasterShipmentDataId = s.MasterShipmentDataId,
                                                         ShipmentPayableStatusCode = s.ShipmentPayableStatusCode,
                                                         ShipmentPayableStatusName = s.ShipmentPayableStatus != null ? s.ShipmentPayableStatus.Name : null,
                                                         ShipmentNumber = s.ShipmentNumber,
                                                         ShipmentReceivableStatusCode = s.ShipmentReceivableStatusCode,
                                                         ShipmentReceivableStatusName = s.ShipmentReceivableStatus != null ? s.ShipmentReceivableStatus.Name : null,
                                                         ShipperId = s.ShipperId,
                                                         ShipperName = s.ShipperCard != null ? s.ShipperCard.EnglishName : null,
                                                         ShipperReference1 = s.ShipperReference1,
                                                         ShipperReference2 = s.ShipperReference2,
                                                         UpdatedByUserId = s.UpdatedByUserId,
                                                         VolumeInCBM = s.VolumeInCBM,
                                                         VolumetricWeight = s.VolumetricWeight,
                                                         QuoteId = s.QuoteId,
                                                         QuoteNumber = s.QuoteNumber,
                                                         MainCarriageFullCarrierNumber = (m.MainCarriageCarrierNumber != null && m.MainCarriageCarrierCard != null) ? m.MainCarriageCarrierCard.Code + m.MainCarriageCarrierNumber : null,
                                                         Transshipment1FullCarrierNumber = (m.Transshipment1CarrierNumber != null && m.Transshipment1CarrierPrefix != null) ? m.Transshipment1CarrierPrefix + m.Transshipment1CarrierNumber : null,
                                                         Transshipment2FullCarrierNumber = (m.Transshipment2CarrierNumber != null && m.Transshipment2CarrierPrefix != null) ? m.Transshipment2CarrierPrefix + m.Transshipment2CarrierNumber : null,
                                                         Transshipment3FullCarrierNumber = (m.Transshipment3CarrierNumber != null && m.Transshipment3CarrierPrefix != null) ? m.Transshipment3CarrierPrefix + m.Transshipment3CarrierNumber : null,
                                                         AsAgreedFreight = s.AsAgreedFreight,
                                                         AsAgreedOtherCharges = s.AsAgreedOtherCharges,
                                                         AccountNumber = s.AccountNumber,
                                                         FinalArrivalDate = s.FinalArrivalDate,
                                                         EstimatedFinalArrivalDate = s.EstimatedFinalArrivalDate,
                                                         ActualFinalArrivalDate = s.ActualFinalArrivalDate,
                                                         AMSBL = s.AMSBL,
                                                         CASSCode = s.CASSCode,
                                                         FreelancerId = s.FreelancerId,
                                                         FreelancerAddressId = s.FreelancerAddressId,
                                                         FreelancerContactId = s.FreelancerContactId,
                                                         ARInvoiceIssued = s.ARInvoiceIssued,
                                                         CreditNoteIssued = s.CreditNoteIssued,
                                                         ProductCode = s.ProductCode,
                                                         LastStatusLogDate = s.LastStatusLogDate,
                                                         ExceptionDescription = s.ExceptionDescription,
                                                         ExceptionDate = s.ExceptionDate,
                                                         HasException = s.HasException,
                                                         ExceptionResolvedDescription = s.ExceptionResolvedDescription,
                                                         LastExceptionDescription = s.LastExceptionDescription,
                                                         CustomConnectToShipment = s.CustomConnectToShipment,
                                                         CustomsDeclarationNumber = s.CustomsDeclarationNumber,
                                                         NumberOfInsidePackages = s.NumberOfInsidePackages,
                                                         NumberOfInsidePackagesDetails = s.NumberOfInsidePackagesDetails,
                                                         ComputedStatusId = s.ComputedStatusId,
                                                         ComputedStatusDate = s.ComputedStatusDate,
                                                         StatusId = s.StatusId,
                                                         StatusName = s.EntityStatus != null ? s.EntityStatus.Name : null,
                                                         StatusDate = s.StatusDate,
                                                         StatusLocation = s.StatusLocation,
                                                         ForeignPartnerCountryCode = s.ForeignPartnerCountryCode,
                                                         DescriptionOfGoods = s.DescriptionOfGoods,
                                                         AgentSharedManifestRef = s.AgentSharedManifestRef,
                                                         IsManifestSentToAgent = s.IsManifestSentToAgent,
                                                         IsNewARInvoiceBlocked = s.IsNewARInvoiceBlocked,
                                                         OperationalDate = s.OperationalDate,
                                                         CutoffDate = m.CutoffDate,
                                                         ValueOfGoods = s.ValueOfGoods,
                                                         ISFDate = s.ISFDate,
                                                         ISFNumber = s.ISFNumber,
                                                         ITDate = s.ITDate,
                                                         ITNumber = s.ITNumber,
                                                         FreightRelease = s.FreightRelease,
                                                         TerminalAvailable = s.TerminalAvailable,
                                                         OBLTypeCode = m.OBLTypeCode,
                                                         DocumentsClosingDate = m.DocumentsClosingDate,
                                                         TEU = s.TEU,
                                                         ENSNumber = s.ENSNumber,
                                                         ENSDate = s.ENSDate,
                                                         RegistryDate = s.RegistryDate,
                                                         IsAssembly = s.IsAssembly,
                                                         FirstOperationalCloseDate = s.FirstOperationalCloseDate,
                                                         FirstAccountingCloseDate = s.FirstAccountingCloseDate,
                                                         ShipmentTypeId = s.ShipmentTypeId,
                                                         LastFinalDestination = s.LastFinalDestination,
                                                         FirstPickupETA = s.FirstPickupETA,
                                                         FirstPickupETD = s.FirstPickupETD,
                                                         ShipmentType = s.ShipmentType == null ? null : s.ShipmentType.Name,
                                                         ShipmentLevelName = s.ShipmentLevel == null ? null : s.ShipmentLevel.Name,
                                                         CreatedByUserName = s.CreatedByUser == null ? null : s.CreatedByUser.Contact.EnglishName,
                                                         SpecialServicesTypeName = s.SpecialServicesType == null ? null : s.SpecialServicesType.EnglishName,
                                                         PreCarriageFromPortId = m.PreCarriageFromPortId,
                                                         OnCarriageToPortId = m.OnCarriageToPortId,
                                                         Transshipment1ToPortId = m.Transshipment1ToPortId,
                                                         Transshipment2ToPortId = m.Transshipment2ToPortId,
                                                         Transshipment3ToPortId = m.Transshipment3ToPortId,
                                                         MainCarriageToPortId = m.MainCarriageToPortId,
                                                         From = s.From,
                                                         To = s.To,
                                                         Origin = s.Origin,
                                                         CreatedByPartner = s.CreatedByPartner,
                                                         PreForwardingFromPortId = s.PreForwardingFromPortId,
                                                         OnForwardingToPortId = s.OnForwardingToPortId,
                                                         IsStandalonePickupDelivery = s.IsStandalonePickupDelivery,
                                                     };

            return shipmentsList;
        }

        public IQueryable<ShipmentList> GetShipmentListsByFromCreateDateAndToCreateDate(DateTime fromCreateDate , DateTime toCreateDate , int tenant)
        {

            IQueryable<ShipmentList> shipmentsLists = from s in repository.context.Shipments
                                                     where s.Tenant == tenant &&   s.CreateDateTime >= fromCreateDate && s.CreateDateTime <= toCreateDate
                                                      select new ShipmentList
                                                     {
                                                         Id= s.Id ,
                                                         Tenant = s.Tenant ,
                                                         ShipmentNumber = s.ShipmentNumber,
                                                     };
            return shipmentsLists;
        }



        public List<ShipmentList> GetShipmentsForCrossDock(List<string> shipmentIds , int tenant)
        {
            IQueryable<Shipment> shipments = repository.GetShipmentsForCrossDock(shipmentIds, tenant);
            List<ShipmentList> shipmentShipmentLists = (from a in shipments
                                            select new ShipmentList()
                                            {
                                                 Id = a.Id,
                                                 TransportModeId = a.TransportModeId,
                                                 DirectionId = a.DirectionId,
                                                 Routing  = a.Routing,
                                                 DirectionName = a.Direction!=null ? a.Direction.Name:null,
                                                 TransportModeName = a.TransportMode!=null? a.TransportMode.Name:null,
                                            }).ToList();
            return shipmentShipmentLists;
        }

        public bool CheckIsCFSShipmentById(string shipmentId, int tenant)
        {
            bool isCFS = (from a in repository.context.Shipments
                             where a.Tenant == tenant && a.Id == shipmentId
                             select a.IsCFSWarehouse).FirstOrDefault();

            return isCFS;
        }

        public List<HouseMaster> GetHouseShipmentsByShipmentNumbers(List<string> ids, int tenant)
        {
            var shipments = (from a in repository.context.Shipments
                             where ids.Contains(a.ShipmentNumber) && a.Tenant == tenant && a.ShipmentLevelCode == "H"
                             select a);

            var masterNumbers =
            (from shipment in shipments
             join masterData in repository.context.ShipmentMasterDatas on shipment.MasterShipmentDataId equals masterData.Id
             select new HouseMaster()
             {
                 HouseId =  shipment.Id,
                 MasterNumber = masterData.MasterShipmentNumber
             }).ToList();

            return masterNumbers;
        }

        public ShipmentPM GetShipmentPMForCargoTrackingByEntityId(string id, int tenant)
        {
            Shipment shipment = repository.GetShipmentForCargoTracking(id, tenant);

            var shipmentPM = new ShipmentPM();

            ShipmentMasterData masterData = (from a in repository.context.ShipmentMasterDatas
                                             where a.Id == shipment.MasterShipmentDataId
                                             select a).FirstOrDefault();

            MapShipmentToShipmentPM(shipmentPM, shipment, null, masterData, false);

            CreateShipmentPMForCargoTracking(tenant, shipment, shipmentPM);

            SetShipmentCloudDataFields(shipment, shipmentPM);

            return shipmentPM;
        }

        private void CreateShipmentPMForCargoTracking(int tenant, Shipment shipment, ShipmentPM shipmentPM)
        {
            if (shipmentPM != null)
            {
                shipmentPM.Id = shipment.Id;
                shipmentPM.CustomFileNumber = shipment.CustomFileNumber;
                shipmentPM.ShipmentTypeName = shipment.ShipmentType?.Name;
                shipmentPM.IncotermName = shipment.Incoterm?.Name;
                shipmentPM.IncotermCode = shipment.Incoterm?.Code;
                shipmentPM.WarehouseLegEnglishName = shipment.WarehouseLegCard?.EnglishName;
                shipmentPM.WarehouseLegLocalName = shipment.WarehouseLegCard?.LocalName;
                shipmentPM.PackagesTypesNames = GetShipmentPackagesTypeNames(tenant, shipment.Id);
                shipmentPM.NumberOfPackages = GetShipmentPackagesQuantity(tenant, shipment.Id);
                shipmentPM.Volume = GetShipmentPackagesVolume(tenant, shipment.Id);
            }
        }

        private static Address GetCardAddress(int tenant, string cardId)
        {
            AddressRepository addressRepository = new AddressRepository(tenant);
            Address shipperAddress = addressRepository.GetSingleAddress(cardId, tenant);
            return shipperAddress;
        }

        private static void SetShipmentCloudDataFields(Shipment shipment, ShipmentPM shipmentPM)
        {
            if (shipment.ShipmentAdditionalCloudData != null)
            {
                ShipmentCloudCustomDataDeserializer deserializer = new ShipmentCloudCustomDataDeserializer();
                var cloudCustomData = deserializer.BuildCustomDataFromXML(shipment.ShipmentAdditionalCloudData);
                shipmentPM.TotalTax = cloudCustomData?.TotalTax;
            }
        }

        private string GetShipmentPackagesTypeNames(int tenant, string shipmentId)
        {
            List<ShipmentPackagePM> shipmentPackages = GetPackagesOfShipment(tenant, shipmentId);
            string combinedPackagesTypesNames = GetCombinedPackagesTypesNames(shipmentPackages);
            return combinedPackagesTypesNames;
        }
        private int GetShipmentPackagesQuantity(int tenant, string shipmentId)
        {
            List<ShipmentPackagePM> shipmentPackages = GetPackagesOfShipment(tenant, shipmentId);
            int? quantity = shipmentPackages.Sum(package => package.Quantity);
            return quantity ?? 0;
        }
        private double GetShipmentPackagesVolume(int tenant, string shipmentId)
        {
            List<ShipmentPackagePM> shipmentPackages = GetPackagesOfShipment(tenant, shipmentId);
            double? quantity = shipmentPackages.Sum(package => package.Volume);
            return quantity ?? 0;
        }

        private static string GetCombinedPackagesTypesNames(List<ShipmentPackagePM> shipmentPackages)
        {
            var packagesTypesNames = shipmentPackages.Select(package => package.PackageTypeName).ToList();
            var combinedPackagesTypesNames = string.Join(";", packagesTypesNames);
            return combinedPackagesTypesNames;
        }

        public List<ShipmentPackagePM> GetPackagesOfShipment(int tenant, string shipmentId)
        {
            var shipmentIds = new List<string>() { shipmentId };
            ShipmentPackageQuery shipmentPackageQuery = new ShipmentPackageQuery(tenant);
            var shipmentPackages = shipmentPackageQuery.GetShipmentPackages(shipmentIds, tenant);
            return shipmentPackages;
        }

        public Tuple<string, string> GetShipmentFieldsForPickUpDelivery(string shipmentId, int tenant)
        {
            string bookingConfirmationNumber = "", shipmentNumber = "";
            if (!string.IsNullOrEmpty(shipmentId))
            {
                var shipmentSelectedData = (from a in repository.context.Shipments
                                    where a.Id == shipmentId && a.Tenant == tenant
                                    select new
                                    {
                                        ShipmentNumber = a.ShipmentNumber,
                                        MasterShipmentDataId = a.MasterShipmentDataId
                                    }).FirstOrDefault();
                shipmentNumber = shipmentSelectedData.ShipmentNumber;
                if (shipmentSelectedData != null)
                {
                    bookingConfirmationNumber = (from a in repository.context.ShipmentMasterDatas
                                                 where a.Id == shipmentSelectedData.MasterShipmentDataId
                                                 select a.BookingConfirmationNumber).FirstOrDefault();
                }
            }
            return Tuple.Create(shipmentNumber, bookingConfirmationNumber);
        }


        public CargoTrackingShipmentCustomsData GetCargoTrackingShipmentCustomsData(string shipmentId, int tenant)
        {
            Shipment shipment = repository.GetShipmentForCargoTracking(shipmentId, tenant);

            if (shipment.ShipmentAdditionalCloudData == null) return null;
            
            ShipmentAdditionalCloudCustomData cloudCustomData = GetDeserializedCloudCustomData(shipment.ShipmentAdditionalCloudData);
            CargoTrackingShipmentCustomsData shipmentCustomsData = null;
            if (cloudCustomData != null)
            {
                shipmentCustomsData = BuildCargoTrackingShipmentCustomsData(cloudCustomData, tenant);
            }

            return shipmentCustomsData;
            
        }

        private CargoTrackingShipmentCustomsData BuildCargoTrackingShipmentCustomsData(ShipmentAdditionalCloudCustomData cloudCustomData, int tenant)
        {
            return new CargoTrackingShipmentCustomsData()
            {
                DeclarationNumber = cloudCustomData.DeclarationNo,
                TotalValueInNIS = Convert.ToDecimal(cloudCustomData.GoodsValue),
                TotalValueInForeignCurrency = cloudCustomData.GoodsValueDetails == null ? 0 : cloudCustomData.GoodsValueDetails.Sum(good => Convert.ToDecimal(good.Value)),
                GoodsDescription = cloudCustomData.MishgorDescOfGoods1,
                TotalTax = Convert.ToDecimal(cloudCustomData.TotalTax),
                ImporterVatAmount = CalculateImporterVatAmountFromCloudCustomData(cloudCustomData),
                TaxDetails = BuildCargoTrackingShipmentCustomTaxDetails(cloudCustomData),
                CurrencyCode = cloudCustomData.GoodsValueDetails == null ? null : cloudCustomData.GoodsValueDetails.FirstOrDefault()?.CurrencyName,
                CurrencySign = GetCurrencySignFromCloudCustomData(cloudCustomData, tenant)
            };

        }

        private static string GetCurrencySignFromCloudCustomData(ShipmentAdditionalCloudCustomData cloudCustomData, int tenant)
        {
            if (cloudCustomData.GoodsValueDetails == null)
                return null;

            CurrencyQuery currencyQuery = new CurrencyQuery(tenant);
            string currencyCode = cloudCustomData.GoodsValueDetails.FirstOrDefault()?.CurrencyName;
            var currency = currencyQuery.GetSinglePMByCode(currencyCode, tenant);
            string sign = currency?.Sign;
            return sign;
        }

        private decimal CalculateImporterVatAmountFromCloudCustomData(ShipmentAdditionalCloudCustomData cloudCustomData)
        {
            if (cloudCustomData.TaxesDetails == null)
                return 0;
            return cloudCustomData.TaxesDetails.Where(detail => detail.TaxTypeCode == "15")
                                                .Sum(detail => Convert.ToDecimal(detail.TaxAmount));
        }
        private List<CargoTrackingShipmentCustomTaxDetails> BuildCargoTrackingShipmentCustomTaxDetails(ShipmentAdditionalCloudCustomData cloudCustomData)
        {
            if (cloudCustomData.TaxesDetails == null)
                return new List<CargoTrackingShipmentCustomTaxDetails>();

            return cloudCustomData.TaxesDetails.Select(detail => new CargoTrackingShipmentCustomTaxDetails()
            {
                TaxTypeName = detail.Taxtypename,
                TaxAmount = Convert.ToDecimal(detail.TaxAmount),
                TaxBasis = detail.TaxBasis
            }).ToList();
        }

        private static ShipmentAdditionalCloudCustomData GetDeserializedCloudCustomData(ShipmentAdditionalCloudData shipmentAdditionalCloudData)
        {
            ShipmentCloudCustomDataDeserializer deserializer = new ShipmentCloudCustomDataDeserializer();
            var cloudCustomData = deserializer.BuildCustomDataFromXML(shipmentAdditionalCloudData);
            return cloudCustomData;
        }
    }

    public class CargoTrackingShipmentCustomsData
    {
        public string DeclarationNumber { get; set; }
        public string DeclarationStatus { get; set; }
        public string CurrencySign { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public string GoodsDescription { get; set; }
        public decimal ImporterVatAmount { get; set; }
        public decimal TotalValueInNIS { get; set; }
        public decimal TotalValueInForeignCurrency { get; set; }
        public decimal TotalTax { get; set; }

        public List<CargoTrackingShipmentCustomTaxDetails> TaxDetails;
    }
    public class CargoTrackingShipmentCustomTaxDetails
    {
        public string TaxTypeName { get; set; }
        public string TaxBasis{ get; set; }
        public decimal TaxAmount { get; set; }
    }


    public class DeparturesArrivalsDataItem
    {
        public string Id { get; set; }
        public string DirectionId { get; set; }
        public string TransportModeId { get; set; }
        public string MainCarriageCarrierId { get; set; }
        public string MainCarriageCarrierNumber { get; set; }
        public string TruckNumber { get; set; }
        public DateTime? MainCarriageETD { get; set; }
        public DateTime? MainCarriageATD { get; set; }
        public DateTime? MainCarriageETA { get; set; }
        public DateTime? MainCarriageATA { get; set; }
        public DateTime? DepartureArrivalToDate { get; set; }
        public DateTime? DepartureArrivalFromDate { get; set; }
        public DateTime? MainCarriageFinalDestinationETA { get; set; }
        public DateTime? MainCarriageFinalDestinationATA { get; set; }
    }

    public class ShipmentJoinPayablesList
    {

    }

    public class ShipmentsQueriesCountsArgs
    {
        public int Tenant { get; set; }
        public string TransportModeId { get; set; }
        public string DirectionId { get; set; }
        public string SearchFilter { get; set; }
        public string ServiceContextUser { get; set; }
        public string TypeCode { get; set; }
        public string ForwarderPartnerId { get; set; }
    }

    public class HouseMaster
    {
        public string HouseId { get; set; }
        public string MasterNumber { get; set; }
    }


}
