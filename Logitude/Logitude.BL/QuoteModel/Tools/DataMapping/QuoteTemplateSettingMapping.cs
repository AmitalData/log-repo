using Simplog.Data.QuoteModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.QuoteModel.EntityPMs;

namespace Logitude.BL.QuoteModel.Tools.DataMapping
{
    public class QuoteTemplateSettingMapping
    {
        internal static void MappingQuoteTemplateSetting(QuoteTemplateSettingPM itemPM, QuoteTemplateSetting itemPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Id = itemPM.Id;
                itemPoco.Tenant = itemPM.Tenant;
            }

            itemPoco.PackagesTableDesignId = itemPM.PackagesTableDesignId;
            itemPoco.ContainserTableDesignId = itemPM.ContainserTableDesignId;
            itemPoco.TotalsPackagesLabelDesignId = itemPM.TotalsPackagesLabelDesignId;
            itemPoco.TotalsContainsersLabelDesignId = itemPM.TotalsContainsersLabelDesignId;

            itemPoco.TotalsPackagesValueDesignId = itemPM.TotalsPackagesValueDesignId;
            itemPoco.TotalsContainsersValueDesignId = itemPM.TotalsContainsersValueDesignId;

            itemPoco.RightToLeft = itemPM.RightToLeft;

            itemPoco.AlignRight = itemPM.AlignRight;
            itemPoco.ShowUnitPricePackages = itemPM.ShowUnitPricePackages;
            itemPoco.ShowUnitsPackages = itemPM.ShowUnitsPackages;
            itemPoco.ShowChargeCodePackages = itemPM.ShowChargeCodePackages;
            itemPoco.ShowChargeCodeContainers = itemPM.ShowChargeCodeContainers;
            itemPoco.ShowChargeNamePackages = itemPM.ShowChargeNamePackages;
            itemPoco.ShowChargeNameContainers = itemPM.ShowChargeNameContainers;
            itemPoco.ShowMeasurementPackages = itemPM.ShowMeasurementPackages;
            itemPoco.ShowMeasurementContainers = itemPM.ShowMeasurementContainers;
            itemPoco.ShowContainerNameInsteadOfCodeContainers = itemPM.ShowContainerNameInsteadOfCodeContainers;
            itemPoco.ShowFixedPriceContainers = itemPM.ShowFixedPriceContainers;
            itemPoco.ShowTitleQuoteDetails = itemPM.ShowTitleQuoteDetails;
            

            itemPoco.ShowSaleCurrencyColumnPackages = itemPM.ShowSaleCurrencyColumnPackages;
            itemPoco.ShowSaleCurrencyColumnContainers = itemPM.ShowSaleCurrencyColumnContainers;
            itemPoco.ShowLocalCurrencyColumnContainers = itemPM.ShowLocalCurrencyColumnContainers;
            itemPoco.ShowLocalCurrencyColumnPackages = itemPM.ShowLocalCurrencyColumnPackages;
            itemPoco.SplitChargesbyGroupsPackages = itemPM.SplitChargesbyGroupsPackages;
            itemPoco.SplitChargesbyGroupsContainers = itemPM.SplitChargesbyGroupsContainers;
            itemPoco.ShowLocalLanguage = itemPM.ShowLocalLanguage;
            itemPoco.ShowTotalInLocalCurrencyPackages = itemPM.ShowTotalInLocalCurrencyPackages;
            itemPoco.ShowTotalInLocalCurrencyContainers = itemPM.ShowTotalInLocalCurrencyContainers;
            itemPoco.ShowTotalInSaleCurrencyContainers = itemPM.ShowTotalInSaleCurrencyContainers;
            itemPoco.ShowTotalInSaleCurrencyPackages = itemPM.ShowTotalInSaleCurrencyPackages;
            itemPoco.ShowPricesTablePackages = itemPM.ShowPricesTablePackages;
            itemPoco.ShowPricesTableContainers = itemPM.ShowPricesTableContainers;

            itemPoco.GroupByPackagesLabelDesignId = itemPM.GroupByPackagesLabelDesignId;
            itemPoco.GroupByPackagesValueDesignId = itemPM.GroupByPackagesValueDesignId;
            itemPoco.GroupByContainsersLabelDesignId = itemPM.GroupByContainsersLabelDesignId;
            itemPoco.GroupByContainsersValueDesignId = itemPM.GroupByContainsersValueDesignId;

            itemPoco.DetailsTableDesignId = itemPM.DetailsTableDesignId;

            itemPoco.DetailsSectionHasTwoColumns = itemPM.DetailsSectionHasTwoColumns;

          
            itemPoco.HeaderTableDesignId = itemPM.HeaderTableDesignId;

  
            itemPoco.HeaderSectionHasTwoColumns = itemPM.HeaderSectionHasTwoColumns;


