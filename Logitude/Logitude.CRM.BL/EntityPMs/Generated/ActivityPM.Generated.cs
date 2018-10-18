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
using Logitude.CRM.BL.Validators;
  
namespace Logitude.CRM.BL.EntityPMs
{
   [CustomValidation(typeof(CRMClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class ActivityPM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  private string activityTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ActivityTypeCode  
	   {
	    
	     get
		{
		   return activityTypeCode;
		 }
		 set
		 {
		   if(activityTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ActivityTypeCode",OldValue=activityTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   activityTypeCode=value;
		   }
			
		 }
	   }
	  private string subject ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Subject  
	   {
	    
	     get
		{
		   return subject;
		 }
		 set
		 {
		   if(subject != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Subject",OldValue=subject,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   subject=value;
		   }
			
		 }
	   }
	  private DateTime? dueDate ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  private string priorityCode ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PriorityCode  
	   {
	    
	     get
		{
		   return priorityCode;
		 }
		 set
		 {
		   if(priorityCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PriorityCode",OldValue=priorityCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   priorityCode=value;
		   }
			
		 }
	   }
	  private string ownerId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string OwnerId  
	   {
	    
	     get
		{
		   return ownerId;
		 }
		 set
		 {
		   if(ownerId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OwnerId",OldValue=ownerId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   ownerId=value;
		   }
			
		 }
	   }
	  private DateTime? startDateTime ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? StartDateTime  
	   {
	    
	     get
		{
		   return startDateTime;
		 }
		 set
		 {
		   if(startDateTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StartDateTime",OldValue=startDateTime,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   startDateTime=value;
		   }
			
		 }
	   }
	  private DateTime? endDateTime ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? EndDateTime  
	   {
	    
	     get
		{
		   return endDateTime;
		 }
		 set
		 {
		   if(endDateTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EndDateTime",OldValue=endDateTime,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   endDateTime=value;
		   }
			
		 }
	   }
	  private string description ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  private string callTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CallTypeCode  
	   {
	    
	     get
		{
		   return callTypeCode;
		 }
		 set
		 {
		   if(callTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CallTypeCode",OldValue=callTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   callTypeCode=value;
		   }
			
		 }
	   }
	  private string callPurpose ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CallPurpose  
	   {
	    
	     get
		{
		   return callPurpose;
		 }
		 set
		 {
		   if(callPurpose != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CallPurpose",OldValue=callPurpose,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   callPurpose=value;
		   }
			
		 }
	   }
	  private string callDetails ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CallDetails  
	   {
	    
	     get
		{
		   return callDetails;
		 }
		 set
		 {
		   if(callDetails != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CallDetails",OldValue=callDetails,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   callDetails=value;
		   }
			
		 }
	   }
	  private string callResult ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CallResult  
	   {
	    
	     get
		{
		   return callResult;
		 }
		 set
		 {
		   if(callResult != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CallResult",OldValue=callResult,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   callResult=value;
		   }
			
		 }
	   }
	  private string phoneNumber ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PhoneNumber  
	   {
	    
	     get
		{
		   return phoneNumber;
		 }
		 set
		 {
		   if(phoneNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PhoneNumber",OldValue=phoneNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   phoneNumber=value;
		   }
			
		 }
	   }
	  private int? duration ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public int? Duration  
	   {
	    
	     get
		{
		   return duration;
		 }
		 set
		 {
		   if(duration != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Duration",OldValue=duration,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   duration=value;
		   }
			
		 }
	   }
	  private string notes ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  private string createdByUserId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  private DateTime createDate ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  private string activityStatusCode ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ActivityStatusCode  
	   {
	    
	     get
		{
		   return activityStatusCode;
		 }
		 set
		 {
		   if(activityStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ActivityStatusCode",OldValue=activityStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   activityStatusCode=value;
		   }
			
		 }
	   }
	  private string updatedByUserId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  private DateTime updateDate ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  private string branchId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string BranchId  
	   {
	    
	     get
		{
		   return branchId;
		 }
		 set
		 {
		   if(branchId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BranchId",OldValue=branchId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   branchId=value;
		   }
			
		 }
	   }
	  private bool allDayEvent ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool AllDayEvent  
	   {
	    
	     get
		{
		   return allDayEvent;
		 }
		 set
		 {
		   if(allDayEvent != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AllDayEvent",OldValue=allDayEvent,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   allDayEvent=value;
		   }
			
		 }
	   }
	  private string activityTimeTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ActivityTimeTypeCode  
	   {
	    
	     get
		{
		   return activityTimeTypeCode;
		 }
		 set
		 {
		   if(activityTimeTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ActivityTimeTypeCode",OldValue=activityTimeTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   activityTimeTypeCode=value;
		   }
			
		 }
	   }
	  private string location ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Location  
	   {
	    
	     get
		{
		   return location;
		 }
		 set
		 {
		   if(location != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Location",OldValue=location,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   location=value;
		   }
			
		 }
	   }
	  private string activityTypeName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ActivityTypeName  
	   {
	    
	     get
		{
		   return activityTypeName;
		 }
		 set
		 {
		   if(activityTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ActivityTypeName",OldValue=activityTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   activityTypeName=value;
		   }
			
		 }
	   }
	  private string searchFields ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  private string activityStatusName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ActivityStatusName  
	   {
	    
	     get
		{
		   return activityStatusName;
		 }
		 set
		 {
		   if(activityStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ActivityStatusName",OldValue=activityStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   activityStatusName=value;
		   }
			
		 }
	   }
	  private string ownerName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string OwnerName  
	   {
	    
	     get
		{
		   return ownerName;
		 }
		 set
		 {
		   if(ownerName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OwnerName",OldValue=ownerName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   ownerName=value;
		   }
			
		 }
	   }
	  private string priorityName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PriorityName  
	   {
	    
	     get
		{
		   return priorityName;
		 }
		 set
		 {
		   if(priorityName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PriorityName",OldValue=priorityName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   priorityName=value;
		   }
			
		 }
	   }
	  private bool isLeftVoiceMail ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsLeftVoiceMail  
	   {
	    
	     get
		{
		   return isLeftVoiceMail;
		 }
		 set
		 {
		   if(isLeftVoiceMail != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsLeftVoiceMail",OldValue=isLeftVoiceMail,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isLeftVoiceMail=value;
		   }
			
		 }
	   }
	  private string outlookId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string OutlookId  
	   {
	    
	     get
		{
		   return outlookId;
		 }
		 set
		 {
		   if(outlookId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OutlookId",OldValue=outlookId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   outlookId=value;
		   }
			
		 }
	   }
	  private bool needSynchronization ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool NeedSynchronization  
	   {
	    
	     get
		{
		   return needSynchronization;
		 }
		 set
		 {
		   if(needSynchronization != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NeedSynchronization",OldValue=needSynchronization,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   needSynchronization=value;
		   }
			
		 }
	   }
	  private bool isMarkedCompleted ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsMarkedCompleted  
	   {
	    
	     get
		{
		   return isMarkedCompleted;
		 }
		 set
		 {
		   if(isMarkedCompleted != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsMarkedCompleted",OldValue=isMarkedCompleted,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isMarkedCompleted=value;
		   }
			
		 }
	   }
	  private bool isOpen ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsOpen  
	   {
	    
	     get
		{
		   return isOpen;
		 }
		 set
		 {
		   if(isOpen != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsOpen",OldValue=isOpen,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isOpen=value;
		   }
			
		 }
	   }
	  private string meetingSummary ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string MeetingSummary  
	   {
	    
	     get
		{
		   return meetingSummary;
		 }
		 set
		 {
		   if(meetingSummary != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MeetingSummary",OldValue=meetingSummary,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   meetingSummary=value;
		   }
			
		 }
	   }
	  private string customerId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  private string opportunityId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string OpportunityId  
	   {
	    
	     get
		{
		   return opportunityId;
		 }
		 set
		 {
		   if(opportunityId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OpportunityId",OldValue=opportunityId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   opportunityId=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field1 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field1  
	   {
	    
	     get
		{
		   return field1;
		 }
		 set
		 {
		   if(field1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field1",OldValue=field1,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field1=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field2 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field2  
	   {
	    
	     get
		{
		   return field2;
		 }
		 set
		 {
		   if(field2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field2",OldValue=field2,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field2=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field3 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field3  
	   {
	    
	     get
		{
		   return field3;
		 }
		 set
		 {
		   if(field3 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field3",OldValue=field3,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field3=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field4 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field4  
	   {
	    
	     get
		{
		   return field4;
		 }
		 set
		 {
		   if(field4 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field4",OldValue=field4,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field4=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field5 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field5  
	   {
	    
	     get
		{
		   return field5;
		 }
		 set
		 {
		   if(field5 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field5",OldValue=field5,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field5=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field6 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field6  
	   {
	    
	     get
		{
		   return field6;
		 }
		 set
		 {
		   if(field6 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field6",OldValue=field6,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field6=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field7 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field7  
	   {
	    
	     get
		{
		   return field7;
		 }
		 set
		 {
		   if(field7 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field7",OldValue=field7,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field7=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field8 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field8  
	   {
	    
	     get
		{
		   return field8;
		 }
		 set
		 {
		   if(field8 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field8",OldValue=field8,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field8=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field9 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field9  
	   {
	    
	     get
		{
		   return field9;
		 }
		 set
		 {
		   if(field9 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field9",OldValue=field9,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field9=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field10 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field10  
	   {
	    
	     get
		{
		   return field10;
		 }
		 set
		 {
		   if(field10 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field10",OldValue=field10,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field10=value;
		   }
			
		 }
	   }
	  private DateTime? completeDate ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? CompleteDate  
	   {
	    
	     get
		{
		   return completeDate;
		 }
		 set
		 {
		   if(completeDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CompleteDate",OldValue=completeDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   completeDate=value;
		   }
			
		 }
	   }
	  private string opportunitySubject ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string OpportunitySubject  
	   {
	    
	     get
		{
		   return opportunitySubject;
		 }
		 set
		 {
		   if(opportunitySubject != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OpportunitySubject",OldValue=opportunitySubject,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   opportunitySubject=value;
		   }
			
		 }
	   }

	   private List<ActivityInviteePM> activityInvitees;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("ActivityInviteeActivity", "Id","ActivityId")]
	   [DataMember]
	   public virtual List<ActivityInviteePM> ActivityInvitees  
	   {
	        get
             {
                 if (activityInvitees == null)
                 {
                     activityInvitees = new List<ActivityInviteePM>();
                 }
                 return activityInvitees;
              }
             set { activityInvitees = value; }
	    }
		   
	   private List<ActivityInviteePM>  deletedActivityInvitees;
	   public virtual List<ActivityInviteePM> DeletedActivityInvitees  
	   {
	        get
             {
                 if ( deletedActivityInvitees == null)
                 {
                      deletedActivityInvitees = new List<ActivityInviteePM>();
                 }
                 return  deletedActivityInvitees;
              }
             set {  deletedActivityInvitees = value; }
	    }
	  	  private string businessUnitId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string BusinessUnitId  
	   {
	    
	     get
		{
		   return businessUnitId;
		 }
		 set
		 {
		   if(businessUnitId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BusinessUnitId",OldValue=businessUnitId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   businessUnitId=value;
		   }
			
		 }
	   }
	  private string senderEmail ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string SenderEmail  
	   {
	    
	     get
		{
		   return senderEmail;
		 }
		 set
		 {
		   if(senderEmail != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SenderEmail",OldValue=senderEmail,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   senderEmail=value;
		   }
			
		 }
	   }
	  private string communicationLogId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CommunicationLogId  
	   {
	    
	     get
		{
		   return communicationLogId;
		 }
		 set
		 {
		   if(communicationLogId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CommunicationLogId",OldValue=communicationLogId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   communicationLogId=value;
		   }
			
		 }
	   }

	   private List<ActivityEmailRecipientPM> activityEmailRecipients;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("ActivityEmailRecipientActivity", "Id","ActivityId")]
	   [DataMember]
	   public virtual List<ActivityEmailRecipientPM> ActivityEmailRecipients  
	   {
	        get
             {
                 if (activityEmailRecipients == null)
                 {
                     activityEmailRecipients = new List<ActivityEmailRecipientPM>();
                 }
                 return activityEmailRecipients;
              }
             set { activityEmailRecipients = value; }
	    }
		   
	   private List<ActivityEmailRecipientPM>  deletedActivityEmailRecipients;
	   public virtual List<ActivityEmailRecipientPM> DeletedActivityEmailRecipients  
	   {
	        get
             {
                 if ( deletedActivityEmailRecipients == null)
                 {
                      deletedActivityEmailRecipients = new List<ActivityEmailRecipientPM>();
                 }
                 return  deletedActivityEmailRecipients;
              }
             set {  deletedActivityEmailRecipients = value; }
	    }
	  
	   private List<ActivityNotePM> activityNotes;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("ActivityNoteActivity", "Id","ActivityId")]
	   [DataMember]
	   public virtual List<ActivityNotePM> ActivityNotes  
	   {
	        get
             {
                 if (activityNotes == null)
                 {
                     activityNotes = new List<ActivityNotePM>();
                 }
                 return activityNotes;
              }
             set { activityNotes = value; }
	    }
		   
	   private List<ActivityNotePM>  deletedActivityNotes;
	   public virtual List<ActivityNotePM> DeletedActivityNotes  
	   {
	        get
             {
                 if ( deletedActivityNotes == null)
                 {
                      deletedActivityNotes = new List<ActivityNotePM>();
                 }
                 return  deletedActivityNotes;
              }
             set {  deletedActivityNotes = value; }
	    }
	  	  private string customerName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  private string senderContactId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string SenderContactId  
	   {
	    
	     get
		{
		   return senderContactId;
		 }
		 set
		 {
		   if(senderContactId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SenderContactId",OldValue=senderContactId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   senderContactId=value;
		   }
			
		 }
	   }
	  private string callWithId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CallWithId  
	   {
	    
	     get
		{
		   return callWithId;
		 }
		 set
		 {
		   if(callWithId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CallWithId",OldValue=callWithId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   callWithId=value;
		   }
			
		 }
	   }
	  private string concurrencyGUID ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConcurrencyGUID  
	   {
	    
	     get
		{
		   return concurrencyGUID;
		 }
		 set
		 {
		   if(concurrencyGUID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConcurrencyGUID",OldValue=concurrencyGUID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   concurrencyGUID=value;
		   }
			
		 }
	   }
	  private string createdByUserName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  private string quoteId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string QuoteId  
	   {
	    
	     get
		{
		   return quoteId;
		 }
		 set
		 {
		   if(quoteId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuoteId",OldValue=quoteId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quoteId=value;
		   }
			
		 }
	   }
	  private string quoteNumber ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string QuoteNumber  
	   {
	    
	     get
		{
		   return quoteNumber;
		 }
		 set
		 {
		   if(quoteNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuoteNumber",OldValue=quoteNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quoteNumber=value;
		   }
			
		 }
	   }
	  private string ticketId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string TicketId  
	   {
	    
	     get
		{
		   return ticketId;
		 }
		 set
		 {
		   if(ticketId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TicketId",OldValue=ticketId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   ticketId=value;
		   }
			
		 }
	   }
	  private DateTime? sortingDate ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? SortingDate  
	   {
	    
	     get
		{
		   return sortingDate;
		 }
		 set
		 {
		   if(sortingDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SortingDate",OldValue=sortingDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   sortingDate=value;
		   }
			
		 }
	   }
	  private string sortingBy ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string SortingBy  
	   {
	    
	     get
		{
		   return sortingBy;
		 }
		 set
		 {
		   if(sortingBy != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SortingBy",OldValue=sortingBy,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   sortingBy=value;
		   }
			
		 }
	   }
	  private DateTime? sendReceiveDate ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? SendReceiveDate  
	   {
	    
	     get
		{
		   return sendReceiveDate;
		 }
		 set
		 {
		   if(sendReceiveDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SendReceiveDate",OldValue=sendReceiveDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   sendReceiveDate=value;
		   }
			
		 }
	   }
	  private string activityWith ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ActivityWith  
	   {
	    
	     get
		{
		   return activityWith;
		 }
		 set
		 {
		   if(activityWith != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ActivityWith",OldValue=activityWith,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   activityWith=value;
		   }
			
		 }
	   }
	  private bool descriptionRightToLeft ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool DescriptionRightToLeft  
	   {
	    
	     get
		{
		   return descriptionRightToLeft;
		 }
		 set
		 {
		   if(descriptionRightToLeft != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DescriptionRightToLeft",OldValue=descriptionRightToLeft,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   descriptionRightToLeft=value;
		   }
			
		 }
	   }
	  private bool meetingSummaryRightToLeft ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool MeetingSummaryRightToLeft  
	   {
	    
	     get
		{
		   return meetingSummaryRightToLeft;
		 }
		 set
		 {
		   if(meetingSummaryRightToLeft != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MeetingSummaryRightToLeft",OldValue=meetingSummaryRightToLeft,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   meetingSummaryRightToLeft=value;
		   }
			
		 }
	   }
	  private string senderContactName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string SenderContactName  
	   {
	    
	     get
		{
		   return senderContactName;
		 }
		 set
		 {
		   if(senderContactName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SenderContactName",OldValue=senderContactName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   senderContactName=value;
		   }
			
		 }
	   }
	  private string originalActivitySubject ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string OriginalActivitySubject  
	   {
	    
	     get
		{
		   return originalActivitySubject;
		 }
		 set
		 {
		   if(originalActivitySubject != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OriginalActivitySubject",OldValue=originalActivitySubject,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   originalActivitySubject=value;
		   }
			
		 }
	   }
	  private bool isHybrid ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsHybrid  
	   {
	    
	     get
		{
		   return isHybrid;
		 }
		 set
		 {
		   if(isHybrid != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsHybrid",OldValue=isHybrid,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isHybrid=value;
		   }
			
		 }
	   }

	   private List<DocumentDataPM> activityDocumentDatas;
	 
	   [DataMember]
	   public virtual List<DocumentDataPM> ActivityDocumentDatas  
	   {
	        get
             {
                 if (activityDocumentDatas == null)
                 {
                     activityDocumentDatas = new List<DocumentDataPM>();
                 }
                 return activityDocumentDatas;
              }
             set { activityDocumentDatas = value; }
	    }
		   
	   private List<DocumentDataPM>  deletedActivityDocumentDatas;
	   public virtual List<DocumentDataPM> DeletedActivityDocumentDatas  
	   {
	        get
             {
                 if ( deletedActivityDocumentDatas == null)
                 {
                      deletedActivityDocumentDatas = new List<DocumentDataPM>();
                 }
                 return  deletedActivityDocumentDatas;
              }
             set {  deletedActivityDocumentDatas = value; }
	    }
	  	  private bool isCustomerBlockedBusinessUnit ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCustomerBlockedBusinessUnit  
	   {
	    
	     get
		{
		   return isCustomerBlockedBusinessUnit;
		 }
		 set
		 {
		   if(isCustomerBlockedBusinessUnit != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCustomerBlockedBusinessUnit",OldValue=isCustomerBlockedBusinessUnit,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCustomerBlockedBusinessUnit=value;
		   }
			
		 }
	   }
	  private bool isCopy ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCopy  
	   {
	    
	     get
		{
		   return isCopy;
		 }
		 set
		 {
		   if(isCopy != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCopy",OldValue=isCopy,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCopy=value;
		   }
			
		 }
	   }
	  private byte[] lastModified ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public byte[] LastModified  
	   {
	    
	     get
		{
		   return lastModified;
		 }
		 set
		 {
		   if(lastModified != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastModified",OldValue=lastModified,NewValue=value,PropertyType="byte[]"};
		    NotifyPropertyChanged(values);
		   lastModified=value;
		   }
			
		 }
	   }
	  private bool dontSetNeedSynchronization ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool DontSetNeedSynchronization  
	   {
	    
	     get
		{
		   return dontSetNeedSynchronization;
		 }
		 set
		 {
		   if(dontSetNeedSynchronization != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DontSetNeedSynchronization",OldValue=dontSetNeedSynchronization,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   dontSetNeedSynchronization=value;
		   }
			
		 }
	   }
	  private string activityTypePathCode ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ActivityTypePathCode  
	   {
	    
	     get
		{
		   return activityTypePathCode;
		 }
		 set
		 {
		   if(activityTypePathCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ActivityTypePathCode",OldValue=activityTypePathCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   activityTypePathCode=value;
		   }
			
		 }
	   }
	  private string objectTableName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  private string from ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string From  
	   {
	    
	     get
		{
		   return from;
		 }
		 set
		 {
		   if(from != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="From",OldValue=from,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   from=value;
		   }
			
		 }
	   }
	  private string to ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string To  
	   {
	    
	     get
		{
		   return to;
		 }
		 set
		 {
		   if(to != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="To",OldValue=to,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   to=value;
		   }
			
		 }
	   }
	  private string cc ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Cc  
	   {
	    
	     get
		{
		   return cc;
		 }
		 set
		 {
		   if(cc != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Cc",OldValue=cc,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cc=value;
		   }
			
		 }
	   }
	  private DateTime? archiveDate ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ArchiveDate  
	   {
	    
	     get
		{
		   return archiveDate;
		 }
		 set
		 {
		   if(archiveDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ArchiveDate",OldValue=archiveDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   archiveDate=value;
		   }
			
		 }
	   }
	  private int? dueDateOffset ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public int? DueDateOffset  
	   {
	    
	     get
		{
		   return dueDateOffset;
		 }
		 set
		 {
		   if(dueDateOffset != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DueDateOffset",OldValue=dueDateOffset,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   dueDateOffset=value;
		   }
			
		 }
	   }
	  private string dueDateDateField ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string DueDateDateField  
	   {
	    
	     get
		{
		   return dueDateDateField;
		 }
		 set
		 {
		   if(dueDateDateField != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DueDateDateField",OldValue=dueDateDateField,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   dueDateDateField=value;
		   }
			
		 }
	   }
	  private string businessProcessQueueId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string BusinessProcessQueueId  
	   {
	    
	     get
		{
		   return businessProcessQueueId;
		 }
		 set
		 {
		   if(businessProcessQueueId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BusinessProcessQueueId",OldValue=businessProcessQueueId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   businessProcessQueueId=value;
		   }
			
		 }
	   }
	  private string teamId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string TeamId  
	   {
	    
	     get
		{
		   return teamId;
		 }
		 set
		 {
		   if(teamId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TeamId",OldValue=teamId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   teamId=value;
		   }
			
		 }
	   }
	  private string shipmentId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipmentId  
	   {
	    
	     get
		{
		   return shipmentId;
		 }
		 set
		 {
		   if(shipmentId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipmentId",OldValue=shipmentId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipmentId=value;
		   }
			
		 }
	   }
   }
   
}
	 