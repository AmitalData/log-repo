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
   public partial class LedgerTransactionPM : EntityPM
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
	  private string journalId ;
	  	  
       
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
	  private int journalLineNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public int JournalLineNumber  
	   {
	    
	     get
		{
		   return journalLineNumber;
		 }
		 set
		 {
		   if(journalLineNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="JournalLineNumber",OldValue=journalLineNumber,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   journalLineNumber=value;
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
	  private string accountId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountId  
	   {
	    
	     get
		{
		   return accountId;
		 }
		 set
		 {
		   if(accountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountId",OldValue=accountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountId=value;
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
	  private decimal localAmountDebit ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal LocalAmountDebit  
	   {
	    
	     get
		{
		   return localAmountDebit;
		 }
		 set
		 {
		   if(localAmountDebit != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LocalAmountDebit",OldValue=localAmountDebit,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   localAmountDebit=value;
		   }
			
		 }
	   }
	  private decimal localAmountCredit ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal LocalAmountCredit  
	   {
	    
	     get
		{
		   return localAmountCredit;
		 }
		 set
		 {
		   if(localAmountCredit != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LocalAmountCredit",OldValue=localAmountCredit,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   localAmountCredit=value;
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
	  private decimal foreignAmountDebit ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal ForeignAmountDebit  
	   {
	    
	     get
		{
		   return foreignAmountDebit;
		 }
		 set
		 {
		   if(foreignAmountDebit != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ForeignAmountDebit",OldValue=foreignAmountDebit,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   foreignAmountDebit=value;
		   }
			
		 }
	   }
	  private decimal foreignAmountCredit ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal ForeignAmountCredit  
	   {
	    
	     get
		{
		   return foreignAmountCredit;
		 }
		 set
		 {
		   if(foreignAmountCredit != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ForeignAmountCredit",OldValue=foreignAmountCredit,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   foreignAmountCredit=value;
		   }
			
		 }
	   }
	  private decimal exchangeRate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal ExchangeRate  
	   {
	    
	     get
		{
		   return exchangeRate;
		 }
		 set
		 {
		   if(exchangeRate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExchangeRate",OldValue=exchangeRate,NewValue=value,PropertyType="decimal"};
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
	  private decimal openAmount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal OpenAmount  
	   {
	    
	     get
		{
		   return openAmount;
		 }
		 set
		 {
		   if(openAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OpenAmount",OldValue=openAmount,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   openAmount=value;
		   }
			
		 }
	   }
	  private string oppositeAccountId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string OppositeAccountId  
	   {
	    
	     get
		{
		   return oppositeAccountId;
		 }
		 set
		 {
		   if(oppositeAccountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OppositeAccountId",OldValue=oppositeAccountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   oppositeAccountId=value;
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
	  private string journalNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string JournalNumber  
	   {
	    
	     get
		{
		   return journalNumber;
		 }
		 set
		 {
		   if(journalNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="JournalNumber",OldValue=journalNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   journalNumber=value;
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
	  private string source ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Source  
	   {
	    
	     get
		{
		   return source;
		 }
		 set
		 {
		   if(source != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Source",OldValue=source,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   source=value;
		   }
			
		 }
	   }
	  private string sourceType ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string SourceType  
	   {
	    
	     get
		{
		   return sourceType;
		 }
		 set
		 {
		   if(sourceType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SourceType",OldValue=sourceType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   sourceType=value;
		   }
			
		 }
	   }
	  private string openAmountCurrencyId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string OpenAmountCurrencyId  
	   {
	    
	     get
		{
		   return openAmountCurrencyId;
		 }
		 set
		 {
		   if(openAmountCurrencyId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OpenAmountCurrencyId",OldValue=openAmountCurrencyId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   openAmountCurrencyId=value;
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
	  private decimal cumulativeLocalAmount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal CumulativeLocalAmount  
	   {
	    
	     get
		{
		   return cumulativeLocalAmount;
		 }
		 set
		 {
		   if(cumulativeLocalAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CumulativeLocalAmount",OldValue=cumulativeLocalAmount,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   cumulativeLocalAmount=value;
		   }
			
		 }
	   }
	  private decimal cumulativeForeignAmount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal CumulativeForeignAmount  
	   {
	    
	     get
		{
		   return cumulativeForeignAmount;
		 }
		 set
		 {
		   if(cumulativeForeignAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CumulativeForeignAmount",OldValue=cumulativeForeignAmount,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   cumulativeForeignAmount=value;
		   }
			
		 }
	   }
	  private decimal amountToReconcile ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal AmountToReconcile  
	   {
	    
	     get
		{
		   return amountToReconcile;
		 }
		 set
		 {
		   if(amountToReconcile != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AmountToReconcile",OldValue=amountToReconcile,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   amountToReconcile=value;
		   }
			
		 }
	   }
	  private bool mark ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool Mark  
	   {
	    
	     get
		{
		   return mark;
		 }
		 set
		 {
		   if(mark != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Mark",OldValue=mark,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   mark=value;
		   }
			
		 }
	   }
	  private string openAmountCurrencyCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string OpenAmountCurrencyCode  
	   {
	    
	     get
		{
		   return openAmountCurrencyCode;
		 }
		 set
		 {
		   if(openAmountCurrencyCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OpenAmountCurrencyCode",OldValue=openAmountCurrencyCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   openAmountCurrencyCode=value;
		   }
			
		 }
	   }
	  private bool isReconciled ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsReconciled  
	   {
	    
	     get
		{
		   return isReconciled;
		 }
		 set
		 {
		   if(isReconciled != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsReconciled",OldValue=isReconciled,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isReconciled=value;
		   }
			
		 }
	   }
	  private string sourceId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string SourceId  
	   {
	    
	     get
		{
		   return sourceId;
		 }
		 set
		 {
		   if(sourceId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SourceId",OldValue=sourceId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   sourceId=value;
		   }
			
		 }
	   }
	  private string sourceNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string SourceNumber  
	   {
	    
	     get
		{
		   return sourceNumber;
		 }
		 set
		 {
		   if(sourceNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SourceNumber",OldValue=sourceNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   sourceNumber=value;
		   }
			
		 }
	   }
	  private string sourceTypeCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string SourceTypeCode  
	   {
	    
	     get
		{
		   return sourceTypeCode;
		 }
		 set
		 {
		   if(sourceTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SourceTypeCode",OldValue=sourceTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   sourceTypeCode=value;
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
	  private string openAmountCurrencySign ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string OpenAmountCurrencySign  
	   {
	    
	     get
		{
		   return openAmountCurrencySign;
		 }
		 set
		 {
		   if(openAmountCurrencySign != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OpenAmountCurrencySign",OldValue=openAmountCurrencySign,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   openAmountCurrencySign=value;
		   }
			
		 }
	   }
	  private int groupHash ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public int GroupHash  
	   {
	    
	     get
		{
		   return groupHash;
		 }
		 set
		 {
		   if(groupHash != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GroupHash",OldValue=groupHash,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   groupHash=value;
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
	  private bool inReconcileProgress ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool InReconcileProgress  
	   {
	    
	     get
		{
		   return inReconcileProgress;
		 }
		 set
		 {
		   if(inReconcileProgress != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InReconcileProgress",OldValue=inReconcileProgress,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   inReconcileProgress=value;
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
	  private string reconcileRemarks ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ReconcileRemarks  
	   {
	    
	     get
		{
		   return reconcileRemarks;
		 }
		 set
		 {
		   if(reconcileRemarks != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReconcileRemarks",OldValue=reconcileRemarks,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   reconcileRemarks=value;
		   }
			
		 }
	   }
	  private string originalJournalId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string OriginalJournalId  
	   {
	    
	     get
		{
		   return originalJournalId;
		 }
		 set
		 {
		   if(originalJournalId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OriginalJournalId",OldValue=originalJournalId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   originalJournalId=value;
		   }
			
		 }
	   }
	  private string oppositeAccountEnglishName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string OppositeAccountEnglishName  
	   {
	    
	     get
		{
		   return oppositeAccountEnglishName;
		 }
		 set
		 {
		   if(oppositeAccountEnglishName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OppositeAccountEnglishName",OldValue=oppositeAccountEnglishName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   oppositeAccountEnglishName=value;
		   }
			
		 }
	   }
	  private string oppositeAccountLocalName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string OppositeAccountLocalName  
	   {
	    
	     get
		{
		   return oppositeAccountLocalName;
		 }
		 set
		 {
		   if(oppositeAccountLocalName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OppositeAccountLocalName",OldValue=oppositeAccountLocalName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   oppositeAccountLocalName=value;
		   }
			
		 }
	   }
	  private string oppositeAccountDisplayNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string OppositeAccountDisplayNumber  
	   {
	    
	     get
		{
		   return oppositeAccountDisplayNumber;
		 }
		 set
		 {
		   if(oppositeAccountDisplayNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OppositeAccountDisplayNumber",OldValue=oppositeAccountDisplayNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   oppositeAccountDisplayNumber=value;
		   }
			
		 }
	   }
	  private string recoNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string RecoNumber  
	   {
	    
	     get
		{
		   return recoNumber;
		 }
		 set
		 {
		   if(recoNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RecoNumber",OldValue=recoNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   recoNumber=value;
		   }
			
		 }
	   }
	  private string reconciliationId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ReconciliationId  
	   {
	    
	     get
		{
		   return reconciliationId;
		 }
		 set
		 {
		   if(reconciliationId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReconciliationId",OldValue=reconciliationId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   reconciliationId=value;
		   }
			
		 }
	   }
	  private decimal? paymentReconciledAmount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? PaymentReconciledAmount  
	   {
	    
	     get
		{
		   return paymentReconciledAmount;
		 }
		 set
		 {
		   if(paymentReconciledAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PaymentReconciledAmount",OldValue=paymentReconciledAmount,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   paymentReconciledAmount=value;
		   }
			
		 }
	   }
	  private bool inProgressExternalReconcile ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool InProgressExternalReconcile  
	   {
	    
	     get
		{
		   return inProgressExternalReconcile;
		 }
		 set
		 {
		   if(inProgressExternalReconcile != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InProgressExternalReconcile",OldValue=inProgressExternalReconcile,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   inProgressExternalReconcile=value;
		   }
			
		 }
	   }
	  private decimal originalAmount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal OriginalAmount  
	   {
	    
	     get
		{
		   return originalAmount;
		 }
		 set
		 {
		   if(originalAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OriginalAmount",OldValue=originalAmount,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   originalAmount=value;
		   }
			
		 }
	   }
   }
   
}
	 