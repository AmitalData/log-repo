using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class RevenueExpenseDataProvider: BaseDataProvider
    {

        public DateTime? ForDate { get; set; }
        public string Level { get; set; }
        public decimal? TotlaRevenue { get; set;}
        public decimal? TotalExpenses { get; set; }
        public List<ExpenseList> ExpenseList { get; set; }
        public List<RevenueList> RevenueList { get; set; }
        public List<ResultList> ResultList { get; set; }
        public decimal? TotalRevenueExpense { get; set; }

        public decimal? TotalLocalOpenBalance { get; set; }
        public decimal? TotalLocalDebit { get; set; }
        public decimal? TotalLocalCredit { get; set; }
        public decimal? TotalLocalCloseBalance { get; set; }



        public decimal? TotalForeignOpenBalance { get; set; }

        public decimal? TotalForeignDebit { get; set; }

        public decimal? TotalForeignCredit { get; set; }

        public decimal? TotalForeignCloseBalance { get; set; }
        public bool CurrencyDetailed { get; set; }
        public string DetailedVendorsAccounts { get; set; }
        public string DetailedCustomersAccounts { get; set; }
        public string Category { get; set; }
        public bool DontShowCardsWith0Balance { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }


    public class ResultList
    {
        [Key]
        public string Id { get; set; }
        public string ChartOfAccountType { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string AccountDisplayNumber { get; set; }
        public string AccountName { get; set; }

        public string ParentId { get; set; }
        public decimal? Balance { get; set; }
        public decimal? LocalOpenBalance { get; set; }
        public decimal? LocalDebit { get; set; }
        public decimal? LocalCredit { get; set; }
        public decimal? LocalCloseBalance { get; set; }
        public bool Error { get; set; }
        public string Type { get; set; }

        public decimal? ForeignOpenBalance { get; set; }

        public decimal? ForeignDebit { get; set; }

        public decimal? ForeignCredit { get; set; }

        public decimal? ForeignCloseBalance { get; set; }
        public string ChartofAccountCode { get; set; }
        public string ChartofAccountLocalName { get; set; }
        public string ChartofAccountTypeCode { get; set; }
        public string ChartofAccountTypeLocalName { get; set; }
        public string CurrencyCode { get; set; }


    }
    public class ExpenseList
    {
        public string ChartOfAccountType { get; set; }
        public string GLAccountNumber { get; set; }
        public string GLAccountName { get; set; }
        public decimal? Balance { get; set; }
        
    }

    public class RevenueList
    {
        public string ChartOfAccountType { get; set; }
        public string GLAccountNumber { get; set; }
        public string GLAccountName { get; set; }
        public decimal? Balance { get; set; }

    }

}