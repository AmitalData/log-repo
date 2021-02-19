using System;
using System.Collections.Generic;
using System.Linq;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ShipmentDeliveryQuery
    {
        ShipmentPickUpDeliveryRepository repository;         
        public ShipmentDeliveryQuery(int tenant)
        {
            repository = new ShipmentPickUpDeliveryRepository(tenant);
        }
        public ShipmentDeliveryQuery(ShipmentPickUpDeliveryRepository repository)
        {
            this.repository = repository;
        }

        public ShipmentDeliveryPM GetSinglePM(string id, int tenant)
        {
            ShipmentPickUpDelivery entityPOCO = repository.GetSingleShipmentPickUpDelivery(tenant, id);
            ShipmentDeliveryPM entityPM = null;

            if (entityPOCO != null)
            {
                entityPM = new ShipmentDeliveryPM()
                {
                    Id = entityPOCO.Id,
                    Tenant = entityPOCO.Tenant,
                    ShipmentId = entityPOCO.ShipmentId,
                    PickUpDeliveryNumber = entityPOCO.PickUpDeliveryNumber,
                    ATA = entityPOCO.ATA,
                    ATD = entityPOCO.ATD,
                    ETA = entityPOCO.ETA,
                    ETD = entityPOCO.ETD,
                    Driver = entityPOCO.Driver,
                    TruckNumber = entityPOCO.TruckNumber,
                    Notes = entityPOCO.Notes,
                    TrailerNumber = entityPOCO.TrailerNumber,
                    CarrierId = entityPOCO.CarrierId,
                    CarrierCode = entityPOCO.CarrierCard == null ? null : entityPOCO.CarrierCard.Code,
                    CarrierName = entityPOCO.CarrierCard == null ? null : entityPOCO.CarrierCard.EnglishName,
                    CarrierWebSite = entityPOCO.CarrierCard == null ? null : entityPOCO.CarrierCard.Website,
                    CarrierNumber = entityPOCO.CarrierNumber,
                    PickUpDeliveryTypeCode = entityPOCO.PickUpDeliveryTypeCode,
                    FullResponsibility = entityPOCO.FullResponsibility,
                    PickUpDeliveryFromTypeCode = entityPOCO.PickUpDeliveryFromTypeCode,
                    FromPortId = entityPOCO.FromPortId,
                    FromPortCode = entityPOCO.FromPort == null ? null : entityPOCO.FromPort.Code,
                    FromPortName = entityPOCO.FromPort == null ? null : entityPOCO.FromPort.EnglishName,
                    FromPortCountryCode = entityPOCO.FromPort == null ? null : (entityPOCO.FromPort.Country == null ? null : entityPOCO.FromPort.Country.Code),
                    FromPortCountryName = entityPOCO.FromPort == null ? null : (entityPOCO.FromPort.Country == null ? null : entityPOCO.FromPort.Country.EnglishName),
                    FromPartnerCardId = entityPOCO.FromPartnerCardId,
                    FromAddressId = entityPOCO.FromAddressId,
                    FromAddress = entityPOCO.FromAddress,
                    FromAddressCity = entityPOCO.FromAddressCity,
                    FromAddressZipCode = entityPOCO.FromAddressZipCode,
                    FromAddressCountryId = entityPOCO.FromAddressCountryId,
                    PickUpDeliveryToTypeCode = entityPOCO.PickUpDeliveryToTypeCode,
                    ToPortId = entityPOCO.ToPortId,
                    ToPortCode = entityPOCO.ToPort == null ? null : entityPOCO.ToPort.Code,
                    ToPortName = entityPOCO.ToPort == null ? null : entityPOCO.ToPort.EnglishName,
                    ToPortCountryCode = entityPOCO.ToPort == null ? null : (entityPOCO.ToPort.Country == null ? null : entityPOCO.ToPort.Country.Code),
                    ToPortCountryName = entityPOCO.ToPort == null ? null : (entityPOCO.ToPort.Country == null ? null : entityPOCO.ToPort.Country.EnglishName),
                    ToPartnerCardId = entityPOCO.ToPartnerCardId,
                    ToAddressId = entityPOCO.ToAddressId,
                    ToAddress = entityPOCO.ToAddress,
                    ToAddressCity = entityPOCO.ToAddressCity,
                    ToAddressZipCode = entityPOCO.ToAddressZipCode,
                    ToAddressCountryId = entityPOCO.ToAddressCountryId,
                    EmptyPickupContainerPartnerId = entityPOCO.EmptyPickupContainerPartnerId,
                    EmptyPickupDepotReference = entityPOCO.EmptyPickupDepotReference,
                    EmptyDeliveryContainerPartnerId = entityPOCO.EmptyDeliveryContainerPartnerId,
                    EmptyDeliveryDepotReference = entityPOCO.EmptyDeliveryDepotReference,
                    TransportModeCode = entityPOCO.TransportModeCode,
                    ParentPickUpDeliveryId = entityPOCO.ParentPickUpDeliveryId,
                    ChildDeliveryIndex = entityPOCO.ChildDeliveryIndex,
                };

                ShipmentPickUpDeliveryPackageQuery packagesQuery = new ShipmentPickUpDeliveryPackageQuery(tenant);
                entityPM.ShipmentPickUpDeliveryPackages = packagesQuery.GetShipmentPickUpDeliveryPackages(entityPOCO.Id, tenant);

                AddressRepository addressRepository = new AddressRepository(tenant);
                CountryRepository countryRepository = new CountryRepository(tenant);
                CardRepository cardRepository = new CardRepository(tenant);

                #region From PART
                if (entityPOCO.PickUpDeliveryFromTypeCode == "PART")
                {
                    if (!string.IsNullOrEmpty(entityPOCO.FromPartnerCardId))
                    {
                        Card card = cardRepository.GetSingleCard(entityPOCO.FromPartnerCardId, tenant);

                        Address address = null;

                        if (!string.IsNullOrEmpty(entityPOCO.FromAddressId))
                        {
                            address = addressRepository.GetSingleAddress(entityPOCO.FromAddressId, tenant);
                        }

                        else
                        {
                            address = addressRepository.GetMainAddressByCardId(entityPOCO.FromPartnerCardId, tenant);
                        }

                        if (address != null)
                        {
                            string location = "";

                            if (address.IsLocalLanguage && !string.IsNullOrEmpty(card.LocalName))
                            {
                                location = card.LocalName + Environment.NewLine;
                            }

                            else
                            {
                                location = card.EnglishName + Environment.NewLine;
                            }


                            location = location + General.GetAddress(address);

                            entityPM.FromLocation = location;
                            entityPM.FromAddressCity_Dummy = address.City;

                            if (!string.IsNullOrEmpty(address.CountryId))
                            {
                                Country country = countryRepository.GetSingleCountry(address.CountryId, tenant);
                                if (country != null)
                                {
                                    entityPM.FromAddressCountryCode = country.Code;
                                    entityPM.FromAddressCountryName = country.EnglishName;
                                }
                            }
                        }
                    }
                }
                #endregion

                #region From PORT
                else if (entityPOCO.PickUpDeliveryFromTypeCode == "PORT")
                {
                    if (!string.IsNullOrEmpty(entityPOCO.FromPortId))
                    {
                        entityPM.FromLocation = entityPOCO.FromAddress;
                    }
                }
                #endregion

                #region From CASL
                else
                {
                    string location = "";

                    if (!string.IsNullOrEmpty(entityPOCO.FromAddressCity))
                    {
                        location = entityPOCO.FromAddressCity;
                    }

                    if (!string.IsNullOrEmpty(entityPOCO.FromAddressZipCode))
                    {
                        location = location + " " + entityPOCO.FromAddressZipCode;
                    }

                    if (!string.IsNullOrEmpty(entityPOCO.FromAddressCountryId))
                    {
                        Country country = countryRepository.GetSingleCountry(entityPOCO.FromAddressCountryId, tenant);
                        if (country != null)
                        {
                            location = location + Environment.NewLine + country.EnglishName;

                            entityPM.FromAddressCountryCode = country.Code;
                            entityPM.FromAddressCountryName = country.EnglishName;
                        }
                    }

                    entityPM.FromLocation = location;
                    entityPM.FromAddressCity_Dummy = entityPOCO.FromAddressCity;
                }
                #endregion

                #region To PART
                if (entityPOCO.PickUpDeliveryToTypeCode == "PART")
                {
                    if (!string.IsNullOrEmpty(entityPOCO.ToPartnerCardId))
                    {
                        Card card = cardRepository.GetSingleCard(entityPOCO.ToPartnerCardId, tenant);

                        Address address = null;

                        if (!string.IsNullOrEmpty(entityPOCO.ToAddressId))
                        {
                            address = addressRepository.GetSingleAddress(entityPOCO.ToAddressId, tenant);
                        }

                        else
                        {
                            address = addressRepository.GetMainAddressByCardId(entityPOCO.ToPartnerCardId, tenant);
                        }

                        if (address != null)
                        {
                            string location = "";

                            if (address.IsLocalLanguage && !string.IsNullOrEmpty(card.LocalName))
                            {
                                location = card.LocalName + Environment.NewLine;
                            }

                            else
                            {
                                location = card.EnglishName + Environment.NewLine;
                            }


                            location = location + General.GetAddress(address);

                            entityPM.ToLocation = location;
                            entityPM.ToAddressCity_Dummy = address.City;

                            if (!string.IsNullOrEmpty(address.CountryId))
                            {
                                Country country = countryRepository.GetSingleCountry(address.CountryId, tenant);
                                if (country != null)
                                {
                                    entityPM.ToAddressCountryCode = country.Code;
                                    entityPM.ToAddressCountryName = country.EnglishName;
                                }
                            }
                        }
                    }
                }
                #endregion

                #region To PORT
                else if (entityPOCO.PickUpDeliveryToTypeCode == "PORT")
                {
                    if (!string.IsNullOrEmpty(entityPOCO.ToPortId))
                    {
                        entityPM.ToLocation = entityPOCO.ToAddress;
                    }
                }
                #endregion

                #region To CASL
                else
                {
                    string location = "";

                    if (!string.IsNullOrEmpty(entityPOCO.ToAddressCity))
                    {
                        location = entityPOCO.ToAddressCity;
                    }

                    if (!string.IsNullOrEmpty(entityPOCO.ToAddressZipCode))
                    {
                        location = location + " " + entityPOCO.ToAddressZipCode;
                    }

                    if (!string.IsNullOrEmpty(entityPOCO.ToAddressCountryId))
                    {
                        Country country = countryRepository.GetSingleCountry(entityPOCO.ToAddressCountryId, tenant);
                        if (country != null)
                        {
                            location = location + Environment.NewLine + country.EnglishName;

                            entityPM.ToAddressCountryCode = country.Code;
                            entityPM.ToAddressCountryName = country.EnglishName;
                        }
                    }

                    entityPM.ToLocation = location;
                    entityPM.ToAddressCity_Dummy = entityPOCO.ToAddressCity;
                }
                #endregion
            }

            return entityPM;
        }

        public List<ShipmentDeliveryPM> GetShipmentDeliveryPMsByTenantAndShipment(string shipmentId, int tenant, bool getEmptyContainerReturn=false)
        {
            IQueryable<ShipmentPickUpDelivery> iQueryable = (from d in repository.context.ShipmentPickUpDeliveries.Include("FromPort").Include("ToPort").Include("CarrierCard").Include("TransportMode")
                                                             where d.ShipmentId == shipmentId && d.Tenant == tenant
                                                             select d);

            if (getEmptyContainerReturn)
            {
                iQueryable = iQueryable.Where(d => d.PickUpDeliveryTypeCode == "DELV" || d.PickUpDeliveryTypeCode == "EMPT");
            }

            else
            {
                iQueryable = iQueryable.Where(d => d.PickUpDeliveryTypeCode == "DELV");
            }

            List<ShipmentDeliveryPM> dataList = (from entityPOCO in iQueryable
                                                 select new ShipmentDeliveryPM()
                                                 {
                                                     Id = entityPOCO.Id,
                                                     Tenant = entityPOCO.Tenant,
                                                     ShipmentId = entityPOCO.ShipmentId,
                                                     PickUpDeliveryNumber = entityPOCO.PickUpDeliveryNumber,
                                                     ATA = entityPOCO.ATA,
                                                     ATD = entityPOCO.ATD,
                                                     ETA = entityPOCO.ETA,
                                                     ETD = entityPOCO.ETD,
                                                     Driver = entityPOCO.Driver,
                                                     TruckNumber = entityPOCO.TruckNumber,
                                                     Notes = entityPOCO.Notes,
                                                     TrailerNumber = entityPOCO.TrailerNumber,
                                                     CarrierId = entityPOCO.CarrierId,
                                                     CarrierCode = entityPOCO.CarrierCard == null ? null : entityPOCO.CarrierCard.Code,
                                                     CarrierName = entityPOCO.CarrierCard == null ? null : entityPOCO.CarrierCard.EnglishName,
                                                     CarrierWebSite = entityPOCO.CarrierCard == null ? null : entityPOCO.CarrierCard.Website,
                                                     CarrierNumber = entityPOCO.CarrierNumber,                                                     
                                                     PickUpDeliveryTypeCode = entityPOCO.PickUpDeliveryTypeCode,
                                                     FullResponsibility = entityPOCO.FullResponsibility,
                                                     PickUpDeliveryFromTypeCode = entityPOCO.PickUpDeliveryFromTypeCode,
                                                     FromPortId = entityPOCO.FromPortId,
                                                     FromPortCode = entityPOCO.FromPort == null ? null : entityPOCO.FromPort.Code,
                                                     FromPortName = entityPOCO.FromPort == null ? null : entityPOCO.FromPort.EnglishName,
                                                     FromPortCountryCode = entityPOCO.FromPort == null ? null : (entityPOCO.FromPort.Country == null ? null : entityPOCO.FromPort.Country.Code),
                                                     FromPortCountryName = entityPOCO.FromPort == null ? null : (entityPOCO.FromPort.Country == null ? null : entityPOCO.FromPort.Country.EnglishName),
                                                     FromPartnerCardId = entityPOCO.FromPartnerCardId,
                                                     FromAddressId = entityPOCO.FromAddressId,
                                                     FromAddress = entityPOCO.FromAddress,
                                                     FromAddressCity = entityPOCO.FromAddressCity,
                                                     FromAddressZipCode = entityPOCO.FromAddressZipCode,
                                                     FromAddressCountryId = entityPOCO.FromAddressCountryId,
                                                     PickUpDeliveryToTypeCode = entityPOCO.PickUpDeliveryToTypeCode,
                                                     ToPortId = entityPOCO.ToPortId,
                                                     ToPortCode = entityPOCO.ToPort == null ? null : entityPOCO.ToPort.Code,
                                                     ToPortName = entityPOCO.ToPort == null ? null : entityPOCO.ToPort.EnglishName,
                                                     ToPortCountryCode = entityPOCO.ToPort == null ? null : (entityPOCO.ToPort.Country == null ? null : entityPOCO.ToPort.Country.Code),
                                                     ToPortCountryName = entityPOCO.ToPort == null ? null : (entityPOCO.ToPort.Country == null ? null : entityPOCO.ToPort.Country.EnglishName),
                                                     ToPartnerCardId = entityPOCO.ToPartnerCardId,
                                                     ToAddressId = entityPOCO.ToAddressId,
                                                     ToAddress = entityPOCO.ToAddress,
                                                     ToAddressCity = entityPOCO.ToAddressCity,
                                                     ToAddressZipCode = entityPOCO.ToAddressZipCode,
                                                     ToAddressCountryId = entityPOCO.ToAddressCountryId,
                                                     EmptyPickupContainerPartnerId = entityPOCO.EmptyPickupContainerPartnerId,
                                                     EmptyPickupDepotReference = entityPOCO.EmptyPickupDepotReference,
                                                     EmptyDeliveryContainerPartnerId = entityPOCO.EmptyDeliveryContainerPartnerId,
                                                     EmptyDeliveryDepotReference = entityPOCO.EmptyDeliveryDepotReference,
                                                     TransportModeCode = entityPOCO.TransportModeCode,
                                                     TransportModeName = entityPOCO.TransportMode == null ? null : entityPOCO.TransportMode.Name,
                                                     ParentPickUpDeliveryId = entityPOCO.ParentPickUpDeliveryId,
                                                     ChildDeliveryIndex = entityPOCO.ChildDeliveryIndex,
                                                 }).ToList();

            if (dataList.Count > 0)
            {
                ShipmentPickUpDeliveryPackageRepository packageRepository = new ShipmentPickUpDeliveryPackageRepository(repository.context);
                ShipmentPickUpDeliveryPackageQuery packagesQuery = new ShipmentPickUpDeliveryPackageQuery(packageRepository);

                AddressRepository addressRepository = new AddressRepository(tenant);
                CountryRepository countryRepository = new CountryRepository(tenant);
                CardRepository cardRepository = new CardRepository(tenant);

                foreach (ShipmentDeliveryPM item in dataList)
                {
                    item.ShipmentPickUpDeliveryPackages = packagesQuery.GetShipmentPickUpDeliveryPackages(item.Id, tenant);

                    #region From PART
                    if (item.PickUpDeliveryFromTypeCode == "PART")
                    {
                        if (!string.IsNullOrEmpty(item.FromPartnerCardId))
                        {
                            Card card = cardRepository.GetSingleCard(item.FromPartnerCardId, tenant);

                            Address address = null;

                            if (!string.IsNullOrEmpty(item.FromAddressId))
                            {
                                address = addressRepository.GetSingleAddress(item.FromAddressId, tenant);
                            }

                            else
                            {
                                address = addressRepository.GetMainAddressByCardId(item.FromPartnerCardId, tenant);
                            }

                            if (address != null)
                            {
                                string location = "";

                                if (address.IsLocalLanguage && !string.IsNullOrEmpty(card.LocalName))
                                {
                                    location = card.LocalName + Environment.NewLine;
                                }

                                else
                                {
                                    location = card.EnglishName + Environment.NewLine;
                                }


                                location = location + General.GetAddress(address);

                                item.FromLocation = location;
                                item.FromAddressCity_Dummy = address.City;

                                if (!string.IsNullOrEmpty(address.CountryId))
                                {
                                    Country country = countryRepository.GetSingleCountry(address.CountryId, tenant);
                                    if (country != null)
                                    {
                                        item.FromAddressCountryCode = country.Code;
                                        item.FromAddressCountryName = country.EnglishName;
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region From PORT
                    else if (item.PickUpDeliveryFromTypeCode == "PORT")
                    {
                        if (!string.IsNullOrEmpty(item.FromPortId))
                        {
                            item.FromLocation = item.FromAddress;
                        }
                    }
                    #endregion

                    #region From CASL
                    else
                    {
                        string location = "";

                        if (!string.IsNullOrEmpty(item.FromAddressCity))
                        {
                            location = item.FromAddressCity;
                        }

                        if (!string.IsNullOrEmpty(item.FromAddressZipCode))
                        {
                            location = location + " " + item.FromAddressZipCode;
                        }

                        if (!string.IsNullOrEmpty(item.FromAddressCountryId))
                        {
                            Country country = countryRepository.GetSingleCountry(item.FromAddressCountryId, tenant);
                            if (country != null)
                            {
                                location = location + Environment.NewLine + country.EnglishName;

                                item.FromAddressCountryCode = country.Code;
                                item.FromAddressCountryName = country.EnglishName;
                            }
                        }

                        item.FromLocation = location;
                        item.FromAddressCity_Dummy = item.FromAddressCity;
                    }
                    #endregion

                    #region To PART
                    if (item.PickUpDeliveryToTypeCode == "PART")
                    {
                        if (!string.IsNullOrEmpty(item.ToPartnerCardId))
                        {
                            Card card = cardRepository.GetSingleCard(item.ToPartnerCardId, tenant);

                            Address address = null;

                            if (!string.IsNullOrEmpty(item.ToAddressId))
                            {
                                address = addressRepository.GetSingleAddress(item.ToAddressId, tenant);
                            }

                            else
                            {
                                address = addressRepository.GetMainAddressByCardId(item.ToPartnerCardId, tenant);
                            }

                            if (address != null)
                            {
                                string location = "";

                                if (address.IsLocalLanguage && !string.IsNullOrEmpty(card.LocalName))
                                {
                                    location = card.LocalName + Environment.NewLine;
                                }

                                else
                                {
                                    location = card.EnglishName + Environment.NewLine;
                                }


                                location = location + General.GetAddress(address);

                                item.ToLocation = location;
                                item.ToAddressCity_Dummy = address.City;

                                if (!string.IsNullOrEmpty(address.CountryId))
                                {
                                    Country country = countryRepository.GetSingleCountry(address.CountryId, tenant);
                                    if (country != null)
                                    {
                                        item.ToAddressCountryCode = country.Code;
                                        item.ToAddressCountryName = country.EnglishName;
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region To PORT
                    else if (item.PickUpDeliveryToTypeCode == "PORT")
                    {
                        if (!string.IsNullOrEmpty(item.ToPortId))
                        {
                            item.ToLocation = item.ToAddress;
                        }
                    }
                    #endregion

                    #region To CASL
                    else
                    {
                        string location = "";

                        if (!string.IsNullOrEmpty(item.ToAddressCity))
                        {
                            location = item.ToAddressCity;
                        }

                        if (!string.IsNullOrEmpty(item.ToAddressZipCode))
                        {
                            location = location + " " + item.ToAddressZipCode;
                        }

                        if (!string.IsNullOrEmpty(item.ToAddressCountryId))
                        {
                            Country country = countryRepository.GetSingleCountry(item.ToAddressCountryId, tenant);
                            if (country != null)
                            {
                                location = location + Environment.NewLine + country.EnglishName;

                                item.ToAddressCountryCode = country.Code;
                                item.ToAddressCountryName = country.EnglishName;
                            }
                        }

                        item.ToLocation = location;
                        item.ToAddressCity_Dummy = item.ToAddressCity;
                    }
                    #endregion
                }
            }

            return dataList;
        }

        public List<ShipmentDeliveryPM> GetShipmentDeliveryByTenant(int tenant)
        {
            List<ShipmentDeliveryPM> dataList = (from entity in repository.context.ShipmentPickUpDeliveries.Include("FromPort").Include("ToPort").Include("CarrierCard").Include("Shipment").Include("Shipment.ShipmentMasterData").Include("Shipment.AgentCard").Include("Shipment.ShipmentMasterData.MainCarriageCarrierCard")
                                                       where entity.Tenant == tenant && entity.PickUpDeliveryTypeCode == "DELV" && entity.Shipment.TransportModeId == "O"
                                                       select new ShipmentDeliveryPM()
                                                       {
                                                           Id = entity.Id,
                                                           Tenant = entity.Tenant,
                                                           ShipmentId = entity.ShipmentId,
                                                           PickUpDeliveryNumber = entity.PickUpDeliveryNumber,
                                                           ATA = entity.ATA,
                                                           ATD = entity.ATD,
                                                           ETA = entity.ETA,
                                                           ETD = entity.ETD,
                                                           Driver = entity.Driver,
                                                           TruckNumber = entity.TruckNumber,
                                                           Notes = entity.Notes,
                                                           TrailerNumber = entity.TrailerNumber,
                                                           CarrierId = entity.CarrierId,
                                                           CarrierCode = entity.CarrierCard == null ? null : entity.CarrierCard.Code,
                                                           CarrierName = entity.CarrierCard == null ? null : entity.CarrierCard.EnglishName,
                                                           CarrierWebSite = entity.CarrierCard == null ? null : entity.CarrierCard.Website,
                                                           CarrierNumber = entity.CarrierNumber,
                                                           PickUpDeliveryTypeCode = entity.PickUpDeliveryTypeCode,
                                                           FullResponsibility = entity.FullResponsibility,
                                                           PickUpDeliveryFromTypeCode = entity.PickUpDeliveryFromTypeCode,
                                                           FromPortId = entity.FromPortId,
                                                           FromPortCode = entity.FromPort == null ? null : entity.FromPort.Code,
                                                           FromPortName = entity.FromPort == null ? null : entity.FromPort.EnglishName,
                                                           FromPortCountryCode = entity.FromPort == null ? null : (entity.FromPort.Country == null ? null : entity.FromPort.Country.Code),
                                                           FromPortCountryName = entity.FromPort == null ? null : (entity.FromPort.Country == null ? null : entity.FromPort.Country.EnglishName),
                                                           FromPartnerCardId = entity.FromPartnerCardId,
                                                           FromAddressId = entity.FromAddressId,
                                                           FromAddress = entity.FromAddress,
                                                           FromAddressCity = entity.FromAddressCity,
                                                           FromAddressZipCode = entity.FromAddressZipCode,
                                                           FromAddressCountryId = entity.FromAddressCountryId,
                                                           PickUpDeliveryToTypeCode = entity.PickUpDeliveryToTypeCode,
                                                           ToPortId = entity.ToPortId,
                                                           ToPortCode = entity.ToPort == null ? null : entity.ToPort.Code,
                                                           ToPortName = entity.ToPort == null ? null : entity.ToPort.EnglishName,
                                                           ToPortCountryCode = entity.ToPort == null ? null : (entity.ToPort.Country == null ? null : entity.ToPort.Country.Code),
                                                           ToPortCountryName = entity.ToPort == null ? null : (entity.ToPort.Country == null ? null : entity.ToPort.Country.EnglishName),
                                                           ToPartnerCardId = entity.ToPartnerCardId,
                                                           ToAddressId = entity.ToAddressId,
                                                           ToAddress = entity.ToAddress,
                                                           ToAddressCity = entity.ToAddressCity,
                                                           ToAddressZipCode = entity.ToAddressZipCode,
                                                           ToAddressCountryId = entity.ToAddressCountryId,
                                                           DirectionId = entity.Shipment.DirectionId,
                                                           CustomerId = entity.Shipment.CustomerId,
                                                           MasterNumber = entity.Shipment.ShipmentMasterData != null ? entity.Shipment.ShipmentMasterData.Master : null,
                                                           AgentName = entity.Shipment.AgentCard != null ? entity.Shipment.AgentCard.EnglishName : null,
                                                           ShipmentNumber = entity.Shipment.ShipmentNumber,
                                                           ShippingLine = entity.Shipment.ShipmentMasterData != null ? (entity.Shipment.ShipmentMasterData.MainCarriageCarrierCard != null ? entity.Shipment.ShipmentMasterData.MainCarriageCarrierCard.EnglishName : null) : null,
                                                           AgentId = entity.Shipment.AgentId,
                                                           IsCancelled = entity.Shipment.IsCancelled,
                                                           EmptyPickupContainerPartnerId = entity.EmptyPickupContainerPartnerId,
                                                           EmptyPickupDepotReference = entity.EmptyPickupDepotReference,
                                                           EmptyDeliveryContainerPartnerId = entity.EmptyDeliveryContainerPartnerId,
                                                           EmptyDeliveryDepotReference = entity.EmptyDeliveryDepotReference,
                                                           TransportModeCode = entity.TransportModeCode,
                                                           ParentPickUpDeliveryId = entity.ParentPickUpDeliveryId,
                                                           ChildDeliveryIndex = entity.ChildDeliveryIndex,
                                                       }).ToList();            

            if (dataList.Count() > 0)
            {
                ShipmentPickUpDeliveryPackageRepository packageRepository = new ShipmentPickUpDeliveryPackageRepository(repository.context);
                ShipmentPickUpDeliveryPackageQuery packagesQuery = new ShipmentPickUpDeliveryPackageQuery(packageRepository);
                
                AddressRepository addressRepository = new AddressRepository(tenant);
                CountryRepository countryRepository = new CountryRepository(tenant);
                CardRepository cardRepository = new CardRepository(tenant);

                foreach (ShipmentDeliveryPM item in dataList)
                {
                    item.ShipmentPickUpDeliveryPackages = packagesQuery.GetShipmentPickUpDeliveryPackages(item.Id, tenant);

                    #region From PART
                    if (item.PickUpDeliveryFromTypeCode == "PART")
                    {
                        if (!string.IsNullOrEmpty(item.FromPartnerCardId))
                        {
                            Card card = cardRepository.GetSingleCard(item.FromPartnerCardId, tenant);

                            Address address = null;

                            if (!string.IsNullOrEmpty(item.FromAddressId))
                            {
                                address = addressRepository.GetSingleAddress(item.FromAddressId, tenant);
                            }
                            else
                            {
                                address = addressRepository.GetMainAddressByCardId(item.FromPartnerCardId, tenant);
                            }

                            if (address != null)
                            {
                                string location = "";

                                if (address.IsLocalLanguage && !string.IsNullOrEmpty(card.LocalName))
                                {
                                    location = card.LocalName + Environment.NewLine;
                                }
                                else
                                {
                                    location = card.EnglishName + Environment.NewLine;
                                }


                                location = location + General.GetAddress(address);

                                item.FromLocation = location;
                                item.FromAddressCity_Dummy = address.City;

                                if (!string.IsNullOrEmpty(address.CountryId))
                                {
                                    Country country = countryRepository.GetSingleCountry(address.CountryId, tenant);
                                    if (country != null)
                                    {
                                        item.FromAddressCountryCode = country.Code;
                                        item.FromAddressCountryName = country.EnglishName;
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region From PORT
                    else if (item.PickUpDeliveryFromTypeCode == "PORT")
                    {
                        if (!string.IsNullOrEmpty(item.FromPortId))
                        {
                            item.FromLocation = item.FromAddress;
                        }
                    }
                    #endregion

                    #region From CASL
                    else
                    {
                        string location = "";

                        if (!string.IsNullOrEmpty(item.FromAddressCity))
                        {
                            location = item.FromAddressCity;
                        }

                        if (!string.IsNullOrEmpty(item.FromAddressZipCode))
                        {
                            location = location + " " + item.FromAddressZipCode;
                        }

                        if (!string.IsNullOrEmpty(item.FromAddressCountryId))
                        {
                            Country country = countryRepository.GetSingleCountry(item.FromAddressCountryId, tenant);
                            if (country != null)
                            {
                                location = location + Environment.NewLine + country.EnglishName;

                                item.FromAddressCountryCode = country.Code;
                                item.FromAddressCountryName = country.EnglishName;
                            }
                        }

                        item.FromLocation = location;
                        item.FromAddressCity_Dummy = item.FromAddressCity;
                    }
                    #endregion

                    #region To PART
                    if (item.PickUpDeliveryToTypeCode == "PART")
                    {
                        if (!string.IsNullOrEmpty(item.ToPartnerCardId))
                        {
                            Card card = cardRepository.GetSingleCard(item.ToPartnerCardId, tenant);

                            Address address = null;

                            if (!string.IsNullOrEmpty(item.ToAddressId))
                            {
                                address = addressRepository.GetSingleAddress(item.ToAddressId, tenant);
                            }
                            else
                            {
                                address = addressRepository.GetMainAddressByCardId(item.ToPartnerCardId, tenant);
                            }

                            if (address != null)
                            {
                                string location = "";

                                if (address.IsLocalLanguage && !string.IsNullOrEmpty(card.LocalName))
                                {
                                    location = card.LocalName + Environment.NewLine;
                                }
                                else
                                {
                                    location = card.EnglishName + Environment.NewLine;
                                }

                                location = location + General.GetAddress(address);

                                item.ToLocation = location;
                                item.ToAddressCity_Dummy = address.City;

                                if (!string.IsNullOrEmpty(address.CountryId))
                                {
                                    Country country = countryRepository.GetSingleCountry(address.CountryId, tenant);
                                    if (country != null)
                                    {
                                        item.ToAddressCountryCode = country.Code;
                                        item.ToAddressCountryName = country.EnglishName;
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region To PORT
                    else if (item.PickUpDeliveryToTypeCode == "PORT")
                    {
                        if (!string.IsNullOrEmpty(item.ToPortId))
                        {
                            item.ToLocation = item.ToAddress;
                        }
                    }
                    #endregion

                    #region To CASL
                    else
                    {
                        string location = "";

                        if (!string.IsNullOrEmpty(item.ToAddressCity))
                        {
                            location = item.ToAddressCity;
                        }

                        if (!string.IsNullOrEmpty(item.ToAddressZipCode))
                        {
                            location = location + " " + item.ToAddressZipCode;
                        }

                        if (!string.IsNullOrEmpty(item.ToAddressCountryId))
                        {
                            Country country = countryRepository.GetSingleCountry(item.ToAddressCountryId, tenant);
                            if (country != null)
                            {
                                location = location + Environment.NewLine + country.EnglishName;

                                item.ToAddressCountryCode = country.Code;
                                item.ToAddressCountryName = country.EnglishName;
                            }
                        }

                        item.ToLocation = location;
                        item.ToAddressCity_Dummy = item.ToAddressCity;
                    }
                    #endregion
                }
            }

            return dataList;
        }

        public ShipmentDeliveryPM GetFirstShipmentDeliveryPMsByTenantAndShipment(string shipmentId,string shipmentNumber ,  int tenant)
        {
            IQueryable<ShipmentPickUpDelivery> iQueryable = (from d in repository.context.ShipmentPickUpDeliveries.Include("FromPort").Include("ToPort").Include("CarrierCard").Include("TransportMode")
                                                             where d.ShipmentId == shipmentId && d.Tenant == tenant && d.PickUpDeliveryTypeCode == "DELV"
                                                             select d).OrderBy(d => d.PickUpDeliveryNumber);

        ShipmentDeliveryPM shipmentDeliveryPM = (from entityPOCO in iQueryable
                                                     select new ShipmentDeliveryPM()
                                                     {
                                                         Id = entityPOCO.Id,
                                                         Tenant = entityPOCO.Tenant,
                                                         ShipmentId = entityPOCO.ShipmentId,
                                                         PickUpDeliveryNumber = entityPOCO.PickUpDeliveryNumber,
                                                         ATA = entityPOCO.ATA,
                                                         ATD = entityPOCO.ATD,
                                                         ETA = entityPOCO.ETA,
                                                         ETD = entityPOCO.ETD,
                                                         Driver = entityPOCO.Driver,
                                                         TruckNumber = entityPOCO.TruckNumber,
                                                         Notes = entityPOCO.Notes,
                                                         TrailerNumber = entityPOCO.TrailerNumber,
                                                         CarrierId = entityPOCO.CarrierId,
                                                         CarrierCode = entityPOCO.CarrierCard == null ? null : entityPOCO.CarrierCard.Code,
                                                         CarrierName = entityPOCO.CarrierCard == null ? null : entityPOCO.CarrierCard.EnglishName,
                                                         CarrierWebSite = entityPOCO.CarrierCard == null ? null : entityPOCO.CarrierCard.Website,
                                                         CarrierNumber = entityPOCO.CarrierNumber,
                                                         PickUpDeliveryTypeCode = entityPOCO.PickUpDeliveryTypeCode,
                                                         FullResponsibility = entityPOCO.FullResponsibility,
                                                         PickUpDeliveryFromTypeCode = entityPOCO.PickUpDeliveryFromTypeCode,
                                                         FromPortId = entityPOCO.FromPortId,
                                                         FromPortCode = entityPOCO.FromPort == null ? null : entityPOCO.FromPort.Code,
                                                         FromPortName = entityPOCO.FromPort == null ? null : entityPOCO.FromPort.EnglishName,
                                                         FromPortCountryCode = entityPOCO.FromPort == null ? null : (entityPOCO.FromPort.Country == null ? null : entityPOCO.FromPort.Country.Code),
                                                         FromPortCountryName = entityPOCO.FromPort == null ? null : (entityPOCO.FromPort.Country == null ? null : entityPOCO.FromPort.Country.EnglishName),
                                                         FromPartnerCardId = entityPOCO.FromPartnerCardId,
                                                         FromAddressId = entityPOCO.FromAddressId,
                                                         FromAddress = entityPOCO.FromAddress,
                                                         FromAddressCity = entityPOCO.FromAddressCity,
                                                         FromAddressZipCode = entityPOCO.FromAddressZipCode,
                                                         FromAddressCountryId = entityPOCO.FromAddressCountryId,
                                                         PickUpDeliveryToTypeCode = entityPOCO.PickUpDeliveryToTypeCode,
                                                         ToPortId = entityPOCO.ToPortId,
                                                         ToPortCode = entityPOCO.ToPort == null ? null : entityPOCO.ToPort.Code,
                                                         ToPortName = entityPOCO.ToPort == null ? null : entityPOCO.ToPort.EnglishName,
                                                         ToPortCountryCode = entityPOCO.ToPort == null ? null : (entityPOCO.ToPort.Country == null ? null : entityPOCO.ToPort.Country.Code),
                                                         ToPortCountryName = entityPOCO.ToPort == null ? null : (entityPOCO.ToPort.Country == null ? null : entityPOCO.ToPort.Country.EnglishName),
                                                         ToPartnerCardId = entityPOCO.ToPartnerCardId,
                                                         ToAddressId = entityPOCO.ToAddressId,
                                                         ToAddress = entityPOCO.ToAddress,
                                                         ToAddressCity = entityPOCO.ToAddressCity,
                                                         ToAddressZipCode = entityPOCO.ToAddressZipCode,
                                                         ToAddressCountryId = entityPOCO.ToAddressCountryId,
                                                         EmptyPickupContainerPartnerId = entityPOCO.EmptyPickupContainerPartnerId,
                                                         EmptyPickupDepotReference = entityPOCO.EmptyPickupDepotReference,
                                                         EmptyDeliveryContainerPartnerId = entityPOCO.EmptyDeliveryContainerPartnerId,
                                                         EmptyDeliveryDepotReference = entityPOCO.EmptyDeliveryDepotReference,
                                                         TransportModeCode = entityPOCO.TransportModeCode,
                                                         TransportModeName = entityPOCO.TransportMode == null ? null : entityPOCO.TransportMode.Name,
                                                         ParentPickUpDeliveryId = entityPOCO.ParentPickUpDeliveryId,
                                                         ChildDeliveryIndex = entityPOCO.ChildDeliveryIndex,
                                                     }).FirstOrDefault();

            if (shipmentDeliveryPM!=null)
            {
                ShipmentPickUpDeliveryPackageRepository packageRepository = new ShipmentPickUpDeliveryPackageRepository(repository.context);
                ShipmentPickUpDeliveryPackageQuery packagesQuery = new ShipmentPickUpDeliveryPackageQuery(packageRepository);

                AddressRepository addressRepository = new AddressRepository(tenant);
                CountryRepository countryRepository = new CountryRepository(tenant);
                CardRepository cardRepository = new CardRepository(tenant);

                shipmentDeliveryPM.ShipmentPickUpDeliveryPackages = packagesQuery.GetShipmentPickUpDeliveryPackages(shipmentDeliveryPM.Id, tenant);

                #region From PART
                if (shipmentDeliveryPM.PickUpDeliveryFromTypeCode == "PART")
                {
                    if (!string.IsNullOrEmpty(shipmentDeliveryPM.FromPartnerCardId))
                    {
                        Card card = cardRepository.GetSingleCard(shipmentDeliveryPM.FromPartnerCardId, tenant);

                        Address address = null;

                        if (!string.IsNullOrEmpty(shipmentDeliveryPM.FromAddressId))
                        {
                            address = addressRepository.GetSingleAddress(shipmentDeliveryPM.FromAddressId, tenant);
                        }

                        else
                        {
                            address = addressRepository.GetMainAddressByCardId(shipmentDeliveryPM.FromPartnerCardId, tenant);
                        }

                        if (address != null)
                        {
                            string location = "";

                            if (address.IsLocalLanguage && !string.IsNullOrEmpty(card.LocalName))
                            {
                                location = card.LocalName + Environment.NewLine;
                            }

                            else
                            {
                                location = card.EnglishName + Environment.NewLine;
                            }


                            location = location + General.GetAddress(address);

                            shipmentDeliveryPM.FromLocation = location;
                            shipmentDeliveryPM.FromAddressCity_Dummy = address.City;

                            if (!string.IsNullOrEmpty(address.CountryId))
                            {
                                Country country = countryRepository.GetSingleCountry(address.CountryId, tenant);
                                if (country != null)
                                {
                                    shipmentDeliveryPM.FromAddressCountryCode = country.Code;
                                    shipmentDeliveryPM.FromAddressCountryName = country.EnglishName;
                                }
                            }
                        }
                    }
                }
                #endregion

                #region From PORT
                else if (shipmentDeliveryPM.PickUpDeliveryFromTypeCode == "PORT")
                {
                    if (!string.IsNullOrEmpty(shipmentDeliveryPM.FromPortId))
                    {
                        shipmentDeliveryPM.FromLocation = shipmentDeliveryPM.FromAddress;
                    }
                }
                #endregion

                #region From CASL
                else
                {
                    string location = "";

                    if (!string.IsNullOrEmpty(shipmentDeliveryPM.FromAddressCity))
                    {
                        location = shipmentDeliveryPM.FromAddressCity;
                    }

                    if (!string.IsNullOrEmpty(shipmentDeliveryPM.FromAddressZipCode))
                    {
                        location = location + " " + shipmentDeliveryPM.FromAddressZipCode;
                    }

                    if (!string.IsNullOrEmpty(shipmentDeliveryPM.FromAddressCountryId))
                    {
                        Country country = countryRepository.GetSingleCountry(shipmentDeliveryPM.FromAddressCountryId, tenant);
                        if (country != null)
                        {
                            location = location + Environment.NewLine + country.EnglishName;

                            shipmentDeliveryPM.FromAddressCountryCode = country.Code;
                            shipmentDeliveryPM.FromAddressCountryName = country.EnglishName;
                        }
                    }

                    shipmentDeliveryPM.FromLocation = location;
                    shipmentDeliveryPM.FromAddressCity_Dummy = shipmentDeliveryPM.FromAddressCity;
                }
                #endregion

                #region To PART
                if (shipmentDeliveryPM.PickUpDeliveryToTypeCode == "PART")
                {
                    if (!string.IsNullOrEmpty(shipmentDeliveryPM.ToPartnerCardId))
                    {
                        Card card = cardRepository.GetSingleCard(shipmentDeliveryPM.ToPartnerCardId, tenant);

                        Address address = null;

                        if (!string.IsNullOrEmpty(shipmentDeliveryPM.ToAddressId))
                        {
                            address = addressRepository.GetSingleAddress(shipmentDeliveryPM.ToAddressId, tenant);
                        }

                        else
                        {
                            address = addressRepository.GetMainAddressByCardId(shipmentDeliveryPM.ToPartnerCardId, tenant);
                        }

                        if (address != null)
                        {
                            string location = "";

                            if (address.IsLocalLanguage && !string.IsNullOrEmpty(card.LocalName))
                            {
                                location = card.LocalName + Environment.NewLine;
                            }

                            else
                            {
                                location = card.EnglishName + Environment.NewLine;
                            }


                            location = location + General.GetAddress(address);

                            shipmentDeliveryPM.ToLocation = location;
                            shipmentDeliveryPM.ToAddressCity_Dummy = address.City;

                            if (!string.IsNullOrEmpty(address.CountryId))
                            {
                                Country country = countryRepository.GetSingleCountry(address.CountryId, tenant);
                                if (country != null)
                                {
                                    shipmentDeliveryPM.ToAddressCountryCode = country.Code;
                                    shipmentDeliveryPM.ToAddressCountryName = country.EnglishName;
                                }
                            }
                        }
                    }
                }
                #endregion

                #region To PORT
                else if (shipmentDeliveryPM.PickUpDeliveryToTypeCode == "PORT")
                {
                    if (!string.IsNullOrEmpty(shipmentDeliveryPM.ToPortId))
                    {
                        shipmentDeliveryPM.ToLocation = shipmentDeliveryPM.ToAddress;
                    }
                }
                #endregion

                #region To CASL
                else
                {
                    string location = "";

                    if (!string.IsNullOrEmpty(shipmentDeliveryPM.ToAddressCity))
                    {
                        location = shipmentDeliveryPM.ToAddressCity;
                    }

                    if (!string.IsNullOrEmpty(shipmentDeliveryPM.ToAddressZipCode))
                    {
                        location = location + " " + shipmentDeliveryPM.ToAddressZipCode;
                    }

                    if (!string.IsNullOrEmpty(shipmentDeliveryPM.ToAddressCountryId))
                    {
                        Country country = countryRepository.GetSingleCountry(shipmentDeliveryPM.ToAddressCountryId, tenant);
                        if (country != null)
                        {
                            location = location + Environment.NewLine + country.EnglishName;

                            shipmentDeliveryPM.ToAddressCountryCode = country.Code;
                            shipmentDeliveryPM.ToAddressCountryName = country.EnglishName;
                        }
                    }

                    shipmentDeliveryPM.ToLocation = location;
                    shipmentDeliveryPM.ToAddressCity_Dummy = shipmentDeliveryPM.ToAddressCity;
                }
                #endregion
            }

            return shipmentDeliveryPM;
        }
    }
}
