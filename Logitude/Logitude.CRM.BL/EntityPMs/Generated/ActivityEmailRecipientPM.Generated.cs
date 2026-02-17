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
   public partial class ActivityEmailRecipientPM : EntityPM
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
	  private string recipientTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string RecipientTypeCode  
	   {
	    
	     get
		{
		   return recipientTypeCode;
		 }
		 set
		 {
		   if(recipientTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RecipientTypeCode",OldValue=recipientTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   recipientTypeCode=value;
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
   }
   
}
	 