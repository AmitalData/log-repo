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
   public partial class QuoteOPTemplateSectionPM : EntityPM
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
	  private string name ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Name  
	   {
	    
	     get
		{
		   return name;
		 }
		 set
		 {
		   if(name != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Name",OldValue=name,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   name=value;
		   }
			
		 }
	   }
	  private string description ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Description  
	   {
	    
	     get
		{
		   return description;
		 }
		 set
		 {
		   if(description != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Description",OldValue=description,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   description=value;
		   }
			
		 }
	   }
	  private bool isCancel ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCancel  
	   {
	    
	     get
		{
		   return isCancel;
		 }
		 set
		 {
		   if(isCancel != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCancel",OldValue=isCancel,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCancel=value;
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
	  private string sectionDocId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string SectionDocId  
	   {
	    
	     get
		{
		   return sectionDocId;
		 }
		 set
		 {
		   if(sectionDocId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SectionDocId",OldValue=sectionDocId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   sectionDocId=value;
		   }
			
		 }
	   }
	  private int order ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int Order  
	   {
	    
	     get
		{
		   return order;
		 }
		 set
		 {
		   if(order != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Order",OldValue=order,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   order=value;
		   }
			
		 }
	   }
	  private string quoteOPTemplateSectionTypeCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string QuoteOPTemplateSectionTypeCode  
	   {
	    
	     get
		{
		   return quoteOPTemplateSectionTypeCode;
		 }
		 set
		 {
		   if(quoteOPTemplateSectionTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuoteOPTemplateSectionTypeCode",OldValue=quoteOPTemplateSectionTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quoteOPTemplateSectionTypeCode=value;
		   }
			
		 }
	   }
	  private bool ischangeBodySection ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IschangeBodySection  
	   {
	    
	     get
		{
		   return ischangeBodySection;
		 }
		 set
		 {
		   if(ischangeBodySection != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IschangeBodySection",OldValue=ischangeBodySection,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   ischangeBodySection=value;
		   }
			
		 }
	   }
	  private bool isSettingTypeCodeS ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsSettingTypeCodeS  
	   {
	    
	     get
		{
		   return isSettingTypeCodeS;
		 }
		 set
		 {
		   if(isSettingTypeCodeS != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsSettingTypeCodeS",OldValue=isSettingTypeCodeS,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isSettingTypeCodeS=value;
		   }
			
		 }
	   }
	  private bool isSettingTypeCodeP ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsSettingTypeCodeP  
	   {
	    
	     get
		{
		   return isSettingTypeCodeP;
		 }
		 set
		 {
		   if(isSettingTypeCodeP != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsSettingTypeCodeP",OldValue=isSettingTypeCodeP,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isSettingTypeCodeP=value;
		   }
			
		 }
	   }
	  private string templatedata ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Templatedata  
	   {
	    
	     get
		{
		   return templatedata;
		 }
		 set
		 {
		   if(templatedata != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Templatedata",OldValue=templatedata,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   templatedata=value;
		   }
			
		 }
	   }
	  private string quoteId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string QuoteId  
	   {
	    
	     get
		{
		   return quoteId;
		 }
		 set
		 {
		   if(quoteId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuoteId",OldValue=quoteId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quoteId=value;
		   }
			
		 }
	   }
	  private bool isQuoteEdited ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsQuoteEdited  
	   {
	    
	     get
		{
		   return isQuoteEdited;
		 }
		 set
		 {
		   if(isQuoteEdited != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsQuoteEdited",OldValue=isQuoteEdited,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isQuoteEdited=value;
		   }
			
		 }
	   }
	  private bool isExcluded ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsExcluded  
	   {
	    
	     get
		{
		   return isExcluded;
		 }
		 set
		 {
		   if(isExcluded != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsExcluded",OldValue=isExcluded,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isExcluded=value;
		   }
			
		 }
	   }
   }
   
}
	 