using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.Behaviours;
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
        private StorageCalculationManager storageCalculationManager;
        private int tenant;

        public bool ReceivablePricingUpdated { get; set; }
        public UpdateCrossDockBehaviour(ShipmentPM shipmentPM)
        {
            this.shipmentPM = shipmentPM;
            this.tenant = shipmentPM.Tenant;

            warehouseContext = WarehouseContext.GetContext(tenant);
            warehouseEntryRepository = new WarehouseEntryRepository(warehouseContext);
            warehouseReleaseRepository = new WarehouseReleaseRepository(warehouseContext);
            warehouseEntries = warehouseEntryRepository.GetWarehouseEntriesByshipmentId(shipmentPM.Id, tenant);
            warehouseRelases = warehouseReleaseRepository.GetWarehouseReleasesByshipmentId(shipmentPM.Id, tenant);

            storageCalculationManager = new StorageCalculationManager(this.shipmentPM);
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

            if (isUpdated && shipmentPM.IsBondedWarehouse)
            {
                storageCalculationManager.CheckStorageProperties();
                ReceivablePricingUpdated = true;
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
                    isUpdated = true;
                }
            }
            else if (warehouseRelases.Count() != 0)
            {
                shipmentPM.WarehouseLegActualReleaseDate = null;
                shipmentPM.WarehouseLegExpectedReleaseDate = null;
                isUpdated = true;
            }

            if (isUpdated && shipmentPM.IsBondedWarehouse)
            {
                storageCalculationManager.CheckStorageProperties();
                ReceivablePricingUpdated = true;
            }
        }
        private void UpdateDeliveryDepartureDates()
        {
            IQueryable<WarehouseRelease> activeDeliveryWarehouseRelases = warehouseRelases.Where(r => r.StatusCode != "CARE" && r.ConnectedTo == "Delivery");
            ShipmentPickUpDeliveryRepository shipmentPickUpDeliveryRepository = new ShipmentPickUpDeliveryRepository(tenant);
            List<ShipmentPickUpDelivery> shipmentDeliveries = shipmentPickUpDeliveryRepository.GetShipmentDeliveryByShipmentId(shipmentPM.Id, tenant);

            IQueryable<WarehouseRelease> deliveryWarehouseReleases = null;
            shipmentDeliveries.ForEach(delivery => {
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

    public class StorageCalculationManager
    {
        private ShipmentPM shipmentPM;
        private int? storageDays;
        public StorageCalculationManager(ShipmentPM shipmentPM)
        {
            this.shipmentPM = shipmentPM;
        }

        private void ComputeStorageDaye()
        {
            if (shipmentPM.WarehouseLegActualEntryDate != null && shipmentPM.WarehouseLegActualReleaseDate != null)
            {
                if (shipmentPM.WarehouseLegActualReleaseDate >= shipmentPM.WarehouseLegActualEntryDate)
                {
                    storageDays = (shipmentPM.WarehouseLegActualReleaseDate - shipmentPM.WarehouseLegActualEntryDate).Value.Days;
                }
            }
        }
        public void CheckStorageProperties()
        {
            ShipmentReceivablePM storageReceivable = shipmentPM.ShipmentReceivables.Where(d => d.ChargesTypeCode == "ISTOR" && d.MeasurementCode == "STFE" && string.IsNullOrEmpty(d.ARInvoiceId)).FirstOrDefault();

            this.ComputeStorageDaye();

            if (shipmentPM.WarehouseLegActualEntryDate != null && shipmentPM.WarehouseLegActualReleaseDate != null && !string.IsNullOrEmpty(shipmentPM.ChargeStorageCurrencyId)
                && shipmentPM.ChargeStorage && shipmentPM.ShipmentStoragePricings.Count > 0 && storageDays != null)
            {
                if (storageReceivable != null)
                {
                    this.UpdateStorageReceivable(storageReceivable);
                }

                else
                {
                    this.CreateReceivable();
                }
            }

            else
            {
                if (storageReceivable != null)
                {
                    storageReceivable.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
                }
            }

            //this.ComputeStorageFee();
        }
        private double? ComputeReceivableAmount()
        {
            double? amount = 0;
            int? allChargeableDays = storageDays - shipmentPM.WarehouseStorageFreeDays;
            double? weight = this.ComputeStorageWeight();

            List<CalculatedPricingItem> myPricigs = new List<CalculatedPricingItem>();
            if (weight != null && weight != 0)
            {
                foreach (ShipmentStoragePricingPM item in shipmentPM.ShipmentStoragePricings.OrderBy(d => d.LineNumber))
                {
                    CalculatedPricingItem newItem = new CalculatedPricingItem();
                    newItem.LineNumber = item.LineNumber;
                    newItem.Price = item.SalePrice;

                    newItem.ChargeableDays = allChargeableDays - myPricigs.Sum(s => s.ChargeableDays);

                    if (item.Days != null && item.Days != 0)
                    {
                        if (newItem.ChargeableDays > item.Days)
                        {
                            newItem.ChargeableDays = item.Days;
                        }
                    }

                    newItem.Amount = MethodHelper.Round((item.SalePrice * Convert.ToDecimal(weight) * newItem.ChargeableDays), 2);
                    myPricigs.Add(newItem);
                }

                this.UpdateShipmentStoragePricingLine(myPricigs);
                amount = Convert.ToDouble(myPricigs.Sum(s => s.Amount));
            }

            double? myResult = amount;
            ShipmentReceivablePM invoiceStorageReceivable = shipmentPM.ShipmentReceivables.Where(d => d.ChargesTypeCode == "ISTOR" && d.MeasurementCode == "STFE" && !string.IsNullOrEmpty(d.ARInvoiceId)).FirstOrDefault();
            if (invoiceStorageReceivable != null)
            {
                myResult = amount - invoiceStorageReceivable.TotalAmount;
            }

            return myResult;
        }
        private void UpdateStorageReceivable(ShipmentReceivablePM storageReceivable)
        {
            double? amount = this.ComputeReceivableAmount();

            storageReceivable.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            storageReceivable.TotalAmount = amount;
            storageReceivable.TotalAmountLocal = MethodHelper.Round(storageReceivable.TotalAmount * storageReceivable.Rate, 2);

            if (storageReceivable.CurrencyId == shipmentPM.ProfitCurrencyId)
            {
                storageReceivable.AmountInProfitCurrency = storageReceivable.TotalAmount;
            }

            else
            {
                storageReceivable.AmountInProfitCurrency = (storageReceivable.TotalAmountLocal / storageReceivable.ProfitCurrencyExchangeRate);
            }
        }
        private void CreateReceivable()
        {
            double? amount = this.ComputeReceivableAmount();

            if (amount != null && amount != 0)
            {
                ChargesTypeRepository chargesTypeRepository = new ChargesTypeRepository(shipmentPM.Tenant);
                ChargesType chargesType = chargesTypeRepository.GetSingleChargesTypeByCode("ISTOR", shipmentPM.Tenant);

                if (chargesType != null)
                {
                    string localCurrencyId = null;
                    string loggedUserId = null;

                    TenantRepository tenantRepository = new TenantRepository(shipmentPM.Tenant);
                    Tenant myTenant = tenantRepository.GetSingleTenant(shipmentPM.Tenant);
                    if (myTenant != null)
                    {
                        localCurrencyId = myTenant.CurrencyId;
                    }

                    string email = HttpContext.Current.User.Identity.Name;
                    ContactRepository contactRepository = new ContactRepository(shipmentPM.Tenant);
                    Contact loggedContact = contactRepository.GetSingleContactByEmail(email, shipmentPM.Tenant);
                    if (loggedContact != null)
                    {
                        loggedUserId = loggedContact.Id;
                    }

                    DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(shipmentPM.Tenant);

                    List<LastRate> allRates = this.GetAllRates(localCurrencyId, todayDate);

                    ShipmentReceivablePM storageReceivable = new ShipmentReceivablePM();
                    storageReceivable.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                    storageReceivable.Tenant = shipmentPM.Tenant;
                    storageReceivable.ShipmentId = shipmentPM.Id;
                    storageReceivable.ChargesTypeId = chargesType.Id;
                    storageReceivable.ChargesTypeCode = chargesType.Code;
                    storageReceivable.ChargesTypeName = chargesType.EnglishName;
                    storageReceivable.MeasurementId = chargesType.MeasurementId;
                    storageReceivable.ChargesGroupCode = chargesType.ChargesGroupCode;
                    storageReceivable.DueTypeCode = chargesType.DueTypeCode;
                    storageReceivable.VatTypeId = chargesType.VatTypeId;
                    storageReceivable.IATACodeId = chargesType.IATACodeId;
                    storageReceivable.IsExpense = chargesType.IsExpense;
                    storageReceivable.ShipmentNumber = shipmentPM.ShipmentNumber;
                    storageReceivable.CreateDate = todayDate;
                    storageReceivable.UpdateDate = todayDate;
                    storageReceivable.CreatedByUserId = loggedUserId;
                    storageReceivable.UpdateByUserId = loggedUserId;
                    storageReceivable.ShipmentReceivableLineStatusCode = "OAMT";
                    storageReceivable.CurrencyId = shipmentPM.ChargeStorageCurrencyId;

                    if (localCurrencyId == storageReceivable.CurrencyId)
                    {
                        storageReceivable.Rate = 1;
                    }
                    else
                    {
                        LastRate lastRate = allRates.Where(d => d.ForeignCurrencyId == storageReceivable.CurrencyId).FirstOrDefault();
                        if (lastRate != null)
                        {
                            storageReceivable.Rate = lastRate.Rate;
                        }
                    }

                    if (shipmentPM.ProfitCurrencyId == localCurrencyId)
                    {
                        storageReceivable.ProfitCurrencyExchangeRate = 1;
                    }

                    else
                    {
                        LastRate myLastRate = allRates.Where(d => d.ForeignCurrencyId == shipmentPM.ProfitCurrencyId).FirstOrDefault();
                        if (myLastRate != null)
                        {
                            storageReceivable.ProfitCurrencyExchangeRate = myLastRate.Rate;
                        }
                    }

                    if (chargesType.ChargesGroupCode == "FRT")
                    {
                        storageReceivable.PrepaidCollectId = shipmentPM.FreightPrepaidCollectId;
                    }

                    else
                    {
                        storageReceivable.PrepaidCollectId = shipmentPM.OtherPrepaidCollectId;
                    }

                    storageReceivable.TotalAmount = amount;
                    storageReceivable.TotalAmountLocal = MethodHelper.Round(storageReceivable.TotalAmount * storageReceivable.Rate, 2);

                    if (storageReceivable.CurrencyId == shipmentPM.ProfitCurrencyId)
                    {
                        storageReceivable.AmountInProfitCurrency = storageReceivable.TotalAmount;
                    }

                    else
                    {
                        storageReceivable.AmountInProfitCurrency = (storageReceivable.TotalAmountLocal / storageReceivable.ProfitCurrencyExchangeRate);
                    }

                    shipmentPM.ShipmentReceivables.Add(storageReceivable);
                }
            }
        }
        private double? ComputeStorageWeight()
        {
            double? weightRounded = 0;
            double? weight = 0;
            double? rounding = 0;

            if (shipmentPM.WeightMeasurementCode == "GRWT")
            {
                weight = shipmentPM.GrossWeight;
            }

            else
            {
                weight = shipmentPM.ChargeableWeight;
            }

            if (shipmentPM.WeightRoundingCode == "HAF")
            {
                rounding = 0.5;
            }

            else if (shipmentPM.WeightRoundingCode == "ONE")
            {
                rounding = 1;
            }

            if (weight != null && weight != 0 && rounding != null && rounding != 0)
            {
                string toString = weight.ToString();
                string[] r = toString.Split('.');

                if (r.Count() > 1)
                {
                    string strDigits = "0." + r[1];
                    double digits = Convert.ToDouble(strDigits);
                    int integer = Convert.ToInt32(r[0]);

                    if (rounding == 0.5)
                    {
                        if (digits <= 0.5)
                        {
                            weightRounded = integer + 0.5;
                        }

                        else
                        {
                            weightRounded = integer + 1;
                        }
                    }

                    else
                    {
                        weightRounded = integer + 1;
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
