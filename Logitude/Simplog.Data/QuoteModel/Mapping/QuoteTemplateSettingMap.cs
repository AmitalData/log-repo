using Simplog.Data.QuoteModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.QuoteModel.Mapping
{
    public class QuoteTemplateSettingMap : EntityTypeConfiguration<QuoteTemplateSetting>
    {

        public QuoteTemplateSettingMap()
        {

            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Tenant)
                   .IsRequired();

            this.Property(t => t.ShowChargeCodePackages)
                .IsRequired();

            this.Property(t => t.ShowChargeCodeContainers)
                .IsRequired();

            this.Property(t => t.RightToLeft)
                 .IsRequired();

            this.Property(t => t.ShowChargeNamePackages)
                 .IsRequired();

            this.Property(t => t.ShowChargeNameContainers)
                .IsRequired();

            this.Property(t => t.ShowMeasurementPackages)
                  .IsRequired();

            this.Property(t => t.ShowPricesTableContainers)
                .IsRequired();



            this.Property(t => t.ShowTitlePricingPackages)
                .IsRequired();


            this.Property(t => t.ShowPriceByContainerColumn)
                 .IsRequired();

            this.Property(t => t.ShowCodeChargeSaleMinMaxContainers)
                 .IsRequired();


            this.Property(t => t.ShowTitlePricingPackages)
                 .IsRequired();




            this.Property(t => t.ShowTitlePricingPackages)
                .IsRequired();



            this.Property(t => t.ShowSaleCurrencyColumnPackages)
                .IsRequired();

            this.Property(t => t.ShowSaleCurrencyColumnContainers)
                .IsRequired();

            this.Property(t => t.ShowLocalCurrencyColumnPackages)
                 .IsRequired();

            this.Property(t => t.ShowLocalCurrencyColumnContainers)
                 .IsRequired();

            this.Property(t => t.ShowTotalInLocalCurrencyPackages)
                .IsRequired();

            this.Property(t => t.ShowTotalInLocalCurrencyContainers)
               .IsRequired();

            this.Property(t => t.ShowTotalInSaleCurrencyPackages)
               .IsRequired();

            this.Property(t => t.ShowTotalInSaleCurrencyContainers)
                .IsRequired();

            this.Property(t => t.ShowPricesTablePackages)
                     .IsRequired();

            this.Property(t => t.ShowPricesTableContainers)
                    .IsRequired();

            this.Property(t => t.ShowUnitPricePackages)
                  .IsRequired();

            this.Property(t => t.ShowUnitsPackages)
                .IsRequired();

            this.Property(t => t.ShowTitleQuoteDetails)
                    .IsRequired();



            this.Property(t => t.SplitChargesbyGroupsPackages)
                .IsRequired();

            this.Property(t => t.SplitChargesbyGroupsContainers)
                .IsRequired();

            this.Property(t => t.ShowLocalLanguage)
                  .IsRequired();

            this.Property(t => t.ShowFixedPriceContainers)
                  .IsRequired();

            this.Property(t => t.ShowContainerNameInsteadOfCodeContainers)
                  .IsRequired();

            this.Property(t => t.AlignRight)
                  .IsRequired();

            //////DetilsHeaderShow


            this.Property(t => t.ShowHeaderQuoteDate)
                  .IsRequired();



            this.Property(t => t.ShowHeaderExpirationDate)
                  .IsRequired();


            this.Property(t => t.ShowHeaderQuoteNumber)
                  .IsRequired();



            this.Property(t => t.ShowHeaderCustomer)
                  .IsRequired();



            this.Property(t => t.ShowDetailsExpirationDate)
                  .IsRequired();



            this.Property(t => t.ShowDetailsExpirationDays)
                  .IsRequired();



            this.Property(t => t.ShowDetailsShipperName)
                  .IsRequired();



            this.Property(t => t.ShowDetailsShipperAddress)
                 .IsRequired();



            this.Property(t => t.ShowDetailsShipperContact)
                  .IsRequired();


            this.Property(t => t.ShowDetailsShipperReferences)
                  .IsRequired();



            this.Property(t => t.ShowDetailsConsigneeName)
                  .IsRequired();



            this.Property(t => t.ShowDetailsConsigneeAddress)
                  .IsRequired();



            this.Property(t => t.ShowDetailsConsigneeContact)
                  .IsRequired();



            this.Property(t => t.ShowDetailsConsigneeReferences)
                  .IsRequired();








            this.Property(t => t.ShowDetailsCustomerName)
                  .IsRequired();



            this.Property(t => t.ShowDetailsCustomerAddress)
                  .IsRequired();



            this.Property(t => t.ShowDetailsCustomerContact)
                  .IsRequired();



            this.Property(t => t.ShowDetailsCustomerReferences)
                  .IsRequired();











            this.Property(t => t.ShowDetailsPickupFrom)
                 .IsRequired();



            this.Property(t => t.ShowDetailsDeliveryTo)
                  .IsRequired();


            this.Property(t => t.ShowDetailsFromPort)
                  .IsRequired();



            this.Property(t => t.ShowDetailsToPort)
                  .IsRequired();



            this.Property(t => t.ShowDetailsIncoterms)
                  .IsRequired();



            this.Property(t => t.ShowDetailsService)
                  .IsRequired();



            this.Property(t => t.ShowDetailsSalesMan)
                  .IsRequired();




            this.Property(t => t.ShowDetailsDescriptionOfGoods)
                  .IsRequired();



            this.Property(t => t.ShowDetailsDangerousGoods)
                  .IsRequired();



            this.Property(t => t.ShowDetailsCarrier)
                  .IsRequired();


            /////////////////// DetailsDesignId

            this.Property(t => t.DetailsTableDesignId)
                .HasMaxLength(15)
                .IsUnicode(false);



            this.Property(t => t.DetailsTitleDesignId)
                       .HasMaxLength(15)
                       .IsUnicode(false);




            this.Property(t => t.DetailsSectionHasTwoColumns)
                 .IsRequired();



            this.Property(t => t.HeaderTableDesignId)
                 .HasMaxLength(15)
                 .IsUnicode(false);






            this.Property(t => t.PricingPackagesTitleDesignId)
                 .HasMaxLength(15)
                 .IsUnicode(false);




            this.Property(t => t.PricingContainsersTitleDesignId)
                 .HasMaxLength(15)
                 .IsUnicode(false);






            this.Property(t => t.HeaderSectionHasTwoColumns)
                  .IsRequired();





            /////////////////////////////////////


            //page Header


            this.Property(t => t.PageHeaderArea1Type)
                 .HasMaxLength(50)
                 .IsUnicode(false);

            this.Property(t => t.PageHeaderArea2Type)
                 .HasMaxLength(50)
                 .IsUnicode(false);

            this.Property(t => t.PageHeaderArea3Type)
               .HasMaxLength(50)
               .IsUnicode(false);

            this.Property(t => t.PageHeaderArea1ImageDetailId)
        .HasMaxLength(15)
        .IsUnicode(false);

            this.Property(t => t.PageHeaderArea2ImageDetailId)
              .HasMaxLength(15)
               .IsUnicode(false);

            this.Property(t => t.PageHeaderArea3ImageDetailId)
                .HasMaxLength(15)
             .IsUnicode(false);



            this.Property(t => t.PageHeaderArea1FreeTextDesignId)
                 .HasMaxLength(15)
                 .IsUnicode(false);

            this.Property(t => t.PageHeaderArea2FreeTextDesignId)
              .HasMaxLength(15)
               .IsUnicode(false);

            this.Property(t => t.PageHeaderArea3FreeTextDesignId)
               .HasMaxLength(15)
             .IsUnicode(false);



            this.Property(t => t.PageHeaderArea1FreeText)
                   .HasMaxLength(1000)
                 .IsUnicode(true);

            this.Property(t => t.PageHeaderArea2FreeText)
              .HasMaxLength(1000)
               .IsUnicode(true);

            this.Property(t => t.PageHeaderArea3FreeText)
           .HasMaxLength(1000)
               .IsUnicode(true);




            this.Property(t => t.PageHeaderArea1Width)
                .IsRequired();

            this.Property(t => t.PageHeaderArea2Width)
                .IsRequired();

            this.Property(t => t.PageHeaderArea3Width)
                .IsRequired();







            this.Property(t => t.PageHeaderImage1Width)
           .IsRequired();

            this.Property(t => t.PageHeaderImage2Width)
                .IsRequired();

            this.Property(t => t.PageHeaderImage3Width)
                .IsRequired();




            this.Property(t => t.PageFooterImage1Width)
           .IsRequired();

            this.Property(t => t.PageFooterImage2Width)
                .IsRequired();

            this.Property(t => t.PageFooterImage3Width)
                .IsRequired();



            this.Property(t => t.PageHeaderAreaHeight)
                     .IsRequired();

            this.Property(t => t.PageFooterAreaHeight)
                .IsRequired();




            this.Property(t => t.PageHeaderArea1Height)
                .IsRequired();

            this.Property(t => t.PageHeaderArea2Height)
                .IsRequired();

            this.Property(t => t.PageHeaderArea3Height)
                .IsRequired();




            this.Property(t => t.PageHeaderArea1ImageAlignment)
                 .HasMaxLength(50)
                 .IsUnicode(false);

            this.Property(t => t.PageHeaderArea2ImageAlignment)
              .HasMaxLength(50)
               .IsUnicode(false);

            this.Property(t => t.PageHeaderArea3ImageAlignment)
                .HasMaxLength(50)
             .IsUnicode(false);



            /////////////////

            //Page Footer




            this.Property(t => t.PageFooterArea1Type)
                   .HasMaxLength(50)
                   .IsUnicode(false);

            this.Property(t => t.PageFooterArea2Type)
                 .HasMaxLength(50)
                 .IsUnicode(false);

            this.Property(t => t.PageFooterArea3Type)
               .HasMaxLength(50)
               .IsUnicode(false);

            this.Property(t => t.PageFooterArea1ImageDetailId)
        .HasMaxLength(15)
        .IsUnicode(false);

            this.Property(t => t.PageFooterArea2ImageDetailId)
              .HasMaxLength(15)
               .IsUnicode(false);

            this.Property(t => t.PageFooterArea3ImageDetailId)
                .HasMaxLength(15)
             .IsUnicode(false);



            this.Property(t => t.PageFooterArea1FreeTextDesignId)
                 .HasMaxLength(15)
                 .IsUnicode(false);

            this.Property(t => t.PageFooterArea2FreeTextDesignId)
              .HasMaxLength(15)
               .IsUnicode(false);

            this.Property(t => t.PageFooterArea3FreeTextDesignId)
               .HasMaxLength(15)
             .IsUnicode(false);



            this.Property(t => t.PageFooterArea1FreeText)
                   .HasMaxLength(1000)
                 .IsUnicode(true);

            this.Property(t => t.PageFooterArea2FreeText)
              .HasMaxLength(1000)
               .IsUnicode(true);

            this.Property(t => t.PageFooterArea3FreeText)
           .HasMaxLength(1000)
               .IsUnicode(true);




            this.Property(t => t.PageFooterArea1Width)
                .IsRequired();

            this.Property(t => t.PageFooterArea2Width)
                .IsRequired();

            this.Property(t => t.PageFooterArea3Width)
                .IsRequired();


            this.Property(t => t.PageFooterArea1Height)
                .IsRequired();

            this.Property(t => t.PageFooterArea2Height)
                .IsRequired();

            this.Property(t => t.PageFooterArea3Height)
                .IsRequired();




            this.Property(t => t.PageFooterArea1ImageAlignment)
                 .HasMaxLength(50)
                  .IsUnicode(false);

            this.Property(t => t.PageFooterArea2ImageAlignment)
              .HasMaxLength(50)
               .IsUnicode(false);

            this.Property(t => t.PageFooterArea3ImageAlignment)
                .HasMaxLength(50)
             .IsUnicode(false);










            ///////////////////////
            this.Property(t => t.PackagesTableDesignId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ContainserTableDesignId)
               .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.TotalsPackagesLabelDesignId)
                       .IsRequired()
                       .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.TotalsContainsersLabelDesignId)
                        .IsRequired()
                        .HasMaxLength(15)
                       .IsUnicode(false);

            this.Property(t => t.TotalsPackagesValueDesignId)
                            .IsRequired()
                            .HasMaxLength(15)
                           .IsUnicode(false);


            this.Property(t => t.TotalsContainsersValueDesignId)
                            .IsRequired()
                            .HasMaxLength(15)
                           .IsUnicode(false);




            this.Property(t => t.GroupByPackagesLabelDesignId)

                            .HasMaxLength(15)
                           .IsUnicode(false);



            this.Property(t => t.GroupByPackagesValueDesignId)

                            .HasMaxLength(15)
                           .IsUnicode(false);



            this.Property(t => t.GroupByContainsersLabelDesignId)

                            .HasMaxLength(15)
                           .IsUnicode(false);



            this.Property(t => t.GroupByContainsersValueDesignId)

                            .HasMaxLength(15)
                           .IsUnicode(false);






            this.Property(t => t.PageHeaderBorderTypeCode)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.PageHeaderBorderColor)
                 .IsRequired()
                 .HasMaxLength(10)
                 .IsUnicode(false);




            this.Property(t => t.PageHeaderBorderThickness)
                .IsRequired();




            this.Property(t => t.PageFooterBorderTypeCode)
               .IsRequired()
               .HasMaxLength(40)
              .IsUnicode(false);

            this.Property(t => t.PageFooterBorderColor)
                 .IsRequired()
                 .HasMaxLength(10)
                 .IsUnicode(false);


            this.Property(t => t.PageFooterBorderThickness)
                .IsRequired();




            this.Property(t => t.QuoteTemplatePDFMarginRight)
             .IsRequired();
            this.Property(t => t.QuoteTemplatePDFMarginLeft)
                   .IsRequired();


            //            public double? DetailsTableColumn1LabelWidth { get; set; }
            //public double? DetailsTableColumn1ValueWidth { get; set; }
            //public double? DetailsTableColumn2LabelWidth { get; set; }
            //public double? DetailsTableColumn2ValueWidth { get; set; }


            //public double? HeaderTableColumn1LabelWidth { get; set; }
            //public double? HeaderTableColumn1ValueWidth { get; set; }
            //public double? HeaderTableColumn2LabelWidth { get; set; }
            //public double? HeaderTableColumn2ValueWidth { get; set; }





            this.Property(t => t.DetailsTableColumn1LabelWidth)
                    .IsRequired();

            this.Property(t => t.DetailsTableColumn1ValueWidth)
                .IsRequired();


            this.Property(t => t.DetailsTableColumn2LabelWidth)
                                  .IsRequired();

            this.Property(t => t.DetailsTableColumn2ValueWidth)
                .IsRequired();



            this.Property(t => t.HeaderTableColumn1LabelWidth)
                      .IsRequired();

            this.Property(t => t.HeaderTableColumn1ValueWidth)
                .IsRequired();


            this.Property(t => t.HeaderTableColumn2LabelWidth)
                                  .IsRequired();

            this.Property(t => t.HeaderTableColumn2ValueWidth)
                .IsRequired();




            this.Property(t => t.DetailsTableColumWidthType)
                 .IsRequired()
                 .HasMaxLength(10)
                 .IsUnicode(false);

            this.Property(t => t.HeaderTableColumWidthType)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false);


               this.Property(t => t.TotalPerContainersAdditionalTextDesignId)
              .HasMaxLength(15)
              .IsUnicode(false);


            this.Property(t => t.TotalPerContainersTableDesignId)
              .HasMaxLength(15)
              .IsUnicode(false);


            this.Property(t => t.TotalPerContainersCurrencyType)
              .HasMaxLength(10)
              .IsUnicode(false);


            // Table & Column Mappings
            this.ToTable("QuoteTemplateSettings");

            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ShowChargeCodePackages).HasColumnName("ShowChargeCodePackages");
            this.Property(t => t.ShowChargeCodeContainers).HasColumnName("ShowChargeCodeContainers");



            this.Property(t => t.ShowChargeNamePackages).HasColumnName("ShowChargeNamePackages");
            this.Property(t => t.ShowChargeNameContainers).HasColumnName("ShowChargeNameContainers");
            this.Property(t => t.ShowMeasurementContainers).HasColumnName("ShowMeasurementContainers");
            this.Property(t => t.ShowMeasurementPackages).HasColumnName("ShowMeasurementPackages");

            this.Property(t => t.ShowFixedPriceContainers).HasColumnName("ShowFixedPriceContainers");

            this.Property(t => t.ShowPricesTablePackages).HasColumnName("ShowPricesTablePackages");
            this.Property(t => t.ShowPricesTableContainers).HasColumnName("ShowPricesTableContainers");
            this.Property(t => t.ShowLocalLanguage).HasColumnName("ShowLocalLanguage");
            this.Property(t => t.ShowUnitsPackages).HasColumnName("ShowUnitsPackages");

            this.Property(t => t.ShowUnitPricePackages).HasColumnName("ShowUnitPricePackages");
            this.Property(t => t.SplitChargesbyGroupsPackages).HasColumnName("SplitChargesbyGroupsPackages");
            this.Property(t => t.SplitChargesbyGroupsContainers).HasColumnName("SplitChargesbyGroupsContainers");



            this.Property(t => t.AlignRight).HasColumnName("AlignRight");
            this.Property(t => t.PackagesTableDesignId).HasColumnName("PackagesTableDesignId");

            this.Property(t => t.ContainserTableDesignId).HasColumnName("ContainserTableDesignId");

            this.Property(t => t.TotalsPackagesLabelDesignId).HasColumnName("TotalsPackagesLabelDesignId");
            this.Property(t => t.TotalsContainsersLabelDesignId).HasColumnName("TotalsContainsersLabelDesignId");

            this.Property(t => t.TotalsPackagesValueDesignId).HasColumnName("TotalsPackagesValueDesignId");
            this.Property(t => t.TotalsContainsersValueDesignId).HasColumnName("TotalsContainsersValueDesignId");

            this.Property(t => t.RightToLeft).HasColumnName("RightToLeft");


            this.Property(t => t.DetailsTableDesignId).HasColumnName("DetailsTableDesignId");

            this.Property(t => t.DetailsSectionHasTwoColumns).HasColumnName("DetailsSectionHasTwoColumns");

            this.Property(t => t.HeaderTableDesignId).HasColumnName("HeaderTableDesignId");


            this.Property(t => t.HeaderSectionHasTwoColumns).HasColumnName("HeaderSectionHasTwoColumns");

            this.Property(t => t.ShowTitleQuoteDetails).HasColumnName("ShowTitleQuoteDetails");
            this.Property(t => t.ShowChargeDescriptionPackages).HasColumnName("ShowChargeDescriptionPackages");
            
            this.Property(t => t.ShowHeaderQuoteDate).HasColumnName("ShowHeaderQuoteDate");
            this.Property(t => t.ShowHeaderExpirationDate).HasColumnName("ShowHeaderExpirationDate");
            this.Property(t => t.ShowHeaderQuoteNumber).HasColumnName("ShowHeaderQuoteNumber");
            this.Property(t => t.ShowHeaderCustomer).HasColumnName("ShowHeaderCustomer");


            this.Property(t => t.ShowDetailsExpirationDate).HasColumnName("ShowDetailsExpirationDate");
            this.Property(t => t.ShowDetailsExpirationDays).HasColumnName("ShowDetailsExpirationDays");
            this.Property(t => t.ShowDetailsShipperName).HasColumnName("ShowDetailsShipperName");
            this.Property(t => t.ShowDetailsShipperAddress).HasColumnName("ShowDetailsShipperAddress");
            this.Property(t => t.ShowDetailsShipperContact).HasColumnName("ShowDetailsShipperContact");
            this.Property(t => t.ShowDetailsShipperReferences).HasColumnName("ShowDetailsShipperReferences");
            this.Property(t => t.ShowDetailsConsigneeName).HasColumnName("ShowDetailsConsigneeName");
            this.Property(t => t.ShowDetailsConsigneeAddress).HasColumnName("ShowDetailsConsigneeAddress");




            this.Property(t => t.ShowDetailsCustomerName).HasColumnName("ShowDetailsCustomerName");
            this.Property(t => t.ShowDetailsCustomerAddress).HasColumnName("ShowDetailsCustomerAddress");
            this.Property(t => t.ShowDetailsCustomerContact).HasColumnName("ShowDetailsCustomerContact");
            this.Property(t => t.ShowDetailsCustomerReferences).HasColumnName("ShowDetailsCustomerReferences");



            this.Property(t => t.ShowDetailsConsigneeContact).HasColumnName("ShowDetailsConsigneeContact");
            this.Property(t => t.ShowDetailsConsigneeReferences).HasColumnName("ShowDetailsConsigneeReferences");
            this.Property(t => t.ShowDetailsPickupFrom).HasColumnName("ShowDetailsPickupFrom");
            this.Property(t => t.ShowDetailsDeliveryTo).HasColumnName("ShowDetailsDeliveryTo");




            this.Property(t => t.ShowDetailsFromPort).HasColumnName("ShowDetailsFromPort");
            this.Property(t => t.ShowDetailsToPort).HasColumnName("ShowDetailsToPort");
            this.Property(t => t.ShowDetailsIncoterms).HasColumnName("ShowDetailsIncoterms");
            this.Property(t => t.ShowDetailsService).HasColumnName("ShowDetailsService");


            this.Property(t => t.ShowDetailsSalesMan).HasColumnName("ShowDetailsSalesMan");
            this.Property(t => t.ShowDetailsDescriptionOfGoods).HasColumnName("ShowDetailsDescriptionOfGoods");
            this.Property(t => t.ShowDetailsDangerousGoods).HasColumnName("ShowDetailsDangerousGoods");
            this.Property(t => t.ShowDetailsCarrier).HasColumnName("ShowDetailsCarrier");



            this.Property(t => t.DetailsTitleDesignId).HasColumnName("DetailsTitleDesignId");

            this.Property(t => t.PageHeaderArea1FreeText).HasColumnName("PageHeaderArea1FreeText");
            this.Property(t => t.PageHeaderArea2FreeText).HasColumnName("PageHeaderArea2FreeText");
            this.Property(t => t.PageHeaderArea3FreeText).HasColumnName("PageHeaderArea3FreeText");


            this.Property(t => t.PageHeaderArea1Type).HasColumnName("PageHeaderArea1Type");
            this.Property(t => t.PageHeaderArea2Type).HasColumnName("PageHeaderArea2Type");
            this.Property(t => t.PageHeaderArea3Type).HasColumnName("PageHeaderArea3Type");

            this.Property(t => t.PageHeaderArea1ImageDetailId).HasColumnName("PageHeaderArea1ImageDetailId");
            this.Property(t => t.PageHeaderArea2ImageDetailId).HasColumnName("PageHeaderArea2ImageDetailId");
            this.Property(t => t.PageHeaderArea3ImageDetailId).HasColumnName("PageHeaderArea3ImageDetailId");







            this.Property(t => t.PageHeaderArea1Width).HasColumnName("PageHeaderArea1Width");
            this.Property(t => t.PageHeaderArea2Width).HasColumnName("PageHeaderArea2Width");
            this.Property(t => t.PageHeaderArea3Width).HasColumnName("PageHeaderArea3Width");


            this.Property(t => t.PageHeaderArea1Height).HasColumnName("PageHeaderArea1Height");
            this.Property(t => t.PageHeaderArea2Height).HasColumnName("PageHeaderArea2Height");
            this.Property(t => t.PageHeaderArea3Height).HasColumnName("PageHeaderArea3Height");



            this.Property(t => t.PageHeaderArea1ImageAlignment).HasColumnName("PageHeaderArea1ImageAlignment");
            this.Property(t => t.PageHeaderArea2ImageAlignment).HasColumnName("PageHeaderArea2ImageAlignment");
            this.Property(t => t.PageHeaderArea3ImageAlignment).HasColumnName("PageHeaderArea3ImageAlignment");




            this.Property(t => t.PageFooterArea1FreeText).HasColumnName("PageFooterArea1FreeText");
            this.Property(t => t.PageFooterArea2FreeText).HasColumnName("PageFooterArea2FreeText");
            this.Property(t => t.PageFooterArea3FreeText).HasColumnName("PageFooterArea3FreeText");


            this.Property(t => t.PageFooterArea1Type).HasColumnName("PageFooterArea1Type");
            this.Property(t => t.PageFooterArea2Type).HasColumnName("PageFooterArea2Type");
            this.Property(t => t.PageFooterArea3Type).HasColumnName("PageFooterArea3Type");

            this.Property(t => t.PageFooterArea1ImageDetailId).HasColumnName("PageFooterArea1ImageDetailId");
            this.Property(t => t.PageFooterArea2ImageDetailId).HasColumnName("PageFooterArea2ImageDetailId");
            this.Property(t => t.PageFooterArea3ImageDetailId).HasColumnName("PageFooterArea3ImageDetailId");






            this.Property(t => t.PageFooterArea1Width).HasColumnName("PageFooterArea1Width");
            this.Property(t => t.PageFooterArea2Width).HasColumnName("PageFooterArea2Width");
            this.Property(t => t.PageFooterArea3Width).HasColumnName("PageFooterArea3Width");


            this.Property(t => t.PageFooterArea1Height).HasColumnName("PageFooterArea1Height");
            this.Property(t => t.PageFooterArea2Height).HasColumnName("PageFooterArea2Height");
            this.Property(t => t.PageFooterArea3Height).HasColumnName("PageFooterArea3Height");



            this.Property(t => t.PageFooterArea1ImageAlignment).HasColumnName("PageFooterArea1ImageAlignment");
            this.Property(t => t.PageFooterArea2ImageAlignment).HasColumnName("PageFooterArea2ImageAlignment");
            this.Property(t => t.PageFooterArea3ImageAlignment).HasColumnName("PageFooterArea3ImageAlignment");




            this.Property(t => t.PricingPackagesTitleDesignId).HasColumnName("PricingPackagesTitleDesignId");

            this.Property(t => t.ShowTitlePricingPackages).HasColumnName("ShowTitlePricingPackages");
            this.Property(t => t.ShowTitlePricingContainsers).HasColumnName("ShowTitlePricingContainsers");



            this.Property(t => t.PageHeaderAreaHeight).HasColumnName("PageHeaderAreaHeight");
            this.Property(t => t.PageFooterAreaHeight).HasColumnName("PageFooterAreaHeight");
            this.Property(t => t.PageHeaderImage1Width).HasColumnName("PageHeaderImage1Width");
            this.Property(t => t.PageHeaderImage2Width).HasColumnName("PageHeaderImage2Width");
            this.Property(t => t.PageHeaderImage3Width).HasColumnName("PageHeaderImage3Width");


            this.Property(t => t.PageFooterImage1Width).HasColumnName("PageFooterImage1Width");
            this.Property(t => t.PageFooterImage2Width).HasColumnName("PageFooterImage2Width");
            this.Property(t => t.PageFooterImage3Width).HasColumnName("PageFooterImage3Width");



            this.Property(t => t.PageHeaderBorderTypeCode).HasColumnName("PageHeaderBorderTypeCode");
            this.Property(t => t.PageHeaderBorderColor).HasColumnName("PageHeaderBorderColor");
            this.Property(t => t.PageHeaderBorderThickness).HasColumnName("PageHeaderBorderThickness");


            this.Property(t => t.PageFooterBorderTypeCode).HasColumnName("PageFooterBorderTypeCode");
            this.Property(t => t.PageFooterBorderColor).HasColumnName("PageFooterBorderColor");
            this.Property(t => t.PageFooterBorderThickness).HasColumnName("PageFooterBorderThickness");





            this.Property(t => t.DetailsTableColumn1LabelWidth).HasColumnName("DetailsTableColumn1LabelWidth");
            this.Property(t => t.DetailsTableColumn1ValueWidth).HasColumnName("DetailsTableColumn1ValueWidth");
            this.Property(t => t.DetailsTableColumn2LabelWidth).HasColumnName("DetailsTableColumn2LabelWidth");
            this.Property(t => t.DetailsTableColumn2ValueWidth).HasColumnName("DetailsTableColumn2ValueWidth");





            this.Property(t => t.HeaderTableColumn1LabelWidth).HasColumnName("HeaderTableColumn1LabelWidth");
            this.Property(t => t.HeaderTableColumn1ValueWidth).HasColumnName("HeaderTableColumn1ValueWidth");
            this.Property(t => t.HeaderTableColumn2LabelWidth).HasColumnName("HeaderTableColumn2LabelWidth");
            this.Property(t => t.HeaderTableColumn2ValueWidth).HasColumnName("HeaderTableColumn2ValueWidth");



            this.Property(t => t.HeaderTableColumWidthType).HasColumnName("HeaderTableColumWidthType");
            this.Property(t => t.DetailsTableColumWidthType).HasColumnName("DetailsTableColumWidthType");

            this.Property(t => t.QuoteTemplatePDFMarginRight).HasColumnName("QuoteTemplatePDFMarginRight");
            this.Property(t => t.QuoteTemplatePDFMarginLeft).HasColumnName("QuoteTemplatePDFMarginLeft");


            this.Property(t => t.ShowPriceByContainerColumn).HasColumnName("ShowPriceByContainerColumn");
            this.Property(t => t.TotalPerContainersCurrencyType).HasColumnName("TotalPerContainersCurrencyType");

            this.Property(t => t.ShowChargeNotePackages).HasColumnName("ShowChargeNotePackages");
            this.Property(t => t.ShowChargeNoteContainers).HasColumnName("ShowChargeNoteContainers");



            this.Property(t => t.ShowSaleMaxMinAmountPackages).HasColumnName("ShowSaleMaxMinAmountPackages");
            this.Property(t => t.ShowSaleMaxMinAmountContainers).HasColumnName("ShowSaleMaxMinAmountContainers");



            this.Property(t => t.SpaceLinesBeforeContainers).HasColumnName("SpaceLinesBeforeContainers");
            this.Property(t => t.SpaceLinesBeforePackages).HasColumnName("SpaceLinesBeforePackages");
            this.Property(t => t.SpaceLinesBeforeQuoteHeaders).HasColumnName("SpaceLinesBeforeQuoteHeaders");
            this.Property(t => t.SpaceLinesBeforeQuoteDetails).HasColumnName("SpaceLinesBeforeQuoteDetails");
            this.Property(t => t.SpaceLinesBeforeHeaders).HasColumnName("SpaceLinesBeforeHeaders");
            this.Property(t => t.SpaceLinesBeforeFooters).HasColumnName("SpaceLinesBeforeFooters");
            this.Property(t => t.SpaceLinesBeforePerContainers).HasColumnName("SpaceLinesBeforePerContainers");




            this.Property(t => t.QuoteTemplatePDFMarginBottom).HasColumnName("QuoteTemplatePDFMarginBottom");
            this.Property(t => t.QuoteTemplatePDFMarginTop).HasColumnName("QuoteTemplatePDFMarginTop");


            
            this.Property(t => t.ShowIncludedChargesPackages).HasColumnName("ShowIncludedChargesPackages");
            this.Property(t => t.ShowIncludedChargesContainers).HasColumnName("ShowIncludedChargesContainers");


            this.Property(t => t.ShowVATTypePackages).HasColumnName("ShowVATTypePackages");
            this.Property(t => t.ShowVATTypeContainers).HasColumnName("ShowVATTypeContainers");
            this.Property(t => t.ShowVATPercentagePackages).HasColumnName("ShowVATPercentagePackages");
            this.Property(t => t.ShowVATPercentageContainers).HasColumnName("ShowVATPercentageContainers");


            //#if ORACLE_DB
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.Property(t => t.ShowIncludedChargesPerContainers).HasColumnName("ShowIncludedChargPerContainers");
                this.Property(t => t.ShowTotalInSaleCurrencyPackages).HasColumnName("ShowTotalSaleCurrencyPackages");
                this.Property(t => t.ShowTotalInSaleCurrencyContainers).HasColumnName("ShowTotaInSaleCurrContainers");
                this.Property(t => t.ShowTotalInLocalCurrencyPackages).HasColumnName("ShowTotalLocalCurrencyPackages");
                this.Property(t => t.ShowTotalInLocalCurrencyContainers).HasColumnName("ShowTotalInLocalCurrContainers");


                this.Property(t => t.ShowSaleCurrencyColumnContainers).HasColumnName("ShowSaleCurrColumnContainers");
                this.Property(t => t.ShowLocalCurrencyColumnPackages).HasColumnName("ShowLocalCurrColumnPackages");
                this.Property(t => t.ShowLocalCurrencyColumnContainers).HasColumnName("ShowLocalCurrColumnContainers");

                this.Property(t => t.ShowContainerNameInsteadOfCodeContainers).HasColumnName("ShowContainerNameInContainers");


                this.Property(t => t.GroupByContainsersLabelDesignId).HasColumnName("GroupByContainersLabelDesignId");
                this.Property(t => t.GroupByContainsersValueDesignId).HasColumnName("GroupByContainersValueDesignId");


                this.Property(t => t.PageHeaderArea1FreeTextDesignId).HasColumnName("PageHeaderArea1FreeTxtDesignId");
                this.Property(t => t.PageHeaderArea2FreeTextDesignId).HasColumnName("PageHeaderArea2FreeTxtDesignId");
                this.Property(t => t.PageHeaderArea3FreeTextDesignId).HasColumnName("PageHeaderArea3FreeTxtDesignId");

                this.Property(t => t.PageFooterArea1FreeTextDesignId).HasColumnName("PageFooterArea1FreeTxtDesignId");
                this.Property(t => t.PageFooterArea2FreeTextDesignId).HasColumnName("PageFooterArea2FreeTxtDesignId");
                this.Property(t => t.PageFooterArea3FreeTextDesignId).HasColumnName("PageFooterArea3FreeTxtDesignId");

                this.Property(t => t.PricingContainsersTitleDesignId).HasColumnName("PricingContainserTitleDesignId");


                this.Property(t => t.ShowCodeChargeSaleMinMaxContainers).HasColumnName("ShowCodeChargSaleMinContainers");
                this.Property(t => t.ShowCodeChargeSaleMinMaxPackages).HasColumnName("ShowCodeChargeSaleMinPackages");
                this.Property(t => t.ShowChargeDescriptionContainers).HasColumnName("ShowChargeDescContainers");
                this.Property(t => t.ShowPageBreakBeforeTotalPerContainersTable).HasColumnName("ShowPageBreakBeforeTotalPer");
                this.Property(t => t.TotalPerContainersAdditionalTextDesignId).HasColumnName("TotalPerAdditionalDesignId");
                this.Property(t => t.TotalPerContainersTableDesignId).HasColumnName("TotalPerTableDesignId");
                this.Property(t => t.ShowTitleTotalPerContainersTable).HasColumnName("ShowTitleTotalPerContainers");
                this.Property(t => t.ShowTotalPerChargeGroupPackages).HasColumnName("ShowTotalPerChargeGroupPacks");
                this.Property(t => t.ShowTotalPerChargeGroupContainers).HasColumnName("ShowTotalPerChargeGroupConts");

            }

            //#else

            else
            {
                this.Property(t => t.ShowIncludedChargesPerContainers).HasColumnName("ShowIncludedChargesPerContainers");
                this.Property(t => t.ShowTotalInSaleCurrencyPackages).HasColumnName("ShowTotalInSaleCurrencyPackages");
                this.Property(t => t.ShowTotalInSaleCurrencyContainers).HasColumnName("ShowTotalInSaleCurrencyContainers");
                this.Property(t => t.ShowTotalInLocalCurrencyPackages).HasColumnName("ShowTotalInLocalCurrencyPackages");
                this.Property(t => t.ShowTotalInLocalCurrencyContainers).HasColumnName("ShowTotalInLocalCurrencyContainers");


                this.Property(t => t.ShowSaleCurrencyColumnContainers).HasColumnName("ShowSaleCurrencyColumnContainers");
                this.Property(t => t.ShowLocalCurrencyColumnPackages).HasColumnName("ShowLocalCurrencyColumnPackages");
                this.Property(t => t.ShowLocalCurrencyColumnContainers).HasColumnName("ShowLocalCurrencyColumnContainers");

                this.Property(t => t.ShowContainerNameInsteadOfCodeContainers).HasColumnName("ShowContainerNameInsteadOfCodeContainers");


                this.Property(t => t.GroupByContainsersLabelDesignId).HasColumnName("GroupByContainsersLabelDesignId");
                this.Property(t => t.GroupByContainsersValueDesignId).HasColumnName("GroupByContainsersValueDesignId");


                this.Property(t => t.PageHeaderArea1FreeTextDesignId).HasColumnName("PageHeaderArea1FreeTextDesignId");
                this.Property(t => t.PageHeaderArea2FreeTextDesignId).HasColumnName("PageHeaderArea2FreeTextDesignId");
                this.Property(t => t.PageHeaderArea3FreeTextDesignId).HasColumnName("PageHeaderArea3FreeTextDesignId");

                this.Property(t => t.PageFooterArea1FreeTextDesignId).HasColumnName("PageFooterArea1FreeTextDesignId");
                this.Property(t => t.PageFooterArea2FreeTextDesignId).HasColumnName("PageFooterArea2FreeTextDesignId");
                this.Property(t => t.PageFooterArea3FreeTextDesignId).HasColumnName("PageFooterArea3FreeTextDesignId");

                this.Property(t => t.PricingContainsersTitleDesignId).HasColumnName("PricingContainsersTitleDesignId");


            this.Property(t => t.ShowCodeChargeSaleMinMaxContainers).HasColumnName("ShowCodeChargeSaleMinMaxContainers");
            this.Property(t => t.ShowCodeChargeSaleMinMaxPackages).HasColumnName("ShowCodeChargeSaleMinMaxPackages");
            this.Property(t => t.ShowTotalPerChargeGroupPackages).HasColumnName("ShowTotalPerChargeGroupPackages");
            this.Property(t => t.ShowTotalPerChargeGroupContainers).HasColumnName("ShowTotalPerChargeGroupContainers");
            this.Property(t => t.ShowPageBreakBeforeTotalPerContainersTable).HasColumnName("ShowPageBreakBeforeTotalPerContainersTable");
            this.Property(t => t.TotalPerContainersAdditionalTextDesignId).HasColumnName("TotalPerContainersAdditionalTextDesignId");
            this.Property(t => t.TotalPerContainersTableDesignId).HasColumnName("TotalPerContainersTableDesignId");
            this.Property(t => t.ShowTitleTotalPerContainersTable).HasColumnName("ShowTitleTotalPerContainersTable");

            this.Property(t => t.ShowHeaderLabelsPackages).HasColumnName("ShowHeaderLabelsPackages");
           this.Property(t => t.ShowHeaderLabelsContainers).HasColumnName("ShowHeaderLabelsContainers");
            }




