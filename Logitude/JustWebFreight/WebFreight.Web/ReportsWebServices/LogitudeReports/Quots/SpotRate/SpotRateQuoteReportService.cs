using Logitude.BL.QuoteModel.DataContracts;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using WebFreight.Web.DataProviders;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.Quots.SpotRate
{
    public class SpotRateQuoteReportService
    {
        private readonly int tenant;
        private readonly SpotRateQuoteReportFilter spotRateQuoteReportFilter;
        private readonly IQuotesContext quotesContext;
        private readonly IShipmentsContext shipmentsContext;
        private readonly ICommonDataContext commonDataContext;
        private readonly CurrencyRepository currencyRepository;

        public SpotRateQuoteReportService(int tenant, SpotRateQuoteReportFilter spotRateQuoteReportFilter)
        {
            this.tenant = tenant;
            this.spotRateQuoteReportFilter = spotRateQuoteReportFilter;
            quotesContext = QuotesContext.GetContext(tenant);
            shipmentsContext = ShipmentsContext.GetContext(tenant);
            commonDataContext = CommonDataContext.GetContext(tenant);
            currencyRepository = new CurrencyRepository(commonDataContext);
        }

        public SpotRateQuoteReportDataProvider Build()
        {
            var loggedTenant = TenantRepository.GetSingleTenant(tenant, true);

            SpotRateQuoteReportDataProvider spotRateQuoteReportDataProvider = new SpotRateQuoteReportDataProvider();
            spotRateQuoteReportDataProvider.Today_DateTime = DateTime.Now;
            spotRateQuoteReportDataProvider.QuoteCharges = BuildQuoteCharges();
            spotRateQuoteReportDataProvider.ProfitCurrency = loggedTenant.ProfitCurrencyId == null ? null : currencyRepository.GetSingleCurrencyById(loggedTenant.ProfitCurrencyId, tenant, true)?.Code;
            spotRateQuoteReportDataProvider.LocalCurrency = loggedTenant.CurrencyId == null ? null : currencyRepository.GetSingleCurrencyById(loggedTenant.CurrencyId, tenant, true)?.Code;
            return spotRateQuoteReportDataProvider;
        }

        private List<QuoteChargeItem> BuildQuoteCharges()
        {
            var quoteCharges = quotesContext.QuoteCharges
                .Include("Quote").Include("Quote.ShipperCard").Include("Quote.ConsigneeCard")
                .Include("ChargesType").Include("CostMeasurement").Include("CostCurrency")
                .Include("SaleMeasurement").Include("Currency").Include("Quote.Stage").Include("Quote.ShipmentType")
                .Where(a => a.Tenant == tenant && a.Quote.QuoteTypeCode == "A");
            quoteCharges = FilterQuoteCharges(quoteCharges);

            var quoteChargeItems = quoteCharges.OrderBy(x => x.ValueDate).ThenBy(x => x.Quote.QuoteNumber).AsEnumerable()
                .Select(quoteCharge => MapQuoteChargeToQuoteChargeItem(quoteCharge)).ToList();
            return SetQuotesShipments(quoteChargeItems);
        }
        private IQueryable<QuoteCharge> FilterQuoteCharges(IQueryable<QuoteCharge> quoteChargeItems)
        {
            if (!string.IsNullOrEmpty(spotRateQuoteReportFilter.CustomerId))
            {
                quoteChargeItems = quoteChargeItems.Where(d => d.Quote != null && d.Quote.CustomerId == spotRateQuoteReportFilter.CustomerId);
            }

            if (!string.IsNullOrEmpty(spotRateQuoteReportFilter.SalesmanId))
            {
                quoteChargeItems = quoteChargeItems.Where(d => d.Quote != null && d.Quote.SalesmanUserId == spotRateQuoteReportFilter.SalesmanId);
            }

            if (spotRateQuoteReportFilter.OpenDateGraterThan != null)
            {
                quoteChargeItems = quoteChargeItems.Where(d => d.Quote != null && d.Quote.OpenDate > spotRateQuoteReportFilter.OpenDateGraterThan);
            }

            if (spotRateQuoteReportFilter.ExpirationDateLessThan != null)
            {
                quoteChargeItems = quoteChargeItems.Where(d => d.Quote != null && d.Quote.ExpirationDate < spotRateQuoteReportFilter.ExpirationDateLessThan);
            }

            return quoteChargeItems;
        }

        private QuoteChargeItem MapQuoteChargeToQuoteChargeItem(QuoteCharge quoteCharge)
        {
            var saleAmountInProfitCurrency = quoteCharge.SaleTotalAmountLocal == null ? 0 : quoteCharge.Quote.ProfitExchangeRate == null || quoteCharge.Quote.ProfitExchangeRate == 0 ? quoteCharge.SaleTotalAmountLocal : quoteCharge.SaleTotalAmountLocal / quoteCharge.Quote.ProfitExchangeRate;
            var costAmountInProfitCurrency = quoteCharge.CostTotalAmountLocal == null ? 0 : quoteCharge.Quote.ProfitExchangeRate == null || quoteCharge.Quote.ProfitExchangeRate == 0 ? quoteCharge.CostTotalAmountLocal : quoteCharge.CostTotalAmountLocal / quoteCharge.Quote.ProfitExchangeRate;
            bool showAmount = quoteCharge.Quote == null || quoteCharge.Quote.ShipmentType == null || (quoteCharge.Quote.ShipmentType.Id != "FCL" && quoteCharge.Quote.ShipmentType.Id != "FTL");

            return new QuoteChargeItem
            {
                QuoteId = quoteCharge.QuoteId,

                CreateDate = quoteCharge.ValueDate,
                QuoteNumber = quoteCharge.Quote?.QuoteNumber,
                ShipperName = quoteCharge.Quote?.ShipperCard?.EnglishName,
                ConsigneeName = quoteCharge.Quote?.ConsigneeCard?.EnglishName,
                ChargeType = quoteCharge.ChargesType?.EnglishName,
                ChargeTypeCode= quoteCharge.ChargesType?.Code,
                CostMeasurement = quoteCharge.CostMeasurement?.Code,
                CostCurrency = quoteCharge.CostCurrency?.Code,
                CostQuantity = showAmount ? quoteCharge.CostQuantity : null,
                CostUnitPrice = showAmount ? quoteCharge.CostUnitPrice : null,
                CostTotalAmount = quoteCharge.CostTotalAmount,

                SaleMeasurement = quoteCharge.SaleMeasurement?.Code,
                SaleCurrency = quoteCharge.Currency?.Code,
                SaleQuantity = showAmount ? quoteCharge.SaleQuantity : null,
                SaleUnitPrice = showAmount ? quoteCharge.SaleUnitPrice : null,
                SaleTotalAmount = quoteCharge.SaleTotalAmount,

                SaleAmountInProfitCurrency = saleAmountInProfitCurrency,
                SaleTotalAmountLocal = quoteCharge.SaleTotalAmountLocal,

                ExpectedProfit = saleAmountInProfitCurrency - costAmountInProfitCurrency,
                Stage = quoteCharge.Quote?.Stage?.Name,
            };
        }

        private List<QuoteChargeItem> SetQuotesShipments(List<QuoteChargeItem> quoteChargeItems)
        {
            var quoteIds = quoteChargeItems.Select(x => x.QuoteId);
            var shipments = shipmentsContext.Shipments.Where(x => quoteIds.Contains(x.QuoteId)).Select(x => new { x.ShipmentNumber, x.QuoteId }).ToList();

            foreach (var quoteChargeItem in quoteChargeItems) SetSingleQuoteShipments(quoteChargeItem, shipments);

            return quoteChargeItems;
        }

        private void SetSingleQuoteShipments(QuoteChargeItem quoteChargeItem, IEnumerable<dynamic> shipments)
        {
            var quoteChargeItemShipments = shipments.Where(x => x.QuoteId == quoteChargeItem.QuoteId).Select(x => x.ShipmentNumber);
            if (!quoteChargeItemShipments.Any()) return;

            quoteChargeItem.ConnectedShipmentsNumbers = string.Join(",", quoteChargeItemShipments);
            quoteChargeItem.ConnectedToShipment = true;
        }
    }
}