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
using Logitude.Workflow.BL.Validators;
  
namespace Logitude.Workflow.BL.EntityPMs
{
   [CustomValidation(typeof(WorkflowClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class WorkFlowInstancePM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
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
	  private string statusCode ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
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
	  private DateTime? startTime ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? StartTime  
	   {
	    
	     get
		{
		   return startTime;
		 }
		 set
		 {
		   if(startTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StartTime",OldValue=startTime,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   startTime=value;
		   }
			
		 }
	   }
	  private DateTime? endTime ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? EndTime  
	   {
	    
	     get
		{
		   return endTime;
		 }
		 set
		 {
		   if(endTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EndTime",OldValue=endTime,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   endTime=value;
		   }
			
		 }
	   }
	  private string businessKey ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public string BusinessKey  
	   {
	    
	     get
		{
		   return businessKey;
		 }
		 set
		 {
		   if(businessKey != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BusinessKey",OldValue=businessKey,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   businessKey=value;
		   }
			
		 }
	   }
	  private decimal? duration ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Duration  
	   {
	    
	     get
		{
		   return duration;
		 }
		 set
		 {
		   if(duration != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Duration",OldValue=duration,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   duration=value;
		   }
			
		 }
	   }
	  private string statusName ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
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
	  private string workFlowVersionId ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public string WorkFlowVersionId  
	   {
	    
	     get
		{
		   return workFlowVersionId;
		 }
		 set
		 {
		   if(workFlowVersionId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="WorkFlowVersionId",OldValue=workFlowVersionId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   workFlowVersionId=value;
		   }
			
		 }
	   }
	  private int workFlowVersionNumber ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public int WorkFlowVersionNumber  
	   {
	    
	     get
		{
		   return workFlowVersionNumber;
		 }
		 set
		 {
		   if(workFlowVersionNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="WorkFlowVersionNumber",OldValue=workFlowVersionNumber,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   workFlowVersionNumber=value;
		   }
			
		 }
	   }
	  private int retryAttemptsNumber ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public int RetryAttemptsNumber  
	   {
	    
	     get
		{
		   return retryAttemptsNumber;
		 }
		 set
		 {
		   if(retryAttemptsNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RetryAttemptsNumber",OldValue=retryAttemptsNumber,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   retryAttemptsNumber=value;
		   }
			
		 }
	   }
	  private string workflowId ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public string WorkflowId  
	   {
	    
	     get
		{
		   return workflowId;
		 }
		 set
		 {
		   if(workflowId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="WorkflowId",OldValue=workflowId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   workflowId=value;
		   }
			
		 }
	   }
	  private string createdByUserName ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
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
	  private int numberOfActivities ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public int NumberOfActivities  
	   {
	    
	     get
		{
		   return numberOfActivities;
		 }
		 set
		 {
		   if(numberOfActivities != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NumberOfActivities",OldValue=numberOfActivities,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   numberOfActivities=value;
		   }
			
		 }
	   }
	    }
   
}
	 