using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Server.Tools.Helpers;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ShipmentPickUpQuery
    {
        ShipmentPickUpDeliveryRepository repository;         
        public ShipmentPickUpQuery(int tenant)
        {
            repository = new ShipmentPickUpDeliveryRepository(tenant);
        }
        public ShipmentPickUpQuery(ShipmentPickUpDeliveryRepository repository)
        {
            this.repository = repository;
        }

        public ShipmentPickUpPM GetSinglePM(string id, int tenant)
        {           
            ShipmentPickUpDelivery entityPOCO = repository.GetSingleShipmentPickUpDelivery(tenant, id);
            ShipmentPickUpPM entityPM = null;

            if (entityPOCO != null)
            {
                entityPM = new ShipmentPickUpPM()
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
                    ChildPickUpIndex = entityPOCO.ChildPickUpIndex,
                    StandaloneShipmentId = entityPOCO.StandaloneShipmentId,
                    StandaloneShipmentNumber = entityPOCO.StandaloneShipmentNumber,
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
                            string location ="";

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
        public List<ShipmentPickUpPM> GetShipmentPickUpPMsByTenantAndShipment(string shipmentId, int tenant)
        {
            List<ShipmentPickUpPM> dataList = (from entityPOCO in repository.context.ShipmentPickUpDeliveries.Include("FromPort").Include("ToPort").Include("CarrierCard").Include("TransportMode")
                                               where entityPOCO.ShipmentId == shipmentId && entityPOCO.Tenant == tenant && entityPOCO.PickUpDeliveryTypeCode == "PICK"
                                               select new ShipmentPickUpPM()
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
                                                   ChildPickUpIndex = entityPOCO.ChildPickUpIndex,
                                                   StandaloneShipmentId = entityPOCO.StandaloneShipmentId,
                                                   StandaloneShipmentNumber = entityPOCO.StandaloneShipmentNumber,
                                               }).ToList();

            if (dataList.Count > 0)
            {
                ShipmentPickUpDeliveryPackageRepository packageRepository = new ShipmentPickUpDeliveryPackageRepository(repository.context);
                ShipmentPickUpDeliveryPackageQuery packagesQuery = new ShipmentPickUpDeliveryPackageQuery(packageRepository);

                AddressRepository addressRepository = new AddressRepository(tenant);
                CountryRepository countryRepository = new CountryRepository(tenant);
                CardRepository cardRepository = new CardRepository(tenant);

                foreach (ShipmentPickUpPM item in dataList)
                {
                    item.ShipmentPickUpDeliveryPackages = packagesQuery.GetShipmentPickUpDeliveryPackages(item.Id, tenant);
                    this.GetPickUpDeliveryIndexes(item);                    

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

        private void GetPickUpDeliveryIndexes(ShipmentPickUpPM item)
        {
            string[] numberArray = item.PickUpDeliveryNumber.Split('/');

            item.PickUpDeliveryIndex = Convert.ToInt32(numberArray[1]);
            if(numberArray.Length > 2)
            {
                item.ChildIndex = Convert.ToInt32(numberArray[2]);
            }
        }

        public ShipmentPickUpPM GetFistShipmentPickUpPMByTenantAndShipmentId(string shipmentId, string shipmentNumber, int tenant)
        {
            ShipmentPickUpPM shipmentPickUpPM = (from entityPOCO in repository.context.ShipmentPickUpDeliveries.Include("FromPort").Include("ToPort").Include("CarrierCard").Include("TransportMode")
                                         where entityPOCO.ShipmentId == shipmentId && entityPOCO.Tenant == tenant && entityPOCO.PickUpDeliveryTypeCode == "PICK" 
                                         select new ShipmentPickUpPM()
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
                                             ChildPickUpIndex = entityPOCO.ChildPickUpIndex,
                                             StandaloneShipmentId = entityPOCO.StandaloneShipmentId,
                                             StandaloneShipmentNumber = entityPOCO.StandaloneShipmentNumber,
                                         }).OrderBy(d=>d.PickUpDeliveryNumber).FirstOrDefault();

            if (shipmentPickUpPM!=null)
            {
                ShipmentPickUpDeliveryPackageRepository packageRepository = new ShipmentPickUpDeliveryPackageRepository(repository.context);
                ShipmentPickUpDeliveryPackageQuery packagesQuery = new ShipmentPickUpDeliveryPackageQuery(packageRepository);

                AddressRepository addressRepository = new AddressRepository(tenant);
                CountryRepository countryRepository = new CountryRepository(tenant);
                CardRepository cardRepository = new CardRepository(tenant);

                shipmentPickUpPM.ShipmentPickUpDeliveryPackages = packagesQuery.GetShipmentPickUpDeliveryPackages(shipmentPickUpPM.Id, tenant);

                #region From PART
                if (shipmentPickUpPM.PickUpDeliveryFromTypeCode == "PART")
                {
                    if (!string.IsNullOrEmpty(shipmentPickUpPM.FromPartnerCardId))
                    {
                        Card card = cardRepository.GetSingleCard(shipmentPickUpPM.FromPartnerCardId, tenant);

                        Address address = null;

                        if (!string.IsNullOrEmpty(shipmentPickUpPM.FromAddressId))
                        {
                            address = addressRepository.GetSingleAddress(shipmentPickUpPM.FromAddressId, tenant);
                        }

                        else
                        {
                            address = addressRepository.GetMainAddressByCardId(shipmentPickUpPM.FromPartnerCardId, tenant);
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

                            shipmentPickUpPM.FromLocation = location;
                            shipmentPickUpPM.FromAddressCity_Dummy = address.City;

                            if (!string.IsNullOrEmpty(address.CountryId))
                            {
                                Country country = countryRepository.GetSingleCountry(address.CountryId, tenant);
                                if (country != null)
                                {
                                    shipmentPickUpPM.FromAddressCountryCode = country.Code;
                                    shipmentPickUpPM.FromAddressCountryName = country.EnglishName;
                                }
                            }
                        }
                    }
                }
                #endregion

                #region From PORT
                else if (shipmentPickUpPM.PickUpDeliveryFromTypeCode == "PORT")
                {
                    if (!string.IsNullOrEmpty(shipmentPickUpPM.FromPortId))
                    {
                        shipmentPickUpPM.FromLocation = shipmentPickUpPM.FromAddress;
                    }
                }
                #endregion

                #region From CASL
                else
                {
                    string location = "";

                    if (!string.IsNullOrEmpty(shipmentPickUpPM.FromAddressCity))
                    {
                        location = shipmentPickUpPM.FromAddressCity;
                    }

                    if (!string.IsNullOrEmpty(shipmentPickUpPM.FromAddressZipCode))
                    {
                        location = location + " " + shipmentPickUpPM.FromAddressZipCode;
                    }

                    if (!string.IsNullOrEmpty(shipmentPickUpPM.FromAddressCountryId))
                    {
                        Country country = countryRepository.GetSingleCountry(shipmentPickUpPM.FromAddressCountryId, tenant);
                        if (country != null)
                        {
                            location = location + Environment.NewLine + country.EnglishName;

                            shipmentPickUpPM.FromAddressCountryCode = country.Code;
                            shipmentPickUpPM.FromAddressCountryName = country.EnglishName;
                        }
                    }

                    shipmentPickUpPM.FromLocation = location;
                    shipmentPickUpPM.FromAddressCity_Dummy = shipmentPickUpPM.FromAddressCity;
                }
                #endregion

                #region To PART
                if (shipmentPickUpPM.PickUpDeliveryToTypeCode == "PART")
                {
                    if (!string.IsNullOrEmpty(shipmentPickUpPM.ToPartnerCardId))
                    {
                        Card card = cardRepository.GetSingleCard(shipmentPickUpPM.ToPartnerCardId, tenant);

                        Address address = null;

                        if (!string.IsNullOrEmpty(shipmentPickUpPM.ToAddressId))
                        {
                            address = addressRepository.GetSingleAddress(shipmentPickUpPM.ToAddressId, tenant);
                        }

                        else
                        {
                            address = addressRepository.GetMainAddressByCardId(shipmentPickUpPM.ToPartnerCardId, tenant);
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

                            shipmentPickUpPM.ToLocation = location;
                            shipmentPickUpPM.ToAddressCity_Dummy = address.City;

                            if (!string.IsNullOrEmpty(address.CountryId))
                            {
                                Country country = countryRepository.GetSingleCountry(address.CountryId, tenant);
                                if (country != null)
                                {
                                    shipmentPickUpPM.ToAddressCountryCode = country.Code;
                                    shipmentPickUpPM.ToAddressCountryName = country.EnglishName;
                                }
                            }
                        }
                    }
                }
                #endregion

                #region To PORT
                else if (shipmentPickUpPM.PickUpDeliveryToTypeCode == "PORT")
                {
                    if (!string.IsNullOrEmpty(shipmentPickUpPM.ToPortId))
                    {
                        shipmentPickUpPM.ToLocation = shipmentPickUpPM.ToAddress;
                    }
                }
                #endregion

                #region To CASL
                else
                {
                    string location = "";

                    if (!string.IsNullOrEmpty(shipmentPickUpPM.ToAddressCity))
                    {
                        location = shipmentPickUpPM.ToAddressCity;
                    }

                    if (!string.IsNullOrEmpty(shipmentPickUpPM.ToAddressZipCode))
                    {
                        location = location + " " + shipmentPickUpPM.ToAddressZipCode;
                    }

                    if (!string.IsNullOrEmpty(shipmentPickUpPM.ToAddressCountryId))
                    {
                        Country country = countryRepository.GetSingleCountry(shipmentPickUpPM.ToAddressCountryId, tenant);
                        if (country != null)
                        {
                            location = location + Environment.NewLine + country.EnglishName;

                            shipmentPickUpPM.ToAddressCountryCode = country.Code;
                            shipmentPickUpPM.ToAddressCountryName = country.EnglishName;
                        }
                    }

                    shipmentPickUpPM.ToLocation = location;
                    shipmentPickUpPM.ToAddressCity_Dummy = shipmentPickUpPM.ToAddressCity;
                }
                #endregion
            }

            return shipmentPickUpPM;
        }
    }
}
