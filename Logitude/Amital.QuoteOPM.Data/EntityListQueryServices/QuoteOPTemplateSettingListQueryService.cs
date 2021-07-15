	using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Data.EntityLists;

namespace Amital.QuoteOPM.Data.EntityListQueryServices
{ 

    public partial class QuoteOPTemplateSettingListQueryService
    {
	    private IQueryable<QuoteOPTemplateSettingList> GetIqueryableList(IQueryable<QuoteOPTemplateSetting> iQueryable)
        {
		IQueryable<QuoteOPTemplateSettingList> query = (from a in iQueryable
                                            select new QuoteOPTemplateSettingList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          ShowTotalInSaleCurrencyPackages = a.ShowTotalInSaleCurrencyPackages,
					
					                          ShowTotalInSaleCurrencyContainers = a.ShowTotalInSaleCurrencyContainers,
					
					                          ShowTotalInLocalCurrencyPackages = a.ShowTotalInLocalCurrencyPackages,
					
					                          ShowTotalInLocalCurrencyContainers = a.ShowTotalInLocalCurrencyContainers,
					
					                          ShowPricesTablePackages = a.ShowPricesTablePackages,
					
					                          ShowPricesTableContainers = a.ShowPricesTableContainers,
					
					                          ShowChargeCodePackages = a.ShowChargeCodePackages,
					
					                          ShowChargeDescriptionPackages = a.ShowChargeDescriptionPackages,
					
					                          ShowChargeDescriptionContainers = a.ShowChargeDescriptionContainers,
					
					                          ShowChargeCodeContainers = a.ShowChargeCodeContainers,
					
					                          ShowChargeNamePackages = a.ShowChargeNamePackages,
					
					                          ShowChargeNameContainers = a.ShowChargeNameContainers,
					
					                          ShowMeasurementPackages = a.ShowMeasurementPackages,
					
					                          ShowMeasurementContainers = a.ShowMeasurementContainers,
					
					                          ShowFixedPriceContainers = a.ShowFixedPriceContainers,
					
					                          ShowUnitsPackages = a.ShowUnitsPackages,
					
					                          ShowUnitPricePackages = a.ShowUnitPricePackages,
					
					                          ShowLocalCurrencyColumnPackages = a.ShowLocalCurrencyColumnPackages,
					
					                          ShowLocalCurrencyColumnContainers = a.ShowLocalCurrencyColumnContainers,
					
					                          ShowSaleCurrencyColumnPackages = a.ShowSaleCurrencyColumnPackages,
					
					                          ShowSaleCurrencyColumnContainers = a.ShowSaleCurrencyColumnContainers,
					
					                          ShowLocalLanguage = a.ShowLocalLanguage,
					
					                          SplitChargesbyGroupsPackages = a.SplitChargesbyGroupsPackages,
					
					                          SplitChargesbyGroupsContainers = a.SplitChargesbyGroupsContainers,
					
					                          ShowContainerNameInsteadOfCodeContainers = a.ShowContainerNameInsteadOfCodeContainers,
					
					                          AlignRight = a.AlignRight,
					
					                          ShowPriceByContainerColumn = a.ShowPriceByContainerColumn,
					
					                          ShowCodeChargeSaleMinMaxContainers = a.ShowCodeChargeSaleMinMaxContainers,
					
					                          ShowCodeChargeSaleMinMaxPackages = a.ShowCodeChargeSaleMinMaxPackages,
					
					                          QuoteTemplatePDFMarginLeft = a.QuoteTemplatePDFMarginLeft,
					
					                          QuoteTemplatePDFMarginRight = a.QuoteTemplatePDFMarginRight,
					
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
					
					                          PageHeaderImage1Width = a.PageHeaderImage1Width,
					
					                          PageHeaderImage2Width = a.PageHeaderImage2Width,
					
					                          PageHeaderImage3Width = a.PageHeaderImage3Width,
					
					                          PageFooterImage1Width = a.PageFooterImage1Width,
					
					                          PageFooterImage2Width = a.PageFooterImage2Width,
					
					                          PageFooterImage3Width = a.PageFooterImage3Width,
					
					                          PageHeaderAreaHeight = a.PageHeaderAreaHeight,
					
					                          PageFooterAreaHeight = a.PageFooterAreaHeight,
					
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
					
					                          ShowDetailsIncoterms = a.ShowDetailsIncoterms,
					
					                          ShowDetailsService = a.ShowDetailsService,
					
					                          ShowDetailsSalesMan = a.ShowDetailsSalesMan,
					
					                          ShowDetailsDescriptionOfGoods = a.ShowDetailsDescriptionOfGoods,
					
					                          ShowDetailsDangerousGoods = a.ShowDetailsDangerousGoods,
					
					                          ShowDetailsCarrier = a.ShowDetailsCarrier,
					
					                          ShowDetailsCustomerName = a.ShowDetailsCustomerName,
					
					                          ShowDetailsCustomerAddress = a.ShowDetailsCustomerAddress,
					
					                          ShowDetailsCustomerContact = a.ShowDetailsCustomerContact,
					
					                          ShowDetailsCustomerReferences = a.ShowDetailsCustomerReferences,
					
					                          ShowTitleQuoteDetails = a.ShowTitleQuoteDetails,
					
					                          ShowTitlePricingPackages = a.ShowTitlePricingPackages,
					
					                          ShowTitlePricingContainsers = a.ShowTitlePricingContainsers,
					
					                          PackagesTableDesignId = a.PackagesTableDesignId,
					
					                          ContainserTableDesignId = a.ContainserTableDesignId,
					
					                          TotalsPackagesLabelDesignId = a.TotalsPackagesLabelDesignId,
					
					                          TotalsContainsersLabelDesignId = a.TotalsContainsersLabelDesignId,
					
					                          TotalsPackagesValueDesignId = a.TotalsPackagesValueDesignId,
					
					                          TotalsContainsersValueDesignId = a.TotalsContainsersValueDesignId,
					
					                          RightToLeft = a.RightToLeft,
					
					                          GroupByPackagesLabelDesignId = a.GroupByPackagesLabelDesignId,
					
					                          GroupByPackagesValueDesignId = a.GroupByPackagesValueDesignId,
					
					                          GroupByContainsersLabelDesignId = a.GroupByContainsersLabelDesignId,
					
					                          GroupByContainsersValueDesignId = a.GroupByContainsersValueDesignId,
					
					                          DetailsTableDesignId = a.DetailsTableDesignId,
					
					                          DetailsSectionHasTwoColumns = a.DetailsSectionHasTwoColumns,
					
					                          HeaderTableDesignId = a.HeaderTableDesignId,
					
					                          HeaderSectionHasTwoColumns = a.HeaderSectionHasTwoColumns,
					
					                          DetailsTitleDesignId = a.DetailsTitleDesignId,
					
					                          PricingPackagesTitleDesignId = a.PricingPackagesTitleDesignId,
					
					                          PricingContainsersTitleDesignId = a.PricingContainsersTitleDesignId,
					
					                          PageHeaderBorderTypeCode = a.PageHeaderBorderTypeCode,
					
					                          PageHeaderBorderColor = a.PageHeaderBorderColor,
					
					                          PageHeaderBorderThickness = a.PageHeaderBorderThickness,
					
					                          PageFooterBorderTypeCode = a.PageFooterBorderTypeCode,
					
					                          PageFooterBorderColor = a.PageFooterBorderColor,
					
					                          PageFooterBorderThickness = a.PageFooterBorderThickness,
					
					                          HeaderTableColumWidthType = a.HeaderTableColumWidthType,
					
					                          DetailsTableColumWidthType = a.DetailsTableColumWidthType,
					
					                          DetailsTableColumn1LabelWidth = a.DetailsTableColumn1LabelWidth,
					
					                          DetailsTableColumn1ValueWidth = a.DetailsTableColumn1ValueWidth,
					
					                          DetailsTableColumn2LabelWidth = a.DetailsTableColumn2LabelWidth,
					
					                          DetailsTableColumn2ValueWidth = a.DetailsTableColumn2ValueWidth,
					
					                          HeaderTableColumn1LabelWidth = a.HeaderTableColumn1LabelWidth,
					
					                          HeaderTableColumn1ValueWidth = a.HeaderTableColumn1ValueWidth,
					
					                          HeaderTableColumn2LabelWidth = a.HeaderTableColumn2LabelWidth,
					
					                          HeaderTableColumn2ValueWidth = a.HeaderTableColumn2ValueWidth,
					
					                          ShowTotalPerChargeGroupPackages = a.ShowTotalPerChargeGroupPackages,
					
					                          ShowTotalPerChargeGroupContainers = a.ShowTotalPerChargeGroupContainers,
					
					                          ShowPageBreakBeforeTotalPerContainersTable = a.ShowPageBreakBeforeTotalPerContainersTable,
					
					                          TotalPerContainersAdditionalTextDesignId = a.TotalPerContainersAdditionalTextDesignId,
					
					                          TotalPerContainersTableDesignId = a.TotalPerContainersTableDesignId,
					
					                          TotalPerContainersCurrencyType = a.TotalPerContainersCurrencyType,
					
					                          ShowTitleTotalPerContainersTable = a.ShowTitleTotalPerContainersTable,
					
					                          ShowChargeNotePackages = a.ShowChargeNotePackages,
					
					                          ShowChargeNoteContainers = a.ShowChargeNoteContainers,
					
					                          ShowSaleMaxMinAmountPackages = a.ShowSaleMaxMinAmountPackages,
					
					                          ShowSaleMaxMinAmountContainers = a.ShowSaleMaxMinAmountContainers,
					
					                          ShowHeaderLabelsPackages = a.ShowHeaderLabelsPackages,
					
					                          ShowHeaderLabelsContainers = a.ShowHeaderLabelsContainers,
					
					                          SpaceLinesBeforeContainers = a.SpaceLinesBeforeContainers,
					
					                          SpaceLinesBeforePackages = a.SpaceLinesBeforePackages,
					
					                          SpaceLinesBeforeQuoteHeaders = a.SpaceLinesBeforeQuoteHeaders,
					
					                          SpaceLinesBeforeQuoteDetails = a.SpaceLinesBeforeQuoteDetails,
					
					                          SpaceLinesBeforeHeaders = a.SpaceLinesBeforeHeaders,
					
					                          SpaceLinesBeforeFooters = a.SpaceLinesBeforeFooters,
					
					                          SpaceLinesBeforePerContainers = a.SpaceLinesBeforePerContainers,
					
					                          QuoteTemplatePDFMarginTop = a.QuoteTemplatePDFMarginTop,
					
					                          QuoteTemplatePDFMarginBottom = a.QuoteTemplatePDFMarginBottom,
					
					                          ShowIncludedChargesPerContainers = a.ShowIncludedChargesPerContainers,
					
					                          ShowIncludedChargesPackages = a.ShowIncludedChargesPackages,
					
					                          ShowIncludedChargesContainers = a.ShowIncludedChargesContainers,
					
					                          ShowVATTypePackages = a.ShowVATTypePackages,
					
					                          ShowVATTypeContainers = a.ShowVATTypeContainers,
					
					                          ShowVATPercentagePackages = a.ShowVATPercentagePackages,
					
					                          HidePageNumber = a.HidePageNumber,
					
					                          PageNumberingTextDesignId = a.PageNumberingTextDesignId,
					
					                          ShowRegionalTAXPackages = a.ShowRegionalTAXPackages,
					
					                          ShowRegionalTAXContainers = a.ShowRegionalTAXContainers,
					
		                    	            });
            return query;
		}

		private IQueryable<QuoteOPTemplateSetting> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<QuoteOPTemplateSetting> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<QuoteOPTemplateSetting> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<QuoteOPTemplateSetting> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	