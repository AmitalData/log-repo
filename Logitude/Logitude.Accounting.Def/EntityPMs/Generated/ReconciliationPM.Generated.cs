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
   public partial class ReconciliationPM : EntityPM
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
	  private string number ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Number  
	   {
	    
	     get
		{
		   return number;
		 }
		 set
		 {
		   if(number != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Number",OldValue=number,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   number=value;
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

	   private List<ReconciliationLinePM> reconciliationLines;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("ReconciliationReconciliationLines", "Id","ReconciliationId")]
	   [DataMember]
	   public virtual List<ReconciliationLinePM> ReconciliationLines  
	   {
	        get
             {
                 if (reconciliationLines == null)
                 {
                     reconciliationLines = new List<ReconciliationLinePM>();
                 }
                 return reconciliationLines;
              }
             set { reconciliationLines = value; }
	    }
		   
	   private List<ReconciliationLinePM>  deletedReconciliationLines;
	   public virtual List<ReconciliationLinePM> DeletedReconciliationLines  
	   {
	        get
             {
                 if ( deletedReconciliationLines == null)
                 {
                      deletedReconciliationLines = new List<ReconciliationLinePM>();
                 }
                 return  deletedReconciliationLines;
              }
             set {  deletedReconciliationLines = value; }
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
	  private string accountReconcileMethodCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountReconcileMethodCode  
	   {
	    
	     get
		{
		   return accountReconcileMethodCode;
		 }
		 set
		 {
		   if(accountReconcileMethodCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountReconcileMethodCode",OldValue=accountReconcileMethodCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountReconcileMethodCode=value;
		   }
			
		 }
	   }
   }
   
}
	 