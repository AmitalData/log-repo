using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.Helpers;
using System.Data;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
using System.Transactions;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BookingLib.Data.Repositories;
using Logitude.BookingLib.Data.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using System.Data.Entity.Core;
using Simplog.Data.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.BL.InvoiceModel.EntityQueries;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.QuoteModel;

namespace Logitude.BL.ShipmentsModel.Tools.Validating
{
    public class ShipmentDirectValidator
    {
        public static void ValidateUpdate(ShipmentPM shipmentPM, ShipmentPM oldShipmentPM)
        {
            if (oldShipmentPM.IsOperationalClosed) throw new Exception("Can't update operationally closed shipments");
            if (oldShipmentPM.IsCancelled) throw new Exception("Can't update cancelled shipments");
            if (IsInlandDomesticShipment(shipmentPM)) ValidateInlandDomesticShipment(shipmentPM);
            else ValidateNotInlandDomesticShipment(shipmentPM);
            ValidateCustomsFields(shipmentPM);
            ValidateOnCarriageDates(shipmentPM);
            ValidatePreCarriageDates(shipmentPM);
        }

        private static void ValidateNotInlandDomesticShipment(ShipmentPM shipmentPM)
        {
            ShipmentMainCarriageLegsValidator shipmentMainCarriageLegsValidator = new ShipmentMainCarriageLegsValidator(shipmentPM);
            shipmentMainCarriageLegsValidator.ValidateMainCarriageLegs();
            shipmentMainCarriageLegsValidator.ValidateRoutingsSeriesDates();
            shipmentMainCarriageLegsValidator.ValidateActualDates();

            ShipmentAccountingValidator shipmentAccountingValidator = new ShipmentAccountingValidator(shipmentPM);
            shipmentAccountingValidator.ValidateAccountingClosed();

            //ValidateCustomerData(shipmentPM);
            ValidateUpdateShipmentPackages(shipmentPM);
        }

        private static bool IsInlandDomesticShipment(ShipmentPM entityPM)
        {
            return entityPM.DirectionId == "D" && entityPM.TransportModeId == "I";
        }

        private static void ValidateCustomerData(ShipmentPM shipmentPM)
        {
            if (string.IsNullOrEmpty(shipmentPM.CustomerId)) ValidateCustomerId(shipmentPM);
            else if (!IsSentCustomerAShipmentPatrner(shipmentPM)) throw new Exception("The sent customer is not one of the sent partners");
        }

        private static void ValidateCustomerId(ShipmentPM shipmentPM)
        {
            string shipmentCustomerId;
            if (shipmentPM.DirectionId == "I") shipmentCustomerId = shipmentPM.ConsigneeId;
            else shipmentCustomerId = shipmentPM.ShipperId;

            if (string.IsNullOrEmpty(shipmentCustomerId)) throw new ApplicationException("The customer is required");
        }

        private static void ValidateInlandDomesticShipment(ShipmentPM entityPM)
        {
            ValidateInlandDomesticShipmentFromTypeCode(entityPM);
            ValidateInlandDomesticShipmentToTypeCode(entityPM);
            ValidateInlandDomesticMainCarriageDates(entityPM);
        }

        private static void ValidateInlandDomesticShipmentToTypeCode(ShipmentPM entityPM)
        {
            if (string.IsNullOrEmpty(entityPM.InlandDomesticToTypeCode))
            {
                throw new Exception("InlandDomesticToTypeCode Field is Required");
            }
            bool isCityOrCountryNull = string.IsNullOrEmpty(entityPM.InlandDomesticToCity) || string.IsNullOrEmpty(entityPM.InlandDomesticToCountryId);
            string[] inlandDomesticToTypeCodes = { "CASL", "PART", "PORT" };
            if (entityPM.InlandDomesticToTypeCode == "CASL" && isCityOrCountryNull)
            {
                throw new Exception("InlandDomestic To City And Country Fields are Required");
            }
            else if (entityPM.InlandDomesticToTypeCode == "PART" && string.IsNullOrEmpty(entityPM.MainCarriageToPartnerId))
            {
                throw new Exception("MainCarriageToPartner Field is Required");
            }
            else if (entityPM.InlandDomesticToTypeCode == "PORT" && string.IsNullOrEmpty(entityPM.MainCarriageToPortId))
            {
                throw new Exception("ToPort Field is Required");
            }
            else if (!inlandDomesticToTypeCodes.Contains(entityPM.InlandDomesticToTypeCode))
            {
                throw new Exception("Invalid InlandDomesticToTypeCode");
            }
        }

