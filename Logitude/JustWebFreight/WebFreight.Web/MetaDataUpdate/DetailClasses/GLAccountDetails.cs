using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.MetaDataUpdate.DetailClasses
{
    public class GLAccountDetails
    {
        public string InternalNumber { get; set; }
        public string AccountTypeCode { get; set; }
        public string AccountTypeName { get; set; }
        public string DisplayNumber { get; set; }
        public string LocalName { get; set; }
        public string EnglishName { get; set; }
        public bool IsMultiCurrency { get; set; }
        public bool IsControlAccount { get; set; }
        public string CurrencyId { get; set; }
        public string CurrencyName{ get; set; }
        public string RevenueExpenseType { get; set; }
        public string RevenueExpenseName { get; set; }
        public string ChartOfAccountsId { get; set; }
        public string ChartOfAccountsName { get; set; }
        public string SearchFileds { get; set; }
        public bool Inactive { get; set; }
        public string ReconcileMethodCode { get; set; }
        public string ReconcileMethodName { get; set; }
    }
}