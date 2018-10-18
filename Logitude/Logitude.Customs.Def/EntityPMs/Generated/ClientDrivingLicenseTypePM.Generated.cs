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
   public partial class ClientDrivingLicenseTypePM : EntityPM
   {
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
	  private int clientDrivingLicenseLine ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int ClientDrivingLicenseLine  
	   {
	    
	     get
		{
		   return clientDrivingLicenseLine;
		 }
		 set
		 {
		   if(clientDrivingLicenseLine != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClientDrivingLicenseLine",OldValue=clientDrivingLicenseLine,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   clientDrivingLicenseLine=value;
		   }
			
		 }
	   }
	  private string driversLicenseTypeCode ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DriversLicenseTypeCode  
	   {
	    
	     get
		{
		   return driversLicenseTypeCode;
		 }
		 set
		 {
		   if(driversLicenseTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DriversLicenseTypeCode",OldValue=driversLicenseTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   driversLicenseTypeCode=value;
		   }
			
		 }
	   }
   }
   
}
	 