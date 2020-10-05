using Logitude.BL.Helpers;
using Logitude.BL.QuoteModel.EntityLists;
using Logitude.BL.QuoteModel.EntityOtherServices;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityQueries;
using Logitude.BL.QuoteModel.Tools.EntityService;
using Logitude.Server.Tools.Counters;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using WebFreight.Web.Security;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.Helpers
{
    public class QuoteTemplateHelper
    {
        
        //TextDesign
        QuoteTemplateTextDesign headerDesignPackagesTableDesign = null;
        QuoteTemplateTextDesign linesDesignPackagesTableDesign = null;
        QuoteTemplateTextDesign groupByDesignPackagesTableDesign = null;
        QuoteTemplateTextDesign headerDesignContainersTableDesign = null;
        QuoteTemplateTextDesign linesDesignContainersTableDesign = null;
        QuoteTemplateTextDesign groupByDesignContainersTableDesign = null;
        QuoteTemplateTextDesign totalsPackagesLabelTextDesign = null;
        QuoteTemplateTextDesign totalsPackagesValueTextDesign = null;
        QuoteTemplateTextDesign totalsContainersLabelTextDesign = null;
        QuoteTemplateTextDesign totalsContainersTextDesign = null;
        QuoteTemplateTextDesign quoteTemplateTextDesignPackagesGroupByLabel = null;
        QuoteTemplateTextDesign quoteTemplateTextDesignContainsersGroupByLabel = null;
        QuoteTemplateTextDesign quoteTemplateTextDesignPackagesGroupByValue = null;
        QuoteTemplateTextDesign quoteTemplateTextDesignContainsersGroupByValue = null;
        QuoteTemplateTextDesign detailsTitleDesign = null;
        QuoteTemplateTextDesign pricingPackagesTitleDesign = null;
        QuoteTemplateTextDesign pricingContainersTitleDesign = null;
        QuoteTemplateTextDesign headerDesignDetailsTableDesign = null;
        QuoteTemplateTextDesign linesDesignDetailsTableDesign = null;
        QuoteTemplateTextDesign groupByDesignDetailsTableDesign = null;
        QuoteTemplateTextDesign headerDesignHeaderTableDesign = null;
        QuoteTemplateTextDesign linesDesignHeaderTableDesign = null;
        QuoteTemplateTextDesign groupByDesignHeaderTableDesign = null;
        QuoteTemplateTextDesign textDesignTextArea1Value = null;
        QuoteTemplateTextDesign textDesignTextArea2Value = null;
        QuoteTemplateTextDesign textDesignTextArea3Value = null;
        QuoteTemplateTextDesign textDesignFooterArea1Value = null;
        QuoteTemplateTextDesign textDesignFooterArea2Value = null;
        QuoteTemplateTextDesign textDesignFooterArea3Value = null;
        QuoteTemplateTextDesign totalPerContainersAdditionalTextDesign = null;
        QuoteTemplateTextDesign headerDesignTotalPerContainersTable = null;
        QuoteTemplateTextDesign linesDesignTotalPerContainersTable  = null;
        QuoteTemplateTextDesign groupByDesignHeaderTotalPerContainersTable = null;
        QuoteTemplateTextDesign headerDesignPageNumberingTable = null;
        QuoteTemplateTextDesign linesDesignPageNumberingTable = null;
        QuoteTemplateTextDesign groupByDesignHeaderPageNumberingTable = null;

        //TableDesign
        QuoteTemplateTableDesign packagesTableDesign = null;
        QuoteTemplateTableDesign containersTableDesign = null;
        QuoteTemplateTableDesign headerTableDesign = null;
        QuoteTemplateTableDesign detailsTableDesign = null;
        QuoteTemplateTableDesign totalPerContainersTableDesign = null;
        QuoteTemplateTableDesign pageNumberingTableDesign = null;


        QuoteTemplateSetting quoteTemplateSetting = null;
        List<QuoteTemplateTextCodePM> QuoteTemplateTextCodeLists = new List<QuoteTemplateTextCodePM>();
        List<QuoteTemplateSectionPM> QuoteTemplateSectionLists = new List<QuoteTemplateSectionPM>();
        QuoteTemplateEntityService quoteTemplateEntityService = null;
        QuoteTemplateReportHelper quoteTemplateReportHelper = null;

        int Tenant = 0;
        string QuoteTemplateId = String.Empty;


        #region CreateQupteTemplate
        public QuoteTemplatePM CreateQuoteTemplate(QuoteTemplatePM quoteTemplatePM)
        {

            if (quoteTemplatePM != null)
            {
                SecurityUtility.CheckContactFeature("QuoteTemplate", "NEW", quoteTemplatePM.Tenant);
                quoteTemplateEntityService = new QuoteTemplateEntityService();

                Tenant = quoteTemplatePM.Tenant;
                CreateQuoteTemplateTextDesign();
                CreateQuoteTemplateTableDesign();
                CreateQuoteTemplateSetting();

                if(quoteTemplateSetting!=null) quoteTemplatePM.QuoteTemplateSettingId = quoteTemplateSetting.Id;
                IQuotesContext objectContext = QuotesContext.GetContext(Tenant);
                QuoteTemplateService quoteTemplateService = new QuoteTemplateService(objectContext, Tenant);
                quoteTemplateService.Create(quoteTemplatePM);
                CreateQuoteTemplateSection(quoteTemplatePM.Id, Tenant);
                CreateQuoteTemplateTextCode(quoteTemplatePM, Tenant);

            }


            return quoteTemplatePM;
        }

        private void CreateQuoteTemplateTextDesign()
        {
            QuoteTemplateTextDesignRepository quoteTemplateTextDesignRepository = new QuoteTemplateTextDesignRepository(Tenant);

            headerDesignPackagesTableDesign = GetNewQuoteTemplateTextDesign();
            linesDesignPackagesTableDesign = GetNewQuoteTemplateTextDesign();
            groupByDesignPackagesTableDesign = GetNewQuoteTemplateTextDesign();
            headerDesignContainersTableDesign = GetNewQuoteTemplateTextDesign();
            linesDesignContainersTableDesign = GetNewQuoteTemplateTextDesign();
            groupByDesignContainersTableDesign = GetNewQuoteTemplateTextDesign();
            totalsPackagesLabelTextDesign = GetNewQuoteTemplateTextDesign();
            totalsPackagesValueTextDesign = GetNewQuoteTemplateTextDesign();
            totalsContainersLabelTextDesign = GetNewQuoteTemplateTextDesign();
            totalsContainersTextDesign = GetNewQuoteTemplateTextDesign();
            quoteTemplateTextDesignPackagesGroupByLabel = GetNewQuoteTemplateTextDesign();
            quoteTemplateTextDesignContainsersGroupByLabel = GetNewQuoteTemplateTextDesign();
            quoteTemplateTextDesignPackagesGroupByValue = GetNewQuoteTemplateTextDesign();
            quoteTemplateTextDesignContainsersGroupByValue = GetNewQuoteTemplateTextDesign();
            detailsTitleDesign = GetNewQuoteTemplateTextDesign();
            pricingPackagesTitleDesign = GetNewQuoteTemplateTextDesign();
            pricingContainersTitleDesign = GetNewQuoteTemplateTextDesign();
            headerDesignDetailsTableDesign = GetNewQuoteTemplateTextDesign();
            linesDesignDetailsTableDesign = GetNewQuoteTemplateTextDesign();
            groupByDesignDetailsTableDesign = GetNewQuoteTemplateTextDesign();
            headerDesignHeaderTableDesign = GetNewQuoteTemplateTextDesign();
            linesDesignHeaderTableDesign = GetNewQuoteTemplateTextDesign();
            groupByDesignHeaderTableDesign = GetNewQuoteTemplateTextDesign();
            textDesignTextArea1Value = GetNewQuoteTemplateTextDesign();
            textDesignTextArea2Value = GetNewQuoteTemplateTextDesign();
            textDesignTextArea3Value = GetNewQuoteTemplateTextDesign();
            textDesignFooterArea1Value = GetNewQuoteTemplateTextDesign();
            textDesignFooterArea2Value = GetNewQuoteTemplateTextDesign();
            textDesignFooterArea3Value = GetNewQuoteTemplateTextDesign();
            totalPerContainersAdditionalTextDesign = GetNewQuoteTemplateTextDesign();
            headerDesignTotalPerContainersTable = GetNewQuoteTemplateTextDesign();
            linesDesignTotalPerContainersTable = GetNewQuoteTemplateTextDesign();
            groupByDesignHeaderTotalPerContainersTable = GetNewQuoteTemplateTextDesign();
            headerDesignPageNumberingTable = GetNewQuoteTemplateTextDesign();
            linesDesignPageNumberingTable = GetNewQuoteTemplateTextDesign();
            groupByDesignHeaderPageNumberingTable = GetNewQuoteTemplateTextDesign();

            quoteTemplateTextDesignRepository.Add(headerDesignPackagesTableDesign);
            quoteTemplateTextDesignRepository.Add(linesDesignPackagesTableDesign);
            quoteTemplateTextDesignRepository.Add(groupByDesignPackagesTableDesign);
            quoteTemplateTextDesignRepository.Add(headerDesignContainersTableDesign);
            quoteTemplateTextDesignRepository.Add(linesDesignContainersTableDesign);
            quoteTemplateTextDesignRepository.Add(groupByDesignContainersTableDesign);
            quoteTemplateTextDesignRepository.Add(totalsPackagesLabelTextDesign);
            quoteTemplateTextDesignRepository.Add(totalsPackagesValueTextDesign);
            quoteTemplateTextDesignRepository.Add(totalsContainersLabelTextDesign);
            quoteTemplateTextDesignRepository.Add(totalsContainersTextDesign);
            quoteTemplateTextDesignRepository.Add(quoteTemplateTextDesignPackagesGroupByLabel);
            quoteTemplateTextDesignRepository.Add(quoteTemplateTextDesignContainsersGroupByLabel);
            quoteTemplateTextDesignRepository.Add(quoteTemplateTextDesignPackagesGroupByValue);
            quoteTemplateTextDesignRepository.Add(quoteTemplateTextDesignContainsersGroupByValue);
            quoteTemplateTextDesignRepository.Add(detailsTitleDesign);
            quoteTemplateTextDesignRepository.Add(pricingPackagesTitleDesign);
            quoteTemplateTextDesignRepository.Add(pricingContainersTitleDesign);
            quoteTemplateTextDesignRepository.Add(headerDesignDetailsTableDesign);
            quoteTemplateTextDesignRepository.Add(linesDesignDetailsTableDesign);
            quoteTemplateTextDesignRepository.Add(groupByDesignDetailsTableDesign);
            quoteTemplateTextDesignRepository.Add(headerDesignHeaderTableDesign);
            quoteTemplateTextDesignRepository.Add(linesDesignHeaderTableDesign);
            quoteTemplateTextDesignRepository.Add(groupByDesignHeaderTableDesign);
            quoteTemplateTextDesignRepository.Add(textDesignTextArea1Value);
            quoteTemplateTextDesignRepository.Add(textDesignTextArea2Value);
            quoteTemplateTextDesignRepository.Add(textDesignTextArea3Value);
            quoteTemplateTextDesignRepository.Add(textDesignFooterArea1Value);
            quoteTemplateTextDesignRepository.Add(textDesignFooterArea2Value);
            quoteTemplateTextDesignRepository.Add(textDesignFooterArea3Value);
            quoteTemplateTextDesignRepository.Add(totalPerContainersAdditionalTextDesign);
            quoteTemplateTextDesignRepository.Add(headerDesignTotalPerContainersTable);
            quoteTemplateTextDesignRepository.Add(linesDesignTotalPerContainersTable);
            quoteTemplateTextDesignRepository.Add(groupByDesignHeaderTotalPerContainersTable);
            quoteTemplateTextDesignRepository.Add(headerDesignPageNumberingTable);
            quoteTemplateTextDesignRepository.Add(linesDesignPageNumberingTable);
            quoteTemplateTextDesignRepository.Add(groupByDesignHeaderPageNumberingTable);
            quoteTemplateTextDesignRepository.SubmitChanges();


        }

        private void CreateQuoteTemplateTableDesign()
        {
            QuoteTemplateTableDesignRepository quoteTemplateTableDesignRepository = new QuoteTemplateTableDesignRepository(Tenant);
            
            packagesTableDesign = GetNewQuoteTemplateTableDesign(headerDesignPackagesTableDesign.Id , linesDesignPackagesTableDesign.Id , groupByDesignPackagesTableDesign.Id);
            containersTableDesign = GetNewQuoteTemplateTableDesign(headerDesignContainersTableDesign.Id, linesDesignContainersTableDesign.Id, groupByDesignContainersTableDesign.Id);
            headerTableDesign = GetNewQuoteTemplateTableDesign(headerDesignHeaderTableDesign.Id, linesDesignHeaderTableDesign.Id, groupByDesignHeaderTableDesign.Id);
            detailsTableDesign = GetNewQuoteTemplateTableDesign(headerDesignDetailsTableDesign.Id, linesDesignDetailsTableDesign.Id, headerDesignDetailsTableDesign.Id);
            totalPerContainersTableDesign = GetNewQuoteTemplateTableDesign(headerDesignTotalPerContainersTable.Id, linesDesignTotalPerContainersTable.Id, groupByDesignHeaderTotalPerContainersTable.Id);
            pageNumberingTableDesign = GetNewQuoteTemplateTableDesign(headerDesignPageNumberingTable.Id, linesDesignPageNumberingTable.Id, groupByDesignHeaderPageNumberingTable.Id);

            quoteTemplateTableDesignRepository.Add(packagesTableDesign);
            quoteTemplateTableDesignRepository.Add(containersTableDesign);
            quoteTemplateTableDesignRepository.Add(headerTableDesign);
            quoteTemplateTableDesignRepository.Add(detailsTableDesign);
            quoteTemplateTableDesignRepository.Add(totalPerContainersTableDesign);
            quoteTemplateTableDesignRepository.Add(pageNumberingTableDesign);
            quoteTemplateTableDesignRepository.SubmitChanges();
        }

        private void CreateQuoteTemplateSetting()
        {
            QuoteTemplateSettingRepository quoteTemplateSettingRepository = new QuoteTemplateSettingRepository(Tenant);

            quoteTemplateSetting = new QuoteTemplateSetting()
            {
                Id = IdCounter.GetNumber("QuoteTemplateSetting", Tenant).ToString(),
                Tenant = Tenant,
                ShowChargeCodeContainers = false,
                ShowChargeCodePackages = false,
                ShowChargeDescriptionPackages = false,
                ShowChargeDescriptionContainers = false,
                ShowChargeNameContainers = true,
                ShowChargeNamePackages = true,
                ShowContainerNameInsteadOfCodeContainers = false,
                ShowFixedPriceContainers = true,
                ShowLocalCurrencyColumnContainers = true,
                ShowLocalCurrencyColumnPackages = true,
                ShowLocalLanguage = false,
                RightToLeft = false,
                ShowMeasurementContainers = true,
                ShowMeasurementPackages = true,
                ShowPricesTableContainers = true,
                ShowPricesTablePackages = true,
                ShowSaleCurrencyColumnContainers = true,
                ShowSaleCurrencyColumnPackages = true,
                ShowTotalInLocalCurrencyContainers = false,
                ShowTotalInLocalCurrencyPackages = false,
                ShowTotalInSaleCurrencyContainers = false,
                ShowTotalInSaleCurrencyPackages = false,
                ShowUnitPricePackages = true,
                ShowUnitsPackages = true,
                AlignRight = false,
                SplitChargesbyGroupsContainers = false,
                SplitChargesbyGroupsPackages = false,
                PackagesTableDesignId = packagesTableDesign.Id,
                ContainserTableDesignId = containersTableDesign.Id,
                TotalsPackagesLabelDesignId = totalsPackagesLabelTextDesign.Id,
                TotalsPackagesValueDesignId = totalsPackagesValueTextDesign.Id,
                TotalsContainsersLabelDesignId = totalsContainersLabelTextDesign.Id,
                TotalsContainsersValueDesignId = totalsContainersTextDesign.Id,
                GroupByPackagesLabelDesignId = quoteTemplateTextDesignPackagesGroupByLabel.Id,
                GroupByContainsersLabelDesignId = quoteTemplateTextDesignContainsersGroupByLabel.Id,
                GroupByPackagesValueDesignId = quoteTemplateTextDesignPackagesGroupByValue.Id,
                GroupByContainsersValueDesignId = quoteTemplateTextDesignContainsersGroupByValue.Id,
                DetailsTableDesignId = detailsTableDesign.Id,
                PageHeaderArea1FreeTextDesignId = textDesignTextArea1Value.Id,
                PageHeaderArea2FreeTextDesignId = textDesignTextArea2Value.Id,
                PageHeaderArea3FreeTextDesignId = textDesignTextArea3Value.Id,
                PageFooterArea1FreeTextDesignId = textDesignFooterArea1Value.Id,
                PageFooterArea2FreeTextDesignId = textDesignFooterArea2Value.Id,
                PageFooterArea3FreeTextDesignId = textDesignFooterArea3Value.Id,
                HeaderTableDesignId = headerTableDesign.Id,
                DetailsSectionHasTwoColumns = false,
                HeaderSectionHasTwoColumns = false,
                DetailsTitleDesignId = detailsTitleDesign.Id,
                PricingContainsersTitleDesignId = pricingContainersTitleDesign.Id,
                PricingPackagesTitleDesignId = pricingPackagesTitleDesign.Id,
                ShowHeaderQuoteDate = true,
                ShowHeaderExpirationDate = true,
                ShowHeaderQuoteNumber = true,
                ShowTitlePricingPackages = false,
                ShowTitlePricingContainsers = false,
                ShowHeaderCustomer = true,
                ShowDetailsExpirationDate = true,
                ShowDetailsExpirationDays = true,
                ShowDetailsShipperName = true,
                ShowDetailsShipperAddress = false,
                ShowDetailsShipperContact = false,
                ShowDetailsShipperReferences = false,
                ShowDetailsConsigneeName = true,
                ShowDetailsConsigneeAddress = false,
                ShowDetailsConsigneeContact = false,
                ShowDetailsConsigneeReferences = false,
                ShowDetailsPickupFrom = true,
                ShowDetailsDeliveryTo = true,
                ShowDetailsFromPort = true,
                ShowDetailsToPort = true,
                ShowDetailsIncoterms = false,
                ShowDetailsService = false,
                ShowDetailsSalesMan = false,
                ShowDetailsDescriptionOfGoods = true,
                ShowDetailsDangerousGoods = false,
                ShowDetailsCarrier = true,
                ShowCodeChargeSaleMinMaxPackages = true,
                ShowCodeChargeSaleMinMaxContainers = true,
                ShowPriceByContainerColumn = true,
                PageHeaderImage1Width = 70,
                PageHeaderImage2Width = 70,
                PageHeaderImage3Width = 70,
                PageHeaderArea1Height = 70,
                PageHeaderArea2Height = 70,
                PageHeaderArea3Height = 70,
                PageHeaderArea1Width = 40,
                PageHeaderArea2Width = 40,
                PageHeaderArea3Width = 20,
                PageHeaderArea1ImageAlignment = "center",
                PageHeaderArea2ImageAlignment = "center",
                PageHeaderArea3ImageAlignment = "center",
                PageHeaderBorderColor = "#FF000000",
                PageHeaderBorderThickness = 1,
                PageHeaderBorderTypeCode = "ALL",
                PageHeaderAreaHeight = 4,
                PageHeaderArea1Type = "Text",
                PageHeaderArea2Type = "Text",
                PageHeaderArea3Type = "Text",
                PageFooterArea1Height = 70,
                PageFooterArea2Height = 70,
                PageFooterArea3Height = 70,
                PageFooterImage1Width = 70,
                PageFooterImage2Width = 70,
                PageFooterImage3Width = 70,
                PageFooterArea1Width = 35,
                PageFooterArea2Width = 35,
                PageFooterArea3Width = 30,
                PageFooterBorderColor = "#FF000000",
                PageFooterBorderTypeCode = "ALL",
                PageFooterBorderThickness = 1,
                PageFooterArea1ImageAlignment = "center",
                PageFooterArea2ImageAlignment = "center",
                PageFooterArea3ImageAlignment = "center",
                PageFooterArea1Type = "Text",
                PageFooterArea2Type = "Text",
                PageFooterArea3Type = "Logo",
                PageFooterAreaHeight = 4,
                HeaderTableColumWidthType = "FIXED",
                DetailsTableColumWidthType = "FIXED",
                HeaderTableColumn1LabelWidth = 25,
                HeaderTableColumn1ValueWidth = 25,
                HeaderTableColumn2LabelWidth = 25,
                HeaderTableColumn2ValueWidth = 25,
                DetailsTableColumn1LabelWidth = 25,
                DetailsTableColumn1ValueWidth = 25,
                DetailsTableColumn2LabelWidth = 25,
                DetailsTableColumn2ValueWidth = 25,
                TotalPerContainersAdditionalTextDesignId = totalPerContainersAdditionalTextDesign.Id,
                TotalPerContainersCurrencyType = "SALE",
                TotalPerContainersTableDesignId = totalPerContainersTableDesign.Id,
                ShowSaleMaxMinAmountContainers = true,
                ShowSaleMaxMinAmountPackages = true,
                ShowHeaderLabelsContainers = true,
                ShowHeaderLabelsPackages = true,
                SpaceLinesBeforeContainers = 1,
                SpaceLinesBeforeFooters = 1,
                SpaceLinesBeforeHeaders = 1,
                SpaceLinesBeforePackages = 1,
                SpaceLinesBeforeQuoteDetails = 1,
                SpaceLinesBeforeQuoteHeaders = 1,
                SpaceLinesBeforePerContainers = 1,
                HidePageNumber = false,
                PageNumberingTextDesignId = pageNumberingTableDesign.Id,

            };


            quoteTemplateSettingRepository.Add(quoteTemplateSetting);
            quoteTemplateSettingRepository.SubmitChanges();

        }

 
        
        private QuoteTemplateTextDesign GetNewQuoteTemplateTextDesign()
        {
            QuoteTemplateTextDesign quoteTemplateTextDesign = new QuoteTemplateTextDesign()
            {
                Id = IdCounter.GetNumber("QuoteTemplateTextDesign", Tenant).ToString(),
                Tenant = Tenant,
                FontSize = 12,
                FontFamily = "Arial",
                FontWeight = "normal",
                Alignment = "left",
                TextColor = "#FF000000",
                BackgroundColor = "#FFFFFFFF",
                UnDerLine = true,
                Italic = false,
            };

            return quoteTemplateTextDesign;
        }

        private QuoteTemplateTableDesign GetNewQuoteTemplateTableDesign(string headerDesignId, string linesDesignId, string groupByDesignId)
        {
            QuoteTemplateTableDesign quoteTemplateTableDesign = new QuoteTemplateTableDesign()
            {
                Id = IdCounter.GetNumber("QuoteTemplateTableDesign", Tenant).ToString(),
                Tenant = Tenant,
                BorderTypeCode = "ALL",
                BorderColor = "#FF000000",
                BorderThickness = 1,
                HeaderDesignId = headerDesignId,
                LinesDesignId = linesDesignId,
                GroupByDesignId = groupByDesignId,
            };

            return quoteTemplateTableDesign;
        }

        #endregion

        #region CreateQuoteTemplateTextCode

        private void CreateQuoteTemplateTextCode(QuoteTemplatePM quoteTemplatePM, int tenant)
        {
            Tenant = tenant;
            QuoteTemplateId = quoteTemplatePM.Id;

            QuoteTemplateTextCodeRepository quoteTemplateTextCodeRepository = new QuoteTemplateTextCodeRepository(Tenant);

            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("CHARGECODEPACKAGES", "Charge Code", "Charge Code", "Packages"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("CHARGECODECONTAINERS", "Charge Code", "Charge Code", "Containers"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("CHARGEDESCRIPTIONPACKAGES", "Charge Description", "Charge Description", "Packages"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("CHARGEDESCRIPTIONCONTAINERS", "Charge Description", "Charge Description", "Containers"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("CHARGEPACKAGES", "Charge", "Charge", "Packages"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("CHARGECONTAINERS", "Charge", "Charge", "Containers"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("UNITSPACKAGES", "Units", "Units", "Packages"));
            string unitPriceLable = quoteTemplatePM.TemplateTypeCode == "P" ? "Step: Unit Price" : "Unit Price";
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("UNITPRICEPACKAGES", unitPriceLable, unitPriceLable, "Packages"));


            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("MEASUREMENTPACKAGES", "Measurement", "Measurement", "Packages"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("MEASUREMENTCONTAINERS", "Measurement", "Measurement", "Containers"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("TOTALPACKAGES", "Total", "Total", "Packages"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("TOTALCONTAINERS", "Total", "Total", "Containers"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("LOCALAMOUNTPACKAGES", "Local Amount", "Local Amount", "Packages"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("LOCALAMOUNTCONTAINERS", "Local Amount", "Local Amount", "Containers"));

            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("FIXEDPRICECONTAINERS", "Fixed Price", "Fixed Price", "Containers"));


            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("TOTALAMOUNTS", "Estimated total based on the above weight/volume", "Estimated total based on the above weight/volume", "Packages"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("TOTALAMOUNTS", "Estimated total based on the above weight/volume", "Estimated total based on the above weight/volume", "Containers"));

            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("CHARGENOTEPACKAGES", "Charge Notes", "Charge Notes", "Packages"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("CHARGENOTECONTAINERS", "Charge Notes", "Charge Notes", "Containers"));


            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("SALEMINMAXPACKAGES", "Min/Max", "Min/Max", "Packages"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("SALEMINMAXCONTAINERS", "Min/Max", "Min/Max", "Containers"));


            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("VATPERCENTAGEPACKAGES", "VAT Percentage", "VAT Percentage", "Packages"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("VATPERCENTAGECONTAINERS", "VAT Percentage", "VAT Percentage", "Containers"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("VATTYPEPACKAGES", "VAT Type", "VAT Type", "Packages"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("VATTYPECONTAINERS", "VAT Type", "VAT Type", "Containers"));


            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("QUOTEDATE", "Quote Date", "Quote Date", "QuoteHeader"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("EXPIRATIONDATE", "Expiration Date", "Expiration Date", "QuoteHeader"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("QUOTENUMBER", "Quote Number", "Quote Number", "QuoteHeader"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("CUSTOMER", "Customer", "Customer", "QuoteHeader"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("ATTN", "ATTN", "ATTN", "QuoteHeader"));


            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("EXPIRATIONDATE", "Expiration Date", "Expiration Date", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("EXPIRATIONDAYS", "Expiration Days", "Expiration Days", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("SHIPPERNAME", "Shipper Name", "Shipper Name", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("SHIPPERADDRESS", "Shipper Address", "Shipper Address", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("SHIPPERCONTACT", "Shipper Contact", "Shipper Contact", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("SHIPPERREFERENCES", "Shipper References", "Shipper References", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("CONSIGNEENAME", "Consignee Name", "Consignee Name", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("CONSIGNEEADDRESS", "Consignee Address", "Consignee Address", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("CONSIGNEECONTACT", "Consignee Contact", "Consignee Contact", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("CONSIGNEEREFERENCES", "Consignee References", "Consignee References", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("CUSTOMERNAME", "Customer Name", "Customer Name", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("CUSTOMERADDRESS", "Customer Address", "Customer Address", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("CUSTOMERCONTACT", "Customer Contact", "Customer Contact", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("CUSTOMERREFERENCES", "Customer References", "Customer References", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("PICKUPFROM", "Pickup From", "Pickup From", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("FROMPORT", "From Port", "From Port", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("DELIVERYTO", "Delivery To", "Delivery To", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("TOPORT", "To Port", "To Port", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("INCOTERMS", "Incoterms", "Incoterms", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("SERVICE", "Service", "Service", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("SALESMAN", "Sales Man", "Sales Man", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("DESCRIPTIONOFGOODS", "Description Of Goods", "Description Of Goods", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("DANGEROUSGOODS", "Dangerous Goods", "Dangerous Goods", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("GENERALDETAILS", "General Details", "General Details", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("QUOTENUMBER", "Quote Number", "Quote Number", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("SHIPINFLINE", "Shipingline", "Shipingline", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("TRUCKER", "Trucker", "Trucker", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("AIRLINE", "Airline", "Airline", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("FROMLOCATION", "From Location", "From Location", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("TOLOCATION", "To Location", "To Location", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("PRICINGPACKAGES", "Pricing Packages", "Pricing Packages", "Packages"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("PRICINGCONTAINERS", "Pricing Containers", "Pricing Containers", "Containers"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("CHARGEABLEWEIGHT", "Chargeable Weight", "Chargeable Weight", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("GROSSWEIGHT", "Gross Weight", "Gross Weight", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("VOLUME", "Volume", "Volume", "QuoteDetails"));
            //quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("VOLUMETRICWEIGHT", "Volumetric Weight", "Volumetric Weight", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("NUMBEROFPACKAGES", "Number Of Packages", "Number Of Packages", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("NUMBEROFCONTAINERS", "Number Of Containers", "Number Of Containers", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("TRANSITTIME", "Transit Time", "Transit Time", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("MOVETYPE", "Move Type", "Move Type", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("CHARGEGROUP", "Charge Group", "Charge Group", "TotalPerContainers"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("TOTALPERCONTAINERS", "Total Per Containers", "Total Per Containers", "TotalPerContainers"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("NOTIFYNAME", "Notify Name", "Notify Name", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("NOTIFYADDRESS", "Notify Address", "Notify Address", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("NOTIFYCONTACT", "Notify Contact", "Notify Contact", "QuoteDetails"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("DEPARTUREFREQUENCY", "Departure Frequency", "Departure Frequency", "QuoteDetails"));

            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("INCLUDED", "Included", "Included", "Packages"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("INCLUDED", "Included", "Included", "Containers"));
            quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("INCLUDED", "Included", "Included", "TotalPerContainers"));


            quoteTemplateTextCodeRepository.SubmitChanges();
        }   

        private QuoteTemplateTextCode GetNewQuoteTemplateTextCode(string textCode, string englishName, string localName, string area , string quoteTemplateId=null)
        {
            QuoteTemplateTextCode quoteTemplateTextCodePM = new QuoteTemplateTextCode()
            {
                Id = IdCounter.GetNumber("QuoteTemplateTextCode", Tenant).ToString(),
                Tenant = Tenant,
                TextCode = textCode,
                EnglishName = englishName,
                OriginalEnglishName = englishName,
                LocalName = localName,
                OriginalLocalName = localName,
                QuoteTemplateId = !string.IsNullOrEmpty(quoteTemplateId)? quoteTemplateId: QuoteTemplateId,
                Area = area,

            };
           
            return quoteTemplateTextCodePM;
        }
        #endregion

        #region Create TemplateSection
        private void CreateQuoteTemplateSection(string quoteTemplateId , int tenant)
        {
            Tenant = tenant;
            QuoteTemplateId = quoteTemplateId;
         
            QuoteTemplateSectionRepository quoteTemplateSectionRepository = new QuoteTemplateSectionRepository(Tenant);

            quoteTemplateSectionRepository.Add(GetNewQuoteTemplateSection("Page Header", "PH", "the header of each page in the quote", "Page Header", 0));
            quoteTemplateSectionRepository.Add(GetNewQuoteTemplateSection("Quote Header", "QH", "the header of the quote, displayed in the first page only", "", 1));
            quoteTemplateSectionRepository.Add(GetNewQuoteTemplateSection("Quote Introduction", "S", "the header of the quote, displayed in the first page only", "Quote Introduction", 2));
            quoteTemplateSectionRepository.Add(GetNewQuoteTemplateSection("Quote Details", "QD", "general details of the quote", "", 3));
            quoteTemplateSectionRepository.Add(GetNewQuoteTemplateSection("Pricing Packages", "PP", "the sales prices for the quote", null, 4));
            quoteTemplateSectionRepository.Add(GetNewQuoteTemplateSection("Pricing Containers", "PC", "the sales prices for the quote", null, 5));
            quoteTemplateSectionRepository.Add(GetNewQuoteTemplateSection("Page Footers", "PF", "the footer of each page in the quote", "", 6));

            quoteTemplateSectionRepository.SubmitChanges();

        }

        private QuoteTemplateSection GetNewQuoteTemplateSection(string name , string quoteTemplateSectionTypeCode , string description , string htmlData , int order)
        {
            byte[] templatedata = new byte[] { };

            if (!string.IsNullOrEmpty(htmlData))
            {
                string HtmlDataPH = htmlData;
                templatedata = UTF8Encoding.UTF8.GetBytes(HtmlDataPH);
            }
            
            QuoteTemplateSection quoteTemplateSection= new QuoteTemplateSection()
            {
                Id = IdCounter.GetNumber("QuoteTemplateSection", Tenant).ToString(),
                Tenant = Tenant,
                Name = name,
                Order = order,
                QuoteTemplateId = QuoteTemplateId,
                QuoteTemplateSectionTypeCode = quoteTemplateSectionTypeCode,
                Description = description,

            };

            if (quoteTemplateSection.QuoteTemplateSectionTypeCode != "PH" && quoteTemplateSection.QuoteTemplateSectionTypeCode != "PF" && quoteTemplateSection.QuoteTemplateSectionTypeCode != "PC" && quoteTemplateSection.QuoteTemplateSectionTypeCode != "PP")
            {
                if (templatedata != null)
                {
                    quoteTemplateSection.SectionDocId = quoteTemplateEntityService.UploadQuoteTemplateSectionDataFile(templatedata, null, quoteTemplateSection.Tenant);
                }
            }


 
            return quoteTemplateSection;
        }
        #endregion


        #region CopyQuoteTemplate

        public QuoteTemplatePM CopyQuoteTemplate(string quoteTemplateId, string copyName, string userId, int tenant ,int? orginalTenant = null)
        {
            this.Tenant = tenant;
            SecurityUtility.CheckContactFeature("QuoteTemplate", "NEW", tenant);
            quoteTemplateEntityService = new QuoteTemplateEntityService();
            QuoteTemplatePM newQuoteTemplateCopy = null;

            QuoteTemplateQuery quoteTemplateQuery = new QuoteTemplateQuery(tenant);
            

            QuoteTemplatePM orignalQuoteTemplatePM = quoteTemplateQuery.GetSinglePM(quoteTemplateId);

            if (orignalQuoteTemplatePM != null)
            {
                QuoteTemplateSetting copySetting = CopyQuoteTemplaetSetting(orignalQuoteTemplatePM.QuoteTemplateSettingId, tenant , orignalQuoteTemplatePM.Tenant);
                if (copySetting != null)
                {
                    newQuoteTemplateCopy = new QuoteTemplatePM()
                    {
                        Tenant = tenant,
                        Name = copyName,
                        UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                        CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                        CreatedByUserId = userId,
                        QuoteTemplateSettingId = copySetting.Id,
                        TemplateTypeCode = orignalQuoteTemplatePM.TemplateTypeCode,
                        IsTemplate = true,
                        OriginalQuoteTemplateId = orignalQuoteTemplatePM.Id,

                    };
                    CreateCopyFromQuoteTemplatePM(newQuoteTemplateCopy, orignalQuoteTemplatePM);

                }

            }


            return newQuoteTemplateCopy;
        }

        private QuoteTemplateSetting CopyQuoteTemplaetSetting(string settingId, int tenant , int? orginalTenant = null)
        {
            if (orginalTenant == null) orginalTenant = tenant;


            QuoteTemplateSetting copySetting = null;
            QuoteTemplateSettingRepository quoteTemplateSettingRepository = new QuoteTemplateSettingRepository((int)orginalTenant);
            QuoteTemplateSetting setting = quoteTemplateSettingRepository.GetSingleQuoteTemplateSetting(settingId, (int)orginalTenant);

            if (setting != null)
            {
                #region Setting Map

                copySetting = new QuoteTemplateSetting()
                {
                    Id = IdCounter.GetNumber("QuoteTemplateSetting", tenant).ToString(),
                    Tenant = tenant,
                    ShowChargeCodeContainers = setting.ShowChargeCodeContainers,
                    ShowChargeCodePackages = setting.ShowChargeCodePackages,
                    ShowChargeNameContainers = setting.ShowChargeNameContainers,
                    ShowChargeNamePackages = setting.ShowChargeNamePackages,
                    ShowContainerNameInsteadOfCodeContainers = setting.ShowContainerNameInsteadOfCodeContainers,
                    ShowFixedPriceContainers = setting.ShowFixedPriceContainers,
                    ShowLocalCurrencyColumnContainers = setting.ShowLocalCurrencyColumnContainers,
                    ShowLocalCurrencyColumnPackages = setting.ShowLocalCurrencyColumnPackages,
                    ShowLocalLanguage = setting.ShowLocalLanguage,
                    RightToLeft = setting.RightToLeft,
                    ShowMeasurementContainers = setting.ShowMeasurementContainers,
                    ShowMeasurementPackages = setting.ShowMeasurementPackages,
                    ShowPricesTableContainers = setting.ShowPricesTableContainers,
                    ShowPricesTablePackages = setting.ShowPricesTablePackages,
                    ShowSaleCurrencyColumnContainers = setting.ShowSaleCurrencyColumnContainers,
                    ShowSaleCurrencyColumnPackages = setting.ShowSaleCurrencyColumnPackages,
                    ShowTotalInLocalCurrencyContainers = setting.ShowTotalInLocalCurrencyContainers,
                    ShowTotalInLocalCurrencyPackages = setting.ShowTotalInLocalCurrencyPackages,
                    ShowTotalInSaleCurrencyContainers = setting.ShowTotalInSaleCurrencyContainers,
                    ShowTotalInSaleCurrencyPackages = setting.ShowTotalInSaleCurrencyPackages,
                    ShowUnitPricePackages = setting.ShowUnitPricePackages,
                    ShowUnitsPackages = setting.ShowUnitsPackages,
                    AlignRight = setting.AlignRight,
                    SplitChargesbyGroupsContainers = setting.SplitChargesbyGroupsContainers,
                    SplitChargesbyGroupsPackages = setting.SplitChargesbyGroupsPackages,
                    PackagesTableDesignId = CopyQuoteTemplateTableDesignPM(setting.PackagesTableDesignId , (int)orginalTenant),
                    ContainserTableDesignId = CopyQuoteTemplateTableDesignPM(setting.ContainserTableDesignId, (int)orginalTenant),
                    TotalsPackagesLabelDesignId = CopyQuoteTemplateTextDesignId(setting.TotalsPackagesLabelDesignId, (int)orginalTenant),
                    TotalsPackagesValueDesignId = CopyQuoteTemplateTextDesignId(setting.TotalsPackagesValueDesignId, (int)orginalTenant),
                    TotalsContainsersLabelDesignId = CopyQuoteTemplateTextDesignId(setting.TotalsContainsersLabelDesignId, (int)orginalTenant),
                    TotalsContainsersValueDesignId = CopyQuoteTemplateTextDesignId(setting.TotalsContainsersValueDesignId, (int)orginalTenant),
                    GroupByPackagesLabelDesignId = CopyQuoteTemplateTextDesignId(setting.GroupByPackagesLabelDesignId, (int)orginalTenant),
                    GroupByContainsersLabelDesignId = CopyQuoteTemplateTextDesignId(setting.GroupByContainsersLabelDesignId, (int)orginalTenant),
                    GroupByPackagesValueDesignId = CopyQuoteTemplateTextDesignId(setting.GroupByPackagesValueDesignId, (int)orginalTenant),
                    GroupByContainsersValueDesignId = CopyQuoteTemplateTextDesignId(setting.GroupByContainsersValueDesignId, (int)orginalTenant),
                    DetailsTableDesignId = CopyQuoteTemplateTableDesignPM(setting.DetailsTableDesignId, (int)orginalTenant),
                    PageHeaderArea1FreeTextDesignId = CopyQuoteTemplateTextDesignId(setting.PageHeaderArea1FreeTextDesignId, (int)orginalTenant),
                    PageHeaderArea2FreeTextDesignId = CopyQuoteTemplateTextDesignId(setting.PageHeaderArea2FreeTextDesignId, (int)orginalTenant),
                    PageHeaderArea3FreeTextDesignId = CopyQuoteTemplateTextDesignId(setting.PageHeaderArea3FreeTextDesignId, (int)orginalTenant),
                    PageFooterArea1FreeTextDesignId = CopyQuoteTemplateTextDesignId(setting.PageFooterArea1FreeTextDesignId, (int)orginalTenant),
                    PageFooterArea2FreeTextDesignId = CopyQuoteTemplateTextDesignId(setting.PageFooterArea2FreeTextDesignId, (int)orginalTenant),
                    PageFooterArea3FreeTextDesignId = CopyQuoteTemplateTextDesignId(setting.PageFooterArea3FreeTextDesignId, (int)orginalTenant),
                    HeaderTableDesignId = CopyQuoteTemplateTableDesignPM(setting.HeaderTableDesignId, (int)orginalTenant),
                    DetailsSectionHasTwoColumns = setting.DetailsSectionHasTwoColumns,
                    HeaderSectionHasTwoColumns = setting.HeaderSectionHasTwoColumns,
                    DetailsTitleDesignId = CopyQuoteTemplateTextDesignId(setting.DetailsTitleDesignId, (int)orginalTenant),
                    PricingContainsersTitleDesignId = CopyQuoteTemplateTextDesignId(setting.PricingContainsersTitleDesignId, (int)orginalTenant),
                    PricingPackagesTitleDesignId = CopyQuoteTemplateTextDesignId(setting.PricingPackagesTitleDesignId, (int)orginalTenant),
                    ShowHeaderQuoteDate = setting.ShowHeaderQuoteDate,
                    ShowHeaderExpirationDate = setting.ShowHeaderExpirationDate,
                    ShowHeaderQuoteNumber = setting.ShowHeaderQuoteNumber,
                    ShowTitlePricingPackages = setting.ShowTitlePricingPackages,
                    ShowTitlePricingContainsers = setting.ShowTitlePricingContainsers,
                    ShowHeaderCustomer = setting.ShowHeaderCustomer,
                    ShowDetailsExpirationDate = setting.ShowDetailsExpirationDate,
                    ShowDetailsExpirationDays = setting.ShowDetailsExpirationDays,
                    ShowDetailsShipperName = setting.ShowDetailsShipperName,
                    ShowDetailsShipperAddress = setting.ShowDetailsShipperAddress,
                    ShowDetailsShipperContact = setting.ShowDetailsShipperContact,
                    ShowDetailsShipperReferences = setting.ShowDetailsShipperReferences,
                    ShowDetailsConsigneeName = setting.ShowDetailsConsigneeName,
                    ShowDetailsConsigneeAddress = setting.ShowDetailsConsigneeAddress,
                    ShowDetailsConsigneeContact = setting.ShowDetailsConsigneeContact,
                    ShowDetailsConsigneeReferences = setting.ShowDetailsConsigneeReferences,
                    ShowDetailsPickupFrom = setting.ShowDetailsPickupFrom,
                    ShowDetailsDeliveryTo = setting.ShowDetailsDeliveryTo,
                    ShowDetailsFromPort = setting.ShowDetailsFromPort,
                    ShowDetailsToPort = setting.ShowDetailsToPort,
                    ShowDetailsIncoterms = setting.ShowDetailsIncoterms,
                    ShowDetailsService = setting.ShowDetailsService,
                    ShowDetailsSalesMan = setting.ShowDetailsSalesMan,
                    ShowDetailsDescriptionOfGoods = setting.ShowDetailsDescriptionOfGoods,
                    ShowDetailsDangerousGoods = setting.ShowDetailsDangerousGoods,
                    ShowDetailsCarrier = setting.ShowDetailsCarrier,
                    ShowCodeChargeSaleMinMaxPackages = setting.ShowCodeChargeSaleMinMaxPackages,
                    ShowCodeChargeSaleMinMaxContainers = setting.ShowCodeChargeSaleMinMaxContainers,
                    ShowPriceByContainerColumn = setting.ShowPriceByContainerColumn,
                    PageHeaderImage1Width = setting.PageHeaderImage1Width,
                    PageHeaderImage2Width = setting.PageHeaderImage2Width,
                    PageHeaderImage3Width = setting.PageHeaderImage3Width,
                    PageHeaderArea1Height = setting.PageHeaderArea1Height,
                    PageHeaderArea2Height = setting.PageHeaderArea2Height,
                    PageHeaderArea3Height = setting.PageHeaderArea3Height,
                    PageHeaderArea1Width = setting.PageHeaderArea1Width,
                    PageHeaderArea2Width = setting.PageHeaderArea2Width,
                    PageHeaderArea3Width = setting.PageHeaderArea3Width,
                    PageHeaderArea1ImageAlignment = setting.PageHeaderArea1ImageAlignment,
                    PageHeaderArea2ImageAlignment = setting.PageHeaderArea2ImageAlignment,
                    PageHeaderArea3ImageAlignment = setting.PageHeaderArea3ImageAlignment,
                    PageHeaderBorderColor = setting.PageHeaderBorderColor,
                    PageHeaderBorderThickness = setting.PageHeaderBorderThickness,
                    PageHeaderBorderTypeCode = setting.PageHeaderBorderTypeCode,
                    PageHeaderAreaHeight = setting.PageHeaderAreaHeight,
                    PageHeaderArea1Type = setting.PageHeaderArea1Type,
                    PageHeaderArea2Type = setting.PageHeaderArea2Type,
                    PageHeaderArea3Type = setting.PageHeaderArea3Type,
                    PageFooterArea1Height = setting.PageFooterArea1Height,
                    PageFooterArea2Height = setting.PageFooterArea2Height,
                    PageFooterArea3Height = setting.PageFooterArea3Height,
                    PageFooterImage1Width = setting.PageFooterImage1Width,
                    PageFooterImage2Width = setting.PageFooterImage2Width,
                    PageFooterImage3Width = setting.PageFooterImage3Width,
                    PageFooterArea1Width = setting.PageFooterArea1Width,
                    PageFooterArea2Width = setting.PageFooterArea2Width,
                    PageFooterArea3Width = setting.PageFooterArea3Width,
                    PageFooterBorderColor = setting.PageFooterBorderColor,
                    PageFooterBorderTypeCode = setting.PageFooterBorderTypeCode,
                    PageFooterBorderThickness = setting.PageFooterBorderThickness,
                    PageFooterArea1ImageAlignment = setting.PageFooterArea1ImageAlignment,
                    PageFooterArea2ImageAlignment = setting.PageFooterArea2ImageAlignment,
                    PageFooterArea3ImageAlignment = setting.PageFooterArea3ImageAlignment,
                    PageFooterArea1Type = setting.PageFooterArea1Type,
                    PageFooterArea2Type = setting.PageFooterArea2Type,
                    PageFooterArea3Type = setting.PageFooterArea3Type,
                    PageFooterAreaHeight = setting.PageFooterAreaHeight,
                    HeaderTableColumWidthType = setting.HeaderTableColumWidthType,
                    DetailsTableColumWidthType = setting.DetailsTableColumWidthType,
                    HeaderTableColumn1LabelWidth = setting.HeaderTableColumn1LabelWidth,
                    HeaderTableColumn1ValueWidth = setting.HeaderTableColumn1ValueWidth,
                    HeaderTableColumn2LabelWidth = setting.HeaderTableColumn2LabelWidth,
                    HeaderTableColumn2ValueWidth = setting.HeaderTableColumn2ValueWidth,
                    DetailsTableColumn1LabelWidth = setting.DetailsTableColumn1LabelWidth,
                    DetailsTableColumn1ValueWidth = setting.DetailsTableColumn1ValueWidth,
                    DetailsTableColumn2LabelWidth = setting.DetailsTableColumn2LabelWidth,
                    DetailsTableColumn2ValueWidth = setting.DetailsTableColumn2ValueWidth,
                    PageFooterArea1ImageDetailId =!string.IsNullOrEmpty(setting.PageFooterArea1ImageDetailId) ? CopyImageDetailId(setting.PageFooterArea1ImageDetailId, orginalTenant!=null ? (int)orginalTenant :Tenant):null,
                    PageFooterArea2ImageDetailId = !string.IsNullOrEmpty(setting.PageFooterArea2ImageDetailId) ? CopyImageDetailId(setting.PageFooterArea2ImageDetailId, orginalTenant != null ? (int)orginalTenant : Tenant) : null,
                    PageFooterArea3ImageDetailId = !string.IsNullOrEmpty(setting.PageFooterArea3ImageDetailId) ? CopyImageDetailId(setting.PageFooterArea3ImageDetailId, orginalTenant != null ? (int)orginalTenant : Tenant) : null,
                    PageFooterArea1FreeText = setting.PageFooterArea1FreeText,
                    PageFooterArea2FreeText = setting.PageFooterArea2FreeText,
                    PageFooterArea3FreeText = setting.PageFooterArea3FreeText,
                    PageHeaderArea1FreeText = setting.PageHeaderArea1FreeText,
                    PageHeaderArea2FreeText = setting.PageHeaderArea2FreeText,
                    PageHeaderArea3FreeText = setting.PageHeaderArea3FreeText,
                    PageHeaderArea1ImageDetailId = !string.IsNullOrEmpty(setting.PageHeaderArea1ImageDetailId) ? CopyImageDetailId(setting.PageHeaderArea1ImageDetailId, orginalTenant != null ? (int)orginalTenant : Tenant) : null,
                    PageHeaderArea2ImageDetailId = !string.IsNullOrEmpty(setting.PageHeaderArea2ImageDetailId) ? CopyImageDetailId(setting.PageHeaderArea2ImageDetailId, orginalTenant != null ? (int)orginalTenant : Tenant) : null,
                    PageHeaderArea3ImageDetailId = !string.IsNullOrEmpty(setting.PageHeaderArea3ImageDetailId) ? CopyImageDetailId(setting.PageHeaderArea3ImageDetailId, orginalTenant != null ? (int)orginalTenant : Tenant) : null,
                    ShowTitleQuoteDetails = setting.ShowTitleQuoteDetails,
                    ShowDetailsCustomerContact = setting.ShowDetailsCustomerContact,
                    ShowDetailsCustomerAddress = setting.ShowDetailsCustomerAddress,
                    ShowDetailsCustomerName = setting.ShowDetailsCustomerName,
                    ShowDetailsCustomerReferences = setting.ShowDetailsCustomerReferences,
                    QuoteTemplatePDFMarginLeft = setting.QuoteTemplatePDFMarginLeft,
                    QuoteTemplatePDFMarginRight = setting.QuoteTemplatePDFMarginRight,
                    TotalPerContainersAdditionalTextDesignId =!string.IsNullOrEmpty(setting.TotalPerContainersAdditionalTextDesignId)? CopyQuoteTemplateTextDesignId(setting.TotalPerContainersAdditionalTextDesignId, (int)orginalTenant):"",
                    TotalPerContainersTableDesignId   = !string.IsNullOrEmpty(setting.TotalPerContainersTableDesignId) ?  CopyQuoteTemplateTableDesignPM(setting.TotalPerContainersTableDesignId, (int)orginalTenant):"",
                    ShowTitleTotalPerContainersTable = setting.ShowTitleTotalPerContainersTable,
                    TotalPerContainersCurrencyType = !string.IsNullOrEmpty(setting.TotalPerContainersCurrencyType)? setting.TotalPerContainersCurrencyType : "SALE",
                    ShowPageBreakBeforeTotalPerContainersTable = setting.ShowPageBreakBeforeTotalPerContainersTable,
                    ShowSaleMaxMinAmountContainers = setting.ShowSaleMaxMinAmountContainers,
                    ShowSaleMaxMinAmountPackages =  setting.ShowSaleMaxMinAmountPackages,
                    ShowChargeNoteContainers = setting.ShowChargeNoteContainers,
                    ShowChargeDescriptionContainers = setting.ShowChargeDescriptionContainers,
                    ShowChargeDescriptionPackages = setting.ShowChargeDescriptionPackages,
                    ShowChargeNotePackages = setting.ShowChargeNotePackages,
                    ShowTotalPerChargeGroupContainers = setting.ShowTotalPerChargeGroupContainers,
                    ShowTotalPerChargeGroupPackages = setting.ShowTotalPerChargeGroupPackages,
                    ShowHeaderLabelsContainers = setting.ShowHeaderLabelsContainers,
                    ShowHeaderLabelsPackages = setting.ShowHeaderLabelsPackages,
                    SpaceLinesBeforeContainers = setting.SpaceLinesBeforeContainers,
                    SpaceLinesBeforeFooters = setting.SpaceLinesBeforeFooters,
                    SpaceLinesBeforeHeaders = setting.SpaceLinesBeforeHeaders,
                    SpaceLinesBeforePackages = setting.SpaceLinesBeforePackages,
                    SpaceLinesBeforeQuoteDetails = setting.SpaceLinesBeforeQuoteDetails,
                    SpaceLinesBeforeQuoteHeaders = setting.SpaceLinesBeforeQuoteHeaders,
                    SpaceLinesBeforePerContainers = setting.SpaceLinesBeforePerContainers,
                    QuoteTemplatePDFMarginTop = setting.QuoteTemplatePDFMarginTop,
                    QuoteTemplatePDFMarginBottom = setting.QuoteTemplatePDFMarginBottom,
                    ShowVATPercentageContainers = setting.ShowVATPercentageContainers,
                    ShowVATPercentagePackages = setting.ShowVATPercentagePackages,
                    ShowVATTypeContainers = setting.ShowVATTypeContainers,
                    ShowVATTypePackages = setting.ShowVATTypePackages,
                    PageNumberingTextDesignId = !string.IsNullOrEmpty(setting.PageNumberingTextDesignId) ? CopyQuoteTemplateTableDesignPM(setting.PageNumberingTextDesignId, (int)orginalTenant) : "",
                    HidePageNumber = setting.HidePageNumber,
                };

                if (string.IsNullOrEmpty(copySetting.TotalPerContainersTableDesignId))
                {
                    GetNewFromTotalByContainer(copySetting);
                }

                #endregion

                quoteTemplateSettingRepository.Add(copySetting);
                quoteTemplateSettingRepository.SubmitChanges();

            }
            return copySetting;
        }

        private string CopyImageDetailId(string imageDetailId , int orginalTenant)
        {
            if (quoteTemplateReportHelper == null) quoteTemplateReportHelper = new QuoteTemplateReportHelper();
            string result = null;
            ImageDetailRepository imageDetailRepository = new ImageDetailRepository(orginalTenant);
            ImageDetail orginalImageDetail = imageDetailRepository.GetSingleImageDetail(imageDetailId, orginalTenant);
            if (orginalImageDetail != null)
            {
                byte[] fileData = quoteTemplateReportHelper.GetFile(orginalImageDetail.Id, orginalImageDetail.Extension, "images", orginalImageDetail.Tenant);

                if (fileData != null)
                {
                    ImageDetail imageDetail = new ImageDetail()
                    {
                        Id = IdCounter.GetNumber("ImageDetail", Tenant),
                        Tenant = Tenant,
                        Extension = orginalImageDetail.Extension,
                        Size = orginalImageDetail.Size,

                    };

                    imageDetailRepository.Add(imageDetail);
                    imageDetailRepository.SubmitChanges();


                    Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
                    {
                        FileName = imageDetail.Id,
                        FolderName = "images",
                        Extension = imageDetail.Extension,
                        Tenant = imageDetail.Tenant,
                        FileSize = fileData.Length,

                    };
                    Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
                    storageservice.Write(fileData, fileInfo);

                    result = imageDetail.Id;

                }

            }
            return result;

        }


        private void CreateCopyFromQuoteTemplatePM(QuoteTemplatePM newQuoteTemplatePM, QuoteTemplatePM orignalQuoteTemplatePM)
        {
            if(quoteTemplateReportHelper == null) quoteTemplateReportHelper = new QuoteTemplateReportHelper();
            if (quoteTemplateEntityService == null)
            {
                quoteTemplateEntityService = new QuoteTemplateEntityService();
            }

            if (!string.IsNullOrEmpty(orignalQuoteTemplatePM.HeaderDocId))
            {
                byte[] headerTemplatedata = quoteTemplateReportHelper.DownloadQuoteTemplateSectionDataFile(orignalQuoteTemplatePM.HeaderDocId, orignalQuoteTemplatePM.Tenant);
                if (headerTemplatedata != null){
                    newQuoteTemplatePM.HeaderDocId = quoteTemplateEntityService.UploadQuoteTemplateSectionDataFile(headerTemplatedata, null, newQuoteTemplatePM.Tenant);
                }
               
            }

            if (!string.IsNullOrEmpty(orignalQuoteTemplatePM.FooterDocId))
            {
                byte[] footerTemplatedata = quoteTemplateReportHelper.DownloadQuoteTemplateSectionDataFile(orignalQuoteTemplatePM.FooterDocId, orignalQuoteTemplatePM.Tenant);
                if (footerTemplatedata != null)
                {
                    newQuoteTemplatePM.FooterDocId = quoteTemplateEntityService.UploadQuoteTemplateSectionDataFile(footerTemplatedata, null, newQuoteTemplatePM.Tenant);
                }
                   
            }

            IQuotesContext objectContext = QuotesContext.GetContext(Tenant);
            QuoteTemplateService quoteTemplateService = new QuoteTemplateService(objectContext, Tenant);
            quoteTemplateService.Create(newQuoteTemplatePM);
            CopyQuoteTemplateTextCodes(newQuoteTemplatePM.Id, orignalQuoteTemplatePM.Id , orignalQuoteTemplatePM.Tenant);
            CopyQuoteTemplateHeaderFields(newQuoteTemplatePM.Id, orignalQuoteTemplatePM.Id, orignalQuoteTemplatePM.Tenant);
            CopyQuoteTemplateDetailsFields(newQuoteTemplatePM.Id, orignalQuoteTemplatePM.Id, orignalQuoteTemplatePM.Tenant);
            CopyQuoteTemplateSection(newQuoteTemplatePM.Id, orignalQuoteTemplatePM.Id, orignalQuoteTemplatePM.Tenant);

        }

        private QuoteTemplateSection copyquoteTemplateSection;
        private void CopyQuoteTemplateSection(string copyQuoteTemplateId ,string orginalQuoteTempalteId, int? orignalTenant = null)
        {
            if (orignalTenant == null) orignalTenant = Tenant;
            if (quoteTemplateEntityService == null)
            {
                quoteTemplateEntityService = new QuoteTemplateEntityService();
            }

            QuoteTemplateSectionRepository quoteTemplateSectionRepository = new QuoteTemplateSectionRepository(Tenant);
            List<QuoteTemplateSection> quoteTemplateSectionLists = quoteTemplateSectionRepository.GetQuoteTemplateSectionByQuoteTemplateId(orginalQuoteTempalteId, (int)orignalTenant).ToList();

            if (quoteTemplateSectionLists.Count() > 0)
            {
                foreach (QuoteTemplateSection section in quoteTemplateSectionLists)
                {
                    if (section != null)
                    {
                        copyquoteTemplateSection = new QuoteTemplateSection()
                        {
                            Id = IdCounter.GetNumber("QuoteTemplateSection", Tenant).ToString(),
                            Tenant = Tenant,
                            Name = section.Name,
                            Order = section.Order,
                            QuoteTemplateSectionTypeCode = section.QuoteTemplateSectionTypeCode,
                            QuoteTemplateId = copyQuoteTemplateId,
                            Description = section.Description,
                            IsCancel = section.IsCancel,
                        };

                        if (!string.IsNullOrEmpty(section.SectionDocId))
                        {
                            byte[] templatedata = quoteTemplateReportHelper.DownloadQuoteTemplateSectionDataFile(section.SectionDocId, section.Tenant);
                            if (templatedata != null) {
                                copyquoteTemplateSection.SectionDocId = quoteTemplateEntityService.UploadQuoteTemplateSectionDataFile(templatedata, null, copyquoteTemplateSection.Tenant);
                            }
                           
                        }

                    }

                    quoteTemplateSectionRepository.Add(copyquoteTemplateSection);

                }

                quoteTemplateSectionRepository.SubmitChanges();
            }



        }

        // QuoteTemplateTableDesign
        private string CopyQuoteTemplateTableDesignPM(string TableDesignId  , int? orginalTenant = null)
        {
            if (orginalTenant == null) orginalTenant = Tenant;

            QuoteTemplateTableDesignRepository quoteTemplateTableDesignRepository = new QuoteTemplateTableDesignRepository(Tenant);
            QuoteTemplateTableDesign quoteTemplateTableDesign = quoteTemplateTableDesignRepository.GetSingleQuoteTemplateTableDesign(TableDesignId, (int)orginalTenant, true);

            QuoteTemplateTableDesign CopyquoteTemplateTableDesign = new QuoteTemplateTableDesign()
            {
                Id = IdCounter.GetNumber("QuoteTemplateTableDesign", Tenant).ToString(),
                BorderColor = quoteTemplateTableDesign.BorderColor,
                BorderThickness = quoteTemplateTableDesign.BorderThickness,
                BorderTypeCode = quoteTemplateTableDesign.BorderTypeCode,
                Tenant = Tenant,
            };

            CopyquoteTemplateTableDesign.HeaderDesignId = CopyQuoteTemplateTextDesignId(quoteTemplateTableDesign.HeaderDesignId , orginalTenant);
            CopyquoteTemplateTableDesign.LinesDesignId = CopyQuoteTemplateTextDesignId(quoteTemplateTableDesign.LinesDesignId, orginalTenant);
            CopyquoteTemplateTableDesign.GroupByDesignId = CopyQuoteTemplateTextDesignId(quoteTemplateTableDesign.GroupByDesignId, orginalTenant);

            quoteTemplateTableDesignRepository.Add(CopyquoteTemplateTableDesign);
            quoteTemplateTableDesignRepository.SubmitChanges();

            return CopyquoteTemplateTableDesign.Id;


        }
        // QuoteTemplateTextDesign
        private string CopyQuoteTemplateTextDesignId(string DesignId , int? orginalTenant = null)
        {


            if (orginalTenant == null) orginalTenant = Tenant;
             QuoteTemplateTextDesignRepository quoteTemplateTextDesignRepository = new QuoteTemplateTextDesignRepository((int)orginalTenant);
            QuoteTemplateTextDesign quoteTemplateTextDesign = quoteTemplateTextDesignRepository.GetSingleQuoteTemplateTextDesign(DesignId, (int)orginalTenant, true);


            QuoteTemplateTextDesign CopyQuoteTemplateTextDesign = new QuoteTemplateTextDesign()
            {
                Id = IdCounter.GetNumber("QuoteTemplateTextDesign", Tenant).ToString(),
                Tenant = Tenant,
                FontSize = quoteTemplateTextDesign.FontSize,
                FontFamily = quoteTemplateTextDesign.FontFamily,
                FontWeight = quoteTemplateTextDesign.FontWeight,
                Alignment = quoteTemplateTextDesign.Alignment,
                TextColor = quoteTemplateTextDesign.TextColor,
                BackgroundColor = quoteTemplateTextDesign.BackgroundColor,
                UnDerLine = quoteTemplateTextDesign.UnDerLine,
                Italic = quoteTemplateTextDesign.Italic,

            };

            quoteTemplateTextDesignRepository.Add(CopyQuoteTemplateTextDesign);
            quoteTemplateTextDesignRepository.SubmitChanges();

            return CopyQuoteTemplateTextDesign.Id;
        }

        //  TextCodes
        private void CopyQuoteTemplateTextCodes(string quoteTemplateId,string orginalQuoteTempalteId , int? orignalTenant = null)
        {
            if (orignalTenant == null) orignalTenant = Tenant;
            QuoteTemplateTextCodeRepository quoteTemplateTextCodeRepository = new QuoteTemplateTextCodeRepository((int)orignalTenant);
            List<QuoteTemplateTextCode> quoteTemplateTextCodeLists = quoteTemplateTextCodeRepository.GetQuoteTemplateTextCodeByQuoteTemplateId(orginalQuoteTempalteId, (int)orignalTenant).ToList();

            if (quoteTemplateTextCodeLists.Count() > 0)
            {
                foreach (QuoteTemplateTextCode quoteTemplateTextCode in quoteTemplateTextCodeLists)
                {
                    QuoteTemplateTextCode quoteTemplateTextCodeCopy = new QuoteTemplateTextCode()
                    {
                        Id = IdCounter.GetNumber("QuoteTemplateTextCode", Tenant).ToString(),
                        Area = quoteTemplateTextCode.Area,
                        EnglishName = quoteTemplateTextCode.EnglishName,
                        OriginalEnglishName = quoteTemplateTextCode.OriginalEnglishName,
                        LocalName = quoteTemplateTextCode.LocalName,
                        OriginalLocalName = quoteTemplateTextCode.OriginalLocalName,
                        QuoteTemplateId = quoteTemplateId,
                        TextCode = quoteTemplateTextCode.TextCode,
                        Tenant =Tenant,

                    };
                    quoteTemplateTextCodeRepository.Add(quoteTemplateTextCodeCopy);

                }


                QuoteTemplateTextCode templateTextCode = quoteTemplateTextCodeLists.Where(d => d.Area == "TotalPerContainers").FirstOrDefault();
                if (templateTextCode == null)
                {
                    quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("CHARGEGROUP", "Charge Group", "Charge Group", "TotalPerContainers", quoteTemplateId));
                    quoteTemplateTextCodeRepository.Add(GetNewQuoteTemplateTextCode("TOTALPERCONTAINERS", "Total Per Containers", "Total Per Containers", "TotalPerContainers", quoteTemplateId));
                }


                quoteTemplateTextCodeRepository.SubmitChanges();
            }
        }


        //  DetailsFields
        private void CopyQuoteTemplateDetailsFields(string quoteTemplateId,string orginalQuoteTempalteId, int? orignalTenant = null)
        {
            if (orignalTenant == null) orignalTenant = Tenant;
            QuoteTemplateDetailsFieldRepository quoteTemplateDetailsFieldsRepository = new QuoteTemplateDetailsFieldRepository((int)orignalTenant);
            List<QuoteTemplateDetailsField> QuoteTemplateDetailsFieldLists = quoteTemplateDetailsFieldsRepository.GetQuoteTemplateDetailsFieldsByQuoteTemplateId(orginalQuoteTempalteId, (int)orignalTenant);

            if (QuoteTemplateDetailsFieldLists.Count > 0)
            {
                foreach (QuoteTemplateDetailsField item in QuoteTemplateDetailsFieldLists)
                {
                    if (item != null)
                    {
                        QuoteTemplateDetailsField quoteTemplateDetailsFieldCopy = new QuoteTemplateDetailsField()
                        {

                            Id = IdCounter.GetNumber("QuoteTemplateDetailsField", Tenant).ToString(),
                            Tenant = Tenant,
                            Row = item.Row,
                            Column = item.Column,
                            FieldCode = item.FieldCode,
                            QuoteTemplateId = quoteTemplateId,

                        };

                        quoteTemplateDetailsFieldsRepository.Add(quoteTemplateDetailsFieldCopy);
                    }

                }
                quoteTemplateDetailsFieldsRepository.SubmitChanges();

            }
        }

        //  HeaderFields
        private void CopyQuoteTemplateHeaderFields(string quoteTemplateId, string orginalQuoteTempalteId , int? orignalTenant = null)
        {
            if (orignalTenant == null) orignalTenant = Tenant;

            QuoteTemplateHeaderFieldRepository quoteTemplateHeaderFieldsRepository = new QuoteTemplateHeaderFieldRepository((int)orignalTenant);
            List<QuoteTemplateHeaderField> QuoteTemplateHeaderFieldLists = quoteTemplateHeaderFieldsRepository.GetQuoteTemplateHeaderFieldsByQuoteTemplateId(orginalQuoteTempalteId, (int)orignalTenant);


            if (QuoteTemplateHeaderFieldLists.Count > 0)
            {
                foreach (QuoteTemplateHeaderField item in QuoteTemplateHeaderFieldLists)
                {
                    if (item != null)
                    {
                        QuoteTemplateHeaderField quoteTemplateHeaderFieldCopy = new QuoteTemplateHeaderField()
                        {
                            Id = IdCounter.GetNumber("QuoteTemplateHeaderField", Tenant).ToString(),
                            Tenant =Tenant,
                            Row = item.Row,
                            Column = item.Column,
                            FieldCode = item.FieldCode,
                            QuoteTemplateId = quoteTemplateId,
                        };
                        quoteTemplateHeaderFieldsRepository.Add(quoteTemplateHeaderFieldCopy);
                    }

                }
                quoteTemplateHeaderFieldsRepository.SubmitChanges();

            }
        }


        private void GetNewFromTotalByContainer(QuoteTemplateSetting setting)
        {

            QuoteTemplateTextDesignRepository quoteTemplateTextDesignRepository = new QuoteTemplateTextDesignRepository(setting.Tenant);
            QuoteTemplateTableDesignRepository quoteTemplateTableDesignRepository = new QuoteTemplateTableDesignRepository(setting.Tenant);
            QuoteTemplateTextDesign totalPerContainersAdditional = GetNewQuoteTemplateTextDesign();
            QuoteTemplateTextDesign headerDesignTotalPer = GetNewQuoteTemplateTextDesign();
            QuoteTemplateTextDesign  linesDesignTotalPer = GetNewQuoteTemplateTextDesign();
            QuoteTemplateTextDesign  groupByDesignHeaderTotalPer = GetNewQuoteTemplateTextDesign();

            quoteTemplateTextDesignRepository.Add(totalPerContainersAdditional);
            quoteTemplateTextDesignRepository.Add(headerDesignTotalPer);
            quoteTemplateTextDesignRepository.Add(linesDesignTotalPer);
            quoteTemplateTextDesignRepository.Add(groupByDesignHeaderTotalPer);
            quoteTemplateTextDesignRepository.SubmitChanges();

            QuoteTemplateTableDesign totalPerContainersTableDesign = GetNewQuoteTemplateTableDesign(headerDesignTotalPer.Id, linesDesignTotalPer.Id, groupByDesignHeaderTotalPer.Id);
            quoteTemplateTableDesignRepository.Add(totalPerContainersTableDesign);
            quoteTemplateTableDesignRepository.SubmitChanges();

  
            setting.TotalPerContainersAdditionalTextDesignId = totalPerContainersAdditional.Id;
            setting.TotalPerContainersTableDesignId = totalPerContainersTableDesign.Id;


        }

        #endregion


        #region CopyQuoteTemplateFromTenantZero

        public string CopyQuoteTemplateFromTenantZero(QuoteTemplateCopyDetails quoteTemplateCopyDetails)
        {
            QuoteTemplateQuery quoteTemplateRepository = new QuoteTemplateQuery(quoteTemplateCopyDetails.Tenant);
            this.Tenant = quoteTemplateCopyDetails.Tenant;
            List<QuoteTemplatePM> QuoteTemplateLists = new List<QuoteTemplatePM>();
            string result = "";
            if (string.IsNullOrEmpty(quoteTemplateCopyDetails.QuoteTemplateId))
            {
                QuoteTemplateLists = quoteTemplateRepository.GetQuoteTemplatePMsByTenant(0).Where(d=>d.IsCopiedAtSignup && d.IsEnabledForCustomers).ToList();
            }
            else
            {
                QuoteTemplateLists.Add(quoteTemplateRepository.GetSinglePM(quoteTemplateCopyDetails.QuoteTemplateId,0));
            }
            if (string.IsNullOrEmpty(quoteTemplateCopyDetails.UserId))
            {
                ContactRepository contactRepository = new ContactRepository(0);
                quoteTemplateCopyDetails.UserId = contactRepository.GetConactIdByemail("system@tenant" + quoteTemplateCopyDetails.Tenant.ToString() + ".com", quoteTemplateCopyDetails.Tenant);
            }
            if (quoteTemplateCopyDetails.UpdateFromTenantData)
            {
                RemoveExisitingQuoteTemplates(quoteTemplateCopyDetails.Tenant, quoteTemplateRepository, QuoteTemplateLists);
            }
            foreach (QuoteTemplatePM item in QuoteTemplateLists)
            {
                QuoteTemplateSetting copySetting = CopyQuoteTemplaetSetting(item.QuoteTemplateSettingId, quoteTemplateCopyDetails.Tenant, 0);
                if (copySetting != null)
                {
                    QuoteTemplatePM newQuoteTemplateCopy = new QuoteTemplatePM()
                    {
                        Tenant = quoteTemplateCopyDetails.Tenant,
                        Name = item.Name,
                        UpdateDate = TenantServerConfigration.GetCurrentDateTime(quoteTemplateCopyDetails.Tenant),
                        CreateDate = TenantServerConfigration.GetCurrentDateTime(quoteTemplateCopyDetails.Tenant),
                        CreatedByUserId = quoteTemplateCopyDetails.UserId,
                        QuoteTemplateSettingId = copySetting.Id,
                        TemplateTypeCode = item.TemplateTypeCode,
                        IsTemplate = true,
                        OriginalQuoteTemplateId = item.Id
                    };
                    CreateCopyFromQuoteTemplatePM(newQuoteTemplateCopy, item);
                    result = !string.IsNullOrEmpty(quoteTemplateCopyDetails.QuoteTemplateId) ? newQuoteTemplateCopy.Id : "";

                }


            }

            return result;
        }

        private static void RemoveExisitingQuoteTemplates(int tenant, QuoteTemplateQuery quoteTemplateRepository, List<QuoteTemplatePM> QuoteTemplateLists)
        {
            List<QuoteTemplatePM> CurrentTenantQuoteTemplateLists = new List<QuoteTemplatePM>();
            CurrentTenantQuoteTemplateLists = quoteTemplateRepository.GetQuoteTemplatePMsByTenant(tenant).ToList();

            foreach (QuoteTemplatePM item in CurrentTenantQuoteTemplateLists)
            {
                QuoteTemplatePM quoteTemplate = QuoteTemplateLists.SingleOrDefault(i => i.Id == item.OriginalQuoteTemplateId);
                if (quoteTemplate != null)
                {
                    QuoteTemplateLists.Remove(quoteTemplate);
                }
            }
        }
        #endregion

    }

    public class QuoteTemplateCopyDetails
    {
        public int Tenant;
        public string QuoteTemplateId;
        public string UserId;
        public bool UpdateFromTenantData;
    }
}