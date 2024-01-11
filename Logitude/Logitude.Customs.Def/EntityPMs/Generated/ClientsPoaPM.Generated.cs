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
using Logitude.Customs.Def.Validators;
  
namespace Logitude.Customs.Def.EntityPMs
{
   [CustomValidation(typeof(CustomsClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class ClientsPoaPM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private string searchFields ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private string clientId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ClientId  
	   {
	    
	     get
		{
		   return clientId;
		 }
		 set
		 {
		   if(clientId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClientId",OldValue=clientId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   clientId=value;
		   }
			
		 }
	   }
	  private string poaID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PoaID  
	   {
	    
	     get
		{
		   return poaID;
		 }
		 set
		 {
		   if(poaID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PoaID",OldValue=poaID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   poaID=value;
		   }
			
		 }
	   }
	  private string authorizedExternalId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AuthorizedExternalId  
	   {
	    
	     get
		{
		   return authorizedExternalId;
		 }
		 set
		 {
		   if(authorizedExternalId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AuthorizedExternalId",OldValue=authorizedExternalId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   authorizedExternalId=value;
		   }
			
		 }
	   }
	  private string authorizerExternalId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AuthorizerExternalId  
	   {
	    
	     get
		{
		   return authorizerExternalId;
		 }
		 set
		 {
		   if(authorizerExternalId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AuthorizerExternalId",OldValue=authorizerExternalId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   authorizerExternalId=value;
		   }
			
		 }
	   }
	  private string authorizerPassportNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AuthorizerPassportNumber  
	   {
	    
	     get
		{
		   return authorizerPassportNumber;
		 }
		 set
		 {
		   if(authorizerPassportNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AuthorizerPassportNumber",OldValue=authorizerPassportNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   authorizerPassportNumber=value;
		   }
			
		 }
	   }
	  private string authorizerPassportCountry ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AuthorizerPassportCountry  
	   {
	    
	     get
		{
		   return authorizerPassportCountry;
		 }
		 set
		 {
		   if(authorizerPassportCountry != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AuthorizerPassportCountry",OldValue=authorizerPassportCountry,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   authorizerPassportCountry=value;
		   }
			
		 }
	   }
	  private string authorizerPassportType ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AuthorizerPassportType  
	   {
	    
	     get
		{
		   return authorizerPassportType;
		 }
		 set
		 {
		   if(authorizerPassportType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AuthorizerPassportType",OldValue=authorizerPassportType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   authorizerPassportType=value;
		   }
			
		 }
	   }
	  private DateTime startDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime StartDate  
	   {
	    
	     get
		{
		   return startDate;
		 }
		 set
		 {
		   if(startDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StartDate",OldValue=startDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   startDate=value;
		   }
			
		 }
	   }
	  private DateTime endDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime EndDate  
	   {
	    
	     get
		{
		   return endDate;
		 }
		 set
		 {
		   if(endDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EndDate",OldValue=endDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   endDate=value;
		   }
			
		 }
	   }
	  private string poaStatus ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PoaStatus  
	   {
	    
	     get
		{
		   return poaStatus;
		 }
		 set
		 {
		   if(poaStatus != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PoaStatus",OldValue=poaStatus,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   poaStatus=value;
		   }
			
		 }
	   }
	  private string poaAuthorizationType ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PoaAuthorizationType  
	   {
	    
	     get
		{
		   return poaAuthorizationType;
		 }
		 set
		 {
		   if(poaAuthorizationType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PoaAuthorizationType",OldValue=poaAuthorizationType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   poaAuthorizationType=value;
		   }
			
		 }
	   }
	  private string poaStatusName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PoaStatusName  
	   {
	    
	     get
		{
		   return poaStatusName;
		 }
		 set
		 {
		   if(poaStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PoaStatusName",OldValue=poaStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   poaStatusName=value;
		   }
			
		 }
	   }
	  private string poaAuthorizationTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PoaAuthorizationTypeName  
	   {
	    
	     get
		{
		   return poaAuthorizationTypeName;
		 }
		 set
		 {
		   if(poaAuthorizationTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PoaAuthorizationTypeName",OldValue=poaAuthorizationTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   poaAuthorizationTypeName=value;
		   }
			
		 }
	   }
	    }
   
}
	 