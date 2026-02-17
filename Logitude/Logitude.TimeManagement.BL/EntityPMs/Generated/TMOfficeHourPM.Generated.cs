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
   public partial class TMOfficeHourPM : EntityPM
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
	  private string searchFields ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
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
	  private string userId ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public string UserId  
	   {
	    
	     get
		{
		   return userId;
		 }
		 set
		 {
		   if(userId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UserId",OldValue=userId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   userId=value;
		   }
			
		 }
	   }
	  private DateTime workDate ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime WorkDate  
	   {
	    
	     get
		{
		   return workDate;
		 }
		 set
		 {
		   if(workDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="WorkDate",OldValue=workDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   workDate=value;
		   }
			
		 }
	   }
	  private DateTime? recordedEntryTime ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? RecordedEntryTime  
	   {
	    
	     get
		{
		   return recordedEntryTime;
		 }
		 set
		 {
		   if(recordedEntryTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RecordedEntryTime",OldValue=recordedEntryTime,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   recordedEntryTime=value;
		   }
			
		 }
	   }
	  private DateTime? recordedExitTime ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? RecordedExitTime  
	   {
	    
	     get
		{
		   return recordedExitTime;
		 }
		 set
		 {
		   if(recordedExitTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RecordedExitTime",OldValue=recordedExitTime,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   recordedExitTime=value;
		   }
			
		 }
	   }
	  private DateTime? entryTime ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? EntryTime  
	   {
	    
	     get
		{
		   return entryTime;
		 }
		 set
		 {
		   if(entryTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntryTime",OldValue=entryTime,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   entryTime=value;
		   }
			
		 }
	   }
	  private DateTime? exitTime ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ExitTime  
	   {
	    
	     get
		{
		   return exitTime;
		 }
		 set
		 {
		   if(exitTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExitTime",OldValue=exitTime,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   exitTime=value;
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
	  private bool inactive ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public bool Inactive  
	   {
	    
	     get
		{
		   return inactive;
		 }
		 set
		 {
		   if(inactive != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Inactive",OldValue=inactive,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   inactive=value;
		   }
			
		 }
	   }
	  private double minutes ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public double Minutes  
	   {
	    
	     get
		{
		   return minutes;
		 }
		 set
		 {
		   if(minutes != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Minutes",OldValue=minutes,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   minutes=value;
		   }
			
		 }
	   }
	  private string updatedByUserName ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
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
   }
   
}
	 