using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Logitude.Accounting.Data;
using Logitude.Server.Tools.Helpers;

[assembly: InternalsVisibleTo("Logitude.UnitTest")]

namespace Logitude.Accounting.BL.CoreBL
{
    public class AccountingSettingResolver : Logitude.Accounting.BL.CoreBL.IAccountingSettingResolver
    {
        
        
        //internal static string ResolveCard1AccountType()
        //{
        //    //Code	LocalName	EnglishName	Inactive
        //    //1	כרטיס	Card	0
        //    //2	לקוח	Client	0
        //    //3	ספק	Vendor	0
        //    //4	ג'וב	Job	0

        //    return "1";
        //}

        //internal static string ResolveLocalCurrency0ReconcileMethodCode()
        //{
        //    //Code	EnglishName	LocalName
        //    //0	Local Currency	מטבע מקומי
        //    //1	Foreign Currency	מטבע חוץ
        //    return "0";//Local Currency
        //}
        public decimal ResolveVat(int tenant, DateTime documentDate)
        {
            //accountingDate = accountingDate ?? DateTime.Now;


            var percentageList = GetAccountingVatList(tenant);

            documentDate = documentDate.Date;
            var percentagePM1 = percentageList.OrderByDescending(d => d.FromDate).ToList().Where(rec => rec.FromDate.Value.Date  <= documentDate).FirstOrDefault();
            if (percentagePM1 == null)
            {
                throw new ApplicationException("No Vat definition for Document Date " + documentDate.ToShortDateString());
            }

            return Convert.ToDecimal(((percentagePM1.Percentage + 100) / 100));
            ////return 1;
        }

        public virtual List<VatTypePercentagePM> GetAccountingVatList(int tenant)
        {
            string key = "ResolveAccountingVatList," + tenant.ToString();

            var percentageList = CacheManager.GetOrInsertNewObject<List<VatTypePercentagePM>>(key, () =>
            {
                var full = new FullAccountingSettingQueryService(tenant);
                var fullPm = full.GetSingleFullAccountingSetting(tenant);

                var vatTypePercentageQuery = new VatTypePercentageQuery(tenant);
                if (String.IsNullOrEmpty(fullPm.DefaultVATTypeId))
                {
                    string textCode = "Accounting.General.O.DefaultVATTypeMissing";
                    bool getLocal = true;
                    string errortext = TranslateTextsClass.Translate(textCode, tenant, getLocal);
                    if (String.IsNullOrEmpty(errortext)) errortext = "Default VAT Type is missing in the Full Accounting Settings";
                    throw new ApplicationException(errortext);
                }
                var percentagesQuery = vatTypePercentageQuery.GetVatTypePercentagesForVatType(tenant, fullPm.DefaultVATTypeId);
                var percentagePM = percentagesQuery
                    //.Where(rec => DbFunctions.TruncateTime(rec.FromDate) <= documentDate)
                    .OrderByDescending(d => d.FromDate).ToList();
                return percentagePM;
            }, true);
            return percentageList;
        }
        public string ResolveVATOutputGLAccountId(int tenant)
        {
            var pm=FullAccountingSettingQueryService.Get(tenant);
            return pm.VATOutputGLAccountId;
        }

        public  string ResolveAccountingCurrencyId(int tenant)
        {
            string key = "ResolveAccountingCurrencyId," + tenant.ToString();

            string val = CacheManager.GetOrInsertNewObject<string>(key, () =>
            {
                TenantQuery tenantQuery = new TenantQuery(tenant);
                TenantPM tPM = tenantQuery.GetSinglePM(tenant);

                //return tPM.AccountingCurrencyCode;
                //CurrencyId = a.CurrencyId,
                //AccountingCurrencyCode = a.Currency != null ? a.Currency.Code : null,
                //AccountingCurrencyName = a.Currency != null ? a.Currency.EnglishName : null,
                return tPM.CurrencyId;
            }, true);
            return val;
        }
    }
}