            itemPoco.ShowHeaderQuoteDate = itemPM.ShowHeaderQuoteDate;
            itemPoco.ShowHeaderExpirationDate = itemPM.ShowHeaderExpirationDate;
            itemPoco.ShowHeaderQuoteNumber = itemPM.ShowHeaderQuoteNumber;
            itemPoco.ShowHeaderCustomer = itemPM.ShowHeaderCustomer;


            itemPoco.ShowDetailsExpirationDate = itemPM.ShowDetailsExpirationDate;
            itemPoco.ShowDetailsExpirationDays = itemPM.ShowDetailsExpirationDays;
            itemPoco.ShowDetailsShipperName = itemPM.ShowDetailsShipperName;
            itemPoco.ShowDetailsShipperAddress = itemPM.ShowDetailsShipperAddress;


            itemPoco.ShowDetailsShipperContact = itemPM.ShowDetailsShipperContact;
            itemPoco.ShowDetailsShipperReferences = itemPM.ShowDetailsShipperReferences;
            itemPoco.ShowDetailsConsigneeName = itemPM.ShowDetailsConsigneeName;
            itemPoco.ShowDetailsConsigneeAddress = itemPM.ShowDetailsConsigneeAddress;


            itemPoco.ShowDetailsConsigneeContact = itemPM.ShowDetailsConsigneeContact;
            itemPoco.ShowDetailsConsigneeReferences = itemPM.ShowDetailsConsigneeReferences;
            itemPoco.ShowDetailsPickupFrom = itemPM.ShowDetailsPickupFrom;
            itemPoco.ShowDetailsDeliveryTo = itemPM.ShowDetailsDeliveryTo;


            itemPoco.ShowDetailsFromPort = itemPM.ShowDetailsFromPort;
            itemPoco.ShowDetailsToPort = itemPM.ShowDetailsToPort;
            itemPoco.ShowDetailsIncoterms = itemPM.ShowDetailsIncoterms;
            itemPoco.ShowDetailsService = itemPM.ShowDetailsService;

            itemPoco.ShowDetailsSalesMan = itemPM.ShowDetailsSalesMan;
            itemPoco.ShowDetailsDescriptionOfGoods = itemPM.ShowDetailsDescriptionOfGoods;
            itemPoco.ShowDetailsDangerousGoods = itemPM.ShowDetailsDangerousGoods;
            itemPoco.ShowDetailsCarrier = itemPM.ShowDetailsCarrier;
          itemPoco.DetailsTitleDesignId = itemPM.DetailsTitleDesignId;



          itemPoco.ShowDetailsCustomerName = itemPM.ShowDetailsCustomerName;
          itemPoco.ShowDetailsCustomerAddress = itemPM.ShowDetailsCustomerAddress;
          itemPoco.ShowDetailsCustomerContact = itemPM.ShowDetailsCustomerContact;
          itemPoco.ShowDetailsCustomerReferences = itemPM.ShowDetailsCustomerReferences;



          itemPoco.PageHeaderArea1Type = itemPM.PageHeaderArea1Type;
          itemPoco.PageHeaderArea2Type = itemPM.PageHeaderArea2Type;
          itemPoco.PageHeaderArea3Type = itemPM.PageHeaderArea3Type;

          itemPoco.PageHeaderArea1ImageDetailId = itemPM.PageHeaderArea1ImageDetailId;
          itemPoco.PageHeaderArea2ImageDetailId = itemPM.PageHeaderArea2ImageDetailId;
          itemPoco.PageHeaderArea3ImageDetailId = itemPM.PageHeaderArea3ImageDetailId;


          itemPoco.PageHeaderArea1FreeText = itemPM.PageHeaderArea1FreeText;
          itemPoco.PageHeaderArea2FreeText = itemPM.PageHeaderArea2FreeText;
          itemPoco.PageHeaderArea3FreeText = itemPM.PageHeaderArea3FreeText;

          itemPoco.PageHeaderArea1FreeTextDesignId = itemPM.PageHeaderArea1FreeTextDesignId;
          itemPoco.PageHeaderArea2FreeTextDesignId = itemPM.PageHeaderArea2FreeTextDesignId;
          itemPoco.PageHeaderArea3FreeTextDesignId = itemPM.PageHeaderArea3FreeTextDesignId;



          itemPoco.PageHeaderArea1Width = itemPM.PageHeaderArea1Width;
          itemPoco.PageHeaderArea2Width = itemPM.PageHeaderArea2Width;
          itemPoco.PageHeaderArea3Width = itemPM.PageHeaderArea3Width;



