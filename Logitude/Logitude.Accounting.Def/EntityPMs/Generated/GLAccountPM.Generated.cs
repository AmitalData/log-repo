using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server; 
using Logitude.Server.Tools; 
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure.DataContracts; 
using Logitude.Accounting.Def.Validators;
  
namespace Logitude.Accounting.Def.EntityPMs
{
   [CustomValidation(typeof(AccountingClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class GLAccountPM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Id  
	   {
	    
	     get
		{
		   return id;
		 }
		 set
		 {
		   if(id != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Id",OldValue=id,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   id=value;
		   }
			
		 }
	   }
	  private int tenant ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public int Tenant  
	   {
	    
	     get
		{
		   return tenant;
		 }
		 set
		 {
		   if(tenant != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Tenant",OldValue=tenant,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   tenant=value;
		   }
			
		 }
	   }
	  private string internalNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string InternalNumber  
	   {
	    
	     get
		{
		   return internalNumber;
		 }
		 set
		 {
		   if(internalNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InternalNumber",OldValue=internalNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   internalNumber=value;
		   }
			
		 }
	   }
	  private string accountTypeCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountTypeCode  
	   {
	    
	     get
		{
		   return accountTypeCode;
		 }
		 set
		 {
		   if(accountTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountTypeCode",OldValue=accountTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountTypeCode=value;
		   }
			
		 }
	   }
	  private string displayNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DisplayNumber  
	   {
	    
	     get
		{
		   return displayNumber;
		 }
		 set
		 {
		   if(displayNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DisplayNumber",OldValue=displayNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   displayNumber=value;
		   }
			
		 }
	   }
	  private string localName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string LocalName  
	   {
	    
	     get
		{
		   return localName;
		 }
		 set
		 {
		   if(localName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LocalName",OldValue=localName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   localName=value;
		   }
			
		 }
	   }
	  private string englishName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string EnglishName  
	   {
	    
	     get
		{
		   return englishName;
		 }
		 set
		 {
		   if(englishName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EnglishName",OldValue=englishName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   englishName=value;
		   }
			
		 }
	   }
	  private string searchFields ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string SearchFields  
	   {
	    
	     get
		{
		   return searchFields;
		 }
		 set
		 {
		   if(searchFields != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SearchFields",OldValue=searchFields,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   searchFields=value;
		   }
			
		 }
	   }
	  private bool? isMultiCurrency ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool? IsMultiCurrency  
	   {
	    
	     get
		{
		   return isMultiCurrency;
		 }
		 set
		 {
		   if(isMultiCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsMultiCurrency",OldValue=isMultiCurrency,NewValue=value,PropertyType="bool?"};
		    NotifyPropertyChanged(values);
		   isMultiCurrency=value;
		   }
			
		 }
	   }
	  private string currencyId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CurrencyId  
	   {
	    
	     get
		{
		   return currencyId;
		 }
		 set
		 {
		   if(currencyId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CurrencyId",OldValue=currencyId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   currencyId=value;
		   }
			
		 }
	   }
	  private string revenueExpenseType ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string RevenueExpenseType  
	   {
	    
	     get
		{
		   return revenueExpenseType;
		 }
		 set
		 {
		   if(revenueExpenseType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RevenueExpenseType",OldValue=revenueExpenseType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   revenueExpenseType=value;
		   }
			
		 }
	   }
	  private bool? isControlAccount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool? IsControlAccount  
	   {
	    
	     get
		{
		   return isControlAccount;
		 }
		 set
		 {
		   if(isControlAccount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsControlAccount",OldValue=isControlAccount,NewValue=value,PropertyType="bool?"};
		    NotifyPropertyChanged(values);
		   isControlAccount=value;
		   }
			
		 }
	   }
	  private string chartOfAccountsId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ChartOfAccountsId  
	   {
	    
	     get
		{
		   return chartOfAccountsId;
		 }
		 set
		 {
		   if(chartOfAccountsId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChartOfAccountsId",OldValue=chartOfAccountsId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   chartOfAccountsId=value;
		   }
			
		 }
	   }
	  private bool? inactive ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool? Inactive  
	   {
	    
	     get
		{
		   return inactive;
		 }
		 set
		 {
		   if(inactive != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Inactive",OldValue=inactive,NewValue=value,PropertyType="bool?"};
		    NotifyPropertyChanged(values);
		   inactive=value;
		   }
			
		 }
	   }
	  private string accountTypeName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountTypeName  
	   {
	    
	     get
		{
		   return accountTypeName;
		 }
		 set
		 {
		   if(accountTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountTypeName",OldValue=accountTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountTypeName=value;
		   }
			
		 }
	   }
	  private string currencyName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CurrencyName  
	   {
	    
	     get
		{
		   return currencyName;
		 }
		 set
		 {
		   if(currencyName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CurrencyName",OldValue=currencyName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   currencyName=value;
		   }
			
		 }
	   }
	  private string revenueExpenseName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string RevenueExpenseName  
	   {
	    
	     get
		{
		   return revenueExpenseName;
		 }
		 set
		 {
		   if(revenueExpenseName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RevenueExpenseName",OldValue=revenueExpenseName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   revenueExpenseName=value;
		   }
			
		 }
	   }
	  private string chartOfAccountsName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ChartOfAccountsName  
	   {
	    
	     get
		{
		   return chartOfAccountsName;
		 }
		 set
		 {
		   if(chartOfAccountsName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChartOfAccountsName",OldValue=chartOfAccountsName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   chartOfAccountsName=value;
		   }
			
		 }
	   }
	  private string chartOfAccountsTypeCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ChartOfAccountsTypeCode  
	   {
	    
	     get
		{
		   return chartOfAccountsTypeCode;
		 }
		 set
		 {
		   if(chartOfAccountsTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChartOfAccountsTypeCode",OldValue=chartOfAccountsTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   chartOfAccountsTypeCode=value;
		   }
			
		 }
	   }
	  private string chartOfAccountsTypeName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ChartOfAccountsTypeName  
	   {
	    
	     get
		{
		   return chartOfAccountsTypeName;
		 }
		 set
		 {
		   if(chartOfAccountsTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChartOfAccountsTypeName",OldValue=chartOfAccountsTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   chartOfAccountsTypeName=value;
		   }
			
		 }
	   }
	  private string currencyCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CurrencyCode  
	   {
	    
	     get
		{
		   return currencyCode;
		 }
		 set
		 {
		   if(currencyCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CurrencyCode",OldValue=currencyCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   currencyCode=value;
		   }
			
		 }
	   }
	  private string reconcileMethodCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ReconcileMethodCode  
	   {
	    
	     get
		{
		   return reconcileMethodCode;
		 }
		 set
		 {
		   if(reconcileMethodCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReconcileMethodCode",OldValue=reconcileMethodCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   reconcileMethodCode=value;
		   }
			
		 }
	   }
	  private string reconcileMethodName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ReconcileMethodName  
	   {
	    
	     get
		{
		   return reconcileMethodName;
		 }
		 set
		 {
		   if(reconcileMethodName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReconcileMethodName",OldValue=reconcileMethodName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   reconcileMethodName=value;
		   }
			
		 }
	   }
	  private string controlAccountId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ControlAccountId  
	   {
	    
	     get
		{
		   return controlAccountId;
		 }
		 set
		 {
		   if(controlAccountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ControlAccountId",OldValue=controlAccountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   controlAccountId=value;
		   }
			
		 }
	   }
	  private string controlAccountName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ControlAccountName  
	   {
	    
	     get
		{
		   return controlAccountName;
		 }
		 set
		 {
		   if(controlAccountName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ControlAccountName",OldValue=controlAccountName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   controlAccountName=value;
		   }
			
		 }
	   }
	  private string controlAccountNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ControlAccountNumber  
	   {
	    
	     get
		{
		   return controlAccountNumber;
		 }
		 set
		 {
		   if(controlAccountNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ControlAccountNumber",OldValue=controlAccountNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   controlAccountNumber=value;
		   }
			
		 }
	   }
	  private string activeStatusName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ActiveStatusName  
	   {
	    
	     get
		{
		   return activeStatusName;
		 }
		 set
		 {
		   if(activeStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ActiveStatusName",OldValue=activeStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   activeStatusName=value;
		   }
			
		 }
	   }
	  private string oldCurrencyId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string OldCurrencyId  
	   {
	    
	     get
		{
		   return oldCurrencyId;
		 }
		 set
		 {
		   if(oldCurrencyId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OldCurrencyId",OldValue=oldCurrencyId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   oldCurrencyId=value;
		   }
			
		 }
	   }
	  private bool oldIsMultiCurrency ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool OldIsMultiCurrency  
	   {
	    
	     get
		{
		   return oldIsMultiCurrency;
		 }
		 set
		 {
		   if(oldIsMultiCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OldIsMultiCurrency",OldValue=oldIsMultiCurrency,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   oldIsMultiCurrency=value;
		   }
			
		 }
	   }
	  private string automaticReconcileId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AutomaticReconcileId  
	   {
	    
	     get
		{
		   return automaticReconcileId;
		 }
		 set
		 {
		   if(automaticReconcileId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AutomaticReconcileId",OldValue=automaticReconcileId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   automaticReconcileId=value;
		   }
			
		 }
	   }
	  private string automaticReconcileName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AutomaticReconcileName  
	   {
	    
	     get
		{
		   return automaticReconcileName;
		 }
		 set
		 {
		   if(automaticReconcileName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AutomaticReconcileName",OldValue=automaticReconcileName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   automaticReconcileName=value;
		   }
			
		 }
	   }
	  private string previousEnglishName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string PreviousEnglishName  
	   {
	    
	     get
		{
		   return previousEnglishName;
		 }
		 set
		 {
		   if(previousEnglishName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PreviousEnglishName",OldValue=previousEnglishName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   previousEnglishName=value;
		   }
			
		 }
	   }
	  private DateTime? previousEnglishNameChangeDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? PreviousEnglishNameChangeDate  
	   {
	    
	     get
		{
		   return previousEnglishNameChangeDate;
		 }
		 set
		 {
		   if(previousEnglishNameChangeDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PreviousEnglishNameChangeDate",OldValue=previousEnglishNameChangeDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   previousEnglishNameChangeDate=value;
		   }
			
		 }
	   }
	  private string previousLocalName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string PreviousLocalName  
	   {
	    
	     get
		{
		   return previousLocalName;
		 }
		 set
		 {
		   if(previousLocalName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PreviousLocalName",OldValue=previousLocalName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   previousLocalName=value;
		   }
			
		 }
	   }
	  private DateTime? previousLocalNameChangeDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? PreviousLocalNameChangeDate  
	   {
	    
	     get
		{
		   return previousLocalNameChangeDate;
		 }
		 set
		 {
		   if(previousLocalNameChangeDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PreviousLocalNameChangeDate",OldValue=previousLocalNameChangeDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   previousLocalNameChangeDate=value;
		   }
			
		 }
	   }
	  private string previousNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string PreviousNumber  
	   {
	    
	     get
		{
		   return previousNumber;
		 }
		 set
		 {
		   if(previousNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PreviousNumber",OldValue=previousNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   previousNumber=value;
		   }
			
		 }
	   }
	  private DateTime? previousNumberChangeDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? PreviousNumberChangeDate  
	   {
	    
	     get
		{
		   return previousNumberChangeDate;
		 }
		 set
		 {
		   if(previousNumberChangeDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PreviousNumberChangeDate",OldValue=previousNumberChangeDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   previousNumberChangeDate=value;
		   }
			
		 }
	   }
	  private string previousChartOfAccountsId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string PreviousChartOfAccountsId  
	   {
	    
	     get
		{
		   return previousChartOfAccountsId;
		 }
		 set
		 {
		   if(previousChartOfAccountsId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PreviousChartOfAccountsId",OldValue=previousChartOfAccountsId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   previousChartOfAccountsId=value;
		   }
			
		 }
	   }
	  private DateTime? previousChartOfAccountsChangeDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? PreviousChartOfAccountsChangeDate  
	   {
	    
	     get
		{
		   return previousChartOfAccountsChangeDate;
		 }
		 set
		 {
		   if(previousChartOfAccountsChangeDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PreviousChartOfAccountsChangeDate",OldValue=previousChartOfAccountsChangeDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   previousChartOfAccountsChangeDate=value;
		   }
			
		 }
	   }
	  private string customerGLAccountId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerGLAccountId  
	   {
	    
	     get
		{
		   return customerGLAccountId;
		 }
		 set
		 {
		   if(customerGLAccountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerGLAccountId",OldValue=customerGLAccountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerGLAccountId=value;
		   }
			
		 }
	   }
	  private string customerGLAccountName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerGLAccountName  
	   {
	    
	     get
		{
		   return customerGLAccountName;
		 }
		 set
		 {
		   if(customerGLAccountName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerGLAccountName",OldValue=customerGLAccountName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerGLAccountName=value;
		   }
			
		 }
	   }
	  private string customerGLAccountNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerGLAccountNumber  
	   {
	    
	     get
		{
		   return customerGLAccountNumber;
		 }
		 set
		 {
		   if(customerGLAccountNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerGLAccountNumber",OldValue=customerGLAccountNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerGLAccountNumber=value;
		   }
			
		 }
	   }
	  private decimal? balanceInLocalCurrency ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? BalanceInLocalCurrency  
	   {
	    
	     get
		{
		   return balanceInLocalCurrency;
		 }
		 set
		 {
		   if(balanceInLocalCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BalanceInLocalCurrency",OldValue=balanceInLocalCurrency,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   balanceInLocalCurrency=value;
		   }
			
		 }
	   }
	  private bool? revaluationEnabled ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool? RevaluationEnabled  
	   {
	    
	     get
		{
		   return revaluationEnabled;
		 }
		 set
		 {
		   if(revaluationEnabled != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RevaluationEnabled",OldValue=revaluationEnabled,NewValue=value,PropertyType="bool?"};
		    NotifyPropertyChanged(values);
		   revaluationEnabled=value;
		   }
			
		 }
	   }
	  private string parentAccountId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ParentAccountId  
	   {
	    
	     get
		{
		   return parentAccountId;
		 }
		 set
		 {
		   if(parentAccountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ParentAccountId",OldValue=parentAccountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   parentAccountId=value;
		   }
			
		 }
	   }
	  private string parentAccountName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ParentAccountName  
	   {
	    
	     get
		{
		   return parentAccountName;
		 }
		 set
		 {
		   if(parentAccountName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ParentAccountName",OldValue=parentAccountName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   parentAccountName=value;
		   }
			
		 }
	   }
	  private string parentAccountNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ParentAccountNumber  
	   {
	    
	     get
		{
		   return parentAccountNumber;
		 }
		 set
		 {
		   if(parentAccountNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ParentAccountNumber",OldValue=parentAccountNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   parentAccountNumber=value;
		   }
			
		 }
	   }
	  private string customerGLAccountInternalNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerGLAccountInternalNumber  
	   {
	    
	     get
		{
		   return customerGLAccountInternalNumber;
		 }
		 set
		 {
		   if(customerGLAccountInternalNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerGLAccountInternalNumber",OldValue=customerGLAccountInternalNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerGLAccountInternalNumber=value;
		   }
			
		 }
	   }
	  private string category1Id ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Category1Id  
	   {
	    
	     get
		{
		   return category1Id;
		 }
		 set
		 {
		   if(category1Id != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Category1Id",OldValue=category1Id,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   category1Id=value;
		   }
			
		 }
	   }
	  private string category1Name ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Category1Name  
	   {
	    
	     get
		{
		   return category1Name;
		 }
		 set
		 {
		   if(category1Name != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Category1Name",OldValue=category1Name,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   category1Name=value;
		   }
			
		 }
	   }
	  private string category2Id ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Category2Id  
	   {
	    
	     get
		{
		   return category2Id;
		 }
		 set
		 {
		   if(category2Id != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Category2Id",OldValue=category2Id,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   category2Id=value;
		   }
			
		 }
	   }
	  private string category2Name ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Category2Name  
	   {
	    
	     get
		{
		   return category2Name;
		 }
		 set
		 {
		   if(category2Name != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Category2Name",OldValue=category2Name,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   category2Name=value;
		   }
			
		 }
	   }
	  private string category3Id ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Category3Id  
	   {
	    
	     get
		{
		   return category3Id;
		 }
		 set
		 {
		   if(category3Id != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Category3Id",OldValue=category3Id,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   category3Id=value;
		   }
			
		 }
	   }
	  private string category3Name ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Category3Name  
	   {
	    
	     get
		{
		   return category3Name;
		 }
		 set
		 {
		   if(category3Name != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Category3Name",OldValue=category3Name,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   category3Name=value;
		   }
			
		 }
	   }
	  private string category4Id ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Category4Id  
	   {
	    
	     get
		{
		   return category4Id;
		 }
		 set
		 {
		   if(category4Id != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Category4Id",OldValue=category4Id,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   category4Id=value;
		   }
			
		 }
	   }
	  private string category4Name ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Category4Name  
	   {
	    
	     get
		{
		   return category4Name;
		 }
		 set
		 {
		   if(category4Name != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Category4Name",OldValue=category4Name,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   category4Name=value;
		   }
			
		 }
	   }
	  private string category5Id ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Category5Id  
	   {
	    
	     get
		{
		   return category5Id;
		 }
		 set
		 {
		   if(category5Id != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Category5Id",OldValue=category5Id,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   category5Id=value;
		   }
			
		 }
	   }
	  private string category5Name ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Category5Name  
	   {
	    
	     get
		{
		   return category5Name;
		 }
		 set
		 {
		   if(category5Name != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Category5Name",OldValue=category5Name,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   category5Name=value;
		   }
			
		 }
	   }
	  private bool? isVATExempt ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool? IsVATExempt  
	   {
	    
	     get
		{
		   return isVATExempt;
		 }
		 set
		 {
		   if(isVATExempt != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsVATExempt",OldValue=isVATExempt,NewValue=value,PropertyType="bool?"};
		    NotifyPropertyChanged(values);
		   isVATExempt=value;
		   }
			
		 }
	   }
	  private string chartOfAccountsCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ChartOfAccountsCode  
	   {
	    
	     get
		{
		   return chartOfAccountsCode;
		 }
		 set
		 {
		   if(chartOfAccountsCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChartOfAccountsCode",OldValue=chartOfAccountsCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   chartOfAccountsCode=value;
		   }
			
		 }
	   }
	  private string customerCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerCode  
	   {
	    
	     get
		{
		   return customerCode;
		 }
		 set
		 {
		   if(customerCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerCode",OldValue=customerCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerCode=value;
		   }
			
		 }
	   }
	  private string parentAccountByCurrency ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ParentAccountByCurrency  
	   {
	    
	     get
		{
		   return parentAccountByCurrency;
		 }
		 set
		 {
		   if(parentAccountByCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ParentAccountByCurrency",OldValue=parentAccountByCurrency,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   parentAccountByCurrency=value;
		   }
			
		 }
	   }
	  private string vatNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string VatNumber  
	   {
	    
	     get
		{
		   return vatNumber;
		 }
		 set
		 {
		   if(vatNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VatNumber",OldValue=vatNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vatNumber=value;
		   }
			
		 }
	   }
	  private string paymentTermId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string PaymentTermId  
	   {
	    
	     get
		{
		   return paymentTermId;
		 }
		 set
		 {
		   if(paymentTermId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PaymentTermId",OldValue=paymentTermId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   paymentTermId=value;
		   }
			
		 }
	   }
	  private string collectorId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CollectorId  
	   {
	    
	     get
		{
		   return collectorId;
		 }
		 set
		 {
		   if(collectorId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CollectorId",OldValue=collectorId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   collectorId=value;
		   }
			
		 }
	   }
	  private string salesmanUserId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string SalesmanUserId  
	   {
	    
	     get
		{
		   return salesmanUserId;
		 }
		 set
		 {
		   if(salesmanUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SalesmanUserId",OldValue=salesmanUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   salesmanUserId=value;
		   }
			
		 }
	   }
	  private string newGLAccountCardId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string NewGLAccountCardId  
	   {
	    
	     get
		{
		   return newGLAccountCardId;
		 }
		 set
		 {
		   if(newGLAccountCardId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NewGLAccountCardId",OldValue=newGLAccountCardId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   newGLAccountCardId=value;
		   }
			
		 }
	   }
	  private decimal? localBalanceInDue ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? LocalBalanceInDue  
	   {
	    
	     get
		{
		   return localBalanceInDue;
		 }
		 set
		 {
		   if(localBalanceInDue != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LocalBalanceInDue",OldValue=localBalanceInDue,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   localBalanceInDue=value;
		   }
			
		 }
	   }
	  private DateTime? nextDueDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? NextDueDate  
	   {
	    
	     get
		{
		   return nextDueDate;
		 }
		 set
		 {
		   if(nextDueDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NextDueDate",OldValue=nextDueDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   nextDueDate=value;
		   }
			
		 }
	   }
	  private string currencySign ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CurrencySign  
	   {
	    
	     get
		{
		   return currencySign;
		 }
		 set
		 {
		   if(currencySign != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CurrencySign",OldValue=currencySign,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   currencySign=value;
		   }
			
		 }
	   }
	  private string connectedItems ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConnectedItems  
	   {
	    
	     get
		{
		   return connectedItems;
		 }
		 set
		 {
		   if(connectedItems != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConnectedItems",OldValue=connectedItems,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   connectedItems=value;
		   }
			
		 }
	   }
	  private string type ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Type  
	   {
	    
	     get
		{
		   return type;
		 }
		 set
		 {
		   if(type != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Type",OldValue=type,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   type=value;
		   }
			
		 }
	   }
	  private string deductionFileTypeId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeductionFileTypeId  
	   {
	    
	     get
		{
		   return deductionFileTypeId;
		 }
		 set
		 {
		   if(deductionFileTypeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeductionFileTypeId",OldValue=deductionFileTypeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   deductionFileTypeId=value;
		   }
			
		 }
	   }
	  private string deductionFileNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeductionFileNumber  
	   {
	    
	     get
		{
		   return deductionFileNumber;
		 }
		 set
		 {
		   if(deductionFileNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeductionFileNumber",OldValue=deductionFileNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   deductionFileNumber=value;
		   }
			
		 }
	   }
	  private string assessingOfficeCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AssessingOfficeCode  
	   {
	    
	     get
		{
		   return assessingOfficeCode;
		 }
		 set
		 {
		   if(assessingOfficeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AssessingOfficeCode",OldValue=assessingOfficeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   assessingOfficeCode=value;
		   }
			
		 }
	   }
	  private string occupation ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Occupation  
	   {
	    
	     get
		{
		   return occupation;
		 }
		 set
		 {
		   if(occupation != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Occupation",OldValue=occupation,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   occupation=value;
		   }
			
		 }
	   }
	  private string deductionTypeId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeductionTypeId  
	   {
	    
	     get
		{
		   return deductionTypeId;
		 }
		 set
		 {
		   if(deductionTypeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeductionTypeId",OldValue=deductionTypeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   deductionTypeId=value;
		   }
			
		 }
	   }
	  private string consolidationVat ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsolidationVat  
	   {
	    
	     get
		{
		   return consolidationVat;
		 }
		 set
		 {
		   if(consolidationVat != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsolidationVat",OldValue=consolidationVat,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consolidationVat=value;
		   }
			
		 }
	   }

	   private List<GLAccountWithholdingTaxPM> gLAccountWithholdingTaxes;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("GLAccountGLAccountWithholdingTaxes", "Id","GLAccountId")]
	   [DataMember]
	   public virtual List<GLAccountWithholdingTaxPM> GLAccountWithholdingTaxes  
	   {
	        get
             {
                 if (gLAccountWithholdingTaxes == null)
                 {
                     gLAccountWithholdingTaxes = new List<GLAccountWithholdingTaxPM>();
                 }
                 return gLAccountWithholdingTaxes;
              }
             set { gLAccountWithholdingTaxes = value; }
	    }
		   
	   private List<GLAccountWithholdingTaxPM>  deletedGLAccountWithholdingTaxes;
	   public virtual List<GLAccountWithholdingTaxPM> DeletedGLAccountWithholdingTaxes  
	   {
	        get
             {
                 if ( deletedGLAccountWithholdingTaxes == null)
                 {
                      deletedGLAccountWithholdingTaxes = new List<GLAccountWithholdingTaxPM>();
                 }
                 return  deletedGLAccountWithholdingTaxes;
              }
             set {  deletedGLAccountWithholdingTaxes = value; }
	    }
	  	  private int taxWithholdingLastLine ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public int TaxWithholdingLastLine  
	   {
	    
	     get
		{
		   return taxWithholdingLastLine;
		 }
		 set
		 {
		   if(taxWithholdingLastLine != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TaxWithholdingLastLine",OldValue=taxWithholdingLastLine,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   taxWithholdingLastLine=value;
		   }
			
		 }
	   }
	  private int? reconcilationCount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public int? ReconcilationCount  
	   {
	    
	     get
		{
		   return reconcilationCount;
		 }
		 set
		 {
		   if(reconcilationCount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReconcilationCount",OldValue=reconcilationCount,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   reconcilationCount=value;
		   }
			
		 }
	   }
	  private bool isEquipmentVendor ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsEquipmentVendor  
	   {
	    
	     get
		{
		   return isEquipmentVendor;
		 }
		 set
		 {
		   if(isEquipmentVendor != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsEquipmentVendor",OldValue=isEquipmentVendor,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isEquipmentVendor=value;
		   }
			
		 }
	   }
	  private bool excludeFromDeductionReport ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ExcludeFromDeductionReport  
	   {
	    
	     get
		{
		   return excludeFromDeductionReport;
		 }
		 set
		 {
		   if(excludeFromDeductionReport != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExcludeFromDeductionReport",OldValue=excludeFromDeductionReport,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   excludeFromDeductionReport=value;
		   }
			
		 }
	   }
	  private string parent ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Parent  
	   {
	    
	     get
		{
		   return parent;
		 }
		 set
		 {
		   if(parent != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Parent",OldValue=parent,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   parent=value;
		   }
			
		 }
	   }
	  private string deductionTypeName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeductionTypeName  
	   {
	    
	     get
		{
		   return deductionTypeName;
		 }
		 set
		 {
		   if(deductionTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeductionTypeName",OldValue=deductionTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   deductionTypeName=value;
		   }
			
		 }
	   }
	  private string deductionFileTypeCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeductionFileTypeCode  
	   {
	    
	     get
		{
		   return deductionFileTypeCode;
		 }
		 set
		 {
		   if(deductionFileTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeductionFileTypeCode",OldValue=deductionFileTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   deductionFileTypeCode=value;
		   }
			
		 }
	   }
	  private string deductionFileTypeName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeductionFileTypeName  
	   {
	    
	     get
		{
		   return deductionFileTypeName;
		 }
		 set
		 {
		   if(deductionFileTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeductionFileTypeName",OldValue=deductionFileTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   deductionFileTypeName=value;
		   }
			
		 }
	   }
	  private string assessingOfficeName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AssessingOfficeName  
	   {
	    
	     get
		{
		   return assessingOfficeName;
		 }
		 set
		 {
		   if(assessingOfficeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AssessingOfficeName",OldValue=assessingOfficeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   assessingOfficeName=value;
		   }
			
		 }
	   }
	  private string deductionTypeEnglishName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeductionTypeEnglishName  
	   {
	    
	     get
		{
		   return deductionTypeEnglishName;
		 }
		 set
		 {
		   if(deductionTypeEnglishName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeductionTypeEnglishName",OldValue=deductionTypeEnglishName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   deductionTypeEnglishName=value;
		   }
			
		 }
	   }
	  private decimal? totalOpenChequesInLocalCur ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? TotalOpenChequesInLocalCur  
	   {
	    
	     get
		{
		   return totalOpenChequesInLocalCur;
		 }
		 set
		 {
		   if(totalOpenChequesInLocalCur != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TotalOpenChequesInLocalCur",OldValue=totalOpenChequesInLocalCur,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   totalOpenChequesInLocalCur=value;
		   }
			
		 }
	   }
	  private decimal? totFutureOpenChequesInLocalCur ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? TotFutureOpenChequesInLocalCur  
	   {
	    
	     get
		{
		   return totFutureOpenChequesInLocalCur;
		 }
		 set
		 {
		   if(totFutureOpenChequesInLocalCur != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TotFutureOpenChequesInLocalCur",OldValue=totFutureOpenChequesInLocalCur,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   totFutureOpenChequesInLocalCur=value;
		   }
			
		 }
	   }
	  private string cardId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CardId  
	   {
	    
	     get
		{
		   return cardId;
		 }
		 set
		 {
		   if(cardId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CardId",OldValue=cardId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cardId=value;
		   }
			
		 }
	   }
	  private string createdByUserId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreatedByUserId  
	   {
	    
	     get
		{
		   return createdByUserId;
		 }
		 set
		 {
		   if(createdByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreatedByUserId",OldValue=createdByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   createdByUserId=value;
		   }
			
		 }
	   }
	  private string updatedByUserId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string UpdatedByUserId  
	   {
	    
	     get
		{
		   return updatedByUserId;
		 }
		 set
		 {
		   if(updatedByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdatedByUserId",OldValue=updatedByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   updatedByUserId=value;
		   }
			
		 }
	   }
	  private DateTime createDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime CreateDate  
	   {
	    
	     get
		{
		   return createDate;
		 }
		 set
		 {
		   if(createDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreateDate",OldValue=createDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   createDate=value;
		   }
			
		 }
	   }
	  private DateTime updateDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime UpdateDate  
	   {
	    
	     get
		{
		   return updateDate;
		 }
		 set
		 {
		   if(updateDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdateDate",OldValue=updateDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   updateDate=value;
		   }
			
		 }
	   }
	  private string createdByUserName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreatedByUserName  
	   {
	    
	     get
		{
		   return createdByUserName;
		 }
		 set
		 {
		   if(createdByUserName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreatedByUserName",OldValue=createdByUserName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   createdByUserName=value;
		   }
			
		 }
	   }
	  private string updatedByUserName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string UpdatedByUserName  
	   {
	    
	     get
		{
		   return updatedByUserName;
		 }
		 set
		 {
		   if(updatedByUserName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdatedByUserName",OldValue=updatedByUserName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   updatedByUserName=value;
		   }
			
		 }
	   }
	  private string updatedByLocalName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string UpdatedByLocalName  
	   {
	    
	     get
		{
		   return updatedByLocalName;
		 }
		 set
		 {
		   if(updatedByLocalName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdatedByLocalName",OldValue=updatedByLocalName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   updatedByLocalName=value;
		   }
			
		 }
	   }
	  private string cardCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CardCode  
	   {
	    
	     get
		{
		   return cardCode;
		 }
		 set
		 {
		   if(cardCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CardCode",OldValue=cardCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cardCode=value;
		   }
			
		 }
	   }
	  private string partnerTypeId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string PartnerTypeId  
	   {
	    
	     get
		{
		   return partnerTypeId;
		 }
		 set
		 {
		   if(partnerTypeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PartnerTypeId",OldValue=partnerTypeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   partnerTypeId=value;
		   }
			
		 }
	   }
	  private bool allowEditChequePayToName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool AllowEditChequePayToName  
	   {
	    
	     get
		{
		   return allowEditChequePayToName;
		 }
		 set
		 {
		   if(allowEditChequePayToName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AllowEditChequePayToName",OldValue=allowEditChequePayToName,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   allowEditChequePayToName=value;
		   }
			
		 }
	   }
	  private bool activeForInterest ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ActiveForInterest  
	   {
	    
	     get
		{
		   return activeForInterest;
		 }
		 set
		 {
		   if(activeForInterest != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ActiveForInterest",OldValue=activeForInterest,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   activeForInterest=value;
		   }
			
		 }
	   }
	  private DateTime? interestCalculationStartDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? InterestCalculationStartDate  
	   {
	    
	     get
		{
		   return interestCalculationStartDate;
		 }
		 set
		 {
		   if(interestCalculationStartDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InterestCalculationStartDate",OldValue=interestCalculationStartDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   interestCalculationStartDate=value;
		   }
			
		 }
	   }
	  private bool activeForInterestCreditInvoice ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ActiveForInterestCreditInvoice  
	   {
	    
	     get
		{
		   return activeForInterestCreditInvoice;
		 }
		 set
		 {
		   if(activeForInterestCreditInvoice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ActiveForInterestCreditInvoice",OldValue=activeForInterestCreditInvoice,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   activeForInterestCreditInvoice=value;
		   }
			
		 }
	   }

	   private List<GLAccountInterestPeriodPM> gLAccountInterestPeriods;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("GLAccountInterestPeriodGLAccount", "Id","GLAccountId")]
	   [DataMember]
	   public virtual List<GLAccountInterestPeriodPM> GLAccountInterestPeriods  
	   {
	        get
             {
                 if (gLAccountInterestPeriods == null)
                 {
                     gLAccountInterestPeriods = new List<GLAccountInterestPeriodPM>();
                 }
                 return gLAccountInterestPeriods;
              }
             set { gLAccountInterestPeriods = value; }
	    }
		   
	   private List<GLAccountInterestPeriodPM>  deletedGLAccountInterestPeriods;
	   public virtual List<GLAccountInterestPeriodPM> DeletedGLAccountInterestPeriods  
	   {
	        get
             {
                 if ( deletedGLAccountInterestPeriods == null)
                 {
                      deletedGLAccountInterestPeriods = new List<GLAccountInterestPeriodPM>();
                 }
                 return  deletedGLAccountInterestPeriods;
              }
             set {  deletedGLAccountInterestPeriods = value; }
	    }
	  	  private decimal? interestCreditLimit ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? InterestCreditLimit  
	   {
	    
	     get
		{
		   return interestCreditLimit;
		 }
		 set
		 {
		   if(interestCreditLimit != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InterestCreditLimit",OldValue=interestCreditLimit,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   interestCreditLimit=value;
		   }
			
		 }
	   }
	  private string nameForPrintingCheques ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string NameForPrintingCheques  
	   {
	    
	     get
		{
		   return nameForPrintingCheques;
		 }
		 set
		 {
		   if(nameForPrintingCheques != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NameForPrintingCheques",OldValue=nameForPrintingCheques,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   nameForPrintingCheques=value;
		   }
			
		 }
	   }
	  private bool smallcashbook ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool Smallcashbook  
	   {
	    
	     get
		{
		   return smallcashbook;
		 }
		 set
		 {
		   if(smallcashbook != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Smallcashbook",OldValue=smallcashbook,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   smallcashbook=value;
		   }
			
		 }
	   }
	  private int? minimumInterestInvoiceBilling ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public int? MinimumInterestInvoiceBilling  
	   {
	    
	     get
		{
		   return minimumInterestInvoiceBilling;
		 }
		 set
		 {
		   if(minimumInterestInvoiceBilling != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MinimumInterestInvoiceBilling",OldValue=minimumInterestInvoiceBilling,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   minimumInterestInvoiceBilling=value;
		   }
			
		 }
	   }
	  private bool isSplitted ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsSplitted  
	   {
	    
	     get
		{
		   return isSplitted;
		 }
		 set
		 {
		   if(isSplitted != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsSplitted",OldValue=isSplitted,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isSplitted=value;
		   }
			
		 }
	   }
	  private string salesmanName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string SalesmanName  
	   {
	    
	     get
		{
		   return salesmanName;
		 }
		 set
		 {
		   if(salesmanName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SalesmanName",OldValue=salesmanName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   salesmanName=value;
		   }
			
		 }
	   }
	  private string collectorName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CollectorName  
	   {
	    
	     get
		{
		   return collectorName;
		 }
		 set
		 {
		   if(collectorName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CollectorName",OldValue=collectorName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   collectorName=value;
		   }
			
		 }
	   }
	  private string splitCurrencyAccount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string SplitCurrencyAccount  
	   {
	    
	     get
		{
		   return splitCurrencyAccount;
		 }
		 set
		 {
		   if(splitCurrencyAccount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SplitCurrencyAccount",OldValue=splitCurrencyAccount,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   splitCurrencyAccount=value;
		   }
			
		 }
	   }
	  private string parentName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ParentName  
	   {
	    
	     get
		{
		   return parentName;
		 }
		 set
		 {
		   if(parentName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ParentName",OldValue=parentName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   parentName=value;
		   }
			
		 }
	   }

	   private List<GLAccountCurrencyPM> gLAccountCurrencies;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("GLAccountCurrencyGLAccount", "Id","MainGLAccountId")]
	   [DataMember]
	   public virtual List<GLAccountCurrencyPM> GLAccountCurrencies  
	   {
	        get
             {
                 if (gLAccountCurrencies == null)
                 {
                     gLAccountCurrencies = new List<GLAccountCurrencyPM>();
                 }
                 return gLAccountCurrencies;
              }
             set { gLAccountCurrencies = value; }
	    }
		   