        private static void ValidateInlandDomesticShipmentFromTypeCode(ShipmentPM entityPM)
        {
            if (string.IsNullOrEmpty(entityPM.InlandDomesticFromTypeCode))
            {
                throw new Exception("InlandDomesticFromTypeCode Field is Required");
            }

            bool isCityOrCountryNull = string.IsNullOrEmpty(entityPM.InlandDomesticFromCity) || string.IsNullOrEmpty(entityPM.InlandDomesticFromCountryId);
            string[] inlandDomesticFromTypeCodes = { "CASL", "PART", "PORT" }; 
            if (entityPM.InlandDomesticFromTypeCode == "CASL" && isCityOrCountryNull)
            {
                throw new Exception("InlandDomesticFrom City And Country Fields are Required");
            }
            else if (entityPM.InlandDomesticFromTypeCode == "PART" && (string.IsNullOrEmpty(entityPM.MainCarriageFromPartnerId)))
            {
                throw new Exception("MainCarriageFromPartner Field is Required");
            }
            else if (entityPM.InlandDomesticFromTypeCode == "PORT" && string.IsNullOrEmpty(entityPM.MainCarriageFromPortId))
            {
                throw new Exception("FromPort Field is Required");
            }
            else if (!inlandDomesticFromTypeCodes.Contains(entityPM.InlandDomesticFromTypeCode))
            {
                throw new Exception("Invalid InlandDomesticFromTypeCode");
            }
        }

        private static void ValidateCustomsFields(ShipmentPM shipmentPM)
        {
            if (!IsAddingCustomsFields(shipmentPM)) return; 
            if (shipmentPM.CustomsClearanceDate != null) return;

            if (shipmentPM.DeclarationDate == null && !string.IsNullOrEmpty(shipmentPM.DeclarationNumber))
                throw new Exception("Declaration Date Field Is Required");
        }

        private static bool IsAddingCustomsFields(ShipmentPM shipmentPM)
        {
            if (shipmentPM.CustomsClearanceDate != null) return true;
            if (shipmentPM.DeclarationDate != null) return true;
            if (!string.IsNullOrEmpty(shipmentPM.DeclarationNumber)) return true;

            return false;
        }

        private static void ValidateOnCarriageDates(ShipmentPM entityPM)
        {
            if (!IsRoutingLegDatesValid(entityPM.OnCarriageETD, entityPM.OnCarriageETA))
            {
                throw new Exception("On Carriage expected departure must be less than On Carriage expected arrival");
            }

            if (!IsRoutingLegDatesValid(entityPM.OnCarriageATD, entityPM.OnCarriageATA))
            {
                throw new Exception("On Carriage actual departure must be less than On Carriage actual arrival");
            }
        }

        private static void ValidatePreCarriageDates(ShipmentPM entityPM)
        {
            if (!IsRoutingLegDatesValid(entityPM.PreCarriageETD, entityPM.PreCarriageETA))
            {
                throw new Exception("Pre Carriage expected departure must be less than Pre Carriage expected arrival");
            }

            if (!IsRoutingLegDatesValid(entityPM.PreCarriageATD, entityPM.PreCarriageATA))
            {
                throw new Exception("Pre Carriage actual departure must be less than Pre Carriage actual arrival");
            }
        }

        private static void ValidateInlandDomesticMainCarriageDates(ShipmentPM entityPM)
        {
            if (!IsRoutingLegDatesValid(entityPM.MainCarriageETD, entityPM.MainCarriageETA))
            {
                throw new Exception("Main-Carriage expected departure must be less than Main-Carriage expected arrival");
            }

            if (!IsRoutingLegDatesValid(entityPM.MainCarriageATD, entityPM.MainCarriageATA))
            {
                throw new Exception("Main-Carriage actual departure must be less than Main-Carriage actual arrival");
            }

        }

        private static bool IsRoutingLegDatesValid(DateTime? fisrtDate, DateTime? secondeDate)
        {
            if (fisrtDate == null || secondeDate == null) return true;
            if (fisrtDate > secondeDate.Value.AddHours(24)) return false;

            return true;
        }

        private static void ValidateUpdateShipmentPackages(ShipmentPM entityPM)
        {
            if (!(entityPM.ShipmentPackages.Count > 0)) return;
            foreach (ShipmentPackagePM item in entityPM.ShipmentPackages)
            {
                ValidateShipmentPackageItem(item, entityPM);
            }
        }

        private static void ValidateShipmentPackageItem(ShipmentPackagePM item, ShipmentPM entityPM)
        {
            if (item.IsContainer) ValidateInsidePackage(item);

            if (item.InsideShipmentPackages != null && item.InsideShipmentPackages.Count > 0)
                throw new Exception("Inside Packages allowed in FCL/FTL shipments only");
        }

        private static void ValidateInsidePackage(ShipmentPackagePM item)
        {
            if (!(item.InsideShipmentPackages != null && item.InsideShipmentPackages.Count > 0)) return;

            item.InsideShipmentPackages.ForEach(inside =>
            {
                if (inside.Quantity == null) throw new Exception("Inside Packages Quantity is required");
            });
        }

        private static bool IsSentCustomerAShipmentPatrner(ShipmentPM entityPM)
        {
            List<string> shipmentPartners = new List<string>
            {
                entityPM.ShipperId,
                entityPM.ConsigneeId,
                entityPM.ShipperNotExporterId,
                entityPM.AgentId,
                entityPM.CustomAgentImportId,
                entityPM.ReleasingAgentId,
                entityPM.FreightForwarderId,
                entityPM.CustomAgentExportId,
                entityPM.ConsigneeNotImporterId
            };

            if (shipmentPartners.Exists(shipmentPartner => shipmentPartner == entityPM.CustomerId)) return true;

            return false;
        }
    }
}