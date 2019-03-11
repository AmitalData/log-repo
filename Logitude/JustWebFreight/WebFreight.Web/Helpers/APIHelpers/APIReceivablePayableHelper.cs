using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.APIHelpers
{
    public class APIReceivablePayableHelper
    {
        private int tenant;
        private ShipmentPM shipmentPM;
        private Tenant tenantEntity;
        private TenantRepository tenantRepository;
        private ChargesTypeRepository chargesTypeRepository;
        private MeasurementRepository measurementRepository;
        RatesTableQuery ratesTableQuery;
        public APIReceivablePayableHelper(ShipmentPM shipment, int tenant)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);

            tenantRepository = new TenantRepository(commonContext);
            chargesTypeRepository = new ChargesTypeRepository(commonContext);
            measurementRepository = new MeasurementRepository(commonContext);
            ratesTableQuery = new RatesTableQuery(tenant);

            this.shipmentPM = shipment;
            this.tenant = tenant;
            this.tenantEntity = tenantRepository.GetSingleTenant(tenant);
        }

        public void ValidateReceivablesAndPayables()
        {
            this.ValidateReceivables();
            this.ValidatePayables();
        }

        private void ValidateReceivables()
        {
            foreach (ShipmentReceivablePM item in shipmentPM.ShipmentReceivables)
            {
                item.CreatedByUserId = shipmentPM.CreatedByUserId;
                item.UpdateByUserId = shipmentPM.UpdatedByUserId;
                item.ShipmentReceivableLineStatusCode = "EMPT";

                if (!string.IsNullOrEmpty(item.ChargesTypeId))
                {
                    Simplog.Data.CommonDataModel.EntityPOCOs.ChargesType chargesType = chargesTypeRepository.GetSingleChargesType(item.ChargesTypeId, tenant);
                    if (chargesType != null)
                    {
                        this.ValidateChargesType(chargesType, "R");

                        item.DueTypeCode = chargesType.DueTypeCode;
                        item.VatTypeId = chargesType.VatTypeId;
                        item.IATACodeId = chargesType.IATACodeId;
                        item.IsExpense = chargesType.IsExpense;

                        if (string.IsNullOrEmpty(item.MeasurementId))
                        {
                            if (MethodHelper.IsLCLEntity(shipmentPM.TransportModeId, shipmentPM.ShipmentTypeId))
                            {
                                if (!string.IsNullOrEmpty(chargesType.MeasurementId))
                                {
                                    item.MeasurementId = chargesType.MeasurementId;
                                }
                                else
                                {
                                    throw new ApplicationException("Receivable Measurement is required");
                                }
                            }

                            else
                            {
                                if (!string.IsNullOrEmpty(chargesType.ContainerMeasurementId))
                                {
                                    item.MeasurementId = chargesType.ContainerMeasurementId;
                                }
                                else if (!string.IsNullOrEmpty(chargesType.MeasurementId))
                                {
                                    item.MeasurementId = chargesType.MeasurementId;
                                }
                                else
                                {
                                    throw new ApplicationException("Receivable Measurement is required");
                                }
                            }
                        }

                        if (string.IsNullOrEmpty(item.PrepaidCollectId))
                        {
                            if (chargesType.ChargesGroupCode == "FRT")
                            {
                                item.PrepaidCollectId = shipmentPM.FreightPrepaidCollectId;
                            }

                            else
                            {
                                item.PrepaidCollectId = shipmentPM.OtherPrepaidCollectId;
                            }
                        }

                        if (string.IsNullOrEmpty(item.CurrencyId))
                        {
                            if (chargesType.ChargesGroupCode == "FRT" || chargesType.ChargesGroupCode == "SCH")
                            {
                                item.CurrencyId = tenantEntity.FreightCurrencyId;
                            }

                            else
                            {
                                item.CurrencyId = tenantEntity.OtherChargesCurrencyId;
                            }
                        }

                        if (string.IsNullOrEmpty(item.MeasurementId))
                        {
                            if (MethodHelper.IsLCLEntity(shipmentPM.TransportModeId, shipmentPM.ShipmentTypeId))
                            {
                                item.MeasurementId = chargesType.MeasurementId;
                            }

                            else
                            {
                                item.MeasurementId = chargesType.ContainerMeasurementId != null ? chargesType.ContainerMeasurementId : chargesType.MeasurementId;
                            }
                        }
                    }
                }

                
                if (item.Rate == null)
                {
                    if (tenantEntity.CurrencyId == item.CurrencyId)
                    {
                        item.Rate = 1;
                    }

                    else
                    {
                        if (!string.IsNullOrEmpty(tenantEntity.CurrencyId) && !string.IsNullOrEmpty(item.CurrencyId))
                        {
                            LastRate lastRate = ratesTableQuery.GetLastRecordByValueDate(tenant, item.CurrencyId, tenantEntity.CurrencyId, TenantServerConfigration.GetCurrentDateTime(tenant).Date);

                            if (lastRate != null && lastRate.Rate != 0)
                            {
                                item.Rate = lastRate.Rate;
                            }
                        }
                    }
                }

                if (item.ProfitCurrencyExchangeRate == null)
                {
                    if (tenantEntity.CurrencyId == shipmentPM.ProfitCurrencyId)
                    {
                        item.ProfitCurrencyExchangeRate = 1;
                    }

                    else
                    {
                        LastRate lastRate = ratesTableQuery.GetLastRecordByValueDate(tenant, shipmentPM.ProfitCurrencyId, tenantEntity.CurrencyId, TenantServerConfigration.GetCurrentDateTime(tenant).Date);
                        if (lastRate != null)
                        {
                            item.ProfitCurrencyExchangeRate = lastRate.Rate;
                        }
                    }
                }


                string measurementCode = "";
                Simplog.Data.CommonDataModel.EntityPOCOs.Measurement measurement = measurementRepository.GetSingleMeasurement(item.MeasurementId, tenant);
                if (measurement != null)
                {
                    measurementCode = measurement.Code;
                }

                if (shipmentPM.ShipmentLevelCode != "C")
                {
                    if (measurementCode == "BCNT")
                    {
                        List<ShipmentPackagePM> packages = shipmentPM.ShipmentPackages.Where(d => d.IsContainer).ToList();

                        if (packages.Count() == 0)
                        {
                            throw new ApplicationException("This shipment doesn't contain any containers");
                        }
                    }
                }

                if (item.Quantity == null)
                {
                    switch (measurementCode)
                    {
                        case "GRWT": { item.Quantity = shipmentPM.GrossWeight; break; }
                        case "CHWT": { item.Quantity = shipmentPM.ChargeableWeight; break; }
                        case "VOLU": { item.Quantity = shipmentPM.Volume; break; }
                        case "BTEU": { item.Quantity = shipmentPM.TEU; break; }
                        case "FIXD": { item.Quantity = 1; break; }
                        case "GWTN": { item.Quantity = shipmentPM.GrossWeightPerTon; break; }
                        case "QTY": { item.Quantity = MethodHelper.IsLCLEntity(shipmentPM.TransportModeId, shipmentPM.ShipmentTypeId) ? shipmentPM.NumberOfPackages : shipmentPM.NumberOfContainers; break; }

                        case "PRVL":
                            {
                                item.Quantity = shipmentPM.ValueOfGoods;
                                break;
                            }

                        case "PRFR":
                            {
                                item.Quantity = shipmentPM.ShipmentReceivables.Where(d => d.ChargesGroupCode == "FRT").Sum(s => s.TotalAmount);
                                break;
                            }
                    }
                }

                if (item.Quantity != null && item.UnitPrice != null)
                {
                    if (item.ShipmentReceivableLineStatusCode != "OAMT")
                    {
                        item.ShipmentReceivableLineStatusCode = "OAMT";
                    }
                }

                else
                {
                    if (item.ShipmentReceivableLineStatusCode != "EMPT")
                    {
                        item.ShipmentReceivableLineStatusCode = "EMPT";
                    }
                }

                if (item.TotalAmount == null)
                {
                    double? iAmount = null;
                    if (item.Quantity != null && item.UnitPrice != null)
                    {
                        if (measurementCode == "PRVL" || measurementCode == "PRFR")
                        {
                            double? price = item.UnitPrice / 100;
                            iAmount = item.Quantity * price;
                        }

                        else
                        {
                            iAmount = item.Quantity * item.UnitPrice;
                        }
                    }

                    /* MinMax */
                    if (iAmount != null)
                    {
                        if (item.QuoteSaleMinAmount != null)
                        {
                            if (iAmount < item.QuoteSaleMinAmount)
                            {
                                iAmount = item.QuoteSaleMinAmount;
                            }
                        }

                        if (item.QuoteSaleMaxAmount != null)
                        {
                            if (iAmount > item.QuoteSaleMaxAmount)
                            {
                                iAmount = item.QuoteSaleMaxAmount;
                            }
                        }
                    }

                    if (iAmount != null)
                    {
                        item.TotalAmount = ComputeHelper.Round(iAmount.Value, 2);
                    }
                }

                else
                {
                    double? price = null;
                    if (item.Quantity != null)
                    {
                        if (item.Quantity == 0)
                        {
                            price = 0;
                        }

                        else
                        {
                            price = item.TotalAmount / item.Quantity;
                        }
                    }

                    if (price != null)
                    {
                        item.UnitPrice = ComputeHelper.Round(price.Value, 3);
                    }
                }

                if (item.TotalAmount != null && item.Rate != null)
                {
                    item.TotalAmountLocal = ComputeHelper.Round(item.TotalAmount.Value * item.Rate.Value, 2);
                }

                if (item.CurrencyId == shipmentPM.ProfitCurrencyId)
                {
                    item.AmountInProfitCurrency = item.TotalAmount;
                }

                else
                {
                    item.AmountInProfitCurrency = (item.TotalAmountLocal / item.ProfitCurrencyExchangeRate);
                }
            }
        }
        private void ValidatePayables()
        {
            foreach (ShipmentPayablePM item in shipmentPM.ShipmentPayables)
            {
                item.CreatedByUserId = shipmentPM.CreatedByUserId;
                item.UpdateByUserId = shipmentPM.UpdatedByUserId;
                item.ShipmentPayableLineStatusCode = "EMPT";
                item.ShipmentPayableAmountTypeCode = "ACCU";
                item.ShipmentPayableAmountTypeName = "Accrual";

                if (!string.IsNullOrEmpty(item.ChargesTypeId))
                {
                    Simplog.Data.CommonDataModel.EntityPOCOs.ChargesType chargesType = chargesTypeRepository.GetSingleChargesType(item.ChargesTypeId, tenant);
                    if (chargesType != null)
                    {
                        this.ValidateChargesType(chargesType, "P");

                        item.DueTypeCode = chargesType.DueTypeCode;
                        item.VatTypeId = chargesType.VatTypeId;
                        item.IATACodeId = chargesType.IATACodeId;

                        if (string.IsNullOrEmpty(item.MeasurementId))
                        {
                            if (MethodHelper.IsLCLEntity(shipmentPM.TransportModeId, shipmentPM.ShipmentTypeId))
                            {
                                if (!string.IsNullOrEmpty(chargesType.MeasurementId))
                                {
                                    item.MeasurementId = chargesType.MeasurementId;
                                }
                                else
                                {
                                    throw new ApplicationException("Payable Measurement is required");
                                }
                            }

                            else
                            {
                                if (!string.IsNullOrEmpty(chargesType.ContainerMeasurementId))
                                {
                                    item.MeasurementId = chargesType.ContainerMeasurementId;
                                }
                                else if (!string.IsNullOrEmpty(chargesType.MeasurementId))
                                {
                                    item.MeasurementId = chargesType.MeasurementId;
                                }
                                else
                                {
                                    throw new ApplicationException("Payable Measurement is required");
                                }
                            }
                        }

                        if (string.IsNullOrEmpty(item.PrepaidCollectId))
                        {
                            if (chargesType.ChargesGroupCode == "FRT")
                            {
                                item.PrepaidCollectId = shipmentPM.FreightPrepaidCollectId;
                            }

                            else
                            {
                                item.PrepaidCollectId = shipmentPM.OtherPrepaidCollectId;
                            }
                        }

                        if (string.IsNullOrEmpty(item.CurrencyId))
                        {
                            if (chargesType.ChargesGroupCode == "FRT" || chargesType.ChargesGroupCode == "SCH")
                            {
                                item.CurrencyId = tenantEntity.FreightCurrencyId;
                            }

                            else
                            {
                                item.CurrencyId = tenantEntity.OtherChargesCurrencyId;
                            }
                        }

                        if (string.IsNullOrEmpty(item.MeasurementId))
                        {
                            if (MethodHelper.IsLCLEntity(shipmentPM.TransportModeId, shipmentPM.ShipmentTypeId))
                            {
                                item.MeasurementId = chargesType.MeasurementId;
                            }

                            else
                            {
                                item.MeasurementId = chargesType.ContainerMeasurementId != null ? chargesType.ContainerMeasurementId : chargesType.MeasurementId;
                            }
                        }
                    }
                }

                RatesTableQuery ratesTableQuery = new RatesTableQuery(tenant);
                if (item.Rate == null)
                {
                    if (tenantEntity.CurrencyId == item.CurrencyId)
                    {
                        item.Rate = 1;
                    }

                    else
                    {
                        if (!string.IsNullOrEmpty(tenantEntity.CurrencyId) && !string.IsNullOrEmpty(item.CurrencyId))
                        {
                            LastRate lastRate = ratesTableQuery.GetLastRecordByValueDate(tenant, item.CurrencyId, tenantEntity.CurrencyId, TenantServerConfigration.GetCurrentDateTime(tenant).Date);

                            if (lastRate != null && lastRate.Rate != 0)
                            {
                                item.Rate = lastRate.Rate;
                            }
                        }
                    }
                }

                if (item.ProfitCurrencyExchangeRate == null)
                {
                    if (tenantEntity.CurrencyId == shipmentPM.ProfitCurrencyId)
                    {
                        item.ProfitCurrencyExchangeRate = 1;
                    }

                    else
                    {
                        LastRate lastRate = ratesTableQuery.GetLastRecordByValueDate(tenant, shipmentPM.ProfitCurrencyId, tenantEntity.CurrencyId, TenantServerConfigration.GetCurrentDateTime(tenant).Date);
                        if (lastRate != null)
                        {
                            item.ProfitCurrencyExchangeRate = lastRate.Rate;
                        }
                    }
                }


                string measurementCode = "";
                Simplog.Data.CommonDataModel.EntityPOCOs.Measurement measurement = measurementRepository.GetSingleMeasurement(item.MeasurementId, tenant);
                if (measurement != null)
                {
                    measurementCode = measurement.Code;
                }

                if (item.Quantity == null)
                {
                    switch (measurementCode)
                    {
                        case "GRWT": { item.Quantity = shipmentPM.GrossWeight; break; }
                        case "CHWT": { item.Quantity = shipmentPM.ChargeableWeight; break; }
                        case "VOLU": { item.Quantity = shipmentPM.Volume; break; }
                        case "BTEU": { item.Quantity = shipmentPM.TEU; break; }
                        case "FIXD": { item.Quantity = 1; break; }
                        case "GWTN": { item.Quantity = shipmentPM.GrossWeightPerTon; break; }
                        case "QTY": { item.Quantity = MethodHelper.IsLCLEntity(shipmentPM.TransportModeId, shipmentPM.ShipmentTypeId) ? shipmentPM.NumberOfPackages : shipmentPM.NumberOfContainers; break; }

                        case "PRVL":
                            {
                                item.Quantity = shipmentPM.ValueOfGoods;
                                break;
                            }

                        case "PRFR":
                            {
                                item.Quantity = shipmentPM.ShipmentReceivables.Where(d => d.ChargesGroupCode == "FRT").Sum(s => s.TotalAmount);
                                break;
                            }
                    }
                }

                string StatusCode = "EMPT";
                if (item.ShipmentPayableAmountTypeCode == "NEXP")
                {
                    StatusCode = "ACCT";
                }

                else if (item.Quantity == null || item.UnitPrice == null)
                {
                    StatusCode = "EMPT";
                }

                else
                {
                    if (item.OpenAmount == null)
                    {
                        item.OpenAmount = 0;
                    }

                    if (item.AccountedAmount == null)
                    {
                        item.AccountedAmount = 0;
                    }

                    if (item.OpenAmount != 0 && item.AccountedAmount != 0)
                    {
                        StatusCode = "PACC";
                    }

                    else if (item.OpenAmount != 0)
                    {
                        StatusCode = "OAMT";
                    }

                    else if (item.AccountedAmount != 0)
                    {
                        StatusCode = "ACCT";
                    }
                }

                if (StatusCode == "EMPT")
                {
                    if (item.Quantity != null && item.UnitPrice != null)
                    {
                        StatusCode = "OAMT";
                    }
                }

                item.ShipmentPayableLineStatusCode = StatusCode;

                if (item.ExpectedAmount == null)
                {
                    double? iAmount = null;
                    if (item.Quantity != null && item.UnitPrice != null)
                    {
                        if (measurementCode == "PRVL" || measurementCode == "PRFR")
                        {
                            double? price = item.UnitPrice / 100;
                            iAmount = item.Quantity * price;
                        }

                        else
                        {
                            iAmount = item.Quantity * item.UnitPrice;
                        }
                    }

                    if (iAmount != null)
                    {
                        item.ExpectedAmount = ComputeHelper.Round(iAmount.Value, 2);
                    }
                }

                else
                {
                    double? price = null;
                    if (item.Quantity != null)
                    {
                        if (item.Quantity == 0)
                        {
                            price = 0;
                        }

                        else
                        {
                            price = item.ExpectedAmount / item.Quantity;
                        }
                    }

                    if (price != null)
                    {
                        item.UnitPrice = ComputeHelper.Round(price.Value, 3);
                    }
                }

                if (item.ExpectedAmount != null && item.Rate != null)
                {
                    item.ExpectedAmountLocal = ComputeHelper.Round(item.ExpectedAmount.Value * item.Rate.Value, 2);
                }

                if (item.CurrencyId == shipmentPM.ProfitCurrencyId)
                {
                    item.ExpectedAmountInProfitCurrency = item.ExpectedAmount;
                }

                else
                {
                    item.ExpectedAmountInProfitCurrency = (item.ExpectedAmountLocal / item.ProfitCurrencyExchangeRate);
                }

                if (item.ShipmentPayableLineStatusCode == "EMPT" || item.ShipmentPayableLineStatusCode == "OAMT")
                {
                    item.OpenAmount = item.ExpectedAmount;
                    item.OpenAmountInLocalCurrency = item.ExpectedAmountLocal;
                    item.OpenAmountInProfitCurrency = item.ExpectedAmountInProfitCurrency;
                }
            }
        }
        private void ValidateChargesType(ChargesType chargesType, string type)
        {
            switch (type)
            {
                case "R":
                    {
                        if (!chargesType.IsReceivable)
                        {
                            throw new ApplicationException("Charge type " + chargesType.Code + " used in receivables should be marked as Receivable");
                        }
                        break;
                    }

                case "P":
                    {
                        if (!chargesType.IsPayable)
                        {
                            throw new ApplicationException("Charge type " + chargesType.Code + " used in payables should be marked as Payable");
                        }
                        break;
                    }
            }

            switch (shipmentPM.TransportModeId)
            {
                case "A":
                    {
                        if (!chargesType.IsAir)
                        {
                            throw new ApplicationException("Charge type " + chargesType.Code + " used in Air shipments should be marked as Air");
                        }
                        break;
                    }

                case "I":
                    {
                        if (!chargesType.IsInland)
                        {
                            throw new ApplicationException("Charge type " + chargesType.Code + " used in Inland shipments should be marked as Inland");
                        }
                        break;
                    }

                case "O":
                    {
                        if (!chargesType.IsOcean)
                        {
                            throw new ApplicationException("Charge type " + chargesType.Code + " used in Ocean shipments should be marked as Ocean");
                        }
                        break;
                    }
            }

            switch(shipmentPM.DirectionId)
            {
                case "E":
                    {
                        if (!chargesType.IsExport)
                        {
                            throw new ApplicationException("Charge type " + chargesType.Code + " used in Export shipments should be marked as Export");
                        }
                        break;
                    }

                case "I":
                    {
                        if (!chargesType.IsImport)
                        {
                            throw new ApplicationException("Charge type " + chargesType.Code + " used in Import shipments should be marked as Import");
                        }
                        break;
                    }

                case "D":
                    {
                        if (!chargesType.IsDomestic)
                        {
                            throw new ApplicationException("Charge type " + chargesType.Code + " used in Domestic shipments should be marked as Domestic");
                        }
                        break;
                    }

                case "R":
                    {
                        if (!chargesType.IsDrop)
                        {
                            throw new ApplicationException("Charge type " + chargesType.Code + " used in Drop shipments should be marked as Drop");
                        }
                        break;
                    }
            }
        }

        public void ComputeReceivablesPayablesTotals()
        {
            // Payables
            double? openPayablesLocal = null;
            double? openPayablesProfit = null;
            if (shipmentPM.ShipmentLevelCode == "C" && shipmentPM.ShipmentConsoleShipments.Count > 0)
            {
                openPayablesLocal = shipmentPM.ShipmentConsoleShipments.Sum(s => s.OAMTPayables_Local);
                openPayablesProfit = shipmentPM.ShipmentConsoleShipments.Sum(s => s.OAMTPayables_Profit);
            }

            else
            {
                openPayablesLocal = shipmentPM.ShipmentPayables.Sum(s => s.OpenAmountInLocalCurrency);
                openPayablesProfit = shipmentPM.ShipmentPayables.Sum(s => s.OpenAmountInProfitCurrency);
            }

            // Receivables
            double? openReceivablesLocal = null;
            double? openReceivablesProfit = null;
            if (shipmentPM.ShipmentLevelCode == "C" && shipmentPM.ShipmentConsoleShipments.Count > 0)
            {
                if (shipmentPM.ProrateReceivables)
                {
                    openReceivablesLocal = shipmentPM.ShipmentConsoleShipments.Sum(s => s.OAMTReceivables_Local);
                    openReceivablesProfit = shipmentPM.ShipmentConsoleShipments.Sum(s => s.OAMTReceivables_Profit);
                }

                else
                {
                    openReceivablesLocal = shipmentPM.ShipmentReceivables.Where(f => f.ShipmentReceivableLineStatusCode != "ACCT").Sum(s => s.TotalAmountLocal);
                    openReceivablesProfit = shipmentPM.ShipmentReceivables.Where(f => f.ShipmentReceivableLineStatusCode != "ACCT").Sum(s => s.AmountInProfitCurrency);

                    openReceivablesLocal += shipmentPM.ShipmentConsoleShipments.Sum(s => s.OAMTReceivables_Local_NoParent);
                    openReceivablesProfit += shipmentPM.ShipmentConsoleShipments.Sum(s => s.OAMTReceivables_Profit_NoParent);
                }
            }

            else
            {
                openReceivablesLocal = shipmentPM.ShipmentReceivables.Where(f => f.ShipmentReceivableLineStatusCode != "ACCT").Sum(s => s.TotalAmountLocal);
                openReceivablesProfit = shipmentPM.ShipmentReceivables.Where(f => f.ShipmentReceivableLineStatusCode != "ACCT").Sum(s => s.AmountInProfitCurrency);
            }

            var allPayablesLocal = openPayablesLocal;
            var allPayablesProfit = openPayablesProfit;
            var allReceivablesLocal = openReceivablesLocal;
            var allReceivablesProfit = openReceivablesProfit;

            // New Design
            var profitInLocal = allReceivablesLocal - allPayablesLocal;
            var profitInProfit = allReceivablesProfit - allPayablesProfit;

            /* Payables */
            if (shipmentPM.OpenPayablesInLocalCurrency != openPayablesLocal)
            {
                shipmentPM.OpenPayablesInLocalCurrency = (openPayablesLocal == null) ? 0 : ComputeHelper.Round(openPayablesLocal, 2);
            }

            if (shipmentPM.OpenPayablesInProfitCurrency != openPayablesProfit)
            {
                shipmentPM.OpenPayablesInProfitCurrency = (openPayablesProfit == null) ? 0 : ComputeHelper.Round(openPayablesProfit, 2);
            }

            /* Receivables */
            if (shipmentPM.OpenReceivablesInLocalCurrency != openReceivablesLocal)
            {
                shipmentPM.OpenReceivablesInLocalCurrency = (openReceivablesLocal == null) ? 0 : ComputeHelper.Round(openReceivablesLocal, 2);
            }

            if (shipmentPM.OpenReceivablesInProfitCurrency != openReceivablesProfit)
            {
                shipmentPM.OpenReceivablesInProfitCurrency = (openReceivablesProfit == null) ? 0 : ComputeHelper.Round(openReceivablesProfit, 2);
            }

            /* Profit */
            if (shipmentPM.ProfitInLocalCurrency != profitInLocal)
            {
                shipmentPM.ProfitInLocalCurrency = ((profitInLocal == null) ? 0 : ComputeHelper.Round(profitInLocal, 2)).Value;
            }

            if (shipmentPM.ProfitInProfitCurrency != profitInProfit)
            {
                shipmentPM.ProfitInProfitCurrency = (profitInProfit == null) ? 0 : ComputeHelper.Round(profitInProfit, 2);
            }
        }
    }
}