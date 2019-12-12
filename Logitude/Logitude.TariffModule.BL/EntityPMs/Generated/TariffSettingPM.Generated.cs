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
using Logitude.TariffModule.BL.Validators;
  
namespace Logitude.TariffModule.BL.EntityPMs
{
   [CustomValidation(typeof(TariffModuleClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class TariffSettingPM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
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
	  private string defaultPriceSteps ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string DefaultPriceSteps  
	   {
	    
	     get
		{
		   return defaultPriceSteps;
		 }
		 set
		 {
		   if(defaultPriceSteps != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DefaultPriceSteps",OldValue=defaultPriceSteps,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   defaultPriceSteps=value;
		   }
			
		 }
	   }
	  private int tenant ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
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
	  private double? defaultWarningPercentage ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public double? DefaultWarningPercentage  
	   {
	    
	     get
		{
		   return defaultWarningPercentage;
		 }
		 set
		 {
		   if(defaultWarningPercentage != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DefaultWarningPercentage",OldValue=defaultWarningPercentage,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   defaultWarningPercentage=value;
		   }
			
		 }
	   }
	  private string airDefaultStepsId ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string AirDefaultStepsId  
	   {
	    
	     get
		{
		   return airDefaultStepsId;
		 }
		 set
		 {
		   if(airDefaultStepsId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AirDefaultStepsId",OldValue=airDefaultStepsId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   airDefaultStepsId=value;
		   }
			
		 }
	   }
	  private string lCLDefaultStepsId ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string LCLDefaultStepsId  
	   {
	    
	     get
		{
		   return lCLDefaultStepsId;
		 }
		 set
		 {
		   if(lCLDefaultStepsId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LCLDefaultStepsId",OldValue=lCLDefaultStepsId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lCLDefaultStepsId=value;
		   }
			
		 }
	   }
   }
   
}
	 