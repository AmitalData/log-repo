using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

using Logitude.BL.ShipmentsModel.EntityPMs;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ShipmentPayableQuery
    {
        ShipmentPayableRepository repository;
         
        public ShipmentPayableQuery(int tenant)
        {
            repository = new ShipmentPayableRepository(tenant);
        }

        public ShipmentPayableQuery(ShipmentPayableRepository repository)
        {
            this.repository = repository;
        }

        public List<ShipmentPayablePM> GetShipmentPayablePMsByShipment(string shipmentId, int tenant)
        {
            IQueryable<ShipmentPayable> iQueryable = (from a in repository.context.ShipmentPayables
                                                      where a.ShipmentId == shipmentId && a.Tenant == tenant
                                                      select a);

            List<ShipmentPayablePM> shipmentPayables = this.MapPocoToPM(iQueryable);

            foreach (ShipmentPayablePM item in shipmentPayables)
            {
                IQueryable<ShipmentPayable> iQueryableChilds = (from a in repository.context.ShipmentPayables
                                                                where a.ShipmentPayableParentId == item.Id && a.Tenant == tenant
                                                                select a);

                item.ChildShipmentPayables = this.MapPocoToPM(iQueryableChilds);
            }

            shipmentPayables = shipmentPayables.OrderBy(d => d.ViewOrder).ThenBy(d => d.ChargesTypeCode).ToList();

            return shipmentPayables;
        }

        public List<ShipmentPayablePM> GetInvoiceOpenAmountPayables(string shipmentId, int tenant)
        {
            IQueryable<ShipmentPayable> iQueryable = (from a in repository.context.ShipmentPayables
                                                      where a.ShipmentId == shipmentId
                                                      && a.Tenant == tenant
                                                      && (a.ShipmentPayableLineStatusCode == "OAMT" || a.ShipmentPayableLineStatusCode == "PACC")
                                                      select a);

            List<ShipmentPayablePM> shipmentPayables = this.MapPocoToPM(iQueryable);
            return shipmentPayables;
        }

        private List<ShipmentPayablePM> MapPocoToPM(IQueryable<ShipmentPayable> iQueryable)
        {
            List<ShipmentPayablePM> myResult = (from a in iQueryable.Include("ChargesType").Include("DueType").Include("Measurement").Include("Currency").Include("ShipmentPayableLineStatus").Include("VendorCard").Include("ShipmentPayableAmountType").Include("Shipment").Include("CreatedByUser.Contact").Include("UpdateByUser.Contact")
                                                select new ShipmentPayablePM()
                                                {
                                                    AWBPrint = a.AWBPrint,
                                                    Id = a.Id,
                                                    Notes = a.Notes,
                                                    IATACodeId = a.IATACodeId,
                                                    CreateDate = a.CreateDate,
                                                    CreatedByUserId = a.CreatedByUserId,
                                                    Quantity = a.Quantity,
                                                    Rate = a.Rate,
                                                    Tenant = a.Tenant,
                                                    ExpectedAmount = a.ExpectedAmount,
                                                    ExpectedAmountLocal = a.ExpectedAmountLocal,
                                                    UnitPrice = a.UnitPrice,
                                                    UpdateByUserId = a.UpdateByUserId,
                                                    UpdateDate = a.UpdateDate,
                                                    ValueDate = a.ValueDate,
                                                    IsFromQuote = a.IsFromQuote,
                                                    PrepaidCollectId = a.PrepaidCollectId,
                                                    MinAmount = a.MinAmount,
                                                    MaxAmount = a.MaxAmount,
                                                    ExpectedAmountInProfitCurrency = a.ExpectedAmountInProfitCurrency,
                                                    ProfitCurrencyExchangeRate = a.ProfitCurrencyExchangeRate,
                                                    IsEditedByUser = a.IsEditedByUser,
                                                    ShipmentPayableParentId = a.ShipmentPayableParentId,
                                                    AccountedAmount = a.AccountedAmount,
                                                    AccountedAmountInLocalCurrency = a.AccountedAmountInLocalCurrency,
                                                    AccountedAmountInProfitCurrency = a.AccountedAmountInProfitCurrency,
                                                    OpenAmount = a.OpenAmount,
                                                    OpenAmountInLocalCurrency = a.OpenAmountInLocalCurrency,
                                                    OpenAmountInProfitCurrency = a.OpenAmountInProfitCurrency,
                                                    CorrectionAmount = a.CorrectionAmount,
                                                    CorrectionByUserId = a.CorrectionByUserId,
                                                    CorrectionDate = a.CorrectionDate,
                                                    CorrectionNote = a.CorrectionNote,
                                                    QuoteChargeId = a.QuoteChargeId,
                                                    IsChargeBySteps = a.IsChargeBySteps,
                                                    ChargesTypeId = a.ChargesTypeId,
                                                    ChargesTypeCode = a.ChargesType == null ? null : a.ChargesType.Code,
                                                    ChargesTypeName = a.ChargesType == null ? null : a.ChargesType.EnglishName,
                                                    ChargesGroupCode = a.ChargesType == null ? null : a.ChargesType.ChargesGroupCode,
                                                    IsExpenseCharge = a.ChargesType == null ? null : (bool?)a.ChargesType.IsExpense,
                                                    ViewOrder = a.ChargesType == null ? 0 : a.ChargesType.ViewOrder,
                                                    DueTypeCode = a.DueTypeCode,
                                                    DueTypeName = a.DueType == null ? null : a.DueType.Name,
                                                    MeasurementId = a.MeasurementId,
                                                    MeasurementCode = a.Measurement == null ? null : a.Measurement.Code,
                                                    MeasurementShortName = a.Measurement == null ? null : a.Measurement.ShortName,
                                                    CurrencyId = a.CurrencyId,
                                                    CurrencyCode = a.Currency == null ? null : a.Currency.Code,
                                                    ShipmentPayableLineStatusCode = a.ShipmentPayableLineStatusCode,
                                                    ShipmentPayableLineStatusName = a.ShipmentPayableLineStatus == null ? null : a.ShipmentPayableLineStatus.Name,
                                                    VendorId = a.VendorId,
                                                    VendorName = a.VendorCard == null ? null : a.VendorCard.EnglishName,
                                                    ShipmentPayableAmountTypeCode = a.ShipmentPayableAmountTypeCode,
                                                    ShipmentPayableAmountTypeName = a.ShipmentPayableAmountType == null ? null : a.ShipmentPayableAmountType.Name,
                                                    ShipmentId = a.ShipmentId,
                                                    ShipmentNumber = a.Shipment == null ? null : a.Shipment.ShipmentNumber,
                                                    VatTypeId = a.VatTypeId,
                                                    UOMPercentage = a.Measurement == null ? "" : (a.Measurement.Code == "PRVL" || a.Measurement.Code == "PRFR" ? "%" : ""),
                                                    IsBackToBack = a.IsBackToBack,
                                                    ReceivableId = a.ReceivableId,
                                                    QuoteCostMinAmount = a.QuoteCostMinAmount,
                                                    QuoteCostMaxAmount = a.QuoteCostMaxAmount,
                                                    TariffId = a.TariffId,
                                                    IsCustomsChargesTariff = a.IsCustomsChargesTariff,
                                                    TariffNumber = a.TariffNumber,
                                                    TariffLineId = a.TariffLineId,
                                                    TariffVersion = a.TariffVersion,
                                                    VatAmountProfit = a.VatAmountProfit,
                                                    VatAmountLocal = a.VatAmountLocal,
                                                    CreatedByUserName = a.CreatedByUser == null ? null : (a.CreatedByUser.Contact == null ? null : a.CreatedByUser.Contact.EnglishName),
                                                    UpdateByUserName = a.UpdateByUser == null ? null : (a.UpdateByUser.Contact == null ? null : a.UpdateByUser.Contact.EnglishName),
                                                }).ToList();

            return myResult;
        }

        public ShipmentPayablePM GetSinglePM(string id, int tenant)
        {
            ShipmentPayablePM myResult
                = (from a in repository.context.ShipmentPayables.Include("ChargesType").Include("DueType").Include("Measurement").Include("Currency").Include("ShipmentPayableLineStatus").Include("VendorCard").Include("ShipmentPayableAmountType").Include("Shipment").Include("CreatedByUser.Contact").Include("UpdateByUser.Contact")
                   where a.Id == id && a.Tenant == tenant
                   select new ShipmentPayablePM()
                   {
                       AWBPrint = a.AWBPrint,
                       Id = a.Id,
                       Notes = a.Notes,
                       IATACodeId = a.IATACodeId,
                       CreateDate = a.CreateDate,
                       CreatedByUserId = a.CreatedByUserId,
                       Quantity = a.Quantity,
                       Rate = a.Rate,
                       Tenant = a.Tenant,
                       ExpectedAmount = a.ExpectedAmount,
                       ExpectedAmountLocal = a.ExpectedAmountLocal,
                       UnitPrice = a.UnitPrice,
                       UpdateByUserId = a.UpdateByUserId,
                       UpdateDate = a.UpdateDate,
                       ValueDate = a.ValueDate,
                       IsFromQuote = a.IsFromQuote,
                       PrepaidCollectId = a.PrepaidCollectId,
                       MinAmount = a.MinAmount,
                       MaxAmount = a.MaxAmount,
                       ExpectedAmountInProfitCurrency = a.ExpectedAmountInProfitCurrency,
                       ProfitCurrencyExchangeRate = a.ProfitCurrencyExchangeRate,
                       IsEditedByUser = a.IsEditedByUser,
                       ShipmentPayableParentId = a.ShipmentPayableParentId,
                       AccountedAmount = a.AccountedAmount,
                       AccountedAmountInLocalCurrency = a.AccountedAmountInLocalCurrency,
                       AccountedAmountInProfitCurrency = a.AccountedAmountInProfitCurrency,
                       OpenAmount = a.OpenAmount,
                       OpenAmountInLocalCurrency = a.OpenAmountInLocalCurrency,
                       OpenAmountInProfitCurrency = a.OpenAmountInProfitCurrency,
                       CorrectionAmount = a.CorrectionAmount,
                       CorrectionByUserId = a.CorrectionByUserId,
                       CorrectionDate = a.CorrectionDate,
                       CorrectionNote = a.CorrectionNote,
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
                       ShipmentPayableLineStatusCode = a.ShipmentPayableLineStatusCode,
                       ShipmentPayableLineStatusName = a.ShipmentPayableLineStatus == null ? null : a.ShipmentPayableLineStatus.Name,
                       VendorId = a.VendorId,
                       VendorName = a.VendorCard == null ? null : a.VendorCard.EnglishName,
                       ShipmentPayableAmountTypeCode = a.ShipmentPayableAmountTypeCode,
                       ShipmentPayableAmountTypeName = a.ShipmentPayableAmountType == null ? null : a.ShipmentPayableAmountType.Name,
                       ShipmentId = a.ShipmentId,
                       ShipmentNumber = a.Shipment == null ? null : a.Shipment.ShipmentNumber,
                       VatTypeId = a.VatTypeId,
                       UOMPercentage = a.Measurement == null ? "" : (a.Measurement.Code == "PRVL" || a.Measurement.Code == "PRFR" ? "%" : ""),
                       IsBackToBack = a.IsBackToBack,
                       ReceivableId = a.ReceivableId,
                       QuoteCostMinAmount = a.QuoteCostMinAmount,
                       QuoteCostMaxAmount = a.QuoteCostMaxAmount,
                       TariffId = a.TariffId,
                       IsCustomsChargesTariff = a.IsCustomsChargesTariff,
                       TariffLineId = a.TariffLineId,
                       TariffNumber = a.TariffNumber,
                       TariffVersion = a.TariffVersion,
                       VatAmountProfit = a.VatAmountProfit,
                       VatAmountLocal = a.VatAmountLocal,
                       CreatedByUserName = a.CreatedByUser == null ? null : (a.CreatedByUser.Contact == null ? null : a.CreatedByUser.Contact.EnglishName),
                       UpdateByUserName = a.UpdateByUser == null ? null : (a.UpdateByUser.Contact == null ? null : a.UpdateByUser.Contact.EnglishName),
                   }).FirstOrDefault();

            return myResult;
        }
    }
}