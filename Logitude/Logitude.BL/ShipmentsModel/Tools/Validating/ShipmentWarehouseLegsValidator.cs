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
using Logitude.BL.ShipmentsModel.Tools.Behaviours.ShipmentBehaviours.Validators;

namespace Logitude.BL.ShipmentsModel.Tools.Validating
{
    public class ShipmentWarehouseLegsValidator
    {
        private ShipmentPM shipmentPM;
        public ShipmentWarehouseLegsValidator(ShipmentPM shipment)
        {
            this.shipmentPM = shipment;
        }
        private bool IsRoutingLegDatesValid(DateTime? firstDate, DateTime? secondDate)
        {
            if (firstDate == null || secondDate == null) return true;
            if (firstDate > secondDate.Value.AddHours(24)) return false;

            return true;
        }
        private bool IsDateSeriesSmaller(DateTime? firstDate, DateTime? secondDate)
        {
            if (firstDate == null || secondDate == null) return false;
            if (firstDate <= secondDate) return true;

            return false;
        }
        private bool IsDateSeriesSmallerNotEqual(DateTime? firstDate, DateTime? secondDate)
        {
            if (firstDate == null || secondDate == null) return false;
            if (firstDate < secondDate) return true;

            return false;
        }
        private bool IsDateSeriesBigger(DateTime? firstDate, DateTime? secondDate)
        {
            if (firstDate == null || secondDate == null) return false;
            if (firstDate >= secondDate) return true;

            return false;
        }
        public void ValidateWarehouseLeg()
        {
            ValidateWarehouseLegExists();
            ValidateWarehouseLegReleaseDates();
            if (shipmentPM.DirectionId == "I") ValidateImportShipments();
            else ValidateNotImportShipments();
        }

        private void ValidateNotImportShipments()
        {
            bool isPickupsExists = shipmentPM.ShipmentPickUps.Count() > 0;
            bool isPreForwardingExists = shipmentPM.PreForwardingFromPortId != null && shipmentPM.PreForwardingToPortId != null;
            bool isPreCarriageExists = shipmentPM.PreCarriageFromPortId != null && shipmentPM.PreCarriageToPortId != null;
            bool isMainCarriageExists = true;
            if (isPickupsExists) ValidateFirstPickupDates();
            if (isPreForwardingExists) ValidatePreForwardingDates();
            else if (isPreCarriageExists) ValidatePreCarriageDates();
            else if (isMainCarriageExists) ValidateMainCarriageReleaseDates();
        }

        private void ValidateImportShipments()
        {
            bool isOnForwardingExists = shipmentPM.OnForwardingFromPortId != null && shipmentPM.OnForwardingToPortId != null;
            bool isTransshipment1Exists = (shipmentPM.Transshipment1FromPortId != null && shipmentPM.Transshipment1ToPortId != null);
            bool isTransshipment2Exists = (shipmentPM.Transshipment2FromPortId != null && shipmentPM.Transshipment2ToPortId != null);
            bool isTransshipment3Exists = (shipmentPM.Transshipment3FromPortId != null && shipmentPM.Transshipment3ToPortId != null);
            bool isOnCarriageExists = shipmentPM.OnCarriageFromPortId != null && shipmentPM.OnCarriageToPortId != null;
            bool isDeliveriesExists = shipmentPM.ShipmentDeliveries.Count() > 0;
            if (isOnForwardingExists) ValidateOnForwardingDates();
            else if (isOnCarriageExists) ValidateOnCarriageDates();
            else if (isTransshipment3Exists) ValidateTransshipment3Dates();
            else if (isTransshipment2Exists) ValidateTransshipment2Dates();
            else if (isTransshipment1Exists) ValidateTransshipment1Dates();
            else ValidateMainCarriageEntryDates();
            if (isDeliveriesExists) { }
        }

        private void ValidateMainCarriageReleaseDates()
        {
            if (this.IsDateSeriesBigger(shipmentPM.WarehouseLegExpectedReleaseDate, shipmentPM.MainCarriageETD))
            {
                throw new ApplicationException("Warehouse expected release must be less than main carriage expected departure");
            }

            if (this.IsDateSeriesBigger(shipmentPM.WarehouseLegActualReleaseDate, shipmentPM.MainCarriageATD))
            {
                throw new ApplicationException("Warehouse actual release must be less than main carriage actual departure");
            }
        }

        private void ValidatePreCarriageDates()
        {
            if (this.IsDateSeriesBigger(shipmentPM.WarehouseLegExpectedReleaseDate, shipmentPM.PreCarriageETD))
            {
                throw new ApplicationException("Warehouse expected release must be less than pre carriage expected departure");
            }

            if (this.IsDateSeriesBigger(shipmentPM.WarehouseLegActualReleaseDate, shipmentPM.PreCarriageATD))
            {
                throw new ApplicationException("Warehouse actual release must be less than pre carriage actual departure");
            }
        }

        private void ValidatePreForwardingDates()
        {
            if (this.IsDateSeriesBigger(shipmentPM.WarehouseLegExpectedReleaseDate, shipmentPM.PreForwardingETD))
            {
                throw new ApplicationException("Warehouse expected release must be less than pre Forwarding expected departure");
            }

            if (this.IsDateSeriesBigger(shipmentPM.WarehouseLegActualReleaseDate, shipmentPM.PreForwardingATD))
            {
                throw new ApplicationException("Warehouse actual release must be less than pre Forwarding actual departure");
            }
        }

