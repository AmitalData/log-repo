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
   public partial class TradeAgreementPM : EntityPM
   {
   	  private string code ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private string localName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string LocalName  
	   {
	    
	     get
		{
		   return localName;
		 }
		 set
		 {
		   if(localName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LocalName",OldValue=localName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   localName=value;
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
	  private bool inactive ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool Inactive  
	   {
	    
	     get
		{
		   return inactive;
		 }
		 set
		 {
		   if(inactive != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Inactive",OldValue=inactive,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   inactive=value;
		   }
			
		 }
	   }
	  private int? customsBookTypeID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int? CustomsBookTypeID  
	   {
	    
	     get
		{
		   return customsBookTypeID;
		 }
		 set
		 {
		   if(customsBookTypeID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsBookTypeID",OldValue=customsBookTypeID,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   customsBookTypeID=value;
		   }
			
		 }
	   }
	  private int? countryGroupID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int? CountryGroupID  
	   {
	    
	     get
		{
		   return countryGroupID;
		 }
		 set
		 {
		   if(countryGroupID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CountryGroupID",OldValue=countryGroupID,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   countryGroupID=value;
		   }
			
		 }
	   }
   }
   
}
	 