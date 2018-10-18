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
   public partial class ReconcileExternalPagePM : EntityPM
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
	  private int pageNo ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public int PageNo  
	   {
	    
	     get
		{
		   return pageNo;
		 }
		 set
		 {
		   if(pageNo != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageNo",OldValue=pageNo,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   pageNo=value;
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
	  private DateTime fromDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime FromDate  
	   {
	    
	     get
		{
		   return fromDate;
		 }
		 set
		 {
		   if(fromDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromDate",OldValue=fromDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   fromDate=value;
		   }
			
		 }
	   }
	  private DateTime toDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime ToDate  
	   {
	    
	     get
		{
		   return toDate;
		 }
		 set
		 {
		   if(toDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToDate",OldValue=toDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   toDate=value;
		   }
			
		 }
	   }
	  private decimal startBalance ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal StartBalance  
	   {
	    
	     get
		{
		   return startBalance;
		 }
		 set
		 {
		   if(startBalance != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StartBalance",OldValue=startBalance,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   startBalance=value;
		   }
			
		 }
	   }
	  private decimal closeBalance ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal CloseBalance  
	   {
	    
	     get
		{
		   return closeBalance;
		 }
		 set
		 {
		   if(closeBalance != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CloseBalance",OldValue=closeBalance,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   closeBalance=value;
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
	  private string approvedByUserId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ApprovedByUserId  
	   {
	    
	     get
		{
		   return approvedByUserId;
		 }
		 set
		 {
		   if(approvedByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ApprovedByUserId",OldValue=approvedByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   approvedByUserId=value;
		   }
			
		 }
	   }

	   private List<ReconcileExternalPageLinePM> reconcileExternalPageLines;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("ReconcileExternalPageReconcileExternalPageLine", "Id","ReconcileExternalPageId")]
	   [DataMember]
	   public virtual List<ReconcileExternalPageLinePM> ReconcileExternalPageLines  
	   {
	        get
             {
                 if (reconcileExternalPageLines == null)
                 {
                     reconcileExternalPageLines = new List<ReconcileExternalPageLinePM>();
                 }
                 return reconcileExternalPageLines;
              }
             set { reconcileExternalPageLines = value; }
	    }
		   
	   private List<ReconcileExternalPageLinePM>  deletedReconcileExternalPageLines;
	   public virtual List<ReconcileExternalPageLinePM> DeletedReconcileExternalPageLines  
	   {
	        get
             {
                 if ( deletedReconcileExternalPageLines == null)
                 {
                      deletedReconcileExternalPageLines = new List<ReconcileExternalPageLinePM>();
                 }
                 return  deletedReconcileExternalPageLines;
              }
             set {  deletedReconcileExternalPageLines = value; }
	    }
	  	  private string statusCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string StatusCode  
	   {
	    
	     get
		{
		   return statusCode;
		 }
		 set
		 {
		   if(statusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StatusCode",OldValue=statusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   statusCode=value;
		   }
			
		 }
	   }
	  private string statusName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string StatusName  
	   {
	    
	     get
		{
		   return statusName;
		 }
		 set
		 {
		   if(statusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StatusName",OldValue=statusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   statusName=value;
		   }
			
		 }
	   }
	  private string entryTypeCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntryTypeCode  
	   {
	    
	     get
		{
		   return entryTypeCode;
		 }
		 set
		 {
		   if(entryTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntryTypeCode",OldValue=entryTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entryTypeCode=value;
		   }
			
		 }
	   }
	  private string entryTypeEnglishName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntryTypeEnglishName  
	   {
	    
	     get
		{
		   return entryTypeEnglishName;
		 }
		 set
		 {
		   if(entryTypeEnglishName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntryTypeEnglishName",OldValue=entryTypeEnglishName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entryTypeEnglishName=value;
		   }
			
		 }
	   }
	  private string entryTypeLocalName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntryTypeLocalName  
	   {
	    
	     get
		{
		   return entryTypeLocalName;
		 }
		 set
		 {
		   if(entryTypeLocalName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntryTypeLocalName",OldValue=entryTypeLocalName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entryTypeLocalName=value;
		   }
			
		 }
	   }
   }
   
}
	 