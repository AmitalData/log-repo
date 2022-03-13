using Logitude.BL.QuoteModel.DataContracts;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Logitude.BL.QuoteModel.Tools.EntityService
{
    public class QuoteTemplateSettingDataBuilder
    {
        private readonly object quoteTemplateSetting;
        private List<PricesFieldSettings> PricesPackagesTableSettings;
        private List<PricesFieldSettings> PricesContainersTableSettings;
        public QuoteTemplateSettingDataBuilder(object quoteTemplateSetting)
        {
            this.quoteTemplateSetting = quoteTemplateSetting;
        }

        public string SerializeNewQuoteTemplateSettingDataToXmlString()
        {
            QuoteTemplateSettingData quoteTemplateSettingData = GetNewQuoteTemplateSettingData();

            return LogitudeXmlSerializer.SerializeObjectToXmlString(quoteTemplateSettingData);
        }

        public QuoteTemplateSettingData GetNewQuoteTemplateSettingData()
        {
            BuildPricesPackagesTableSettings();
            BuildPricesContainersTableSettings();
            QuoteTemplateSettingData quoteTemplateSettingData = new QuoteTemplateSettingData
            {
                PricesPackagesTableSettings = PricesPackagesTableSettings,
                PricesContainersTableSettings = PricesContainersTableSettings
            };

            return quoteTemplateSettingData;
        }

        private void BuildPricesPackagesTableSettings()
        {
            PricesPackagesTableSettings = new List<PricesFieldSettings>();
            string sectionType = "PP";
            AddNewPricesFieldSettingToPricesTableSettings("ShowHeaderLabelsPackages", "HEADERPACKAGES", sectionType);
            AddNewPricesFieldSettingToPricesTableSettings("ShowChargeNamePackages", "CHARGEPACKAGES", sectionType);
            AddNewPricesFieldSettingToPricesTableSettings("ShowChargeCodePackages", "CHARGECODEPACKAGES", sectionType);
            AddNewPricesFieldSettingToPricesTableSettings("ShowMeasurementPackages", "MEASUREMENTPACKAGES", sectionType);
            AddNewPricesFieldSettingToPricesTableSettings("ShowUnitsPackages", "UNITSPACKAGES", sectionType);
            AddNewPricesFieldSettingToPricesTableSettings("ShowUnitPricePackages", "UNITPRICEPACKAGES", sectionType);
            AddNewPricesFieldSettingToPricesTableSettings("ShowSaleCurrencyColumnPackages", "LOCALAMOUNTPACKAGES", sectionType);
            AddNewPricesFieldSettingToPricesTableSettings("ShowLocalCurrencyColumnPackages", "CHARGEDESCRIPTIONPACKAGES", sectionType);
            AddNewPricesFieldSettingToPricesTableSettings("ShowChargeDescriptionPackages", "CHARGEDESCRIPTIONPACKAGES", sectionType);
            AddNewPricesFieldSettingToPricesTableSettings("ShowChargeNotePackages", "CHARGENOTEPACKAGES", sectionType);
            AddNewPricesFieldSettingToPricesTableSettings("ShowSaleMaxMinAmountPackages", "SALEMINMAXPACKAGES", sectionType);
            AddNewPricesFieldSettingToPricesTableSettings("ShowIncludedChargesPackages", "INCLUDEDCHARGESPACKAGES", sectionType);
            AddNewPricesFieldSettingToPricesTableSettings("ShowRegionalTAXPackages", "ISREGIONALTAXPACKAGES", sectionType);
            AddNewPricesFieldSettingToPricesTableSettings("ShowVATTypePackages", "VATTYPEPACKAGES", sectionType);
            AddNewPricesFieldSettingToPricesTableSettings("ShowVATPercentagePackages", "VATPERCENTAGEPACKAGES", sectionType);
        }
        
        private void AddNewPricesFieldSettingToPricesTableSettings(string fieldDBName, string fieldCode, string sectionType)
        {
            List<PricesFieldSettings> pricesTableSettings = GetPricesPackagesListSettingsBySectionType(sectionType);
            PropertyInfo propInfo = quoteTemplateSetting.GetType().GetProperty(fieldDBName);

            pricesTableSettings.Add(new PricesFieldSettings()
            {
                Name = fieldDBName,
                Code = fieldCode,
                Index = pricesTableSettings.Count(),
                InUse = (bool)propInfo.GetValue(quoteTemplateSetting),
            });

            //PricesPackagesTableSettings = sectionType == "PP" ? pricesTableSettings : PricesPackagesTableSettings;
            //PricesContainersTableSettings = sectionType == "PC" ? pricesTableSettings : PricesContainersTableSettings;
        }

        private List<PricesFieldSettings> GetPricesPackagesListSettingsBySectionType(string sectionType)
        {
            return sectionType == "PP" ? PricesPackagesTableSettings : PricesContainersTableSettings;
        }

        private void BuildPricesContainersTableSettings()
        {
            PricesContainersTableSettings = new List<PricesFieldSettings>();
            string sectionType = "PC";

            AddNewPricesFieldSettingToPricesTableSettings("ShowHeaderLabelsContainers", "HEADERCONTAINERS", sectionType);
            AddNewPricesFieldSettingToPricesTableSettings("ShowChargeNameContainers", "CHARGECONTAINERS", sectionType);
            AddNewPricesFieldSettingToPricesTableSettings("ShowChargeCodeContainers", "CHARGECODECONTAINERS", sectionType);
            AddNewPricesFieldSettingToPricesTableSettings("ShowMeasurementContainers", "MEASUREMENTCONTAINERS", sectionType);
            AddNewPricesFieldSettingToPricesTableSettings("ShowFixedPriceContainers", "FIXEDPRICECONTAINERS", sectionType);
            AddNewPricesFieldSettingToPricesTableSettings("ShowPriceByContainerColumn", "PRICEBYCONTAINERS", sectionType);
            AddNewPricesFieldSettingToPricesTableSettings("ShowSaleCurrencyColumnContainers", "TOTALCONTAINERS", sectionType);
            AddNewPricesFieldSettingToPricesTableSettings("ShowLocalCurrencyColumnContainers", "LOCALAMOUNTCONTAINERS", sectionType);
            AddNewPricesFieldSettingToPricesTableSettings("ShowChargeDescriptionContainers", "CHARGEDESCRIPTIONCONTAINERS", sectionType);
            AddNewPricesFieldSettingToPricesTableSettings("ShowChargeNoteContainers", "CHARGENOTECONTAINERS", sectionType);
            AddNewPricesFieldSettingToPricesTableSettings("ShowSaleMaxMinAmountContainers", "SALEMINMAXCONTAINERS", sectionType);
            AddNewPricesFieldSettingToPricesTableSettings("ShowIncludedChargesContainers", "INCLUDEDCHARGESCONTAINERS", sectionType);
            AddNewPricesFieldSettingToPricesTableSettings("ShowRegionalTAXContainers", "ISREGIONALTAXCONTAINERS", sectionType);
            AddNewPricesFieldSettingToPricesTableSettings("ShowVATTypeContainers", "VATTYPECONTAINERS", sectionType);
            AddNewPricesFieldSettingToPricesTableSettings("ShowVATPercentageContainers", "VATPERCENTAGECONTAINERS", sectionType);
        }
    }
}