          itemPoco.PageHeaderArea1Height = itemPM.PageHeaderArea1Height;
          itemPoco.PageHeaderArea2Height = itemPM.PageHeaderArea2Height;

          itemPoco.PageHeaderArea3Height = itemPM.PageHeaderArea3Height;


          itemPoco.PageHeaderArea1ImageAlignment = itemPM.PageHeaderArea1ImageAlignment;
          itemPoco.PageHeaderArea2ImageAlignment = itemPM.PageHeaderArea2ImageAlignment;
          itemPoco.PageHeaderArea3ImageAlignment = itemPM.PageHeaderArea3ImageAlignment;

          itemPoco.PageFooterArea1Type = itemPM.PageFooterArea1Type;
          itemPoco.PageFooterArea2Type = itemPM.PageFooterArea2Type;
          itemPoco.PageFooterArea3Type = itemPM.PageFooterArea3Type;

          itemPoco.PageFooterArea1ImageDetailId = itemPM.PageFooterArea1ImageDetailId;
          itemPoco.PageFooterArea2ImageDetailId = itemPM.PageFooterArea2ImageDetailId;
          itemPoco.PageFooterArea3ImageDetailId = itemPM.PageFooterArea3ImageDetailId;


          itemPoco.PageFooterArea1FreeText = itemPM.PageFooterArea1FreeText;
          itemPoco.PageFooterArea2FreeText = itemPM.PageFooterArea2FreeText;
          itemPoco.PageFooterArea3FreeText = itemPM.PageFooterArea3FreeText;

          itemPoco.PageFooterArea1FreeTextDesignId = itemPM.PageFooterArea1FreeTextDesignId;
          itemPoco.PageFooterArea2FreeTextDesignId = itemPM.PageFooterArea2FreeTextDesignId;
          itemPoco.PageFooterArea3FreeTextDesignId = itemPM.PageFooterArea3FreeTextDesignId;



          itemPoco.PageFooterArea1Width = itemPM.PageFooterArea1Width;
          itemPoco.PageFooterArea2Width = itemPM.PageFooterArea2Width;
          itemPoco.PageFooterArea3Width = itemPM.PageFooterArea3Width;



          itemPoco.PageFooterArea1Height = itemPM.PageFooterArea1Height;
          itemPoco.PageFooterArea2Height = itemPM.PageFooterArea2Height;

          itemPoco.PageFooterArea3Height = itemPM.PageFooterArea3Height;


          itemPoco.PageFooterArea1ImageAlignment = itemPM.PageFooterArea1ImageAlignment;
          itemPoco.PageFooterArea2ImageAlignment = itemPM.PageFooterArea2ImageAlignment;
          itemPoco.PageFooterArea3ImageAlignment = itemPM.PageFooterArea3ImageAlignment;

          itemPoco.PricingPackagesTitleDesignId = itemPM.PricingPackagesTitleDesignId;
          itemPoco.PricingContainsersTitleDesignId = itemPM.PricingContainsersTitleDesignId;
          itemPoco.ShowTitlePricingPackages = itemPM.ShowTitlePricingPackages;
          itemPoco.ShowTitlePricingContainsers = itemPM.ShowTitlePricingContainsers;
          itemPoco.PageHeaderAreaHeight = itemPM.PageHeaderAreaHeight;
          itemPoco.PageFooterAreaHeight = itemPM.PageFooterAreaHeight;
          itemPoco.PageHeaderImage1Width = itemPM.PageHeaderImage1Width;

          itemPoco.PageHeaderImage2Width = itemPM.PageHeaderImage2Width;
          itemPoco.PageHeaderImage3Width = itemPM.PageHeaderImage3Width;
          itemPoco.PageFooterImage1Width = itemPM.PageFooterImage1Width;
          itemPoco.PageFooterImage2Width = itemPM.PageFooterImage2Width;

          itemPoco.PageFooterImage3Width = itemPM.PageFooterImage3Width;


          itemPoco.PageFooterBorderTypeCode = itemPM.PageFooterBorderTypeCode;
          itemPoco.PageFooterBorderColor = itemPM.PageFooterBorderColor;
          itemPoco.PageFooterBorderThickness = itemPM.PageFooterBorderThickness;


          itemPoco.PageHeaderBorderTypeCode = itemPM.PageHeaderBorderTypeCode;
          itemPoco.PageHeaderBorderColor = itemPM.PageHeaderBorderColor;
          itemPoco.PageHeaderBorderThickness = itemPM.PageHeaderBorderThickness;



          itemPoco.DetailsTableColumWidthType = itemPM.DetailsTableColumWidthType;
          itemPoco.HeaderTableColumWidthType = itemPM.HeaderTableColumWidthType;

