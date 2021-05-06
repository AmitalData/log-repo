using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel;
using System.Runtime.CompilerServices;

namespace Logitude.BL.ShipmentsModel.Tools.DataMapping
{
    public partial class ShipmentMapping
    {
        public static void MapPickUp(ShipmentPickUpPM itemPM, ShipmentPickUpDelivery itemPoco, ICommonDataContext commonContext, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Tenant = itemPM.Tenant;
                itemPoco.ShipmentId = itemPM.ShipmentId;
            }

            SetPickupAddress(itemPM, commonContext);

            itemPoco.ATD = itemPM.ATD;
            itemPoco.ATA = itemPM.ATA;
            itemPoco.ToAddress = itemPM.ToAddress;
            itemPoco.ETD = itemPM.ETD;
            itemPoco.CarrierId = itemPM.CarrierId;
            itemPoco.ETA = itemPM.ETA;
            itemPoco.CarrierNumber = itemPM.CarrierNumber;
            itemPoco.Notes = itemPM.Notes;
            itemPoco.PickUpDeliveryNumber = itemPM.PickUpDeliveryNumber;
            itemPoco.Driver = itemPM.Driver;
            itemPoco.TruckNumber = itemPM.TruckNumber;
            itemPoco.FromPartnerCardId = itemPM.FromPartnerCardId;
            itemPoco.FromPortId = itemPM.FromPortId;
            itemPoco.ToPortId = itemPM.ToPortId;
            itemPoco.ToPartnerCardId = itemPM.ToPartnerCardId;
            itemPoco.FromAddress = itemPM.FromAddress;
            itemPoco.FullResponsibility = itemPM.FullResponsibility;
            itemPoco.TrailerNumber = itemPM.TrailerNumber;
            itemPoco.PickUpDeliveryToTypeCode = itemPM.PickUpDeliveryToTypeCode;
            itemPoco.PickUpDeliveryFromTypeCode = itemPM.PickUpDeliveryFromTypeCode;
            itemPoco.PickUpDeliveryTypeCode = itemPM.PickUpDeliveryTypeCode;                     
            itemPoco.FromAddressId = itemPM.FromAddressId;
            itemPoco.FromAddressCity = itemPM.FromAddressCity;
            itemPoco.FromAddressZipCode = itemPM.FromAddressZipCode;
            itemPoco.FromAddressCountryId = itemPM.FromAddressCountryId;
            itemPoco.ToAddressId = itemPM.ToAddressId;
            itemPoco.ToAddressCity = itemPM.ToAddressCity;
            itemPoco.ToAddressZipCode = itemPM.ToAddressZipCode;
            itemPoco.ToAddressCountryId = itemPM.ToAddressCountryId;
            itemPoco.EmptyPickupContainerPartnerId = itemPM.EmptyPickupContainerPartnerId;
            itemPoco.EmptyPickupDepotReference = itemPM.EmptyPickupDepotReference;
            itemPoco.EmptyDeliveryContainerPartnerId = itemPM.EmptyDeliveryContainerPartnerId;
            itemPoco.EmptyDeliveryDepotReference = itemPM.EmptyDeliveryDepotReference;
            itemPoco.TransportModeCode = itemPM.TransportModeCode;
            itemPoco.ParentPickUpDeliveryId = itemPM.ParentPickUpDeliveryId;
            itemPoco.ChildPickUpIndex = itemPM.ChildPickUpIndex;
            itemPoco.StandaloneShipmentId = itemPM.StandaloneShipmentId;
        }

        public static void MapDelivery(ShipmentDeliveryPM itemPM, ShipmentPickUpDelivery itemPoco, ICommonDataContext commonContext, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Tenant = itemPM.Tenant;
                itemPoco.ShipmentId = itemPM.ShipmentId;
            }

            SetDeliveryAddress(itemPM, commonContext);

