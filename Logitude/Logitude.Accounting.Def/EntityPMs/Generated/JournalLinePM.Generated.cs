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
   public partial class JournalLinePM : EntityPM
   {
   	  private string journalId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string JournalId  
	   {
	    
	     get
		{
		   return journalId;
		 }
		 set
		 {
		   if(journalId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="JournalId",OldValue=journalId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   journalId=value;
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
	  private int line ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public int Line  
	   {
	    
	     get
		{
		   return line;
		 }
		 set
		 {
		   if(line != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Line",OldValue=line,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   line=value;
		   }
			
		 }
	   }
	  private string actionCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ActionCode  
	   {
	    
	     get
		{
		   return actionCode;
		 }
		 set
		 {
		   if(actionCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ActionCode",OldValue=actionCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   actionCode=value;
		   }
			
		 }
	   }
	  private string debitControlAccountId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DebitControlAccountId  
	   {
	    
	     get
		{
		   return debitControlAccountId;
		 }
		 set
		 {
		   if(debitControlAccountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DebitControlAccountId",OldValue=debitControlAccountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   debitControlAccountId=value;
		   }
			
		 }
	   }
	  private string debitAccountId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DebitAccountId  
	   {
	    
	     get
		{
		   return debitAccountId;
		 }
		 set
		 {
		   if(debitAccountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DebitAccountId",OldValue=debitAccountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   debitAccountId=value;
		   }
			
		 }
	   }
	  private string creditControlAccountId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreditControlAccountId  
	   {
	    
	     get
		{
		   return creditControlAccountId;
		 }
		 set
		 {
		   if(creditControlAccountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreditControlAccountId",OldValue=creditControlAccountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   creditControlAccountId=value;
		   }
			
		 }
	   }
	  private string creditAccountId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreditAccountId  
	   {
	    
	     get
		{
		   return creditAccountId;
		 }
		 set
		 {
		   if(creditAccountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreditAccountId",OldValue=creditAccountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   creditAccountId=value;
		   }
			
		 }
	   }
	  private DateTime documentDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime DocumentDate  
	   {
	    
	     get
		{
		   return documentDate;
		 }
		 set
		 {
		   if(documentDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DocumentDate",OldValue=documentDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   documentDate=value;
		   }
			
		 }
	   }
	  private DateTime accountingDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime AccountingDate  
	   {
	    
	     get
		{
		   return accountingDate;
		 }
		 set
		 {
		   if(accountingDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountingDate",OldValue=accountingDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   accountingDate=value;
		   }
			
		 }
	   }
	  private DateTime dueDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime DueDate  
	   {
	    
	     get
		{
		   return dueDate;
		 }
		 set
		 {
		   if(dueDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DueDate",OldValue=dueDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   dueDate=value;
		   }
			
		 }
	   }
	  private decimal localAmount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal LocalAmount  
	   {
	    
	     get
		{
		   return localAmount;
		 }
		 set
		 {
		   if(localAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LocalAmount",OldValue=localAmount,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   localAmount=value;
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
	  private decimal foreignAmount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal ForeignAmount  
	   {
	    
	     get
		{
		   return foreignAmount;
		 }
		 set
		 {
		   if(foreignAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ForeignAmount",OldValue=foreignAmount,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   foreignAmount=value;
		   }
			
		 }
	   }
	  private decimal? exchangeRate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? ExchangeRate  
	   {
	    
	     get
		{
		   return exchangeRate;
		 }
		 set
		 {
		   if(exchangeRate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExchangeRate",OldValue=exchangeRate,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   exchangeRate=value;
		   }
			
		 }
	   }
	  private string reference1 ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Reference1  
	   {
	    
	     get
		{
		   return reference1;
		 }
		 set
		 {
		   if(reference1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Reference1",OldValue=reference1,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   reference1=value;
		   }
			
		 }
	   }
	  private string reference2 ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Reference2  
	   {
	    
	     get
		{
		   return reference2;
		 }
		 set
		 {
		   if(reference2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Reference2",OldValue=reference2,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   reference2=value;
		   }
			
		 }
	   }
	  private string reference3 ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Reference3  
	   {
	    
	     get
		{
		   return reference3;
		 }
		 set
		 {
		   if(reference3 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Reference3",OldValue=reference3,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   reference3=value;
		   }
			
		 }
	   }
	  private string actionName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ActionName  
	   {
	    
	     get
		{
		   return actionName;
		 }
		 set
		 {
		   if(actionName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ActionName",OldValue=actionName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   actionName=value;
		   }
			
		 }
	   }
	  private string debitControlAccountName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DebitControlAccountName  
	   {
	    
	     get
		{
		   return debitControlAccountName;
		 }
		 set
		 {
		   if(debitControlAccountName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DebitControlAccountName",OldValue=debitControlAccountName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   debitControlAccountName=value;
		   }
			
		 }
	   }
	  private string creditAccountName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreditAccountName  
	   {
	    
	     get
		{
		   return creditAccountName;
		 }
		 set
		 {
		   if(creditAccountName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreditAccountName",OldValue=creditAccountName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   creditAccountName=value;
		   }
			
		 }
	   }
	  private string debitAccountName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DebitAccountName  
	   {
	    
	     get
		{
		   return debitAccountName;
		 }
		 set
		 {
		   if(debitAccountName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DebitAccountName",OldValue=debitAccountName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   debitAccountName=value;
		   }
			
		 }
	   }
	  private string creditControlAccountName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreditControlAccountName  
	   {
	    
	     get
		{
		   return creditControlAccountName;
		 }
		 set
		 {
		   if(creditControlAccountName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreditControlAccountName",OldValue=creditControlAccountName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   creditControlAccountName=value;
		   }
			
		 }
	   }
	  private string creditControlAccountNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreditControlAccountNumber  
	   {
	    
	     get
		{
		   return creditControlAccountNumber;
		 }
		 set
		 {
		   if(creditControlAccountNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreditControlAccountNumber",OldValue=creditControlAccountNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   creditControlAccountNumber=value;
		   }
			
		 }
	   }
	  private string debitControlAccountNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DebitControlAccountNumber  
	   {
	    
	     get
		{
		   return debitControlAccountNumber;
		 }
		 set
		 {
		   if(debitControlAccountNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DebitControlAccountNumber",OldValue=debitControlAccountNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   debitControlAccountNumber=value;
		   }
			
		 }
	   }
	  private string creditAccountNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreditAccountNumber  
	   {
	    
	     get
		{
		   return creditAccountNumber;
		 }
		 set
		 {
		   if(creditAccountNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreditAccountNumber",OldValue=creditAccountNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   creditAccountNumber=value;
		   }
			
		 }
	   }
	  private string debitAccountNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DebitAccountNumber  
	   {
	    
	     get
		{
		   return debitAccountNumber;
		 }
		 set
		 {
		   if(debitAccountNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DebitAccountNumber",OldValue=debitAccountNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   debitAccountNumber=value;
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
	  private string notes ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Notes  
	   {
	    
	     get
		{
		   return notes;
		 }
		 set
		 {
		   if(notes != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Notes",OldValue=notes,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   notes=value;
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
	  private string actionTypeCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ActionTypeCode  
	   {
	    
	     get
		{
		   return actionTypeCode;
		 }
		 set
		 {
		   if(actionTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ActionTypeCode",OldValue=actionTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   actionTypeCode=value;
		   }
			
		 }
	   }
	  private decimal? externalOpenAmount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? ExternalOpenAmount  
	   {
	    
	     get
		{
		   return externalOpenAmount;
		 }
		 set
		 {
		   if(externalOpenAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExternalOpenAmount",OldValue=externalOpenAmount,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   externalOpenAmount=value;
		   }
			
		 }
	   }
	  private bool? isCreditAccountMulti ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool? IsCreditAccountMulti  
	   {
	    
	     get
		{
		   return isCreditAccountMulti;
		 }
		 set
		 {
		   if(isCreditAccountMulti != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCreditAccountMulti",OldValue=isCreditAccountMulti,NewValue=value,PropertyType="bool?"};
		    NotifyPropertyChanged(values);
		   isCreditAccountMulti=value;
		   }
			
		 }
	   }
	  private bool? isDebitAccountMulti ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool? IsDebitAccountMulti  
	   {
	    
	     get
		{
		   return isDebitAccountMulti;
		 }
		 set
		 {
		   if(isDebitAccountMulti != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsDebitAccountMulti",OldValue=isDebitAccountMulti,NewValue=value,PropertyType="bool?"};
		    NotifyPropertyChanged(values);
		   isDebitAccountMulti=value;
		   }
			
		 }
	   }
	  private string externalReconcileNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExternalReconcileNumber  
	   {
	    
	     get
		{
		   return externalReconcileNumber;
		 }
		 set
		 {
		   if(externalReconcileNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExternalReconcileNumber",OldValue=externalReconcileNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   externalReconcileNumber=value;
		   }
			
		 }
	   }
	  private bool isExternalReconcile ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsExternalReconcile  
	   {
	    
	     get
		{
		   return isExternalReconcile;
		 }
		 set
		 {
		   if(isExternalReconcile != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsExternalReconcile",OldValue=isExternalReconcile,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isExternalReconcile=value;
		   }
			
		 }
	   }
	  private string actionId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ActionId  
	   {
	    
	     get
		{
		   return actionId;
		 }
		 set
		 {
		   if(actionId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ActionId",OldValue=actionId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   actionId=value;
		   }
			
		 }
	   }
	  private string creditAccountCOACode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreditAccountCOACode  
	   {
	    
	     get
		{
		   return creditAccountCOACode;
		 }
		 set
		 {
		   if(creditAccountCOACode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreditAccountCOACode",OldValue=creditAccountCOACode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   creditAccountCOACode=value;
		   }
			
		 }
	   }
	  private string debitAccountCOACode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DebitAccountCOACode  
	   {
	    
	     get
		{
		   return debitAccountCOACode;
		 }
		 set
		 {
		   if(debitAccountCOACode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DebitAccountCOACode",OldValue=debitAccountCOACode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   debitAccountCOACode=value;
		   }
			
		 }
	   }
   }
   
}
	 