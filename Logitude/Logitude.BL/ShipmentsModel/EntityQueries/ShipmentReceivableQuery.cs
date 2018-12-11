using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ShipmentReceivableQuery
    {
        ShipmentReceivableRepository repository;

        public ShipmentReceivableQuery(int tenant)
        {
            repository = new ShipmentReceivableRepository(tenant);
        }

        public ShipmentReceivableQuery(ShipmentReceivableRepository repository)
        {
            this.repository = repository;
        }

        public List<ShipmentReceivablePM> GetShipmentReceivablePMsByShipmentId(string shipmentId, int tenant)
        {
            IQueryable<ShipmentReceivable> iQueryable = (from a in repository.context.ShipmentReceivables
                                                         where a.ShipmentId == shipmentId && a.Tenant == tenant
                                                         select a);

            List<ShipmentReceivablePM> shipmentReceivables = this.MapPocoToPM(iQueryable);

            foreach (ShipmentReceivablePM item in shipmentReceivables)
            {
                IQueryable<ShipmentReceivable> iQueryableChilds = (from a in repository.context.ShipmentReceivables
                                                                     where a.ShipmentReceivableParentId == item.Id && a.Tenant == tenant
                                                                     select a);

                item.ChildShipmentReceivables = this.MapPocoToPM(iQueryableChilds);
            }

            return shipmentReceivables.OrderBy(d => d.ViewOrder).ThenBy(d => d.ChargesTypeCode).ToList();
        }

        private List<ShipmentReceivablePM> MapPocoToPM(IQueryable<ShipmentReceivable> iQueryable)
        {
            List<ShipmentReceivablePM> myResult = (from a in iQueryable.Include("ChargesType").Include("DueType").Include("Measurement").Include("Currency").Include("ShipmentReceivableLineStatus").Include("Shipment")
                                                   select new ShipmentReceivablePM()
                                                   {
                                                       Quantity = a.Quantity,
                                                       Id = a.Id,
                                                       Notes = a.Notes,
                                                       Rate = a.Rate,
                                                       Tenant = a.Tenant,
                                                       TotalAmount = a.TotalAmount,
                                                       TotalAmountLocal = a.TotalAmountLocal,
                                                       UnitPrice = a.UnitPrice,
                                                       PayableLocal = a.PayableLocal,
                                                       UpdateByUserId = a.UpdateByUserId,
                                                       UpdateDate = a.UpdateDate,
                                                       PrepaidCollectId = a.PrepaidCollectId,
                                                       AWBPrint = a.AWBPrint,
                                                       IATACodeId = a.IATACodeId,
                                                       ARInvoiceLineId = a.ARInvoiceLineId,
                                                       IsFromQuote = a.IsFromQuote,
                                                       IsFixedPrice = a.IsFixedPrice,
                                                       IsExchangeRateFixed = a.IsExchangeRateFixed,
                                                       AmountInProfitCurrency = a.AmountInProfitCurrency,
                                                       ProfitCurrencyExchangeRate = a.ProfitCurrencyExchangeRate,
                                                       CreateDate = a.CreateDate,
                                                       CreatedByUserId = a.CreatedByUserId,
                                                       ShipmentId = a.ShipmentId,
                                                       ARInvoiceId = a.ARInvoiceId,
                                                       QuoteChargeId = a.QuoteChargeId,
                                                       IsChargeBySteps = a.IsChargeBySteps,
                                                       ChargesTypeId = a.ChargesTypeId,
                                                       ChargesTypeCode = a.ChargesType == null ? null : a.ChargesType.Code,
                                                       ChargesTypeName = a.ChargesType == null ? null : a.ChargesType.EnglishName,
                                                       ChargesGroupCode = a.ChargesType == null ? null : a.ChargesType.ChargesGroupCode,
                                                       ViewOrder = a.ChargesType == null ? 0 : a.ChargesType.ViewOrder,
                                                       DueTypeCode = a.DueTypeCode,
                                                       DueTypeName = a.DueType == null ? null : a.DueType.Name,
                                                       MeasurementId = a.MeasurementId,
                                                       MeasurementCode = a.Measurement == null ? null : a.Measurement.Code,
                                                       MeasurementShortName = a.Measurement == null ? null : a.Measurement.ShortName,
                                                       CurrencyId = a.CurrencyId,
                                                       CurrencyCode = a.Currency == null ? null : a.Currency.Code,
                                                       ShipmentReceivableLineStatusCode = a.ShipmentReceivableLineStatusCode,
                                                       ShipmentReceivableLineStatusName = a.ShipmentReceivableLineStatus == null ? null : a.ShipmentReceivableLineStatus.Name,
                                                       VatTypeId = a.VatTypeId,
                                                       UOMPercentage = a.Measurement == null ? "" : (a.Measurement.Code == "PRVL" || a.Measurement.Code == "PRFR" ? "%" : ""),
                                                       IsBackToBack = a.IsBackToBack,
                                                       IsExpense = a.IsExpense,
                                                       ShipmentReceivableParentId = a.ShipmentReceivableParentId,
                                                       ShipmentNumber = a.Shipment == null ? null : a.Shipment.ShipmentNumber,
                                                       QuoteSaleMinAmount = a.QuoteSaleMinAmount,
                                                       QuoteSaleMaxAmount = a.QuoteSaleMaxAmount,
                                                   }).ToList();
            return myResult;
        }

        public ShipmentReceivablePM GetSinglePM(string id, int tenant)
        {
            ShipmentReceivablePM myResult
                = (from a in repository.context.ShipmentReceivables.Include("ChargesType").Include("DueType").Include("Measurement").Include("Currency").Include("ShipmentReceivableLineStatus").Include("Shipment")
                   where a.Id == id && a.Tenant == tenant
                   select new ShipmentReceivablePM()
                   {
                       Quantity = a.Quantity,
                       Id = a.Id,
                       Notes = a.Notes,
                       Rate = a.Rate,
                       Tenant = a.Tenant,
                       TotalAmount = a.TotalAmount,
                       TotalAmountLocal = a.TotalAmountLocal,
                       UnitPrice = a.UnitPrice,
                       PayableLocal = a.PayableLocal,
                       UpdateByUserId = a.UpdateByUserId,
                       UpdateDate = a.UpdateDate,
                       PrepaidCollectId = a.PrepaidCollectId,
                       AWBPrint = a.AWBPrint,
                       IATACodeId = a.IATACodeId,
                       ARInvoiceLineId = a.ARInvoiceLineId,
                       IsFromQuote = a.IsFromQuote,
                       IsFixedPrice = a.IsFixedPrice,
                       IsExchangeRateFixed = a.IsExchangeRateFixed,
                       AmountInProfitCurrency = a.AmountInProfitCurrency,
                       ProfitCurrencyExchangeRate = a.ProfitCurrencyExchangeRate,
                       CreateDate = a.CreateDate,
                       CreatedByUserId = a.CreatedByUserId,
                       ShipmentId = a.ShipmentId,
                       ARInvoiceId = a.ARInvoiceId,
                       QuoteChargeId = a.QuoteChargeId,
                       IsChargeBySteps = a.IsChargeBySteps,
                       ChargesTypeId = a.ChargesTypeId,
                       ChargesTypeCode = a.ChargesType == null ? null : a.ChargesType.Code,
                       ChargesTypeName = a.ChargesType == null ? null : a.ChargesType.EnglishName,
                       ChargesGroupCode = a.ChargesType == null ? null : a.ChargesType.ChargesGroupCode,
                       ViewOrder = a.ChargesType == null ? 0 : a.ChargesType.ViewOrder,
                       DueTypeCode = a.DueTypeCode,
                       DueTypeName = a.DueType == null ? null : a.DueType.Name,
                       MeasurementId = a.MeasurementId,
                       MeasurementCode = a.Measurement == null ? null : a.Measurement.Code,
                       MeasurementShortName = a.Measurement == null ? null : a.Measurement.ShortName,
                       CurrencyId = a.CurrencyId,
                       CurrencyCode = a.Currency == null ? null : a.Currency.Code,
                       ShipmentReceivableLineStatusCode = a.ShipmentReceivableLineStatusCode,
                       ShipmentReceivableLineStatusName = a.ShipmentReceivableLineStatus == null ? null : a.ShipmentReceivableLineStatus.Name,
                       VatTypeId = a.VatTypeId,
                       UOMPercentage = a.Measurement == null ? "" : (a.Measurement.Code == "PRVL" || a.Measurement.Code == "PRFR" ? "%" : ""),
                       IsBackToBack = a.IsBackToBack,
                       IsExpense = a.IsExpense,
                       ShipmentReceivableParentId = a.ShipmentReceivableParentId,
                       ShipmentNumber = a.Shipment == null ? null : a.Shipment.ShipmentNumber,
                       QuoteSaleMinAmount = a.QuoteSaleMinAmount,
                       QuoteSaleMaxAmount = a.QuoteSaleMaxAmount,
                   }).FirstOrDefault();

            return myResult;
        }
    }
}