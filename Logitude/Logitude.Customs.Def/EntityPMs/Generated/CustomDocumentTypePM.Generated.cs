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
   public partial class CustomDocumentTypePM : EntityPM
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
	  private string pointerLevel ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PointerLevel  
	   {
	    
	     get
		{
		   return pointerLevel;
		 }
		 set
		 {
		   if(pointerLevel != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PointerLevel",OldValue=pointerLevel,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pointerLevel=value;
		   }
			
		 }
	   }
	  private bool autoSetOriginalDocumentTrue ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool AutoSetOriginalDocumentTrue  
	   {
	    
	     get
		{
		   return autoSetOriginalDocumentTrue;
		 }
		 set
		 {
		   if(autoSetOriginalDocumentTrue != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AutoSetOriginalDocumentTrue",OldValue=autoSetOriginalDocumentTrue,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   autoSetOriginalDocumentTrue=value;
		   }
			
		 }
	   }
	  private string pointerLevelName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PointerLevelName  
	   {
	    
	     get
		{
		   return pointerLevelName;
		 }
		 set
		 {
		   if(pointerLevelName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PointerLevelName",OldValue=pointerLevelName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pointerLevelName=value;
		   }
			
		 }
	   }
	  private bool isCourierManadatory ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCourierManadatory  
	   {
	    
	     get
		{
		   return isCourierManadatory;
		 }
		 set
		 {
		   if(isCourierManadatory != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCourierManadatory",OldValue=isCourierManadatory,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCourierManadatory=value;
		   }
			
		 }
	   }
	  private bool isDiamondManadatory ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsDiamondManadatory  
	   {
	    
	     get
		{
		   return isDiamondManadatory;
		 }
		 set
		 {
		   if(isDiamondManadatory != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsDiamondManadatory",OldValue=isDiamondManadatory,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isDiamondManadatory=value;
		   }
			
		 }
	   }
	  private string customsDocumentUpload ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsDocumentUpload  
	   {
	    
	     get
		{
		   return customsDocumentUpload;
		 }
		 set
		 {
		   if(customsDocumentUpload != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsDocumentUpload",OldValue=customsDocumentUpload,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsDocumentUpload=value;
		   }
			
		 }
	   }
	  private string customsDocumentUploadName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsDocumentUploadName  
	   {
	    
	     get
		{
		   return customsDocumentUploadName;
		 }
		 set
		 {
		   if(customsDocumentUploadName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsDocumentUploadName",OldValue=customsDocumentUploadName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsDocumentUploadName=value;
		   }
			
		 }
	   }
	    }
   
}
	 