using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.QuoteModel.EntityPOCOs
{
    public class QuoteTemplateSetting
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public bool ShowTotalInSaleCurrencyPackages { get; set; }
        public bool ShowTotalInSaleCurrencyContainers { get; set; }

        public bool ShowTotalInLocalCurrencyPackages { get; set; }
        public bool ShowTotalInLocalCurrencyContainers { get; set; }

        public bool ShowPricesTablePackages { get; set; }
        public bool ShowPricesTableContainers { get; set; }

        public bool ShowChargeCodePackages { get; set; }
        public bool ShowChargeDescriptionPackages { get; set; }
        public bool ShowChargeDescriptionContainers { get; set; }


        public bool ShowChargeCodeContainers { get; set; }
    



        public bool ShowChargeNamePackages { get; set; }
        public bool ShowChargeNameContainers { get; set; }

        public bool ShowMeasurementPackages { get; set; }
        public bool ShowMeasurementContainers { get; set; }

        public bool ShowFixedPriceContainers { get; set; }
        public bool ShowUnitsPackages { get; set; }

        public bool ShowUnitPricePackages { get; set; }
        public bool ShowLocalCurrencyColumnPackages { get; set; }

        public bool ShowLocalCurrencyColumnContainers { get; set; }
        public bool ShowSaleCurrencyColumnPackages { get; set; }

        public bool ShowSaleCurrencyColumnContainers { get; set; }
        public bool ShowLocalLanguage { get; set; }
        public bool SplitChargesbyGroupsPackages { get; set; }
        public bool SplitChargesbyGroupsContainers { get; set; }
        public bool ShowContainerNameInsteadOfCodeContainers { get; set; }
        public bool AlignRight { get; set; }


        public bool ShowPriceByContainerColumn { get; set; }
        public bool ShowCodeChargeSaleMinMaxContainers { get; set; }
        public bool ShowCodeChargeSaleMinMaxPackages { get; set; }
        //////

        public int QuoteTemplatePDFMarginLeft { get; set; }
        public int QuoteTemplatePDFMarginRight { get; set; }


        public string PageHeaderArea1Type { get; set; }
        public string PageHeaderArea2Type { get; set; }
        public string PageHeaderArea3Type { get; set; }

        public string PageHeaderArea1ImageDetailId { get; set; }

        public string PageHeaderArea2ImageDetailId { get; set; }
        public string PageHeaderArea3ImageDetailId { get; set; }


        public string PageHeaderArea1FreeText { get; set; }

        public string PageHeaderArea2FreeText { get; set; }
        public string PageHeaderArea3FreeText { get; set; }



        public string PageHeaderArea1FreeTextDesignId { get; set; }

        public string PageHeaderArea2FreeTextDesignId { get; set; }
        public string PageHeaderArea3FreeTextDesignId { get; set; }



        public double? PageHeaderArea1Width { get; set; }

        public double? PageHeaderArea2Width { get; set; }
        public double? PageHeaderArea3Width { get; set; }



        public int PageHeaderImage1Width { get; set; }

        public int PageHeaderImage2Width { get; set; }
        public int PageHeaderImage3Width { get; set; }

        public int PageFooterImage1Width { get; set; }

        public int PageFooterImage2Width { get; set; }
        public int PageFooterImage3Width { get; set; }


        public int PageHeaderAreaHeight { get; set; }
        public int PageFooterAreaHeight { get; set; }

        public int PageHeaderArea1Height { get; set; }

        public int PageHeaderArea2Height { get; set; }
        public int PageHeaderArea3Height { get; set; }

        public string PageHeaderArea1ImageAlignment { get; set; }

        public string PageHeaderArea2ImageAlignment { get; set; }
        public string PageHeaderArea3ImageAlignment { get; set; }



        public string PageFooterArea1Type { get; set; }
        public string PageFooterArea2Type { get; set; }
        public string PageFooterArea3Type { get; set; }

        public string PageFooterArea1ImageDetailId { get; set; }

        public string PageFooterArea2ImageDetailId { get; set; }
        public string PageFooterArea3ImageDetailId { get; set; }


        public string PageFooterArea1FreeText { get; set; }

        public string PageFooterArea2FreeText { get; set; }
        public string PageFooterArea3FreeText { get; set; }



        public string PageFooterArea1FreeTextDesignId { get; set; }

        public string PageFooterArea2FreeTextDesignId { get; set; }
        public string PageFooterArea3FreeTextDesignId { get; set; }



        public double? PageFooterArea1Width { get; set; }

        public double? PageFooterArea2Width { get; set; }
        public double? PageFooterArea3Width { get; set; }

        public int PageFooterArea1Height { get; set; }

        public int PageFooterArea2Height { get; set; }
        public int PageFooterArea3Height { get; set; }

        public string PageFooterArea1ImageAlignment { get; set; }

        public string PageFooterArea2ImageAlignment { get; set; }
        public string PageFooterArea3ImageAlignment { get; set; }
		




        //Details Header


        public bool ShowHeaderQuoteDate{ get; set; }
        public bool ShowHeaderExpirationDate { get; set; }
        public bool ShowHeaderQuoteNumber { get; set; }
        public bool ShowHeaderCustomer { get; set; }
        public bool ShowDetailsExpirationDate { get; set; }
        public bool ShowDetailsExpirationDays { get; set; }
        // Shipper
        public bool ShowDetailsShipperName { get; set; }
        public bool ShowDetailsShipperAddress { get; set; }
        public bool ShowDetailsShipperContact { get; set; }
        public bool ShowDetailsShipperReferences { get; set; }
        // Consignee
        public bool ShowDetailsConsigneeName { get; set; }
        public bool ShowDetailsConsigneeAddress { get; set; }
        public bool ShowDetailsConsigneeContact { get; set; }
        public bool ShowDetailsConsigneeReferences { get; set; }
        public bool ShowDetailsPickupFrom { get; set; }
        public bool ShowDetailsDeliveryTo { get; set; }
        public bool ShowDetailsFromPort  { get; set; }
        public bool ShowDetailsToPort  { get; set; }
        public bool ShowDetailsIncoterms { get; set; }
        public bool ShowDetailsService { get; set; }
        public bool ShowDetailsSalesMan { get; set; }
        public bool ShowDetailsDescriptionOfGoods  { get; set; }
        public bool ShowDetailsDangerousGoods  { get; set; }
        public bool ShowDetailsCarrier { get; set; }
        
        //Customer 
        public bool ShowDetailsCustomerName { get; set; }
        public bool ShowDetailsCustomerAddress { get; set; }
        public bool ShowDetailsCustomerContact { get; set; }
        public bool ShowDetailsCustomerReferences { get; set; }

        public bool ShowTitleQuoteDetails { get; set; }


        public bool ShowTitlePricingPackages { get; set; }
        public bool ShowTitlePricingContainsers { get; set; }

        /// <summary>
      
        /// </summary>


        public string PackagesTableDesignId { get; set; }
        public string ContainserTableDesignId { get; set; }



        public string TotalsPackagesLabelDesignId { get; set; }

        public string TotalsContainsersLabelDesignId { get; set; }

        public string TotalsPackagesValueDesignId { get; set; }

        public string TotalsContainsersValueDesignId { get; set; }
        public bool RightToLeft { get; set; }



        public string GroupByPackagesLabelDesignId { get; set; }
        public string GroupByPackagesValueDesignId { get; set; }

        public string GroupByContainsersLabelDesignId { get; set; }
        public string GroupByContainsersValueDesignId { get; set; }




        public string DetailsTableDesignId { get; set; }


        public bool DetailsSectionHasTwoColumns { get; set; }


        public string HeaderTableDesignId { get; set; }





        public bool HeaderSectionHasTwoColumns { get; set; }



        public string DetailsTitleDesignId { get; set; }

        public string PricingPackagesTitleDesignId { get; set; }

        public string PricingContainsersTitleDesignId { get; set; }









        public string PageHeaderBorderTypeCode { get; set; }
        public string PageHeaderBorderColor { get; set; }
        public int PageHeaderBorderThickness { get; set; }



        public string PageFooterBorderTypeCode { get; set; }
        public string PageFooterBorderColor { get; set; }
        public int PageFooterBorderThickness { get; set; }







        public string HeaderTableColumWidthType { get; set; }
        public string DetailsTableColumWidthType { get; set; }



        public double? DetailsTableColumn1LabelWidth { get; set; }
        public double? DetailsTableColumn1ValueWidth { get; set; }
        public double? DetailsTableColumn2LabelWidth { get; set; }
        public double? DetailsTableColumn2ValueWidth { get; set; }


        public double? HeaderTableColumn1LabelWidth { get; set; }
        public double? HeaderTableColumn1ValueWidth { get; set; }
        public double? HeaderTableColumn2LabelWidth { get; set; }
        public double? HeaderTableColumn2ValueWidth { get; set; }


        public bool ShowTotalPerChargeGroupPackages { get; set; }


        public bool ShowTotalPerChargeGroupContainers { get; set; }

        public bool ShowPageBreakBeforeTotalPerContainersTable { get; set; }
        public string TotalPerContainersAdditionalTextDesignId { get; set; }
        public string TotalPerContainersTableDesignId { get; set; }
        public string TotalPerContainersCurrencyType { get; set; }
        public bool ShowTitleTotalPerContainersTable { get; set; }


        public bool ShowChargeNotePackages { get; set; }
        public bool ShowChargeNoteContainers { get; set; }



        public bool ShowSaleMaxMinAmountPackages { get; set; }
        public bool ShowSaleMaxMinAmountContainers { get; set; }


        public bool ShowHeaderLabelsPackages { get; set; }
        public bool ShowHeaderLabelsContainers { get; set; }



        public int SpaceLinesBeforeContainers { get; set; }
        public int SpaceLinesBeforePackages { get; set; }
        public int SpaceLinesBeforeQuoteHeaders { get; set; }
        public int SpaceLinesBeforeQuoteDetails { get; set; }
        public int SpaceLinesBeforeHeaders { get; set; }
        public int SpaceLinesBeforeFooters { get; set; }
        public int SpaceLinesBeforePerContainers { get; set; }



        public int QuoteTemplatePDFMarginTop { get; set; }
        public int QuoteTemplatePDFMarginBottom { get; set; }


        public bool ShowIncludedChargesPerContainers { get; set; }
        public bool ShowIncludedChargesPackages { get; set; }
        public bool ShowIncludedChargesContainers { get; set; }




        public bool ShowVATTypePackages { get; set; }
        public bool ShowVATTypeContainers { get; set; }
        public bool ShowVATPercentagePackages { get; set; }
        public bool ShowVATPercentageContainers { get; set; }
        public string PageNumberingTextDesignId { get; set; }
        public bool HidePageNumber { get; set; }
        public bool ShowRegionalTAXPackages { get; set; }
        public bool ShowRegionalTAXContainers { get; set; }

        public string XMLData { get; set; }


        [ForeignKey("TotalPerContainersAdditionalTextDesignId")]
        public virtual QuoteTemplateTextDesign TotalPerContainersAdditionalTextDesign { get; set; }


        [ForeignKey("TotalPerContainersTableDesignId")]
        public virtual QuoteTemplateTableDesign TotalPerContainersTableDesign { get; set; }

        [ForeignKey("PageNumberingTextDesignId")]
        public virtual QuoteTemplateTableDesign PageNumberingTextDesign { get; set; }


        [ForeignKey("PricingPackagesTitleDesignId")]
        public virtual QuoteTemplateTextDesign PricingPackagesTitleDesign { get; set; }


        [ForeignKey("PricingContainsersTitleDesignId")]
        public virtual QuoteTemplateTextDesign PricingContainsersTitleDesign { get; set; }


        [ForeignKey("DetailsTitleDesignId")]
        public virtual QuoteTemplateTextDesign DetailsTitleDesign { get; set; }



        [ForeignKey("HeaderTableDesignId")]
        public virtual QuoteTemplateTableDesign HeaderTableDesign { get; set; }






        [ForeignKey("DetailsTableDesignId")]
        public virtual QuoteTemplateTableDesign DetailsTableDesign { get; set; }






        [ForeignKey("GroupByPackagesLabelDesignId")]
        public virtual QuoteTemplateTextDesign QuoteTemplateTextDesignPackagesGroupByLabel { get; set; }

        [ForeignKey("GroupByPackagesValueDesignId")]
        public virtual QuoteTemplateTextDesign QuoteTemplateTextDesignPackagesGroupByValue { get; set; }

        [ForeignKey("GroupByContainsersLabelDesignId")]
        public virtual QuoteTemplateTextDesign QuoteTemplateTextDesignContainsersGroupByLabel { get; set; }

        [ForeignKey("GroupByContainsersValueDesignId")]
        public virtual QuoteTemplateTextDesign QuoteTemplateTextDesignContainsersGroupByValue { get; set; }


        
        [ForeignKey("PackagesTableDesignId")]
        public virtual QuoteTemplateTableDesign PackagesTableDesign { get; set; }


    
        [ForeignKey("ContainserTableDesignId")]
        public virtual QuoteTemplateTableDesign ContainserTableDesign { get; set; }

        [ForeignKey("TotalsPackagesLabelDesignId")]
        public virtual QuoteTemplateTextDesign QuoteTemplateTextDesignPackagesLabel { get; set; }

        [ForeignKey("TotalsContainsersLabelDesignId")]
        public virtual QuoteTemplateTextDesign QuoteTemplateTextDesignContainsersLabel { get; set; }


        [ForeignKey("TotalsPackagesValueDesignId")]
        public virtual QuoteTemplateTextDesign QuoteTemplateTextDesignPackagesValue { get; set; }

        [ForeignKey("TotalsContainsersValueDesignId")]
        public virtual QuoteTemplateTextDesign QuoteTemplateTextDesignContainsersValue { get; set; }



        [ForeignKey("PageHeaderArea1ImageDetailId")]
        public virtual ImageDetail PageHeaderArea1ImageDetail { get; set; }


        [ForeignKey("PageHeaderArea2ImageDetailId")]
        public virtual ImageDetail PageHeaderArea2ImageDetail { get; set; }

        [ForeignKey("PageHeaderArea3ImageDetailId")]
        public virtual ImageDetail PageHeaderArea3ImageDetail { get; set; }



        [ForeignKey("PageHeaderArea1FreeTextDesignId")]
        public virtual QuoteTemplateTextDesign PageHeaderArea1FreeTextDesign { get; set; }


        [ForeignKey("PageHeaderArea2FreeTextDesignId")]
        public virtual QuoteTemplateTextDesign PageHeaderArea2FreeTextDesign { get; set; }


        [ForeignKey("PageHeaderArea3FreeTextDesignId")]
        public virtual QuoteTemplateTextDesign PageHeaderArea3FreeTextDesign { get; set; }






        [ForeignKey("PageFooterArea1ImageDetailId")]
        public virtual ImageDetail PageFooterArea1ImageDetail { get; set; }


        [ForeignKey("PageFooterArea2ImageDetailId")]
        public virtual ImageDetail PageFooterArea2ImageDetail { get; set; }

        [ForeignKey("PageFooterArea3ImageDetailId")]
        public virtual ImageDetail PageFooterArea3ImageDetail { get; set; }



        [ForeignKey("PageFooterArea1FreeTextDesignId")]
        public virtual QuoteTemplateTextDesign PageFooterArea1FreeTextDesign { get; set; }


        [ForeignKey("PageFooterArea2FreeTextDesignId")]
        public virtual QuoteTemplateTextDesign PageFooterArea2FreeTextDesign { get; set; }


        [ForeignKey("PageFooterArea3FreeTextDesignId")]
        public virtual QuoteTemplateTextDesign PageFooterArea3FreeTextDesign { get; set; }






        
    }
    


   
}
