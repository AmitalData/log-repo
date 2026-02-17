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
using Logitude.Customs.Def.Validators;
  
namespace Logitude.Customs.Def.EntityPMs
{
   [CustomValidation(typeof(CustomsClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class NotificationPM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private string notificationDefinitionCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string NotificationDefinitionCode  
	   {
	    
	     get
		{
		   return notificationDefinitionCode;
		 }
		 set
		 {
		   if(notificationDefinitionCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NotificationDefinitionCode",OldValue=notificationDefinitionCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   notificationDefinitionCode=value;
		   }
			
		 }
	   }
	  private DateTime? createDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? CreateDate  
	   {
	    
	     get
		{
		   return createDate;
		 }
		 set
		 {
		   if(createDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreateDate",OldValue=createDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   createDate=value;
		   }
			
		 }
	   }
	  private string assigneToId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AssigneToId  
	   {
	    
	     get
		{
		   return assigneToId;
		 }
		 set
		 {
		   if(assigneToId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AssigneToId",OldValue=assigneToId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   assigneToId=value;
		   }
			
		 }
	   }
	  private string assigneToNotificationTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AssigneToNotificationTypeCode  
	   {
	    
	     get
		{
		   return assigneToNotificationTypeCode;
		 }
		 set
		 {
		   if(assigneToNotificationTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AssigneToNotificationTypeCode",OldValue=assigneToNotificationTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   assigneToNotificationTypeCode=value;
		   }
			
		 }
	   }
	  private string declarationOfficeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeclarationOfficeCode  
	   {
	    
	     get
		{
		   return declarationOfficeCode;
		 }
		 set
		 {
		   if(declarationOfficeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeclarationOfficeCode",OldValue=declarationOfficeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   declarationOfficeCode=value;
		   }
			
		 }
	   }
	  private string entityId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntityId  
	   {
	    
	     get
		{
		   return entityId;
		 }
		 set
		 {
		   if(entityId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntityId",OldValue=entityId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entityId=value;
		   }
			
		 }
	   }
	  private string objectTableId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ObjectTableId  
	   {
	    
	     get
		{
		   return objectTableId;
		 }
		 set
		 {
		   if(objectTableId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ObjectTableId",OldValue=objectTableId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   objectTableId=value;
		   }
			
		 }
	   }
	  private bool isClosedBCustomOffice ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsClosedBCustomOffice  
	   {
	    
	     get
		{
		   return isClosedBCustomOffice;
		 }
		 set
		 {
		   if(isClosedBCustomOffice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsClosedBCustomOffice",OldValue=isClosedBCustomOffice,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isClosedBCustomOffice=value;
		   }
			
		 }
	   }
	  private bool isClosedByAssignee ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsClosedByAssignee  
	   {
	    
	     get
		{
		   return isClosedByAssignee;
		 }
		 set
		 {
		   if(isClosedByAssignee != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsClosedByAssignee",OldValue=isClosedByAssignee,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isClosedByAssignee=value;
		   }
			
		 }
	   }
	  private DateTime? dueDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private bool isSeenByAssignee ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsSeenByAssignee  
	   {
	    
	     get
		{
		   return isSeenByAssignee;
		 }
		 set
		 {
		   if(isSeenByAssignee != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsSeenByAssignee",OldValue=isSeenByAssignee,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isSeenByAssignee=value;
		   }
			
		 }
	   }
	  private string responseNotes ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ResponseNotes  
	   {
	    
	     get
		{
		   return responseNotes;
		 }
		 set
		 {
		   if(responseNotes != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ResponseNotes",OldValue=responseNotes,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   responseNotes=value;
		   }
			
		 }
	   }
	  private string createdByRequestID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreatedByRequestID  
	   {
	    
	     get
		{
		   return createdByRequestID;
		 }
		 set
		 {
		   if(createdByRequestID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreatedByRequestID",OldValue=createdByRequestID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   createdByRequestID=value;
		   }
			
		 }
	   }
	  private string searchFields ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private string objectTableName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ObjectTableName  
	   {
	    
	     get
		{
		   return objectTableName;
		 }
		 set
		 {
		   if(objectTableName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ObjectTableName",OldValue=objectTableName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   objectTableName=value;
		   }
			
		 }
	   }
	  private string notificationDefinitionName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string NotificationDefinitionName  
	   {
	    
	     get
		{
		   return notificationDefinitionName;
		 }
		 set
		 {
		   if(notificationDefinitionName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NotificationDefinitionName",OldValue=notificationDefinitionName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   notificationDefinitionName=value;
		   }
			
		 }
	   }
	  private string assigneToName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AssigneToName  
	   {
	    
	     get
		{
		   return assigneToName;
		 }
		 set
		 {
		   if(assigneToName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AssigneToName",OldValue=assigneToName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   assigneToName=value;
		   }
			
		 }
	   }
	  private bool isHandledByCustomOffice ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsHandledByCustomOffice  
	   {
	    
	     get
		{
		   return isHandledByCustomOffice;
		 }
		 set
		 {
		   if(isHandledByCustomOffice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsHandledByCustomOffice",OldValue=isHandledByCustomOffice,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isHandledByCustomOffice=value;
		   }
			
		 }
	   }
	  private string reference1Number ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Reference1Number  
	   {
	    
	     get
		{
		   return reference1Number;
		 }
		 set
		 {
		   if(reference1Number != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Reference1Number",OldValue=reference1Number,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   reference1Number=value;
		   }
			
		 }
	   }
	  private string reference2Number ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Reference2Number  
	   {
	    
	     get
		{
		   return reference2Number;
		 }
		 set
		 {
		   if(reference2Number != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Reference2Number",OldValue=reference2Number,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   reference2Number=value;
		   }
			
		 }
	   }
	  private string departmentId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DepartmentId  
	   {
	    
	     get
		{
		   return departmentId;
		 }
		 set
		 {
		   if(departmentId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DepartmentId",OldValue=departmentId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   departmentId=value;
		   }
			
		 }
	   }
	  private string closedByCustomOfficeUserId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ClosedByCustomOfficeUserId  
	   {
	    
	     get
		{
		   return closedByCustomOfficeUserId;
		 }
		 set
		 {
		   if(closedByCustomOfficeUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClosedByCustomOfficeUserId",OldValue=closedByCustomOfficeUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   closedByCustomOfficeUserId=value;
		   }
			
		 }
	   }
	  private string description ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Description  
	   {
	    
	     get
		{
		   return description;
		 }
		 set
		 {
		   if(description != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Description",OldValue=description,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   description=value;
		   }
			
		 }
	   }
	  private string departmentName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DepartmentName  
	   {
	    
	     get
		{
		   return departmentName;
		 }
		 set
		 {
		   if(departmentName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DepartmentName",OldValue=departmentName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   departmentName=value;
		   }
			
		 }
	   }
	  private string closedByAssignee ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ClosedByAssignee  
	   {
	    
	     get
		{
		   return closedByAssignee;
		 }
		 set
		 {
		   if(closedByAssignee != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClosedByAssignee",OldValue=closedByAssignee,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   closedByAssignee=value;
		   }
			
		 }
	   }
	  private string customerName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerName  
	   {
	    
	     get
		{
		   return customerName;
		 }
		 set
		 {
		   if(customerName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerName",OldValue=customerName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerName=value;
		   }
			
		 }
	   }
	  private bool badjCount ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool BadjCount  
	   {
	    
	     get
		{
		   return badjCount;
		 }
		 set
		 {
		   if(badjCount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BadjCount",OldValue=badjCount,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   badjCount=value;
		   }
			
		 }
	   }
	  private string closedByCustomOfficeUserName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ClosedByCustomOfficeUserName  
	   {
	    
	     get
		{
		   return closedByCustomOfficeUserName;
		 }
		 set
		 {
		   if(closedByCustomOfficeUserName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClosedByCustomOfficeUserName",OldValue=closedByCustomOfficeUserName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   closedByCustomOfficeUserName=value;
		   }
			
		 }
	   }
	  private string closedByAssigneeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ClosedByAssigneeName  
	   {
	    
	     get
		{
		   return closedByAssigneeName;
		 }
		 set
		 {
		   if(closedByAssigneeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClosedByAssigneeName",OldValue=closedByAssigneeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   closedByAssigneeName=value;
		   }
			
		 }
	   }

	   private List<NotificationReplyPM> notificationRplies;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("NotificationNotificationReplies", "Id","NotificationId")]
	   [DataMember]
	   public virtual List<NotificationReplyPM> NotificationRplies  
	   {
	        get
             {
                 if (notificationRplies == null)
                 {
                     notificationRplies = new List<NotificationReplyPM>();
                 }
                 return notificationRplies;
              }
             set { notificationRplies = value; }
	    }
		   
	   private List<NotificationReplyPM>  deletedNotificationRplies;
	   public virtual List<NotificationReplyPM> DeletedNotificationRplies  
	   {
	        get
             {
                 if ( deletedNotificationRplies == null)
                 {
                      deletedNotificationRplies = new List<NotificationReplyPM>();
                 }
                 return  deletedNotificationRplies;
              }
             set {  deletedNotificationRplies = value; }
	    }
	  	  private int notificationReplyLastLineNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int NotificationReplyLastLineNumber  
	   {
	    
	     get
		{
		   return notificationReplyLastLineNumber;
		 }
		 set
		 {
		   if(notificationReplyLastLineNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NotificationReplyLastLineNumber",OldValue=notificationReplyLastLineNumber,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   notificationReplyLastLineNumber=value;
		   }
			
		 }
	   }
	  private string customerId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerId  
	   {
	    
	     get
		{
		   return customerId;
		 }
		 set
		 {
		   if(customerId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerId",OldValue=customerId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerId=value;
		   }
			
		 }
	   }
   }
   
}
	 