	   private List<GLAccountCurrencyPM>  deletedGLAccountCurrencies;
	   public virtual List<GLAccountCurrencyPM> DeletedGLAccountCurrencies  
	   {
	        get
             {
                 if ( deletedGLAccountCurrencies == null)
                 {
                      deletedGLAccountCurrencies = new List<GLAccountCurrencyPM>();
                 }
                 return  deletedGLAccountCurrencies;
              }
             set {  deletedGLAccountCurrencies = value; }
	    }
	  	  private string parentCurrencyId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ParentCurrencyId  
	   {
	    
	     get
		{
		   return parentCurrencyId;
		 }
		 set
		 {
		   if(parentCurrencyId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ParentCurrencyId",OldValue=parentCurrencyId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   parentCurrencyId=value;
		   }
			
		 }
	   }
	  private bool reportingAsAnotherDocument ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ReportingAsAnotherDocument  
	   {
	    
	     get
		{
		   return reportingAsAnotherDocument;
		 }
		 set
		 {
		   if(reportingAsAnotherDocument != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReportingAsAnotherDocument",OldValue=reportingAsAnotherDocument,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   reportingAsAnotherDocument=value;
		   }
			
		 }
	   }
	  private decimal? creditAllotmentPercentage ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? CreditAllotmentPercentage  
	   {
	    
	     get
		{
		   return creditAllotmentPercentage;
		 }
		 set
		 {
		   if(creditAllotmentPercentage != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreditAllotmentPercentage",OldValue=creditAllotmentPercentage,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   creditAllotmentPercentage=value;
		   }
			
		 }
	   }
	  private string relatedGLAccount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string RelatedGLAccount  
	   {
	    
	     get
		{
		   return relatedGLAccount;
		 }
		 set
		 {
		   if(relatedGLAccount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RelatedGLAccount",OldValue=relatedGLAccount,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   relatedGLAccount=value;
		   }
			
		 }
	   }

