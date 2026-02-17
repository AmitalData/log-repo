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
   public partial class VehicleSafetyAccessoryPM : EntityPM
   {
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
	  private string vehicleId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string VehicleId  
	   {
	    
	     get
		{
		   return vehicleId;
		 }
		 set
		 {
		   if(vehicleId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VehicleId",OldValue=vehicleId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vehicleId=value;
		   }
			
		 }
	   }
	  private int lineNumber ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int LineNumber  
	   {
	    
	     get
		{
		   return lineNumber;
		 }
		 set
		 {
		   if(lineNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LineNumber",OldValue=lineNumber,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   lineNumber=value;
		   }
			
		 }
	   }
	  private string vehicleSafetyAccessoryCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string VehicleSafetyAccessoryCode  
	   {
	    
	     get
		{
		   return vehicleSafetyAccessoryCode;
		 }
		 set
		 {
		   if(vehicleSafetyAccessoryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VehicleSafetyAccessoryCode",OldValue=vehicleSafetyAccessoryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vehicleSafetyAccessoryCode=value;
		   }
			
		 }
	   }
	  private string vehicleSafAccessoryInstlTypCod ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string VehicleSafAccessoryInstlTypCod  
	   {
	    
	     get
		{
		   return vehicleSafAccessoryInstlTypCod;
		 }
		 set
		 {
		   if(vehicleSafAccessoryInstlTypCod != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VehicleSafAccessoryInstlTypCod",OldValue=vehicleSafAccessoryInstlTypCod,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vehicleSafAccessoryInstlTypCod=value;
		   }
			
		 }
	   }
	  private string vehicleSafAccessoryInstlTypName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string VehicleSafAccessoryInstlTypName  
	   {
	    
	     get
		{
		   return vehicleSafAccessoryInstlTypName;
		 }
		 set
		 {
		   if(vehicleSafAccessoryInstlTypName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VehicleSafAccessoryInstlTypName",OldValue=vehicleSafAccessoryInstlTypName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vehicleSafAccessoryInstlTypName=value;
		   }
			
		 }
	   }
	  private string vehicleSafetyAccessoryName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string VehicleSafetyAccessoryName  
	   {
	    
	     get
		{
		   return vehicleSafetyAccessoryName;
		 }
		 set
		 {
		   if(vehicleSafetyAccessoryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VehicleSafetyAccessoryName",OldValue=vehicleSafetyAccessoryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vehicleSafetyAccessoryName=value;
		   }
			
		 }
	   }
   }
   
}
	 