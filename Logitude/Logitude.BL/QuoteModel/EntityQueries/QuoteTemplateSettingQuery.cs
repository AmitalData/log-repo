using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.QuoteModel.EntityLists;
using Logitude.BL.QuoteModel.EntityPMs;

namespace Logitude.BL.QuoteModel.EntityQueries
{
    public class QuoteTemplateSettingQuery
    {
        QuoteTemplateSettingRepository repository;
             
        public QuoteTemplateSettingQuery()
        {
            repository = new QuoteTemplateSettingRepository(); 
        }

        public QuoteTemplateSettingQuery(int tenant)
        {
            repository = new QuoteTemplateSettingRepository(tenant);
        }

        public QuoteTemplateSettingQuery(QuoteTemplateSettingRepository quoteTemplateSettingRepository)
        {
            repository = quoteTemplateSettingRepository;
        }
     
        public QuoteTemplateSettingPM GetSinglePM(string id, int tenant)
        {
            QuoteTemplateSettingPM entity;
            entity = (from a in repository.quotesContext.QuoteTemplateSettings
                      where a.Tenant == tenant && a.Id == id
                                              select new QuoteTemplateSettingPM()
                                           {
                                                 Id = a.Id,
                                                 Tenant = a.Tenant,
                                                 PackagesTableDesignId = a.PackagesTableDesignId,
                                                 ContainserTableDesignId = a.ContainserTableDesignId,
                                                 TotalsPackagesLabelDesignId = a.TotalsPackagesLabelDesignId,
                                                 TotalsContainsersLabelDesignId = a.TotalsContainsersLabelDesignId,
                                                 TotalsPackagesValueDesignId = a.TotalsPackagesValueDesignId,
                                                 TotalsContainsersValueDesignId = a.TotalsContainsersValueDesignId,
                                                 RightToLeft = a.RightToLeft,
                                                 ShowTitleQuoteDetails = a.ShowTitleQuoteDetails,
                                                 AlignRight = a.AlignRight,
                                                 ShowUnitPricePackages = a.ShowUnitPricePackages,
                                                 ShowUnitsPackages = a.ShowUnitsPackages,
                                                 ShowChargeCodePackages = a.ShowChargeCodePackages,
                                                 ShowChargeCodeContainers = a.ShowChargeCodeContainers,
                                                 ShowChargeNamePackages = a.ShowChargeNamePackages,
                                                 ShowChargeNameContainers = a.ShowChargeNameContainers,
                                                 ShowMeasurementPackages = a.ShowMeasurementPackages,
                                                 ShowMeasurementContainers = a.ShowMeasurementContainers,
                                                 ShowContainerNameInsteadOfCodeContainers = a.ShowContainerNameInsteadOfCodeContainers,
                                                 ShowFixedPriceContainers = a.ShowFixedPriceContainers,
                                                 ShowSaleCurrencyColumnPackages = a.ShowSaleCurrencyColumnPackages,
                                                 ShowSaleCurrencyColumnContainers = a.ShowSaleCurrencyColumnContainers,
                                                 ShowLocalCurrencyColumnContainers = a.ShowLocalCurrencyColumnContainers,
                                                 ShowLocalCurrencyColumnPackages = a.ShowLocalCurrencyColumnPackages,
                                                 SplitChargesbyGroupsPackages = a.SplitChargesbyGroupsPackages,
                                                 SplitChargesbyGroupsContainers = a.SplitChargesbyGroupsContainers,
                                                 ShowLocalLanguage = a.ShowLocalLanguage,
                                                 ShowTotalInLocalCurrencyPackages = a.ShowTotalInLocalCurrencyPackages,
                                                 ShowTotalInLocalCurrencyContainers = a.ShowTotalInLocalCurrencyContainers,
                                                 ShowTotalInSaleCurrencyContainers = a.ShowTotalInSaleCurrencyContainers,
                                                 ShowPricesTablePackages = a.ShowPricesTablePackages,
                                                 ShowPricesTableContainers = a.ShowPricesTableContainers,
                                                 ShowTotalInSaleCurrencyPackages = a.ShowTotalInSaleCurrencyPackages,
                                                  GroupByPackagesLabelDesignId = a.GroupByPackagesLabelDesignId,
                                                 GroupByPackagesValueDesignId = a.GroupByPackagesValueDesignId,
                                                 GroupByContainsersLabelDesignId = a.GroupByContainsersLabelDesignId,
                                                 GroupByContainsersValueDesignId = a.GroupByContainsersValueDesignId,
                                                 DetailsTableDesignId = a.DetailsTableDesignId,
                                                 DetailsSectionHasTwoColumns = a.DetailsSectionHasTwoColumns,
                                                 HeaderTableDesignId = a.HeaderTableDesignId,
                                                 HeaderSectionHasTwoColumns = a.HeaderSectionHasTwoColumns,
                                                  ShowHeaderQuoteDate = a.ShowHeaderQuoteDate,
                                                 ShowHeaderExpirationDate = a.ShowHeaderExpirationDate,
                                                 ShowHeaderQuoteNumber = a.ShowHeaderQuoteNumber,
                                                 ShowHeaderCustomer = a.ShowHeaderCustomer,
                                                 ShowDetailsExpirationDate = a.ShowDetailsExpirationDate,
                                                 ShowDetailsExpirationDays = a.ShowDetailsExpirationDays,
                                                 ShowDetailsShipperName = a.ShowDetailsShipperName,
                                                 ShowDetailsShipperAddress = a.ShowDetailsShipperAddress,
                                                 ShowDetailsShipperContact = a.ShowDetailsShipperContact,
                                                 ShowDetailsShipperReferences = a.ShowDetailsShipperReferences,
                                                 ShowDetailsConsigneeName = a.ShowDetailsConsigneeName,
                                                 ShowDetailsConsigneeAddress = a.ShowDetailsConsigneeAddress,
                                                 ShowDetailsConsigneeContact = a.ShowDetailsConsigneeContact,
                                                 ShowDetailsConsigneeReferences = a.ShowDetailsConsigneeReferences,
                                                 ShowDetailsPickupFrom = a.ShowDetailsPickupFrom,
                                                 ShowDetailsDeliveryTo = a.ShowDetailsDeliveryTo,
                                                  ShowDetailsFromPort = a.ShowDetailsFromPort,
                                                 ShowDetailsToPort = a.ShowDetailsToPort,
                                                 ShowDetailsSalesMan = a.ShowDetailsSalesMan,
                                                 ShowDetailsIncoterms = a.ShowDetailsIncoterms,
                                                 ShowDetailsService = a.ShowDetailsService,
                                                 ShowDetailsDescriptionOfGoods = a.ShowDetailsDescriptionOfGoods,
                                                 ShowDetailsDangerousGoods = a.ShowDetailsDangerousGoods,
                                                 ShowDetailsCarrier = a.ShowDetailsCarrier,
                                                DetailsTitleDesignId = a.DetailsTitleDesignId,
                                                 ShowDetailsCustomerName = a.ShowDetailsCustomerName,
                                                 ShowDetailsCustomerAddress = a.ShowDetailsCustomerAddress,
                                                 ShowDetailsCustomerContact = a.ShowDetailsCustomerContact,
                                                 ShowDetailsCustomerReferences = a.ShowDetailsCustomerReferences,


                                                 PageHeaderArea1Type = a.PageHeaderArea1Type,
                                                 PageHeaderArea2Type = a.PageHeaderArea2Type,
                                                 PageHeaderArea3Type = a.PageHeaderArea3Type,

                                                 PageHeaderArea1ImageDetailId = a.PageHeaderArea1ImageDetailId,
                                                 PageHeaderArea2ImageDetailId = a.PageHeaderArea2ImageDetailId,
                                                 PageHeaderArea3ImageDetailId = a.PageHeaderArea3ImageDetailId,

                                                 PageHeaderArea1FreeText = a.PageHeaderArea1FreeText,
                                                 PageHeaderArea2FreeText = a.PageHeaderArea2FreeText,
                                                 PageHeaderArea3FreeText = a.PageHeaderArea3FreeText,

                                                 PageHeaderArea1FreeTextDesignId = a.PageHeaderArea1FreeTextDesignId,
                                                 PageHeaderArea2FreeTextDesignId = a.PageHeaderArea2FreeTextDesignId,
                                                 PageHeaderArea3FreeTextDesignId = a.PageHeaderArea3FreeTextDesignId,
                                                 PageHeaderArea1Width = a.PageHeaderArea1Width,
                                                 PageHeaderArea2Width = a.PageHeaderArea2Width,
                                                 PageHeaderArea3Width = a.PageHeaderArea3Width,

                                                 PageHeaderArea1Height = a.PageHeaderArea1Height,
                                                 PageHeaderArea2Height = a.PageHeaderArea2Height,
                                                 PageHeaderArea3Height = a.PageHeaderArea3Height,

                                                 PageHeaderArea1ImageAlignment = a.PageHeaderArea1ImageAlignment,
                                                 PageHeaderArea2ImageAlignment = a.PageHeaderArea2ImageAlignment,
                                                 PageHeaderArea3ImageAlignment = a.PageHeaderArea3ImageAlignment,




                                                 PageFooterArea1Type = a.PageFooterArea1Type,
                                                 PageFooterArea2Type = a.PageFooterArea2Type,
                                                 PageFooterArea3Type = a.PageFooterArea3Type,

                                                 PageFooterArea1ImageDetailId = a.PageFooterArea1ImageDetailId,
                                                 PageFooterArea2ImageDetailId = a.PageFooterArea2ImageDetailId,
                                                 PageFooterArea3ImageDetailId = a.PageFooterArea3ImageDetailId,

                                                 PageFooterArea1FreeText = a.PageFooterArea1FreeText,
                                                 PageFooterArea2FreeText = a.PageFooterArea2FreeText,
                                                 PageFooterArea3FreeText = a.PageFooterArea3FreeText,

                                                 PageFooterArea1FreeTextDesignId = a.PageFooterArea1FreeTextDesignId,
                                                 PageFooterArea2FreeTextDesignId = a.PageFooterArea2FreeTextDesignId,
                                                 PageFooterArea3FreeTextDesignId = a.PageFooterArea3FreeTextDesignId,
                                                 PageFooterArea1Width = a.PageFooterArea1Width,
                                                 PageFooterArea2Width = a.PageFooterArea2Width,
                                                 PageFooterArea3Width = a.PageFooterArea3Width,

                                                 PageFooterArea1Height = a.PageFooterArea1Height,
                                                 PageFooterArea2Height = a.PageFooterArea2Height,
                                                 PageFooterArea3Height = a.PageFooterArea3Height,

                                                 PageFooterArea1ImageAlignment = a.PageFooterArea1ImageAlignment,
                                                 PageFooterArea2ImageAlignment = a.PageFooterArea2ImageAlignment,
                                                 PageFooterArea3ImageAlignment = a.PageFooterArea3ImageAlignment,



                                                 PricingPackagesTitleDesignId = a.PricingPackagesTitleDesignId,
                                                 PricingContainsersTitleDesignId = a.PricingContainsersTitleDesignId,
                                                 ShowTitlePricingPackages = a.ShowTitlePricingPackages,
                                                 ShowTitlePricingContainsers = a.ShowTitlePricingContainsers,






                                                 PageHeaderAreaHeight = a.PageHeaderAreaHeight,
                                                 PageFooterAreaHeight = a.PageFooterAreaHeight,
                                                 PageHeaderImage1Width = a.PageHeaderImage1Width,
                                                 PageHeaderImage2Width = a.PageHeaderImage2Width,


                                                 PageHeaderImage3Width = a.PageHeaderImage3Width,
                                                 PageFooterImage1Width = a.PageFooterImage1Width,
                                                 PageFooterImage2Width = a.PageFooterImage2Width,
                                                 PageFooterImage3Width = a.PageFooterImage3Width,


                                                 PageHeaderBorderTypeCode = a.PageHeaderBorderTypeCode,
                                                 PageHeaderBorderColor = a.PageHeaderBorderColor,
                                                 PageHeaderBorderThickness = a.PageHeaderBorderThickness,







                                                 PageFooterBorderTypeCode = a.PageFooterBorderTypeCode,
                                                 PageFooterBorderColor = a.PageFooterBorderColor,
                                                 PageFooterBorderThickness = a.PageFooterBorderThickness,


                                                 DetailsTableColumWidthType = a.DetailsTableColumWidthType,
                                                 HeaderTableColumWidthType = a.HeaderTableColumWidthType,

                                                 DetailsTableColumn1LabelWidth = a.DetailsTableColumn1LabelWidth,
                                                 DetailsTableColumn1ValueWidth = a.DetailsTableColumn1ValueWidth,
                                                 DetailsTableColumn2LabelWidth = a.DetailsTableColumn2LabelWidth,
                                                 DetailsTableColumn2ValueWidth = a.DetailsTableColumn2ValueWidth,

                                                 HeaderTableColumn1LabelWidth = a.HeaderTableColumn1LabelWidth,
                                                 HeaderTableColumn1ValueWidth = a.HeaderTableColumn1ValueWidth,
                                                 HeaderTableColumn2LabelWidth = a.HeaderTableColumn2LabelWidth,
                                                 HeaderTableColumn2ValueWidth = a.HeaderTableColumn2ValueWidth,
                                                   QuoteTemplatePDFMarginRight = a.QuoteTemplatePDFMarginRight,
                                                     QuoteTemplatePDFMarginLeft = a.QuoteTemplatePDFMarginLeft,

                                                 ShowCodeChargeSaleMinMaxContainers = a.ShowCodeChargeSaleMinMaxContainers,
                                                 ShowCodeChargeSaleMinMaxPackages = a.ShowCodeChargeSaleMinMaxPackages,

                                              
                                                 ShowPriceByContainerColumn = a.ShowPriceByContainerColumn,

                                                 ShowChargeDescriptionContainers = a.ShowChargeDescriptionContainers,
                                                 ShowChargeDescriptionPackages = a.ShowChargeDescriptionPackages,
                                                 ShowTotalPerChargeGroupPackages = a.ShowTotalPerChargeGroupPackages,
                                                  ShowTotalPerChargeGroupContainers = a.ShowTotalPerChargeGroupContainers,
                                                  ShowPageBreakBeforeTotalPerContainersTable = a.ShowPageBreakBeforeTotalPerContainersTable,
                                                  TotalPerContainersCurrencyType = a.TotalPerContainersCurrencyType,
                                                  TotalPerContainersAdditionalTextDesignId = a.TotalPerContainersAdditionalTextDesignId,
                                                  TotalPerContainersTableDesignId = a.TotalPerContainersTableDesignId,
                                                  ShowTitleTotalPerContainersTable = a.ShowTitleTotalPerContainersTable,
                                                  ShowChargeNoteContainers = a.ShowChargeNoteContainers,
                                                  ShowChargeNotePackages = a.ShowChargeNotePackages,
                                                  ShowSaleMaxMinAmountContainers = a.ShowSaleMaxMinAmountContainers,
                                                  ShowSaleMaxMinAmountPackages =a.ShowSaleMaxMinAmountPackages,
                                                  ShowHeaderLabelsContainers = a.ShowHeaderLabelsContainers,
                                                  ShowHeaderLabelsPackages = a.ShowHeaderLabelsPackages,

                                                  SpaceLinesBeforeContainers =a.SpaceLinesBeforeContainers,
                                                  SpaceLinesBeforeFooters =a.SpaceLinesBeforeFooters,
                                                  SpaceLinesBeforeHeaders =a.SpaceLinesBeforeHeaders,
                                                  SpaceLinesBeforePackages = a.SpaceLinesBeforePackages,
                                                  SpaceLinesBeforeQuoteDetails = a.SpaceLinesBeforeQuoteDetails,
                                                  SpaceLinesBeforeQuoteHeaders = a.SpaceLinesBeforeQuoteHeaders,
                                                  SpaceLinesBeforePerContainers = a.SpaceLinesBeforePerContainers,
                                                  QuoteTemplatePDFMarginBottom = a.QuoteTemplatePDFMarginBottom,
                                                  QuoteTemplatePDFMarginTop = a.QuoteTemplatePDFMarginTop,
                                                  ShowIncludedChargesContainers = a.ShowIncludedChargesContainers,
                                                  ShowIncludedChargesPackages = a.ShowIncludedChargesPackages,
                                                  ShowIncludedChargesPerContainers = a.ShowIncludedChargesPerContainers,
                                                  ShowVATPercentageContainers = a.ShowVATPercentageContainers,
                                                  ShowVATPercentagePackages = a.ShowVATPercentagePackages, 
                                                  ShowVATTypeContainers =a.ShowVATTypeContainers,
                                                  ShowVATTypePackages =a.ShowVATTypePackages,
                                                  PageNumberingTextDesignId = a.PageNumberingTextDesignId,
                                                  HidePageNumber = a.HidePageNumber,
                                              }).FirstOrDefault();

            return entity;
         
        }

