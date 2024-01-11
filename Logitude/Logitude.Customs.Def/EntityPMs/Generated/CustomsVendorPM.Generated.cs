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
   public partial class CustomsVendorPM : EntityPM
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
	  private string vendorNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string VendorNumber  
	   {
	    
	     get
		{
		   return vendorNumber;
		 }
		 set
		 {
		   if(vendorNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VendorNumber",OldValue=vendorNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vendorNumber=value;
		   }
			
		 }
	   }
	  private string vendorTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string VendorTypeCode  
	   {
	    
	     get
		{
		   return vendorTypeCode;
		 }
		 set
		 {
		   if(vendorTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VendorTypeCode",OldValue=vendorTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vendorTypeCode=value;
		   }
			
		 }
	   }
	  private string vendorName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string VendorName  
	   {
	    
	     get
		{
		   return vendorName;
		 }
		 set
		 {
		   if(vendorName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VendorName",OldValue=vendorName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vendorName=value;
		   }
			
		 }
	   }
	  private string countryCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private string subCountryCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string SubCountryCode  
	   {
	    
	     get
		{
		   return subCountryCode;
		 }
		 set
		 {
		   if(subCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SubCountryCode",OldValue=subCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   subCountryCode=value;
		   }
			
		 }
	   }
	  private string subCountryName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string SubCountryName  
	   {
	    
	     get
		{
		   return subCountryName;
		 }
		 set
		 {
		   if(subCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SubCountryName",OldValue=subCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   subCountryName=value;
		   }
			
		 }
	   }
	  private string cityName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CityName  
	   {
	    
	     get
		{
		   return cityName;
		 }
		 set
		 {
		   if(cityName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CityName",OldValue=cityName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cityName=value;
		   }
			
		 }
	   }
	  private string mainAddressLine ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainAddressLine  
	   {
	    
	     get
		{
		   return mainAddressLine;
		 }
		 set
		 {
		   if(mainAddressLine != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainAddressLine",OldValue=mainAddressLine,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainAddressLine=value;
		   }
			
		 }
	   }
	  private string postalCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PostalCode  
	   {
	    
	     get
		{
		   return postalCode;
		 }
		 set
		 {
		   if(postalCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PostalCode",OldValue=postalCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   postalCode=value;
		   }
			
		 }
	   }
	  private string dunsNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DunsNumber  
	   {
	    
	     get
		{
		   return dunsNumber;
		 }
		 set
		 {
		   if(dunsNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DunsNumber",OldValue=dunsNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   dunsNumber=value;
		   }
			
		 }
	   }
	  private string vATNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string VATNumber  
	   {
	    
	     get
		{
		   return vATNumber;
		 }
		 set
		 {
		   if(vATNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VATNumber",OldValue=vATNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vATNumber=value;
		   }
			
		 }
	   }
	  private string statusCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string StatusCode  
	   {
	    
	     get
		{
		   return statusCode;
		 }
		 set
		 {
		   if(statusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StatusCode",OldValue=statusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   statusCode=value;
		   }
			
		 }
	   }
	  private string transactionTypeID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TransactionTypeID  
	   {
	    
	     get
		{
		   return transactionTypeID;
		 }
		 set
		 {
		   if(transactionTypeID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TransactionTypeID",OldValue=transactionTypeID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transactionTypeID=value;
		   }
			
		 }
	   }
	  private bool isPalestinian ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsPalestinian  
	   {
	    
	     get
		{
		   return isPalestinian;
		 }
		 set
		 {
		   if(isPalestinian != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsPalestinian",OldValue=isPalestinian,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isPalestinian=value;
		   }
			
		 }
	   }
	  private string vendorTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string VendorTypeName  
	   {
	    
	     get
		{
		   return vendorTypeName;
		 }
		 set
		 {
		   if(vendorTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VendorTypeName",OldValue=vendorTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vendorTypeName=value;
		   }
			
		 }
	   }
	  private string countryName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CountryName  
	   {
	    
	     get
		{
		   return countryName;
		 }
		 set
		 {
		   if(countryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CountryName",OldValue=countryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   countryName=value;
		   }
			
		 }
	   }

	   private List<VendorCommunicationPM> vendorCommunications;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("VendorVendorCommunications", "Id","VendorId")]
	   [DataMember]
	   public virtual List<VendorCommunicationPM> VendorCommunications  
	   {
	        get
             {
                 if (vendorCommunications == null)
                 {
                     vendorCommunications = new List<VendorCommunicationPM>();
                 }
                 return vendorCommunications;
              }
             set { vendorCommunications = value; }
	    }
		   
	   private List<VendorCommunicationPM>  deletedVendorCommunications;
	   public virtual List<VendorCommunicationPM> DeletedVendorCommunications  
	   {
	        get
             {
                 if ( deletedVendorCommunications == null)
                 {
                      deletedVendorCommunications = new List<VendorCommunicationPM>();
                 }
                 return  deletedVendorCommunications;
              }
             set {  deletedVendorCommunications = value; }
	    }
	  	  private bool inActive ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool InActive  
	   {
	    
	     get
		{
		   return inActive;
		 }
		 set
		 {
		   if(inActive != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InActive",OldValue=inActive,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   inActive=value;
		   }
			
		 }
	   }
	  private string externalId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExternalId  
	   {
	    
	     get
		{
		   return externalId;
		 }
		 set
		 {
		   if(externalId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExternalId",OldValue=externalId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   externalId=value;
		   }
			
		 }
	   }
	  private string statusName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string StatusName  
	   {
	    
	     get
		{
		   return statusName;
		 }
		 set
		 {
		   if(statusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StatusName",OldValue=statusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   statusName=value;
		   }
			
		 }
	   }
	  private string concurrencyGUID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConcurrencyGUID  
	   {
	    
	     get
		{
		   return concurrencyGUID;
		 }
		 set
		 {
		   if(concurrencyGUID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConcurrencyGUID",OldValue=concurrencyGUID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   concurrencyGUID=value;
		   }
			
		 }
	   }
	  private string newConcurrencyGUID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string NewConcurrencyGUID  
	   {
	    
	     get
		{
		   return newConcurrencyGUID;
		 }
		 set
		 {
		   if(newConcurrencyGUID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NewConcurrencyGUID",OldValue=newConcurrencyGUID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   newConcurrencyGUID=value;
		   }
			
		 }
	   }
	    }
   
}
	 