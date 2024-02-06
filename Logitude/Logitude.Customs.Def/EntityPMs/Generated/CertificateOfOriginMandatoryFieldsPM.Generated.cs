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
   public partial class CertificateOfOriginMandatoryFieldsPM : EntityPM
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
	  private bool isMandatory ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsMandatory  
	   {
	    
	     get
		{
		   return isMandatory;
		 }
		 set
		 {
		   if(isMandatory != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsMandatory",OldValue=isMandatory,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isMandatory=value;
		   }
			
		 }
	   }
	  private int? location ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int? Location  
	   {
	    
	     get
		{
		   return location;
		 }
		 set
		 {
		   if(location != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Location",OldValue=location,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   location=value;
		   }
			
		 }
	   }
	  private DateTime? lastUpdatedDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? LastUpdatedDate  
	   {
	    
	     get
		{
		   return lastUpdatedDate;
		 }
		 set
		 {
		   if(lastUpdatedDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastUpdatedDate",OldValue=lastUpdatedDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   lastUpdatedDate=value;
		   }
			
		 }
	   }
	  private string mappedCertificateFieldsName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MappedCertificateFieldsName  
	   {
	    
	     get
		{
		   return mappedCertificateFieldsName;
		 }
		 set
		 {
		   if(mappedCertificateFieldsName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MappedCertificateFieldsName",OldValue=mappedCertificateFieldsName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mappedCertificateFieldsName=value;
		   }
			
		 }
	   }
	  private string mappedCertificateFields ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MappedCertificateFields  
	   {
	    
	     get
		{
		   return mappedCertificateFields;
		 }
		 set
		 {
		   if(mappedCertificateFields != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MappedCertificateFields",OldValue=mappedCertificateFields,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mappedCertificateFields=value;
		   }
			
		 }
	   }
   }
   
}
	 