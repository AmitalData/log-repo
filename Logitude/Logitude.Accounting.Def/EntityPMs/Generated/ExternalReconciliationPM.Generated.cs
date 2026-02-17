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
   public partial class ExternalReconciliationPM : EntityPM
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
	  private string gLAccountId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string GLAccountId  
	   {
	    
	     get
		{
		   return gLAccountId;
		 }
		 set
		 {
		   if(gLAccountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GLAccountId",OldValue=gLAccountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   gLAccountId=value;
		   }
			
		 }
	   }
	  private int? reconciliationNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public int? ReconciliationNumber  
	   {
	    
	     get
		{
		   return reconciliationNumber;
		 }
		 set
		 {
		   if(reconciliationNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReconciliationNumber",OldValue=reconciliationNumber,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   reconciliationNumber=value;
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

	   private List<ExternalReconciliationLinePM> externalReconciliationLines;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("ExternalReconciliationExternalReconciliationLines", "Id","ReconciliationId")]
	   [DataMember]
	   public virtual List<ExternalReconciliationLinePM> ExternalReconciliationLines  
	   {
	        get
             {
                 if (externalReconciliationLines == null)
                 {
                     externalReconciliationLines = new List<ExternalReconciliationLinePM>();
                 }
                 return externalReconciliationLines;
              }
             set { externalReconciliationLines = value; }
	    }
		   
	   private List<ExternalReconciliationLinePM>  deletedExternalReconciliationLines;
	   public virtual List<ExternalReconciliationLinePM> DeletedExternalReconciliationLines  
	   {
	        get
             {
                 if ( deletedExternalReconciliationLines == null)
                 {
                      deletedExternalReconciliationLines = new List<ExternalReconciliationLinePM>();
                 }
                 return  deletedExternalReconciliationLines;
              }
             set {  deletedExternalReconciliationLines = value; }
	    }
	  	  private string accountNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountNumber  
	   {
	    
	     get
		{
		   return accountNumber;
		 }
		 set
		 {
		   if(accountNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountNumber",OldValue=accountNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountNumber=value;
		   }
			
		 }
	   }
	  private string accountName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountName  
	   {
	    
	     get
		{
		   return accountName;
		 }
		 set
		 {
		   if(accountName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountName",OldValue=accountName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountName=value;
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
	  private string accountCurrencyId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountCurrencyId  
	   {
	    
	     get
		{
		   return accountCurrencyId;
		 }
		 set
		 {
		   if(accountCurrencyId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountCurrencyId",OldValue=accountCurrencyId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountCurrencyId=value;
		   }
			
		 }
	   }
	  private bool isCancelled ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCancelled  
	   {
	    
	     get
		{
		   return isCancelled;
		 }
		 set
		 {
		   if(isCancelled != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCancelled",OldValue=isCancelled,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCancelled=value;
		   }
			
		 }
	   }
	  private string bankAccountId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string BankAccountId  
	   {
	    
	     get
		{
		   return bankAccountId;
		 }
		 set
		 {
		   if(bankAccountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BankAccountId",OldValue=bankAccountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   bankAccountId=value;
		   }
			
		 }
	   }
   }
   
}
	 