        public IQueryable<QuoteTemplateSettingPM> GetQuoteTemplateSettingPMsByTenant(int tenant)
        {
            IQueryable<QuoteTemplateSettingPM> qUoteTemplateSetting = from a in repository.quotesContext.QuoteTemplateSettings
                                                   where a.Tenant == tenant
                                                                      select new QuoteTemplateSettingPM()
                                                   {
                                                       Id = a.Id,
                                                       Tenant = a.Tenant,
                                                       PackagesTableDesignId = a.PackagesTableDesignId,
                                                       ContainserTableDesignId = a.ContainserTableDesignId,
                                                       TotalsPackagesLabelDesignId = a.TotalsPackagesLabelDesignId,
                                                       TotalsContainsersLabelDesignId = a.TotalsContainsersLabelDesignId,
                                                       TotalsPackagesValueDesignId = a.TotalsPackagesValueDesignId,
                                                       TotalsContainsersValueDesignId = a.TotalsContainsersValueDesignId,
                                                       AlignRight = a.AlignRight,
                                                       RightToLeft = a.RightToLeft,
                                                       ShowUnitPricePackages = a.ShowUnitPricePackages,
                                                       ShowUnitsPackages = a.ShowUnitsPackages,
                                                       ShowChargeCodePackages = a.ShowChargeCodePackages,
                                                       ShowChargeCodeContainers = a.ShowChargeCodeContainers,
                                                       ShowChargeNamePackages = a.ShowChargeNamePackages,
                                                       ShowChargeNameContainers = a.ShowChargeNameContainers,
                                                       ShowMeasurementPackages = a.ShowMeasurementPackages,
                                                       ShowMeasurementContainers = a.ShowMeasurementContainers,
                                                       ShowContainerNameInsteadOfCodeContainers = a.ShowContainerNameInsteadOfCodeContainers,
                                                       ShowFixedPriceContainers = a.ShowFixedPriceContainers,
                                                       ShowSaleCurrencyColumnPackages = a.ShowSaleCurrencyColumnPackages,
                                                       ShowSaleCurrencyColumnContainers = a.ShowSaleCurrencyColumnContainers,
                                                       ShowLocalCurrencyColumnContainers = a.ShowLocalCurrencyColumnContainers,
                                                       ShowLocalCurrencyColumnPackages = a.ShowLocalCurrencyColumnPackages,
                                                       SplitChargesbyGroupsPackages = a.SplitChargesbyGroupsPackages,
                                                       SplitChargesbyGroupsContainers = a.SplitChargesbyGroupsContainers,
                                                       ShowLocalLanguage = a.ShowLocalLanguage,
                                                       ShowTotalInLocalCurrencyPackages = a.ShowTotalInLocalCurrencyPackages,
                                                       ShowTotalInLocalCurrencyContainers = a.ShowTotalInLocalCurrencyContainers,
                                                       ShowTotalInSaleCurrencyContainers = a.ShowTotalInSaleCurrencyContainers,
                                                       ShowPricesTablePackages = a.ShowPricesTablePackages,
                                                       ShowPricesTableContainers = a.ShowPricesTableContainers,
                                                       ShowTotalInSaleCurrencyPackages = a.ShowTotalInSaleCurrencyPackages,
                                                       GroupByPackagesLabelDesignId = a.GroupByPackagesLabelDesignId,
                                                       GroupByPackagesValueDesignId = a.GroupByPackagesValueDesignId,
                                                       GroupByContainsersLabelDesignId = a.GroupByContainsersLabelDesignId,
                                                       GroupByContainsersValueDesignId = a.GroupByContainsersValueDesignId,
                                                       DetailsTableDesignId = a.DetailsTableDesignId,
                                                       DetailsSectionHasTwoColumns = a.DetailsSectionHasTwoColumns,
                                                       ShowTitleQuoteDetails = a.ShowTitleQuoteDetails,
                                                       HeaderTableDesignId = a.HeaderTableDesignId,
                                                       HeaderSectionHasTwoColumns = a.HeaderSectionHasTwoColumns,
                                                       ShowHeaderQuoteDate = a.ShowHeaderQuoteDate,
                                                       ShowHeaderExpirationDate = a.ShowHeaderExpirationDate,
                                                       ShowHeaderQuoteNumber = a.ShowHeaderQuoteNumber,
                                                       ShowHeaderCustomer = a.ShowHeaderCustomer,
                                                       ShowDetailsCustomerName = a.ShowDetailsCustomerName,
                                                       ShowDetailsCustomerAddress = a.ShowDetailsCustomerAddress,
                                                       ShowDetailsCustomerContact = a.ShowDetailsCustomerContact,
                                                       ShowDetailsCustomerReferences = a.ShowDetailsCustomerReferences,
                                                       ShowDetailsExpirationDate = a.ShowDetailsExpirationDate,
                                                       ShowDetailsExpirationDays = a.ShowDetailsExpirationDays,
                                                       ShowDetailsShipperName = a.ShowDetailsShipperName,
                                                       ShowDetailsShipperAddress = a.ShowDetailsShipperAddress,
                                                       ShowDetailsShipperContact = a.ShowDetailsShipperContact,
                                                       ShowDetailsShipperReferences = a.ShowDetailsShipperReferences,
                                                       ShowDetailsConsigneeName = a.ShowDetailsConsigneeName,
                                                       ShowDetailsConsigneeAddress = a.ShowDetailsConsigneeAddress,
                                                       ShowDetailsConsigneeContact = a.ShowDetailsConsigneeContact,
                                                       ShowDetailsConsigneeReferences = a.ShowDetailsConsigneeReferences,
                                                       ShowDetailsPickupFrom = a.ShowDetailsPickupFrom,
                                                       ShowDetailsDeliveryTo = a.ShowDetailsDeliveryTo,
                                                       ShowDetailsFromPort = a.ShowDetailsFromPort,
                                                       ShowDetailsToPort = a.ShowDetailsToPort,
                                                       ShowDetailsSalesMan = a.ShowDetailsSalesMan,
                                                       ShowDetailsIncoterms = a.ShowDetailsIncoterms,
                                                       ShowDetailsService = a.ShowDetailsService,
                                                       ShowDetailsDescriptionOfGoods = a.ShowDetailsDescriptionOfGoods,
                                                       ShowDetailsDangerousGoods = a.ShowDetailsDangerousGoods,
                                                       ShowDetailsCarrier = a.ShowDetailsCarrier,
                                                       DetailsTitleDesignId = a.DetailsTitleDesignId,



                                                       PageHeaderArea1Type = a.PageHeaderArea1Type,
                                                       PageHeaderArea2Type = a.PageHeaderArea2Type,
                                                       PageHeaderArea3Type = a.PageHeaderArea3Type,

                                                       PageHeaderArea1ImageDetailId = a.PageHeaderArea1ImageDetailId,
                                                       PageHeaderArea2ImageDetailId = a.PageHeaderArea2ImageDetailId,
                                                       PageHeaderArea3ImageDetailId = a.PageHeaderArea3ImageDetailId,

                                                       PageHeaderArea1FreeText = a.PageHeaderArea1FreeText,
                                                       PageHeaderArea2FreeText = a.PageHeaderArea2FreeText,
                                                       PageHeaderArea3FreeText = a.PageHeaderArea3FreeText,

                                                       PageHeaderArea1FreeTextDesignId = a.PageHeaderArea1FreeTextDesignId,
                                                       PageHeaderArea2FreeTextDesignId = a.PageHeaderArea2FreeTextDesignId,
                                                       PageHeaderArea3FreeTextDesignId = a.PageHeaderArea3FreeTextDesignId,
                                                       PageHeaderArea1Width = a.PageHeaderArea1Width,
                                                       PageHeaderArea2Width = a.PageHeaderArea2Width,
                                                       PageHeaderArea3Width = a.PageHeaderArea3Width,

                                                       PageHeaderArea1Height = a.PageHeaderArea1Height,
                                                       PageHeaderArea2Height = a.PageHeaderArea2Height,
                                                       PageHeaderArea3Height = a.PageHeaderArea3Height,

                                                       PageHeaderArea1ImageAlignment = a.PageHeaderArea1ImageAlignment,
                                                       PageHeaderArea2ImageAlignment = a.PageHeaderArea2ImageAlignment,
                                                       PageHeaderArea3ImageAlignment = a.PageHeaderArea3ImageAlignment,





                                                       PageFooterArea1Type = a.PageFooterArea1Type,
                                                       PageFooterArea2Type = a.PageFooterArea2Type,
                                                       PageFooterArea3Type = a.PageFooterArea3Type,

                                                       PageFooterArea1ImageDetailId = a.PageFooterArea1ImageDetailId,
                                                       PageFooterArea2ImageDetailId = a.PageFooterArea2ImageDetailId,
                                                       PageFooterArea3ImageDetailId = a.PageFooterArea3ImageDetailId,

                                                       PageFooterArea1FreeText = a.PageFooterArea1FreeText,
                                                       PageFooterArea2FreeText = a.PageFooterArea2FreeText,
                                                       PageFooterArea3FreeText = a.PageFooterArea3FreeText,

                                                       PageFooterArea1FreeTextDesignId = a.PageFooterArea1FreeTextDesignId,
                                                       PageFooterArea2FreeTextDesignId = a.PageFooterArea2FreeTextDesignId,
                                                       PageFooterArea3FreeTextDesignId = a.PageFooterArea3FreeTextDesignId,
                                                       PageFooterArea1Width = a.PageFooterArea1Width,
                                                       PageFooterArea2Width = a.PageFooterArea2Width,
                                                       PageFooterArea3Width = a.PageFooterArea3Width,

                                                       PageFooterArea1Height = a.PageFooterArea1Height,
                                                       PageFooterArea2Height = a.PageFooterArea2Height,
                                                       PageFooterArea3Height = a.PageFooterArea3Height,

                                                       PageFooterArea1ImageAlignment = a.PageFooterArea1ImageAlignment,
                                                       PageFooterArea2ImageAlignment = a.PageFooterArea2ImageAlignment,
                                                       PageFooterArea3ImageAlignment = a.PageFooterArea3ImageAlignment,


                                                       PricingPackagesTitleDesignId = a.PricingPackagesTitleDesignId,
                                                       PricingContainsersTitleDesignId = a.PricingContainsersTitleDesignId,
                                                       ShowTitlePricingPackages = a.ShowTitlePricingPackages,
                                                       ShowTitlePricingContainsers = a.ShowTitlePricingContainsers,




                                                       PageHeaderAreaHeight = a.PageHeaderAreaHeight,
                                                       PageFooterAreaHeight = a.PageFooterAreaHeight,
                                                       PageHeaderImage1Width = a.PageHeaderImage1Width,
                                                       PageHeaderImage2Width = a.PageHeaderImage2Width,


                                                       PageHeaderImage3Width = a.PageHeaderImage3Width,
                                                       PageFooterImage1Width = a.PageFooterImage1Width,
                                                       PageFooterImage2Width = a.PageFooterImage2Width,
                                                       PageFooterImage3Width = a.PageFooterImage3Width,



                                                       PageFooterBorderTypeCode = a.PageFooterBorderTypeCode,
                                                       PageFooterBorderColor = a.PageFooterBorderColor,
                                                       PageFooterBorderThickness = a.PageFooterBorderThickness,



                                                       PageHeaderBorderTypeCode = a.PageHeaderBorderTypeCode,
                                                       PageHeaderBorderColor = a.PageHeaderBorderColor,
                                                       PageHeaderBorderThickness = a.PageHeaderBorderThickness,

                                                       DetailsTableColumWidthType = a.DetailsTableColumWidthType,
                                                       HeaderTableColumWidthType = a.HeaderTableColumWidthType,

                                                       DetailsTableColumn1LabelWidth = a.DetailsTableColumn1LabelWidth,
                                                       DetailsTableColumn1ValueWidth = a.DetailsTableColumn1ValueWidth,
                                                       DetailsTableColumn2LabelWidth = a.DetailsTableColumn2LabelWidth,
                                                       DetailsTableColumn2ValueWidth = a.DetailsTableColumn2ValueWidth,

                                                       HeaderTableColumn1LabelWidth = a.HeaderTableColumn1LabelWidth,
                                                       HeaderTableColumn1ValueWidth = a.HeaderTableColumn1ValueWidth,
                                                       HeaderTableColumn2LabelWidth = a.HeaderTableColumn2LabelWidth,
                                                       HeaderTableColumn2ValueWidth = a.HeaderTableColumn2ValueWidth,

                                                       QuoteTemplatePDFMarginRight = a.QuoteTemplatePDFMarginRight,
                                                       QuoteTemplatePDFMarginLeft = a.QuoteTemplatePDFMarginLeft,

                                                       ShowCodeChargeSaleMinMaxContainers = a.ShowCodeChargeSaleMinMaxContainers,
                                                       ShowCodeChargeSaleMinMaxPackages = a.ShowCodeChargeSaleMinMaxPackages,

                                                       ShowPriceByContainerColumn = a.ShowPriceByContainerColumn,
                                                       ShowChargeDescriptionContainers = a.ShowChargeDescriptionContainers,
                                                       ShowChargeDescriptionPackages = a.ShowChargeDescriptionPackages,
                                                       ShowTotalPerChargeGroupPackages = a.ShowTotalPerChargeGroupPackages,
                                                       ShowTotalPerChargeGroupContainers = a.ShowTotalPerChargeGroupContainers,
                                                       ShowPageBreakBeforeTotalPerContainersTable = a.ShowPageBreakBeforeTotalPerContainersTable,
                                                       TotalPerContainersCurrencyType = a.TotalPerContainersCurrencyType,
                                                       TotalPerContainersAdditionalTextDesignId = a.TotalPerContainersAdditionalTextDesignId,
                                                       TotalPerContainersTableDesignId = a.TotalPerContainersTableDesignId,
                                                       ShowTitleTotalPerContainersTable = a.ShowTitleTotalPerContainersTable,
                                                       ShowChargeNoteContainers = a.ShowChargeNoteContainers,
                                                       ShowChargeNotePackages = a.ShowChargeNotePackages,
                                                       ShowSaleMaxMinAmountContainers = a.ShowSaleMaxMinAmountContainers,
                                                       ShowSaleMaxMinAmountPackages = a.ShowSaleMaxMinAmountPackages,
                                                       ShowHeaderLabelsContainers = a.ShowHeaderLabelsContainers,
                                                       ShowHeaderLabelsPackages = a.ShowHeaderLabelsPackages,
                                                       SpaceLinesBeforeContainers = a.SpaceLinesBeforeContainers,
                                                       SpaceLinesBeforeFooters = a.SpaceLinesBeforeFooters,
                                                       SpaceLinesBeforeHeaders = a.SpaceLinesBeforeHeaders,
                                                       SpaceLinesBeforePackages = a.SpaceLinesBeforePackages,
                                                       SpaceLinesBeforeQuoteDetails = a.SpaceLinesBeforeQuoteDetails,
                                                       SpaceLinesBeforeQuoteHeaders = a.SpaceLinesBeforeQuoteHeaders,
                                                       SpaceLinesBeforePerContainers = a.SpaceLinesBeforePerContainers,
                                                       QuoteTemplatePDFMarginBottom = a.QuoteTemplatePDFMarginBottom,
                                                       QuoteTemplatePDFMarginTop = a.QuoteTemplatePDFMarginTop,
                                                       ShowIncludedChargesContainers = a.ShowIncludedChargesContainers,
                                                       ShowIncludedChargesPackages = a.ShowIncludedChargesPackages,
                                                       ShowIncludedChargesPerContainers = a.ShowIncludedChargesPerContainers,
                                                       ShowVATPercentageContainers = a.ShowVATPercentageContainers,
                                                       ShowVATPercentagePackages = a.ShowVATPercentagePackages,
                                                       ShowVATTypeContainers = a.ShowVATTypeContainers,
                                                       ShowVATTypePackages = a.ShowVATTypePackages,
                                                       PageNumberingTextDesignId = a.PageNumberingTextDesignId,
                                                       HidePageNumber = a.HidePageNumber,

                                                                      };
            return qUoteTemplateSetting;
        }