//#endif














            this.HasOptional(t => t.DetailsTableDesign)
               .WithMany()
               .HasForeignKey(d => d.DetailsTableDesignId);











            this.HasOptional(t => t.HeaderTableDesign)
                .WithMany()
                .HasForeignKey(d => d.HeaderTableDesignId);








            this.HasRequired(t => t.PackagesTableDesign)
                .WithMany()
                .HasForeignKey(d => d.PackagesTableDesignId);

            this.HasRequired(t => t.ContainserTableDesign)
                .WithMany()
                .HasForeignKey(d => d.ContainserTableDesignId);





            this.HasOptional(t => t.DetailsTitleDesign)
                .WithMany()
                .HasForeignKey(d => d.DetailsTitleDesignId);



            this.HasRequired(t => t.QuoteTemplateTextDesignPackagesLabel)
                .WithMany()
                .HasForeignKey(d => d.TotalsPackagesLabelDesignId);

            this.HasRequired(t => t.QuoteTemplateTextDesignContainsersLabel)
                .WithMany()
                .HasForeignKey(d => d.TotalsContainsersLabelDesignId);


            this.HasRequired(t => t.QuoteTemplateTextDesignPackagesValue)
                .WithMany()
                .HasForeignKey(d => d.TotalsPackagesValueDesignId);

            this.HasRequired(t => t.QuoteTemplateTextDesignContainsersValue)
                .WithMany()
                .HasForeignKey(d => d.TotalsContainsersValueDesignId);




            this.HasOptional(t => t.QuoteTemplateTextDesignPackagesGroupByLabel)
            .WithMany()
            .HasForeignKey(d => d.GroupByPackagesLabelDesignId);

            this.HasOptional(t => t.QuoteTemplateTextDesignPackagesGroupByValue)
                .WithMany()
                .HasForeignKey(d => d.GroupByPackagesValueDesignId);

            this.HasOptional(t => t.QuoteTemplateTextDesignContainsersGroupByLabel)
                .WithMany()
                .HasForeignKey(d => d.GroupByContainsersLabelDesignId);

            this.HasOptional(t => t.QuoteTemplateTextDesignContainsersGroupByValue)
                .WithMany()
                .HasForeignKey(d => d.GroupByContainsersValueDesignId);



            this.HasOptional(t => t.PageHeaderArea1ImageDetail)
                .WithMany()
                .HasForeignKey(d => d.PageHeaderArea1ImageDetailId);



            this.HasOptional(t => t.PageHeaderArea2ImageDetail)
                .WithMany()
                .HasForeignKey(d => d.PageHeaderArea2ImageDetailId);



            this.HasOptional(t => t.PageHeaderArea3ImageDetail)
                .WithMany()
                .HasForeignKey(d => d.PageHeaderArea3ImageDetailId);





            this.HasOptional(t => t.PageHeaderArea1FreeTextDesign)
                .WithMany()
                .HasForeignKey(d => d.PageHeaderArea1FreeTextDesignId);





            this.HasOptional(t => t.PageHeaderArea2FreeTextDesign)
                .WithMany()
                .HasForeignKey(d => d.PageHeaderArea2FreeTextDesignId);




            this.HasOptional(t => t.PageHeaderArea3FreeTextDesign)
                .WithMany()
                .HasForeignKey(d => d.PageHeaderArea3FreeTextDesignId);




            this.HasOptional(t => t.PageFooterArea1ImageDetail)
                            .WithMany()
                            .HasForeignKey(d => d.PageFooterArea1ImageDetailId);



            this.HasOptional(t => t.PageFooterArea2ImageDetail)
                .WithMany()
                .HasForeignKey(d => d.PageFooterArea2ImageDetailId);



            this.HasOptional(t => t.PageFooterArea3ImageDetail)
                .WithMany()
                .HasForeignKey(d => d.PageFooterArea3ImageDetailId);





            this.HasOptional(t => t.PageFooterArea1FreeTextDesign)
                .WithMany()
                .HasForeignKey(d => d.PageFooterArea1FreeTextDesignId);





            this.HasOptional(t => t.PageFooterArea2FreeTextDesign)
                .WithMany()
                .HasForeignKey(d => d.PageFooterArea2FreeTextDesignId);




            this.HasOptional(t => t.PageFooterArea3FreeTextDesign)
                .WithMany()
                .HasForeignKey(d => d.PageFooterArea3FreeTextDesignId);






            this.HasOptional(t => t.PricingPackagesTitleDesign)
                 .WithMany()
                .HasForeignKey(d => d.PricingPackagesTitleDesignId);



            this.HasOptional(t => t.PricingContainsersTitleDesign)
                 .WithMany()
                .HasForeignKey(d => d.PricingContainsersTitleDesignId);





            this.HasOptional(t => t.TotalPerContainersTableDesign)
                 .WithMany()
                .HasForeignKey(d => d.TotalPerContainersTableDesignId);




            this.HasOptional(t => t.TotalPerContainersAdditionalTextDesign)
                 .WithMany()
                .HasForeignKey(d => d.TotalPerContainersAdditionalTextDesignId);

        }





        //[ForeignKey("PageHeaderArea1mageDetailId")]
        //public virtual ImageDetail PageHeaderArea1mageDetail { get; set; }


        //[ForeignKey("PageHeaderArea2mageDetailId")]
        //public virtual ImageDetail PageHeaderArea2mageDetail { get; set; }

        //[ForeignKey("PageHeaderArea3mageDetailId")]
        //public virtual ImageDetail PageHeaderArea3mageDetail { get; set; }



    }
}
