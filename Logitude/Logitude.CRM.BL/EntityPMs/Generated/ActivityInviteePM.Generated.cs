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
   public partial class ActivityInviteePM : EntityPM
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
	  private string activityId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ActivityId  
	   {
	    
	     get
		{
		   return activityId;
		 }
		 set
		 {
		   if(activityId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ActivityId",OldValue=activityId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   activityId=value;
		   }
			
		 }
	   }
	  private string contactId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContactId  
	   {
	    
	     get
		{
		   return contactId;
		 }
		 set
		 {
		   if(contactId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContactId",OldValue=contactId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   contactId=value;
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
	  private string email ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Email  
	   {
	    
	     get
		{
		   return email;
		 }
		 set
		 {
		   if(email != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Email",OldValue=email,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   email=value;
		   }
			
		 }
	   }
	  private bool isRequired ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsRequired  
	   {
	    
	     get
		{
		   return isRequired;
		 }
		 set
		 {
		   if(isRequired != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsRequired",OldValue=isRequired,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isRequired=value;
		   }
			
		 }
	   }
	  private string contactName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContactName  
	   {
	    
	     get
		{
		   return contactName;
		 }
		 set
		 {
		   if(contactName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContactName",OldValue=contactName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   contactName=value;
		   }
			
		 }
	   }
   }
   
}
	 