        private void ValidateFirstPickupDates()
        {
            ShipmentPickUpPM firsPickup = this.GetFirstPickup(shipmentPM.ShipmentPickUps);
            if (firsPickup != null)
            {
                if (IsDateSeriesSmallerNotEqual(shipmentPM.WarehouseLegExpectedEntryDate, firsPickup.ETA))
                {
                    throw new ApplicationException("Warehouse expected entry must be equal or bigger than first pick up expected arrival");
                }

                if (this.IsDateSeriesSmallerNotEqual(shipmentPM.WarehouseLegActualEntryDate, firsPickup.ATA))
                {
                    throw new ApplicationException("Warehouse actual entry must be equal or bigger than first pick up actual arrival");
                }
            }
        }

        private void ValidateMainCarriageEntryDates()
        {
            if (IsDateSeriesSmaller(shipmentPM.WarehouseLegExpectedEntryDate, shipmentPM.MainCarriageETA))
            {
                throw new ApplicationException("Warehouse expected entry must be bigger than Main-Carriage expected arrival");
            }

            if (IsDateSeriesSmaller(shipmentPM.WarehouseLegActualEntryDate, shipmentPM.MainCarriageATA))
            {
                throw new ApplicationException("Warehouse actual entry must be bigger than Main-Carriage actual arrival");
            }
        }

        private void ValidateTransshipment1Dates()
        {
            if (IsDateSeriesSmaller(shipmentPM.WarehouseLegExpectedEntryDate, shipmentPM.Transshipment1ETA))
            {
                throw new ApplicationException("Warehouse expected entry must be bigger than Transshipment1 expected arrival");
            }

            if (IsDateSeriesSmaller(shipmentPM.WarehouseLegActualEntryDate, shipmentPM.Transshipment1ATA))
            {
                throw new ApplicationException("Warehouse actual entry must be bigger than Transshipment1 actual arrival");
            }
        }

        private void ValidateTransshipment2Dates()
        {
            if (IsDateSeriesSmaller(shipmentPM.WarehouseLegExpectedEntryDate, shipmentPM.Transshipment2ETA))
            {
                throw new ApplicationException("Warehouse expected entry must be bigger than Transshipment2 expected arrival");
            }

            if (IsDateSeriesSmaller(shipmentPM.WarehouseLegActualEntryDate, shipmentPM.Transshipment2ATA))
            {
                throw new ApplicationException("Warehouse actual entry must be bigger than Transshipment2 actual arrival");
            }
        }

        private void ValidateTransshipment3Dates()
        {
            if (IsDateSeriesSmaller(shipmentPM.WarehouseLegExpectedEntryDate, shipmentPM.Transshipment3ETA))
            {
                throw new ApplicationException("Warehouse expected entry must be bigger than Transshipment3 expected arrival");
            }

            if (IsDateSeriesSmaller(shipmentPM.WarehouseLegActualEntryDate, shipmentPM.Transshipment3ATA))
            {
                throw new ApplicationException("Warehouse actual entry must be bigger than Transshipment3 actual arrival");
            }
        }

        private void ValidateOnCarriageDates()
        {
            if (IsDateSeriesSmaller(shipmentPM.WarehouseLegExpectedEntryDate, shipmentPM.OnCarriageETA))
            {
                throw new ApplicationException("Warehouse expected entry must be bigger than On-Carriage expected arrival");
            }

            if (IsDateSeriesSmaller(shipmentPM.WarehouseLegActualEntryDate, shipmentPM.OnCarriageATA))
            {
                throw new ApplicationException("Warehouse actual entry must be bigger than On-Carriage actual arrival");
            }
        }

        private void ValidateOnForwardingDates()
        {
            if (IsDateSeriesSmaller(shipmentPM.WarehouseLegExpectedEntryDate, shipmentPM.OnForwardingETA))
            {
                throw new ApplicationException("Warehouse expected entry must be bigger than On-Forwarding expected arrival");
            }

            if (IsDateSeriesSmaller(shipmentPM.WarehouseLegActualEntryDate, shipmentPM.OnForwardingATA))
            {
                throw new ApplicationException("Warehouse actual entry must be bigger than On-Forwarding actual arrival");
            }
        }

        private void ValidateWarehouseLegReleaseDates()
        {
            if (!IsRoutingLegDatesValid(shipmentPM.WarehouseLegExpectedEntryDate, shipmentPM.WarehouseLegExpectedReleaseDate))
            {
                throw new ApplicationException("Warehouse expected entry must be less than Warehouse expected release");
            }

            if (!IsRoutingLegDatesValid(shipmentPM.WarehouseLegActualEntryDate, shipmentPM.WarehouseLegActualReleaseDate))
            {
                throw new ApplicationException("Warehouse actual entry must be less than Warehouse actual release");
            }
        }

        private void ValidateWarehouseLegExists()
        {
            bool isWarehouseLegExists = shipmentPM.WarehouseLegWarehouseId != null;
            bool isEmptyWarehouseLegDates = shipmentPM.WarehouseLegExpectedReleaseDate == null && shipmentPM.WarehouseLegActualReleaseDate == null && shipmentPM.WarehouseLegExpectedEntryDate == null && shipmentPM.WarehouseLegActualEntryDate == null;
            if (!isWarehouseLegExists && !isEmptyWarehouseLegDates)
            {
                throw new ApplicationException("Terminal Field Is Required");
            }
        }

        private ShipmentPickUpPM GetFirstPickup(List<ShipmentPickUpPM> ShipmentPickUps)
        {
            ShipmentPickUpPM shipmentPickUpPM = null;
            if (ShipmentPickUps.Count() <= 0) return shipmentPickUpPM;

            int? index = null;
            ShipmentPickUps.ForEach(item =>
            {
                int itemIndex = Convert.ToInt32(item.PickUpDeliveryNumber.Split('/')[1]);
                if (index == null) shipmentPickUpPM = item;
                else if (itemIndex < index) shipmentPickUpPM = item;
                index = itemIndex;
            });

            return shipmentPickUpPM;
        }
    }
}