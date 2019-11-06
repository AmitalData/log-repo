
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.QuoteModel.APIDataContract.ApiV1; 
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using System.Xml.Serialization;

namespace Logitude.Accounting.BL.APIDataContract.ApiV1
{
   
    public partial class GLAccount
    {

	    
	[XmlAttribute]
    public string Id { get; set; }
    
    public int Tenant { get; set; }
    
    public string InternalNumber { get; set; }
    
    public GLAccountType GLAccountType { get; set; }
    
    public string DisplayNumber { get; set; }
    
    public string LocalName { get; set; }
    
    public string EnglishName { get; set; }
    
    public bool? IsMultiCurrency { get; set; }
    
    public Currency Currency { get; set; }
    
    public ChartOfAccount ChartOfAccount { get; set; }
    
    public bool? Inactive { get; set; }
    
    public string AccountTypeName { get; set; }
    
    public string CurrencyName { get; set; }
    
    public string ChartOfAccountsName { get; set; }
    
    public ChartOfAccountsType ChartOfAccountsType { get; set; }
    
    public string ChartOfAccountsTypeName { get; set; }
    
    public string CurrencyCode { get; set; }
    
    public ReconcileMethod ReconcileMethod { get; set; }
    
    public string ReconcileMethodName { get; set; }
    
    public GLAccount ControlAccount { get; set; }
    
    public string ControlAccountName { get; set; }
    
    public string ControlAccountNumber { get; set; }
    
    public string ActiveStatusName { get; set; }
    
    public string OldCurrencyId { get; set; }
    
    public bool OldIsMultiCurrency { get; set; }
    
    public AutomaticReconcileMethod AutomaticReconcileMethod { get; set; }
    
    public string AutomaticReconcileName { get; set; }
    
    public string PreviousEnglishName { get; set; }
    
    public string PreviousLocalName { get; set; }
    
    public string PreviousNumber { get; set; }
    
    public ChartOfAccount PreviousChartOfAccount { get; set; }
    
    public GLAccount CustomerGLAccount { get; set; }
    
    public string CustomerGLAccountName { get; set; }
    
    public string CustomerGLAccountNumber { get; set; }
    
    public decimal? BalanceInLocalCurrency { get; set; }
    
    public bool? RevaluationEnabled { get; set; }
    
    public GLAccount ParentAccount { get; set; }
    
    public string ParentAccountName { get; set; }
    
    public string ParentAccountNumber { get; set; }
    
    public string CustomerGLAccountInternalNumber { get; set; }
    
    public Category1 Category1 { get; set; }
    
    public string Category1Name { get; set; }
    
    public Category2 Category2 { get; set; }
    
    public string Category2Name { get; set; }
    
    public Category3 Category3 { get; set; }
    
    public string Category3Name { get; set; }
    
    public Category4 Category4 { get; set; }
    
    public string Category4Name { get; set; }
    
    public Category5 Category5 { get; set; }
    
    public string Category5Name { get; set; }
    
    public bool? IsVATExempt { get; set; }
    
    public string ChartOfAccountsCode { get; set; }
    
    public string CustomerCode { get; set; }
    
    public string ParentAccountByCurrency { get; set; }
    
    public string VatNumber { get; set; }
    
    public string PaymentTermId { get; set; }
    
    public User Collector { get; set; }
    
    public string SalesmanUserId { get; set; }
    
    public string NewGLAccountCardId { get; set; }
    
    public decimal? LocalBalanceInDue { get; set; }
    
    public DateTime? NextDueDate { get; set; }
    
    public string CurrencySign { get; set; }
    
    public string ConnectedItems { get; set; }
    
    public string Type { get; set; }
    
    public string DeductionFileTypeId { get; set; }
    
    public string DeductionFileNumber { get; set; }
    
    public string AssessingOfficeCode { get; set; }
    
    public string Occupation { get; set; }
    
    public string DeductionTypeId { get; set; }
    
    public string ConsolidationVat { get; set; }
    
    public RevenueExpenseType RevenueExpenseType { get; set; }
    
    public string Parent { get; set; }
    
    public string CardCode { get; set; }
    
    public string PartnerTypeId { get; set; }
    }
} 