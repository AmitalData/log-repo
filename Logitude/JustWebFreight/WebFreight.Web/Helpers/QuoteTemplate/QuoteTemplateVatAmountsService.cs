using Logitude.BL.Helpers;
using Logitude.BL.QuoteModel.DataContracts;
using Logitude.BL.QuoteModel.EntityPMs;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebFreight.Web.Helpers.QuoteTemplate
{
    public class QuoteTemplateVatAmountsService
    {
        private readonly QuoteTemplateReportHelper quoteTemplateReportHelper;
        private readonly VatAmountsAruments vatAmountsAruments;
        private readonly bool localBeforeSale;

        public Dictionary<string, string> headerColumns;
        public List<PricesFieldSettings> pricesPackagesTableSettings;
        public List<PricesFieldSettings> pricesContainersTableSettings;

        public Dictionary<string, string> tableRows;

        private readonly int shiftListByOneColumn = 1;
        private readonly int shiftListByThreeColumns = 3;
        public QuoteTemplateVatAmountsService(QuoteTemplateReportHelper quoteTemplateReportHelper, VatAmountsAruments vatAmountsAruments)
        {
            headerColumns = new Dictionary<string, string>();
            tableRows = new Dictionary<string, string>();
            pricesPackagesTableSettings = new List<PricesFieldSettings>();
            pricesContainersTableSettings = new List<PricesFieldSettings>();

            this.quoteTemplateReportHelper = quoteTemplateReportHelper;
            this.vatAmountsAruments = vatAmountsAruments;
            this.localBeforeSale = GetLocalBeforeSale();
        }

        private bool GetLocalBeforeSale()
        {
            if (IsPackage()) return GetPackageLocalBeforeSale();
            else return GetContainerLocalBeforeSale();
        }

        private bool GetContainerLocalBeforeSale()
        {
            if (vatAmountsAruments.PricesContainersTableSettings == null) return false;
            if (!vatAmountsAruments.QuoteTemplateSettingPM.ShowSaleCurrencyColumnContainers) return true;
            if (!vatAmountsAruments.QuoteTemplateSettingPM.ShowLocalCurrencyColumnContainers) return false;
            return vatAmountsAruments.PricesContainersTableSettings.FirstOrDefault(x => x.Code == "TOTALCONTAINERS").Index > vatAmountsAruments.PricesContainersTableSettings.FirstOrDefault(x => x.Code == "LOCALAMOUNTCONTAINERS").Index;
        }

        private bool GetPackageLocalBeforeSale()
        {
            if (vatAmountsAruments.PricesPackagesTableSettings == null) return false;
            if (!vatAmountsAruments.QuoteTemplateSettingPM.ShowSaleCurrencyColumnPackages) return true;
            if (!vatAmountsAruments.QuoteTemplateSettingPM.ShowLocalCurrencyColumnPackages) return false;
            return vatAmountsAruments.PricesPackagesTableSettings.FirstOrDefault(x => x.Code == "TOTALPACKAGES").Index > vatAmountsAruments.PricesPackagesTableSettings.FirstOrDefault(x => x.Code == "LOCALAMOUNTPACKAGES").Index;
        }

        private bool IsPackage()
        {
            return vatAmountsAruments.PricingSectionType == "PP";
        }

        public void Reset()
        {
            headerColumns = new Dictionary<string, string>();
            tableRows = new Dictionary<string, string>();
            pricesPackagesTableSettings = new List<PricesFieldSettings>();
            pricesContainersTableSettings = new List<PricesFieldSettings>();
        }

        public void SetQuoteSaleChargePM(QuoteSaleChargePM quoteSaleChargePM)
        {
            vatAmountsAruments.QuoteSaleChargePM = quoteSaleChargePM;
        }

        public void BuildSaleVatPackagesHeaderColumns()
        {
            if (!vatAmountsAruments.QuotePM.IsChargesByVAT || !vatAmountsAruments.QuoteTemplateSettingPM.ShowSaleCurrencyColumnPackages) return;

            var shiftBy = localBeforeSale ? shiftListByThreeColumns : shiftListByOneColumn;
            var index = vatAmountsAruments.PricesPackagesTableSettings.FirstOrDefault(x => x.Code == "TOTALPACKAGES").Index + shiftBy;
            headerColumns.Add("VATPACKAGES", BuildVatHeaderColumnValue("VATPACKAGES", true, index));
            headerColumns.Add("TOTALINCLUDINGVATPACKAGES", BuildVatHeaderColumnValue("TOTALINCLUDINGVATPACKAGES", true, index + 1));
        }

        public void BuildSaleVatContainersHeaderColumns()
        {
            if (!vatAmountsAruments.QuotePM.IsChargesByVAT || !vatAmountsAruments.QuoteTemplateSettingPM.ShowSaleCurrencyColumnContainers) return;

            var shiftBy = localBeforeSale ? shiftListByThreeColumns : shiftListByOneColumn;
            var index = vatAmountsAruments.PricesContainersTableSettings.FirstOrDefault(x => x.Code == "TOTALCONTAINERS").Index + shiftBy;
            headerColumns.Add("VATCONTAINERS", BuildVatHeaderColumnValue("VATCONTAINERS", false, index));
            headerColumns.Add("TOTALINCLUDINGVATCONTAINERS", BuildVatHeaderColumnValue("TOTALINCLUDINGVATCONTAINERS", false, index + 1));
        }

        public void BuildLocalVatPackagesHeaderColumns()
        {
            if (!vatAmountsAruments.QuotePM.IsChargesByVAT || !vatAmountsAruments.QuoteTemplateSettingPM.ShowLocalCurrencyColumnPackages) return;

            var shiftBy = localBeforeSale ? shiftListByOneColumn : shiftListByThreeColumns;
            var index = vatAmountsAruments.PricesPackagesTableSettings.FirstOrDefault(x => x.Code == "LOCALAMOUNTPACKAGES").Index + shiftBy;
            headerColumns.Add("LOCALVATPACKAGES", BuildVatHeaderColumnValue("LOCALVATPACKAGES", true, index));
            headerColumns.Add("TOTALINCLUDINGVATLOCALPACKAGES", BuildVatHeaderColumnValue("TOTALINCLUDINGVATLOCALPACKAGES", true, index + 1));
        }

        public void BuildLocalVatContainersHeaderColumns()
        {
            if (!vatAmountsAruments.QuotePM.IsChargesByVAT || !vatAmountsAruments.QuoteTemplateSettingPM.ShowLocalCurrencyColumnContainers) return;

            var shiftBy = localBeforeSale ? shiftListByOneColumn : shiftListByThreeColumns;
            var index = vatAmountsAruments.PricesContainersTableSettings.FirstOrDefault(x => x.Code == "LOCALAMOUNTCONTAINERS").Index + shiftBy;
            headerColumns.Add("LOCALVATCONTAINERS", BuildVatHeaderColumnValue("LOCALVATCONTAINERS", false, index));
            headerColumns.Add("TOTALINCLUDINGVATLOCALCONTAINERS", BuildVatHeaderColumnValue("TOTALINCLUDINGVATLOCALCONTAINERS", false, index + 1));
        }

        public Dictionary<string, string> AddColumns(Dictionary<string, string> allHeaderColumns, Dictionary<string, string> vatColumns)
        {
            foreach (var item in vatColumns) allHeaderColumns.Add(item.Key, item.Value);
            return allHeaderColumns;
        }

        private string BuildVatHeaderColumnValue(string textCode, bool isPackage, int index)
        {
            if (isPackage) pricesPackagesTableSettings.Add(new PricesFieldSettings { Code = textCode, Name = textCode, InUse = true, Index = index });
            else pricesContainersTableSettings.Add(new PricesFieldSettings { Code = textCode, Name = textCode, InUse = true, Index = index });
            return quoteTemplateReportHelper.GetHeaderColumn(textCode, vatAmountsAruments.StringBuilder, vatAmountsAruments.QuoteTemplateSettingPM, vatAmountsAruments.QuoteTemplateTextDesignPM, vatAmountsAruments.QuoteTemplateTableDesignPM, vatAmountsAruments.PricingSectionType, vatAmountsAruments.TextCodes);
        }

        public void BuildLocalVatPackagesColumns()
        {
            if (!vatAmountsAruments.QuotePM.IsChargesByVAT || !vatAmountsAruments.QuoteTemplateSettingPM.ShowLocalCurrencyColumnPackages) return;
            tableRows.Add("LOCALVATPACKAGES", BuildVatPackagesRowValue("LOCALVATPACKAGES", "VATAmountInLocalCurrency"));
            tableRows.Add("TOTALINCLUDINGVATLOCALPACKAGES", BuildVatPackagesRowValue("TOTALINCLUDINGVATLOCALPACKAGES", "SaleTotalAmountLocalIncludingVAT"));
        }


        public void BuildSaleVatPackagesColumns()
        {
            if (!vatAmountsAruments.QuotePM.IsChargesByVAT || !vatAmountsAruments.QuoteTemplateSettingPM.ShowSaleCurrencyColumnPackages) return;
            tableRows.Add("VATPACKAGES", BuildVatPackagesRowValue("VATPACKAGES", "VATAmountInLineSaleCurrency"));
            tableRows.Add("TOTALINCLUDINGVATPACKAGES", BuildVatPackagesRowValue("TOTALINCLUDINGVATPACKAGES", "SaleTotalAmountIncludingVAT"));
        }

        public void BuildLocalVatContainersColumns()
        {
            if (!vatAmountsAruments.QuotePM.IsChargesByVAT || !vatAmountsAruments.QuoteTemplateSettingPM.ShowLocalCurrencyColumnContainers) return;
            tableRows.Add("LOCALVATCONTAINERS", BuildVatPackagesRowValue("LOCALVATCONTAINERS", "VATAmountInLocalCurrency"));
            tableRows.Add("TOTALINCLUDINGVATLOCALCONTAINERS", BuildVatPackagesRowValue("TOTALINCLUDINGVATLOCALCONTAINERS", "SaleTotalAmountLocalIncludingVAT"));
        }

        public void BuildSaleVatContainersColumns()
        {
            if (!vatAmountsAruments.QuotePM.IsChargesByVAT || !vatAmountsAruments.QuoteTemplateSettingPM.ShowSaleCurrencyColumnPackages) return;
            tableRows.Add("VATCONTAINERS", BuildVatPackagesRowValue("VATCONTAINERS", "VATAmountInLineSaleCurrency"));
            tableRows.Add("TOTALINCLUDINGVATCONTAINERS", BuildVatPackagesRowValue("TOTALINCLUDINGVATCONTAINERS", "SaleTotalAmountIncludingVAT"));
        }


        private string BuildVatPackagesRowValue(string textCode, string getValueFrom)
        {
            return quoteTemplateReportHelper.AddTableRows(new PricingTableRowDetailsArgs() { FieldCode = textCode, Value = quoteTemplateReportHelper.GetNumberValueFormate(GetPropValue(vatAmountsAruments.QuoteSaleChargePM, getValueFrom)), RowDataType = "Field", QuoteTemplateSettingPM = vatAmountsAruments.QuoteTemplateSettingPM, HeaderDesign = vatAmountsAruments.QuoteTemplateTextDesignPM, TableDesign = vatAmountsAruments.QuoteTemplateTableDesignPM, PricingSectionType = vatAmountsAruments.PricingSectionType });
        }

        public double? GetPropValue(object srcObject, string propName)
        {
            return (double?)srcObject.GetType().GetProperty(propName).GetValue(srcObject, null);
        }

        public List<PricesFieldSettings> AddRangeTableSettings(List<PricesFieldSettings> fieldSettings, List<PricesFieldSettings> newFiledSettings)
        {
            if (newFiledSettings == null || !newFiledSettings.Any()) return fieldSettings;
            fieldSettings = fieldSettings.OrderBy(x => x.Index).ToList();
            newFiledSettings = newFiledSettings.OrderBy(x => x.Index).ToList();

            foreach (var field in newFiledSettings) fieldSettings.Insert(field.Index, field);
            for (int i = 0; i < fieldSettings.Count; i++) fieldSettings[i].Index = i;
            return fieldSettings;
        }
    }

    public class VatAmountsAruments
    {
        public StringBuilder StringBuilder { get; set; }
        public QuoteTemplateSettingPM QuoteTemplateSettingPM { get; set; }
        public QuoteTemplateTextDesignPM QuoteTemplateTextDesignPM { get; set; }
        public QuoteTemplateTableDesignPM QuoteTemplateTableDesignPM { get; set; }
        public string PricingSectionType { get; set; }
        public List<QuoteTemplateTextCodePM> TextCodes { get; set; }
        public QuotePM QuotePM { get; set; }
        public List<PricesFieldSettings> PricesPackagesTableSettings { get; set; }
        public List<PricesFieldSettings> PricesContainersTableSettings { get; set; }
        public QuoteSaleChargePM QuoteSaleChargePM { get; set; }
    }
}