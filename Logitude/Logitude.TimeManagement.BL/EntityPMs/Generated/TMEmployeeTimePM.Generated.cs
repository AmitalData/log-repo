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
using Logitude.TimeManagement.BL.Validators;
  
namespace Logitude.TimeManagement.BL.EntityPMs
{
   [CustomValidation(typeof(TimeManagementClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class TMEmployeeTimePM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
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
	  private string employeeUserId ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public string EmployeeUserId  
	   {
	    
	     get
		{
		   return employeeUserId;
		 }
		 set
		 {
		   if(employeeUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EmployeeUserId",OldValue=employeeUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   employeeUserId=value;
		   }
			
		 }
	   }
	  private DateTime dateOfWork ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime DateOfWork  
	   {
	    
	     get
		{
		   return dateOfWork;
		 }
		 set
		 {
		   if(dateOfWork != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DateOfWork",OldValue=dateOfWork,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   dateOfWork=value;
		   }
			
		 }
	   }
	  private string description ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
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
	  private int timeInMinutes ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public int TimeInMinutes  
	   {
	    
	     get
		{
		   return timeInMinutes;
		 }
		 set
		 {
		   if(timeInMinutes != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TimeInMinutes",OldValue=timeInMinutes,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   timeInMinutes=value;
		   }
			
		 }
	   }
	  private string wINumber ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public string WINumber  
	   {
	    
	     get
		{
		   return wINumber;
		 }
		 set
		 {
		   if(wINumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="WINumber",OldValue=wINumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   wINumber=value;
		   }
			
		 }
	   }
	  private string projectId ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public string ProjectId  
	   {
	    
	     get
		{
		   return projectId;
		 }
		 set
		 {
		   if(projectId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ProjectId",OldValue=projectId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   projectId=value;
		   }
			
		 }
	   }
	  private string locationCode ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public string LocationCode  
	   {
	    
	     get
		{
		   return locationCode;
		 }
		 set
		 {
		   if(locationCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LocationCode",OldValue=locationCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   locationCode=value;
		   }
			
		 }
	   }
	  private string projectName ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public string ProjectName  
	   {
	    
	     get
		{
		   return projectName;
		 }
		 set
		 {
		   if(projectName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ProjectName",OldValue=projectName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   projectName=value;
		   }
			
		 }
	   }
	  private string projectDescription ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public string ProjectDescription  
	   {
	    
	     get
		{
		   return projectDescription;
		 }
		 set
		 {
		   if(projectDescription != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ProjectDescription",OldValue=projectDescription,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   projectDescription=value;
		   }
			
		 }
	   }
	  private string analyzeQueueId ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public string AnalyzeQueueId  
	   {
	    
	     get
		{
		   return analyzeQueueId;
		 }
		 set
		 {
		   if(analyzeQueueId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AnalyzeQueueId",OldValue=analyzeQueueId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   analyzeQueueId=value;
		   }
			
		 }
	   }
	  private string projectId_db ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public string ProjectId_db  
	   {
	    
	     get
		{
		   return projectId_db;
		 }
		 set
		 {
		   if(projectId_db != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ProjectId_db",OldValue=projectId_db,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   projectId_db=value;
		   }
			
		 }
	   }
	  private string description_db ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public string Description_db  
	   {
	    
	     get
		{
		   return description_db;
		 }
		 set
		 {
		   if(description_db != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Description_db",OldValue=description_db,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   description_db=value;
		   }
			
		 }
	   }
	  private string wINumber_db ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public string WINumber_db  
	   {
	    
	     get
		{
		   return wINumber_db;
		 }
		 set
		 {
		   if(wINumber_db != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="WINumber_db",OldValue=wINumber_db,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   wINumber_db=value;
		   }
			
		 }
	   }
	  private int timeInMinutes_db ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public int TimeInMinutes_db  
	   {
	    
	     get
		{
		   return timeInMinutes_db;
		 }
		 set
		 {
		   if(timeInMinutes_db != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TimeInMinutes_db",OldValue=timeInMinutes_db,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   timeInMinutes_db=value;
		   }
			
		 }
	   }
	  private string sprintId ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public string SprintId  
	   {
	    
	     get
		{
		   return sprintId;
		 }
		 set
		 {
		   if(sprintId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SprintId",OldValue=sprintId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   sprintId=value;
		   }
			
		 }
	   }
	  private double proratedDuration ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public double ProratedDuration  
	   {
	    
	     get
		{
		   return proratedDuration;
		 }
		 set
		 {
		   if(proratedDuration != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ProratedDuration",OldValue=proratedDuration,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   proratedDuration=value;
		   }
			
		 }
	   }
	  private double fullDuration ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public double FullDuration  
	   {
	    
	     get
		{
		   return fullDuration;
		 }
		 set
		 {
		   if(fullDuration != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FullDuration",OldValue=fullDuration,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   fullDuration=value;
		   }
			
		 }
	   }
	  private bool needsProrating ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public bool NeedsProrating  
	   {
	    
	     get
		{
		   return needsProrating;
		 }
		 set
		 {
		   if(needsProrating != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NeedsProrating",OldValue=needsProrating,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   needsProrating=value;
		   }
			
		 }
	   }
	  private string locationName ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public string LocationName  
	   {
	    
	     get
		{
		   return locationName;
		 }
		 set
		 {
		   if(locationName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LocationName",OldValue=locationName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   locationName=value;
		   }
			
		 }
	   }
	  private string sprintName ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public string SprintName  
	   {
	    
	     get
		{
		   return sprintName;
		 }
		 set
		 {
		   if(sprintName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SprintName",OldValue=sprintName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   sprintName=value;
		   }
			
		 }
	   }
   }
   
}
	 