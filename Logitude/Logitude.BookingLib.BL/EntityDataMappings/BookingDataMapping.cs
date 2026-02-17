
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.BL.EntityPMs; 
using Logitude.BookingLib.Data;
using Simplog.Server.Infrastructure;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BookingLib.Data.Repositories;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;

namespace Logitude.BookingLib.BL.EntityDataMappings
{
   
   public partial class BookingDataMapping: IMapping<BookingPM, Booking>
   {
        public void CustomPMToPOCO(BookingPM entityPM, Booking entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.ConcurrencyGUID);
            entityPOCO.Id = entityPM.Id;

            entityPOCO.ConcurrencyGUID = Guid.NewGuid().ToString();
            entityPM.ConcurrencyGUID = entityPOCO.ConcurrencyGUID;

            BuildRoutingField(entityPM, entityPOCO);
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert ? true : false);
        }

        public void CustomPOCOToPM(BookingPM entityPM, Booking entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.MainCarriageCarrierName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.LongMaster);
            this.CustomMappedPMProperties.Add(PMPropertyNames.BookingStatusName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.FFRStatusName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.FirstFlight);
            this.CustomMappedPMProperties.Add(PMPropertyNames.BookingLevelName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.BookingProductName);

            BookingStatusRepository bookingStatusRepository = new BookingStatusRepository(entityPOCO.Tenant);
            BookingStatus bookingStatus = bookingStatusRepository.GetSingle(entityPOCO.BookingStatusCode);
            if (bookingStatus != null)
            {
                entityPM.BookingStatusName = bookingStatus.Name;
            }

            FFRStatusRepository ffrStatusRepository = new FFRStatusRepository(entityPOCO.Tenant);
            FFRStatus ffrStatus = ffrStatusRepository.GetSingle(entityPOCO.FFRStatusCode);
            if (ffrStatus != null)
            {
                entityPM.FFRStatusName = ffrStatus.Name;
            }

            BookingLevelRepository bookingLevelRepository = new BookingLevelRepository(entityPOCO.Tenant);
            BookingLevel bookingLevel = bookingLevelRepository.GetSingle(entityPOCO.BookingLevelCode);
            if (bookingLevel != null)
            {
                entityPM.BookingLevelName = bookingLevel.Name;
            }

            Card cardObject = CardRepository.GetSingleCard(entityPOCO.MainCarriageCarrierId, entityPOCO.Tenant, true);
            Card card1Object = CardRepository.GetSingleCard(entityPOCO.Transshipment1CarrierId, entityPOCO.Tenant, true);
            Card card2Object = CardRepository.GetSingleCard(entityPOCO.Transshipment2CarrierId, entityPOCO.Tenant, true);

            if (cardObject != null)
            {
                entityPM.MainCarriageCarrierName = cardObject.EnglishName;
                entityPM.MainCarriageCarrierCode = cardObject.Code;

                //if (cardObject.Airline != null)
                //{
                //    entityPM.AirlineAccountNumber = cardObject.Airline.AccountNumber;
                //}
            }

            if (!string.IsNullOrEmpty(entityPOCO.MainCarriageCarrierNumber) && !string.IsNullOrEmpty(entityPOCO.MainCarriageCarrierPrefix))
            {
                entityPM.FirstFlight = entityPOCO.MainCarriageCarrierPrefix + entityPOCO.MainCarriageCarrierNumber;
            }

            if (card1Object != null)
            {
                entityPM.Transshipment1CarrierName = card1Object.EnglishName;
            }

            if (card2Object != null)
            {
                entityPM.Transshipment2CarrierName = card2Object.EnglishName;
            }

            if (!string.IsNullOrEmpty(entityPOCO.AirlinePrefix) && !string.IsNullOrEmpty(entityPOCO.Master))
            {
                entityPM.LongMaster = entityPOCO.AirlinePrefix + "-" + entityPOCO.Master;
            }

            AirlineRepository airlineRepository = new AirlineRepository(entityPOCO.Tenant);

            if (!string.IsNullOrEmpty(entityPOCO.MainCarriageCarrierId))
            {
                Airline airline = airlineRepository.GetSingleAirline(entityPOCO.MainCarriageCarrierId, entityPOCO.Tenant);
                if (airline != null)
                {
                    entityPM.CarrierIsChampRegistered = airline.IsChampRegistered;
                    entityPM.CarrierIsGLSHKRegistered = airline.IsGLSHKRegistered;
                    entityPM.CarrierIsCheckDigit = airline.CheckDigit;
                    entityPM.CarrierIsLimitedLength = airline.LimitedLength;

                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        AirlineRepository tenantZeroAirlineRepository = new AirlineRepository(0);
                        Airline tenantZeroAirline = tenantZeroAirlineRepository.GetSingleAirlineByCode(airline.Card.Code, 0);
                        if (tenantZeroAirline != null)
                        {
                            entityPM.TenantZeroAirlineId = tenantZeroAirline.Id;
                            entityPM.TenantZeroAirlineTTY = tenantZeroAirline.TTY;
                            entityPM.TenantZeroAirlinePIMA = tenantZeroAirline.GLSHKPIMA;
                            entityPM.TenantZeroAirlineChampFFR = tenantZeroAirline.ChampFFRFFA;
                            entityPM.TenantZeroAirlineChampFVR = tenantZeroAirline.ChampFVRFVA;
                            entityPM.TenantZeroAirlineGLSHKFFR = tenantZeroAirline.GLSHKFFRFFA;
                            entityPM.TenantZeroAirlineGLSHKFVR = tenantZeroAirline.GLSHKFVRFVA;
                            entityPM.ZeroChampNeedsRegistration = tenantZeroAirline.ChampNeedsRegistration;
                            entityPM.ZeroGLSHKNeedsRegistration = tenantZeroAirline.GLSHKNeedsRegistration;
                            entityPM.TenantZeroIsManagingProduct = tenantZeroAirline.IsManagingProduct;
                            entityPM.TenantZeroIsProductMandatory = tenantZeroAirline.IsProductMandatory;
                            entityPM.ZeroIsDescOfGoodsFromList = tenantZeroAirline.IsDescriptionOfGoodsFromList;
                            entityPM.TenantZeroAirlineChampFSRFSA = tenantZeroAirline.ChampFSRFSA;
                            entityPM.TenantZeroAirlineGLSHKFSRFSA = tenantZeroAirline.GLSHKFSRFSA;
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(entityPOCO.InterlineId))
            {
                Airline airline = airlineRepository.GetSingleAirline(entityPOCO.InterlineId, entityPOCO.Tenant);
                if (airline != null)
                {
                    entityPM.CarrierIsCheckDigit = airline.CheckDigit;
                    entityPM.CarrierIsLimitedLength = airline.LimitedLength;
                }
            }

            PortRepository portRep = new PortRepository(entityPOCO.Tenant);
            Port mainCarriageFromPort = portRep.GetSinglePort(entityPOCO.Tenant, entityPOCO.MainCarriageFromPortId);
            if (mainCarriageFromPort != null)
            {
                entityPM.MainFromPortCode = mainCarriageFromPort.Code;
                entityPM.MainFromPortName = mainCarriageFromPort.EnglishName;
                entityPM.MainFromPortCountryCode = mainCarriageFromPort.Country.Code;
                entityPM.MainFromPortCountryName = mainCarriageFromPort.Country.EnglishName;
            }

            Port mainCarriageToPort = portRep.GetSinglePort(entityPOCO.Tenant, entityPOCO.MainCarriageToPortId);
            if (mainCarriageToPort != null)
            {
                entityPM.MainToPortCode = mainCarriageToPort.Code;
                entityPM.MainToPortName = mainCarriageToPort.EnglishName;
                entityPM.MainToPortCountryCode = mainCarriageToPort.Country.Code;
                entityPM.MainToPortCountryName = mainCarriageToPort.Country.EnglishName;            
            }

            Port transshipment1FromPort = portRep.GetSinglePort(entityPOCO.Tenant, entityPOCO.Transshipment1FromPortId);
            if (transshipment1FromPort != null)
            {
                entityPM.Trans1FromPortCode = transshipment1FromPort.Code;
                entityPM.Trans1FromPortName = transshipment1FromPort.EnglishName;
                entityPM.Trans1FromPortCountryCode = transshipment1FromPort.Country.Code;
                entityPM.Trans1FromPortCountryName = transshipment1FromPort.Country.EnglishName;
            }

            Port transshipment1ToPort = portRep.GetSinglePort(entityPOCO.Tenant, entityPOCO.Transshipment1ToPortId);
            if (transshipment1ToPort != null)
            {
                entityPM.Trans1ToPortCode = transshipment1ToPort.Code;
                entityPM.Trans1ToPortName = transshipment1ToPort.EnglishName;
                entityPM.Trans1ToPortCountryCode = transshipment1ToPort.Country.Code;
                entityPM.Trans1ToPortCountryName = transshipment1ToPort.Country.EnglishName;
            }

            Port transshipment2FromPort = portRep.GetSinglePort(entityPOCO.Tenant, entityPOCO.Transshipment2FromPortId);
            if (transshipment2FromPort != null)
            {
                entityPM.Trans2FromPortCode = transshipment2FromPort.Code;
                entityPM.Trans2FromPortName = transshipment2FromPort.EnglishName;
                entityPM.Trans2FromPortCountryCode = transshipment2FromPort.Country.Code;
                entityPM.Trans2FromPortCountryName = transshipment2FromPort.Country.EnglishName;
            }

            Port transshipment2ToPort = portRep.GetSinglePort(entityPOCO.Tenant, entityPOCO.Transshipment2ToPortId);
            if (transshipment2ToPort != null)
            {
                entityPM.Trans2ToPortCode = transshipment2ToPort.Code;
                entityPM.Trans2ToPortName = transshipment2ToPort.EnglishName;
                entityPM.Trans2ToPortCountryCode = transshipment2ToPort.Country.Code;
                entityPM.Trans2ToPortCountryName = transshipment2ToPort.Country.EnglishName;
            }

            Port finalDestinationPort = portRep.GetSinglePort(entityPOCO.Tenant, entityPOCO.MainCarriageFinalDestinationPortId);
            if (finalDestinationPort != null)
            {
                entityPM.FinalDestinationPortCode = finalDestinationPort.Code;
                entityPM.FinalDestinationPortName = finalDestinationPort.EnglishName;
            }

            AddressRepository addressRepository = new AddressRepository(entityPOCO.Tenant);
            Address shipperAddress = addressRepository.GetSingleAddress(entityPOCO.ShipperAddressId, entityPOCO.Tenant);
            if (shipperAddress != null)
            {
                entityPM.ShipperAddress1 = shipperAddress.Address1;
                entityPM.ShipperAddress2 = shipperAddress.Address2;
                entityPM.ShipperCity = shipperAddress.City;
                entityPM.ShipperCountryId = shipperAddress.CountryId;
                entityPM.ShipperStateId = shipperAddress.StateId;
                entityPM.ShipperZipCode = shipperAddress.ZipCode;
            }

            Address consigneeAddress = addressRepository.GetSingleAddress(entityPOCO.ConsigneeAddressId, entityPOCO.Tenant);
            if (consigneeAddress != null)
            {
                entityPM.ConsigneeAddress1 = consigneeAddress.Address1;
                entityPM.ConsigneeAddress2 = consigneeAddress.Address2;
                entityPM.ConsigneeCity = consigneeAddress.City;
                entityPM.ConsigneeCountryId = consigneeAddress.CountryId;
                entityPM.ConsigneeStateId = consigneeAddress.StateId;
                entityPM.ConsigneeZipCode = consigneeAddress.ZipCode;
            }

            BookingProductRepository bookingProductRepository = new BookingProductRepository(entityPOCO.Tenant);
            BookingProduct bookingProduct = bookingProductRepository.GetSingle(entityPOCO.BookingProductId);
            if (bookingProduct != null)
            {
                entityPM.BookingProductName = bookingProduct.Name;
            }

            BookingSpaceAllocationRepository spaceAllocationRepository = new BookingSpaceAllocationRepository(entityPOCO.Tenant);
            BookingSpaceAllocation mainSpaceAllocation = spaceAllocationRepository.GetSingle(entityPOCO.MainCarriageSpaceAllocationCode);
            BookingSpaceAllocation transshipment1paceAllocation = spaceAllocationRepository.GetSingle(entityPOCO.Transshipment1SpaceAllocationCode);
            BookingSpaceAllocation transshipment2paceAllocation = spaceAllocationRepository.GetSingle(entityPOCO.Transshipment2SpaceAllocationCode);

            if (mainSpaceAllocation != null)
            {
                entityPM.MainCarriageSpaceAllocationName = mainSpaceAllocation.Name;
            }

            if (transshipment1paceAllocation != null)
            {
                entityPM.Transshipment1SpaceAllocationName = transshipment1paceAllocation.Name;
            }

            if (transshipment2paceAllocation != null)
            {
                entityPM.Transshipment2SpaceAllocationName = transshipment2paceAllocation.Name;
            }

            if (!string.IsNullOrEmpty(entityPOCO.ShipmentId))
            {
                ShipmentRepository shipmentRepository = new ShipmentRepository(entityPOCO.Tenant);
                Shipment connectedShipment = shipmentRepository.GetSingleShipment(entityPOCO.ShipmentId, entityPOCO.Tenant);

                if(connectedShipment != null)
                {
                    entityPM.ShipmentNumber = connectedShipment.ShipmentNumber;
                }
            }

            if (!string.IsNullOrEmpty(entityPOCO.DescriptionOfGoodsId))
            {
                AWBDescriptionOfGoodsRepository awbRepository = new AWBDescriptionOfGoodsRepository(entityPOCO.Tenant);
                AWBDescriptionOfGoods awbDescriptionOfGoods = awbRepository.GetSingleAWBDescriptionOfGoods(entityPOCO.DescriptionOfGoodsId);

                if (awbDescriptionOfGoods != null)
                {
                    entityPM.DescriptionOfGoodsService = awbDescriptionOfGoods.Service;
                }
            }
        }

        private void BuildRoutingField(BookingPM entityPM, Booking entityPOCO)
        {
            int tenant = entityPM.Tenant;

            string fromCode = "";
            string toCode = "";

            if (!string.IsNullOrEmpty(entityPM.MainCarriageFromPortId))
            {
                PortPM myPort = PortQuery.GetSinglePort(tenant, entityPM.MainCarriageFromPortId, true);
                if (myPort != null)
                {
                    fromCode = myPort.Code;
                }
            }

            if (!string.IsNullOrEmpty(entityPM.Transshipment2ToPortId))
            {
                PortPM myPort = PortQuery.GetSinglePort(tenant, entityPM.Transshipment2ToPortId, true);
                if (myPort != null)
                {
                    toCode = myPort.Code;
                }
            }

            else if (!string.IsNullOrEmpty(entityPM.Transshipment1ToPortId))
            {
                PortPM myPort = PortQuery.GetSinglePort(tenant, entityPM.Transshipment1ToPortId, true);
                if (myPort != null)
                {
                    toCode = myPort.Code;
                }
            }

            else if (!string.IsNullOrEmpty(entityPM.MainCarriageToPortId))
            {
                PortPM myPort = PortQuery.GetSinglePort(tenant, entityPM.MainCarriageToPortId, true);
                if (myPort != null)
                {
                    toCode = myPort.Code;
                }
            }

            string myRouting = fromCode + " , " + toCode;
            entityPM.Routing = myRouting;
            entityPOCO.Routing = myRouting;
        }

        private void BuildSearchFields(BookingPM entityPM, Booking entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";

            int tenant = entityPOCO.Tenant;

            if (!string.IsNullOrEmpty(entityPM.BookingNumber))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityPM.BookingNumber : mySearchFields + "," + entityPM.BookingNumber;
            }

            if (!string.IsNullOrEmpty(entityPM.Master))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityPM.Master : mySearchFields + "," + entityPM.Master;
            }

            if (!string.IsNullOrEmpty(entityPM.AWBCarrierTarrifReference))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityPM.AWBCarrierTarrifReference : mySearchFields + "," + entityPM.AWBCarrierTarrifReference;
            }

            if (!string.IsNullOrEmpty(entityPM.BookingStatusCode))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityPM.BookingStatusCode : mySearchFields + "," + entityPM.BookingStatusCode;

                BookingStatusRepository myBookingStatusRepository = new BookingStatusRepository(tenant);
                BookingStatus myBookingStatus = myBookingStatusRepository.GetSingle(entityPM.BookingStatusCode);
                if (myBookingStatus != null)
                {
                    mySearchFields = string.IsNullOrEmpty(mySearchFields) ? myBookingStatus.Name : mySearchFields + "," + myBookingStatus.Name;
                }
            }

            if (!string.IsNullOrEmpty(entityPM.ShipmentId))
            {
                ShipmentRepository myShipmentRepository = new ShipmentRepository(tenant);
                Shipment myShipment = myShipmentRepository.GetSingleShipment(entityPM.ShipmentId, tenant);
                if (myShipment != null)
                {
                    mySearchFields = string.IsNullOrEmpty(mySearchFields) ? myShipment.ShipmentNumber : mySearchFields + "," + myShipment.ShipmentNumber;
                }
            }

            if (!string.IsNullOrEmpty(entityPM.MainCarriageCarrierId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.MainCarriageCarrierId, tenant, true);
                if (myCard != null)
                {
                    mySearchFields = string.IsNullOrEmpty(mySearchFields) ? myCard.Code : mySearchFields + "," + myCard.Code;
                    mySearchFields = string.IsNullOrEmpty(mySearchFields) ? myCard.EnglishName : mySearchFields + "," + myCard.EnglishName;
                }
            }

            if (!string.IsNullOrEmpty(entityPM.MainCarriageCarrierNumber))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityPM.MainCarriageCarrierNumber : mySearchFields + "," + entityPM.MainCarriageCarrierNumber;
            }

            if (!string.IsNullOrEmpty(entityPM.MainCarriageFromPortId))
            {
                PortPM myPort = PortQuery.GetSinglePort(tenant, entityPM.MainCarriageFromPortId, true);
                if (myPort != null)
                {
                    mySearchFields = string.IsNullOrEmpty(mySearchFields) ? myPort.Code : mySearchFields + "," + myPort.Code;
                    mySearchFields = string.IsNullOrEmpty(mySearchFields) ? myPort.EnglishName : mySearchFields + "," + myPort.EnglishName;
                    mySearchFields = string.IsNullOrEmpty(mySearchFields) ? myPort.CountryCode : mySearchFields + "," + myPort.CountryCode;
                    mySearchFields = string.IsNullOrEmpty(mySearchFields) ? myPort.CountryName : mySearchFields + "," + myPort.CountryName;
                    }
                }

            if (!string.IsNullOrEmpty(entityPM.Transshipment1FromPortId))
            {
                PortPM myPort = PortQuery.GetSinglePort(tenant, entityPM.Transshipment1FromPortId, true);
                if (myPort != null)
                {
                    mySearchFields = string.IsNullOrEmpty(mySearchFields) ? myPort.Code : mySearchFields + "," + myPort.Code;
                    mySearchFields = string.IsNullOrEmpty(mySearchFields) ? myPort.EnglishName : mySearchFields + "," + myPort.EnglishName;
                    mySearchFields = string.IsNullOrEmpty(mySearchFields) ? myPort.CountryCode : mySearchFields + "," + myPort.CountryCode;
                    mySearchFields = string.IsNullOrEmpty(mySearchFields) ? myPort.CountryName : mySearchFields + "," + myPort.CountryName;
                    }
                }

            if (!string.IsNullOrEmpty(entityPM.Transshipment2FromPortId))
            {
                PortPM myPort = PortQuery.GetSinglePort(tenant, entityPM.Transshipment2FromPortId, true);
                if (myPort != null)
                {
                    mySearchFields = string.IsNullOrEmpty(mySearchFields) ? myPort.Code : mySearchFields + "," + myPort.Code;
                    mySearchFields = string.IsNullOrEmpty(mySearchFields) ? myPort.EnglishName : mySearchFields + "," + myPort.EnglishName;
                    mySearchFields = string.IsNullOrEmpty(mySearchFields) ? myPort.CountryCode : mySearchFields + "," + myPort.CountryCode;
                    mySearchFields = string.IsNullOrEmpty(mySearchFields) ? myPort.CountryName : mySearchFields + "," + myPort.CountryName;
                }
            }

            if (!string.IsNullOrEmpty(entityPM.MainCarriageToPortId))
            {
                PortPM myPort = PortQuery.GetSinglePort(tenant, entityPM.MainCarriageToPortId, true);
                if (myPort != null)
                {
                    mySearchFields = string.IsNullOrEmpty(mySearchFields) ? myPort.Code : mySearchFields + "," + myPort.Code;
                    mySearchFields = string.IsNullOrEmpty(mySearchFields) ? myPort.EnglishName : mySearchFields + "," + myPort.EnglishName;
                    mySearchFields = string.IsNullOrEmpty(mySearchFields) ? myPort.CountryCode : mySearchFields + "," + myPort.CountryCode;
                    mySearchFields = string.IsNullOrEmpty(mySearchFields) ? myPort.CountryName : mySearchFields + "," + myPort.CountryName;
                }
            }

            if (!string.IsNullOrEmpty(entityPM.Transshipment1ToPortId))
            {
                PortPM myPort = PortQuery.GetSinglePort(tenant, entityPM.Transshipment1ToPortId, true);
                if (myPort != null)
                {
                    mySearchFields = string.IsNullOrEmpty(mySearchFields) ? myPort.Code : mySearchFields + "," + myPort.Code;
                    mySearchFields = string.IsNullOrEmpty(mySearchFields) ? myPort.EnglishName : mySearchFields + "," + myPort.EnglishName;
                    mySearchFields = string.IsNullOrEmpty(mySearchFields) ? myPort.CountryCode : mySearchFields + "," + myPort.CountryCode;
                    mySearchFields = string.IsNullOrEmpty(mySearchFields) ? myPort.CountryName : mySearchFields + "," + myPort.CountryName;
                }
            }

            if (!string.IsNullOrEmpty(entityPM.Transshipment2ToPortId))
            {
                PortPM myPort = PortQuery.GetSinglePort(tenant, entityPM.Transshipment2ToPortId, true);
                if (myPort != null)
                {
                    mySearchFields = string.IsNullOrEmpty(mySearchFields) ? myPort.Code : mySearchFields + "," + myPort.Code;
                    mySearchFields = string.IsNullOrEmpty(mySearchFields) ? myPort.EnglishName : mySearchFields + "," + myPort.EnglishName;
                    mySearchFields = string.IsNullOrEmpty(mySearchFields) ? myPort.CountryCode : mySearchFields + "," + myPort.CountryCode;
                    mySearchFields = string.IsNullOrEmpty(mySearchFields) ? myPort.CountryName : mySearchFields + "," + myPort.CountryName;
                }
            }

            if (!string.IsNullOrEmpty(entityPM.ShipperId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.ShipperId, tenant, true);
                if (myCard != null)
                {
                    mySearchFields = string.IsNullOrEmpty(mySearchFields) ? myCard.EnglishName : mySearchFields + "," + myCard.EnglishName;
                }

                if (!string.IsNullOrEmpty(entityPM.ShipperReference))
                {
                    mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityPM.ShipperReference : mySearchFields + "," + entityPM.ShipperReference;
                }
            }


            if (!string.IsNullOrEmpty(entityPM.ConsigneeId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.ConsigneeId, tenant, true);
                if (myCard != null)
                {
                    mySearchFields = string.IsNullOrEmpty(mySearchFields) ? myCard.EnglishName : mySearchFields + "," + myCard.EnglishName;
                }

                if (!string.IsNullOrEmpty(entityPM.ConsigneeReference))
                {
                    mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityPM.ConsigneeReference : mySearchFields + "," + entityPM.ConsigneeReference;
                }
            }

            if (!string.IsNullOrEmpty(entityPM.IssuingCarrierAgentId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.IssuingCarrierAgentId, tenant, true);
                if (myCard != null)
                {
                    mySearchFields = string.IsNullOrEmpty(mySearchFields) ? myCard.EnglishName : mySearchFields + "," + myCard.EnglishName;
                }
            }
            
            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;  
        }        
   }
}
   