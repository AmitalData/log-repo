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
   public partial class QuoteOPTemplatePM : EntityPM
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
	  private string headerDocId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string HeaderDocId  
	   {
	    
	     get
		{
		   return headerDocId;
		 }
		 set
		 {
		   if(headerDocId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HeaderDocId",OldValue=headerDocId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   headerDocId=value;
		   }
			
		 }
	   }
	  private string footerDocId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string FooterDocId  
	   {
	    
	     get
		{
		   return footerDocId;
		 }
		 set
		 {
		   if(footerDocId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FooterDocId",OldValue=footerDocId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   footerDocId=value;
		   }
			
		 }
	   }
	  private string quoteOPTemplateSettingId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string QuoteOPTemplateSettingId  
	   {
	    
	     get
		{
		   return quoteOPTemplateSettingId;
		 }
		 set
		 {
		   if(quoteOPTemplateSettingId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuoteOPTemplateSettingId",OldValue=quoteOPTemplateSettingId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quoteOPTemplateSettingId=value;
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
	  private bool isTemplate ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsTemplate  
	   {
	    
	     get
		{
		   return isTemplate;
		 }
		 set
		 {
		   if(isTemplate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsTemplate",OldValue=isTemplate,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isTemplate=value;
		   }
			
		 }
	   }
	  private string originalQuoteOPTemplateId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string OriginalQuoteOPTemplateId  
	   {
	    
	     get
		{
		   return originalQuoteOPTemplateId;
		 }
		 set
		 {
		   if(originalQuoteOPTemplateId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OriginalQuoteOPTemplateId",OldValue=originalQuoteOPTemplateId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   originalQuoteOPTemplateId=value;
		   }
			
		 }
	   }
	  private DateTime createDate ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  private string createdByUserId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreatedByUserId  
	   {
	    
	     get
		{
		   return createdByUserId;
		 }
		 set
		 {
		   if(createdByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreatedByUserId",OldValue=createdByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   createdByUserId=value;
		   }
			
		 }
	   }
	  private string updatedByUserId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string UpdatedByUserId  
	   {
	    
	     get
		{
		   return updatedByUserId;
		 }
		 set
		 {
		   if(updatedByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdatedByUserId",OldValue=updatedByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   updatedByUserId=value;
		   }
			
		 }
	   }
	  private string searchFields ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  private string templateTypeCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string TemplateTypeCode  
	   {
	    
	     get
		{
		   return templateTypeCode;
		 }
		 set
		 {
		   if(templateTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TemplateTypeCode",OldValue=templateTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   templateTypeCode=value;
		   }
			
		 }
	   }
	  private bool isDefault ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsDefault  
	   {
	    
	     get
		{
		   return isDefault;
		 }
		 set
		 {
		   if(isDefault != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsDefault",OldValue=isDefault,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isDefault=value;
		   }
			
		 }
	   }
	  private bool inActive ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  private bool isLastQuoteOPTemplateDocumentVersion ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsLastQuoteOPTemplateDocumentVersion  
	   {
	    
	     get
		{
		   return isLastQuoteOPTemplateDocumentVersion;
		 }
		 set
		 {
		   if(isLastQuoteOPTemplateDocumentVersion != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsLastQuoteOPTemplateDocumentVersion",OldValue=isLastQuoteOPTemplateDocumentVersion,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isLastQuoteOPTemplateDocumentVersion=value;
		   }
			
		 }
	   }

	   private List<QuoteOPTemplateSectionPM> templateSections;
	 
		     
	   [Include]
	   [Association("QuoteOPTemplateQuoteOPTemplateSection", "Id","QuoteOPTemplateId")]
	   [DataMember]
	   public virtual List<QuoteOPTemplateSectionPM> TemplateSections  
	   {
	        get
             {
                 if (templateSections == null)
                 {
                     templateSections = new List<QuoteOPTemplateSectionPM>();
                 }
                 return templateSections;
              }
             set { templateSections = value; }
	    }
		   
	   private List<QuoteOPTemplateSectionPM>  deletedTemplateSections;
	   public virtual List<QuoteOPTemplateSectionPM> DeletedTemplateSections  
	   {
	        get
             {
                 if ( deletedTemplateSections == null)
                 {
                      deletedTemplateSections = new List<QuoteOPTemplateSectionPM>();
                 }
                 return  deletedTemplateSections;
              }
             set {  deletedTemplateSections = value; }
	    }
	  	  private bool isCopiedAtSignup ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCopiedAtSignup  
	   {
	    
	     get
		{
		   return isCopiedAtSignup;
		 }
		 set
		 {
		   if(isCopiedAtSignup != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCopiedAtSignup",OldValue=isCopiedAtSignup,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCopiedAtSignup=value;
		   }
			
		 }
	   }
	  private bool isEnabledForCustomers ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsEnabledForCustomers  
	   {
	    
	     get
		{
		   return isEnabledForCustomers;
		 }
		 set
		 {
		   if(isEnabledForCustomers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsEnabledForCustomers",OldValue=isEnabledForCustomers,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isEnabledForCustomers=value;
		   }
			
		 }
	   }
	  private string tenantName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string TenantName  
	   {
	    
	     get
		{
		   return tenantName;
		 }
		 set
		 {
		   if(tenantName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TenantName",OldValue=tenantName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tenantName=value;
		   }
			
		 }
	   }
   }
   
}
	 