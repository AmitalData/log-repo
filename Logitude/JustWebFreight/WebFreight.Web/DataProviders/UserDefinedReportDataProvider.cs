using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class UserDefinedReportDataProvider : BaseDataProvider
    { 
        public DateTime? FirstPeriodDateFrom { get; set; }
        public DateTime? FirstPeriodDateTo { get; set; }
        public DateTime? SecoundPeriodDateFrom { get; set; }
        public DateTime? SecoundPeriodDateTo { get; set; }
        public bool IncludeAnOpeningBalance { get; set; }
        public bool ExpandChartOfAccountToGLAccounts { get; set; }
        public UserDefinedReportPeriod UserDefinedReportPeriod { get; set; }

    }

    public class UserDefinedReportPeriod
    {
        public string EnglishName { get; set; }
        public string LocalName { get; set; }
        public string CreateByUserLocalName { get; set; }
        public string CreateByUserEnglishName { get; set; }
        public List<CalculatedChartsOfAccountPeriod> CalculatedChartsOfAccountPeriods { get; set; }
        public List<CalculatedChartsOfAccountsLinePeriod> AllCalculatedChartsOfAccountsLinePeriods { get; set; }
    }


    public class CalculatedChartsOfAccountPeriod
    {
        public string LocalName { get; set; }
        public string EnglishName { get; set; }
        public string Id { get; set; }
        public string ChartOfAccountTypeLocalName { get; set; }
        public string ChartOfAccountTypeEnglishName { get; set; }
        public string ChartOfAccountTypeCode { get; set; }
        public List<CalculatedChartsOfAccountsLinePeriod> CalculatedChartsOfAccountsLinePeriods { get; set; }

    }

    public class CalculatedChartsOfAccountsLinePeriod
    {
 
        public decimal  FirstPeriodAmount { get; set; }
        public decimal  SecoundPeriodAmount { get; set; }
        public decimal  Difference  { get; set; }
        public string LocalName { get; set; }
        public string EnglishName { get; set; }
        public string Code { get; set; }
        public string EnglishType { get; set; }
        public string LocalType { get; set; }
        public string GLAccountId { get; set; }
        public string ChartsofAccountId { get; set; }
        public string ChartsofAccountTypeCode { get; set; }
        public string ParentChartsofAccountTypeCode { get; set; }
        public string LineTypeCode { get; set; }
        public string ParentLocalType { get; set; }
        public string ParentEnglishType { get; set; }
        public string SubParentLocalType { get; set; }
        public string SubParentEnglishType { get; set; }
        public bool IsParent { get; set; }
        public bool IsSubParent { get; set; }
        public string OriginalChartOfAccountTypeCode { get; set; }
    }



}