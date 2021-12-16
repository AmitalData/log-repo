using Logitude.Accounting.Data;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.BL.CommonDataModel.APIDataContract
{
    public class GLAccount
    {
        public string DisplayNumber { get; set; }
        public string LocalName { get; set; }
        public string EnglishName { get; set; }
        public bool? IsMultiCurrency { get; set; }
        public string InternalNumber { get; set; }
        public string DeductionFileNumber { get; set;}
        public string AssessingOfficeCode { get; set; }
        public string DeductionFileTypeCode { get; set; }
        public string DeductionTypeCode { get; set; }
        public string Occupation { get; set; }
        public string ConsolidationVat { get; set; }
        public Currency Currency { get; set; }
        public ReconcileMethod ReconcileMethod { get; set; }
        public ChartOfAccount ChartOfAccount { get; set; }
    }

    public class ReconcileMethod
    {
        [XmlAttribute]
        public string Code { get; set; }
        public string EnglishName { get; set; }
        public string LocalName { get; set; }
    }

    public class ChartOfAccount
    {
        [XmlAttribute]
        public string Code { get; set; }

        [XmlAttribute]
        public string Id { get; set; }

        public string LocalName { get; set; }
        public string EnglishName { get; set; }
    }

    public class GLAccountPM
    {
        public string Id { get; set; }
        public string DisplayNumber { get; set; }
        public string InternalNumber { get; set; }

        public string LocalName { get; set; }
        public string EnglishName { get; set; }
        public bool? IsMultiCurrency { get; set; }

        public string CurrencyId { get; set; }
        public string ReconcileMethodCode { get; set; }
        public string ChartOfAccountCode { get; set; }
    }

    public class GLAccountQueryService
    {
        IAccountingContext accountingContext;
        public GLAccountQueryService(int tenant)
        {
            
        }

        public GLAccount GLAccountCustomDataMapping(CardPM cardPM, int Tenant, string ComputingPartnerName = "")
        {
             accountingContext = AccountingContext.GetContext(Tenant);
             var glaccount = (from a in accountingContext.GLAccounts.Include("AccountingCompanyType").Include("TaxWithholdingAssessOffice").Include("WithholdingTaxDeductionType")
             where a.Id == cardPM.GLAccountId && a.Tenant == Tenant

             select new GLAccount()
             {
                 DisplayNumber = a.DisplayNumber,
                 InternalNumber = a.InternalNumber,
                 EnglishName = a.EnglishName,
                 LocalName = a.LocalName,
                 IsMultiCurrency = a.IsMultiCurrency,
                 DeductionTypeCode = a.AccountingCompanyType != null ? a.AccountingCompanyType.Code : null,
                 Occupation = a.Occupation,
                 ConsolidationVat = a.ConsolidationVat,
                 AssessingOfficeCode = a.TaxWithholdingAssessOffice != null ? a.TaxWithholdingAssessOffice.Code : null,
                 DeductionFileTypeCode = a.WithholdingTaxDeductionType != null ? a.WithholdingTaxDeductionType.Code : null,
                 DeductionFileNumber = a.DeductionFileNumber,
                 ChartOfAccount = new ChartOfAccount() { 
                     Id = a.ChartOfAccount.Id,
                     Code = a.ChartOfAccount.Code,
                     EnglishName = a.ChartOfAccount.EnglishName,
                     LocalName = a.ChartOfAccount.LocalName
                 },
                 Currency = new Currency(){
                     Id = a.Currency.Id,
                     Code = a.Currency.Code,
                     EnglishName = a.Currency.EnglishName
                    },
                 ReconcileMethod = new ReconcileMethod {
                 Code = a.ReconcileMethod.Code,
                 EnglishName = a.EnglishName,
                 LocalName = a.LocalName,
                 },

             }).FirstOrDefault();

            return glaccount;
        }

        public GLAccountPM GLAccountCustomDataMappingAndValidatin(GLAccount MyEntity, int Tenant, string ComputingPartnerName = "")
        {
            try
            {
                return new GLAccountPM();
            
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
