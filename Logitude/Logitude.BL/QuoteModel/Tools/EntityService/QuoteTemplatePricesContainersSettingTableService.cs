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
    public class QuoteTemplatePricesContainersSettingTableService
    {
        //QuoteTemplateBuildArges quoteTemplateBuildArges = null;
        //QuoteTemplateSettingPM quoteTemplateSettingPM = null;
        //bool IsShowLocalLanguage = new bool();
        //string TranslateInclueLable = string.Empty;
        //string LocalCurrencyCode = string.Empty;
        //public QuoteTemplatePricesContainersSettingTableService(QuoteTemplateBuildArges quoteTemplateBuildArges, QuoteTemplateSettingPM quoteTemplateSettingPM)
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



        //public string GetchargecodecontainersValue(QuoteSaleChargePM quoteSaleChargePM)
        //{
        //    return quoteSaleChargePM.ChargesTypeCode;
        //}
        
        //public string GetchargecontainersValue(QuoteSaleChargePM quoteSaleChargePM)
        //{
        //    return IsShowLocalLanguage && quoteSaleChargePM.ChargesTypeLocalName != null ? quoteSaleChargePM.ChargesTypeLocalName : quoteSaleChargePM.ChargesTypeName;
        //}
        
        //public string GetmeasurementcontainersValue(QuoteSaleChargePM quoteSaleChargePM)
        //{
        //    bool included = (quoteSaleChargePM.IsAllIN != null && quoteSaleChargePM.IsAllIN.ToLower() == "true" ? true : false);
        //    string unitspackagesValue = " ";
        //    if (quoteSaleChargePM.SaleQuantity != null) unitspackagesValue = ((double)quoteSaleChargePM.SaleQuantity).ToString("N");
        //    if (included) unitspackagesValue = TranslateInclueLable;
        //    return unitspackagesValue;
        //}
        
        //public string GetfixedpricecontainersValue(QuoteSaleChargePM quoteSaleChargePM)
        //{
            
        //    return "";
        //}
        
        



        //public string GettotalcontainersValue(QuoteSaleChargePM quoteSaleChargePM)
        //{
        //    return quoteTemplateSettingPM.RightToLeft ? quoteSaleChargePM.SaleMeasurementLocalName : quoteSaleChargePM.SaleMeasurementShortName;
        //}
        
        //public string GetlocalamountcontainersValue(QuoteSaleChargePM quoteSaleChargePM)
        //{
        //    return "";
        //}
        
        //public string GetchargedescriptioncontainersValue(QuoteSaleChargePM quoteSaleChargePM)
        //{
        //    bool included = (quoteSaleChargePM.IsAllIN != null && quoteSaleChargePM.IsAllIN.ToLower() == "true" ? true : false);
        //    string localamountpackagesValue = " ";
        //    if (quoteSaleChargePM.SaleTotalAmountLocal != null)
        //        localamountpackagesValue = ((double)quoteSaleChargePM.SaleTotalAmountLocal).ToString("N") + " " + LocalCurrencyCode;
        //    if (included) localamountpackagesValue = TranslateInclueLable;
        //    return localamountpackagesValue;
        //}
        
        //public string GetchargenotecontainersValue(QuoteSaleChargePM quoteSaleChargePM)
        //{
        //    return quoteSaleChargePM.ChargesTypeDescription;
        //}
        
        //public string GetsaleminmaxcontainersValue(QuoteSaleChargePM quoteSaleChargePM)
        //{
        //    return quoteSaleChargePM.Notes;
        //}
        
        //public string GetisregionaltaxcontainersValue(QuoteSaleChargePM quoteSaleChargePM)
        //{
        //    bool included = (quoteSaleChargePM.IsAllIN != null && quoteSaleChargePM.IsAllIN.ToLower() == "true" ? true : false);
        //    return TranslateInclueLable ;
        //}
        
        //public string GetvattypecontainersValue(QuoteSaleChargePM quoteSaleChargePM)
        //{
        //    return quoteSaleChargePM.IsRegionalTax ? "Yes" : "No";
        //}
        
        //public string GetvatpercentagecontainersValue(QuoteSaleChargePM quoteSaleChargePM)
        //{
        //    if (quoteTemplateBuildArges.QuotePM.IsChargesByVAT)
        //        return quoteSaleChargePM.VatTypeName;
        //    return "XDontAppendToHtmlTemplateX";
        //}
    }
}
