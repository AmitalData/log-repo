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
using Logitude.CargoTracking.Def.Validators;
  
namespace Logitude.CargoTracking.Def.EntityPMs
{
   [CustomValidation(typeof(CargoTrackingClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class CargoTrackingPortPM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
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
	  private string code ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Code  
	   {
	    
	     get
		{
		   return code;
		 }
		 set
		 {
		   if(code != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Code",OldValue=code,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   code=value;
		   }
			
		 }
	   }
	  private string englishName ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public string EnglishName  
	   {
	    
	     get
		{
		   return englishName;
		 }
		 set
		 {
		   if(englishName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EnglishName",OldValue=englishName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   englishName=value;
		   }
			
		 }
	   }
	  private string countryId ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CountryId  
	   {
	    
	     get
		{
		   return countryId;
		 }
		 set
		 {
		   if(countryId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CountryId",OldValue=countryId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   countryId=value;
		   }
			
		 }
	   }
	  private int tenant ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
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
	  private string countryCode ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CountryCode  
	   {
	    
	     get
		{
		   return countryCode;
		 }
		 set
		 {
		   if(countryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CountryCode",OldValue=countryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   countryCode=value;
		   }
			
		 }
	   }
   }
   
}
	 