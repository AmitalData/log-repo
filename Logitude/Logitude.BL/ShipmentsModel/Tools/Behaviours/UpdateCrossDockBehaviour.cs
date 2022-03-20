using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.Behaviours;
using Logitude.BL.ShipmentsModel.Tools.TraceEvents;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.WarehouseLib.Data;
using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.WarehouseLib.Data.Repositories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviour
{
    public class UpdateCrossDockBehaviour : IShipmentBehaviour
    {
        private ShipmentPM shipmentPM;
        private IWarehouseContext warehouseContext;
        private WarehouseEntryRepository warehouseEntryRepository;
        private WarehouseReleaseRepository warehouseReleaseRepository;
        private IQueryable<WarehouseEntry> warehouseEntries;
        private IQueryable<WarehouseRelease> warehouseRelases;
        private StorageCalculater storageCalculater;
        private int tenant;

        public bool ReceivablePricingUpdated { get; set; }
        public bool DatesFromCrossDocsUpdated { get; set; }

        public UpdateCrossDockBehaviour(ShipmentPM shipmentPM)
        {
            this.shipmentPM = shipmentPM;
            this.tenant = shipmentPM.Tenant;

            warehouseContext = WarehouseContext.GetContext(tenant);
            warehouseEntryRepository = new WarehouseEntryRepository(warehouseContext);
            warehouseReleaseRepository = new WarehouseReleaseRepository(warehouseContext);
            warehouseEntries = warehouseEntryRepository.GetWarehouseEntriesByshipmentId(shipmentPM.Id, tenant);
            warehouseRelases = warehouseReleaseRepository.GetWarehouseReleasesByshipmentId(shipmentPM.Id, tenant);

            storageCalculater = new StorageCalculater(this.shipmentPM);
        }

        public void Handle()
        {
            UpdateShipmentWarehouseLegData();
            UpdateWarehouseLegDates();
            UpdateDeliveryDepartureDates();
            UpdateCrossDockReleaseStatus();
            UpdateWareHouseEntryPartners();
            ConnectWarehouseReleaseToShipment();
        }
        public void Trace(ShipmentTracing shipmentTracing)
        {
            shipmentTracing.TraceTerminalData();
        }
        private void UpdateShipmentWarehouseLegData()
        {
            if (shipmentPM.IsUpdateWarehouseLegData)
            {
                ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
                CardRepository cardRepository = new CardRepository(commonContext);
                Card entityPoco = cardRepository.GetSingleCardByIdAndTenant(shipmentPM.WarehouseLegWarehouseId, tenant, true);

                if (entityPoco != null)
                {
                    AddressRepository addressRepository = new AddressRepository(commonContext);
                    Address mainAddress = addressRepository.GetSingleAddressByCardIdAndTypeId(entityPoco.Id, "M", tenant);
                    shipmentPM.WarehouseLegAddressId = mainAddress == null ? null : mainAddress.Id;
                    shipmentPM.WarehouseLegTerminalName = entityPoco.EnglishName;
                    shipmentPM.WarehouseLegTerminalCode = entityPoco.Warehouse != null ? entityPoco.Warehouse.FirmCode : null;

                    //TenantQuery tenantQuery = new TenantQuery(tenant);
                    //string countryCode = tenantQuery.GetTenantCountryCodeOnly(tenant);
                    //bool usTenant = countryCode == null ? false : countryCode.ToUpper() == "US" ? true : false;
                    //if(usTenant) shipmentPM.WarehouseLegTerminalCode = entityPoco.Warehouse != null ? entityPoco.Warehouse.FirmCode : null;
                }
            }
        }
        private void UpdateWarehouseLegDates()
        {
            UpdateActualExpectedWarehouseEntriesDates();
            UpdateActualExpectedWarehouseReleasesDates();
            UpdateActualExpectedShipmentWarehouseLegDates();
        }
        public void Save()
        {
            warehouseContext.SaveChanges();
        }
        private void UpdateActualExpectedWarehouseEntriesDates()
        {
            bool isUpdated = false;

            IQueryable<WarehouseEntry> activeWarehouseEntries = warehouseEntries.Where(e => e.StatusCode != "CAEA");
            if (activeWarehouseEntries.Count() != 0)
            {
                WarehouseEntry leastWarehouseEntry = activeWarehouseEntries.Where(e => e.ActualEntryDate != null).OrderBy(e => e.ActualEntryDate).FirstOrDefault();
                if (leastWarehouseEntry != null)
                {
                    shipmentPM.WarehouseLegActualEntryDate = leastWarehouseEntry.ActualEntryDate;
                    shipmentPM.WarehouseLegExpectedEntryDate = leastWarehouseEntry.ExpectedEntryDate;
                    isUpdated = true;
                }
            }
            else if (warehouseEntries.Count() != 0)
            {
                shipmentPM.WarehouseLegActualEntryDate = null;
                shipmentPM.WarehouseLegExpectedEntryDate = null;
                isUpdated = true;
            }

            if (isUpdated)
            {
                DatesFromCrossDocsUpdated = true;

                if (shipmentPM.IsCFSWarehouse)
                {
                    storageCalculater.StartCalculations();
                    ReceivablePricingUpdated = true;
                    shipmentPM.CalculateProfit = true;
                }
            }
        }
        private void UpdateActualExpectedWarehouseReleasesDates()
        {
            IQueryable<WarehouseRelease> activeWarehouseRelases = warehouseRelases.Where(e => e.StatusCode != "CARE");
            if (activeWarehouseRelases.Count() == 1)
            {
                WarehouseRelease warehouseRelease = activeWarehouseRelases.FirstOrDefault();
                if (shipmentPM.WarehouseLegActualReleaseDate != null && warehouseRelease.ActualReleaseDate == null)
                {
                    warehouseRelease.ActualReleaseDate = shipmentPM.WarehouseLegActualReleaseDate;
                }
                if (shipmentPM.WarehouseLegExpectedReleaseDate != null && warehouseRelease.ExpectedReleaseDate == null)
                {
                    warehouseRelease.ExpectedReleaseDate = shipmentPM.WarehouseLegExpectedReleaseDate;
                }
                warehouseReleaseRepository.Update(warehouseRelease);
            }
        }
        private void UpdateActualExpectedShipmentWarehouseLegDates()
        {
            bool isUpdated = false;

            IQueryable<WarehouseRelease> activeWarehouseRelases = warehouseRelases.Where(e => e.StatusCode != "CARE");
            if (activeWarehouseRelases.Count() != 0)
            {
                WarehouseRelease greatestWarehouseRelease = activeWarehouseRelases.Where(r => r.ActualReleaseDate != null).OrderByDescending(r => r.ActualReleaseDate).FirstOrDefault();
                if (greatestWarehouseRelease != null)
                {
                    shipmentPM.WarehouseLegActualReleaseDate = greatestWarehouseRelease.ActualReleaseDate;
                    shipmentPM.WarehouseLegExpectedReleaseDate = greatestWarehouseRelease.ExpectedReleaseDate;
                    shipmentPM.GrossWeightPerStorageDays = storageCalculater.ComputeGrossWeight_PerStorageDays();
                    shipmentPM.WarehouseLegLastFreeDate = storageCalculater.GetWarehouseLegLastFreeDate();
                    isUpdated = true;
                }
            }
            else if (warehouseRelases.Count() != 0)
            {
                shipmentPM.WarehouseLegActualReleaseDate = null;
                shipmentPM.WarehouseLegExpectedReleaseDate = null;
                shipmentPM.GrossWeightPerStorageDays = null;
                shipmentPM.WarehouseLegLastFreeDate = null;
               isUpdated = true;
            }

            if (isUpdated)
            {
                DatesFromCrossDocsUpdated = true;

                if (shipmentPM.IsCFSWarehouse)
                {
                    storageCalculater.StartCalculations();
                    ReceivablePricingUpdated = true;
                    shipmentPM.CalculateProfit = true;
                }
            }
        }
        private void UpdateDeliveryDepartureDates()
        {
            IQueryable<WarehouseRelease> activeDeliveryWarehouseRelases = warehouseRelases.Where(r => r.StatusCode != "CARE" && r.ConnectedTo == "Delivery");
            ShipmentPickUpDeliveryRepository shipmentPickUpDeliveryRepository = new ShipmentPickUpDeliveryRepository(tenant);
            List<ShipmentPickUpDelivery> shipmentDeliveries = shipmentPickUpDeliveryRepository.GetShipmentDeliveryByShipmentId(shipmentPM.Id, tenant);

            IQueryable<WarehouseRelease> deliveryWarehouseReleases = null;
            shipmentDeliveries.ForEach(delivery =>
            {
                deliveryWarehouseReleases = activeDeliveryWarehouseRelases.Where(r => r.ChildEntityReference == delivery.PickUpDeliveryNumber);
                if (deliveryWarehouseReleases.Count() == 1)
                {
                    WarehouseRelease warehouseRelease = deliveryWarehouseReleases.FirstOrDefault();
                    delivery.ATD = warehouseRelease != null ? warehouseRelease.ActualReleaseDate : null;
                    delivery.ETD = warehouseRelease != null ? warehouseRelease.ExpectedReleaseDate : null;
                    shipmentPickUpDeliveryRepository.Update(delivery);
                }
            });

            shipmentPickUpDeliveryRepository.SubmitChanges();

        }
        private void UpdateCrossDockReleaseStatus()
        {
            if (shipmentPM.MainCarriageATD == null) ChangeCrossDockReleaseStatus("RELE", "CREA");
            else
            {
                EntityStatus departedStatus = EntityStatusRepository.GetSingleEntityStatusByCode("SDEP", tenant, true);
                if (departedStatus != null)
                {
                    EntityStatus entityStatus = EntityStatusRepository.GetSingleEntityStatus(shipmentPM.StatusId, tenant, true);
                    if (entityStatus != null && entityStatus.StatusWeight >= departedStatus.StatusWeight) ChangeCrossDockReleaseStatus("CREA", "RELE");
                }
            }
        }
        private void ChangeCrossDockReleaseStatus(string fromStatusCode, string toStatusCode)
        {
            List<WarehouseRelease> warehouseReleasesLists = warehouseRelases.Where(r => r.StatusCode == fromStatusCode).ToList();
            foreach (WarehouseRelease item in warehouseReleasesLists)
            {
                if (IsUpdateWarehouseStatus(item, fromStatusCode))
                {
                    item.StatusCode = toStatusCode;
                    warehouseReleaseRepository.Update(item);
                }
            }
        }
        private bool IsUpdateWarehouseStatus(WarehouseRelease warehouseRelease, string fromStatusCode)
        {
            return (fromStatusCode == "CREA" || (fromStatusCode == "RELE" && warehouseRelease.ActualReleaseDate == null));
        }
        private void UpdateWareHouseEntryPartners()
        {
            List<WarehouseEntry> warehouseEntryLists = null;
            string fromPortId = !string.IsNullOrEmpty(shipmentPM.MainCarriageFromPortId) ? shipmentPM.MainCarriageFromPortId : shipmentPM.FromPortId;
            string toPortId = shipmentPM.ShipmentLevelCode == "H" ? shipmentPM.MainCarriageFinalDestinationPortId : shipmentPM.FinalDistenationPortId;

            if (shipmentPM.DirectionId == "D" && shipmentPM.TransportModeId == "I")
            {
                warehouseEntryLists = warehouseEntries.Where(d => d.FromAddressId != shipmentPM.MainCarriageFromAddressId || d.ToAddressId != shipmentPM.MainCarriageToAddressId || d.FromPartnerId != shipmentPM.MainCarriageFromPartnerId || d.ToAddressId != shipmentPM.MainCarriageToPartnerId).ToList();
            }
            else warehouseEntryLists = warehouseEntries.Where(d => d.FromPortId != fromPortId || d.ToPortId != toPortId).ToList();

            if (warehouseEntryLists != null && warehouseEntryLists.Count > 0)
            {
                foreach (WarehouseEntry warehouseEntry in warehouseEntryLists)
                {
                    if (shipmentPM.DirectionId == "D" && shipmentPM.TransportModeId == "I")
                    {
                        warehouseEntry.FromAddressId = shipmentPM.MainCarriageFromAddressId;
                        warehouseEntry.ToAddressId = shipmentPM.MainCarriageToAddressId;
                        warehouseEntry.FromPartnerId = shipmentPM.MainCarriageFromPartnerId;
                        warehouseEntry.ToPartnerId = shipmentPM.MainCarriageToPartnerId;
                    }
                    else
                    {
                        warehouseEntry.FromPortId = fromPortId;
                        warehouseEntry.ToPortId = toPortId;
                    }

                    warehouseEntryRepository.Update(warehouseEntry);
                }
            }
        }
        private void ConnectWarehouseReleaseToShipment()
        {
            if (!string.IsNullOrEmpty(shipmentPM.WarehouseReleasesIds))
            {
                List<string> warehouseReleasesIdLists = shipmentPM.WarehouseReleasesIds.Split(',').ToList();
                if (warehouseReleasesIdLists.Count > 0)
                {
                    List<WarehouseRelease> warehouseReleasesLists = warehouseReleaseRepository.GetAll(tenant).Where(d => warehouseReleasesIdLists.Contains(d.Id)).ToList();
                    foreach (WarehouseRelease item in warehouseReleasesLists)
                    {
                        item.IsUsed = true;
                        if (string.IsNullOrEmpty(item.ShipmentId)) item.ShipmentId = shipmentPM.Id;
                        warehouseReleaseRepository.Update(item);
                    }
                }
            }
            shipmentPM.WarehouseReleasesIds = null;
        }
    }

    public class StorageCalculater
    {
        private ShipmentPM shipmentPM;
        private int tenant;
        private int? storageDays;
        private int? freeDays;
        private ShipmentReceivablePM storageShipmentReceivable;
        private bool readyToCalculateStorage = true;
        private DateTime todayDate;
        public StorageCalculater(ShipmentPM shipmentPM)
        {
            this.shipmentPM = shipmentPM;
            this.tenant = shipmentPM.Tenant;
            this.todayDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            this.storageShipmentReceivable = shipmentPM.ShipmentReceivables.Where(d => d.ChargesTypeCode == "ISTOR" && d.MeasurementCode == "STFE" && string.IsNullOrEmpty(d.ARInvoiceId)).FirstOrDefault();
        }

        public void StartCalculations()
        {
            this.storageDays = this.ComputeStorageDays();
            this.freeDays = this.GetFreeDays();
            this.readyToCalculateStorage = this.CheckIfReadyToCalculateStorage();
            if (readyToCalculateStorage)
            {
                double? amount = this.ComputeReceivableAmount();

                if (storageShipmentReceivable != null)
                {
                    this.UpdateStorageShipmentReceivable(amount);
                }

                else
                {
                    this.CreateStorageShipmentReceivable(amount);
                }
            }

            else
            {
                this.DeleteStorageShipmentReceivable();                
            }
        }
        private int? ComputeStorageDays()
        {
            int? storageDays = null;

            if (shipmentPM.WarehouseLegActualEntryDate != null && shipmentPM.WarehouseLegActualReleaseDate != null)
            {
                if (shipmentPM.WarehouseLegActualReleaseDate >= shipmentPM.WarehouseLegActualEntryDate)
                {
                    storageDays = (shipmentPM.WarehouseLegActualReleaseDate - shipmentPM.WarehouseLegActualEntryDate).Value.Days;
                }
            }

            return storageDays;
        }

        public double? ComputeGrossWeight_PerStorageDays()
        {
            double? weightPerStorageDays;
            var storageDays = this.ComputeStorageDays();
            if (shipmentPM.TransportModeId != "A" && shipmentPM.GrossWeightPerTon != null && shipmentPM.WarehouseStorageFreeDays != null)
            {
                weightPerStorageDays = Math.Ceiling(shipmentPM.GrossWeightPerTon.Value) * (storageDays - shipmentPM.WarehouseStorageFreeDays.Value);
            }
            else
            {
                weightPerStorageDays = shipmentPM.ChargeableWeight * (storageDays - shipmentPM.WarehouseStorageFreeDays);
            }
            return weightPerStorageDays < 0 ? 0 : weightPerStorageDays;
        }
        public DateTime? GetWarehouseLegLastFreeDate()
        {
            DateTime? WarehouseLegLastFreeDate = null;
            if (shipmentPM.WarehouseLegActualEntryDate != null && shipmentPM.WarehouseStorageFreeDays != null)
            {
                var date = shipmentPM.WarehouseLegActualEntryDate.Value.AddDays(shipmentPM.WarehouseStorageFreeDays.Value);
                if (date != null)
                {
                    WarehouseLegLastFreeDate = date;
                }
            }

            return WarehouseLegLastFreeDate;
        }

        private int? GetFreeDays()
        {
            int? freeDays = 0;

            if (shipmentPM.WarehouseStorageFreeDays != null)
            {
                freeDays = shipmentPM.WarehouseStorageFreeDays;
            }

            return freeDays;
        }
        private bool CheckIfReadyToCalculateStorage()
        {
            if (shipmentPM.WarehouseLegActualEntryDate == null)
            {
                return false;
            }

            if (shipmentPM.WarehouseLegActualReleaseDate == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(shipmentPM.ChargeStorageCurrencyId))
            {
                return false;
            }

            if (!shipmentPM.ChargeStorage)
            {
                return false;
            }

            if (shipmentPM.ShipmentStoragePricings.Count == 0)
            {
                return false;
            }

            if (storageDays == null)
            {
                return false;
            }

            if (storageDays <= freeDays)
            {
                return false;
            }

            return true;
        }
        private void CreateStorageShipmentReceivable(double? amount)
        {
            if (amount != null && amount != 0)
            {
                ChargesType importStorageChargesType = this.GetImportStorageChargesType();                

                if (importStorageChargesType != null)
                {
                    this.InitiateStorageShipmentReceivable();
                    this.SetReceivaleFieldsFromImportStorageCharge(importStorageChargesType);
                    this.SetReceivableRates();
                    this.SetPrepaidCollect(importStorageChargesType);
                    this.SetAmountsFields(amount);

                    shipmentPM.ShipmentReceivables.Add(storageShipmentReceivable);
                }
            }
        }
        private void UpdateStorageShipmentReceivable(double? amount)
        {
            if (storageShipmentReceivable.TotalAmount != amount)
            {
                storageShipmentReceivable.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                this.SetAmountsFields(amount);
            }
        }
        private void DeleteStorageShipmentReceivable()
        {
            if (storageShipmentReceivable != null)
            {
                storageShipmentReceivable.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
            }
        }    
        private ChargesType GetImportStorageChargesType()
        {
            string importStorageChargeCode = "ISTOR";
            ChargesTypeRepository chargesTypeRepository = new ChargesTypeRepository(tenant);
            return chargesTypeRepository.GetSingleChargesTypeByCode(importStorageChargeCode, tenant);
        }
        private string GetLocalCurrencyId()
        {
            string localCurrencyId = null;

            TenantRepository tenantRepository = new TenantRepository(tenant);
            Tenant myTenant = tenantRepository.GetSingleTenant(tenant);
            if (myTenant != null)
            {
                localCurrencyId = myTenant.CurrencyId;
            }

            return localCurrencyId;
        }
        private string GetLoggedUserId()
        {
            string loggedUserId = null;
            string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);
            if (loggedContact != null)
            {
                loggedUserId = loggedContact.Id;
            }

            return loggedUserId;
        }
        private void InitiateStorageShipmentReceivable()
        {
            string loggedUserId = this.GetLoggedUserId();

            storageShipmentReceivable = new ShipmentReceivablePM();
            storageShipmentReceivable.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            storageShipmentReceivable.Tenant = shipmentPM.Tenant;
            storageShipmentReceivable.ShipmentId = shipmentPM.Id;
            storageShipmentReceivable.ShipmentNumber = shipmentPM.ShipmentNumber;
            storageShipmentReceivable.CreateDate = todayDate;
            storageShipmentReceivable.UpdateDate = todayDate;
            storageShipmentReceivable.CreatedByUserId = loggedUserId;
            storageShipmentReceivable.UpdateByUserId = loggedUserId;
            storageShipmentReceivable.ShipmentReceivableLineStatusCode = "OAMT";
            storageShipmentReceivable.CurrencyId = shipmentPM.ChargeStorageCurrencyId;
        }
        private void SetReceivaleFieldsFromImportStorageCharge(ChargesType importStorageChargesType)
        {
            storageShipmentReceivable.ChargesTypeId = importStorageChargesType.Id;
            storageShipmentReceivable.ChargesTypeCode = importStorageChargesType.Code;
            storageShipmentReceivable.ChargesTypeName = importStorageChargesType.EnglishName;
            storageShipmentReceivable.MeasurementId = importStorageChargesType.MeasurementId;
            storageShipmentReceivable.MeasurementCode = importStorageChargesType.Measurement == null ? null : importStorageChargesType.Measurement.Code;
            storageShipmentReceivable.ChargesGroupCode = importStorageChargesType.ChargesGroupCode;
            storageShipmentReceivable.DueTypeCode = importStorageChargesType.DueTypeCode;
            storageShipmentReceivable.VatTypeId = importStorageChargesType.VatTypeId;
            storageShipmentReceivable.IATACodeId = importStorageChargesType.IATACodeId;
            storageShipmentReceivable.IsExpense = importStorageChargesType.IsExpense;
        }
        private void SetPrepaidCollect(ChargesType importStorageChargesType)
        {
            if (importStorageChargesType.ChargesGroupCode == "FRT")
            {
                storageShipmentReceivable.PrepaidCollectId = shipmentPM.FreightPrepaidCollectId;
            }

            else
            {
                storageShipmentReceivable.PrepaidCollectId = shipmentPM.OtherPrepaidCollectId;
            }
        }
        private void SetReceivableRates()
        {
            string localCurrencyId = this.GetLocalCurrencyId();
            List<LastRate> allRates = this.GetAllRates(localCurrencyId, todayDate);
            this.SetRate(allRates, localCurrencyId);
            this.SetProfitRate(allRates, localCurrencyId); 
        }
        private void SetAmountsFields(double? amount)
        {
            storageShipmentReceivable.TotalAmount = amount;
            storageShipmentReceivable.TotalAmountLocal = MethodHelper.Round(storageShipmentReceivable.TotalAmount * storageShipmentReceivable.Rate, 2);

            if (storageShipmentReceivable.CurrencyId == shipmentPM.ProfitCurrencyId)
            {
                storageShipmentReceivable.AmountInProfitCurrency = storageShipmentReceivable.TotalAmount;
            }

            else
            {
                storageShipmentReceivable.AmountInProfitCurrency = (storageShipmentReceivable.TotalAmountLocal / storageShipmentReceivable.ProfitCurrencyExchangeRate);
            }
        }
        private void SetProfitRate(List<LastRate> allRates, string localCurrencyId)
        {
            if (shipmentPM.ProfitCurrencyId == localCurrencyId)
            {
                storageShipmentReceivable.ProfitCurrencyExchangeRate = 1;
            }

            else
            {
                LastRate myLastRate = allRates.Where(d => d.ForeignCurrencyId == shipmentPM.ProfitCurrencyId).FirstOrDefault();
                if (myLastRate != null)
                {
                    storageShipmentReceivable.ProfitCurrencyExchangeRate = myLastRate.Rate;
                }
            }
        }
        private void SetRate(List<LastRate> allRates, string localCurrencyId)
        {
            if (localCurrencyId == storageShipmentReceivable.CurrencyId)
            {
                storageShipmentReceivable.Rate = 1;
            }
            else
            {
                LastRate lastRate = allRates.Where(d => d.ForeignCurrencyId == storageShipmentReceivable.CurrencyId).FirstOrDefault();
                if (lastRate != null)
                {
                    storageShipmentReceivable.Rate = lastRate.Rate;
                }
            }
        }

        private double? ComputeReceivableAmount()
        {
            double? amount = 0;            
            double? weight = this.ComputeStorageWeight();

            List<CalculatedPricingItem> myPricigs = new List<CalculatedPricingItem>();
            if (weight != null && weight != 0)
            {
                foreach (ShipmentStoragePricingPM item in shipmentPM.ShipmentStoragePricings.OrderBy(d => d.LineNumber))
                {
                    CalculatedPricingItem calculatedPricingItem = this.CreateCalculatedPricingItem(item, weight, myPricigs);
                    myPricigs.Add(calculatedPricingItem);
                }

                this.UpdateShipmentStoragePricingLine(myPricigs);
                amount = Convert.ToDouble(myPricigs.Sum(s => s.Amount));
            }

            double? invoicedStorageReceivableAmount = this.GetInvoicedStorageReceivableAmount();
            if (invoicedStorageReceivableAmount != null)
            {
                return (amount - invoicedStorageReceivableAmount);
            }

            else
            {
                return amount;
            }
        }
        private double? GetInvoicedStorageReceivableAmount()
        {
            double? invoicedStorageReceivableAmount = null;

            ShipmentReceivablePM invoiceStorageReceivable = shipmentPM.ShipmentReceivables.Where(d => d.ChargesTypeCode == "ISTOR" && d.MeasurementCode == "STFE" && !string.IsNullOrEmpty(d.ARInvoiceId)).FirstOrDefault();
            if (invoiceStorageReceivable != null)
            {
                invoicedStorageReceivableAmount = invoiceStorageReceivable.TotalAmount;
            }

            return invoicedStorageReceivableAmount;
        }
        private CalculatedPricingItem CreateCalculatedPricingItem(ShipmentStoragePricingPM shipmentStoragePricing, double? weight, List<CalculatedPricingItem> myPricigs)
        {
            int? allChargeableDays = storageDays - freeDays;

            CalculatedPricingItem calculatedPricingItem = new CalculatedPricingItem();
            calculatedPricingItem.LineNumber = shipmentStoragePricing.LineNumber;
            calculatedPricingItem.Price = shipmentStoragePricing.SalePrice;
            calculatedPricingItem.ChargeableDays = allChargeableDays - myPricigs.Sum(s => s.ChargeableDays);

            if (shipmentStoragePricing.Days != null && shipmentStoragePricing.Days != 0)
            {
                if (calculatedPricingItem.ChargeableDays > shipmentStoragePricing.Days)
                {
                    calculatedPricingItem.ChargeableDays = shipmentStoragePricing.Days;
                }
            }

            calculatedPricingItem.Amount = MethodHelper.Round((shipmentStoragePricing.SalePrice * Convert.ToDecimal(weight) * calculatedPricingItem.ChargeableDays), 2);

            return calculatedPricingItem;
        }
        private double? ComputeStorageWeight()
        {
            double? weightRounded;
            double? weight = this.GetWeight();
            double? rounding = this.GetRounding();

            if (weight != null && weight != 0 && rounding != null && rounding != 0)
            {
                string[] weightParts = weight.ToString().Split('.');

                if (weightParts.Count() > 1)
                {
                    string stringFormattedDecimalDigits = "0." + weightParts[1];
                    double decimalDigits = Convert.ToDouble(stringFormattedDecimalDigits);
                    int integerDigits = Convert.ToInt32(weightParts[0]);

                    if (rounding == 0.5)
                    {
                        if (decimalDigits <= 0.5)
                        {
                            weightRounded = integerDigits + 0.5;
                        }

                        else
                        {
                            weightRounded = integerDigits + 1;
                        }
                    }

                    else
                    {
                        weightRounded = integerDigits + 1;
                    }
                }

                else
                {
                    weightRounded = weight;
                }
            }

            else
            {
                weightRounded = weight;
            }

            return weightRounded;
        }
        private double? GetRounding()
        {
            double? rounding = 0;

            if (shipmentPM.WeightRoundingCode == "HAF")
            {
                rounding = 0.5;
            }

            else if (shipmentPM.WeightRoundingCode == "ONE")
            {
                rounding = 1;
            }

            return rounding;
        }
        private double? GetWeight()
        {
            double? weight = 0;

            if (shipmentPM.WeightMeasurementCode == "GRWT")
            {
                weight = shipmentPM.GrossWeight;
            }

            else
            {
                weight = shipmentPM.ChargeableWeight;
            }

            return weight;
        }

        private void UpdateShipmentStoragePricingLine(List<CalculatedPricingItem> myPricigs)
        {
            foreach (CalculatedPricingItem item in myPricigs)
            {
                ShipmentStoragePricingPM shipmentPricing = shipmentPM.ShipmentStoragePricings.Where(d => d.LineNumber == item.LineNumber).FirstOrDefault();
                if (shipmentPricing != null)
                {
                    shipmentPricing.Amount = item.Amount;
                    shipmentPricing.ChargeableDays = item.ChargeableDays;
                    shipmentPricing.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                }
            }
        }
        public List<LastRate> GetAllRates(string baseCurrencyId, DateTime date)
        {
            List<LastRate> myResult = new List<LastRate>();

            if (!string.IsNullOrEmpty(baseCurrencyId))
            {
                IWebFreightContext objectContext = WebFreightContext.GetContext(shipmentPM.Tenant);

                RatesTableRepository myRepository = new RatesTableRepository(objectContext);
                RatesTableQuery myQuery = new RatesTableQuery(myRepository);
                CurrencyRepository currencyRepository = new CurrencyRepository(shipmentPM.Tenant);
                Currency baseCurrency = currencyRepository.GetCurrencies(shipmentPM.Tenant).Where(r => r.Id == baseCurrencyId).FirstOrDefault();
                List<Currency> foreignCurrencies = currencyRepository.GetCurrencies(shipmentPM.Tenant).Where(c => c.Id != baseCurrencyId).ToList();

                foreach (Currency currency in foreignCurrencies)
                {
                    LastRate lastRate = myQuery.GetLastRecordByValueDate(shipmentPM.Tenant, currency.Id, baseCurrencyId, date);
                    if (lastRate != null)
                    {
                        lastRate.BaseCurrencyId = baseCurrencyId;
                        lastRate.BaseCurrencyCode = baseCurrency.Code;
                        myResult.Add(lastRate);
                    }

                    else
                    {
                        LastRate newLastRate = new LastRate()
                        {
                            Id = IdCounter.GetNumber("LastRate", shipmentPM.Tenant).ToString(),
                            Tenant = shipmentPM.Tenant,
                            ForeignCurrencyId = currency.Id,
                            ForeignCurrencyCode = currency.Code,
                            ForeignCurrencyName = currency.EnglishName,
                            BaseCurrencyId = baseCurrency.Id,
                            BaseCurrencyCode = baseCurrency.Code,
                            HistoryCount = 0,
                            Rate = null,
                        };

                        myResult.Add(newLastRate);
                    }
                }
            }

            return myResult;
        }
    }

    public class CalculatedPricingItem
    {
        public int? LineNumber { get; set; }
        public int? To { get; set; }
        public decimal? Price { get; set; }
        public decimal? Amount { get; set; }
        public int? ChargeableDays { get; set; }
    }
}
