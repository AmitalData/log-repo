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
   public partial class ServiceProviderSubscriptionPM : EntityPM
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
	  private string userEmail ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public string UserEmail  
	   {
	    
	     get
		{
		   return userEmail;
		 }
		 set
		 {
		   if(userEmail != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UserEmail",OldValue=userEmail,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   userEmail=value;
		   }
			
		 }
	   }
	  private string accessToken ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccessToken  
	   {
	    
	     get
		{
		   return accessToken;
		 }
		 set
		 {
		   if(accessToken != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccessToken",OldValue=accessToken,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accessToken=value;
		   }
			
		 }
	   }
	  private string refreshToken ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public string RefreshToken  
	   {
	    
	     get
		{
		   return refreshToken;
		 }
		 set
		 {
		   if(refreshToken != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RefreshToken",OldValue=refreshToken,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   refreshToken=value;
		   }
			
		 }
	   }
	  private string emailProvider ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public string EmailProvider  
	   {
	    
	     get
		{
		   return emailProvider;
		 }
		 set
		 {
		   if(emailProvider != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EmailProvider",OldValue=emailProvider,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   emailProvider=value;
		   }
			
		 }
	   }
	  private string workflowNumber ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public string WorkflowNumber  
	   {
	    
	     get
		{
		   return workflowNumber;
		 }
		 set
		 {
		   if(workflowNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="WorkflowNumber",OldValue=workflowNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   workflowNumber=value;
		   }
			
		 }
	   }
	  private string additionalSettings ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public string AdditionalSettings  
	   {
	    
	     get
		{
		   return additionalSettings;
		 }
		 set
		 {
		   if(additionalSettings != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AdditionalSettings",OldValue=additionalSettings,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   additionalSettings=value;
		   }
			
		 }
	   }
	  private DateTime subscriptionExpirationDateTime ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime SubscriptionExpirationDateTime  
	   {
	    
	     get
		{
		   return subscriptionExpirationDateTime;
		 }
		 set
		 {
		   if(subscriptionExpirationDateTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SubscriptionExpirationDateTime",OldValue=subscriptionExpirationDateTime,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   subscriptionExpirationDateTime=value;
		   }
			
		 }
	   }
	  private DateTime accessTokenExpirationDateTime ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime AccessTokenExpirationDateTime  
	   {
	    
	     get
		{
		   return accessTokenExpirationDateTime;
		 }
		 set
		 {
		   if(accessTokenExpirationDateTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccessTokenExpirationDateTime",OldValue=accessTokenExpirationDateTime,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   accessTokenExpirationDateTime=value;
		   }
			
		 }
	   }
	  private string webhookParams ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public string WebhookParams  
	   {
	    
	     get
		{
		   return webhookParams;
		 }
		 set
		 {
		   if(webhookParams != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="WebhookParams",OldValue=webhookParams,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   webhookParams=value;
		   }
			
		 }
	   }
	    }
   
}
	 