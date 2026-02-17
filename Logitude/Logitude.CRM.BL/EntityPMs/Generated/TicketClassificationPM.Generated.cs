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
   public partial class TicketClassificationPM : EntityPM
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
	  private string name ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  private bool inactive ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  private string parentId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ParentId  
	   {
	    
	     get
		{
		   return parentId;
		 }
		 set
		 {
		   if(parentId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ParentId",OldValue=parentId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   parentId=value;
		   }
			
		 }
	   }
	  private string parentName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ParentName  
	   {
	    
	     get
		{
		   return parentName;
		 }
		 set
		 {
		   if(parentName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ParentName",OldValue=parentName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   parentName=value;
		   }
			
		 }
	   }
	  private string defaultSeverityId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string DefaultSeverityId  
	   {
	    
	     get
		{
		   return defaultSeverityId;
		 }
		 set
		 {
		   if(defaultSeverityId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DefaultSeverityId",OldValue=defaultSeverityId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   defaultSeverityId=value;
		   }
			
		 }
	   }
	  private string employeeGroupId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string EmployeeGroupId  
	   {
	    
	     get
		{
		   return employeeGroupId;
		 }
		 set
		 {
		   if(employeeGroupId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EmployeeGroupId",OldValue=employeeGroupId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   employeeGroupId=value;
		   }
			
		 }
	   }
	  private string managerUserId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ManagerUserId  
	   {
	    
	     get
		{
		   return managerUserId;
		 }
		 set
		 {
		   if(managerUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ManagerUserId",OldValue=managerUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   managerUserId=value;
		   }
			
		 }
	   }
	  private string escalationNotify ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string EscalationNotify  
	   {
	    
	     get
		{
		   return escalationNotify;
		 }
		 set
		 {
		   if(escalationNotify != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EscalationNotify",OldValue=escalationNotify,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   escalationNotify=value;
		   }
			
		 }
	   }
	  private string managerUserEmail ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ManagerUserEmail  
	   {
	    
	     get
		{
		   return managerUserEmail;
		 }
		 set
		 {
		   if(managerUserEmail != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ManagerUserEmail",OldValue=managerUserEmail,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   managerUserEmail=value;
		   }
			
		 }
	   }
   }
   
}
	 