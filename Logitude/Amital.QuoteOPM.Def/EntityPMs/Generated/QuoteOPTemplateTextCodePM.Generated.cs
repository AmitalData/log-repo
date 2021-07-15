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
using Amital.QuoteOPM.Def.Validators;
  
namespace Amital.QuoteOPM.Def.EntityPMs
{
   [CustomValidation(typeof(QuoteOPMClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class QuoteOPTemplateTextCodePM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  private string textCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string TextCode  
	   {
	    
	     get
		{
		   return textCode;
		 }
		 set
		 {
		   if(textCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TextCode",OldValue=textCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   textCode=value;
		   }
			
		 }
	   }
	  private string englishName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  private string quoteTemplateId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string QuoteTemplateId  
	   {
	    
	     get
		{
		   return quoteTemplateId;
		 }
		 set
		 {
		   if(quoteTemplateId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuoteTemplateId",OldValue=quoteTemplateId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quoteTemplateId=value;
		   }
			
		 }
	   }
	  private string area ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Area  
	   {
	    
	     get
		{
		   return area;
		 }
		 set
		 {
		   if(area != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Area",OldValue=area,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   area=value;
		   }
			
		 }
	   }
	  private string originalEnglishName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string OriginalEnglishName  
	   {
	    
	     get
		{
		   return originalEnglishName;
		 }
		 set
		 {
		   if(originalEnglishName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OriginalEnglishName",OldValue=originalEnglishName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   originalEnglishName=value;
		   }
			
		 }
	   }
	  private string originalLocalName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string OriginalLocalName  
	   {
	    
	     get
		{
		   return originalLocalName;
		 }
		 set
		 {
		   if(originalLocalName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OriginalLocalName",OldValue=originalLocalName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   originalLocalName=value;
		   }
			
		 }
	   }
   }
   
}
	 