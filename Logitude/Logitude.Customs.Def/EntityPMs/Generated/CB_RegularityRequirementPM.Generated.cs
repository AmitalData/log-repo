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
   public partial class CB_RegularityRequirementPM : EntityPM
   {
   	  private int iD ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int ID  
	   {
	    
	     get
		{
		   return iD;
		 }
		 set
		 {
		   if(iD != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ID",OldValue=iD,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   iD=value;
		   }
			
		 }
	   }
	  private DateTime createDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private DateTime? updateDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? UpdateDate  
	   {
	    
	     get
		{
		   return updateDate;
		 }
		 set
		 {
		   if(updateDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdateDate",OldValue=updateDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   updateDate=value;
		   }
			
		 }
	   }
	  private string countryID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CountryID  
	   {
	    
	     get
		{
		   return countryID;
		 }
		 set
		 {
		   if(countryID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CountryID",OldValue=countryID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   countryID=value;
		   }
			
		 }
	   }
	  private bool isAllCountries ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsAllCountries  
	   {
	    
	     get
		{
		   return isAllCountries;
		 }
		 set
		 {
		   if(isAllCountries != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsAllCountries",OldValue=isAllCountries,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isAllCountries=value;
		   }
			
		 }
	   }
	  private int? customsItemID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int? CustomsItemID  
	   {
	    
	     get
		{
		   return customsItemID;
		 }
		 set
		 {
		   if(customsItemID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsItemID",OldValue=customsItemID,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   customsItemID=value;
		   }
			
		 }
	   }
	  private bool isAllCustomsItems ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsAllCustomsItems  
	   {
	    
	     get
		{
		   return isAllCustomsItems;
		 }
		 set
		 {
		   if(isAllCustomsItems != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsAllCustomsItems",OldValue=isAllCustomsItems,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isAllCustomsItems=value;
		   }
			
		 }
	   }
	  private bool isLimitedCountryRegularRequire ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsLimitedCountryRegularRequire  
	   {
	    
	     get
		{
		   return isLimitedCountryRegularRequire;
		 }
		 set
		 {
		   if(isLimitedCountryRegularRequire != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsLimitedCountryRegularRequire",OldValue=isLimitedCountryRegularRequire,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isLimitedCountryRegularRequire=value;
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
	  private string inceptionCodeID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string InceptionCodeID  
	   {
	    
	     get
		{
		   return inceptionCodeID;
		 }
		 set
		 {
		   if(inceptionCodeID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InceptionCodeID",OldValue=inceptionCodeID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   inceptionCodeID=value;
		   }
			
		 }
	   }
	  private string regularityPublicationCodeID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string RegularityPublicationCodeID  
	   {
	    
	     get
		{
		   return regularityPublicationCodeID;
		 }
		 set
		 {
		   if(regularityPublicationCodeID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RegularityPublicationCodeID",OldValue=regularityPublicationCodeID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   regularityPublicationCodeID=value;
		   }
			
		 }
	   }
	  private string regularitySourceCodeID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string RegularitySourceCodeID  
	   {
	    
	     get
		{
		   return regularitySourceCodeID;
		 }
		 set
		 {
		   if(regularitySourceCodeID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RegularitySourceCodeID",OldValue=regularitySourceCodeID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   regularitySourceCodeID=value;
		   }
			
		 }
	   }
	  private string customsBookTypeID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsBookTypeID  
	   {
	    
	     get
		{
		   return customsBookTypeID;
		 }
		 set
		 {
		   if(customsBookTypeID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsBookTypeID",OldValue=customsBookTypeID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsBookTypeID=value;
		   }
			
		 }
	   }
   }
   
}
	 