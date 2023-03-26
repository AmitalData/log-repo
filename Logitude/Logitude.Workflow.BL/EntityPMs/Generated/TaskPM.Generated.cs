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
   public partial class TaskPM : EntityPM
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
	  private string subject ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
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
	  private DateTime dueDate ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
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
	  private string ownerId ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
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
	  private string priorityId ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public string PriorityId  
	   {
	    
	     get
		{
		   return priorityId;
		 }
		 set
		 {
		   if(priorityId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PriorityId",OldValue=priorityId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   priorityId=value;
		   }
			
		 }
	   }
	  private string statusId ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public string StatusId  
	   {
	    
	     get
		{
		   return statusId;
		 }
		 set
		 {
		   if(statusId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StatusId",OldValue=statusId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   statusId=value;
		   }
			
		 }
	   }
	  private string entityId ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
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
	  private string taskTypeId ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public string TaskTypeId  
	   {
	    
	     get
		{
		   return taskTypeId;
		 }
		 set
		 {
		   if(taskTypeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TaskTypeId",OldValue=taskTypeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   taskTypeId=value;
		   }
			
		 }
	   }
	  private string entityObjectTableId ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntityObjectTableId  
	   {
	    
	     get
		{
		   return entityObjectTableId;
		 }
		 set
		 {
		   if(entityObjectTableId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntityObjectTableId",OldValue=entityObjectTableId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entityObjectTableId=value;
		   }
			
		 }
	   }
	  private string closedByUserId ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public string ClosedByUserId  
	   {
	    
	     get
		{
		   return closedByUserId;
		 }
		 set
		 {
		   if(closedByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClosedByUserId",OldValue=closedByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   closedByUserId=value;
		   }
			
		 }
	   }
	  private DateTime? closedDate ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ClosedDate  
	   {
	    
	     get
		{
		   return closedDate;
		 }
		 set
		 {
		   if(closedDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClosedDate",OldValue=closedDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   closedDate=value;
		   }
			
		 }
	   }
	  private bool isClosed ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsClosed  
	   {
	    
	     get
		{
		   return isClosed;
		 }
		 set
		 {
		   if(isClosed != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsClosed",OldValue=isClosed,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isClosed=value;
		   }
			
		 }
	   }
	  private bool isCancelled ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
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
	  private string checkWithId ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public string CheckWithId  
	   {
	    
	     get
		{
		   return checkWithId;
		 }
		 set
		 {
		   if(checkWithId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CheckWithId",OldValue=checkWithId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   checkWithId=value;
		   }
			
		 }
	   }
	  private string entityNumber ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntityNumber  
	   {
	    
	     get
		{
		   return entityNumber;
		 }
		 set
		 {
		   if(entityNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntityNumber",OldValue=entityNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entityNumber=value;
		   }
			
		 }
	   }
	  private string ownerName ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
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
	  private string taskTypeName ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public string TaskTypeName  
	   {
	    
	     get
		{
		   return taskTypeName;
		 }
		 set
		 {
		   if(taskTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TaskTypeName",OldValue=taskTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   taskTypeName=value;
		   }
			
		 }
	   }
	  private string entityObjectTableName ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntityObjectTableName  
	   {
	    
	     get
		{
		   return entityObjectTableName;
		 }
		 set
		 {
		   if(entityObjectTableName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntityObjectTableName",OldValue=entityObjectTableName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entityObjectTableName=value;
		   }
			
		 }
	   }
	  private string closedByUserName ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public string ClosedByUserName  
	   {
	    
	     get
		{
		   return closedByUserName;
		 }
		 set
		 {
		   if(closedByUserName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClosedByUserName",OldValue=closedByUserName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   closedByUserName=value;
		   }
			
		 }
	   }
	  private string checkWithName ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public string CheckWithName  
	   {
	    
	     get
		{
		   return checkWithName;
		 }
		 set
		 {
		   if(checkWithName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CheckWithName",OldValue=checkWithName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   checkWithName=value;
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
	  private string fields ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public string Fields  
	   {
	    
	     get
		{
		   return fields;
		 }
		 set
		 {
		   if(fields != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Fields",OldValue=fields,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fields=value;
		   }
			
		 }
	   }
	  private string toDoConditions ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToDoConditions  
	   {
	    
	     get
		{
		   return toDoConditions;
		 }
		 set
		 {
		   if(toDoConditions != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToDoConditions",OldValue=toDoConditions,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toDoConditions=value;
		   }
			
		 }
	   }
	  private string doneConditions ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public string DoneConditions  
	   {
	    
	     get
		{
		   return doneConditions;
		 }
		 set
		 {
		   if(doneConditions != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DoneConditions",OldValue=doneConditions,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   doneConditions=value;
		   }
			
		 }
	   }
	  private string description ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
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
	  private bool isAssigned ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsAssigned  
	   {
	    
	     get
		{
		   return isAssigned;
		 }
		 set
		 {
		   if(isAssigned != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsAssigned",OldValue=isAssigned,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isAssigned=value;
		   }
			
		 }
	   }
	  private DateTime? startDate ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? StartDate  
	   {
	    
	     get
		{
		   return startDate;
		 }
		 set
		 {
		   if(startDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StartDate",OldValue=startDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   startDate=value;
		   }
			
		 }
	   }
	    }
   
}
	 