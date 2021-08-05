using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Amital.QuoteOPM.Data.EntityLists
{
   [DataContract]
   public partial class QuoteOPTemplateSettingList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public bool ShowTotalInSaleCurrencyPackages  { get; set; }
       [DataMember]
       public bool ShowTotalInSaleCurrencyContainers  { get; set; }
       [DataMember]
       public bool ShowTotalInLocalCurrencyPackages  { get; set; }
       [DataMember]
       public bool ShowTotalInLocalCurrencyContainers  { get; set; }
       [DataMember]
       public bool ShowPricesTablePackages  { get; set; }
       [DataMember]
       public bool ShowPricesTableContainers  { get; set; }
       [DataMember]
       public bool ShowChargeCodePackages  { get; set; }
       [DataMember]
       public bool ShowChargeDescriptionPackages  { get; set; }
       [DataMember]
       public bool ShowChargeDescriptionContainers  { get; set; }
       [DataMember]
       public bool ShowChargeCodeContainers  { get; set; }
       [DataMember]
       public bool ShowChargeNamePackages  { get; set; }
       [DataMember]
       public bool ShowChargeNameContainers  { get; set; }
       [DataMember]
       public bool ShowMeasurementPackages  { get; set; }
       [DataMember]
       public bool ShowMeasurementContainers  { get; set; }
       [DataMember]
       public bool ShowFixedPriceContainers  { get; set; }
       [DataMember]
       public bool ShowUnitsPackages  { get; set; }
       [DataMember]
       public bool ShowUnitPricePackages  { get; set; }
       [DataMember]
       public bool ShowLocalCurrencyColumnPackages  { get; set; }
       [DataMember]
       public bool ShowLocalCurrencyColumnContainers  { get; set; }
       [DataMember]
       public bool ShowSaleCurrencyColumnPackages  { get; set; }
       [DataMember]
       public bool ShowSaleCurrencyColumnContainers  { get; set; }
       [DataMember]
       public bool ShowLocalLanguage  { get; set; }
       [DataMember]
       public bool SplitChargesbyGroupsPackages  { get; set; }
       [DataMember]
       public bool SplitChargesbyGroupsContainers  { get; set; }
       [DataMember]
       public bool ShowContainerNameInsteadOfCodeContainers  { get; set; }
       [DataMember]
       public bool AlignRight  { get; set; }
       [DataMember]
       public bool ShowPriceByContainerColumn  { get; set; }
       [DataMember]
       public bool ShowCodeChargeSaleMinMaxContainers  { get; set; }
       [DataMember]
       public bool ShowCodeChargeSaleMinMaxPackages  { get; set; }
       [DataMember]
       public int QuoteTemplatePDFMarginLeft  { get; set; }
       [DataMember]
       public int QuoteTemplatePDFMarginRight  { get; set; }
       [DataMember]
       public string PageHeaderArea1Type  { get; set; }
       [DataMember]
       public string PageHeaderArea2Type  { get; set; }
       [DataMember]
       public string PageHeaderArea3Type  { get; set; }
       [DataMember]
       public string PageHeaderArea1ImageDetailId  { get; set; }
       [DataMember]
       public string PageHeaderArea2ImageDetailId  { get; set; }
       [DataMember]
       public string PageHeaderArea3ImageDetailId  { get; set; }
       [DataMember]
       public string PageHeaderArea1FreeText  { get; set; }
       [DataMember]
       public string PageHeaderArea2FreeText  { get; set; }
       [DataMember]
       public string PageHeaderArea3FreeText  { get; set; }
       [DataMember]
       public string PageHeaderArea1FreeTextDesignId  { get; set; }
       [DataMember]
       public string PageHeaderArea2FreeTextDesignId  { get; set; }
       [DataMember]
       public string PageHeaderArea3FreeTextDesignId  { get; set; }
       [DataMember]
       public double PageHeaderArea1Width  { get; set; }
       [DataMember]
       public double PageHeaderArea2Width  { get; set; }
       [DataMember]
       public double PageHeaderArea3Width  { get; set; }
       [DataMember]
       public int PageHeaderImage1Width  { get; set; }
       [DataMember]
       public int PageHeaderImage2Width  { get; set; }
       [DataMember]
       public int PageHeaderImage3Width  { get; set; }
       [DataMember]
       public int PageFooterImage1Width  { get; set; }
       [DataMember]
       public int PageFooterImage2Width  { get; set; }
       [DataMember]
       public int PageFooterImage3Width  { get; set; }
       [DataMember]
       public int PageHeaderAreaHeight  { get; set; }
       [DataMember]
       public int PageFooterAreaHeight  { get; set; }
       [DataMember]
       public int PageHeaderArea1Height  { get; set; }
       [DataMember]
       public int PageHeaderArea2Height  { get; set; }
       [DataMember]
       public int PageHeaderArea3Height  { get; set; }
       [DataMember]
       public string PageHeaderArea1ImageAlignment  { get; set; }
       [DataMember]
       public string PageHeaderArea2ImageAlignment  { get; set; }
       [DataMember]
       public string PageHeaderArea3ImageAlignment  { get; set; }
       [DataMember]
       public string PageFooterArea1Type  { get; set; }
       [DataMember]
       public string PageFooterArea2Type  { get; set; }
       [DataMember]
       public string PageFooterArea3Type  { get; set; }
       [DataMember]
       public string PageFooterArea1ImageDetailId  { get; set; }
       [DataMember]
       public string PageFooterArea2ImageDetailId  { get; set; }
       [DataMember]
       public string PageFooterArea3ImageDetailId  { get; set; }
       [DataMember]
       public string PageFooterArea1FreeText  { get; set; }
       [DataMember]
       public string PageFooterArea2FreeText  { get; set; }
       [DataMember]
       public string PageFooterArea3FreeText  { get; set; }
       [DataMember]
       public string PageFooterArea1FreeTextDesignId  { get; set; }
       [DataMember]
       public string PageFooterArea2FreeTextDesignId  { get; set; }
       [DataMember]
       public string PageFooterArea3FreeTextDesignId  { get; set; }
       [DataMember]
       public double PageFooterArea1Width  { get; set; }
       [DataMember]
       public double PageFooterArea2Width  { get; set; }
       [DataMember]
       public double PageFooterArea3Width  { get; set; }
       [DataMember]
       public int PageFooterArea1Height  { get; set; }
       [DataMember]
       public int PageFooterArea2Height  { get; set; }
       [DataMember]
       public int PageFooterArea3Height  { get; set; }
       [DataMember]
       public string PageFooterArea1ImageAlignment  { get; set; }
       [DataMember]
       public string PageFooterArea2ImageAlignment  { get; set; }
       [DataMember]
       public string PageFooterArea3ImageAlignment  { get; set; }
       [DataMember]
       public bool ShowHeaderQuoteDate  { get; set; }
       [DataMember]
       public bool ShowHeaderExpirationDate  { get; set; }
       [DataMember]
       public bool ShowHeaderQuoteNumber  { get; set; }
       [DataMember]
       public bool ShowHeaderCustomer  { get; set; }
       [DataMember]
       public bool ShowDetailsExpirationDate  { get; set; }
       [DataMember]
       public bool ShowDetailsExpirationDays  { get; set; }
       [DataMember]
       public bool ShowDetailsShipperName  { get; set; }
       [DataMember]
       public bool ShowDetailsShipperAddress  { get; set; }
       [DataMember]
       public bool ShowDetailsShipperContact  { get; set; }
       [DataMember]
       public bool ShowDetailsShipperReferences  { get; set; }
       [DataMember]
       public bool ShowDetailsConsigneeName  { get; set; }
       [DataMember]
       public bool ShowDetailsConsigneeAddress  { get; set; }
       [DataMember]
       public bool ShowDetailsConsigneeContact  { get; set; }
       [DataMember]
       public bool ShowDetailsConsigneeReferences  { get; set; }
       [DataMember]
       public bool ShowDetailsPickupFrom  { get; set; }
       [DataMember]
       public bool ShowDetailsDeliveryTo  { get; set; }
       [DataMember]
       public bool ShowDetailsFromPort  { get; set; }
       [DataMember]
       public bool ShowDetailsToPort  { get; set; }
       [DataMember]
       public bool ShowDetailsIncoterms  { get; set; }
       [DataMember]
       public bool ShowDetailsService  { get; set; }
       [DataMember]
       public bool ShowDetailsSalesMan  { get; set; }
       [DataMember]
       public bool ShowDetailsDescriptionOfGoods  { get; set; }
       [DataMember]
       public bool ShowDetailsDangerousGoods  { get; set; }
       [DataMember]
       public bool ShowDetailsCarrier  { get; set; }
       [DataMember]
       public bool ShowDetailsCustomerName  { get; set; }
       [DataMember]
       public bool ShowDetailsCustomerAddress  { get; set; }
       [DataMember]
       public bool ShowDetailsCustomerContact  { get; set; }
       [DataMember]
       public bool ShowDetailsCustomerReferences  { get; set; }
       [DataMember]
       public bool ShowTitleQuoteDetails  { get; set; }
       [DataMember]
       public bool ShowTitlePricingPackages  { get; set; }
       [DataMember]
       public bool ShowTitlePricingContainsers  { get; set; }
       [DataMember]
       public string PackagesTableDesignId  { get; set; }
       [DataMember]
       public string ContainserTableDesignId  { get; set; }
       [DataMember]
       public string TotalsPackagesLabelDesignId  { get; set; }
       [DataMember]
       public string TotalsContainsersLabelDesignId  { get; set; }
       [DataMember]
       public string TotalsPackagesValueDesignId  { get; set; }
       [DataMember]
       public string TotalsContainsersValueDesignId  { get; set; }
       [DataMember]
       public bool RightToLeft  { get; set; }
       [DataMember]
       public string GroupByPackagesLabelDesignId  { get; set; }
       [DataMember]
       public string GroupByPackagesValueDesignId  { get; set; }
       [DataMember]
       public string GroupByContainsersLabelDesignId  { get; set; }
       [DataMember]
       public string GroupByContainsersValueDesignId  { get; set; }
       [DataMember]
       public string DetailsTableDesignId  { get; set; }
       [DataMember]
       public bool DetailsSectionHasTwoColumns  { get; set; }
       [DataMember]
       public string HeaderTableDesignId  { get; set; }
       [DataMember]
       public bool HeaderSectionHasTwoColumns  { get; set; }
       [DataMember]
       public string DetailsTitleDesignId  { get; set; }
       [DataMember]
       public string PricingPackagesTitleDesignId  { get; set; }
       [DataMember]
       public string PricingContainsersTitleDesignId  { get; set; }
       [DataMember]
       public string PageHeaderBorderTypeCode  { get; set; }
       [DataMember]
       public string PageHeaderBorderColor  { get; set; }
       [DataMember]
       public int PageHeaderBorderThickness  { get; set; }
       [DataMember]
       public string PageFooterBorderTypeCode  { get; set; }
       [DataMember]
       public string PageFooterBorderColor  { get; set; }
       [DataMember]
       public int PageFooterBorderThickness  { get; set; }
       [DataMember]
       public string HeaderTableColumWidthType  { get; set; }
       [DataMember]
       public string DetailsTableColumWidthType  { get; set; }
       [DataMember]
       public double DetailsTableColumn1LabelWidth  { get; set; }
       [DataMember]
       public double DetailsTableColumn1ValueWidth  { get; set; }
       [DataMember]
       public double DetailsTableColumn2LabelWidth  { get; set; }
       [DataMember]
       public double DetailsTableColumn2ValueWidth  { get; set; }
       [DataMember]
       public double HeaderTableColumn1LabelWidth  { get; set; }
       [DataMember]
       public double HeaderTableColumn1ValueWidth  { get; set; }
       [DataMember]
       public double HeaderTableColumn2LabelWidth  { get; set; }
       [DataMember]
       public double HeaderTableColumn2ValueWidth  { get; set; }
       [DataMember]
       public bool ShowTotalPerChargeGroupPackages  { get; set; }
       [DataMember]
       public bool ShowTotalPerChargeGroupContainers  { get; set; }
       [DataMember]
       public bool ShowPageBreakBeforeTotalPerContainersTable  { get; set; }
       [DataMember]
       public string TotalPerContainersAdditionalTextDesignId  { get; set; }
       [DataMember]
       public string TotalPerContainersTableDesignId  { get; set; }
       [DataMember]
       public string TotalPerContainersCurrencyType  { get; set; }
       [DataMember]
       public bool ShowTitleTotalPerContainersTable  { get; set; }
       [DataMember]
       public bool ShowChargeNotePackages  { get; set; }
       [DataMember]
       public bool ShowChargeNoteContainers  { get; set; }
       [DataMember]
       public bool ShowSaleMaxMinAmountPackages  { get; set; }
       [DataMember]
       public bool ShowSaleMaxMinAmountContainers  { get; set; }
       [DataMember]
       public bool ShowHeaderLabelsPackages  { get; set; }
       [DataMember]
       public bool ShowHeaderLabelsContainers  { get; set; }
       [DataMember]
       public int SpaceLinesBeforeContainers  { get; set; }
       [DataMember]
       public int SpaceLinesBeforePackages  { get; set; }
       [DataMember]
       public int SpaceLinesBeforeQuoteHeaders  { get; set; }
       [DataMember]
       public int SpaceLinesBeforeQuoteDetails  { get; set; }
       [DataMember]
       public int SpaceLinesBeforeHeaders  { get; set; }
       [DataMember]
       public int SpaceLinesBeforeFooters  { get; set; }
       [DataMember]
       public int SpaceLinesBeforePerContainers  { get; set; }
       [DataMember]
       public int QuoteTemplatePDFMarginTop  { get; set; }
       [DataMember]
       public int QuoteTemplatePDFMarginBottom  { get; set; }
       [DataMember]
       public bool ShowIncludedChargesPerContainers  { get; set; }
       [DataMember]
       public bool ShowIncludedChargesPackages  { get; set; }
       [DataMember]
       public bool ShowIncludedChargesContainers  { get; set; }
       [DataMember]
       public bool ShowVATTypePackages  { get; set; }
       [DataMember]
       public bool ShowVATTypeContainers  { get; set; }
       [DataMember]
       public bool ShowVATPercentagePackages  { get; set; }
       [DataMember]
       public bool HidePageNumber  { get; set; }
       [DataMember]
       public string PageNumberingTextDesignId  { get; set; }
       [DataMember]
       public bool ShowRegionalTAXPackages  { get; set; }
       [DataMember]
       public bool ShowRegionalTAXContainers  { get; set; }
   }

}
	 