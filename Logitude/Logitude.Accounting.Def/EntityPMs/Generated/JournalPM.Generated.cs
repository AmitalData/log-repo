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
   public partial class JournalPM : EntityPM
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
	  private string typeCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string TypeCode  
	   {
	    
	     get
		{
		   return typeCode;
		 }
		 set
		 {
		   if(typeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TypeCode",OldValue=typeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   typeCode=value;
		   }
			
		 }
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
	  private string accountingEntityCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountingEntityCode  
	   {
	    
	     get
		{
		   return accountingEntityCode;
		 }
		 set
		 {
		   if(accountingEntityCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountingEntityCode",OldValue=accountingEntityCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountingEntityCode=value;
		   }
			
		 }
	   }
	  private string accountingEntityId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountingEntityId  
	   {
	    
	     get
		{
		   return accountingEntityId;
		 }
		 set
		 {
		   if(accountingEntityId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountingEntityId",OldValue=accountingEntityId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountingEntityId=value;
		   }
			
		 }
	   }
	  private string externalNo ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExternalNo  
	   {
	    
	     get
		{
		   return externalNo;
		 }
		 set
		 {
		   if(externalNo != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExternalNo",OldValue=externalNo,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   externalNo=value;
		   }
			
		 }
	   }
	  private string typeName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string TypeName  
	   {
	    
	     get
		{
		   return typeName;
		 }
		 set
		 {
		   if(typeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TypeName",OldValue=typeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   typeName=value;
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
	  private string accountingEntityName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountingEntityName  
	   {
	    
	     get
		{
		   return accountingEntityName;
		 }
		 set
		 {
		   if(accountingEntityName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountingEntityName",OldValue=accountingEntityName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountingEntityName=value;
		   }
			
		 }
	   }

	   private List<JournalLinePM> journalLines;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("JournalJournalLines", "Id","JournalId")]
	   [DataMember]
	   public virtual List<JournalLinePM> JournalLines  
	   {
	        get
             {
                 if (journalLines == null)
                 {
                     journalLines = new List<JournalLinePM>();
                 }
                 return journalLines;
              }
             set { journalLines = value; }
	    }
		   
	   private List<JournalLinePM>  deletedJournalLines;
	   public virtual List<JournalLinePM> DeletedJournalLines  
	   {
	        get
             {
                 if ( deletedJournalLines == null)
                 {
                      deletedJournalLines = new List<JournalLinePM>();
                 }
                 return  deletedJournalLines;
              }
             set {  deletedJournalLines = value; }
	    }
	  	  private DateTime? updateDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? UpdateDate  
	   {
	    
	     get
		{
		   return updateDate;
		 }
		 set
		 {
		   if(updateDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdateDate",OldValue=updateDate,NewValue=value,PropertyType="DateTime?"};
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
	  private DateTime? approveDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ApproveDate  
	   {
	    
	     get
		{
		   return approveDate;
		 }
		 set
		 {
		   if(approveDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ApproveDate",OldValue=approveDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   approveDate=value;
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
	  private string approvedByUserName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ApprovedByUserName  
	   {
	    
	     get
		{
		   return approvedByUserName;
		 }
		 set
		 {
		   if(approvedByUserName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ApprovedByUserName",OldValue=approvedByUserName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   approvedByUserName=value;
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
	  private string accountingEntityReference ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountingEntityReference  
	   {
	    
	     get
		{
		   return accountingEntityReference;
		 }
		 set
		 {
		   if(accountingEntityReference != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountingEntityReference",OldValue=accountingEntityReference,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountingEntityReference=value;
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
	  private string voidedByUserId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string VoidedByUserId  
	   {
	    
	     get
		{
		   return voidedByUserId;
		 }
		 set
		 {
		   if(voidedByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VoidedByUserId",OldValue=voidedByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   voidedByUserId=value;
		   }
			
		 }
	   }
	  private DateTime? voidDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? VoidDate  
	   {
	    
	     get
		{
		   return voidDate;
		 }
		 set
		 {
		   if(voidDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VoidDate",OldValue=voidDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   voidDate=value;
		   }
			
		 }
	   }
	  private string originalJournalName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string OriginalJournalName  
	   {
	    
	     get
		{
		   return originalJournalName;
		 }
		 set
		 {
		   if(originalJournalName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OriginalJournalName",OldValue=originalJournalName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   originalJournalName=value;
		   }
			
		 }
	   }
	  private string voidedByUserName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string VoidedByUserName  
	   {
	    
	     get
		{
		   return voidedByUserName;
		 }
		 set
		 {
		   if(voidedByUserName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VoidedByUserName",OldValue=voidedByUserName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   voidedByUserName=value;
		   }
			
		 }
	   }
	  private bool? isVoided ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool? IsVoided  
	   {
	    
	     get
		{
		   return isVoided;
		 }
		 set
		 {
		   if(isVoided != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsVoided",OldValue=isVoided,NewValue=value,PropertyType="bool?"};
		    NotifyPropertyChanged(values);
		   isVoided=value;
		   }
			
		 }
	   }
	  private string voidedByJournalId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string VoidedByJournalId  
	   {
	    
	     get
		{
		   return voidedByJournalId;
		 }
		 set
		 {
		   if(voidedByJournalId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VoidedByJournalId",OldValue=voidedByJournalId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   voidedByJournalId=value;
		   }
			
		 }
	   }
	  private string externalSystem ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExternalSystem  
	   {
	    
	     get
		{
		   return externalSystem;
		 }
		 set
		 {
		   if(externalSystem != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExternalSystem",OldValue=externalSystem,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   externalSystem=value;
		   }
			
		 }
	   }
	  private string queueId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string QueueId  
	   {
	    
	     get
		{
		   return queueId;
		 }
		 set
		 {
		   if(queueId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QueueId",OldValue=queueId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   queueId=value;
		   }
			
		 }
	   }
	  private string statusLocalName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string StatusLocalName  
	   {
	    
	     get
		{
		   return statusLocalName;
		 }
		 set
		 {
		   if(statusLocalName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StatusLocalName",OldValue=statusLocalName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   statusLocalName=value;
		   }
			
		 }
	   }

	   private List<JournalReconcilePM> journalReconciles;
	 
		     
	   [Include]
	   [Association("JournalJournalReconciles", "Id","JournalId")]
	   [DataMember]
	   public virtual List<JournalReconcilePM> JournalReconciles  
	   {
	        get
             {
                 if (journalReconciles == null)
                 {
                     journalReconciles = new List<JournalReconcilePM>();
                 }
                 return journalReconciles;
              }
             set { journalReconciles = value; }
	    }
		   
	   private List<JournalReconcilePM>  deletedJournalReconciles;
	   public virtual List<JournalReconcilePM> DeletedJournalReconciles  
	   {
	        get
             {
                 if ( deletedJournalReconciles == null)
                 {
                      deletedJournalReconciles = new List<JournalReconcilePM>();
                 }
                 return  deletedJournalReconciles;
              }
             set {  deletedJournalReconciles = value; }
	    }
	  	  private bool isLedgerCreated ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsLedgerCreated  
	   {
	    
	     get
		{
		   return isLedgerCreated;
		 }
		 set
		 {
		   if(isLedgerCreated != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsLedgerCreated",OldValue=isLedgerCreated,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isLedgerCreated=value;
		   }
			
		 }
	   }

	   private List<JournalExternalReconcilePM> journalExternalReconciles;
	 
		     
	   [Include]
	   [Association("JournalJournalExternalReconciles", "Id","JournalId")]
	   [DataMember]
	   public virtual List<JournalExternalReconcilePM> JournalExternalReconciles  
	   {
	        get
             {
                 if (journalExternalReconciles == null)
                 {
                     journalExternalReconciles = new List<JournalExternalReconcilePM>();
                 }
                 return journalExternalReconciles;
              }
             set { journalExternalReconciles = value; }
	    }
		   
	   private List<JournalExternalReconcilePM>  deletedJournalExternalReconciles;
	   public virtual List<JournalExternalReconcilePM> DeletedJournalExternalReconciles  
	   {
	        get
             {
                 if ( deletedJournalExternalReconciles == null)
                 {
                      deletedJournalExternalReconciles = new List<JournalExternalReconcilePM>();
                 }
                 return  deletedJournalExternalReconciles;
              }
             set {  deletedJournalExternalReconciles = value; }
	    }
	  	  private string lineCreditAccountTypeCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string LineCreditAccountTypeCode  
	   {
	    
	     get
		{
		   return lineCreditAccountTypeCode;
		 }
		 set
		 {
		   if(lineCreditAccountTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LineCreditAccountTypeCode",OldValue=lineCreditAccountTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lineCreditAccountTypeCode=value;
		   }
			
		 }
	   }
	  private int taxReportJournalLineNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public int TaxReportJournalLineNumber  
	   {
	    
	     get
		{
		   return taxReportJournalLineNumber;
		 }
		 set
		 {
		   if(taxReportJournalLineNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TaxReportJournalLineNumber",OldValue=taxReportJournalLineNumber,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   taxReportJournalLineNumber=value;
		   }
			
		 }
	   }
	  private DateTime? documentDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? DocumentDate  
	   {
	    
	     get
		{
		   return documentDate;
		 }
		 set
		 {
		   if(documentDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DocumentDate",OldValue=documentDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   documentDate=value;
		   }
			
		 }
	   }
	  private DateTime? dueDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? DueDate  
	   {
	    
	     get
		{
		   return dueDate;
		 }
		 set
		 {
		   if(dueDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DueDate",OldValue=dueDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   dueDate=value;
		   }
			
		 }
	   }
	  private DateTime? aPPaymentCancelDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? APPaymentCancelDate  
	   {
	    
	     get
		{
		   return aPPaymentCancelDate;
		 }
		 set
		 {
		   if(aPPaymentCancelDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="APPaymentCancelDate",OldValue=aPPaymentCancelDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   aPPaymentCancelDate=value;
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
	  private bool isNew ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsNew  
	   {
	    
	     get
		{
		   return isNew;
		 }
		 set
		 {
		   if(isNew != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsNew",OldValue=isNew,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isNew=value;
		   }
			
		 }
	   }
	  private bool copied ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool Copied  
	   {
	    
	     get
		{
		   return copied;
		 }
		 set
		 {
		   if(copied != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Copied",OldValue=copied,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   copied=value;
		   }
			
		 }
	   }
	  private string copiedFrom ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CopiedFrom  
	   {
	    
	     get
		{
		   return copiedFrom;
		 }
		 set
		 {
		   if(copiedFrom != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CopiedFrom",OldValue=copiedFrom,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   copiedFrom=value;
		   }
			
		 }
	   }
	  private int? securityLevel ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public int? SecurityLevel  
	   {
	    
	     get
		{
		   return securityLevel;
		 }
		 set
		 {
		   if(securityLevel != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SecurityLevel",OldValue=securityLevel,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   securityLevel=value;
		   }
			
		 }
	   }
	    }
   
}
	 