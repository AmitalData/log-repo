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
   public partial class TMProjectPM : EntityPM
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
	  private string name ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public string Name  
	   {
	    
	     get
		{
		   return name;
		 }
		 set
		 {
		   if(name != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Name",OldValue=name,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   name=value;
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
	  private string customerId ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
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
	  private string ownerId ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
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
	  private string projectNumber ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public string ProjectNumber  
	   {
	    
	     get
		{
		   return projectNumber;
		 }
		 set
		 {
		   if(projectNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ProjectNumber",OldValue=projectNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   projectNumber=value;
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
	  private string ownerName ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
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
	  private string customerName ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
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
	  private bool isInnerProject ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsInnerProject  
	   {
	    
	     get
		{
		   return isInnerProject;
		 }
		 set
		 {
		   if(isInnerProject != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsInnerProject",OldValue=isInnerProject,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isInnerProject=value;
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
	  private string budgetId ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public string BudgetId  
	   {
	    
	     get
		{
		   return budgetId;
		 }
		 set
		 {
		   if(budgetId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BudgetId",OldValue=budgetId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   budgetId=value;
		   }
			
		 }
	   }
	  private string categoryId ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public string CategoryId  
	   {
	    
	     get
		{
		   return categoryId;
		 }
		 set
		 {
		   if(categoryId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CategoryId",OldValue=categoryId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   categoryId=value;
		   }
			
		 }
	   }
	  private bool isProrated ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsProrated  
	   {
	    
	     get
		{
		   return isProrated;
		 }
		 set
		 {
		   if(isProrated != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsProrated",OldValue=isProrated,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isProrated=value;
		   }
			
		 }
	   }
	  private string externalProjectNumber ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExternalProjectNumber  
	   {
	    
	     get
		{
		   return externalProjectNumber;
		 }
		 set
		 {
		   if(externalProjectNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExternalProjectNumber",OldValue=externalProjectNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   externalProjectNumber=value;
		   }
			
		 }
	   }
	  private string categoryName ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public string CategoryName  
	   {
	    
	     get
		{
		   return categoryName;
		 }
		 set
		 {
		   if(categoryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CategoryName",OldValue=categoryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   categoryName=value;
		   }
			
		 }
	   }
	  private bool excludeFromProrating ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ExcludeFromProrating  
	   {
	    
	     get
		{
		   return excludeFromProrating;
		 }
		 set
		 {
		   if(excludeFromProrating != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExcludeFromProrating",OldValue=excludeFromProrating,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   excludeFromProrating=value;
		   }
			
		 }
	   }
	  private string dayOffTypeCode ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public string DayOffTypeCode  
	   {
	    
	     get
		{
		   return dayOffTypeCode;
		 }
		 set
		 {
		   if(dayOffTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DayOffTypeCode",OldValue=dayOffTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   dayOffTypeCode=value;
		   }
			
		 }
	   }
	  private bool blockedForDataEntry ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public bool BlockedForDataEntry  
	   {
	    
	     get
		{
		   return blockedForDataEntry;
		 }
		 set
		 {
		   if(blockedForDataEntry != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BlockedForDataEntry",OldValue=blockedForDataEntry,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   blockedForDataEntry=value;
		   }
			
		 }
	   }
   }
   
}
	 