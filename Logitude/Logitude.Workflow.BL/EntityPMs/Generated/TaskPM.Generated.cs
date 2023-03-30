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
using Logitude.BL.InfrastructureModel.EntityPMs;
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

	   private List<CustomChildEntity> customChildEntities;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public List<CustomChildEntity> CustomChildEntities
	   {
	     get { return customChildEntities; }
		 set {
		   if(customChildEntities == value) return;
		   NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomChildEntities",OldValue=customChildEntities,NewValue=value,PropertyType="List<CustomChildEntity>"};
		   NotifyPropertyChanged(values);
		   customChildEntities=value;
		 }
	   }
	 	   private CustomFieldClass field1;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
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
	   private CustomFieldClass field2;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
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
	   private CustomFieldClass field3;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
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
	   private CustomFieldClass field4;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
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
	   private CustomFieldClass field5;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
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
	   private CustomFieldClass field6;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
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
	   private CustomFieldClass field7;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
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
	   private CustomFieldClass field8;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
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
	   private CustomFieldClass field9;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
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
	   private CustomFieldClass field10;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
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
	   private CustomFieldClass field11;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field11  
	   {
	    
	     get
		{
		   return field11;
		 }
		 set
		 {
		   if(field11 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field11",OldValue=field11,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field11=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field12;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field12  
	   {
	    
	     get
		{
		   return field12;
		 }
		 set
		 {
		   if(field12 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field12",OldValue=field12,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field12=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field13;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field13  
	   {
	    
	     get
		{
		   return field13;
		 }
		 set
		 {
		   if(field13 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field13",OldValue=field13,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field13=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field14;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field14  
	   {
	    
	     get
		{
		   return field14;
		 }
		 set
		 {
		   if(field14 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field14",OldValue=field14,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field14=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field15;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field15  
	   {
	    
	     get
		{
		   return field15;
		 }
		 set
		 {
		   if(field15 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field15",OldValue=field15,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field15=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field16;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field16  
	   {
	    
	     get
		{
		   return field16;
		 }
		 set
		 {
		   if(field16 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field16",OldValue=field16,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field16=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field17;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field17  
	   {
	    
	     get
		{
		   return field17;
		 }
		 set
		 {
		   if(field17 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field17",OldValue=field17,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field17=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field18;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field18  
	   {
	    
	     get
		{
		   return field18;
		 }
		 set
		 {
		   if(field18 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field18",OldValue=field18,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field18=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field19;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field19  
	   {
	    
	     get
		{
		   return field19;
		 }
		 set
		 {
		   if(field19 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field19",OldValue=field19,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field19=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field20;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field20  
	   {
	    
	     get
		{
		   return field20;
		 }
		 set
		 {
		   if(field20 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field20",OldValue=field20,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field20=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field21;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field21  
	   {
	    
	     get
		{
		   return field21;
		 }
		 set
		 {
		   if(field21 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field21",OldValue=field21,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field21=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field22;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field22  
	   {
	    
	     get
		{
		   return field22;
		 }
		 set
		 {
		   if(field22 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field22",OldValue=field22,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field22=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field23;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field23  
	   {
	    
	     get
		{
		   return field23;
		 }
		 set
		 {
		   if(field23 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field23",OldValue=field23,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field23=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field24;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field24  
	   {
	    
	     get
		{
		   return field24;
		 }
		 set
		 {
		   if(field24 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field24",OldValue=field24,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field24=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field25;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field25  
	   {
	    
	     get
		{
		   return field25;
		 }
		 set
		 {
		   if(field25 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field25",OldValue=field25,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field25=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field26;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field26  
	   {
	    
	     get
		{
		   return field26;
		 }
		 set
		 {
		   if(field26 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field26",OldValue=field26,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field26=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field27;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field27  
	   {
	    
	     get
		{
		   return field27;
		 }
		 set
		 {
		   if(field27 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field27",OldValue=field27,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field27=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field28;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field28  
	   {
	    
	     get
		{
		   return field28;
		 }
		 set
		 {
		   if(field28 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field28",OldValue=field28,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field28=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field29;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field29  
	   {
	    
	     get
		{
		   return field29;
		 }
		 set
		 {
		   if(field29 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field29",OldValue=field29,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field29=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field30;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field30  
	   {
	    
	     get
		{
		   return field30;
		 }
		 set
		 {
		   if(field30 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field30",OldValue=field30,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field30=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field31;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field31  
	   {
	    
	     get
		{
		   return field31;
		 }
		 set
		 {
		   if(field31 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field31",OldValue=field31,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field31=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field32;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field32  
	   {
	    
	     get
		{
		   return field32;
		 }
		 set
		 {
		   if(field32 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field32",OldValue=field32,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field32=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field33;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field33  
	   {
	    
	     get
		{
		   return field33;
		 }
		 set
		 {
		   if(field33 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field33",OldValue=field33,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field33=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field34;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field34  
	   {
	    
	     get
		{
		   return field34;
		 }
		 set
		 {
		   if(field34 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field34",OldValue=field34,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field34=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field35;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field35  
	   {
	    
	     get
		{
		   return field35;
		 }
		 set
		 {
		   if(field35 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field35",OldValue=field35,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field35=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field36;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field36  
	   {
	    
	     get
		{
		   return field36;
		 }
		 set
		 {
		   if(field36 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field36",OldValue=field36,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field36=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field37;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field37  
	   {
	    
	     get
		{
		   return field37;
		 }
		 set
		 {
		   if(field37 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field37",OldValue=field37,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field37=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field38;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field38  
	   {
	    
	     get
		{
		   return field38;
		 }
		 set
		 {
		   if(field38 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field38",OldValue=field38,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field38=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field39;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field39  
	   {
	    
	     get
		{
		   return field39;
		 }
		 set
		 {
		   if(field39 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field39",OldValue=field39,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field39=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field40;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field40  
	   {
	    
	     get
		{
		   return field40;
		 }
		 set
		 {
		   if(field40 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field40",OldValue=field40,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field40=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field41;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field41  
	   {
	    
	     get
		{
		   return field41;
		 }
		 set
		 {
		   if(field41 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field41",OldValue=field41,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field41=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field42;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field42  
	   {
	    
	     get
		{
		   return field42;
		 }
		 set
		 {
		   if(field42 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field42",OldValue=field42,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field42=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field43;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field43  
	   {
	    
	     get
		{
		   return field43;
		 }
		 set
		 {
		   if(field43 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field43",OldValue=field43,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field43=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field44;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field44  
	   {
	    
	     get
		{
		   return field44;
		 }
		 set
		 {
		   if(field44 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field44",OldValue=field44,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field44=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field45;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field45  
	   {
	    
	     get
		{
		   return field45;
		 }
		 set
		 {
		   if(field45 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field45",OldValue=field45,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field45=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field46;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field46  
	   {
	    
	     get
		{
		   return field46;
		 }
		 set
		 {
		   if(field46 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field46",OldValue=field46,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field46=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field47;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field47  
	   {
	    
	     get
		{
		   return field47;
		 }
		 set
		 {
		   if(field47 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field47",OldValue=field47,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field47=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field48;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field48  
	   {
	    
	     get
		{
		   return field48;
		 }
		 set
		 {
		   if(field48 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field48",OldValue=field48,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field48=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field49;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field49  
	   {
	    
	     get
		{
		   return field49;
		 }
		 set
		 {
		   if(field49 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field49",OldValue=field49,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field49=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field50;
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field50  
	   {
	    
	     get
		{
		   return field50;
		 }
		 set
		 {
		   if(field50 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field50",OldValue=field50,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field50=value;
		   }
			
		 }
	   }
   }
   
}
	 