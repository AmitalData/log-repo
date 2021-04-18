using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ShipmentPickUpDeliveryQuery
    {
        ShipmentPickUpDeliveryRepository repository;
        public ShipmentPickUpDeliveryQuery(int tenant)
        {
            repository = new ShipmentPickUpDeliveryRepository(tenant);
        }
        public ShipmentPickUpDeliveryQuery(ShipmentPickUpDeliveryRepository repository)
        {
            this.repository = repository;
        }

        public object GetSinglePM(string id, int tenant)
        {
            AddressRepository addressRepository = new AddressRepository(tenant);
            CountryRepository countryRepository = new CountryRepository(tenant);
            CardRepository cardRepository = new CardRepository(tenant);

            ShipmentPickUpDelivery entityPOCO = repository.GetSingleShipmentPickUpDelivery(tenant, id);
            ShipmentPickUpPM pickUp = null;
            ShipmentDeliveryPM delivery = null;
            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            string shipmentNumber = "", bookingConfirmationNumber = "";
             
            if (entityPOCO != null)
            {
                Tuple<string, string> shipmentFields = shipmentQuery.GetShipmentFieldsForPickUpDelivery(entityPOCO.ShipmentId, entityPOCO.Tenant);
                if (shipmentFields != null)
                {
                    shipmentNumber = shipmentFields.Item1;
                    bookingConfirmationNumber = shipmentFields.Item2;
                }

                if (entityPOCO.PickUpDeliveryTypeCode == "PICK")
                {
                    pickUp = new ShipmentPickUpPM()
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
                        ShipmentNumber = shipmentNumber,
                        EmptyPickupContainerPartnerId = entityPOCO.EmptyPickupContainerPartnerId,
                        EmptyPickupDepotReference = entityPOCO.EmptyPickupDepotReference,
                        EmptyDeliveryContainerPartnerId = entityPOCO.EmptyDeliveryContainerPartnerId,
                        EmptyDeliveryDepotReference = entityPOCO.EmptyDeliveryDepotReference,
                        TransportModeCode = entityPOCO.TransportModeCode,
                        ParentPickUpDeliveryId = entityPOCO.ParentPickUpDeliveryId,
                        ChildPickUpIndex = entityPOCO.ChildPickUpIndex,
                        BookingConfirmationNumber = bookingConfirmationNumber,
                    };

                    ShipmentPickUpDeliveryPackageQuery packagesQuery = new ShipmentPickUpDeliveryPackageQuery(tenant);
                    pickUp.ShipmentPickUpDeliveryPackages = packagesQuery.GetShipmentPickUpDeliveryPackages(entityPOCO.Id, tenant);

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

                                pickUp.FromLocation = location;
                                pickUp.FromAddressCity_Dummy = address.City;

                                if (!string.IsNullOrEmpty(address.CountryId))
                                {
                                    Country country = countryRepository.GetSingleCountry(address.CountryId, tenant);
                                    if (country != null)
                                    {
                                        pickUp.FromAddressCountryCode = country.Code;
                                        pickUp.FromAddressCountryName = country.EnglishName;
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
                            pickUp.FromLocation = entityPOCO.FromAddress;
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

                                pickUp.FromAddressCountryCode = country.Code;
                                pickUp.FromAddressCountryName = country.EnglishName;
                            }
                        }

                        pickUp.FromLocation = location;
                        pickUp.FromAddressCity_Dummy = entityPOCO.FromAddressCity;
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

                                pickUp.ToLocation = location;
                                pickUp.ToAddressCity_Dummy = address.City;

                                if (!string.IsNullOrEmpty(address.CountryId))
                                {
                                    Country country = countryRepository.GetSingleCountry(address.CountryId, tenant);
                                    if (country != null)
                                    {
                                        pickUp.ToAddressCountryCode = country.Code;
                                        pickUp.ToAddressCountryName = country.EnglishName;
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
                            pickUp.ToLocation = entityPOCO.ToAddress;
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

                                pickUp.ToAddressCountryCode = country.Code;
                                pickUp.ToAddressCountryName = country.EnglishName;
                            }
                        }

                        pickUp.ToLocation = location;
                        pickUp.ToAddressCity_Dummy = entityPOCO.ToAddressCity;
                    }
                    #endregion
                }

                else
                {
                    delivery = new ShipmentDeliveryPM()
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
                        ShipmentNumber = shipmentNumber,
                        EmptyPickupContainerPartnerId = entityPOCO.EmptyPickupContainerPartnerId,
                        EmptyPickupDepotReference = entityPOCO.EmptyPickupDepotReference,
                        EmptyDeliveryContainerPartnerId = entityPOCO.EmptyDeliveryContainerPartnerId,
                        EmptyDeliveryDepotReference = entityPOCO.EmptyDeliveryDepotReference,
                        TransportModeCode = entityPOCO.TransportModeCode,
                        ParentPickUpDeliveryId = entityPOCO.ParentPickUpDeliveryId,
                        ChildDeliveryIndex = entityPOCO.ChildDeliveryIndex,
                        BookingConfirmationNumber = bookingConfirmationNumber,
                    };

                    ShipmentPickUpDeliveryPackageQuery packagesQuery = new ShipmentPickUpDeliveryPackageQuery(tenant);
                    delivery.ShipmentPickUpDeliveryPackages = packagesQuery.GetShipmentPickUpDeliveryPackages(entityPOCO.Id, tenant);

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

                                delivery.FromLocation = location;
                                delivery.FromAddressCity_Dummy = address.City;

                                if (!string.IsNullOrEmpty(address.CountryId))
                                {
                                    Country country = countryRepository.GetSingleCountry(address.CountryId, tenant);
                                    if (country != null)
                                    {
                                        delivery.FromAddressCountryCode = country.Code;
                                        delivery.FromAddressCountryName = country.EnglishName;
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
                            delivery.FromLocation = entityPOCO.FromAddress;
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

                                delivery.FromAddressCountryCode = country.Code;
                                delivery.FromAddressCountryName = country.EnglishName;
                            }
                        }

                        delivery.FromLocation = location;
                        delivery.FromAddressCity_Dummy = entityPOCO.FromAddressCity;
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

                                delivery.ToLocation = location;
                                delivery.ToAddressCity_Dummy = address.City;

                                if (!string.IsNullOrEmpty(address.CountryId))
                                {
                                    Country country = countryRepository.GetSingleCountry(address.CountryId, tenant);
                                    if (country != null)
                                    {
                                        delivery.ToAddressCountryCode = country.Code;
                                        delivery.ToAddressCountryName = country.EnglishName;
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
                            delivery.ToLocation = entityPOCO.ToAddress;
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

                                delivery.ToAddressCountryCode = country.Code;
                                delivery.ToAddressCountryName = country.EnglishName;
                            }
                        }

                        delivery.ToLocation = location;
                        delivery.ToAddressCity_Dummy = entityPOCO.ToAddressCity;
                    }
                    #endregion
                }                
            }

            if (entityPOCO == null)
            {
                return null;
            }
            else
            {
                if (entityPOCO.PickUpDeliveryTypeCode == "PICK")
                {
                    return pickUp;
                }
                else
                {
                    return delivery;
                }
            }
        }
        public string GetShipmentPickUpDeliveryCarrierIdById(string id, int tenant)
        {
            string entityId = (from a in repository.context.ShipmentPickUpDeliveries
                               where a.Id == id && a.Tenant == tenant 
                               select a.CarrierId).FirstOrDefault();
            return entityId;

        }      

    }
}
