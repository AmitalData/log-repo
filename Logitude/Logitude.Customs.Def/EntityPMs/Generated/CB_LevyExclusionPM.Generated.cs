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
   public partial class CB_LevyExclusionPM : EntityPM
   {
   	  private string iD ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ID  
	   {
	    
	     get
		{
		   return iD;
		 }
		 set
		 {
		   if(iD != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ID",OldValue=iD,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   iD=value;
		   }
			
		 }
	   }
	  private int? levyExclusionNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int? LevyExclusionNumber  
	   {
	    
	     get
		{
		   return levyExclusionNumber;
		 }
		 set
		 {
		   if(levyExclusionNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LevyExclusionNumber",OldValue=levyExclusionNumber,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   levyExclusionNumber=value;
		   }
			
		 }
	   }
	  private string tradeLevyID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TradeLevyID  
	   {
	    
	     get
		{
		   return tradeLevyID;
		 }
		 set
		 {
		   if(tradeLevyID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TradeLevyID",OldValue=tradeLevyID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tradeLevyID=value;
		   }
			
		 }
	   }
	  private string vendorID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string VendorID  
	   {
	    
	     get
		{
		   return vendorID;
		 }
		 set
		 {
		   if(vendorID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VendorID",OldValue=vendorID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vendorID=value;
		   }
			
		 }
	   }
	  private string countryGroupID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CountryGroupID  
	   {
	    
	     get
		{
		   return countryGroupID;
		 }
		 set
		 {
		   if(countryGroupID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CountryGroupID",OldValue=countryGroupID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   countryGroupID=value;
		   }
			
		 }
	   }
   }
   
}
	 