        public IQueryable<QuoteTemplateSettingList> GetIQueryableEntityList(IQueryable<QuoteTemplateSetting> iQueryable)
        {
            IQueryable<QuoteTemplateSettingList> result = from quoteTemplateSetting in iQueryable
                                                          select new QuoteTemplateSettingList()
                                                {
                                                    Id = quoteTemplateSetting.Id,
                                                    Tenant = quoteTemplateSetting.Tenant,
                                                    PackagesTableDesignId = quoteTemplateSetting.PackagesTableDesignId,
                                                    ContainserTableDesignId = quoteTemplateSetting.ContainserTableDesignId,
                                                    TotalsPackagesLabelDesignId = quoteTemplateSetting.TotalsPackagesLabelDesignId,
                                                    TotalsContainsersLabelDesignId = quoteTemplateSetting.TotalsContainsersLabelDesignId,
                                                    TotalsPackagesValueDesignId = quoteTemplateSetting.TotalsPackagesValueDesignId,
                                                    TotalsContainsersValueDesignId = quoteTemplateSetting.TotalsContainsersValueDesignId,
                                                    AlignRight = quoteTemplateSetting.AlignRight,
                                                    ShowUnitPricePackages = quoteTemplateSetting.ShowUnitPricePackages,
                                                    ShowUnitsPackages = quoteTemplateSetting.ShowUnitsPackages,
                                                    ShowChargeCodePackages = quoteTemplateSetting.ShowChargeCodePackages,
                                                    ShowChargeCodeContainers = quoteTemplateSetting.ShowChargeCodeContainers,
                                                    ShowChargeNamePackages = quoteTemplateSetting.ShowChargeNamePackages,
                                                    ShowChargeNameContainers = quoteTemplateSetting.ShowChargeNameContainers,
                                                    ShowMeasurementPackages = quoteTemplateSetting.ShowMeasurementPackages,
                                                    ShowMeasurementContainers = quoteTemplateSetting.ShowMeasurementContainers,
                                                    ShowContainerNameInsteadOfCodeContainers = quoteTemplateSetting.ShowContainerNameInsteadOfCodeContainers,
                                                    ShowFixedPriceContainers = quoteTemplateSetting.ShowFixedPriceContainers,
                                                    ShowSaleCurrencyColumnPackages = quoteTemplateSetting.ShowSaleCurrencyColumnPackages,
                                                    ShowSaleCurrencyColumnContainers = quoteTemplateSetting.ShowSaleCurrencyColumnContainers,
                                                    ShowLocalCurrencyColumnContainers = quoteTemplateSetting.ShowLocalCurrencyColumnContainers,
                                                    ShowLocalCurrencyColumnPackages = quoteTemplateSetting.ShowLocalCurrencyColumnPackages,
                                                    SplitChargesbyGroupsPackages = quoteTemplateSetting.SplitChargesbyGroupsPackages,
                                                    SplitChargesbyGroupsContainers = quoteTemplateSetting.SplitChargesbyGroupsContainers,
                                                    ShowLocalLanguage = quoteTemplateSetting.ShowLocalLanguage,
                                                    ShowTotalInLocalCurrencyPackages = quoteTemplateSetting.ShowTotalInLocalCurrencyPackages,
                                                    ShowTotalInLocalCurrencyContainers = quoteTemplateSetting.ShowTotalInLocalCurrencyContainers,
                                                    ShowTotalInSaleCurrencyContainers = quoteTemplateSetting.ShowTotalInSaleCurrencyContainers,
                                                    ShowPricesTablePackages = quoteTemplateSetting.ShowPricesTablePackages,
                                                    ShowPricesTableContainers = quoteTemplateSetting.ShowPricesTableContainers,
                                                    ShowTotalInSaleCurrencyPackages = quoteTemplateSetting.ShowTotalInSaleCurrencyPackages,
                                                    RightToLeft = quoteTemplateSetting.RightToLeft,
                                                    GroupByPackagesLabelDesignId = quoteTemplateSetting.GroupByPackagesLabelDesignId,
                                                    GroupByPackagesValueDesignId = quoteTemplateSetting.GroupByPackagesValueDesignId,
                                                    GroupByContainsersLabelDesignId = quoteTemplateSetting.GroupByContainsersLabelDesignId,
                                                    GroupByContainsersValueDesignId = quoteTemplateSetting.GroupByContainsersValueDesignId,
                                                    DetailsTableDesignId = quoteTemplateSetting.DetailsTableDesignId,
                                                    DetailsSectionHasTwoColumns = quoteTemplateSetting.DetailsSectionHasTwoColumns,
                                                    HeaderTableDesignId = quoteTemplateSetting.HeaderTableDesignId,
                                                    HeaderSectionHasTwoColumns = quoteTemplateSetting.HeaderSectionHasTwoColumns,
                                                    ShowTitleQuoteDetails = quoteTemplateSetting.ShowTitleQuoteDetails,
                                                    ShowHeaderQuoteDate = quoteTemplateSetting.ShowHeaderQuoteDate,
                                                    ShowHeaderExpirationDate = quoteTemplateSetting.ShowHeaderExpirationDate,
                                                    ShowHeaderQuoteNumber = quoteTemplateSetting.ShowHeaderQuoteNumber,
                                                    ShowHeaderCustomer = quoteTemplateSetting.ShowHeaderCustomer,
                                                    ShowDetailsExpirationDate = quoteTemplateSetting.ShowDetailsExpirationDate,
                                                    ShowDetailsExpirationDays = quoteTemplateSetting.ShowDetailsExpirationDays,
                                                    ShowDetailsShipperName = quoteTemplateSetting.ShowDetailsShipperName,
                                                    ShowDetailsShipperAddress = quoteTemplateSetting.ShowDetailsShipperAddress,
                                                    ShowDetailsShipperContact = quoteTemplateSetting.ShowDetailsShipperContact,
                                                    ShowDetailsShipperReferences = quoteTemplateSetting.ShowDetailsShipperReferences,
                                                    ShowDetailsConsigneeName = quoteTemplateSetting.ShowDetailsConsigneeName,
                                                    ShowDetailsConsigneeAddress = quoteTemplateSetting.ShowDetailsConsigneeAddress,
                                                    ShowDetailsConsigneeContact = quoteTemplateSetting.ShowDetailsConsigneeContact,
                                                    ShowDetailsConsigneeReferences = quoteTemplateSetting.ShowDetailsConsigneeReferences,
                                                    ShowDetailsPickupFrom = quoteTemplateSetting.ShowDetailsPickupFrom,
                                                    ShowDetailsDeliveryTo = quoteTemplateSetting.ShowDetailsDeliveryTo,
                                                    ShowDetailsFromPort = quoteTemplateSetting.ShowDetailsFromPort,
                                                    ShowDetailsToPort = quoteTemplateSetting.ShowDetailsToPort,
                                                    ShowDetailsSalesMan = quoteTemplateSetting.ShowDetailsSalesMan,
                                                    ShowDetailsIncoterms = quoteTemplateSetting.ShowDetailsIncoterms,
                                                    ShowDetailsService = quoteTemplateSetting.ShowDetailsService,
                                                    ShowDetailsDescriptionOfGoods = quoteTemplateSetting.ShowDetailsDescriptionOfGoods,
                                                    ShowDetailsDangerousGoods = quoteTemplateSetting.ShowDetailsDangerousGoods,
                                                    ShowDetailsCarrier = quoteTemplateSetting.ShowDetailsCarrier,
                                                    DetailsTitleDesignId = quoteTemplateSetting.DetailsTitleDesignId,
                                                    ShowDetailsCustomerName = quoteTemplateSetting.ShowDetailsCustomerName,
                                                    ShowDetailsCustomerAddress = quoteTemplateSetting.ShowDetailsCustomerAddress,
                                                    ShowDetailsCustomerContact = quoteTemplateSetting.ShowDetailsCustomerContact,
                                                    ShowDetailsCustomerReferences = quoteTemplateSetting.ShowDetailsCustomerReferences,



                                                    PageHeaderArea1Type = quoteTemplateSetting.PageHeaderArea1Type,
                                                    PageHeaderArea2Type = quoteTemplateSetting.PageHeaderArea2Type,
                                                    PageHeaderArea3Type = quoteTemplateSetting.PageHeaderArea3Type,

                                                    PageHeaderArea1ImageDetailId = quoteTemplateSetting.PageHeaderArea1ImageDetailId,
                                                    PageHeaderArea2ImageDetailId = quoteTemplateSetting.PageHeaderArea2ImageDetailId,
                                                    PageHeaderArea3ImageDetailId = quoteTemplateSetting.PageHeaderArea3ImageDetailId,

                                                    PageHeaderArea1FreeText = quoteTemplateSetting.PageHeaderArea1FreeText,
                                                    PageHeaderArea2FreeText = quoteTemplateSetting.PageHeaderArea2FreeText,
                                                    PageHeaderArea3FreeText = quoteTemplateSetting.PageHeaderArea3FreeText,

                                                    PageHeaderArea1FreeTextDesignId = quoteTemplateSetting.PageHeaderArea1FreeTextDesignId,
                                                    PageHeaderArea2FreeTextDesignId = quoteTemplateSetting.PageHeaderArea2FreeTextDesignId,
                                                    PageHeaderArea3FreeTextDesignId = quoteTemplateSetting.PageHeaderArea3FreeTextDesignId,
                                                    PageHeaderArea1Width = quoteTemplateSetting.PageHeaderArea1Width,
                                                    PageHeaderArea2Width = quoteTemplateSetting.PageHeaderArea2Width,
                                                    PageHeaderArea3Width = quoteTemplateSetting.PageHeaderArea3Width,

                                                    PageHeaderArea1Height = quoteTemplateSetting.PageHeaderArea1Height,
                                                    PageHeaderArea2Height = quoteTemplateSetting.PageHeaderArea2Height,
                                                    PageHeaderArea3Height = quoteTemplateSetting.PageHeaderArea3Height,

                                                    PageHeaderArea1ImageAlignment = quoteTemplateSetting.PageHeaderArea1ImageAlignment,
                                                    PageHeaderArea2ImageAlignment = quoteTemplateSetting.PageHeaderArea2ImageAlignment,
                                                    PageHeaderArea3ImageAlignment = quoteTemplateSetting.PageHeaderArea3ImageAlignment,


                                                    PageFooterArea1Type = quoteTemplateSetting.PageFooterArea1Type,
                                                    PageFooterArea2Type = quoteTemplateSetting.PageFooterArea2Type,
                                                    PageFooterArea3Type = quoteTemplateSetting.PageFooterArea3Type,

                                                    PageFooterArea1ImageDetailId = quoteTemplateSetting.PageFooterArea1ImageDetailId,
                                                    PageFooterArea2ImageDetailId = quoteTemplateSetting.PageFooterArea2ImageDetailId,
                                                    PageFooterArea3ImageDetailId = quoteTemplateSetting.PageFooterArea3ImageDetailId,

                                                    PageFooterArea1FreeText = quoteTemplateSetting.PageFooterArea1FreeText,
                                                    PageFooterArea2FreeText = quoteTemplateSetting.PageFooterArea2FreeText,
                                                    PageFooterArea3FreeText = quoteTemplateSetting.PageFooterArea3FreeText,

                                                    PageFooterArea1FreeTextDesignId = quoteTemplateSetting.PageFooterArea1FreeTextDesignId,
                                                    PageFooterArea2FreeTextDesignId = quoteTemplateSetting.PageFooterArea2FreeTextDesignId,
                                                    PageFooterArea3FreeTextDesignId = quoteTemplateSetting.PageFooterArea3FreeTextDesignId,
                                                    PageFooterArea1Width = quoteTemplateSetting.PageFooterArea1Width,
                                                    PageFooterArea2Width = quoteTemplateSetting.PageFooterArea2Width,
                                                    PageFooterArea3Width = quoteTemplateSetting.PageFooterArea3Width,

                                                    PageFooterArea1Height = quoteTemplateSetting.PageFooterArea1Height,
                                                    PageFooterArea2Height = quoteTemplateSetting.PageFooterArea2Height,
                                                    PageFooterArea3Height = quoteTemplateSetting.PageFooterArea3Height,

                                                    PageFooterArea1ImageAlignment = quoteTemplateSetting.PageFooterArea1ImageAlignment,
                                                    PageFooterArea2ImageAlignment = quoteTemplateSetting.PageFooterArea2ImageAlignment,
                                                    PageFooterArea3ImageAlignment = quoteTemplateSetting.PageFooterArea3ImageAlignment,



                                                    PricingPackagesTitleDesignId = quoteTemplateSetting.PricingPackagesTitleDesignId,
                                                    PricingContainsersTitleDesignId = quoteTemplateSetting.PricingContainsersTitleDesignId,
                                                    ShowTitlePricingPackages = quoteTemplateSetting.ShowTitlePricingPackages,
                                                    ShowTitlePricingContainsers = quoteTemplateSetting.ShowTitlePricingContainsers,


                                                    PageHeaderAreaHeight = quoteTemplateSetting.PageHeaderAreaHeight,
                                                    PageFooterAreaHeight = quoteTemplateSetting.PageFooterAreaHeight,
                                                    PageHeaderImage1Width = quoteTemplateSetting.PageHeaderImage1Width,
                                                    PageHeaderImage2Width = quoteTemplateSetting.PageHeaderImage2Width,


                                                    PageHeaderImage3Width = quoteTemplateSetting.PageHeaderImage3Width,
                                                    PageFooterImage1Width = quoteTemplateSetting.PageFooterImage1Width,
                                                    PageFooterImage2Width = quoteTemplateSetting.PageFooterImage2Width,
                                                    PageFooterImage3Width = quoteTemplateSetting.PageFooterImage3Width,


                                                    PageFooterBorderTypeCode = quoteTemplateSetting.PageFooterBorderTypeCode,
                                                    PageFooterBorderColor = quoteTemplateSetting.PageFooterBorderColor,
                                                    PageFooterBorderThickness = quoteTemplateSetting.PageFooterBorderThickness,



                                                    PageHeaderBorderTypeCode = quoteTemplateSetting.PageHeaderBorderTypeCode,
                                                    PageHeaderBorderColor = quoteTemplateSetting.PageHeaderBorderColor,
                                                    PageHeaderBorderThickness = quoteTemplateSetting.PageHeaderBorderThickness,

                                                    DetailsTableColumWidthType = quoteTemplateSetting.DetailsTableColumWidthType,
                                                    HeaderTableColumWidthType = quoteTemplateSetting.HeaderTableColumWidthType,

                                                    DetailsTableColumn1LabelWidth = quoteTemplateSetting.DetailsTableColumn1LabelWidth,
                                                    DetailsTableColumn1ValueWidth = quoteTemplateSetting.DetailsTableColumn1ValueWidth,
                                                    DetailsTableColumn2LabelWidth = quoteTemplateSetting.DetailsTableColumn2LabelWidth,
                                                    DetailsTableColumn2ValueWidth = quoteTemplateSetting.DetailsTableColumn2ValueWidth,

                                                    HeaderTableColumn1LabelWidth = quoteTemplateSetting.HeaderTableColumn1LabelWidth,
                                                    HeaderTableColumn1ValueWidth = quoteTemplateSetting.HeaderTableColumn1ValueWidth,
                                                    HeaderTableColumn2LabelWidth = quoteTemplateSetting.HeaderTableColumn2LabelWidth,
                                                    HeaderTableColumn2ValueWidth = quoteTemplateSetting.HeaderTableColumn2ValueWidth,


                                                    QuoteTemplatePDFMarginRight = quoteTemplateSetting.QuoteTemplatePDFMarginRight,
                                                    QuoteTemplatePDFMarginLeft = quoteTemplateSetting.QuoteTemplatePDFMarginLeft,

                                                    ShowCodeChargeSaleMinMaxContainers = quoteTemplateSetting.ShowCodeChargeSaleMinMaxContainers,
                                                    ShowCodeChargeSaleMinMaxPackages = quoteTemplateSetting.ShowCodeChargeSaleMinMaxPackages,

                                                    ShowPriceByContainerColumn = quoteTemplateSetting.ShowPriceByContainerColumn,
                                                    ShowChargeDescriptionContainers = quoteTemplateSetting.ShowChargeDescriptionContainers,
                                                    ShowChargeDescriptionPackages = quoteTemplateSetting.ShowChargeDescriptionPackages,
                                                    ShowTotalPerChargeGroupPackages = quoteTemplateSetting.ShowTotalPerChargeGroupPackages,
                                                    ShowTotalPerChargeGroupContainers = quoteTemplateSetting.ShowTotalPerChargeGroupContainers,
                                                    ShowPageBreakBeforeTotalPerContainersTable = quoteTemplateSetting.ShowPageBreakBeforeTotalPerContainersTable,
                                                    TotalPerContainersCurrencyType = quoteTemplateSetting.TotalPerContainersCurrencyType,
                                                    TotalPerContainersAdditionalTextDesignId = quoteTemplateSetting.TotalPerContainersAdditionalTextDesignId,
                                                    TotalPerContainersTableDesignId = quoteTemplateSetting.TotalPerContainersTableDesignId,
                                                    ShowTitleTotalPerContainersTable = quoteTemplateSetting.ShowTitleTotalPerContainersTable,
                                                    ShowChargeNoteContainers = quoteTemplateSetting.ShowChargeNoteContainers,
                                                    ShowChargeNotePackages = quoteTemplateSetting.ShowChargeNotePackages,
                                                    ShowSaleMaxMinAmountContainers = quoteTemplateSetting.ShowSaleMaxMinAmountContainers,
                                                    ShowSaleMaxMinAmountPackages = quoteTemplateSetting.ShowSaleMaxMinAmountPackages,
                                                    ShowHeaderLabelsContainers = quoteTemplateSetting.ShowHeaderLabelsContainers,
                                                    ShowHeaderLabelsPackages = quoteTemplateSetting.ShowHeaderLabelsPackages,
                                                    SpaceLinesBeforeContainers = quoteTemplateSetting.SpaceLinesBeforeContainers,
                                                    SpaceLinesBeforeFooters = quoteTemplateSetting.SpaceLinesBeforeFooters,
                                                    SpaceLinesBeforeHeaders = quoteTemplateSetting.SpaceLinesBeforeHeaders,
                                                    SpaceLinesBeforePackages = quoteTemplateSetting.SpaceLinesBeforePackages,
                                                    SpaceLinesBeforeQuoteDetails = quoteTemplateSetting.SpaceLinesBeforeQuoteDetails,
                                                    SpaceLinesBeforeQuoteHeaders = quoteTemplateSetting.SpaceLinesBeforeQuoteHeaders,
                                                    SpaceLinesBeforePerContainers = quoteTemplateSetting.SpaceLinesBeforePerContainers,
                                                    QuoteTemplatePDFMarginBottom = quoteTemplateSetting.QuoteTemplatePDFMarginBottom,
                                                    QuoteTemplatePDFMarginTop = quoteTemplateSetting.QuoteTemplatePDFMarginTop,
                                                    ShowIncludedChargesContainers = quoteTemplateSetting.ShowIncludedChargesContainers,
                                                    ShowIncludedChargesPackages = quoteTemplateSetting.ShowIncludedChargesPackages,
                                                    ShowIncludedChargesPerContainers = quoteTemplateSetting.ShowIncludedChargesPerContainers,
                                                    ShowVATPercentageContainers = quoteTemplateSetting.ShowVATPercentageContainers,
                                                    ShowVATPercentagePackages = quoteTemplateSetting.ShowVATPercentagePackages,
                                                    ShowVATTypeContainers = quoteTemplateSetting.ShowVATTypeContainers,
                                                    ShowVATTypePackages = quoteTemplateSetting.ShowVATTypePackages,
                                                    PageNumberingTextDesignId = quoteTemplateSetting.PageNumberingTextDesignId,
                                                    HidePageNumber = quoteTemplateSetting.HidePageNumber,
                                                          };
            return result;
        }

        

        public QuoteTemplateSetting GetFirstQuoteTemplateSettingForTenant(int tenant)
        {
            return (from a in repository.quotesContext.QuoteTemplateSettings
                    where a.Tenant == tenant
                    select a).FirstOrDefault();
        }
    }
}