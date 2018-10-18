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
   public partial class SLAEscalationRecepientPM : EntityPM
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
	  private string sLAEscalationId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string SLAEscalationId  
	   {
	    
	     get
		{
		   return sLAEscalationId;
		 }
		 set
		 {
		   if(sLAEscalationId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SLAEscalationId",OldValue=sLAEscalationId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   sLAEscalationId=value;
		   }
			
		 }
	   }
	  private string preDefinitionId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PreDefinitionId  
	   {
	    
	     get
		{
		   return preDefinitionId;
		 }
		 set
		 {
		   if(preDefinitionId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PreDefinitionId",OldValue=preDefinitionId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   preDefinitionId=value;
		   }
			
		 }
	   }
	  private string userId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  private string preDefinitionName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PreDefinitionName  
	   {
	    
	     get
		{
		   return preDefinitionName;
		 }
		 set
		 {
		   if(preDefinitionName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PreDefinitionName",OldValue=preDefinitionName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   preDefinitionName=value;
		   }
			
		 }
	   }
	  private string userName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string UserName  
	   {
	    
	     get
		{
		   return userName;
		 }
		 set
		 {
		   if(userName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UserName",OldValue=userName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   userName=value;
		   }
			
		 }
	   }
	  private string userEmail ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
   }
   
}
	 