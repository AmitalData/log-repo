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
   public partial class BankDepositPM : EntityPM
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
	  private int depositNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public int DepositNumber  
	   {
	    
	     get
		{
		   return depositNumber;
		 }
		 set
		 {
		   if(depositNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DepositNumber",OldValue=depositNumber,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   depositNumber=value;
		   }
			
		 }
	   }
	  private DateTime depositDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime DepositDate  
	   {
	    
	     get
		{
		   return depositDate;
		 }
		 set
		 {
		   if(depositDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DepositDate",OldValue=depositDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   depositDate=value;
		   }
			
		 }
	   }
	  private string depositCurrencyId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DepositCurrencyId  
	   {
	    
	     get
		{
		   return depositCurrencyId;
		 }
		 set
		 {
		   if(depositCurrencyId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DepositCurrencyId",OldValue=depositCurrencyId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   depositCurrencyId=value;
		   }
			
		 }
	   }
	  private decimal localDepositAmount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal LocalDepositAmount  
	   {
	    
	     get
		{
		   return localDepositAmount;
		 }
		 set
		 {
		   if(localDepositAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LocalDepositAmount",OldValue=localDepositAmount,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   localDepositAmount=value;
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
	  private string depositBankAccountId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DepositBankAccountId  
	   {
	    
	     get
		{
		   return depositBankAccountId;
		 }
		 set
		 {
		   if(depositBankAccountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DepositBankAccountId",OldValue=depositBankAccountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   depositBankAccountId=value;
		   }
			
		 }
	   }
	  private string cashBookId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CashBookId  
	   {
	    
	     get
		{
		   return cashBookId;
		 }
		 set
		 {
		   if(cashBookId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CashBookId",OldValue=cashBookId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cashBookId=value;
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

	   private List<BankDepositLinePM> bankDepositLines;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("BankDepositLines", "Id","DepositId")]
	   [DataMember]
	   public virtual List<BankDepositLinePM> BankDepositLines  
	   {
	        get
             {
                 if (bankDepositLines == null)
                 {
                     bankDepositLines = new List<BankDepositLinePM>();
                 }
                 return bankDepositLines;
              }
             set { bankDepositLines = value; }
	    }
		   
	   private List<BankDepositLinePM>  deletedBankDepositLines;
	   public virtual List<BankDepositLinePM> DeletedBankDepositLines  
	   {
	        get
             {
                 if ( deletedBankDepositLines == null)
                 {
                      deletedBankDepositLines = new List<BankDepositLinePM>();
                 }
                 return  deletedBankDepositLines;
              }
             set {  deletedBankDepositLines = value; }
	    }
	  	  private string cashBookGLAccountId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CashBookGLAccountId  
	   {
	    
	     get
		{
		   return cashBookGLAccountId;
		 }
		 set
		 {
		   if(cashBookGLAccountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CashBookGLAccountId",OldValue=cashBookGLAccountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cashBookGLAccountId=value;
		   }
			
		 }
	   }
	  private bool isCashDeposit ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCashDeposit  
	   {
	    
	     get
		{
		   return isCashDeposit;
		 }
		 set
		 {
		   if(isCashDeposit != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCashDeposit",OldValue=isCashDeposit,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCashDeposit=value;
		   }
			
		 }
	   }
	  private string deferredGLAccountId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeferredGLAccountId  
	   {
	    
	     get
		{
		   return deferredGLAccountId;
		 }
		 set
		 {
		   if(deferredGLAccountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeferredGLAccountId",OldValue=deferredGLAccountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   deferredGLAccountId=value;
		   }
			
		 }
	   }
	  private string cashGLAccountId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CashGLAccountId  
	   {
	    
	     get
		{
		   return cashGLAccountId;
		 }
		 set
		 {
		   if(cashGLAccountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CashGLAccountId",OldValue=cashGLAccountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cashGLAccountId=value;
		   }
			
		 }
	   }
	  private bool isCanceled ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCanceled  
	   {
	    
	     get
		{
		   return isCanceled;
		 }
		 set
		 {
		   if(isCanceled != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCanceled",OldValue=isCanceled,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCanceled=value;
		   }
			
		 }
	   }
	  private string depositCurrencyCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DepositCurrencyCode  
	   {
	    
	     get
		{
		   return depositCurrencyCode;
		 }
		 set
		 {
		   if(depositCurrencyCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DepositCurrencyCode",OldValue=depositCurrencyCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   depositCurrencyCode=value;
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
	  private string cashBookName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CashBookName  
	   {
	    
	     get
		{
		   return cashBookName;
		 }
		 set
		 {
		   if(cashBookName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CashBookName",OldValue=cashBookName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cashBookName=value;
		   }
			
		 }
	   }
	  private DateTime? lastActivityDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? LastActivityDate  
	   {
	    
	     get
		{
		   return lastActivityDate;
		 }
		 set
		 {
		   if(lastActivityDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastActivityDate",OldValue=lastActivityDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   lastActivityDate=value;
		   }
			
		 }
	   }
	  private string lastActivityTypeName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string LastActivityTypeName  
	   {
	    
	     get
		{
		   return lastActivityTypeName;
		 }
		 set
		 {
		   if(lastActivityTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastActivityTypeName",OldValue=lastActivityTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lastActivityTypeName=value;
		   }
			
		 }
	   }
	  private string lastActivityByUserName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string LastActivityByUserName  
	   {
	    
	     get
		{
		   return lastActivityByUserName;
		 }
		 set
		 {
		   if(lastActivityByUserName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastActivityByUserName",OldValue=lastActivityByUserName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lastActivityByUserName=value;
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
	  private string bankAccountNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string BankAccountNumber  
	   {
	    
	     get
		{
		   return bankAccountNumber;
		 }
		 set
		 {
		   if(bankAccountNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BankAccountNumber",OldValue=bankAccountNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   bankAccountNumber=value;
		   }
			
		 }
	   }
	  private string journalQueueId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string JournalQueueId  
	   {
	    
	     get
		{
		   return journalQueueId;
		 }
		 set
		 {
		   if(journalQueueId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="JournalQueueId",OldValue=journalQueueId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   journalQueueId=value;
		   }
			
		 }
	   }
	    }
   
}
	 