	   private List<GLAccountPM> gLAccountChildren;
	 
		     
	   [Include]
	   [Association("", "","")]
	   [DataMember]
	   public virtual List<GLAccountPM> GLAccountChildren  
	   {
	        get
             {
                 if (gLAccountChildren == null)
                 {
                     gLAccountChildren = new List<GLAccountPM>();
                 }
                 return gLAccountChildren;
              }
             set { gLAccountChildren = value; }
	    }
		   
	   private List<GLAccountPM>  deletedGLAccountChildren;
	   public virtual List<GLAccountPM> DeletedGLAccountChildren  
	   {
	        get
             {
                 if ( deletedGLAccountChildren == null)
                 {
                      deletedGLAccountChildren = new List<GLAccountPM>();
                 }
                 return  deletedGLAccountChildren;
              }
             set {  deletedGLAccountChildren = value; }
	    }
	  	  private string cardsDataId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CardsDataId  
	   {
	    
	     get
		{
		   return cardsDataId;
		 }
		 set
		 {
		   if(cardsDataId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CardsDataId",OldValue=cardsDataId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cardsDataId=value;
		   }
			
		 }
	   }
	  private string paymentTermName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string PaymentTermName  
	   {
	    
	     get
		{
		   return paymentTermName;
		 }
		 set
		 {
		   if(paymentTermName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PaymentTermName",OldValue=paymentTermName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   paymentTermName=value;
		   }
			
		 }
	   }
	  private string lastReconciledBy ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string LastReconciledBy  
	   {
	    
	     get
		{
		   return lastReconciledBy;
		 }
		 set
		 {
		   if(lastReconciledBy != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastReconciledBy",OldValue=lastReconciledBy,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lastReconciledBy=value;
		   }
			
		 }
	   }
	  private DateTime? lastReconcileDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? LastReconcileDate  
	   {
	    
	     get
		{
		   return lastReconcileDate;
		 }
		 set
		 {
		   if(lastReconcileDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastReconcileDate",OldValue=lastReconcileDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   lastReconcileDate=value;
		   }
			
		 }
	   }
   }
   
}
	 