            itemPoco.ATD = itemPM.ATD;
            itemPoco.ATA = itemPM.ATA;
            itemPoco.ToAddress = itemPM.ToAddress;
            itemPoco.ETD = itemPM.ETD;
            itemPoco.CarrierId = itemPM.CarrierId;
            itemPoco.ETA = itemPM.ETA;
            itemPoco.CarrierNumber = itemPM.CarrierNumber;
            itemPoco.Notes = itemPM.Notes;
            itemPoco.PickUpDeliveryNumber = itemPM.PickUpDeliveryNumber;
            itemPoco.Driver = itemPM.Driver;
            itemPoco.TruckNumber = itemPM.TruckNumber;
            itemPoco.FromPartnerCardId = itemPM.FromPartnerCardId;
            itemPoco.FromPortId = itemPM.FromPortId;
            itemPoco.ToPortId = itemPM.ToPortId;
            itemPoco.ToPartnerCardId = itemPM.ToPartnerCardId;
            itemPoco.FromAddress = itemPM.FromAddress;
            itemPoco.FullResponsibility = itemPM.FullResponsibility;
            itemPoco.TrailerNumber = itemPM.TrailerNumber;
            itemPoco.PickUpDeliveryToTypeCode = itemPM.PickUpDeliveryToTypeCode;
            itemPoco.PickUpDeliveryFromTypeCode = itemPM.PickUpDeliveryFromTypeCode;
            itemPoco.PickUpDeliveryTypeCode = itemPM.PickUpDeliveryTypeCode;
            itemPoco.FromAddressId = itemPM.FromAddressId;
            itemPoco.FromAddressCity = itemPM.FromAddressCity;
            itemPoco.FromAddressZipCode = itemPM.FromAddressZipCode;
            itemPoco.FromAddressCountryId = itemPM.FromAddressCountryId;
            itemPoco.ToAddressId = itemPM.ToAddressId;
            itemPoco.ToAddressCity = itemPM.ToAddressCity;
            itemPoco.ToAddressZipCode = itemPM.ToAddressZipCode;
            itemPoco.ToAddressCountryId = itemPM.ToAddressCountryId;
            itemPoco.EmptyPickupContainerPartnerId = itemPM.EmptyPickupContainerPartnerId;
            itemPoco.EmptyPickupDepotReference = itemPM.EmptyPickupDepotReference;
            itemPoco.EmptyDeliveryContainerPartnerId = itemPM.EmptyDeliveryContainerPartnerId;
            itemPoco.EmptyDeliveryDepotReference = itemPM.EmptyDeliveryDepotReference;
            itemPoco.TransportModeCode = itemPM.TransportModeCode;
            itemPoco.ParentPickUpDeliveryId = itemPM.ParentPickUpDeliveryId;
            itemPoco.ChildDeliveryIndex = itemPM.ChildDeliveryIndex;
            itemPoco.StandaloneShipmentId = itemPM.StandaloneShipmentId;
        }

        private static void SetPickupAddress(ShipmentPickUpPM itemPM, ICommonDataContext commonContext)
        {
            if (itemPM.FromPartnerCardId != null && itemPM.FromAddressId == null)
            {
                itemPM.FromAddressId = GetPartnerAddress(itemPM.FromPartnerCardId, itemPM.Tenant, commonContext);
            }

            else if (itemPM.FromPortId != null && itemPM.FromAddress == null)
            {
                itemPM.FromAddress = GetPortAddress(itemPM.FromPortId, itemPM.Tenant, commonContext);
            }

            if (itemPM.ToPartnerCardId != null && itemPM.ToAddressId == null)
            {
                itemPM.ToAddressId = GetPartnerAddress(itemPM.ToPartnerCardId, itemPM.Tenant, commonContext);
            }

            else if (itemPM.ToPortId != null && itemPM.ToAddress == null)
            {
                itemPM.ToAddress = GetPortAddress(itemPM.ToPortId, itemPM.Tenant, commonContext);
            }
        }

        private static void SetDeliveryAddress(ShipmentDeliveryPM itemPM, ICommonDataContext commonContext)
        {
            if (itemPM.FromPartnerCardId != null && itemPM.FromAddressId == null)
            {
                itemPM.FromAddressId = GetPartnerAddress(itemPM.FromPartnerCardId, itemPM.Tenant, commonContext);
            }

            else if (itemPM.FromPortId != null && itemPM.FromAddress == null)
            {
                itemPM.FromAddress = GetPortAddress(itemPM.FromPortId, itemPM.Tenant, commonContext);
            }

            if (itemPM.ToPartnerCardId != null && itemPM.ToAddressId == null)
            {
                itemPM.ToAddressId = GetPartnerAddress(itemPM.ToPartnerCardId, itemPM.Tenant, commonContext);
            }

            else if (itemPM.ToPortId != null && itemPM.ToAddress == null)
            {
                itemPM.ToAddress = GetPortAddress(itemPM.ToPortId, itemPM.Tenant, commonContext);
            }
        }

        private static string GetPartnerAddress(string cardId, int tenant, ICommonDataContext context)
        {
            string output = null;

            output = (from d in context.Addresses
                      where
                      d.CardId == cardId
                      && d.Tenant == tenant
                      && d.AddressTypeId == "P"
                      select d.Id).FirstOrDefault();


            if (output == null)
            {
                output = (from d in context.Addresses
                          where
                          d.CardId == cardId
                          && d.Tenant == tenant
                          && d.AddressTypeId == "M"
                          select d.Id).FirstOrDefault();
            }

            return output;
        }

        private static string GetPortAddress(string portId, int tenant, ICommonDataContext context)
        {
            string output = null;

            string englishName = (from d in context.Ports
                                  where
                                  d.Id == portId
                                  && d.Tenant == tenant
                                  select d.EnglishName).FirstOrDefault();

            if (englishName != null)
            {
                output = "Port Of: " + englishName;
            }

            return output;
        }
    }
}