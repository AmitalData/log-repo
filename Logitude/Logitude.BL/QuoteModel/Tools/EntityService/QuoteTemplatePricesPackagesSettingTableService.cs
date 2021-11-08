using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.QuoteModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.BL.QuoteModel.Tools.EntityService
{
    public class QuoteTemplatePricesPackagesSettingTableService
    {
        //QuoteTemplateBuildArges quoteTemplateBuildArges = null;
        //QuoteTemplateSettingPM quoteTemplateSettingPM = null;
        //bool IsShowLocalLanguage = new bool();
        //string TranslateInclueLable = string.Empty;
        //string LocalCurrencyCode = string.Empty;
        //public QuoteTemplatePricesPackagesSettingTableService(QuoteTemplateBuildArges quoteTemplateBuildArges, QuoteTemplateSettingPM quoteTemplateSettingPM)
        //{
        //    this.quoteTemplateBuildArges = quoteTemplateBuildArges;
        //    this.quoteTemplateSettingPM = quoteTemplateSettingPM;
        //    this.IsShowLocalLanguage = quoteTemplateSettingPM.ShowLocalLanguage;
        //    this.TranslateInclueLable = GetTranslateInclueLable(quoteTemplateBuildArges.QuoteTemplateTextCodePMLists, quoteTemplateBuildArges.SectionTypeCode);
        //    this.LocalCurrencyCode = GetLocalCurrencyCode();
        //}

        //private string GetTranslateInclueLable(List<QuoteTemplateTextCodePM> quoteTemplateTextCodePMLists, string pricingSectionType)
        //{
        //    return GetNameColum("INCLUDED", quoteTemplateTextCodePMLists, pricingSectionType);
        //}
        
        //private string GetNameColum(string TextCode, List<QuoteTemplateTextCodePM> textcodes, string typeSetting)
        //{
        //    string NameTextCode = "";
        //    List<QuoteTemplateTextCodePM> qoutetemplatetextcodeList = new List<QuoteTemplateTextCodePM>();
        //    if (typeSetting == "PP") qoutetemplatetextcodeList = textcodes.Where(d => d.Area == "Packages").ToList();
        //    else if (typeSetting == "PC") qoutetemplatetextcodeList = textcodes.Where(d => d.Area == "Containers").ToList();
        //    else if (typeSetting == "QD") qoutetemplatetextcodeList = textcodes.Where(d => d.Area == "QuoteDetails").ToList();
        //    else if (typeSetting == "QH") qoutetemplatetextcodeList = textcodes.Where(d => d.Area == "QuoteHeader").ToList();
        //    else if (typeSetting == "TotalPerContainers") qoutetemplatetextcodeList = textcodes.Where(d => d.Area == "TotalPerContainers").ToList();
            
        //    foreach (QuoteTemplateTextCodePM qoutetemplatetextcode in qoutetemplatetextcodeList)
        //    {
        //        if (qoutetemplatetextcode.TextCode == TextCode)
        //        {
        //            if (IsShowLocalLanguage) NameTextCode = qoutetemplatetextcode.LocalName;
        //            else NameTextCode = qoutetemplatetextcode.EnglishName;
        //            break;
        //        }
        //    }
        //    return NameTextCode;
        //}

        //private string GetLocalCurrencyCode()
        //{
        //    string email = HttpContext.Current.User.Identity.Name;
        //    int tenant = quoteTemplateBuildArges.UserTenant != null ? (int)quoteTemplateBuildArges.UserTenant : quoteTemplateBuildArges.Tenant;

        //    string localCurrencyCode = string.Empty;

        //    ContactRepository contactrep = new ContactRepository(tenant);
        //    Contact contact = contactrep.GetSingleContactByEmail(email, tenant);
        //    CurrencyRepository currencyRepository = new CurrencyRepository(tenant);

        //    if (contact != null)
        //    {
        //        SystemDataQuery systemDataQuery = new SystemDataQuery(tenant);
        //        SystemDataPM systemEntity = systemDataQuery.GetSinglePM(contact.Id, tenant);

        //        if (systemEntity != null)
        //        {
        //            if (!string.IsNullOrEmpty(systemEntity.LocalCurrencyId))
        //            {
        //                Currency currency = currencyRepository.GetSingleCurrency(systemEntity.LocalCurrencyId, tenant);
        //                if (currency != null) localCurrencyCode = currency.Code;
        //            }
        //        }
        //    }

        //    return localCurrencyCode;
        //}

        //public string GetchargecodepackagesValue(QuoteSaleChargePM quoteSaleChargePM)
        //{
        //    return quoteSaleChargePM.ChargesTypeCode;
        //}
        
        //public string GetchargepackagesValue(QuoteSaleChargePM quoteSaleChargePM)
        //{
        //    return IsShowLocalLanguage && quoteSaleChargePM.ChargesTypeLocalName != null ? quoteSaleChargePM.ChargesTypeLocalName : quoteSaleChargePM.ChargesTypeName;
        //}
        
        //public string GetunitspackagesValue(QuoteSaleChargePM quoteSaleChargePM)
        //{
        //    bool included = (quoteSaleChargePM.IsAllIN != null && quoteSaleChargePM.IsAllIN.ToLower() == "true" ? true : false);
        //    string unitspackagesValue = " ";
        //    if (quoteSaleChargePM.SaleQuantity != null) unitspackagesValue = ((double)quoteSaleChargePM.SaleQuantity).ToString("N");
        //    if (included) unitspackagesValue = TranslateInclueLable;
        //    return unitspackagesValue;
        //}
        
        //public string GetunitpricepackagesValue(QuoteSaleChargePM quoteSaleChargePM)
        //{
        //    string saleUnitPriceValues = "";
        //    bool included = (quoteSaleChargePM.IsAllIN != null && quoteSaleChargePM.IsAllIN.ToLower() == "true" ? true : false);
        //    QuotePM quotePM = quoteTemplateBuildArges.QuotePM;

        //    if (quoteSaleChargePM.IsChargeBySteps && quotePM.QuoteTypeCode == "P")
        //    {
        //        if (!string.IsNullOrEmpty(quoteSaleChargePM.PriceBreaks))
        //        {
        //            var priceBreaks = quoteSaleChargePM.PriceBreaks;
        //            priceBreaks = priceBreaks.Replace("\r", "^");
        //            var priceBreaksArrays = priceBreaks.Split('^');
        //            priceBreaks = null;
        //            int i = 1;
        //            foreach (var item in priceBreaksArrays)
        //            {
        //                var priceBreak = FormatPriceBreaksWithTwoDecimalDigits(item);
        //                if (i > 1 || quoteTemplateBuildArges.QuoteTemplateSettingPM.RightToLeft) priceBreaks += "&nbsp;";
        //                priceBreaks += ((priceBreak + " " + GetChargeCurrencyCode(quotePM, quoteSaleChargePM)) + "<br>");


        //                i += 1;
        //            }
        //            saleUnitPriceValues = priceBreaks;

        //        }
        //    }
        //    else
        //    {
        //        if (quoteSaleChargePM.SaleUnitPrice != null)
        //        {
        //            double value = (double)quoteSaleChargePM.SaleUnitPrice;
        //            saleUnitPriceValues = value.ToString("N"); // 1,234.512
        //            string saleUnitPriceCurrency = quoteSaleChargePM.SaleMeasurementCode == "PRFR" ? "%" : quoteSaleChargePM.CurrencyCode;
        //            saleUnitPriceValues += " " + saleUnitPriceCurrency;
        //        }
        //        if (included)
        //        {
        //            saleUnitPriceValues = GetNameColum("INCLUDED", quoteTemplateBuildArges.QuoteTemplateTextCodePMLists, quoteTemplateBuildArges.SectionTypeCode);
        //        }
        //    }

        //    return saleUnitPriceValues;
        //}
        
        //private string FormatPriceBreaksWithTwoDecimalDigits(string priceBreak)
        //{
        //    string priceBreakFormatted = priceBreak;
        //    if (!string.IsNullOrEmpty(priceBreak) && priceBreak.Contains(":"))
        //    {
        //        string priceBreakPartDigitNumber = priceBreak.Split(':')[1];
        //        if (!string.IsNullOrEmpty(priceBreakPartDigitNumber))
        //        {
        //            string stringDigitNumber = Regex.Replace(priceBreakPartDigitNumber, @"\D", "");
        //            if (!string.IsNullOrEmpty(stringDigitNumber))
        //            {
        //                string priceBreakPartDigitNumberFormated = priceBreakPartDigitNumber.Replace(stringDigitNumber, Double.Parse(stringDigitNumber).ToString("N"));
        //                priceBreakFormatted = priceBreakFormatted.Replace(priceBreakPartDigitNumber, priceBreakPartDigitNumberFormated);
        //            }
        //        }
        //    }
        //    return priceBreakFormatted;
        //}

        //private static string GetChargeCurrencyCode(QuotePM quotePM, QuoteSaleChargePM chargePM)
        //{
        //    return (quotePM.IsSaleCurrencySameAsCost || quotePM.IsMultiCurrency) ? chargePM.CurrencyCode : quotePM.SaleCurrencyCode;
        //}

        //public string GetmeasurementpackagesValue(QuoteSaleChargePM quoteSaleChargePM)
        //{
        //    return quoteTemplateSettingPM.RightToLeft ? quoteSaleChargePM.SaleMeasurementLocalName : quoteSaleChargePM.SaleMeasurementShortName;
        //}
        
        //public string GettotalpackagesValue(QuoteSaleChargePM quoteSaleChargePM)
        //{
        //    bool included = (quoteSaleChargePM.IsAllIN != null && quoteSaleChargePM.IsAllIN.ToLower() == "true" ? true : false);
        //    string totalpackagesValue = " ";
        //    if (quoteSaleChargePM.SaleTotalAmount != null)
        //        totalpackagesValue = ((double)quoteSaleChargePM.SaleTotalAmount).ToString("N") + " " + GetChargeCurrencyCode(quoteTemplateBuildArges.QuotePM, quoteSaleChargePM);
        //    if (included) totalpackagesValue = TranslateInclueLable;
        //    return totalpackagesValue;
        //}
        
        //public string GetlocalamountpackagesValue(QuoteSaleChargePM quoteSaleChargePM)
        //{
        //    bool included = (quoteSaleChargePM.IsAllIN != null && quoteSaleChargePM.IsAllIN.ToLower() == "true" ? true : false);
        //    string localamountpackagesValue = " ";
        //    if (quoteSaleChargePM.SaleTotalAmountLocal != null)
        //        localamountpackagesValue = ((double)quoteSaleChargePM.SaleTotalAmountLocal).ToString("N") + " " + LocalCurrencyCode;
        //    if (included) localamountpackagesValue = TranslateInclueLable;
        //    return localamountpackagesValue;
        //}
        
        //public string GetchargedescriptionpackagesValue(QuoteSaleChargePM quoteSaleChargePM)
        //{
        //    return quoteSaleChargePM.ChargesTypeDescription;
        //}
        
        //public string GetchargenotepackagesValue(QuoteSaleChargePM quoteSaleChargePM)
        //{
        //    return quoteSaleChargePM.Notes;
        //}
        
        //public string GetsaleminmaxpackagesValue(QuoteSaleChargePM quoteSaleChargePM)
        //{
        //    bool included = (quoteSaleChargePM.IsAllIN != null && quoteSaleChargePM.IsAllIN.ToLower() == "true" ? true : false);
        //    return included ? TranslateInclueLable : GetSaleMaxMinAmountValue(quoteSaleChargePM);
        //}
        
        //private string GetSaleMaxMinAmountValue(QuoteSaleChargePM chargePM)
        //{
        //    var saleMinAmount = string.Empty;
        //    var saleMaxAmount = string.Empty;
        //    string saleMaxMinAmount = string.Empty;

        //    if (chargePM.SaleMinAmount != null && chargePM.SaleMinAmount > 0)
        //    {
        //        string AA = " ";
        //        double value = (double)chargePM.SaleMinAmount;
        //        AA = value.ToString("N");
        //        saleMinAmount = "min " + AA;
        //        saleMaxMinAmount = saleMinAmount;
        //    }

        //    if (chargePM.SaleMaxAmount != null && chargePM.SaleMaxAmount > 0)
        //    {
        //        string AA = " ";
        //        double value = (double)chargePM.SaleMaxAmount;
        //        AA = value.ToString("N");
        //        saleMaxAmount = "max " + AA;
        //        if (!string.IsNullOrEmpty(saleMaxMinAmount)) saleMaxMinAmount += " , ";
        //        saleMaxMinAmount += saleMaxAmount;
        //    }
        //    return saleMaxMinAmount;
        //}
        
        //public string GetisregionaltaxpackagesValue(QuoteSaleChargePM quoteSaleChargePM)
        //{
        //    return quoteSaleChargePM.IsRegionalTax ? "Yes" : "No";
        //}
        
        //public string GetvattypepackagesValue(QuoteSaleChargePM quoteSaleChargePM)
        //{
        //    if (quoteTemplateBuildArges.QuotePM.IsChargesByVAT)
        //        return quoteSaleChargePM.VatTypeName;
        //    return "XDontAppendToHtmlTemplateX";
        //}
        
        //public string GetvatpercentagepackagesValue(QuoteSaleChargePM quoteSaleChargePM)
        //{
        //    if (quoteTemplateBuildArges.QuotePM.IsChargesByVAT)
        //        return quoteSaleChargePM.VatPercentage != null ? (quoteSaleChargePM.VatPercentage + "%") : "";
        //    return "XDontAppendToHtmlTemplateX";
        //}
    }

    //public class QuoteTemplateBuildArges
    //{
    //    public List<QuoteTemplateTableDesignPM> QuoteTemplateTableDesignsLists { get; set; }
    //    public List<QuoteTemplateTextCodePM> QuoteTemplateTextCodePMLists { get; set; }
    //    public QuotePM QuotePM { get; set; }
    //    public QuoteTemplatePM QuoteTemplatePM { get; set; }
    //    public QuoteTemplateSettingPM QuoteTemplateSettingPM { get; set; }
    //    public List<QuoteTemplateTextDesignPM> QuoteTemplateTextDesignPMLists { get; set; }
    //    public string SectionTypeCode { get; set; }
    //    public bool IsResultPDF { get; set; }
    //    public int Tenant { get; set; }
    //    public int? UserTenant { get; set; }
    //    public string UserId { get; set; }
    //    public List<QuoteTemplateSectionPM> QuoteTemplateSectionPMLists { get; set; }
    //    public bool HideQuoteHeaderFromPdf { get; set; }
    //    public string RequestArea { get; set; }
    //    public bool FirstSectionInBody { get; set; }
    //    public int? VersionNumber { get; set; }
    //}
}