          itemPoco.DetailsTableColumn1LabelWidth = itemPM.DetailsTableColumn1LabelWidth;
          itemPoco.DetailsTableColumn1ValueWidth = itemPM.DetailsTableColumn1ValueWidth;
          itemPoco.DetailsTableColumn2LabelWidth = itemPM.DetailsTableColumn2LabelWidth;
          itemPoco.DetailsTableColumn2ValueWidth = itemPM.DetailsTableColumn2ValueWidth;


          itemPoco.HeaderTableColumn1LabelWidth = itemPM.HeaderTableColumn1LabelWidth;
          itemPoco.HeaderTableColumn1ValueWidth = itemPM.HeaderTableColumn1ValueWidth;
          itemPoco.HeaderTableColumn2LabelWidth = itemPM.HeaderTableColumn2LabelWidth;
          itemPoco.HeaderTableColumn2ValueWidth = itemPM.HeaderTableColumn2ValueWidth; 
           
          itemPoco.QuoteTemplatePDFMarginRight = itemPM.QuoteTemplatePDFMarginRight;
          itemPoco.QuoteTemplatePDFMarginLeft = itemPM.QuoteTemplatePDFMarginLeft;


          itemPoco.ShowPriceByContainerColumn = itemPM.ShowPriceByContainerColumn;
          itemPoco.ShowCodeChargeSaleMinMaxContainers = itemPM.ShowCodeChargeSaleMinMaxContainers;

          itemPoco.ShowCodeChargeSaleMinMaxPackages = itemPM.ShowCodeChargeSaleMinMaxPackages;

          itemPoco.ShowChargeDescriptionContainers = itemPM.ShowChargeDescriptionContainers;
          itemPoco.ShowChargeDescriptionPackages = itemPM.ShowChargeDescriptionPackages;
          itemPoco.ShowTotalPerChargeGroupPackages = itemPM.ShowTotalPerChargeGroupPackages;
          itemPoco.ShowTotalPerChargeGroupContainers = itemPM.ShowTotalPerChargeGroupContainers;

          itemPoco.ShowPageBreakBeforeTotalPerContainersTable = itemPM.ShowPageBreakBeforeTotalPerContainersTable;
          itemPoco.TotalPerContainersAdditionalTextDesignId = itemPM.TotalPerContainersAdditionalTextDesignId;
          itemPoco.TotalPerContainersTableDesignId = itemPM.TotalPerContainersTableDesignId;
          itemPoco.TotalPerContainersCurrencyType = itemPM.TotalPerContainersCurrencyType;
          itemPoco.ShowTitleTotalPerContainersTable = itemPM.ShowTitleTotalPerContainersTable;
          itemPoco.ShowChargeNotePackages = itemPM.ShowChargeNotePackages;
          itemPoco.ShowChargeNoteContainers = itemPM.ShowChargeNoteContainers;
          itemPoco.ShowSaleMaxMinAmountContainers = itemPM.ShowSaleMaxMinAmountContainers;
          itemPoco.ShowSaleMaxMinAmountPackages = itemPM.ShowSaleMaxMinAmountPackages;
          itemPoco.ShowHeaderLabelsContainers = itemPM.ShowHeaderLabelsContainers;
          itemPoco.ShowHeaderLabelsPackages = itemPM.ShowHeaderLabelsPackages;

            itemPoco.SpaceLinesBeforeContainers = itemPM.SpaceLinesBeforeContainers;
            itemPoco.SpaceLinesBeforeFooters = itemPM.SpaceLinesBeforeFooters;
            itemPoco.SpaceLinesBeforeHeaders = itemPM.SpaceLinesBeforeHeaders;
            itemPoco.SpaceLinesBeforePackages = itemPM.SpaceLinesBeforePackages;
            itemPoco.SpaceLinesBeforeQuoteDetails = itemPM.SpaceLinesBeforeQuoteDetails;
            itemPoco.SpaceLinesBeforeQuoteHeaders = itemPM.SpaceLinesBeforeQuoteHeaders;
            itemPoco.SpaceLinesBeforePerContainers = itemPM.SpaceLinesBeforePerContainers;
            itemPoco.QuoteTemplatePDFMarginBottom = itemPM.QuoteTemplatePDFMarginBottom;
            itemPoco.QuoteTemplatePDFMarginTop = itemPM.QuoteTemplatePDFMarginTop;
            itemPoco.ShowIncludedChargesPerContainers = itemPM.ShowIncludedChargesPerContainers;
            itemPoco.ShowIncludedChargesPackages = itemPM.ShowIncludedChargesPackages;
            itemPoco.ShowIncludedChargesContainers = itemPM.ShowIncludedChargesContainers;

        }
    }
}