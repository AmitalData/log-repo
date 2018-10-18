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
   public partial class OpportunityProductLocationPM : EntityPM
   {
   	  private string opportunityId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string OpportunityId  
	   {
	    
	     get
		{
		   return opportunityId;
		 }
		 set
		 {
		   if(opportunityId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OpportunityId",OldValue=opportunityId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   opportunityId=value;
		   }
			
		 }
	   }
	  private string opportunityProductTypeCode ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string OpportunityProductTypeCode  
	   {
	    
	     get
		{
		   return opportunityProductTypeCode;
		 }
		 set
		 {
		   if(opportunityProductTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OpportunityProductTypeCode",OldValue=opportunityProductTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   opportunityProductTypeCode=value;
		   }
			
		 }
	   }
	  private int lineNumber ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  private string countryId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  private string locationCode ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string LocationCode  
	   {
	    
	     get
		{
		   return locationCode;
		 }
		 set
		 {
		   if(locationCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LocationCode",OldValue=locationCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   locationCode=value;
		   }
			
		 }
	   }
	  private string locationName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string LocationName  
	   {
	    
	     get
		{
		   return locationName;
		 }
		 set
		 {
		   if(locationName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LocationName",OldValue=locationName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   locationName=value;
		   }
			
		 }
	   }
	  private decimal? tEU ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? TEU  
	   {
	    
	     get
		{
		   return tEU;
		 }
		 set
		 {
		   if(tEU != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TEU",OldValue=tEU,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   tEU=value;
		   }
			
		 }
	   }
	  private int? numberOfShipments ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public int? NumberOfShipments  
	   {
	    
	     get
		{
		   return numberOfShipments;
		 }
		 set
		 {
		   if(numberOfShipments != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NumberOfShipments",OldValue=numberOfShipments,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   numberOfShipments=value;
		   }
			
		 }
	   }
	  private decimal? chargeableWeight ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? ChargeableWeight  
	   {
	    
	     get
		{
		   return chargeableWeight;
		 }
		 set
		 {
		   if(chargeableWeight != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChargeableWeight",OldValue=chargeableWeight,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   chargeableWeight=value;
		   }
			
		 }
	   }
	  private decimal? revenue ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Revenue  
	   {
	    
	     get
		{
		   return revenue;
		 }
		 set
		 {
		   if(revenue != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Revenue",OldValue=revenue,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   revenue=value;
		   }
			
		 }
	   }
   }
   
}
	 