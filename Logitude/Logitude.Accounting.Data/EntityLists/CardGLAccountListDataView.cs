using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Data.EntityPOCOs
{
    public class CardGLAccountListDataView
    {
        // GLAccount table
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string InternalNumber { get; set; }
        public string AccountTypeCode { get; set; }
        public string DisplayNumber { get; set; }
        public string GLAccountLocalName { get; set; }
        public string GLAccountEnglishName { get; set; }
        public string SearchFields { get; set; }
        public bool? IsMultiCurrency { get; set; }
        public string CurrencyId { get; set; }
        public string RevenueExpenseType { get; set; }
        public bool? IsControlAccount { get; set; }
        public string ChartOfAccountsId { get; set; }
        public bool? Inactive { get; set; }
        public string ChartOfAccountsTypeCode { get; set; }
        public string ReconcileMethodCode { get; set; }
        public string ControlAccountId { get; set; }
        public string AutomaticReconcileId { get; set; }
        public string PreviousEnglishName { get; set; }
        public DateTime? PreviousEnglishNameChangeDate { get; set; }
        public string PreviousLocalName { get; set; }
        public DateTime? PreviousLocalNameChangeDate { get; set; }
        public string PreviousNumber { get; set; }
        public DateTime? PreviousNumberChangeDate { get; set; }
        public string PreviousChartOfAccountsId { get; set; }
        public DateTime? PreviousChartOfAccountsChangeDate { get; set; }
        public string CustomerGLAccountId { get; set; }
        public bool? RevaluationEnabled { get; set; }
        public string ParentAccountId { get; set; }
        public string Category1Id { get; set; }
        public string Category2Id { get; set; }
        public string Category3Id { get; set; }
        public string Category4Id { get; set; }
        public string Category5Id { get; set; }
        public bool? IsVATExempt { get; set; }
        public string DeductionFileNumber { get; set; }

        // glaccount list properties
        public string ActiveStatusName { get; set; }
        public string ChartOfAccountsName { get; set; }
        public DateTime? LastActivityDate { get; set; }
        public string LastActivityTypeName { get; set; }
        public string LastActivityByUserName { get; set; }

        // Card
        public string SalesmanUserId { get; set; }
        public string CollectorId { get; set; }
        public string CardEnglishName { get; set; }
        public string CardLocalName { get; set; }
        public string CardGLAccountId { get; set; }
        public string VatTypeId { get; set; }
        public string CountryId { get; set; }
        public string CountryCode { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string PaymentTermId { get; set; }
        public string VatNumber { get; set; }
        
        // Contacts (SalesMans)
        public string SalesManEnglishName { get; set; }
        public string SalesManLocalName { get; set; }

        // Contacts (Collectors)
        public string CollectorEnglishName { get; set; }
        public string CollectorLocalName { get; set; }

        // ChartOfAccounts
        public string ChartOfAccountsLocalName { get; set; }
        public string ChartOfAccountsEnglishName { get; set; }
        public string ChartOfAccountsCode { get; set; }


        // More Data: dummy fields
        public decimal? BalanceInLocalCurrency { get; set; }

        public decimal? LocalBalanceInDue { get; set; }
        public DateTime? NextDueDate { get; set; }

    }
}
