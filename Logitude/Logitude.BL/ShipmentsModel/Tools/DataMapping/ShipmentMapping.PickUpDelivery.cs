using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.BL.ShipmentsModel.Tools.DataMapping
{
    public partial class ShipmentMapping
    {
        public static void MapPickUp(ShipmentPickUpPM itemPM, ShipmentPickUpDelivery itemPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Tenant = itemPM.Tenant;
                itemPoco.ShipmentId = itemPM.ShipmentId;
            }

            PortRepository portRepository = new PortRepository(itemPM.Tenant);

            if (!string.IsNullOrEmpty(itemPM.FromPortId))
            {
                Port port = portRepository.GetSinglePort(itemPM.Tenant, itemPM.FromPortId);
                if (port != null)
                {
                    itemPM.FromAddress = "Port Of: " + port.EnglishName;
                }
            }

            if (!string.IsNullOrEmpty(itemPM.ToPortId))
            {
                Port port = portRepository.GetSinglePort(itemPM.Tenant, itemPM.ToPortId);
                if (port != null)
                {
                    itemPM.ToAddress = "Port Of: " + port.EnglishName;
                }
            }

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
        }

        public static void MapDelivery(ShipmentDeliveryPM itemPM, ShipmentPickUpDelivery itemPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Tenant = itemPM.Tenant;
                itemPoco.ShipmentId = itemPM.ShipmentId;
            }

            PortRepository portRepository = new PortRepository(itemPM.Tenant);

            if (!string.IsNullOrEmpty(itemPM.FromPortId))
            {
                Port port = portRepository.GetSinglePort(itemPM.Tenant, itemPM.FromPortId);
                if (port != null)
                {
                    itemPM.FromAddress = "Port Of: " + port.EnglishName;
                }
            }

            if (!string.IsNullOrEmpty(itemPM.ToPortId))
            {
                Port port = portRepository.GetSinglePort(itemPM.Tenant, itemPM.ToPortId);
                if (port != null)
                {
                    itemPM.ToAddress = "Port Of: " + port.EnglishName;
                }
            }

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
        }
    }
}